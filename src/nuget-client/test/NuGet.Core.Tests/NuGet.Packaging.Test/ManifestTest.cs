// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    [UseCulture("en-US")] // We are asserting exception messages in English
    public class ManifestTest
    {
        [Fact]
        public void ManifestValidatesMetadata()
        {
            // Arrange
            var manifest = new Manifest(new ManifestMetadata
            {
                Id = String.Empty,
                Version = null,
                Authors = new[] { String.Empty },
                Description = null
            });

            // Act and Assert
            ExceptionAssert.Throws<Exception>(() => Manifest.Validate(manifest),
                "Id is required." + Environment.NewLine + "Version is required." + Environment.NewLine + "Authors is required." + Environment.NewLine + "Description is required.");
        }

        [Fact]
        public void ManifestValidatesMetadataUrlsIfEmpty()
        {
            // Arrange
            var manifestMetadata = new ManifestMetadata
            {
                Id = "Foobar",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "test-author" },
                Description = "desc"
            };

            manifestMetadata.SetIconUrl("");
            manifestMetadata.SetLicenseUrl("");
            manifestMetadata.SetProjectUrl("");

            var manifest = new Manifest(manifestMetadata);

            // Act and Assert
            ExceptionAssert.Throws<Exception>(() => Manifest.Validate(manifest),
                "LicenseUrl cannot be empty." + Environment.NewLine + "IconUrl cannot be empty." + Environment.NewLine + "ProjectUrl cannot be empty.");
        }

        [Fact]
        public void ManifestValidatesManifestFiles()
        {
            // Arrange
            var manifestMetadata = new ManifestMetadata
            {
                Id = "Foobar",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "test-author" },
                Description = "desc",
            };

            var manifest = new Manifest(manifestMetadata);

            manifest.Files.AddRange(new[] {
                new ManifestFile {
                    Source = "|",
                    Target = "<"
                },
                new ManifestFile {
                    Source = @"foo" + Path.DirectorySeparatorChar + "bar" + Path.DirectorySeparatorChar + "|>",
                    Target = "lib"
                },
                new ManifestFile {
                    Source = @"foo" + Path.DirectorySeparatorChar + "**" + Path.DirectorySeparatorChar + "*.cs",
                    Exclude = "Exclude|"
                }
            });

            // Act and Assert
            ExceptionAssert.Throws<Exception>(() => Manifest.Validate(manifest),
                "Source path '|' contains invalid characters." + Environment.NewLine + "Target path '<' contains invalid characters." + Environment.NewLine + "Source path 'foo" + Path.DirectorySeparatorChar + "bar" + Path.DirectorySeparatorChar + "|>' contains invalid characters." + Environment.NewLine + "Exclude path 'Exclude|' contains invalid characters.");
        }

        [Fact]
        public void ManifestEnsuresManifestReferencesDoNotContainInvalidCharacters()
        {
            // Arrange
            var manifestMetadata = new ManifestMetadata
            {
                Id = "Foobar",
                Version = NuGetVersion.Parse("1.0"),
                Authors = new[] { "test-author" },
                Description = "desc",
                PackageAssemblyReferences = new[] {
                        new PackageReferenceSet(new [] {
                            "Foo?.dll",
                            "Bar*.dll",
                            @"net40" + Path.DirectorySeparatorChar + "baz.dll"
                        }),
                        new PackageReferenceSet(".NETFramework, Version=4.0", new [] {
                            "wee?dd.dll"
                        })
                    }
            };

            var manifest = new Manifest(manifestMetadata);
            manifest.Files.AddRange(new[] {
                new ManifestFile { Source = "Foo.dll", Target = "lib" }
            });

            // Act and Assert
            ExceptionAssert.Throws<Exception>(() => Manifest.Validate(manifest),
                "Assembly reference 'Foo?.dll' contains invalid characters." + Environment.NewLine + "Assembly reference 'Bar*.dll' contains invalid characters." + Environment.NewLine + "Assembly reference 'net40" + Path.DirectorySeparatorChar + "baz.dll' contains invalid characters." + Environment.NewLine + "Assembly reference 'wee?dd.dll' contains invalid characters.");
        }

        [Fact]
        public void ReadFromReadsRequiredValues()
        {
            // Arrange
            var manifestStream = CreateManifest();
            var expectedManifest = new Manifest(
                new ManifestMetadata
                {
                    Id = "Test-Pack",
                    Version = NuGetVersion.Parse("1.0.0"),
                    Description = "Test description",
                    Authors = new[] { "NuGet Test" }
                }
            );

            // Act 
            var manifest = Manifest.ReadFrom(manifestStream, validateSchema: true);

            // Assert
            AssertManifest(expectedManifest, manifest);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("1")]
        [InlineData("1.0.2.4.2")]
        [InlineData("abc.2")]
        [InlineData("1.2-alpha")]
        public void InvalidReuiredMinVersionValueWillThrow(string minVersionValue)
        {
            // Arrange
            var manifestStream = CreateManifest(minClientVersion: minVersionValue);

            // Act && Assert
            ExceptionAssert.Throws<InvalidDataException>(
                () => Manifest.ReadFrom(manifestStream, validateSchema: true),
                "The 'minClientVersion' attribute in the package manifest has invalid value. It must be a valid version string.");
        }

        [Fact]
        public void EmptyReuiredMinVersionValueWillNotThrow()
        {
            // Arrange
            var manifestStream = CreateManifest(minClientVersion: "");

            // Act
            var manifest = Manifest.ReadFrom(manifestStream, validateSchema: true);

            // Assert
            Assert.Null(manifest.Metadata.MinClientVersion);
        }

        [Fact]
        public void ReadFromReadsAllMetadataValues()
        {
            var references = new List<PackageReferenceSet>
                    {
                        new PackageReferenceSet(
                            (NuGetFramework) null,
                            new [] { "Test.dll" }
                        ),
                        new PackageReferenceSet(
                            NuGetFramework.Parse("hello"),
                            new [] { "world.winmd" }
                        )
                    };

            // Arrange
            var manifestStream = CreateManifest(
                id: "Test-Pack2",
                version: "1.0.0-alpha",
                title: "blah",
                authors: "Outercurve",
                licenseUrl: "http://nuget.org/license", projectUrl: "http://nuget.org/project", iconUrl: "https://nuget.org/icon",
                requiresLicenseAcceptance: true, developmentDependency: true, description: "This is not a description",
                summary: "This is a summary", releaseNotes: "Release notes",
                copyright: "Copyright 2012", language: "fr-FR", tags: "Test Unit",
                dependencies: new[] { new PackageDependency("Test", VersionRange.Parse("1.2.0")) },
                assemblyReference: new[] { new FrameworkAssemblyReference("System.Data", new[] { NuGetFramework.Parse("4.0") }) },
                references: references,
                serviceable: true,
                packageTypes: new[]
                {
                    new PackageType("foo", new Version(2, 0, 0)),
                    new PackageType("bar", new Version(0, 0))
                },
                minClientVersion: "2.0.1.0"
            );

            var manifestMetadata = new ManifestMetadata
            {
                Id = "Test-Pack2",
                Version = NuGetVersion.Parse("1.0.0-alpha"),
                Description = "This is not a description",
                Authors = new[] { "Outercurve" },
                RequireLicenseAcceptance = true,
                DevelopmentDependency = true,
                Summary = "This is a summary",
                ReleaseNotes = "Release notes",
                Copyright = "Copyright 2012",
                Language = "fr-FR",
                Tags = "Test Unit",
                Serviceable = true,
                DependencyGroups = new[]
                                    {
                                        new PackageDependencyGroup(
                                            NuGetFramework.AnyFramework,
                                            new []
                                            {
                                                new PackageDependency("Test", VersionRange.Parse("1.2.0"))
                                            }
                                        )
                                    },
                FrameworkReferences = new[]
                                        {
                                            new FrameworkAssemblyReference("System.Data",
                                                new [] { NuGetFramework.Parse("4.0") }
                                            )
                                        },
                PackageAssemblyReferences = new[]
                                {
                                    new PackageReferenceSet(
                                        (NuGetFramework) null,
                                        new [] { "Test.dll" }
                                    ),
                                    new PackageReferenceSet(
                                        NuGetFramework.Parse("hello"),
                                        new [] { "world.winmd" }
                                    )
                                },
                PackageTypes = new[]
                {
                    new PackageType("foo", new Version(2, 0, 0)),
                    new PackageType("bar", new Version(0, 0))
                },
                MinClientVersionString = "2.0.1.0",
            };

            manifestMetadata.SetLicenseUrl("http://nuget.org/license");
            manifestMetadata.SetProjectUrl("http://nuget.org/project");
            manifestMetadata.SetIconUrl("https://nuget.org/icon");

            var expectedManifest = new Manifest(manifestMetadata);

            // Act 
            var manifest = Manifest.ReadFrom(manifestStream, validateSchema: true);

            // Assert
            AssertManifest(expectedManifest, manifest);
        }

        [Fact]
        public void ReadFromReadsFilesAndExpandsDelimitedFileList()
        {
            // Arrange
            var manifestStream = CreateManifest(files: new[] {
                            new ManifestFile { Source = "Foo.cs", Target = "src" },
                            new ManifestFile { Source = @"**" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "*.dll;**" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "*.exe", Target = @"lib" + Path.DirectorySeparatorChar + "net40", Exclude = @"**" + Path.DirectorySeparatorChar + "*Test*" }
                    });

            var expectedManifest = new Manifest(
                new ManifestMetadata { Id = "Test-Pack", Version = NuGetVersion.Parse("1.0.0"), Description = "Test description", Authors = new[] { "NuGet Test" } },
                new List<ManifestFile> {
                            new ManifestFile { Source = "Foo.cs", Target = "src" },
                            new ManifestFile { Source = @"**" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "*.dll", Target = @"lib" + Path.DirectorySeparatorChar + "net40", Exclude = @"**" + Path.DirectorySeparatorChar + "*Test*" },
                            new ManifestFile { Source = @"**" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "*.exe", Target = @"lib" + Path.DirectorySeparatorChar + "net40", Exclude = @"**" + Path.DirectorySeparatorChar + "*Test*" },
                        }
            );

            // Act 
            var manifest = Manifest.ReadFrom(manifestStream, validateSchema: true);

            // Assert
            AssertManifest(expectedManifest, manifest);
        }

        [Fact]
        public void ReadFromDoesNotThrowIfValidateSchemaIsFalse()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata hello=""world"">
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>

    <extra>This element is not defined in schema.</extra>
  </metadata>
  <clark>meko</clark>
  <files>
      <file src=""my.txt"" destination=""outdir"" />
  </files>
