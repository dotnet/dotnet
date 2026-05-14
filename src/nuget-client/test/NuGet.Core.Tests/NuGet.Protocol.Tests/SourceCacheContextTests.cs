// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class SourceCacheContextTests
    {
        [Fact]
        public void SourceCacheContext_StoresGeneratedTempFolder()
        {
            // Arrange
            using (var target = new SourceCacheContext())
            {
                // Act
                var folderA = target.GeneratedTempFolder;
                var folderB = target.GeneratedTempFolder;

                // Assert
                Assert.Equal(folderA, folderB);
            }
        }

        [Fact]
        public void SourceCacheContext_DisposeDeletesGeneratedTempFolder()
        {
            // Arrange
            string directoryPath;
            using (var target = new SourceCacheContext())
            {
                directoryPath = target.GeneratedTempFolder;
                Directory.CreateDirectory(directoryPath);
                var filePath = Path.Combine(directoryPath, "test.txt");
                File.WriteAllText(filePath, string.Empty);

                // Act
                target.Dispose();
                target.Dispose();

                // Assert
                Assert.False(Directory.Exists(directoryPath));
                Assert.False(File.Exists(filePath));
            }
        }
    }
}
