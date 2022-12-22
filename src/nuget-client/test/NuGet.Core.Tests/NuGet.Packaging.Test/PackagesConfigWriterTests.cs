// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class PackagesConfigWriterTests
    {
        [Fact]
        public void PackagesConfigWriter_Basic()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                // Act
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                    writer.WriteMinClientVersion(NuGetVersion.Parse("3.0.1"));

                    writer.AddPackageEntry("packageB", NuGetVersion.Parse("2.0.0"), NuGetFramework.Parse("portable-net45+win8"));

                    writer.AddPackageEntry("packageA", NuGetVersion.Parse("1.0.1"), NuGetFramework.Parse("net45"));
                }

                stream.Seek(0, SeekOrigin.Begin);

                var xml = XDocument.Load(stream);

                // Assert
                Assert.Equal("utf-8", xml.Declaration.Encoding);

                var reader = new PackagesConfigReader(xml);

                Assert.Equal("3.0.1", reader.GetMinClientVersion().ToNormalizedString());

                var packages = reader.GetPackages().ToArray();
                Assert.Equal("packageA", packages[0].PackageIdentity.Id);
                Assert.Equal("packageB", packages[1].PackageIdentity.Id);

                Assert.Equal("1.0.1", packages[0].PackageIdentity.Version.ToNormalizedString());
                Assert.Equal("2.0.0", packages[1].PackageIdentity.Version.ToNormalizedString());

                Assert.Equal("net45", packages[0].TargetFramework.GetShortFolderName());
                Assert.Equal("portable-net45+win8", packages[1].TargetFramework.GetShortFolderName());
            }
        }

        [Fact]
        public void PackagesConfigWriter_BasicWithDifferentCulture()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var currentUICulture = CultureInfo.CurrentUICulture;

            try
            {
                var calendar = new CultureInfo("he-IL");
                calendar.DateTimeFormat.Calendar = new HebrewCalendar();

                CultureInfo.CurrentCulture = calendar;
                CultureInfo.CurrentUICulture = calendar;

                // Arrange
                using (var testFolder = TestDirectory.Create())
                {
                    var path = Path.Combine(testFolder + "packages.config");

                    // Act
                    using (var writer = new PackagesConfigWriter(path, true))
                    {
                        writer.WriteMinClientVersion(NuGetVersion.Parse("3.0.1"));

                        writer.AddPackageEntry("packageB", NuGetVersion.Parse("2.0.0"), NuGetFramework.Parse("portable-net45+win8"));

                        writer.AddPackageEntry("packageA", NuGetVersion.Parse("1.0.1"), NuGetFramework.Parse("net45"));
                    }

                    // Assert
                    var xml = XDocument.Load(path);

                    // Assert
                    Assert.Equal("utf-8", xml.Declaration.Encoding);

                    var reader = new PackagesConfigReader(xml);

                    Assert.Equal("3.0.1", reader.GetMinClientVersion().ToNormalizedString());

                    var packages = reader.GetPackages().ToArray();
                    Assert.Equal("packageA", packages[0].PackageIdentity.Id);
                    Assert.Equal("packageB", packages[1].PackageIdentity.Id);

                    Assert.Equal("1.0.1", packages[0].PackageIdentity.Version.ToNormalizedString());
                    Assert.Equal("2.0.0", packages[1].PackageIdentity.Version.ToNormalizedString());

                    Assert.Equal("net45", packages[0].TargetFramework.GetShortFolderName());
                    Assert.Equal("portable-net45+win8", packages[1].TargetFramework.GetShortFolderName());
                }
            }
            finally
            {
                CultureInfo.CurrentCulture = currentCulture;
                CultureInfo.CurrentUICulture = currentUICulture;
            }
        }

        [Fact]
        public void PackagesConfigWriter_Update()
        {
            // Arrage
            using (var stream = new MemoryStream())
            {
                // Act
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                    var packageIdentityA = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.1"));
                    var packageReferenceA = new PackageReference(packageIdentityA, NuGetFramework.Parse("net45"));

                    writer.AddPackageEntry(packageReferenceA);

                    var packageIdentityB = new PackageIdentity("packageA", NuGetVersion.Parse("2.0.0"));
                    var packageReferenceB = new PackageReference(packageIdentityB, NuGetFramework.Parse("portable-net45+win8"));

                    writer.UpdatePackageEntry(packageReferenceA, packageReferenceB);
                }

                stream.Seek(0, SeekOrigin.Begin);

                var xml = XDocument.Load(stream);

                // Assert
                Assert.Equal("utf-8", xml.Declaration.Encoding);

                var reader = new PackagesConfigReader(xml);

                var packages = reader.GetPackages().ToArray();
                Assert.Equal("1", packages.Count().ToString());
                Assert.Equal("packageA", packages[0].PackageIdentity.Id);
                Assert.Equal("2.0.0", packages[0].PackageIdentity.Version.ToNormalizedString());
                Assert.Equal("portable-net45+win8", packages[0].TargetFramework.GetShortFolderName());
            }
        }

        [Fact]
        public void PackagesConfigWriter_UpdateAttributes()
        {
            // Arrange
            var stream = new MemoryStream();
            {
                // Act
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                    var vensionRange = new VersionRange(NuGetVersion.Parse("0.5.0"));
                    var packageIdentityA = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.1"));
                    var packageReferenceA = new PackageReference(packageIdentityA, NuGetFramework.Parse("net45"),
                        userInstalled: false, developmentDependency: false, requireReinstallation: true, allowedVersions: vensionRange);

                    writer.AddPackageEntry(packageReferenceA);

                    var packageIdentityB = new PackageIdentity("packageA", NuGetVersion.Parse("3.0.1"));
                    var packageReferenceB = new PackageReference(packageIdentityB, NuGetFramework.Parse("dnxcore50"),
                        userInstalled: false, developmentDependency: false, requireReinstallation: false);

                    writer.UpdatePackageEntry(packageReferenceA, packageReferenceB);
                }

                stream.Seek(0, SeekOrigin.Begin);

                var xml = XDocument.Load(stream);

                // Assert
                Assert.Equal("utf-8", xml.Declaration.Encoding);

                var reader = new PackagesConfigReader(xml);

                var packages = reader.GetPackages().ToArray();
                Assert.Equal("1", packages.Count().ToString());
                Assert.Equal("packageA", packages[0].PackageIdentity.Id);
                Assert.Equal("3.0.1", packages[0].PackageIdentity.Version.ToNormalizedString());
                Assert.Equal("dnxcore50", packages[0].TargetFramework.GetShortFolderName());

                // Verify allowedVersions attribute is kept after package update.
                Assert.Equal("[0.5.0, )", packages[0].AllowedVersions.ToNormalizedString());

                // Verify that RequireReinstallation attribute is removed after package upate.
                Assert.Equal("False", packages[0].RequireReinstallation.ToString());
            }
        }

        [Fact]
        public void PackagesConfigWriter_UpdateAttributesFromOriginalConfig()
        {
            // Arrange
            using (var stream = new MemoryStream())
            using (var stream2 = new MemoryStream())
            {
                // Act
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                    var vensionRange = new VersionRange(NuGetVersion.Parse("0.5.0"));
                    var packageIdentityA = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.1"));
                    var packageReferenceA = new PackageReference(packageIdentityA, NuGetFramework.Parse("net45"),
                        userInstalled: false, developmentDependency: false, requireReinstallation: true, allowedVersions: vensionRange);

                    writer.AddPackageEntry(packageReferenceA);
                }

                stream.Seek(0, SeekOrigin.Begin);
                var xml = XDocument.Load(stream);

                var packageIdentityB = new PackageIdentity("packageA", NuGetVersion.Parse("3.0.1"));
                var packageReferenceB = new PackageReference(packageIdentityB, NuGetFramework.Parse("dnxcore50"),
                    userInstalled: false, developmentDependency: false, requireReinstallation: false);

                using (var writer = new PackagesConfigWriter(stream2, true))
                {
                    writer.UpdateOrAddPackageEntry(xml, packageReferenceB);
                }

                stream2.Seek(0, SeekOrigin.Begin);
                var xml2 = XDocument.Load(stream2);
                var reader = new PackagesConfigReader(xml2);

                // Assert

                var packages = reader.GetPackages().ToArray();
                Assert.Equal("1", packages.Count().ToString());
                Assert.Equal("packageA", packages[0].PackageIdentity.Id);
                Assert.Equal("3.0.1", packages[0].PackageIdentity.Version.ToNormalizedString());
                Assert.Equal("dnxcore50", packages[0].TargetFramework.GetShortFolderName());

                // Verify allowedVersions attribute is kept after package update.
                Assert.Equal("[0.5.0, )", packages[0].AllowedVersions.ToNormalizedString());

                // Verify that RequireReinstallation attribute is removed after package upate.
                Assert.Equal("False", packages[0].RequireReinstallation.ToString());
            }
        }

        [Fact]
        public void PackagesConfigWriter_UpdateError()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                // Act
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                    var packageIdentityA = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.1"));
                    var packageReferenceA = new PackageReference(packageIdentityA, NuGetFramework.Parse("net45"));

                    writer.AddPackageEntry(packageReferenceA);

                    var packageIdentityB = new PackageIdentity("packageB", NuGetVersion.Parse("2.0.0"));
                    var packageReferenceB = new PackageReference(packageIdentityB, NuGetFramework.Parse("portable-net45+win8"));

                    var packageIdentityC = new PackageIdentity("packageC", NuGetVersion.Parse("1.0.1"));
                    var packageReferenceC = new PackageReference(packageIdentityC, NuGetFramework.Parse("net45"));

                    // Assert
                    Assert.Throws<PackagesConfigWriterException>(() => writer.UpdatePackageEntry(packageReferenceB, packageReferenceC));
                }
            }
        }

        [Fact]
        public void PackagesConfigWriter_Remove()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                // Act
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                    writer.AddPackageEntry("packageB", NuGetVersion.Parse("2.0.0"), NuGetFramework.Parse("portable-net45+win8"));

                    writer.AddPackageEntry("packageA", NuGetVersion.Parse("1.0.1"), NuGetFramework.Parse("net45"));

                    writer.RemovePackageEntry("packageB", NuGetVersion.Parse("2.0.0"), NuGetFramework.Parse("portable-net45+win8"));
                }

                stream.Seek(0, SeekOrigin.Begin);

                var xml = XDocument.Load(stream);

                // Assert
                Assert.Equal("utf-8", xml.Declaration.Encoding);

                var reader = new PackagesConfigReader(xml);

                var packages = reader.GetPackages().ToArray();
                Assert.Equal("1", packages.Count().ToString());
                Assert.Equal("packageA", packages[0].PackageIdentity.Id);
                Assert.Equal("1.0.1", packages[0].PackageIdentity.Version.ToNormalizedString());
                Assert.Equal("net45", packages[0].TargetFramework.GetShortFolderName());
            }
        }

        [Fact]
        public void PackagesConfigWriter_RemoveError()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                // Act
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                    writer.AddPackageEntry("packageB", NuGetVersion.Parse("2.0.0"), NuGetFramework.Parse("portable-net45+win8"));

                    // Assert
                    Assert.Throws<PackagesConfigWriterException>(() => writer.RemovePackageEntry("packageA", NuGetVersion.Parse("2.0.1"), NuGetFramework.Parse("net4")));
                }
            }
        }

        [Fact]
        public void PackagesConfigWriter_Duplicate()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                // Act
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                    writer.AddPackageEntry("packageA", NuGetVersion.Parse("1.0.1"), NuGetFramework.Parse("net45"));

                    // Assert
                    Assert.Throws<PackagesConfigWriterException>(() => writer.AddPackageEntry("packageA", NuGetVersion.Parse("2.0.1"), NuGetFramework.Parse("net4")));
                }
            }
        }

        [Fact]
        public void PackagesConfigWriter_CreateEmptyFile()
        {
            // Arrange
            using (var testFolder = TestDirectory.Create())
            {
                var path = Path.Combine(testFolder + "packages.config");

                // Act
                using (var stream = File.Create(path))
                using (var writer = new PackagesConfigWriter(stream, true))
                {
                }

                // Assert
                var xml = XDocument.Load(path);

                Assert.NotNull(xml);
            }
        }

        [Fact]
        public void PackagesConfigWriter_OpenExistingFile()
        {
            // Arrange
            using (var folderPath = TestDirectory.Create())
            {
                var filePath = Path.Combine(folderPath, "packages.config");

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var fileWriter = new StreamWriter(fileStream))
                    {
                        var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <packages>
            <package id = ""packageA"" version = ""1.0.0"" targetFramework = ""win81"" userInstalled = ""true"" protocolVersion = ""V2"" />
            <package id = ""Microsoft.ApplicationInsights.PersistenceChannel"" version = ""0.14.3-build00177"" targetFramework = ""win81"" />
            <package id = ""Microsoft.ApplicationInsights.WindowsApps"" version = ""0.14.3-build00177"" targetFramework = ""win81"" />
            <package id = ""Microsoft.Diagnostics.Tracing.EventSource.Redist"" version = ""1.1.16-beta"" targetFramework = ""win81"" />
            <package id = ""System.Numerics.Vectors"" version = ""4.0.0"" targetFramework = ""win81"" />
    </packages>";

                        fileWriter.Write(content);
                    }
                }

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    using (var writer = new PackagesConfigWriter(stream, false))
                    {
                        // Act
                        var packageIdentityA1 = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0"));
                        var packageReferenceA1 = new PackageReference(packageIdentityA1, NuGetFramework.Parse("win81"),
                            userInstalled: true, developmentDependency: false, requireReinstallation: false);

                        var packageIdentityA2 = new PackageIdentity("packageA", NuGetVersion.Parse("1.0.1"));
                        var packageReferenceA2 = new PackageReference(packageIdentityA2, NuGetFramework.Parse("net45"));

                        writer.UpdatePackageEntry(packageReferenceA1, packageReferenceA2);
                    }

                    // Assert
                    stream.Seek(0, SeekOrigin.Begin);

                    var xml = XDocument.Load(stream);

                    // Assert
                    Assert.Equal("utf-8", xml.Declaration.Encoding);

                    var packageNode = xml.Descendants(PackagesConfig.PackageNodeName).FirstOrDefault();
                    Assert.Equal(packageNode.ToString(), "<package id=\"packageA\" version=\"1.0.1\" targetFramework=\"net45\" userInstalled=\"true\" protocolVersion=\"V2\" />");
                }
            }
        }

        [Fact]
        public void PackagesConfigWriter_ThrowOnMalformedPackagesConfigXml()
        {
            // Arrange
            using (var folderPath = TestDirectory.Create())
            {
                var filePath = Path.Combine(folderPath, "packages.config");

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var fileWriter = new StreamWriter(fileStream))
                    {
                        var content = @"<?xml version=""1.0"" encoding=""utf-8""?>";

                        fileWriter.Write(content);
                    }
                }

                Assert.Throws<PackagesConfigWriterException>(() => new PackagesConfigWriter(filePath, false));
            }
        }

        [Fact]
        public void PackagesConfigWriter_ThrowOnMissingPackagesNode()
        {
            // Arrange
            using (var folderPath = TestDirectory.Create())
            {
                var filePath = Path.Combine(folderPath, "packages.config");

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var fileWriter = new StreamWriter(fileStream))
                    {
                        var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <configuration>
    </configuration> ";

                        fileWriter.Write(content);
                    }
                }

                using (var writer = new PackagesConfigWriter(filePath, false))
                {
                    // Assert
                    Assert.Throws<PackagesConfigWriterException>(() => writer.AddPackageEntry("packageA", NuGetVersion.Parse("2.0.1"), NuGetFramework.Parse("net4")));
                }
            }
        }

        [Fact]
        public void PackagesConfigWriter_NoOldPackagesConfigFileLeftOnDisk()
        {
            // Arrange
            using (var folderPath = TestDirectory.Create())
            {
                var directoryInfo = new DirectoryInfo(folderPath);
                var filePath = Path.Combine(folderPath, "packages.config");

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var fileWriter = new StreamWriter(fileStream))
                    {
                        var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <packages>
            <package id = ""packageA"" version = ""1.0.0"" targetFramework = ""win81"" userInstalled = ""true"" protocolVersion = ""V2"" />
    </packages>";

                        fileWriter.Write(content);
                    }
                }

                using (var writer = new PackagesConfigWriter(filePath, false))
                {
                    // Act
                    writer.AddPackageEntry("packageB", NuGetVersion.Parse("2.0.1"), NuGetFramework.Parse("net4"));
                }

                // Assert
                var packagesConfigFiles = directoryInfo.GetFiles().
                    Where(p => p.Name.ToLowerInvariant().Contains("packages.config"));

                Assert.Equal(1, packagesConfigFiles.Count());
            }
        }
    }
}
