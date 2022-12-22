// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// Rerpresents a builder for <c>TextDirectory</c> objects, with
    /// utilities for creating file in the test directory. For testing purposes
    /// </summary>
    public class TestDirectoryBuilder
    {
        public NuspecBuilder NuspecBuilder { get; private set; }
        public string NuspecFile { get; private set; }
        public Dictionary<string, FileEntry> Files { get; private set; }
        public string BaseDir { get; private set; }
        public string NuspecPath => Path.Combine(BaseDir, NuspecFile);

        private TestDirectoryBuilder()
        {
            Files = new Dictionary<string, FileEntry>();
        }

        /// <summary>
        /// Factory method for creating the builder
        /// </summary>
        /// <returns>An instance of <c>TestDirectoryBuilder</c></returns>
        public static TestDirectoryBuilder Create()
        {
            return new TestDirectoryBuilder();
        }

        public TestDirectoryBuilder WithNuspec(NuspecBuilder nuspecBuilder, string filepath = "Package.nuspec")
        {
            NuspecBuilder = nuspecBuilder;
            NuspecFile = filepath;
            return this;
        }

        public TestDirectoryBuilder WithFile(string filepath, int size)
        {
            Files.Add(filepath, new FileEntry(filepath, size));

            return this;
        }

        public TestDirectoryBuilder WithFile(string filepath, string content)
        {
            Files.Add(filepath, new FileEntry(filepath, content));

            return this;
        }

        /// <summary>
        /// Creates the TestDirectory and all the files
        /// </summary>
        /// <returns>A <c>TestDirectory</c> object with the file structure created</returns>
        public TestDirectory Build()
        {
            TestDirectory testDirectory = TestDirectory.Create();
            BaseDir = testDirectory.Path;

            CreateFiles();

            if (NuspecBuilder != null)
            {
                var dir = Path.GetDirectoryName(NuspecPath);
                Directory.CreateDirectory(dir);
                File.WriteAllText(NuspecPath, NuspecBuilder.Build().ToString());
            }

            return testDirectory;
        }

        private void CreateFiles()
        {
            foreach (var f in Files)
            {
                var filepath = Path.Combine(BaseDir, f.Key);
                var dir = Path.GetDirectoryName(filepath);

                Directory.CreateDirectory(dir);

                if (f.Value.Content != null)
                {
                    File.WriteAllText(Path.Combine(BaseDir, f.Key), f.Value.Content);
                }
                else if (f.Value.Size > -1)
                {
                    using (var fileStream = File.OpenWrite(Path.Combine(BaseDir, f.Key)))
                    {
                        fileStream.SetLength(f.Value.Size);
                    }
                }
            }
        }

        /// <summary>
        /// Rerpesents a File to be created by <c>TestDirecotryBuilder</c>.
        /// For testing purposes.
        /// </summary>
        public class FileEntry
        {
            public string Path { get; }
            public string Content { get; }
            public long Size { get; } = -1;

            public FileEntry(string path, long size)
            {
                Path = path;
                Size = size;
            }

            public FileEntry(string path, string content)
            {
                Path = path;
                Content = content;
            }
        }
    }
}
