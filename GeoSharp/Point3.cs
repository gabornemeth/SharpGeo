using System;
//using Newtonsoft.Json;

namespace GeoSharp
{
    /// <summary>
    /// 3D position
    /// </summary>
    public struct Point3 : IPoint
    {
        /// <summary>
        /// Longitude in degrees
        /// </summary>
        public float X { get; set; }
        /// <summary>
        /// Latitude in degrees
        /// </summary>
        public float Y { get; set; }
        /// <summary>
        /// Altitude in meters
        /// </summary>
        public float Z { get; set; }

        //[JsonIgnore]
        public bool IsEmpty
        {
            get
            {
                return X.Equals(0) && Y.Equals(0) && Z.Equals(0);
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
        /// <param name="z"></param>
        public Point3(float x, float y, float z = 0)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return string.Format("X={0} Y={1} Z={2}", X, Y, Z);
        }

        public bool Equals(Position pos)
        {
            return X.Equals(pos.Longitude) && Y.Equals(pos.Latitude) && Z.Equals(pos.Altitude);
        }

        public bool Equals(Position pos, double err)
        {
            return Math.Abs(X - pos.Longitude) <= err &&
                Math.Abs(Y - pos.Latitude) <= err &&
                Math.Abs(Z - pos.Altitude) <= err;
        }

        /// <summary>
        /// Distance from a position in degrees
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Position DistanceFrom(IPoint pos)
        {
            return new Position(System.Math.Abs(pos.X - X), System.Math.Abs(pos.Y - Y));
        }
    }
}
