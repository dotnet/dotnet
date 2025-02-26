// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Linq;

namespace Microsoft.DotNet.MacOsPkg.Core
{
    internal static class Utilities
    {
        public static bool IsPkg(string path) =>
            Path.GetExtension(path).Equals(".pkg");

        public static bool IsAppBundle(string path) =>
            Path.GetExtension(path).Equals(".app");

        public static string? FindInPath(string name, string path, bool isDirectory, SearchOption searchOption = SearchOption.AllDirectories)
        {
            string[] results = isDirectory ? Directory.GetDirectories(path, name, searchOption) : Directory.GetFiles(path, name, searchOption);
            return results.FirstOrDefault();
        }

        public static void CleanupPath(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void CreateParentDirectory(string path)
        {
            string? parent = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(parent))
            {
                Directory.CreateDirectory(parent);
            }
        }
    }
}
