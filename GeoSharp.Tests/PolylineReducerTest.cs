using GeoSharp.Google;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace GeoSharp.Tests
{
    [TestFixture]
    public class PolylineReducerTest
    {
        [Test]
        public void Reduce()
        {
            var points = new List<IPoint>();
            using (var stream = TestHelper.GetResourceStream("GooglePolyline.txt"))
            {
                var reader = new StreamReader(stream);
                var decodedPoints = PolylineEncoder.Decode(reader.ReadToEnd());
                points = (from p in decodedPoints select p as IPoint).ToList();
            }
            Assert.NotNull(points);

            var reducedPoints = PolylineReducer.DouglasPeuckerReduction(points, 0.01);
            Assert.NotNull(reducedPoints);
            Assert.True(reducedPoints.Count > 0);
            Assert.True(reducedPoints.Count < points.Count);
        }
    }
}
