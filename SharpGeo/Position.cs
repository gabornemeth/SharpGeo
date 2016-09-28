using System;
//using Newtonsoft.Json;

namespace SharpGeo
{
    /// <summary>
    /// Geographic position in WGS84
    /// </summary>
    public struct Position : IPoint
    {
        /// <summary>
        /// Longitude in degrees
        /// </summary>
        public float Longitude { get; set; }
        /// <summary>
        /// Latitude in degrees
        /// </summary>
        public float Latitude { get; set; }
        /// <summary>
        /// Altitude in meters
        /// </summary>
        public float Altitude { get; set; }

        //[JsonIgnore]
        public bool IsEmpty
        {
            get
            {
                return Longitude.Equals(0) && Latitude.Equals(0) && Altitude.Equals(0);
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

        public float X
        {
            get
            {
                return Longitude;
            }
            set
            {
                Longitude = value;
            }
        }

        public float Y
        {
            get
            {
                return Latitude;
            }

            set
            {
                Latitude = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of <c>GeoPosition</c>
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="altitude"></param>
        public Position(float longitude, float latitude, float altitude = 0)
            : this()
        {
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
        }

        public override string ToString()
        {
            return string.Format("Lon={0} Lat={1} Alt={2}", Longitude, Latitude, Altitude);
        }

        public bool Equals(Position pos)
        {
            return Longitude.Equals(pos.Longitude) && Latitude.Equals(pos.Latitude) && Altitude.Equals(pos.Altitude);
        }

        public bool Equals(Position pos, double err)
        {
            return Math.Abs(Longitude - pos.Longitude) <= err &&
                Math.Abs(Latitude - pos.Latitude) <= err &&
                Math.Abs(Altitude - pos.Altitude) <= err;
        }

        /// <summary>
        /// Distance from a position in degrees
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Position DistanceFrom(Position pos)
        {
            return new Position(System.Math.Abs(pos.Longitude - Longitude), System.Math.Abs(pos.Latitude - Latitude));
        }
    }
}
