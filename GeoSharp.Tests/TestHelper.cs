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

namespace GeoSharp.Tests
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
    }
}
