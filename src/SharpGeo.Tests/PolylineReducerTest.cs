using SharpGeo.Google;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SharpGeo.Tests
{
    [TestFixture]
    public class PolylineReducerTest
    {
        [Test]
        //[Ignore("Temporarily ignored")]
        public void Reduce()
        {
            var points = new List<IPoint>();
            using (var stream = TestHelper.GetResourceStream("GooglePolyline.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var decodedPoints = PolylineEncoder.Decode(reader.ReadToEnd());
                    points = (from p in decodedPoints select p as IPoint).ToList();
                }
            }
            Assert.NotNull(points);

            var reducedPoints = PolylineReducer.DouglasPeuckerReduction(points, 0.01);
            Assert.NotNull(reducedPoints);
            Assert.True(reducedPoints.Count > 0);
            Assert.True(reducedPoints.Count < points.Count);
        }

        [Test]
        public void ReduceMemoryFootprint()
        {
            var pointsAsJson = TestHelper.ReadFromResource("SamplePolyline1.json");
            var positions = JsonConvert.DeserializeObject<Position[]>(pointsAsJson);
            Assert.NotNull(positions);
            Assert.True(positions.Length > 0);
            var positionsAsList = (from p in positions select p as IPoint).ToList();
            // the following line causes stack overflow exception, which cannot be caught by the .NET runtime, as there is no
            // System.StackOverflowException implemented in WinRT, so this crashes the whole program
            var reducedPoints = PolylineReducer.DouglasPeuckerReduction(positionsAsList, 0.001);
            Assert.True(reducedPoints.Count > 0);
            Assert.True(reducedPoints.Count < positionsAsList.Count);
        }
    }
}
