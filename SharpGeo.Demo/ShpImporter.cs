using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpGeo.Demo
{
    /// <summary>
    /// Loading SHP file
    /// </summary>
    class ShpImporter
    {
        public void Load(string fileName)
        {
            DotSpatial.Data.Shapefile.Open(fileName);

            using (var stream = File.Open(fileName, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (reader.Read() >= 0)
                    {
                    }
                }
            }
        }
    }
}