</package>";
            // Act
            var manifest = Manifest.ReadFrom(content.AsStream(), validateSchema: false);

            // Assert
            Assert.Equal("A", manifest.Metadata.Id);
            Assert.Equal(NuGetVersion.Parse("1.0"), manifest.Metadata.Version);
            Assert.Equal(new[] { "Luan" }, manifest.Metadata.Authors);
            Assert.False(manifest.Metadata.RequireLicenseAcceptance);
            Assert.False(manifest.Metadata.DevelopmentDependency);
            Assert.Equal("Descriptions", manifest.Metadata.Description);
        }

        [Fact]
        public void ReadDevelopmentDependency()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata hello=""world"">
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <developmentDependency>true</developmentDependency>
    <description>Descriptions</description>

    <extra>This element is not defined in schema.</extra>
  </metadata>
  <clark>meko</clark>
  <files>
      <file src=""my.txt"" destination=""outdir"" />
  </files>
</package>";
            // Act
            var manifest = Manifest.ReadFrom(content.AsStream(), validateSchema: false);

            // Assert
            Assert.Equal("A", manifest.Metadata.Id);
            Assert.Equal(NuGetVersion.Parse("1.0"), manifest.Metadata.Version);
            Assert.Equal(new[] { "Luan" }, manifest.Metadata.Authors);
            Assert.False(manifest.Metadata.RequireLicenseAcceptance);
            Assert.True(manifest.Metadata.DevelopmentDependency);
            Assert.Equal("Descriptions", manifest.Metadata.Description);
        }

        [Fact]
        public void ReadServiceable()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata hello=""world"">
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <developmentDependency>true</developmentDependency>
    <description>Descriptions</description>
    <serviceable>true</serviceable>
    <extra>This element is not defined in schema.</extra>
  </metadata>
  <clark>meko</clark>
  <files>
      <file src=""my.txt"" destination=""outdir"" />
  </files>
