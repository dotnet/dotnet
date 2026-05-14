// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace Microsoft.Build.NuGetSdkResolver.Test
{
    public class RestoreRunnerExTests
    {
        [Fact]
        public async Task RestoreRunnerEx_WithExistingPackage_DoesNotCreateAnyAssetsAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            using (var context = new SourceCacheContext())
            {
                // Arrange
                var packageId = "x";
                var packageVersion = "1.0.0";
                var logger = new TestLogger();
                var library = new LibraryIdentity(packageId, NuGetVersion.Parse(packageVersion), LibraryType.Reference);

                var package = new SimpleTestPackageContext(packageId, packageVersion);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    package);

                // Act
                var results = await RestoreRunnerEx.RunWithoutCommit(library, Settings.LoadDefaultSettings(pathContext.SolutionRoot), logger);

                // Assert
                results.Count.Should().Be(1);
                RestoreResult restoreResult = results.Single().Result;
                restoreResult.Success.Should().BeTrue();
                File.Exists(restoreResult.LockFilePath).Should().BeFalse();
                File.Exists(Path.GetDirectoryName(restoreResult.LockFilePath)).Should().BeFalse();
            }
        }
    }
}
