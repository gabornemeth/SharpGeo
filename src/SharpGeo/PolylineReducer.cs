using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGeo
{
    /// <summary>
    /// http://www.codeproject.com/Articles/18936/A-Csharp-Implementation-of-Douglas-Peucker-Line-Ap
    /// </summary>
    public class PolylineReducer
    {
        /// <span class="code-SummaryComment"><summary></span>
        /// Uses the Douglas Peucker algorithm to reduce the number of points.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="points">The points.</param></span>
        /// <span class="code-SummaryComment"><param name="tolerance">The tolerance.</param></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        public static IList<IPoint> DouglasPeuckerReduction(IList<IPoint> points, double tolerance)
        {
            if (points == null || points.Count < 3)
                return points;

            var firstPoint = 0;
            var lastPoint = points.Count - 1;
            var pointIndexsToKeep = new List<int>();

            //Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            //The first and the last point cannot be the same
            while (points[firstPoint].Equals(points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(points, firstPoint, lastPoint, tolerance, pointIndexsToKeep);

            var returnPoints = new List<IPoint>();
            pointIndexsToKeep.Sort();
            foreach (var index in pointIndexsToKeep)
            {
                returnPoints.Add(points[index]);
            }

            return returnPoints;
        }

        /// <summary>
        /// Non recursive version of Douglas Peucker polyline reduction
        /// </summary>
        /// <param name="points"></param>
        /// <param name="firstIndex"></param>
        /// <param name="lastIndex"></param>
        /// <param name="tolerance"></param>
        /// <param name="pointIndexsToKeep"></param>
        private static void DouglasPeuckerReduction(IList<IPoint> points, int firstIndex, int lastIndex, double tolerance, IList<Int32> pointIndexsToKeep)
        {

            var indexes = new Stack<Tuple<int, int>>();

            Debug.WriteLine("firstPoint = {0}, lastPoint = {1}", firstIndex, lastIndex);

            indexes.Push(new Tuple<int, int>(firstIndex, lastIndex));

            while (indexes.Count > 0)
            {
                var range = indexes.Pop();
                firstIndex = range.Item1;
                lastIndex = range.Item2;
                var indexFarthest = -1;
                var maxDistance = 0.0;
                for (var index = firstIndex + 1; index < lastIndex; index++)
                {
                    var distance = PerpendicularDistance(points[firstIndex], points[lastIndex], points[index]);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        indexFarthest = index;
                    }
                }

                if (maxDistance > tolerance && indexFarthest != -1)
                {
                    // Add the largest point that exceeds the tolerance
                    pointIndexsToKeep.Add(indexFarthest);

                    indexes.Push(new Tuple<int, int>(firstIndex, indexFarthest));
                    indexes.Push(new Tuple<int, int>(indexFarthest, lastIndex));
                }
            }
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Douglases the peucker reduction.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="points">The points.</param></span>
        /// <span class="code-SummaryComment"><param name="firstPoint">The first point.</param></span>
        /// <span class="code-SummaryComment"><param name="lastPoint">The last point.</param></span>
        /// <span class="code-SummaryComment"><param name="tolerance">The tolerance.</param></span>
        /// <span class="code-SummaryComment"><param name="pointIndexsToKeep">The point index to keep.</param></span>
        //private static void DouglasPeuckerReduction(IList<IPoint> points, Int32 firstPoint, Int32 lastPoint, Double tolerance,
        //    List<Int32> pointIndexsToKeep)
        //{
        //    var maxDistance = 0.0;
        //    var indexFarthest = 0;

        //    Debug.WriteLine("firstPoint = {0}, lastPoint = {1}", firstPoint, lastPoint);

        //    for (var index = firstPoint; index < lastPoint; index++)
        //    {
        //        var distance = PerpendicularDistance(points[firstPoint], points[lastPoint], points[index]);
        //        if (distance > maxDistance)
        //        {
        //            maxDistance = distance;
        //            indexFarthest = index;
        //        }
        //    }

        //    if (maxDistance > tolerance && indexFarthest != 0)
        //    {
        //        // Add the largest point that exceeds the tolerance
        //        pointIndexsToKeep.Add(indexFarthest);

        //        DouglasPeuckerReduction(points, firstPoint, indexFarthest, tolerance, pointIndexsToKeep);
        //        DouglasPeuckerReduction(points, indexFarthest, lastPoint, tolerance, pointIndexsToKeep);
        //    }
        //}

        /// <span class="code-SummaryComment"><summary></span>
        /// The distance of a point from a line made from point1 and point2.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="pt1">The PT1.</param></span>
        /// <span class="code-SummaryComment"><param name="pt2">The PT2.</param></span>
        /// <span class="code-SummaryComment"><param name="p">The p.</param></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        public static double PerpendicularDistance(IPoint point1, IPoint point2, IPoint point)
        {
            //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
            //Base = v((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
            //Area = .5*Base*H                                          *Solve for height
            //Height = Area/.5/Base

            var area = Math.Abs(.5 * (point1.X * point2.Y + point2.X *
            point.Y + point.X * point1.Y - point2.X * point1.Y - point.X *
            point2.Y - point1.X * point.Y));
            var bottom = Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
            var height = area / bottom * 2;

            return height;

            //Another option
            //Double A = Point.X - Point1.X;
            //Double B = Point.Y - Point1.Y;
            //Double C = Point2.X - Point1.X;
            //Double D = Point2.Y - Point1.Y;

            //Double dot = A * C + B * D;
            //Double len_sq = C * C + D * D;
            //Double param = dot / len_sq;

            //Double xx, yy;

            //if (param < 0)
            //{
            //    xx = Point1.X;
            //    yy = Point1.Y;
            //}
            //else if (param > 1)
            //{
            //    xx = Point2.X;
            //    yy = Point2.Y;
            //}
            //else
            //{
            //    xx = Point1.X + param * C;
            //    yy = Point1.Y + param * D;
            //}

            //Double d = DistanceBetweenOn2DPlane(Point, new Point(xx, yy));
        }
    }
}
