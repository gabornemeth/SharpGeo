using System;
//using Newtonsoft.Json;

namespace GeoSharp
{
    /// <summary>
    /// 2D position
    /// </summary>
    public struct Point2 : IPoint
    {
        /// <summary>
        /// Longitude in degrees
        /// </summary>
        public float X { get; set; }
        /// <summary>
        /// Latitude in degrees
        /// </summary>
        public float Y { get; set; }

        //[JsonIgnore]
        public bool IsEmpty
        {
            get
            {
                return X.Equals(0) && Y.Equals(0);
            }
        }

        private static Position empty = new Position();

        public static Position Empty
        {
            get
            {
                return empty;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Point3"/> 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point2(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("X={0} Y={1}", X, Y);
        }

        public bool Equals(Position pos)
        {
            return X.Equals(pos.Longitude) && Y.Equals(pos.Latitude);
        }

        public bool Equals(Position pos, double err)
        {
            return Math.Abs(X - pos.Longitude) <= err &&
                Math.Abs(Y - pos.Latitude) <= err;
        }

        /// <summary>
        /// Distance from a position in degrees
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Position DistanceFrom(Position pos)
        {
            return new Position(System.Math.Abs(pos.Longitude - X), System.Math.Abs(pos.Latitude - Y));
        }
    }
}