</package>";
            // Act
            var manifest = Manifest.ReadFrom(content.AsStream(), validateSchema: false);

            // Assert
            Assert.Equal("A", manifest.Metadata.Id);
            Assert.Equal(NuGetVersion.Parse("1.0"), manifest.Metadata.Version);
            Assert.Equal(new[] { "Luan" }, manifest.Metadata.Authors);
            Assert.False(manifest.Metadata.RequireLicenseAcceptance);
            Assert.True(manifest.Metadata.DevelopmentDependency);
            Assert.True(manifest.Metadata.Serviceable);
            Assert.Equal("Descriptions", manifest.Metadata.Description);
        }

        [Fact]
        public void ReadPackageType()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <description>Descriptions</description>
    <packageTypes>
      <packageType name=""foo"" version=""2.0.0"" />
      <packageType name=""bar"" />
    </packageTypes>
  </metadata>
</package>";
            // Act
            var manifest = Manifest.ReadFrom(content.AsStream(), validateSchema: false);

            // Assert
            Assert.Equal("A", manifest.Metadata.Id);
            Assert.Equal(NuGetVersion.Parse("1.0"), manifest.Metadata.Version);
            Assert.Equal(2, manifest.Metadata.PackageTypes.Count());
            Assert.Equal("foo", manifest.Metadata.PackageTypes.ElementAt(0).Name);
            Assert.Equal(new Version(2, 0, 0), manifest.Metadata.PackageTypes.ElementAt(0).Version);
            Assert.Equal("bar", manifest.Metadata.PackageTypes.ElementAt(1).Name);
            Assert.Equal(new Version(0, 0), manifest.Metadata.PackageTypes.ElementAt(1).Version);
        }

        [Fact]
        public void RejectsInvalidPackageTypeWhenValidatingSchema()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <description>Descriptions</description>
    <packageTypes>
      <packageType version=""2.0.0"" />
    </packageTypes>
  </metadata>
