//
// TestHelper.cs
//
// Author:
//    Gabor Nemeth (gabor.nemeth.dev@gmail.com)
//
//    Copyright (C) 2015, Gabor Nemeth
//

using System;
using System.IO;
using System.Reflection;

namespace SharpGeo.Tests
{
    public static class TestHelper
    {
        public static Stream GetResourceStream(string resourceName)
        {
            var asm = typeof(TestHelper).GetTypeInfo().Assembly;
            foreach (var name in asm.GetManifestResourceNames())
            {
                if (name.Contains(resourceName))
                    return asm.GetManifestResourceStream(name);
            }

            return null;
        }

        public static string ReadFromResource(string resourceName)
        {
            using (var stream = TestHelper.GetResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
