// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class PackageUtilitiesTests
    {
        [Fact]
        public async Task OpenNuspecFromNupkg_ReturnsTheNuspecOfThePackage()
        {
            using var workingDir = TestDirectory.Create();
            FileInfo package = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "a", "1.0.0");

            using FileStream stream = package.OpenRead();
            NuspecReader nuspec = PackageUtilities.OpenNuspecFromNupkg("a", stream, NullLogger.Instance);

            Assert.Equal("a", nuspec.GetId());
        }

        [Fact]
        public void OpenNuspecFromNupkg_WithoutNuspec_Throws()
        {
            using var stream = new MemoryStream();
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
            {
                zip.CreateEntry("lib/net472/a.dll");
            }
            stream.Position = 0;

            Assert.Throws<PackagingException>(() => PackageUtilities.OpenNuspecFromNupkg("a", stream, NullLogger.Instance));
        }
    }
}
