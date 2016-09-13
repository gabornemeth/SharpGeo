//
// Bound.cs
//
// Author:
//    Gabor Nemeth (gabor.nemeth.dev@gmail.com)
//
//    Copyright (C) 2015, Gabor Nemeth
//

using System;

namespace GeoSharp
{
    /// <summary>
    /// Geographical bound
    /// </summary>
    public struct Bound
    {
        private static Bound _empty = new Bound();

        public static Bound Empty
        {
            get
            {
                return _empty;
            }
        }

        public Position Min, Max;

        public void Reset()
        {
            Min = Max = Position.Empty;
        }

        public Bound(float north, float west, float south, float east)
            : this()
        {
            North = north;
            West = west;
            South = south;
            East = east;
        }

        public static Bound FromCenterAndRadius(float latitudeCenter, float longitudeCenter, float heightLatitude,
                                                   float widthLongitude)
        {
            return new Bound(latitudeCenter + heightLatitude * 0.5f, longitudeCenter - widthLongitude * 0.5f,
                latitudeCenter - heightLatitude * 0.5f, longitudeCenter + widthLongitude * 0.5f);
        }

        public bool IsEmpty
        {
            get
            {
                return Min.IsEmpty && Max.IsEmpty;
            }
        }

        public bool Equals(Bound bound)
        {
            return Min.Equals(bound.Min) && Max.Equals(bound.Max);
        }

        public bool Contains(float latitude, float longitude)
        {
            return (latitude >= Min.Latitude && latitude <= Max.Latitude &&
            longitude >= Min.Longitude && longitude <= Max.Longitude);
        }

        public void Enlarge(float latitude, float longitude)
        {
            Min = new Position(Min.Longitude - longitude, Min.Latitude - latitude, Min.Altitude);
            Max = new Position(Max.Longitude + longitude, Max.Latitude + latitude, Max.Altitude);
        }

        /// <summary>
        /// Check if the bound overlaps with another bound
        /// </summary>
        /// <param name="north"></param>
        /// <param name="west"></param>
        /// <param name="south"></param>
        /// <param name="east"></param>
        /// <returns></returns>
        public bool Overlaps(float north, float west, float south, float east)
        {
            return west <= East && east >= West && north >= South && south <= North;
        }

        public bool Overlaps(Bound bound)
        {
            return Overlaps(bound.North, bound.West, bound.South, bound.East);
        }

        public void Extend(float latitude, float longitude, float altitude = 0.0f)
        {
            if (Min.IsEmpty && Max.IsEmpty)
                Min = Max = new Position(longitude, latitude, altitude); // start with the first position

            // extend latitude
            if (latitude > Max.Latitude)
                Max.Latitude = latitude;
            else if (latitude < Min.Latitude)
                Min.Latitude = latitude;
            // extend longitude
            if (longitude > Max.Longitude)
                Max.Longitude = longitude;
            else if (longitude < Min.Longitude)
                Min.Longitude = longitude;
            // extend altitude
            if (altitude > Max.Altitude)
                Max.Altitude = altitude;
            else if (altitude < Min.Altitude)
                Min.Altitude = altitude;
        }

        public void Extend(Position pos)
        {
            Extend(pos.Latitude, pos.Longitude, pos.Altitude);
        }

        /// <summary>
        /// Center of the bound
        /// </summary>
        //[JsonIgnore]
        public Position Center
        {
            get
            {
                return new Position
                {
                    Latitude = (Min.Latitude + Max.Latitude) * 0.5f,
                    Longitude = (Min.Longitude + Max.Longitude) * 0.5f,
                    Altitude = (Min.Altitude + Max.Altitude) * 0.5f
                };
            }
        }

        /// <summary>
        /// Width in longitude degrees (from west to east)
        /// </summary>
        //[JsonIgnore]
        public float Width
        {
            get
            {
                return Max.Longitude - Min.Longitude;
            }
        }

        /// <summary>
        /// Height in latitude degrees (from north to south)
        /// </summary>
        //[JsonIgnore]
        public float Height
        {
            get
            {
                return Max.Latitude - Min.Latitude;
            }
        }

        /// <summary>
        /// North-west corner
        /// </summary>
        //[JsonIgnore]
        public Position NorthWest
        {
            get
            {
                return new Position { Longitude = West, Latitude = North };
            }
        }

        /// <summary>
        /// North-east corner
        /// </summary>
        //[JsonIgnore]
        public Position NorthEast
        {
            get
            {
                return new Position { Longitude = East, Latitude = North };
            }
        }

        /// <summary>
        /// South-west corner
        /// </summary>
        //[JsonIgnore]
        public Position SouthWest
        {
            get
            {
                return new Position { Longitude = West, Latitude = South };
            }
        }

        /// <summary>
        /// South-east corner
        /// </summary>
        //[JsonIgnore]
        public Position SouthEast
        {
            get
            {
                return new Position { Longitude = East, Latitude = South };
            }
        }

        /// <summary>
        /// South latitude in degrees
        /// </summary>
        //[JsonIgnore]
        public float South
        {
            get { return Min.Latitude; }
            private set { Min.Latitude = value; }
        }

        /// <summary>
        /// East longitude in degrees
        /// </summary>
        //[JsonIgnore]
        public float East
        {
            get { return Max.Longitude; }
            private set { Max.Longitude = value; }
        }

        /// <summary>
        /// West longitude in degrees
        /// </summary>
        //[JsonIgnore]
        public float West
        {
            get { return Min.Longitude; }
            private set { Min.Longitude = value; }
        }

        /// <summary>
        /// North latitude in degrees
        /// </summary>
        //[JsonIgnore]
        public float North
        {
            get { return Max.Latitude; }
            private set { Max.Latitude = value; }
        }
    }
}
