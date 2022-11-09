// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Reflection;

namespace NuGet.Test.Utility
{
    public static class ResourceTestUtility
    {
        public static string GetResource(string name, Type type)
        {
            using (var reader = new StreamReader(type.GetTypeInfo().Assembly.GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }

        public static void CopyResourceToFile(string resourceName, Type type, string filePath)
        {
            var bytes = GetResourceBytes(resourceName, type);

            File.WriteAllBytes(filePath, bytes);
        }

        public static byte[] GetResourceBytes(string name, Type type)
        {
            using (var reader = new BinaryReader(type.GetTypeInfo().Assembly.GetManifestResourceStream(name)))
            {
                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }
    }
}