</package>";
            // Act & Assert
#if !IS_CORECLR
            var exception = Assert.Throws<InvalidOperationException>(
                () => Manifest.ReadFrom(content.AsStream(), validateSchema: true));
            Assert.Equal(
                "The required attribute 'name' is missing. " +
                "This validation error occurred in a 'packageType' element.",
                exception.Message);
#else
            var exception = Assert.Throws<PackagingException>(
                () => Manifest.ReadFrom(content.AsStream(), validateSchema: true));
            Assert.Equal("Nuspec file contains a package type that is missing the name attribute.", exception.Message);
#endif
        }

        [Fact]
        public void RejectsInvalidCombinationOfLicenseUrlAndLicense()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <description>Descriptions</description>
    <license type=""expression"">MIT</license>
    <licenseUrl>https://www.mycoolproject.org/license.txt</licenseUrl>
  </metadata>
</package>";
            // Act & Assert
            var exception = Assert.Throws<Exception>(
                () => Manifest.ReadFrom(content.AsStream(), validateSchema: false));
            Assert.Equal("The licenseUrl and license elements cannot be used together.", exception.Message);
        }

        [Fact]
        public void RejectsInvalidPackageTypeWhenNotValidatingSchema()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <description>Descriptions</description>
    <packageTypes>
      <packageType version=""2.0.0"" />
    </packageTypes>
  </metadata>
</package>";
            // Act & Assert
            var exception = Assert.Throws<PackagingException>(
                () => Manifest.ReadFrom(content.AsStream(), validateSchema: false));
            Assert.Equal("Nuspec file contains a package type that is missing the name attribute.", exception.Message);
        }

        [Fact]
        public void ReadFromThrowIfValidateSchemaIsTrue()
        {
            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
#endif

            // Act && Assert
#if !IS_CORECLR
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata hello=""world"">
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>";

            ExceptionAssert.Throws<InvalidOperationException>(
                () => Manifest.ReadFrom(content.AsStream(), validateSchema: true),
                "The 'hello' attribute is not declared.");
#else
            // REVIEW: Not thrown in CoreCLR due to no XmlSchema validation
#endif
        }

        [Fact]
        public void ReadFromThrowIfReferenceGroupIsEmptyAndValidateSchemaIsTrue()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <references>
        <group>
        </group>
    </references>
  </metadata>
