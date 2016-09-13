// 
// Base of this code is from:
// http://www.codeproject.com/Tips/312248/Google-Maps-Direction-API-V-Polyline-Decoder
// https://developers.google.com/maps/documentation/utilities/polylinealgorithm
//
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSharp
{
    /// <summary>
    /// Polyline decoder for Google Maps API
    /// </summary>
    public static class PolylineDecoder
    {
        private static int floor1e5(double coordinate)
        {
            return (int)Math.Floor(coordinate * 1e5);
        }

        private static String encodeSignedNumber(int num)
        {
            int sgn_num = num << 1;
            if (num < 0)
            {
                sgn_num = ~(sgn_num);
            }
            return (encodeNumber(sgn_num));
        }

        private static String encodeNumber(int num)
        {

            StringBuilder encodeString = new StringBuilder();

            while (num >= 0x20)
            {
                int nextValue = (0x20 | (num & 0x1f)) + 63;
                encodeString.Append((char)(nextValue));
                num >>= 5;
            }

            num += 63;
            encodeString.Append((char)(num));

            return encodeString.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static string Encode(IList<Position> positions)
        {
            var encodedPositions = new StringBuilder();
            int plat = 0; // previous latitude
            int plng = 0; // previous longitude

            foreach (var position in positions)
            {
                var late5 = floor1e5(position.Latitude);
                var lnge5 = floor1e5(position.Longitude);

                var dlat = late5 - plat;
                var dlng = lnge5 - plng;

                plat = late5;
                plng = lnge5;

                // encode only the differences
                encodedPositions.Append(encodeSignedNumber(dlat));
                encodedPositions.Append(encodeSignedNumber(dlng));
            }

            return encodedPositions.ToString();
        }

        public static List<Position> DecodePolylinePoints(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                return null;

            var poly = new List<Position>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylinechars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                var p = new Position
                {
                    Latitude = Convert.ToSingle(currentLat) / 100000.0f,
                    Longitude = Convert.ToSingle(currentLng) / 100000.0f
                };
                poly.Add(p);
            }
            return poly;
        }
    }
}
