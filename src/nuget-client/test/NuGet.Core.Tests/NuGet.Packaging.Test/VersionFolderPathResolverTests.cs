// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class VersionFolderPathResolverTests
    {
        [Fact]
        public void RootPath_ReturnsRootPath()
        {
            // Arrange && Act
            var resolver = new VersionFolderPathResolver("/tmp/test", isLowercase: false);

            // Assert
            Assert.Equal("/tmp/test", resolver.RootPath);
            Assert.False(resolver.IsLowerCase);
        }

        [Theory]
        [InlineData("nuget.packaging", "3.4.3-beta", true)]
        [InlineData("NuGet.Packaging", "3.4.3-Beta", false)]
        public void GetInstallPath_ReturnsInstallPath(string id, string version, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetInstallPath(tc.Id, tc.Version);

            // Assert
            Assert.Equal(Path.Combine(tc.PackagesPath, id, version), actual);
        }

        [Theory]
        [InlineData("nuget.packaging", "3.4.3-beta", "nuget.packaging.3.4.3-beta.nupkg", true)]
        [InlineData("NuGet.Packaging", "3.4.3-Beta", "NuGet.Packaging.3.4.3-Beta.nupkg", false)]
        public void GetPackageFilePath_ReturnsPackageFilePath(string id, string version, string file, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetPackageFilePath(tc.Id, tc.Version);

            // Assert
            Assert.Equal(
                Path.Combine(tc.PackagesPath, id, version, file),
                actual);
        }

        [Theory]
        [InlineData("nuget.packaging", "3.4.3-beta", "nuget.packaging.nuspec", true)]
        [InlineData("NuGet.Packaging", "3.4.3-Beta", "NuGet.Packaging.nuspec", false)]
        public void GetManifestFilePath_ReturnsManifestFilePath(string id, string version, string file, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetManifestFilePath(tc.Id, tc.Version);

            // Assert
            Assert.Equal(
                Path.Combine(tc.PackagesPath, id, version, file),
                actual);
        }

        [Theory]
        [InlineData("nuget.packaging", "3.4.3-beta", "nuget.packaging.3.4.3-beta.nupkg.sha512", true)]
        [InlineData("NuGet.Packaging", "3.4.3-Beta", "NuGet.Packaging.3.4.3-Beta.nupkg.sha512", false)]
        public void GetHashPath_ReturnsHashPath(string id, string version, string file, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetHashPath(tc.Id, tc.Version);

            // Assert
            Assert.Equal(
                Path.Combine(tc.PackagesPath, id, version, file),
                actual);
        }

        [Theory]
        [InlineData("nuget.packaging.3.4.3-beta.nupkg.sha512", true)]
        [InlineData("NuGet.Packaging.3.4.3-Beta.nupkg.sha512", false)]
        public void GetHashFileName_ReturnsHashFileName(string file, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetHashFileName(tc.Id, tc.Version);

            // Assert
            Assert.Equal(file, actual);
        }

        [Theory]
        [InlineData("nuget.packaging", "3.4.3-beta", true)]
        [InlineData("NuGet.Packaging", "3.4.3-Beta", false)]
        public void GetPackageDirectory_ReturnsPackageDirectory(string id, string version, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetPackageDirectory(tc.Id, tc.Version);

            // Assert
            Assert.Equal(Path.Combine(id, version), actual);
        }

        [Theory]
        [InlineData("nuget.packaging.3.4.3-beta.nupkg", true)]
        [InlineData("NuGet.Packaging.3.4.3-Beta.nupkg", false)]
        public void GetPackageFileName_ReturnsPackageFileName(string file, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetPackageFileName(tc.Id, tc.Version);

            // Assert
            Assert.Equal(file, actual);
        }

        [Theory]
        [InlineData("nuget.packaging.nuspec", true)]
        [InlineData("NuGet.Packaging.nuspec", false)]
        public void GetManifestFileName_ReturnsManifestFileName(string file, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetManifestFileName(tc.Id, tc.Version);

            // Assert
            Assert.Equal(file, actual);
        }

        [Theory]
        [InlineData("nuget.packaging", true)]
        [InlineData("NuGet.Packaging", false)]
        public void GetVersionListPath_ReturnsVersionListPath(string directory, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetVersionListPath(tc.Id);

            // Assert
            Assert.Equal(
                Path.Combine(tc.PackagesPath, directory),
                actual);
        }

        [Theory]
        [InlineData("nuget.packaging", true)]
        [InlineData("NuGet.Packaging", false)]
        public void GetVersionListDirectory_ReturnsVersionListDirectory(string directory, bool isLowercase)
        {
            // Arrange
            var tc = new TestContext { IsLowercase = isLowercase };

            // Act
            var actual = tc.Target.GetVersionListDirectory(tc.Id);

            // Assert
            Assert.Equal(directory, actual);
        }

        [Theory]
        [InlineData("nuget.packaging.packagedownload.marker", true)]
        [InlineData("NuGet.Packaging.packagedownload.marker", false)]
        public void GetPackageDownloadMarkerFileName_ReturnsPackageDownloadMarkerFileName(
            string expectedFileName,
            bool isLowercase)
        {
            var context = new TestContext { IsLowercase = isLowercase };

            var actualFileName = context.Target.GetPackageDownloadMarkerFileName(context.Id);

            Assert.Equal(expectedFileName, actualFileName);
        }

        [Fact]
        public void GetInstallPath_ReturnsOverridenValue()
        {
            var context = new TestContext(useExtendedResolver: true);

            var actualFileName = context.Target.GetInstallPath(context.Id, context.Version);

            Assert.Equal(string.Empty, actualFileName);
        }

        [Fact]
        public void GetPackageFileName_ReturnsOverridenValue()
        {
            var context = new TestContext(useExtendedResolver: true);

            var actualFileName = context.Target.GetPackageFileName(context.Id, context.Version);

            Assert.Equal(string.Empty, actualFileName);
        }

        [Fact]
        public void GetVersionListDirectory_ReturnsOverridenValue()
        {
            var context = new TestContext(useExtendedResolver: true);

            var actualFileName = context.Target.GetVersionListDirectory(context.Id);

            Assert.Equal(string.Empty, actualFileName);
        }

        [Fact]
        public void GetManifestFileName_ReturnsOverridenValue()
        {
            var context = new TestContext(useExtendedResolver: true);

            var actualFileName = context.Target.GetManifestFileName(context.Id, context.Version);

            Assert.Equal(string.Empty, actualFileName);
        }

        [Fact]
        public void GetPackageDirectory_ReturnsOverridenValue()
        {

            var context = new TestContext(useExtendedResolver: true);

            var actualFileName = context.Target.GetPackageDirectory(context.Id, context.Version);

            Assert.Equal(string.Empty, actualFileName);
        }

        private class TestContext
        {
            private bool _useExtendedResolver;
            public TestContext()
            {
                // data
                PackagesPath = "prefix";
                Id = "NuGet.Packaging";
                Version = new NuGetVersion("3.04.3-Beta");
                IsLowercase = true;
            }

            public TestContext(bool useExtendedResolver) : base()
            {
                _useExtendedResolver = useExtendedResolver;
            }

            public string Id { get; private set; }
            public string PackagesPath { get; set; }
            public NuGetVersion Version { get; set; }
            public bool IsLowercase { get; set; }
            public VersionFolderPathResolver Target
            {
                get
                {
                    return _useExtendedResolver ?
                        new VersionFolderPathResolverExtended(PackagesPath) :
                        new VersionFolderPathResolver(PackagesPath, IsLowercase);
                }
            }
        }
    }

    internal class VersionFolderPathResolverExtended : VersionFolderPathResolver
    {
        public VersionFolderPathResolverExtended(string rootPath) : base(rootPath)
        {

        }
        public override string GetInstallPath(string packageId, NuGetVersion version)
        {
            return string.Empty;
        }

        public override string GetPackageFileName(string packageId, NuGetVersion version)
        {
            return string.Empty;
        }

        public override string GetVersionListDirectory(string packageId)
        {
            return string.Empty;
        }

        public override string GetManifestFileName(string packageId, NuGetVersion version)
        {
            return string.Empty;
        }

        public override string GetPackageDirectory(string packageId, NuGetVersion version)
        {
            return string.Empty;
        }
    }
}
