// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if !IS_CORECLR
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
#endif

namespace NuGet.Protocol.Tests
{
    // Negative tests here won't run well on *nix because bad test data used will trigger new exceptions
    // TODO: we can revisit to catch them if there is value.

#if !IS_CORECLR
    public class OfflineFeedUtilityTests
    {
        [Theory]
        [InlineData("c:\\foo|<>|bar")]
        [InlineData("c:\\foo|<>|bar.nupkg")]
        public void ThrowIfInvalid_ThrowsForInvalidPath(string path)
        {
            // Act & Assert
            var expectedMessage = string.Format("'{0}' is not a valid path.", path);

            var exception = Assert.Throws<ArgumentException>(() => OfflineFeedUtility.ThrowIfInvalid(path));

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("http://foonugetbar.org")]
        [InlineData("http://foonugetbar.org/A.nupkg")]
        public void ThrowIfInvalid_ThrowsIfPathNotFileOrUncPath(string path)
        {
            // Act & Assert
            var expectedMessage = string.Format("'{0}' should be a local path or a UNC share path.", path);

            var exception
                = Assert.Throws<ArgumentException>(() => OfflineFeedUtility.ThrowIfInvalid(path));

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("foo\\bar")]
        [InlineData("c:\\foo\\bar")]
        [InlineData("\\foouncshare\\bar")]
        public void ThrowIfInvalid_DoesNotThrowForValidPath(string path)
        {
            // Act & Assert that the following call does not throw
            OfflineFeedUtility.ThrowIfInvalid(path);
        }

        [Theory]
        [InlineData("c:\\foobardoesnotexist", true)]
        [InlineData("foobardoesnotexist\\A.nupkg", false)]
        public void ThrowIfInvalidOrNotFound_ThrowsForInvalidOrNonexistentPath(string path, bool isDirectory)
        {
            // Act & Assert
            var exception
                = Assert.Throws<ArgumentException>(()
                    => OfflineFeedUtility.ThrowIfInvalidOrNotFound(
                        path,
                        isDirectory,
                        "some exception message"));
        }

        [Fact]
        public async Task AddPackageToSource_ThrowsForNullOfflineFeedAddContextAsync()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => OfflineFeedUtility.AddPackageToSource(
                    offlineFeedAddContext: null,
                    token: CancellationToken.None));

            Assert.Equal("offlineFeedAddContext", exception.ParamName);
        }

        [Fact]
        public async Task AddPackageToSource_ThrowsIfCancelledAsync()
        {
            var extractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv3,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                clientPolicyContext: null,
                logger: NullLogger.Instance);

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => OfflineFeedUtility.AddPackageToSource(
                    new OfflineFeedAddContext(
                        "a",
                        "b",
                        NullLogger.Instance,
                        throwIfSourcePackageIsInvalid: false,
                        throwIfPackageExistsAndInvalid: false,
                        throwIfPackageExists: false,
                        extractionContext: extractionContext),
                    new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task AddPackageToSource_InstallsPackageAsync()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var packageIdentity = new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0"));
                var packageContext = new SimpleTestPackageContext()
                {
                    Id = packageIdentity.Id,
                    Version = packageIdentity.Version.ToNormalizedString(),
                    Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>{packageIdentity.Id}</id>
                            <version>{packageIdentity.Version.ToNormalizedString()}</version>
                            <title />
                            <frameworkAssemblies>
                                <frameworkAssembly assemblyName=""System.Runtime"" />
                            </frameworkAssemblies>
                            <contentFiles>
                                <files include=""lib/net45/{packageIdentity.Id}.dll"" copyToOutput=""true"" flatten=""false"" />
                            </contentFiles>
                        </metadata>
                        </package>")
                };

                packageContext.AddFile($"lib/net45/{packageIdentity.Id}.dll");

                var sourcePackageDirectoryPath = Path.Combine(testDirectory.Path, "source");
                var destinationDirectoryPath = Path.Combine(testDirectory.Path, "destination");

                Directory.CreateDirectory(destinationDirectoryPath);

                await SimpleTestPackageUtility.CreatePackagesAsync(sourcePackageDirectoryPath, packageContext);

                var sourcePackageFilePath = Path.Combine(
                    sourcePackageDirectoryPath,
                    $"{packageIdentity.Id}.{packageIdentity.Version.ToNormalizedString()}.nupkg");

                var destinationPackageDirectoryPath = Path.Combine(
                    destinationDirectoryPath,
                    packageIdentity.Id,
                    packageIdentity.Version.ToNormalizedString());

                var extractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    clientPolicyContext: null,
                    logger: NullLogger.Instance);

                var context = new OfflineFeedAddContext(
                    sourcePackageFilePath,
                    destinationDirectoryPath,
                    NullLogger.Instance,
                    throwIfSourcePackageIsInvalid: false,
                    throwIfPackageExistsAndInvalid: false,
                    throwIfPackageExists: false,
                    extractionContext: extractionContext);

                await OfflineFeedUtility.AddPackageToSource(context, CancellationToken.None);

                Assert.True(File.Exists(Path.Combine(destinationPackageDirectoryPath, $"{packageIdentity.Id}.{packageIdentity.Version.ToNormalizedString()}.nupkg")));
                Assert.True(File.Exists(Path.Combine(destinationPackageDirectoryPath, $"{packageIdentity.Id}.{packageIdentity.Version.ToNormalizedString()}.nupkg.sha512")));
                Assert.True(File.Exists(Path.Combine(destinationPackageDirectoryPath, $"{packageIdentity.Id}.nuspec")));
                Assert.True(File.Exists(Path.Combine(destinationPackageDirectoryPath, "lib", "net45", $"{packageIdentity.Id}.dll")));
            }
        }
    }
#endif
}
