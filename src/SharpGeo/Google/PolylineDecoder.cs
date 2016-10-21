// 
// Base of this code is from:
// http://www.codeproject.com/Tips/312248/Google-Maps-Direction-API-V-Polyline-Decoder
// https://github.com/balb/csharp-polyline-encoder
//
// Documentation from Google
// https://developers.google.com/maps/documentation/utilities/polylinealgorithm
//
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGeo.Google
{
    /// <summary>
    /// Polyline decoder for Google Maps API
    /// </summary>
    public static class PolylineEncoder
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
            return (EncodeNumber(sgn_num));
        }

        private static string EncodeNumber(int num)
        {
            var encodedText = new StringBuilder();

            while (num >= 0x20)
            {
                int nextValue = (0x20 | (num & 0x1f)) + 63;
                encodedText.Append((char)(nextValue));
                num >>= 5;
            }

            num += 63;
            encodedText.Append((char)(num));

            return encodedText.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static string Encode(IList<Position> positions)
        {
            var encodedPositions = new StringBuilder();
            int prevLatitude = 0; // previous latitude
            int prevLongitude = 0; // previous longitude

            foreach (var position in positions)
            {
                var latitude = floor1e5(position.Latitude);
                var longitude = floor1e5(position.Longitude);

                var diffLatitude = latitude - prevLatitude;
                var diffLongitude = longitude - prevLongitude;

                prevLatitude = latitude;
                prevLongitude = longitude;

                // encode only the differences
                encodedPositions.Append(encodeSignedNumber(diffLatitude));
                encodedPositions.Append(encodeSignedNumber(diffLongitude));
            }

            return encodedPositions.ToString();
        }

        public static List<Position> Decode(string encodedPositions)
        {
            if (string.IsNullOrEmpty(encodedPositions))
                return null;

            var poly = new List<Position>();
            var polylineCharacters = encodedPositions.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylineCharacters.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineCharacters[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineCharacters.Length);

                if (index >= polylineCharacters.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineCharacters[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineCharacters.Length);

                if (index >= polylineCharacters.Length && next5bits >= 32)
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
