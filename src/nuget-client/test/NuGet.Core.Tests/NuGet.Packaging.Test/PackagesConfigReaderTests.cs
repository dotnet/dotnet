// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class PackagesConfigReaderTests
    {
        [Fact]
        public void PackagesConfigReader_Basic()
        {
            var reader = new PackagesConfigReader(PackagesConf1);

            var version = reader.GetMinClientVersion();

            Assert.Equal("2.5.0", version.ToNormalizedString());

            var packageEntries = reader.GetPackages().ToArray();

            Assert.Equal(1, packageEntries.Length);
            Assert.Equal("Newtonsoft.Json", packageEntries[0].PackageIdentity.Id);
            Assert.Equal("6.0.4", packageEntries[0].PackageIdentity.Version.ToNormalizedString());
            Assert.Equal("net45", packageEntries[0].TargetFramework.GetShortFolderName());
            Assert.False(packageEntries[0].HasAllowedVersions);
            Assert.False(packageEntries[0].IsDevelopmentDependency);
            Assert.True(packageEntries[0].IsUserInstalled);
            Assert.False(packageEntries[0].RequireReinstallation);
            Assert.Null(packageEntries[0].AllowedVersions);
        }

        [Fact]
        public void PackagesConfigReader_Basic2()
        {
            var reader = new PackagesConfigReader(PackagesConf2);

            var version = reader.GetMinClientVersion();

            Assert.Equal("2.5.0", version.ToNormalizedString());

            var packageEntries = reader.GetPackages().ToArray();

            Assert.Equal(1, packageEntries.Length);
            Assert.Equal("Newtonsoft.Json", packageEntries[0].PackageIdentity.Id);
            Assert.Equal("6.0.4", packageEntries[0].PackageIdentity.Version.ToNormalizedString());
            Assert.Equal("net45", packageEntries[0].TargetFramework.GetShortFolderName());
            Assert.True(packageEntries[0].HasAllowedVersions);
            Assert.True(packageEntries[0].IsDevelopmentDependency);
            Assert.True(packageEntries[0].IsUserInstalled);
            Assert.True(packageEntries[0].RequireReinstallation);
            Assert.Equal("[6.0.0, )", packageEntries[0].AllowedVersions.ToString());
        }

        [Fact]
        public void PackagesConfigReader_Basic3()
        {
            var reader = new PackagesConfigReader(PackagesConf3);

            var version = reader.GetMinClientVersion();

            Assert.Equal("3.0.0", version.ToNormalizedString());

            var packageEntries = reader.GetPackages().ToArray();

            Assert.Equal(2, packageEntries.Length);
            Assert.Equal("Newtonsoft.Json", packageEntries[0].PackageIdentity.Id);
            Assert.Equal("TestPackage", packageEntries[1].PackageIdentity.Id);
        }

        private static XDocument PackagesConf1
        {
            get { return XDocument.Parse(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                <packages>
                                    <package id=""Newtonsoft.Json"" version=""6.0.4"" targetFramework=""net45"" />
                                </packages>"); }
        }

        private static XDocument PackagesConf2
        {
            get { return XDocument.Parse(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                <packages>
                                    <package id=""Newtonsoft.Json"" version=""6.0.4"" targetFramework=""net45"" allowedVersions=""6.0.0"" developmentDependency=""true"" requireReinstallation=""true"" userInstalled=""true"" />
                                </packages>"); }
        }

        private static XDocument PackagesConf3
        {
            get { return XDocument.Parse(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                <packages minClientVersion=""3.0.0"">
                                    <package id=""Newtonsoft.Json"" version=""6.0.4"" targetFramework=""net45"" />
                                    <package id=""TestPackage"" version=""1.0.0"" targetFramework=""net4"" />
                                </packages>"); }
        }

        [Fact]
        public void PackagesConfigReader_BadMinClientVersion()
        {
            var doc = XDocument.Parse(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages minClientVersion=""abc"">
  <package id=""test"" version=""1.0"" />
</packages>");
            var reader = new PackagesConfigReader(doc);

            var ex = Assert.Throws<PackagesConfigReaderException>(() => reader.GetMinClientVersion());
            Assert.Equal(ex.Message, "Invalid minClientVersion: 'abc'");
        }

        [Fact]
        public void PackagesConfigReader_BadId()
        {
            var doc = XDocument.Parse(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id="""" />
</packages>");
            var reader = new PackagesConfigReader(doc);

            var ex = Assert.Throws<PackagesConfigReaderException>(() => reader.GetPackages());
            Assert.Equal(ex.Message, "Null or empty package id");
        }

        [Fact]
        public void PackagesConfigReader_EmptyVersion()
        {
            var doc = XDocument.Parse(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""test"" />
</packages>");
            var reader = new PackagesConfigReader(doc);

            var ex = Assert.Throws<PackagesConfigReaderException>(() => reader.GetPackages());
            Assert.Equal(ex.Message, "Invalid package version for package id 'test': ''");
        }

        [Fact]
        public void PackagesConfigReader_InvalidVersion()
        {
            var doc = XDocument.Parse(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""test"" version=""abc""/>
</packages>");
            var reader = new PackagesConfigReader(doc);

            var ex = Assert.Throws<PackagesConfigReaderException>(() => reader.GetPackages());
            Assert.Equal(ex.Message, "Invalid package version for package id 'test': 'abc'");
        }

        [Fact]
        public void PackagesConfigReader_InvalidAllowedVersions()
        {
            var doc = XDocument.Parse(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""test"" version=""1.0"" allowedVersions=""xyz""/>
</packages>");
            var reader = new PackagesConfigReader(doc);

            var ex = Assert.Throws<PackagesConfigReaderException>(() => reader.GetPackages());
            Assert.Equal(ex.Message, "Invalid allowedVersions for package id 'test': 'xyz'");
        }

        [Fact]
        public void PackagesConfigReader_DuplicateEntries()
        {
            // Arrange
            var doc = XDocument.Parse(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                  <package id=""test1"" version=""1.0""/>
                  <package id=""test1"" version=""1.0""/>

                  <package id=""test2"" version=""1.0""/>
                  <package id=""test2"" version=""1.0""/>
                </packages>");
            var reader = new PackagesConfigReader(doc);

            // Act
            var ex = Assert.Throws<PackagesConfigReaderException>(() => reader.GetPackages());

            // Assert
            Assert.Equal(ex.Message, "There are duplicate packages: test1, test2");
        }

        [Fact]
        public void PackagesConfigReader_DuplicateEntries_Casing()
        {
            // Arrange
            var doc = XDocument.Parse(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                  <package id=""test1"" version=""1.0""/>
                  <package id=""TEST1"" version=""1.0""/>

                  <package id=""test2"" version=""1.0""/>
                  <package id=""TEST2"" version=""1.0""/>
                </packages>");
            var reader = new PackagesConfigReader(doc);

            // Act
            var ex = Assert.Throws<PackagesConfigReaderException>(() => reader.GetPackages());

            // Assert
            Assert.Equal(ex.Message, "There are duplicate packages: test1, test2");
        }

        [Fact]
        public void PackagesConfigReader_AllowDuplicateEntriesFailOnDuplicateVersions()
        {
            // Arrange
            var doc = XDocument.Parse(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                  <package id=""test1"" version=""1.0""/>
                  <package id=""test1"" version=""1.0""/>
                  <package id=""test2"" version=""1.0""/>
                  <package id=""test2"" version=""2.0""/>
                </packages>");
            var reader = new PackagesConfigReader(doc);

            // Act
            var ex = Assert.Throws<PackagesConfigReaderException>(() =>
                reader.GetPackages(allowDuplicatePackageIds: true));

            // Assert
            Assert.Equal(ex.Message, "There are duplicate packages: test1.1.0.0");
        }

        [Fact]
        public void PackagesConfigReader_AllowDuplicateEntriesNoFailures()
        {
            // Arrange
            var doc = XDocument.Parse(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                  <package id=""test1"" version=""1.0""/>
                  <package id=""test1"" version=""1.1.0""/>
                  <package id=""test2"" version=""1.0""/>
                  <package id=""test2"" version=""2.0""/>
                </packages>");
            var reader = new PackagesConfigReader(doc);

            // Act
            var packages = reader.GetPackages(allowDuplicatePackageIds: true);

            // Assert
            Assert.Equal(4, packages.Count());
        }

        [Fact]
        public void PackagesConfigReader_DuplicateEntriesWithNonNormalizedVersions()
        {
            // Arrange
            var doc = XDocument.Parse(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                  <package id=""test1"" version=""1.1.0""/>
                  <package id=""test1"" version=""1.1""/>
                </packages>");
            var reader = new PackagesConfigReader(doc);

            // Act
            var ex = Assert.Throws<PackagesConfigReaderException>(() =>
                reader.GetPackages(allowDuplicatePackageIds: true));

            // Assert
            Assert.Equal(ex.Message, "There are duplicate packages: test1.1.1.0");
        }
    }
}