</package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
#endif

            // Act && Assert
#if !IS_CORECLR
            ExceptionAssert.Throws<InvalidOperationException>(
                () => Manifest.ReadFrom(content.AsStream(), validateSchema: true),
                "The element 'group' in namespace 'http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd' has incomplete content. " +
                "List of possible elements expected: 'reference' in namespace 'http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd'. " +
                "This validation error occurred in a 'group' element.");
#else
            ExceptionAssert.Throws<InvalidDataException>(
                () => Manifest.ReadFrom(content.AsStream(), validateSchema: true),
                @"The element package\metadata\references\group must contain at least one <reference> child element.");
#endif
        }

        [Fact]
        public void ReadFromThrowIfReferenceGroupIsEmptyAndValidateSchemaIsFalse()
        {
            // Arrange
            string content = @"<?xml version=""1.0""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0</version>
    <authors>Luan</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <references>
        <group>
        </group>
    </references>
  </metadata>
</package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
#endif

            // Act && Assert
            ExceptionAssert.Throws<InvalidDataException>(
                () => Manifest.ReadFrom(content.AsStream(), validateSchema: false),
                @"The element package\metadata\references\group must contain at least one <reference> child element.");
        }

        [Fact]
        public void ManifestGroupDependencySetsByTargetFrameworkAndPutNullFrameworkFirst()
        {
            // Arrange
            var manifest = new Manifest(
                new ManifestMetadata
                {
                    Id = "Foobar",
                    Version = NuGetVersion.Parse("1.0"),
                    Authors = new[] { "test-author" },
                    Description = "desc",
                    DependencyGroups = new[] {
                                    new PackageDependencyGroup(
                                        NuGetFramework.Parse(".NETFramework40"),
                                        new [] {
                                            new PackageDependency("B")
                                        }
                                    ),

                                    new PackageDependencyGroup(
                                        NuGetFramework.AnyFramework,
                                        new [] {
                                            new PackageDependency("A")
                                        }
                                    ),

                                    new PackageDependencyGroup(
                                        NuGetFramework.AnyFramework,
                                        new [] {
                                            new PackageDependency("C")
                                        }
                                    ),

                                    new PackageDependencyGroup(
                                        NuGetFramework.Parse("Silverlight35"),
                                        new [] {
                                            new PackageDependency("D")
                                        }
                                    ),

                                    new PackageDependencyGroup(
                                        NuGetFramework.Parse("net40"),
                                        new [] {
                                            new PackageDependency("E")
                                        }
                                    ),

                                    new PackageDependencyGroup(
                                        NuGetFramework.Parse("sl35"),
                                        new [] {
                                            new PackageDependency("F")
                                        }
                                    ),

                                    new PackageDependencyGroup(
                                        NuGetFramework.Parse("winrt45"),
                                        new List<PackageDependency>()
                                    ),
                            }
                }
            );

            // Act
            var dependencyGroups = manifest.Metadata.DependencyGroups.ToList();

            // Assert
            Assert.Equal(4, dependencyGroups.Count);

            Assert.Equal(NuGetFramework.AnyFramework, dependencyGroups[0].TargetFramework);
            Assert.Equal(2, dependencyGroups[0].Packages.Count());
            Assert.Equal("A", dependencyGroups[0].Packages.First().Id);
            Assert.Equal("C", dependencyGroups[0].Packages.Last().Id);

            Assert.Equal(NuGetFramework.Parse(".NETFramework, Version=4.0"), dependencyGroups[1].TargetFramework);
            Assert.Equal(2, dependencyGroups[1].Packages.Count());
            Assert.Equal("B", dependencyGroups[1].Packages.First().Id);
            Assert.Equal("E", dependencyGroups[1].Packages.Last().Id);

            Assert.Equal(NuGetFramework.Parse("Silverlight, Version=3.5"), dependencyGroups[2].TargetFramework);
            Assert.Equal(2, dependencyGroups[2].Packages.Count());
            Assert.Equal("D", dependencyGroups[2].Packages.First().Id);
            Assert.Equal("F", dependencyGroups[2].Packages.Last().Id);

            Assert.Equal(NuGetFramework.Parse("WinRT, Version=4.5"), dependencyGroups[3].TargetFramework);
            Assert.Equal(0, dependencyGroups[3].Packages.Count());
        }

        // Test that manifest is serialized correctly.
        [Fact]
        public void ManifestSerialization()
        {
            var manifestMetadata = new ManifestMetadata()
            {
                Id = "id",
                Authors = new[] { "author" },
                Version = NuGetVersion.Parse("1.0.0"),
                Description = "description"
            };

            var manifest = new Manifest(manifestMetadata, new List<ManifestFile>());
            var file = new ManifestFile();
            file.Source = "file_source";
            file.Target = "file_target";
            manifest.Files.Add(file);

            var memoryStream = new MemoryStream();
            manifest.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // read the serialized manifest.
            var newManifest = Manifest.ReadFrom(memoryStream, validateSchema: true);
            Assert.Equal(newManifest.Metadata.Id, manifest.Metadata.Id);
            Assert.Equal(newManifest.Metadata.Authors, manifest.Metadata.Authors);
            Assert.Equal(newManifest.Metadata.Description, manifest.Metadata.Description);
            Assert.Equal(newManifest.Files.Count, manifest.Files.Count);

            var actualFiles = manifest.Files.ToList();
            var expectedFiles = newManifest.Files.ToList();
            for (int i = 0; i < expectedFiles.Count; ++i)
            {
                AssertFile(expectedFiles[i], actualFiles[i]);
            }
        }

        private void AssertManifest(Manifest expected, Manifest actual)
        {
            Assert.Equal(expected.Metadata.Id, actual.Metadata.Id);
            Assert.Equal(expected.Metadata.Version, actual.Metadata.Version);
            Assert.Equal(expected.Metadata.Description, actual.Metadata.Description);
            Assert.Equal(expected.Metadata.Authors, actual.Metadata.Authors);
            Assert.Equal(expected.Metadata.Copyright, actual.Metadata.Copyright);
            Assert.Equal(expected.Metadata.IconUrl, actual.Metadata.IconUrl);
            Assert.Equal(expected.Metadata.Language, actual.Metadata.Language);
            Assert.Equal(expected.Metadata.LicenseUrl, actual.Metadata.LicenseUrl);
            Assert.Equal(expected.Metadata.Owners, actual.Metadata.Owners);
            Assert.Equal(expected.Metadata.ProjectUrl, actual.Metadata.ProjectUrl);
            Assert.Equal(expected.Metadata.ReleaseNotes, actual.Metadata.ReleaseNotes);
            Assert.Equal(expected.Metadata.RequireLicenseAcceptance, actual.Metadata.RequireLicenseAcceptance);
            Assert.Equal(expected.Metadata.DevelopmentDependency, actual.Metadata.DevelopmentDependency);
            Assert.Equal(expected.Metadata.Summary, actual.Metadata.Summary);
            Assert.Equal(expected.Metadata.Tags, actual.Metadata.Tags);
            Assert.Equal(expected.Metadata.Serviceable, actual.Metadata.Serviceable);
            Assert.Equal(expected.Metadata.MinClientVersion, actual.Metadata.MinClientVersion);

            if (expected.Metadata.DependencyGroups != null)
            {
                var actualDependencyGroups = actual.Metadata.DependencyGroups.ToList();
                var expectedDependencyGroups = expected.Metadata.DependencyGroups.ToList();

                for (int i = 0; i < expectedDependencyGroups.Count; i++)
                {
                    AssertDependencyGroup(expectedDependencyGroups[i], actualDependencyGroups[i]);
                }
            }
            if (expected.Metadata.FrameworkReferences != null)
            {
                var actualFrameworkReferences = actual.Metadata.FrameworkReferences.ToList();
                var expectedFrameworkReferences = expected.Metadata.FrameworkReferences.ToList();

                for (int i = 0; i < expectedFrameworkReferences.Count; i++)
                {
                    AssertFrameworkAssemblies(expectedFrameworkReferences[i], actualFrameworkReferences[i]);
                }
            }
            if (expected.Metadata.PackageAssemblyReferences != null)
            {
                var actualAssemblyReferences = actual.Metadata.PackageAssemblyReferences.ToList();
                var expectedAssemblyReferences = expected.Metadata.PackageAssemblyReferences.ToList();

                for (int i = 0; i < expectedAssemblyReferences.Count; i++)
                {
                    AssertReference(expectedAssemblyReferences[i], actualAssemblyReferences[i]);
                }
            }
            if (expected.Files != null)
            {
                var actualFiles = actual.Files.ToList();
                var expectedFiles = expected.Files.ToList();

                for (int i = 0; i < expectedFiles.Count; i++)
                {
                    AssertFile(expectedFiles[i], actualFiles[i]);
                }
            }

            if (expected.Metadata.PackageTypes != null)
            {
                var actualPackageTypes = actual.Metadata.PackageTypes.ToList();
                var expectedPackageTypes = expected.Metadata.PackageTypes.ToList();

                Assert.Equal(expectedPackageTypes.Count, actualPackageTypes.Count);

                for (int i = 0; i < expectedPackageTypes.Count; i++)
                {
                    Assert.Equal(expectedPackageTypes[i], actualPackageTypes[i]);
                }
            }
        }

        private void AssertFile(ManifestFile expected, ManifestFile actual)
        {
            Assert.Equal(expected.Source, actual.Source);
            Assert.Equal(expected.Target, actual.Target);
            Assert.Equal(expected.Exclude, actual.Exclude);
        }

        private static void AssertDependencyGroup(PackageDependencyGroup expected, PackageDependencyGroup actual)
        {
            Assert.Equal(expected.TargetFramework, actual.TargetFramework);

            var actualDependencies = actual.Packages.ToList();
            var expectedDependencies = expected.Packages.ToList();

            Assert.Equal(expectedDependencies.Count, actualDependencies.Count);
            for (int i = 0; i < expectedDependencies.Count; i++)
            {
                AssertDependency(expectedDependencies[i], actualDependencies[i]);
            }
        }

        private static void AssertDependency(PackageDependency expected, PackageDependency actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.VersionRange, actual.VersionRange);
        }

        private static void AssertFrameworkAssemblies(FrameworkAssemblyReference expected, FrameworkAssemblyReference actual)
        {
            Assert.Equal(expected.AssemblyName, actual.AssemblyName);

            var actualSupportedFrameworks = expected.SupportedFrameworks.ToList();
            var expectedSupportedFrameworks = expected.SupportedFrameworks.ToList();
            for (int i = 0; i < expectedSupportedFrameworks.Count; i++)
            {
                Assert.Equal(expectedSupportedFrameworks[i], actualSupportedFrameworks[i]);
            }
        }

        private static void AssertReference(string expected, string actual)
        {
            Assert.Equal(expected, actual);
        }

        private static void AssertReference(PackageReferenceSet expected, PackageReferenceSet actual)
        {
            Assert.Equal(expected.TargetFramework, actual.TargetFramework);

            var actualReferences = actual.References.ToList();
            var expectedReferences = expected.References.ToList();

            Assert.Equal(expected.References.Count, actualReferences.Count);
            for (int i = 0; i < expectedReferences.Count; i++)
            {
                AssertReference(expectedReferences[i], actualReferences[i]);
            }
        }

        public static Stream CreateManifest(string id = "Test-Pack",
                                            string version = "1.0.0",
                                            string title = null,
                                            string authors = "NuGet Test",
                                            string owners = null,
                                            string licenseUrl = null,
                                            string projectUrl = null,
                                            string iconUrl = null,
                                            bool? requiresLicenseAcceptance = null,
                                            bool? developmentDependency = null,
                                            bool serviceable = false,
                                            string description = "Test description",
                                            string summary = null,
                                            string releaseNotes = null,
                                            string copyright = null,
                                            string language = null,
                                            string tags = null,
                                            IEnumerable<PackageDependency> dependencies = null,
                                            IEnumerable<FrameworkAssemblyReference> assemblyReference = null,
                                            IEnumerable<PackageReferenceSet> references = null,
                                            IEnumerable<ManifestFile> files = null,
                                            IEnumerable<PackageType> packageTypes = null,
                                            string minClientVersion = null)
        {
            var document = new XDocument(new XElement("package"));
            var metadata = new XElement("metadata", new XElement("id", id), new XElement("version", version),
                                                    new XElement("description", description), new XElement("authors", authors));

            if (minClientVersion != null)
            {
                metadata.Add(new XAttribute("minClientVersion", minClientVersion));
            }

            document.Root.Add(metadata);

            if (title != null)
            {
                metadata.Add(new XElement("title", title));
            }
            if (owners != null)
            {
                metadata.Add(new XElement("owners", owners));
            }
            if (licenseUrl != null)
            {
                metadata.Add(new XElement("licenseUrl", licenseUrl));
            }
            if (projectUrl != null)
            {
                metadata.Add(new XElement("projectUrl", projectUrl));
            }
            if (iconUrl != null)
            {
                metadata.Add(new XElement("iconUrl", iconUrl));
            }
            if (requiresLicenseAcceptance != null)
            {
                metadata.Add(new XElement("requireLicenseAcceptance", requiresLicenseAcceptance.ToString().ToLowerInvariant()));
            }
            if (developmentDependency != null)
            {
                metadata.Add(new XElement("developmentDependency", developmentDependency.ToString().ToLowerInvariant()));
            }
            if (summary != null)
            {
                metadata.Add(new XElement("summary", summary));
            }
            if (releaseNotes != null)
            {
                metadata.Add(new XElement("releaseNotes", releaseNotes));
            }
            if (copyright != null)
            {
                metadata.Add(new XElement("copyright", copyright));
            }
            if (language != null)
            {
                metadata.Add(new XElement("language", language));
            }
            if (tags != null)
            {
                metadata.Add(new XElement("tags", tags));
            }
            if (serviceable)
            {
                metadata.Add(new XElement("serviceable", true));
            }
            if (dependencies != null)
            {
                metadata.Add(new XElement("dependencies",
                    dependencies.Select(d => new XElement("dependency", new XAttribute("id", d.Id), new XAttribute("version", d.VersionRange)))));
            }
            if (assemblyReference != null)
            {
                metadata.Add(new XElement("frameworkAssemblies",
                    assemblyReference.Select(r => new XElement("frameworkAssembly",
                        new XAttribute("assemblyName", r.AssemblyName),
                        new XAttribute("targetFramework", r.SupportedFrameworks.FirstOrDefault())))));
            }

            if (references != null)
            {
                if (references.Any(r => r.TargetFramework != null))
                {
                    metadata.Add(new XElement("references",
                        references.Select(r => new XElement("group",
                            r.TargetFramework != null ? new XAttribute("targetFramework", r.TargetFramework) : null,
                            r.References.Select(f => new XElement("reference", new XAttribute("file", f))))
                        )));
                }
                else
                {
                    metadata.Add(new XElement("references", references.SelectMany(r => r.References).Select(r => new XElement("reference", new XAttribute("file", r)))));
                }
            }

            if (files != null)
            {
                var filesNode = new XElement("files");
                foreach (var file in files)
                {
                    var fileNode = new XElement("file", new XAttribute("src", file.Source));
                    if (file.Target != null)
                    {
                        fileNode.Add(new XAttribute("target", file.Target));
                    }
                    if (file.Exclude != null)
                    {
                        fileNode.Add(new XAttribute("exclude", file.Exclude));
                    }

                    filesNode.Add(fileNode);
                }
                document.Root.Add(filesNode);
            }

            if (packageTypes != null)
            {
                var packageTypesNode = new XElement(NuspecUtility.PackageTypes);

                foreach (var packageType in packageTypes)
                {
                    var packageTypeNode = new XElement(NuspecUtility.PackageType);
                    packageTypeNode.SetAttributeValue(NuspecUtility.Name, packageType.Name);
                    if (packageType.Version != PackageType.EmptyVersion)
                    {
                        packageTypeNode.SetAttributeValue(NuspecUtility.Version, packageType.Version);
                    }

                    packageTypesNode.Add(packageTypeNode);
                }

                metadata.Add(packageTypesNode);
            }

            var stream = new MemoryStream();
            document.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
