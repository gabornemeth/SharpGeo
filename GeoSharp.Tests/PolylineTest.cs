using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GeoSharp.Tests
{
    [TestFixture]
    public class PolylineTest
    {
        private string GetEncodedPoints()
        {
            using (var stream = TestHelper.GetResourceStream("GooglePolyline.txt"))
            {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }

        private IList<Position> DecodePositions()
        {
            var encodedPoints = GetEncodedPoints();
            var points = PolylineDecoder.DecodePolylinePoints(encodedPoints);
            Assert.IsNotNull(points);
            Assert.True(points.Count > 0);
            foreach (var point in points)
            {
                Assert.False(point.IsEmpty);
            }

            return points;
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
            var encodedPositions = GetEncodedPoints();
            var positions = DecodePositions();
            var reencodedPositions = PolylineDecoder.Encode(positions);
            //Assert.AreEqual(encodedPositions, reencodedPositions);
            var redecodedPositions = PolylineDecoder.DecodePolylinePoints(reencodedPositions);
            Assert.AreEqual(positions.Count, redecodedPositions.Count);
            for (var i = 0; i < positions.Count; i++)
            {
                Assert.IsTrue(positions[i].Equals(redecodedPositions[i], 2e-5));
                Debug.WriteLine(i + ". position is ok.");
            }
        }
    }
}
