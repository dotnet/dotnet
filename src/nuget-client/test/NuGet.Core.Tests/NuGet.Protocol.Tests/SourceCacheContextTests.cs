// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
#if !NET7_0_OR_GREATER
using NuGet.Common;
#endif
using NuGet.Protocol.Core.Types;
#if NET7_0_OR_GREATER
using NuGet.Test.Utility;
#endif
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
        public void SourceCacheContext_GeneratedTempFolder_UsesTargetSpecificLocation()
        {
            using var target = new SourceCacheContext();

            var folder = target.GeneratedTempFolder;

#if NET7_0_OR_GREATER
            Assert.True(Directory.Exists(folder));
            Assert.Equal(Path.GetFullPath(Path.GetTempPath()).TrimEnd(Path.DirectorySeparatorChar), Path.GetDirectoryName(folder));
            Assert.StartsWith("NuGet-", Path.GetFileName(folder));
#else
            Assert.False(Directory.Exists(folder));
            Assert.Equal(NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp), Path.GetDirectoryName(folder));
#endif
        }

#if NET7_0_OR_GREATER
        [PlatformFact(Platform.Linux, Platform.Darwin)]
        public void SourceCacheContext_GeneratedTempFolder_HasOwnerOnlyPermissions()
        {
            using var target = new SourceCacheContext();

            var mode = File.GetUnixFileMode(target.GeneratedTempFolder);

            Assert.Equal(UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute, mode);
        }
#endif

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
