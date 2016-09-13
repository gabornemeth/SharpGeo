using NUnit.Framework;
using System;

namespace GeoSharp.Tests
{
    [TestFixture]
    public class PositionTest
    {
        [Test]
        public void IsEmpty()
        {
            var pos = new Position();
            Assert.True(pos.IsEmpty);
        }

        [Test]
        public void NotEmpty()
        {
            var pos = new Position
            {
                Longitude = 16.86038f,
                Latitude = 46.8473f,
            };
            Assert.False(pos.IsEmpty);
        }
    }
}
