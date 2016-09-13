using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GeoSharp.Google;

namespace GeoSharp.Tests
{
    /// <summary>
    /// Tests of Google's polyline encoding and decoding algorithms
    /// </summary>
    [TestFixture]
    public class PolylineEncodingTest
    {
        private string GetEncodedPositions()
        {
            using (var stream = TestHelper.GetResourceStream("GooglePolyline.txt"))
            {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }

        private IList<Position> DecodePositions()
        {
            var encodedPositions = GetEncodedPositions();
            var decodedPositions = PolylineEncoder.Decode(encodedPositions);
            Assert.IsNotNull(decodedPositions);
            Assert.True(decodedPositions.Count > 0);
            foreach (var point in decodedPositions)
            {
                Assert.False(point.IsEmpty);
            }

            return decodedPositions;
        }

        /// <summary>
        /// Decoding polyline encoded with
        /// </summary>
        [Test]
        public void Decode()
        {
            DecodePositions();
        }

        /// <summary>
        /// Decoding the polyline then reencode
        /// </summary>
        [Test]
        public void DecodeAndEncode()
        {
            var encodedPositions = GetEncodedPositions();
            var decodedPositions = DecodePositions();
            // encode then decode the positions again
            var reencodedPositions = PolylineEncoder.Encode(decodedPositions);
            var redecodedPositions = PolylineEncoder.Decode(reencodedPositions);
            // compare the positions got this time with the first ones
            Assert.AreEqual(decodedPositions.Count, redecodedPositions.Count);
            for (var i = 0; i < decodedPositions.Count; i++)
            {
                Assert.IsTrue(decodedPositions[i].Equals(redecodedPositions[i], 2e-5));
                Debug.WriteLine(i + ". position is ok.");
            }
        }
    }
}
