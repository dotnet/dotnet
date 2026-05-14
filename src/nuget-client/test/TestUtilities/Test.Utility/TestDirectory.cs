// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using PathIO = System.IO.Path;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// Temporary new folder under .test/work/
    /// </summary>
    public class TestDirectory : IDisposable
    {
        private readonly string _parentPath;

        private TestDirectory(string path, string parentPath)
        {
            Path = path;
            _parentPath = parentPath;
            Info = new DirectoryInfo(path);
        }

        public string Path { get; }

        public DirectoryInfo Info { get; }

        public void Dispose()
        {
            // Try to delete the sub folder first, avoid removing
            // the parent folder containing extra test info
            // if the sub folder cannot be removed.
            if (TestFileSystemUtility.DeleteRandomTestFolder(Path))
            {
                TestFileSystemUtility.DeleteRandomTestFolder(_parentPath);
            }
        }

        /// <summary>
        /// Create a temp folder under .test/work/
        /// </summary>
        public static TestDirectory Create()
        {
            var root = TestFileSystemUtility.NuGetTestFolder;
            return Create(root);
        }

        /// <summary>
        /// Create a temp folder under %TEMP%
        /// </summary>
        public static TestDirectory CreateInTemp()
        {
            var root = PathIO.Combine(PathIO.GetTempPath(), "NuGetTestFolder");
            Directory.CreateDirectory(root);

            return Create(root);
        }

        public static TestDirectory Create(string root)
        {
            string parentPath;

            // Loop until we find a directory that isn't taken (extremely unlikely this would need multiple guids).
            while (true)
            {
                var guid = Guid.NewGuid().ToString();

                // Use a shorter path to this easier when debugging
                parentPath = PathIO.Combine(root, guid.Split('-')[0]);

                if (Directory.Exists(parentPath))
                {
                    // If a collision happens use the full guid
                    parentPath = PathIO.Combine(root, guid);

                    if (!Directory.Exists(parentPath))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            Directory.CreateDirectory(parentPath);

            // Record what created this folder in case there is a problem with clean up.
            File.WriteAllText(PathIO.Combine(parentPath, "testStack.txt"), Environment.StackTrace);

            // Create a random sub folder to use, this keeps tests from relying on the folder name.
            var path = PathIO.Combine(parentPath, Guid.NewGuid().ToString().Split('-')[0]);
            Directory.CreateDirectory(path);

            return new TestDirectory(path, parentPath);
        }

        public static implicit operator string(TestDirectory directory) => directory.Path;

        public override string ToString() => Path;
    }
}
