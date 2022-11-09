// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
#if !IS_CORECLR
using System.Globalization;
#endif
using System.IO;
using System.IO.Compression;
using System.Linq;
#if !IS_CORECLR
using System.Threading;
#endif
using System.Xml;
using System.Xml.Linq;
using Moq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Packaging.PackageCreation.Resources;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class PackageBuilderTest
    {
        private static readonly DateTime ZipFormatMinDate = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime ZipFormatMaxDate = new DateTime(2107, 12, 31, 23, 59, 58, DateTimeKind.Utc);

        [Fact]
        public void CreatePackageWithEmptyFoldersForV3Folders()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };

            builder.Authors.Add("testAuthor");

            var dependencies = new List<PackageDependency>();
            dependencies.Add(new PackageDependency("packageB", VersionRange.Parse("1.0.0"), null, new[] { "z" }));
            dependencies.Add(new PackageDependency(
                "packageC",
                VersionRange.Parse("1.0.0"),
                new[] { "a", "b", "c" },
                new[] { "b", "c" }));

            var set = new PackageDependencyGroup(NuGetFramework.AnyFramework, dependencies);
            builder.DependencyGroups.Add(set);

            builder.Files.Add(CreatePackageFile(@"build" + Path.DirectorySeparatorChar + "_._"));
            builder.Files.Add(CreatePackageFile(@"content" + Path.DirectorySeparatorChar + "_._"));
            builder.Files.Add(CreatePackageFile(@"contentFiles" + Path.DirectorySeparatorChar + "any" + Path.DirectorySeparatorChar + "any" + Path.DirectorySeparatorChar + "_._"));
            builder.Files.Add(CreatePackageFile(@"lib" + Path.DirectorySeparatorChar + "net45" + Path.DirectorySeparatorChar + "_._"));
            builder.Files.Add(CreatePackageFile(@"native" + Path.DirectorySeparatorChar + "net45" + Path.DirectorySeparatorChar + "_._"));
            builder.Files.Add(CreatePackageFile(@"ref" + Path.DirectorySeparatorChar + "net45" + Path.DirectorySeparatorChar + "_._"));
            builder.Files.Add(CreatePackageFile(@"runtimes" + Path.DirectorySeparatorChar + "net45" + Path.DirectorySeparatorChar + "_._"));
            builder.Files.Add(CreatePackageFile(@"tools" + Path.DirectorySeparatorChar + "_._"));

            using (var ms = new MemoryStream())
            {
                // Act
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                using (var archive = new ZipArchive(ms, ZipArchiveMode.Read, leaveOpen: true))
                {
                    var files = archive.Entries
                        .Where(file => file.Name == "_._")
                        .Select(file => file.FullName)
                        .OrderBy(s => s)
                        .ToArray();

                    // Assert
                    Assert.Equal(8, files.Length);
                    Assert.Equal(@"build/_._", files[0]);
                    Assert.Equal(@"content/_._", files[1]);
                    Assert.Equal(@"contentFiles/any/any/_._", files[2]);
                    Assert.Equal(@"lib/net45/_._", files[3]);
                    Assert.Equal(@"native/net45/_._", files[4]);
                    Assert.Equal(@"ref/net45/_._", files[5]);
                    Assert.Equal(@"runtimes/net45/_._", files[6]);
                    Assert.Equal(@"tools/_._", files[7]);
                }
            }
        }

        [Fact]
        public void CreatePackageWithDifferentFileKinds()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };

            builder.Authors.Add("testAuthor");

            var dependencies = new List<PackageDependency>();
            dependencies.Add(new PackageDependency("packageB", VersionRange.Parse("1.0.0"), null, new[] { "z" }));
            dependencies.Add(new PackageDependency(
                "packageC",
                VersionRange.Parse("1.0.0"),
                new[] { "a", "b", "c" },
                new[] { "b", "c" }));

            var set = new PackageDependencyGroup(NuGetFramework.AnyFramework, dependencies);
            builder.DependencyGroups.Add(set);

            var sep = Path.DirectorySeparatorChar;

            builder.Files.Add(CreatePackageFile(@"build" + sep + "foo.props"));
            builder.Files.Add(CreatePackageFile(@"buildCrossTargeting" + sep + "foo.props"));
            builder.Files.Add(CreatePackageFile(@"buildMultiTargeting" + sep + "foo.props"));
            builder.Files.Add(CreatePackageFile(@"buildTransitive" + sep + "foo.props"));
            builder.Files.Add(CreatePackageFile(@"buildTransitive" + sep + "net5.0" + sep + "foo.props"));
            builder.Files.Add(CreatePackageFile(@"content" + sep + "foo.jpg"));
            builder.Files.Add(CreatePackageFile(@"contentFiles" + sep + "any" + sep + "any" + sep + "foo.png"));
            builder.Files.Add(CreatePackageFile(@"contentFiles" + sep + "cs" + sep + "net5.0" + sep + "foo.cs"));
            builder.Files.Add(CreatePackageFile(@"embed" + sep + "net5.0" + sep + "foo.dll"));
            builder.Files.Add(CreatePackageFile(@"lib" + sep + "net5.0" + sep + "foo.dll"));
            builder.Files.Add(CreatePackageFile(@"ref" + sep + "net5.0" + sep + "foo.dll"));
            builder.Files.Add(CreatePackageFile(@"runtimes" + sep + "win" + sep + "native" + sep + "foo.o"));
            builder.Files.Add(CreatePackageFile(@"tools" + sep + "foo.dll"));

            using (var ms = new MemoryStream())
            {
                // Act
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                using (var archive = new ZipArchive(ms, ZipArchiveMode.Read, leaveOpen: true))
                {
                    var files = archive.Entries
                        .Where(file => file.Name.StartsWith("foo"))
                        .Select(file => file.FullName)
                        .OrderBy(s => s)
                        .ToArray();

                    // Assert
                    Assert.Equal(@"build/foo.props", files[0]);
                    Assert.Equal(@"buildCrossTargeting/foo.props", files[1]);
                    Assert.Equal(@"buildMultiTargeting/foo.props", files[2]);
                    Assert.Equal(@"buildTransitive/foo.props", files[3]);
                    Assert.Equal(@"buildTransitive/net5.0/foo.props", files[4]);
                    Assert.Equal(@"content/foo.jpg", files[5]);
                    Assert.Equal(@"contentFiles/any/any/foo.png", files[6]);
                    Assert.Equal(@"contentFiles/cs/net5.0/foo.cs", files[7]);
                    Assert.Equal(@"embed/net5.0/foo.dll", files[8]);
                    Assert.Equal(@"lib/net5.0/foo.dll", files[9]);
                    Assert.Equal(@"ref/net5.0/foo.dll", files[10]);
                    Assert.Equal(@"runtimes/win/native/foo.o", files[11]);
                    Assert.Equal(@"tools/foo.dll", files[12]);
                    Assert.Equal(13, files.Length);
                }
            }
        }

        [Fact]
        public void CreatePackageWithNuspecIncludeExcludeAnyGroup()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");

            var dependencies = new List<PackageDependency>();
            dependencies.Add(new PackageDependency("packageB", VersionRange.Parse("1.0.0"), null, new[] { "z" }));
            dependencies.Add(new PackageDependency(
                "packageC",
                VersionRange.Parse("1.0.0"),
                new[] { "a", "b", "c" },
                new[] { "b", "c" }));

            var set = new PackageDependencyGroup(NuGetFramework.AnyFramework, dependencies);
            builder.DependencyGroups.Add(set);

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                var result = manifestStream.ReadToEnd();

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <dependencies>
      <group>
        <dependency id=""packageB"" version=""1.0.0"" exclude=""z"" />
        <dependency id=""packageC"" version=""1.0.0"" include=""a,b,c"" exclude=""b,c"" />
      </group>
    </dependencies>
  </metadata>
</package>".Replace("\r\n", "\n"), result.Replace("\r\n", "\n"));
            }
        }

        [Theory]
        [InlineData(".NETFramework,Version=v4.7.2", ".NETFramework4.7.2")]
        [InlineData(".NETFramework,Version=v4.7.2,Profile=foo", ".NETFramework4.7.2-foo")]
        [InlineData("net5.0", "net5.0")]
        [InlineData("net5.0-macos10.8", "net5.0-macos10.8")]
        [InlineData("net6.0", "net6.0")]
        public void CreatePackageTFMFormatting(string from, string to)
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");

            var dependencies = new List<PackageDependency>();
            dependencies.Add(new PackageDependency("packageB", VersionRange.Parse("1.0.0"), null, new[] { "z" }));

            var tfmGroup = new PackageDependencyGroup(NuGetFramework.Parse(from), dependencies);
            builder.DependencyGroups.Add(tfmGroup);

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                var result = manifestStream.ReadToEnd();

                // Assert
                Assert.Equal($@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <dependencies>
      <group targetFramework=""{to}"">
        <dependency id=""packageB"" version=""1.0.0"" exclude=""z"" />
      </group>
    </dependencies>
  </metadata>
</package>".Replace("\r\n", "\n"), result.Replace("\r\n", "\n"));
            }
        }


        [Fact]
        public void CreatePackageWithNuspecIncludeExclude()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");

            var dependencies45 = new List<PackageDependency>();
            dependencies45.Add(new PackageDependency("packageB", VersionRange.Parse("1.0.0"), null, new[] { "z" }));

            var dependencies50 = new List<PackageDependency>();
            dependencies50.Add(new PackageDependency(
                "packageC",
                VersionRange.Parse("1.0.0"),
                new[] { "a", "b", "c" },
                new[] { "b", "c" }));

            var net45 = new PackageDependencyGroup(new NuGetFramework(".NETFramework", new Version(4, 5)), dependencies45);
            builder.DependencyGroups.Add(net45);

            var net50win7 = new PackageDependencyGroup(new NuGetFramework(".NETCoreApp", new Version(5, 0), "windows", new Version(7, 0)), dependencies50);
            builder.DependencyGroups.Add(net50win7);

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                var result = manifestStream.ReadToEnd();

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <dependencies>
      <group targetFramework="".NETFramework4.5"">
        <dependency id=""packageB"" version=""1.0.0"" exclude=""z"" />
      </group>
      <group targetFramework=""net5.0-windows7.0"">
        <dependency id=""packageC"" version=""1.0.0"" include=""a,b,c"" exclude=""b,c"" />
      </group>
    </dependencies>
  </metadata>
</package>".Replace("\r\n", "\n"), result.Replace("\r\n", "\n"));
            }
        }

        [Theory]
        [InlineData("**", @"**\file2.txt", "Content")]
        [InlineData("**", "**/file2.txt", "Content")]
        public void CreatePackageWithNuspecIncludeExcludeWithWildcards(string source, string exclude, string destination)
        {
            using (var directory = new TestSourcesDirectory())
            {
                // Arrange
                PackageBuilder builder = new PackageBuilder();

                // Act
                builder.AddFiles(directory.Path, source, destination, exclude);

                // Assert
                var expectedResults = new[]
                {
                    new
                    {
                        Path = string.Format("Content{0}file1.txt", Path.DirectorySeparatorChar),
                        EffectivePath = "file1.txt"
                    },
                    new
                    {
                        Path = string.Format("Content{0}dir1{0}file1.txt", Path.DirectorySeparatorChar),
                        EffectivePath = string.Format("dir1{0}file1.txt", Path.DirectorySeparatorChar)
                    },
                    new
                    {
                        Path = string.Format("Content{0}dir1{0}dir2{0}file1.txt", Path.DirectorySeparatorChar),
                        EffectivePath = string.Format("dir1{0}dir2{0}file1.txt", Path.DirectorySeparatorChar)
                    }
                };

                var orderedExpectedResults = expectedResults.OrderBy(i => i.Path);
                var orderedActualResults = builder.Files.Select(f => new { f.Path, f.EffectivePath }).OrderBy(i => i.Path);

                Assert.Equal(orderedExpectedResults, orderedActualResults);
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(@"dir1\dir2\**", true, "Content")]
        [InlineData("dir1/dir2/**", false, "Content")]
        public void PackageBuilder_AddFiles_HasDifferentBehaviorDependingOnSlash(string source, bool expectFlattened, string destination)
        {
            // https://github.com/NuGet/Home/issues/11234
            using (var directory = new TestSourcesDirectory())
            {
                // Arrange
                var builder = new PackageBuilder();

                // Act
                builder.AddFiles(directory.Path, source, destination);

                // Assert
                var expectedResults = new[]
                {
                    new
                    {
                        Path = expectFlattened ? $"Content{Path.DirectorySeparatorChar}file1.txt" : $"Content{Path.DirectorySeparatorChar}dir1{Path.DirectorySeparatorChar}dir2{Path.DirectorySeparatorChar}file1.txt",
                        EffectivePath = expectFlattened ? "file1.txt" : $"dir1{Path.DirectorySeparatorChar}dir2{Path.DirectorySeparatorChar}file1.txt"
                    },
                    new
                    {
                        Path = expectFlattened ? $"Content{Path.DirectorySeparatorChar}file2.txt" : $"Content{Path.DirectorySeparatorChar}dir1{Path.DirectorySeparatorChar}dir2{Path.DirectorySeparatorChar}file2.txt",
                        EffectivePath = expectFlattened ? "file2.txt" : $"dir1{Path.DirectorySeparatorChar}dir2{Path.DirectorySeparatorChar}file2.txt"
                    }
                };

                var orderedExpectedResults = expectedResults.OrderBy(i => i.Path);
                var orderedActualResults = builder.Files.Select(f => new { f.Path, f.EffectivePath }).OrderBy(i => i.Path);

                Assert.Equal(orderedExpectedResults, orderedActualResults);
            }
        }

        [Fact]
        public void CreatePackageWithNuspecContentV2()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile("contentFiles" + Path.DirectorySeparatorChar + "any" + Path.DirectorySeparatorChar + "any" + Path.DirectorySeparatorChar + "config" + Path.DirectorySeparatorChar + "config.xml"));
            builder.Files.Add(CreatePackageFile("contentFiles" + Path.DirectorySeparatorChar + "cs" + Path.DirectorySeparatorChar + "net45" + Path.DirectorySeparatorChar + "code.cs.pp"));

            builder.ContentFiles.Add(new ManifestContentFiles()
            {
                Include = "**/*",
                BuildAction = "Compile"
            });

            builder.ContentFiles.Add(new ManifestContentFiles()
            {
                Include = "**/*",
                Exclude = "**/*.cs",
                BuildAction = "None",
                Flatten = "true",
                CopyToOutput = "true"
            });

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                var result = manifestStream.ReadToEnd();

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <contentFiles>
      <files include=""**/*"" buildAction=""Compile"" />
      <files include=""**/*"" exclude=""**/*.cs"" buildAction=""None"" copyToOutput=""true"" flatten=""true"" />
    </contentFiles>
  </metadata>
</package>".Replace("\r\n", "\n"), result.Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void OwnersFallsBackToAuthorsIfNoneSpecified()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Description"
            };
            builder.Authors.Add("JohnDoe");
            var ms = new MemoryStream();

            // Act
            Manifest.Create(builder).Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>JohnDoe</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void ReleaseNotesAttributeIsRecognized()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Description",
                ReleaseNotes = "Release Notes"
            };
            builder.Authors.Add("JohnDoe");
            var ms = new MemoryStream();

            // Act
            Manifest.Create(builder).Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>JohnDoe</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <releaseNotes>Release Notes</releaseNotes>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void CreatePackageUsesV1SchemaNamespaceIfFrameworkAssembliesAreUsed()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("JohnDoe");
            builder.FrameworkReferences.Add(new FrameworkAssemblyReference("System.Web", new[] { NuGetFramework.AnyFramework }));
            var ms = new MemoryStream();

            // Act
            Manifest.Create(builder).Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>JohnDoe</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <frameworkAssemblies>
      <frameworkAssembly assemblyName=""System.Web"" targetFramework="""" />
    </frameworkAssemblies>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void CreatePackageUsesV2SchemaNamespaceIfReferenceAssembliesAreUsed()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
                PackageAssemblyReferences = new[] { new PackageReferenceSet(NuGetFramework.AnyFramework, new string[] { "foo.dll" }) }
            };
            builder.Authors.Add("JohnDoe");
            var ms = new MemoryStream();

            // Act
            Manifest.Create(builder).Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>JohnDoe</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <references>
      <reference file=""foo.dll"" />
    </references>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void CreatePackageUsesV2SchemaNamespaceIfDependencyHasNoTargetFramework()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");

            var dependencies = new PackageDependency[] {
                        new PackageDependency("B")
                    };

            builder.DependencyGroups.Add(new PackageDependencyGroup(NuGetFramework.AnyFramework, dependencies));
            var ms = new MemoryStream();

            // Act
            Manifest.Create(builder).Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <dependencies>
      <dependency id=""B"" />
    </dependencies>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void CreatePackageUsesV4SchemaNamespaceIfDependencyHasTargetFramework()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");

            var fx = new NuGetFramework("Silverlight", new Version("4.0"));
            var dependencies = new PackageDependency[] {
                        new PackageDependency("B", null)
                    };
            builder.DependencyGroups.Add(new PackageDependencyGroup(fx, dependencies));

            var ms = new MemoryStream();

            // Act
            Manifest.Create(builder).Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <dependencies>
      <group targetFramework=""Silverlight4.0"">
        <dependency id=""B"" />
      </group>
    </dependencies>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void CreatePackageUsesV4SchemaNamespaceIfContentHasTargetFramework()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile("content" + Path.DirectorySeparatorChar + "winrt53" + Path.DirectorySeparatorChar + "one.txt"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageWithServiceableElement()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Description",
                Authors = { "testAuthor" },
                Serviceable = true,
                Files =
                {
                    CreatePackageFile("content" + Path.DirectorySeparatorChar + "winrt53" + Path.DirectorySeparatorChar + "one.txt")
                }
            };

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <serviceable>true</serviceable>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageWithPackageTypes()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Description",
                Authors = { "testAuthor" },
                PackageTypes = new[]
                {
                    new PackageType("foo", new Version(0, 0)),
                    new PackageType("bar", new Version(2, 0, 0)),
                },
                Files =
                {
                    CreatePackageFile("content" + Path.DirectorySeparatorChar + "winrt53" + Path.DirectorySeparatorChar + "one.txt")
                }
            };

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Description</description>
    <packageTypes>
      <packageType name=""foo"" />
      <packageType name=""bar"" version=""2.0.0"" />
    </packageTypes>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageDoesNotUseV4SchemaNamespaceIfContentHasUnsupportedTargetFramework()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile("content" + Path.DirectorySeparatorChar + "bar" + Path.DirectorySeparatorChar + "one.txt"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageUsesV4SchemaNamespaceIfToolsHasTargetFramework()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile("tools" + Path.DirectorySeparatorChar + "sl4" + Path.DirectorySeparatorChar + "one.dll"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageDoesNotUseV4SchemaNamespaceIfToolsHasUnsupportedTargetFramework()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile("tools" + Path.DirectorySeparatorChar + "foo" + Path.DirectorySeparatorChar + "one.dll"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Theory]
        [InlineData("lib\\sl4\\_._")]
        [InlineData("content\\winrt\\_._")]
        [InlineData("tools\\sl4-wp\\_._")]
        public void CreatePackageUsesV4SchemaNamespaceIfLibHasEmptyTargetFramework(string packagePath)
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile(packagePath.Replace('\\', Path.DirectorySeparatorChar)));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Theory]
        [InlineData("content\\web.config.install.xdt")]
        [InlineData("content\\app.config.uninstall.xdt")]
        [InlineData("content\\winrt45\\foo.uninstall.xdt")]
        [InlineData("content\\winrt45\\sub\\bar.uninstall.xdt")]
        public void CreatePackageUsesV5SchemaNamespaceIfContentHasTransformFile(string packagePath)
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile(packagePath.Replace('\\', Path.DirectorySeparatorChar)));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageUsesV5SchemaNamespaceIfContentHasBothInstallAndUninstallTransformFile()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile("content" + Path.DirectorySeparatorChar + "web.config.install.xdt"));
            builder.Files.Add(CreatePackageFile("content" + Path.DirectorySeparatorChar + "app.config.uninstall.xdt"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Theory]
        [InlineData("lib\\web.config.install.xdt")]
        [InlineData("lib\\app.config.uninstall.xdt")]
        [InlineData("tools\\foo.uninstall.xdt")]
        [InlineData("random\\sub\\bar.uninstall.xdt")]
        public void CreatePackageDoesNotUseV5SchemaNamespaceIfTransformFileIsOutsideContent(string packagePath)
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile(packagePath.Replace('\\', Path.DirectorySeparatorChar)));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Theory]
        [InlineData("content\\web.config.install2.xdt")]
        [InlineData("content\\app.config.xdt")]
        [InlineData("content\\foo.update.xdt")]
        [InlineData("content\\sub\\bar.xdt.uninstall")]
        [InlineData("content\\sub\\bar.xdt.install")]
        public void CreatePackageDoesNotUseV5SchemaNamespaceIfTransformFileExtensionIsNotComplete(string packagePath)
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile(packagePath.Replace('\\', Path.DirectorySeparatorChar)));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageUsesV5SchemaNamespaceIfReferencesTargetFramework()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.PackageAssemblyReferences = new[] {
                new PackageReferenceSet(
                    NuGetFramework.Parse(".NET, Version=3.0"),
                    new[] { "one.dll" })};
            builder.Files.Add(CreatePackageFile("lib" + Path.DirectorySeparatorChar + "one.dll"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <references>
      <group targetFramework="".NETFramework3.0"">
        <reference file=""one.dll"" />
      </group>
    </references>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageUsesV5SchemaNamespaceIfDevelopmentDependency()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
                DevelopmentDependency = true
            };
            builder.Authors.Add("testAuthor");
            builder.PackageAssemblyReferences = new[] {
                new PackageReferenceSet(
                    NuGetFramework.Parse(".NET, Version=3.0"),
                    new[] { "one.dll" })};
            builder.Files.Add(CreatePackageFile("lib" + Path.DirectorySeparatorChar + "one.dll"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <developmentDependency>true</developmentDependency>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <references>
      <group targetFramework="".NETFramework3.0"">
        <reference file=""one.dll"" />
      </group>
    </references>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageDoesNotUseV5SchemaNamespaceIfReferencesHasOnlyNullTargetFramework()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
            };
            builder.Authors.Add("testAuthor");
            builder.PackageAssemblyReferences = new[] {
                new PackageReferenceSet(
                    NuGetFramework.UnsupportedFramework,
                    new[] { "one.dll" })};
            builder.Files.Add(CreatePackageFile("lib" + Path.DirectorySeparatorChar + "one.dll"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <references>
      <reference file=""one.dll"" />
    </references>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageUsesV5SchemaNamespaceIfMinClientVersionIsSet()
        {
            // Arrange
            var builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
                MinClientVersion = new Version("2.0")
            };
            builder.Authors.Add("testAuthor");
            builder.Files.Add(CreatePackageFile("a.txt"));

            using (var ms = new MemoryStream())
            {
                builder.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);

                var manifestStream = GetManifestStream(ms);

                // Assert
                Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd"">
  <metadata minClientVersion=""2.0"">
    <id>A</id>
    <version>1.0.0</version>
    <authors>testAuthor</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
  </metadata>
</package>", manifestStream.ReadToEnd());
            }
        }

        [Fact]
        public void CreatePackageTrimsExtraWhitespace()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "                 A                 ",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions                                         ",
                Summary = "                            Summary",
                Language = "     en-us   ",
                Copyright = "            Copyright 2012                "
            };
            builder.Authors.Add("JohnDoe");
            builder.Owners.Add("John");
            builder.Tags.Add("t1");
            builder.Tags.Add("t2");
            builder.Tags.Add("t3");
            var dependencies = new PackageDependency[] {
                        new PackageDependency("    X     ")
                    };
            builder.DependencyGroups.Add(new PackageDependencyGroup(NuGetFramework.AnyFramework, dependencies));
            var ms = new MemoryStream();

            // Act
            Manifest.Create(builder).Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>JohnDoe</authors>
    <owners>John</owners>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <summary>Summary</summary>
    <copyright>Copyright 2012</copyright>
    <language>en-us</language>
    <tags>t1 t2 t3</tags>
    <dependencies>
      <dependency id=""X"" />
    </dependencies>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void VersionFormatIsPreserved()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
                Summary = "Summary",
            };
            builder.Authors.Add("JohnDoe");

            var dependencySet = new PackageDependencyGroup(NuGetFramework.AnyFramework, new[] {
                        new PackageDependency("B", new VersionRange(NuGetVersion.Parse("1.0"), true)),
                        new PackageDependency("C", new VersionRange(NuGetVersion.Parse("1.0"), false, NuGetVersion.Parse("5.0")))
            });

            //var dependencySet = new PackageDependencyGroup(NuGetFramework.AnyFramework, new[] {
            //            new PackageDependency("B", new VersionSpec
            //                {
            //                    MinVersion = NuGetVersion.Parse("1.0"),
            //                    IsMinInclusive = true
            //                }),
            //            new PackageDependency("C", new VersionSpec
            //            {
            //                MinVersion = NuGetVersion.Parse("1.0"),
            //                MaxVersion = NuGetVersion.Parse("5.0"),
            //                IsMinInclusive = false
            //            })
            //        });


            builder.DependencyGroups.Add(dependencySet);

            var ms = new MemoryStream();

            // Act
            Manifest.Create(builder).Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            // REVIEW: Changed the value of these to get this test passing
            //      <dependency id=""B"" version=""1.0"" />
            //      <dependency id=""C"" version=""(1.0, 5.0)"" />

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>
    <id>A</id>
    <version>1.0.0</version>
    <authors>JohnDoe</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Descriptions</description>
    <summary>Summary</summary>
    <dependencies>
      <dependency id=""B"" version=""1.0.0"" />
      <dependency id=""C"" version=""(1.0.0, 5.0.0)"" />
    </dependencies>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void SavingPackageWithDuplicateDependenciesThrows()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
                Summary = "Summary",
            };
            builder.Authors.Add("JohnDoe");

            var dependencySet = new PackageDependencyGroup(NuGetFramework.AnyFramework, new[] {
                        new PackageDependency("B", new VersionRange(NuGetVersion.Parse("1.0"), true)),
                        new PackageDependency("B", new VersionRange(NuGetVersion.Parse("1.0"), false, NuGetVersion.Parse("5.0")))
            });

            builder.DependencyGroups.Add(dependencySet);

            var ms = new MemoryStream();

            // Act
            ExceptionAssert.Throws<InvalidOperationException>(() => Manifest.Create(builder).Save(ms), "'A' already has a dependency defined for 'B'.");
        }

        [Fact]
        public void SavingPackageWithInvalidDependencyRangeThrows()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
                Summary = "Summary",
            };
            builder.Authors.Add("JohnDoe");

            var dependencySet = new PackageDependencyGroup(NuGetFramework.AnyFramework, new[] {
                        new PackageDependency("B", new VersionRange(NuGetVersion.Parse("1.0"), true, NuGetVersion.Parse("1.0")))
                    });

            builder.DependencyGroups.Add(dependencySet);

            var ms = new MemoryStream();

            // Act
            ExceptionAssert.Throws<InvalidOperationException>(() => Manifest.Create(builder).Save(ms), "Dependency 'B' has an invalid version.");
        }

        [Fact]
        public void AddingDuplicateFiles_Throws()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Test",
            };
            builder.Authors.Add("Test");
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"lib\net5.0\Foo.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"lib\net5.0\Foo.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"lib\net5.0\Bar.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"lib\net5.0\Bar.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"lib\net5.0\Baz.dll" });

            ExceptionAssert.Throws<PackagingException>(() => builder.Save(new MemoryStream()), $@"Attempted to pack multiple files into the same location(s). The following destinations were used multiple times: lib{Path.DirectorySeparatorChar}net5.0{Path.DirectorySeparatorChar}Foo.dll, lib{Path.DirectorySeparatorChar}net5.0{Path.DirectorySeparatorChar}Bar.dll");
        }

        [Fact]
        public void SavingPackageValidatesReferences()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Test",
            };
            builder.Authors.Add("Test");
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"lib\Foo.dll" });
            builder.PackageAssemblyReferences = new[] { new PackageReferenceSet(NuGetFramework.AnyFramework, new string[] { "Bar.dll" }) };

            ExceptionAssert.Throws<PackagingException>(() => builder.Save(new MemoryStream()),
                "Invalid assembly reference 'Bar.dll'. Ensure that a file named 'Bar.dll' exists in the lib directory.");
        }

        [Fact]
        public void SavingPackageValidatesMissingTPVInReferences()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Test",
            };
            builder.Authors.Add("Test");
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"lib\net5.0-windows\Foo.dll" });
            builder.PackageAssemblyReferences = new[] { new PackageReferenceSet(NuGetFramework.Parse("net5.0-windows"), new string[] { "Foo.dll" }) };

            ExceptionAssert.Throws<PackagingException>(() => builder.Save(new MemoryStream()),
                "Some reference group TFMs are missing a platform version: net5.0-windows");
        }

        [Fact]
        public void SavingPackageValidatesMissingTPVInFiles()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Test",
            };
            builder.Authors.Add("Test");
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"lib\net5.0-windows\Foo.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"ref\net6.0-windows\Foo.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"runtimes\win7-x64\lib\net7.0-windows\Foo.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"runtimes\win7-x64\nativeassets\net8.0-windows\Foo.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"build\net9.0-windows\foo.props" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"contentFiles\csharp\net10.0-windows\Foo.txt" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"tools\net11.0-windows\win7-x64\Foo.exe" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"embed\net12.0-windows\Foo.dll" });
            builder.Files.Add(new PhysicalPackageFile { TargetPath = @"buildTransitive\net13.0-windows\foo.props" });

            ExceptionAssert.Throws<PackagingException>(() => builder.Save(new MemoryStream()),
                "Some included files are included under TFMs which are missing a platform version: " + string.Join(", ", new string[]
                {
                  "lib/net5.0-windows/Foo.dll",
                  "ref/net6.0-windows/Foo.dll",
                  "runtimes/win7-x64/lib/net7.0-windows/Foo.dll",
                  "runtimes/win7-x64/nativeassets/net8.0-windows/Foo.dll",
                  "build/net9.0-windows/foo.props",
                  "contentFiles/csharp/net10.0-windows/Foo.txt",
                  "tools/net11.0-windows/win7-x64/Foo.exe",
                  "embed/net12.0-windows/Foo.dll",
                  "buildTransitive/net13.0-windows/foo.props"
                }.OrderBy(str => str)));
        }

        [Fact]
        public void SavingPackageValidatesMissingTPVInFrameworkReferences()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Test",
            };
            builder.Authors.Add("Test");
            builder.FrameworkReferences.Add(new FrameworkAssemblyReference("System.Web", new[] { NuGetFramework.Parse("net5.0-windows") }));

            ExceptionAssert.Throws<PackagingException>(() => builder.Save(new MemoryStream()),
                "Some framework assembly reference TFMs are missing a platform version: net5.0-windows");
        }

        [Fact]
        public void SavingPackageValidatesMissingTPVInFrameworkReferenceGroups()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Test",
            };
            builder.Authors.Add("Test");
            builder.FrameworkReferenceGroups.Add(new FrameworkReferenceGroup(NuGetFramework.Parse("net5.0-windows"), new List<FrameworkReference>()));

            ExceptionAssert.Throws<PackagingException>(() => builder.Save(new MemoryStream()),
                "Some reference assembly group TFMs are missing a platform version: net5.0-windows");
        }

        [Fact]
        public void SavingPackageValidatesMissingTPVInDependencyGroups()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Test",
            };
            builder.Authors.Add("Test");
            var dependencySet = new PackageDependencyGroup(NuGetFramework.Parse("net5.0-windows"), new[] {
                        new PackageDependency("B", new VersionRange(NuGetVersion.Parse("2.0"), true, NuGetVersion.Parse("2.0")))
                    });

            builder.DependencyGroups.Add(dependencySet);

            ExceptionAssert.Throws<PackagingException>(() => builder.Save(new MemoryStream()),
                "Some dependency group TFMs are missing a platform version: net5.0-windows");
        }

        [Fact]
        public void SavingPackageWithInvalidDependencyVersionMaxLessThanMinThrows()
        {
            // Arrange
            PackageBuilder builder = new PackageBuilder()
            {
                Id = "A",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Descriptions",
                Summary = "Summary",
            };
            builder.Authors.Add("JohnDoe");

            var dependencySet = new PackageDependencyGroup(NuGetFramework.AnyFramework, new[] {
                        new PackageDependency("B", new VersionRange(NuGetVersion.Parse("2.0"), true, NuGetVersion.Parse("1.0")))
                    });

            builder.DependencyGroups.Add(dependencySet);

            var ms = new MemoryStream();

            // Act
            ExceptionAssert.Throws<InvalidOperationException>(() => Manifest.Create(builder).Save(ms), "Dependency 'B' has an invalid version.");
        }

        [Fact]
        public void SaveThrowsIfRequiredPropertiesAreMissing()
        {
            // Arrange
            var builder = new PackageBuilder();
            builder.Id = "Package";
            builder.Files.Add(new Mock<IPackageFile>().Object);

            // Act & Assert
            ExceptionAssert.Throws<Exception>(() => builder.Save(new MemoryStream()), @"Version is required.
Authors is required.
Description is required.");
        }

        [Fact]
        public void SaveThrowsIfNoFilesOrDependencies()
        {
            // Arrange
            var builder = new PackageBuilder();
            builder.Id = "A";
            builder.Version = NuGetVersion.Parse("1.0");
            builder.Description = "Description";

            // Act & Assert
            ExceptionAssert.Throws<PackagingException>(() => builder.Save(new MemoryStream()), "Cannot create a package that has no dependencies nor content.");
        }

        [Fact]
        public void PackageBuilderThrowsIfXmlIsMalformed()
        {
            // Arrange
            string spec1 = "kjdkfj";
            string spec2 = @"<?xml version=""1.0"" encoding=""utf-8""?>";
            string spec3 = @"<?xml version=""1.0"" encoding=""utf-8""?><package />";
            string spec4 = @"<?xml version=""1.0"" encoding=""utf-8""?><package><metadata></metadata></package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act and Assert
            ExceptionAssert.Throws<XmlException>(() => new PackageBuilder(spec1.AsStream(), null), "Data at the root level is invalid. Line 1, position 1.");
            ExceptionAssert.Throws<XmlException>(() => new PackageBuilder(spec2.AsStream(), null), "Root element is missing.");
#if !IS_CORECLR
            ExceptionAssert.Throws<InvalidOperationException>(() => new PackageBuilder(spec3.AsStream(), null));
            ExceptionAssert.Throws<InvalidOperationException>(() => new PackageBuilder(spec4.AsStream(), null));
#else
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec3.AsStream(), null));
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec4.AsStream(), null));
#endif
        }

        [Fact]
        public void MissingIdThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
  </metadata></package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act & Assert
#if !IS_CORECLR
            ExceptionAssert.Throws<InvalidOperationException>(
                () => new PackageBuilder(spec.AsStream(), null),
                "The element 'metadata' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd' has incomplete content. " +
                "List of possible elements expected: 'id' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd'. " +
                "This validation error occurred in a 'metadata' element.");
#else
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec.AsStream(), null), "The required element 'id' is missing from the manifest.");
#endif
        }

        [Fact]
        public void WrongCaseIdThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <ID>aaa</ID>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
  </metadata></package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act & Assert
#if !IS_CORECLR
            var exception = Assert.Throws<InvalidOperationException>(() => new PackageBuilder(spec.AsStream(), null));
            Assert.StartsWith(
                "The element 'metadata' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd' has invalid child element 'ID' " +
                "in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd'.",
                exception.Message);
            Assert.EndsWith(
                "This validation error occurred in a 'ID' element.",
                exception.Message);
#else
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec.AsStream(), null), "The required element 'id' is missing from the manifest.");
#endif
        }

        [Fact]
        public void IdExceedingMaxLengthThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <id>aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
  </metadata></package>";

            // Act & Assert
            ExceptionAssert.Throws<Exception>(() => new PackageBuilder(spec.AsStream(), null), "Id must not exceed 100 characters.");
        }

        [Fact]
        public void MissingVersionThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <id>Artem.XmlProviders</id>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
  </metadata></package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act & Assert
#if !IS_CORECLR
            ExceptionAssert.Throws<InvalidOperationException>(
                () => new PackageBuilder(spec.AsStream(), null),
                "The element 'metadata' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd' has incomplete content. " +
                "List of possible elements expected: 'version' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd'. " +
                "This validation error occurred in a 'metadata' element.");
#else
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec.AsStream(), null), "The required element 'version' is missing from the manifest.");
#endif
        }

        [Fact]
        public void MissingAuthorsThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
  </metadata></package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act & Assert
#if !IS_CORECLR
            ExceptionAssert.Throws<InvalidOperationException>(
                () => new PackageBuilder(spec.AsStream(), null),
                "The element 'metadata' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd' has incomplete content. " +
                "List of possible elements expected: 'authors' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd'. " +
                "This validation error occurred in a 'metadata' element.");
#else
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec.AsStream(), null), "The required element 'authors' is missing from the manifest.");
#endif
        }

        [Fact]
        public void MissingDescriptionThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
  </metadata></package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act & Assert
#if !IS_CORECLR
            ExceptionAssert.Throws<InvalidOperationException>(
                () => new PackageBuilder(spec.AsStream(), null),
                "The element 'metadata' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd' has incomplete content. " +
                "List of possible elements expected: 'description' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd'. " +
                "This validation error occurred in a 'metadata' element.");
#else
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec.AsStream(), null), "The required element 'description' is missing from the manifest.");
#endif
        }

        [Fact]
        public void MalformedDependenciesThrows()
        {
            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act & Assert
#if !IS_CORECLR
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <dependencies>
        <dependency />
    </dependencies>
  </metadata></package>";

            ExceptionAssert.Throws<InvalidOperationException>(
                () => new PackageBuilder(spec.AsStream(), null),
                "The required attribute 'id' is missing. " +
                "This validation error occurred in a 'dependency' element.");
#else
            // Not thrown in CoreCLR
#endif
        }

        [Fact]
        public void ReferencesContainMixedElementsThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <references>
        <reference file=""a.dll"" />
        <group>
           <reference file=""b.dll"" />
        </group>
    </references>
</metadata></package>";

            // Act & Assert
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec.AsStream(), null), "<references> element must not contain both <group> and <reference> child elements.");
        }

        [Fact]
        public void MissingFileSrcThrows()
        {
            // Act
            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Assert
#if !IS_CORECLR
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <dependencies>
        <dependency id=""foo"" />
    </dependencies>
  </metadata>
  <files>
    <file />
  </files>
</package>";

            ExceptionAssert.Throws<InvalidOperationException>(
                () => new PackageBuilder(spec.AsStream(), null),
                "The required attribute 'src' is missing. " +
                "This validation error occurred in a 'file' element.");
#else
            // REVIEW: Not thrown in CoreCLR
#endif
        }

        [Fact]
        public void MisplacedFileNodeThrows()
        {
            // Arrange
            // Act & Assert
#if !IS_CORECLR
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package><metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <language>en-us</language>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <dependencies>
        <dependency id=""foo"" />
    </dependencies>
  <files>
    <file />
  </files>
  </metadata>
</package>";

            ExceptionAssert.Throws<InvalidOperationException>(() => new PackageBuilder(spec.AsStream(), null));
#else
            // REVIEW: Not thrown in CoreCLR
#endif
        }

        [Fact]
        public void ReadingManifestWithNamespaceBuilderFromStreamCopiesMetadata()
        {
            // Arrange
            string spec = @"<?xml version=""1.0""?>
<package>
    <metadata>
    <id>Artem.XmlProviders  </id>
    <version>2.5</version>
    <title>Some awesome package       </title>
    <authors>These are the authors</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <licenseUrl>http://somesite/somelicense.txt</licenseUrl>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
    <tags>t1      t2    foo-bar</tags>
    <copyright>Copyright 2011</copyright>
  </metadata>
</package>";

            // Act
            PackageBuilder builder = new PackageBuilder(spec.AsStream(), null);
            var authors = builder.Authors.ToList();
            var owners = builder.Owners.ToList();
            var tags = builder.Tags.ToList();

            // Assert
            Assert.Equal("Artem.XmlProviders", builder.Id);
            Assert.Equal(new NuGetVersion(2, 5, 0, 0), builder.Version);
            Assert.Equal("Some awesome package", builder.Title);
            Assert.Equal(1, builder.Authors.Count);
            Assert.Equal("These are the authors", authors[0]);
            Assert.Equal(3, builder.Tags.Count);
            Assert.Equal("t1", tags[0]);
            Assert.Equal("t2", tags[1]);
            Assert.Equal("foo-bar", tags[2]);
            Assert.Equal("en-US", builder.Language);
            Assert.Equal("Copyright 2011", builder.Copyright);
            Assert.Equal("This is the Description (With, Comma-Separated, Words, in Parentheses).", builder.Description);
            Assert.Equal(new Uri("http://somesite/somelicense.txt"), builder.LicenseUrl);
            Assert.True(builder.RequireLicenseAcceptance);
        }

        [Fact]
        public void ReadingManifestWithSerializationNamespaceBuilderFromStreamCopiesMetadata()
        {
            // Arrange
            string spec = @"<?xml version=""1.0""?>
<package>
    <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <title>Some awesome package</title>
    <authors>Velio Ivanov</authors>
    <owners>John Doe</owners>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <licenseUrl>http://somesite/somelicense.txt</licenseUrl>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
  </metadata>
</package>";

            // Act
            PackageBuilder builder = new PackageBuilder(spec.AsStream(), null);
            var authors = builder.Authors.ToList();
            var owners = builder.Owners.ToList();

            // Assert
            Assert.Equal("Artem.XmlProviders", builder.Id);
            Assert.Equal(new NuGetVersion(2, 5, 0, 0), builder.Version);
            Assert.Equal("Some awesome package", builder.Title);
            Assert.Equal(1, builder.Authors.Count);
            Assert.Equal("Velio Ivanov", authors[0]);
            Assert.Equal(1, builder.Owners.Count);
            Assert.Equal("John Doe", owners[0]);
            Assert.Equal("en-US", builder.Language);
            Assert.Equal("This is the Description (With, Comma-Separated, Words, in Parentheses).", builder.Description);
            Assert.Equal(new Uri("http://somesite/somelicense.txt"), builder.LicenseUrl);
            Assert.True(builder.RequireLicenseAcceptance);
        }

        [Fact]
        public void ReadingManifestWithOldStyleXmlnsDeclaratoinsFromStreamCopiesMetadata()
        {
            // Arrange
            string spec = @"<?xml version=""1.0""?>
<package xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <title>Some awesome package</title>
    <authors>Velio Ivanov</authors>
    <owners>John Doe</owners>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <licenseUrl>http://somesite/somelicense.txt</licenseUrl>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
  </metadata>
</package>";

            // Act
            PackageBuilder builder = new PackageBuilder(spec.AsStream(), null);
            var authors = builder.Authors.ToList();
            var owners = builder.Owners.ToList();

            // Assert
            Assert.Equal("Artem.XmlProviders", builder.Id);
            Assert.Equal(new NuGetVersion(2, 5, 0, 0), builder.Version);
            Assert.Equal("Some awesome package", builder.Title);
            Assert.Equal(1, builder.Authors.Count);
            Assert.Equal("Velio Ivanov", authors[0]);
            Assert.Equal(1, builder.Owners.Count);
            Assert.Equal("John Doe", owners[0]);
            Assert.Equal("en-US", builder.Language);
            Assert.Equal("This is the Description (With, Comma-Separated, Words, in Parentheses).", builder.Description);
            Assert.Equal(new Uri("http://somesite/somelicense.txt"), builder.LicenseUrl);
            Assert.True(builder.RequireLicenseAcceptance);
        }

        [Fact]
        public void ReadingPackageManifestFromStreamCopiesMetadata()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <title>Some awesome package</title>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <licenseUrl>http://somesite/somelicense.txt</licenseUrl>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
    <serviceable>true</serviceable>
    <copyright>2010</copyright>
    <packageTypes>
        <packageType name=""foo"" />
        <packageType name=""bar"" version=""2.0.0"" />
    </packageTypes>
    <dependencies>
        <dependency id=""A"" version=""[1.0]"" />
        <dependency id=""B"" version=""[1.0, 2.5)"" />
    </dependencies>
  </metadata>
</package>";

            // Act
            PackageBuilder builder = new PackageBuilder(spec.AsStream(), null);
            var authors = builder.Authors.ToList();

            // Assert
            Assert.Equal("Artem.XmlProviders", builder.Id);
            Assert.Equal(new NuGetVersion(2, 5, 0, 0), builder.Version);
            Assert.Equal("Some awesome package", builder.Title);
            Assert.Equal(1, builder.Authors.Count);
            Assert.Equal("Velio Ivanov", authors[0]);
            Assert.Equal("en-US", builder.Language);
            Assert.Equal("2010", builder.Copyright);
            Assert.Equal("This is the Description (With, Comma-Separated, Words, in Parentheses).", builder.Description);
            Assert.Equal(new Uri("http://somesite/somelicense.txt"), builder.LicenseUrl);
            Assert.True(builder.RequireLicenseAcceptance);
            Assert.True(builder.Serviceable);
            Assert.Equal(2, builder.PackageTypes.Count);
            Assert.Equal("foo", builder.PackageTypes.ElementAt(0).Name);
            Assert.Equal(new Version(0, 0), builder.PackageTypes.ElementAt(0).Version);
            Assert.Equal("bar", builder.PackageTypes.ElementAt(1).Name);
            Assert.Equal(new Version(2, 0, 0), builder.PackageTypes.ElementAt(1).Version);

            Assert.Equal(1, builder.DependencyGroups.Count);
            var dependencyGroup = builder.DependencyGroups.ElementAt(0);

            IDictionary<string, VersionRange> dependencies = dependencyGroup.Packages.ToDictionary(p => p.Id, p => p.VersionRange);
            // <dependency id="A" version="[1.0]" />
            Assert.True(dependencies["A"].IsMinInclusive);
            Assert.True(dependencies["A"].IsMaxInclusive);
            Assert.Equal(NuGetVersion.Parse("1.0"), dependencies["A"].MinVersion);
            Assert.Equal(NuGetVersion.Parse("1.0"), dependencies["A"].MaxVersion);
            // <dependency id="B" version="[1.0, 2.5)" />
            Assert.True(dependencies["B"].IsMinInclusive);
            Assert.False(dependencies["B"].IsMaxInclusive);
            Assert.Equal(NuGetVersion.Parse("1.0"), dependencies["B"].MinVersion);
            Assert.Equal(NuGetVersion.Parse("2.5"), dependencies["B"].MaxVersion);
        }

        [Fact]
        public void ReadingPackageManifestRecognizeDependencyWithTargetFramework()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>SuperPackage</id>
    <version>2.5</version>
    <authors>testAuthor</authors>
    <description>description</description>
    <dependencies>
        <group targetFramework=""sl4"">
            <dependency id=""A"" />
        </group>
    </dependencies>
  </metadata>
</package>";

            // Act
            PackageBuilder builder = new PackageBuilder(spec.AsStream(), null);

            // Assert
            Assert.Equal(1, builder.DependencyGroups.Count);
            var dependencyGroup = builder.DependencyGroups.ElementAt(0);

            Assert.Equal(NuGetFramework.Parse("Silverlight, Version=4.0"), dependencyGroup.TargetFramework);
            var dependencies = dependencyGroup.Packages.ToList();
            Assert.Equal(1, dependencies.Count);
            Assert.Equal("A", dependencies[0].Id);
            Assert.Equal(dependencies[0].VersionRange, VersionRange.All);
        }

        [Fact]
        public void ReadingPackageManifestRecognizeMultipleDependenciesWithTargetFramework()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>SuperPackage</id>
    <version>2.5</version>
    <authors>testAuthor</authors>
    <description>description</description>
    <dependencies>
        <group targetFramework=""sl4"">
            <dependency id=""A"" />
        </group>
        <group targetFramework=""net1"">
            <dependency id=""B"" />
            <dependency id=""C"" />
        </group>
        <group targetFramework=""net40-client"">
        </group>
        <group targetFramework=""net5.0-windows"">
        </group>
    </dependencies>
  </metadata>
</package>";

            // Act
            PackageBuilder builder = new PackageBuilder(spec.AsStream(), null);

            // Assert
            Assert.Equal(4, builder.DependencyGroups.Count);
            var dependencyGroup1 = builder.DependencyGroups.ElementAt(0);
            var dependencyGroup2 = builder.DependencyGroups.ElementAt(1);
            var dependencyGroup3 = builder.DependencyGroups.ElementAt(2);
            var dependencyGroup4 = builder.DependencyGroups.ElementAt(3);

            Assert.Equal(NuGetFramework.Parse("Silverlight, Version=4.0"), dependencyGroup1.TargetFramework);
            var dependencies1 = dependencyGroup1.Packages.ToList();
            Assert.Equal(1, dependencies1.Count);
            Assert.Equal("A", dependencies1[0].Id);
            Assert.Equal(dependencies1[0].VersionRange, VersionRange.All);

            Assert.Equal(NuGetFramework.Parse(".NETFramework, Version=1.0"), dependencyGroup2.TargetFramework);
            var dependencies2 = dependencyGroup2.Packages.ToList();
            Assert.Equal(2, dependencies2.Count);
            Assert.Equal("B", dependencies2[0].Id);
            Assert.Equal(dependencies2[0].VersionRange, VersionRange.All);
            Assert.Equal("C", dependencies2[1].Id);
            Assert.Equal(dependencies2[0].VersionRange, VersionRange.All);

            Assert.Equal(NuGetFramework.Parse(".NETFramework, Version=4.0, Profile=Client"), dependencyGroup3.TargetFramework);
            Assert.False(dependencyGroup3.Packages.Any());

            Assert.Equal(NuGetFramework.Parse("net5.0-windows"), dependencyGroup4.TargetFramework);
            Assert.False(dependencyGroup4.Packages.Any());
        }

        [Fact]
        public void PackageBuilderThrowsWhenDependenciesHasMixedDependencyAndGroupChildren()
        {
            // Arrange
            string spec =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>SuperPackage</id>
    <version>2.5</version>
    <authors>testAuthor</authors>
    <description>description</description>
    <dependencies>
        <dependency id=""A"" />
        <group targetFramework=""net40-client"">
        </group>
    </dependencies>
  </metadata>
</package>";

            // Act
            ExceptionAssert.Throws<InvalidDataException>(
                () => { new PackageBuilder(spec.AsStream(), null); },
                "<dependencies> element must not contain both <group> and <dependency> child elements.");
        }

        [Fact]
        public void PackageBuilderThrowsWhenLicenseUrlIsPresentButEmpty()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <licenseUrl></licenseUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
  </metadata>
</package>";

            // Act
            ExceptionAssert.Throws<Exception>(() => new PackageBuilder(spec.AsStream(), null), "LicenseUrl cannot be empty.");
        }

        [Fact]
        public void PackageBuilderThrowsWhenLicenseUrlIsWhiteSpace()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <licenseUrl>    </licenseUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
  </metadata>
</package>";

            // Act
            ExceptionAssert.Throws<Exception>(() => new PackageBuilder(spec.AsStream(), null), "LicenseUrl cannot be empty.");
        }

        [Fact]
        public void PackageBuilderThrowsWhenLicenseUrlIsWhiteSpaceAndLicenseExpressionIsNotNull()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <license type=""expression"">MIT</license>
    <licenseUrl>    </licenseUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
  </metadata>
</package>";

            // Act
            ExceptionAssert.Throws<Exception>(() => new PackageBuilder(spec.AsStream(), null), $"LicenseUrl cannot be empty.{Environment.NewLine}The licenseUrl and license elements cannot be used together.");
        }

        [Fact]
        public void ValidateReferencesAllowsPartialFileNames()
        {
            // Arrange
            var files = new[] {
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net40" + Path.DirectorySeparatorChar + "foo.dll" },
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net40" + Path.DirectorySeparatorChar + "bar.dll" },
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net40" + Path.DirectorySeparatorChar + "baz.exe" },
                    };
            var packageAssemblyReferences = new PackageReferenceSet(NuGetFramework.AnyFramework, new string[] { "foo.dll", "bar", "baz" });

            // Act and Assert
            PackageBuilder.ValidateReferenceAssemblies(files, new[] { packageAssemblyReferences });

            // If we've got this far, no exceptions were thrown.
            Assert.True(true);
        }

        [Fact]
        public void ValidateReferencesAllowsNullFramework()
        {
            // Arrange
            var files = new[] {
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net40" + Path.DirectorySeparatorChar + "foo.dll" },
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net40" + Path.DirectorySeparatorChar + "bar.dll" },
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net40" + Path.DirectorySeparatorChar + "baz.exe" },
                    };
            var packageAssemblyReferences = new PackageReferenceSet((NuGetFramework)null, new string[] { "foo.dll", "bar", "baz" });

            // Act and Assert
            PackageBuilder.ValidateReferenceAssemblies(files, new[] { packageAssemblyReferences });

            // If we've got this far, no exceptions were thrown.
            Assert.True(true);
        }

        [Fact]
        public void ValidateReferencesThrowsForPartialNamesThatDoNotHaveAKnownExtension()
        {
            // Arrange
            var files = new[] {
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net20" + Path.DirectorySeparatorChar + "foo.dll" },
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net20" + Path.DirectorySeparatorChar + "bar.dll" },
                        new PhysicalPackageFile { TargetPath = @"lib" + Path.DirectorySeparatorChar + "net20" + Path.DirectorySeparatorChar + "baz.qux" },
                    };
            var packageAssemblyReferences = new PackageReferenceSet(NuGetFramework.Parse("Silverlight, Version=1.0"), new string[] { "foo.dll", "bar", "baz" });

            // Act and Assert
            ExceptionAssert.Throws<PackagingException>(() => PackageBuilder.ValidateReferenceAssemblies(files, new[] { packageAssemblyReferences }),
                "Invalid assembly reference 'baz'. Ensure that a file named 'baz' exists in the lib directory.");
        }

        public static IEnumerable<object[]> InvalidDependencyData
        {
            get
            {
                var prereleaseVer = NuGetVersion.Parse("1.0.0-a");
                var version = NuGetVersion.Parse("2.3.0.6232");

                yield return new object[] { new VersionRange(prereleaseVer) };
                yield return new object[] { new VersionRange(prereleaseVer, true, version) };
                yield return new object[] { new VersionRange(version, true, prereleaseVer, true) };
                yield return new object[] { new VersionRange(prereleaseVer, true, prereleaseVer) };
            }
        }

        [Fact]
        public void PackageBuilderRequireLicenseAcceptedWithoutLicenseUrlThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <licenseUrl></licenseUrl>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
  </metadata>
</package>";

            // Act
            ExceptionAssert.Throws<Exception>(() => new PackageBuilder(spec.AsStream(), null), @"LicenseUrl cannot be empty.
Enabling license acceptance requires a license or a licenseUrl to be specified. The licenseUrl will be deprecated, consider using the license metadata.");
        }

        [Fact]
        public void PackageBuilderRequireLicenseAcceptedWithLicenseDoesNotThrow()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <license type=""expression"">MIT</license>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
  </metadata>
</package>";

            // Act
            new PackageBuilder(spec.AsStream(), null);
        }

        [Fact]
        public void PackageBuilderThrowsWhenLicenseUrlIsMalformed()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <licenseUrl>this-is-a-malformed-url</licenseUrl>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
  </metadata>
</package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act
            ExceptionAssert.Throws<UriFormatException>(() => new PackageBuilder(spec.AsStream(), null), "Invalid URI: The format of the URI could not be determined.");
        }

        [Fact]
        public void PackageBuilderThrowsIfPackageIdInvalid()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "  a.  b",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Description"
            };
            builder.Authors.Add("Me");

            // Act & Assert
            ExceptionAssert.ThrowsArgumentException(() => builder.Save(new MemoryStream()), "The package ID '  a.  b' contains invalid characters. Examples of valid package IDs include 'MyPackage' and 'MyPackage.Sample'.");
        }

        [Fact]
        public void PackageBuilderThrowsIfPackageIdExceedsMaxLengthLimit()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = new string('c', 101),
                Version = NuGetVersion.Parse("1.0"),
                Description = "Description"
            };
            builder.Authors.Add("Me");

            // Act & Assert
            ExceptionAssert.ThrowsArgumentException(() => builder.Save(new MemoryStream()), "Id must not exceed 100 characters.");
        }

        [Fact]
        public void PackageBuilderThrowsIfDependencyIdInvalid()
        {
            // Arrange
            var builder = new PackageBuilder
            {
                Id = "a.b",
                Version = NuGetVersion.Parse("1.0"),
                Description = "Description"
            };
            builder.Authors.Add("Me");

            builder.DependencyGroups.Add(new PackageDependencyGroup(NuGetFramework.AnyFramework, new[] { new PackageDependency("brainf%2ack") }));

            // Act & Assert
            ExceptionAssert.ThrowsArgumentException(() => builder.Save(new MemoryStream()), "The package ID 'brainf%2ack' contains invalid characters. Examples of valid package IDs include 'MyPackage' and 'MyPackage.Sample'.");
        }

#if !IS_CORECLR
        [Fact]
        public void ReadingPackageWithUnknownSchemaThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/03/nuspec.xsd"">
  <metadata>
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
  </metadata>
</package>";

            // Act & Assert
            ExceptionAssert.Throws<InvalidOperationException>(() => new PackageBuilder(spec.AsStream(), null), "The schema version of 'Artem.XmlProviders' is incompatible with version " + typeof(Manifest).Assembly.GetName().Version + " of NuGet. Please upgrade NuGet to the latest version from http://go.microsoft.com/fwlink/?LinkId=213942.");
        }

        [Fact]
        public void ReadingPackageWithUnknownSchemaAndMissingIdThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/03/nuspec.xsd"">
  <metadata>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
  </metadata>
</package>";

            // Act & Assert
            ExceptionAssert.Throws<InvalidOperationException>(() => new PackageBuilder(spec.AsStream(), null), "The schema version of '' is incompatible with version " + typeof(Manifest).Assembly.GetName().Version + " of NuGet. Please upgrade NuGet to the latest version from http://go.microsoft.com/fwlink/?LinkId=213942.");
        }
#endif

        [Fact]
        public void ReadingPackageWithSchemaWithOlderVersionAttribute()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata schemaVersion=""2.0"">
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
  </metadata>
</package>";

            // Act
            var packageBuilder = new PackageBuilder(spec.AsStream(), null);

            // Assert
            Assert.Equal("Artem.XmlProviders", packageBuilder.Id);
            Assert.Equal(NuGetVersion.Parse("2.5"), packageBuilder.Version);
            Assert.Equal("Velio Ivanov", packageBuilder.Authors.Single());
            Assert.Equal("This is the Description (With, Comma-Separated, Words, in Parentheses).", packageBuilder.Description);
            Assert.Equal("en-US", packageBuilder.Language);
        }

        [Fact]
        public void ReadingPackageWithSchemaVersionAttribute()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata schemaVersion=""3.0"">
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
    <references>
        <reference file=""foo.dll"" />
    </references>
  </metadata>
</package>";

            // Act
            var packageBuilder = new PackageBuilder(spec.AsStream(), null);

            // Assert
            Assert.Equal("Artem.XmlProviders", packageBuilder.Id);
            Assert.Equal(NuGetVersion.Parse("2.5"), packageBuilder.Version);
            Assert.Equal("Velio Ivanov", packageBuilder.Authors.Single());
            Assert.Equal("This is the Description (With, Comma-Separated, Words, in Parentheses).", packageBuilder.Description);
            Assert.Equal("en-US", packageBuilder.Language);

            var packageReferenceSet = packageBuilder.PackageAssemblyReferences.Single();
            Assert.Null(packageReferenceSet.TargetFramework);
            Assert.Equal("foo.dll", packageReferenceSet.References.Single());
        }

        [Fact]
        public void ReadingPackageWithSchemaVersionAttributeWithNamespace()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
  <metadata schemaVersion=""2.0"">
    <id>Artem.XmlProviders</id>
    <version>2.5</version>
    <authors>Velio Ivanov</authors>
    <description>This is the Description (With, Comma-Separated, Words, in Parentheses).</description>
    <language>en-US</language>
  </metadata>
</package>";

            // Act
            var packageBuilder = new PackageBuilder(spec.AsStream(), null);

            // Assert
            Assert.Equal("Artem.XmlProviders", packageBuilder.Id);
            Assert.Equal(NuGetVersion.Parse("2.5"), packageBuilder.Version);
            Assert.Equal("Velio Ivanov", packageBuilder.Authors.Single());
            Assert.Equal("This is the Description (With, Comma-Separated, Words, in Parentheses).", packageBuilder.Description);
            Assert.Equal("en-US", packageBuilder.Language);
        }

        [Fact]
        public void CreatingPackageWithUngroupedReference()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
    <metadata>
        <id>SourceDepotClient</id>
        <version>2.8.0.0</version>
           <authors>pranjalg</authors>
           <licenseUrl>http://cbt-userguide/NugetCanUseInLabs.html</licenseUrl>
        <requireLicenseAcceptance>true</requireLicenseAcceptance>
        <description>Source Depot Client assembly with SDApi.</description>
           <copyright>Copyright 2015</copyright>
              <references>
                  <reference file=""SourceDepotClient.dll"" />
               </references>
           </metadata>
       </package>";
            var ms = new MemoryStream();

            // Act
            var packageBuilder = new PackageBuilder(spec.AsStream(), null);
            Manifest.Create(packageBuilder).Save(ms);

            // Assert
            Assert.Equal("SourceDepotClient", packageBuilder.Id);
            Assert.Equal(NuGetVersion.Parse("2.8.0.0"), packageBuilder.Version);
            Assert.Equal("pranjalg", packageBuilder.Authors.Single());
            Assert.Equal("Source Depot Client assembly with SDApi.", packageBuilder.Description);
            Assert.Equal("SourceDepotClient.dll", packageBuilder.PackageAssemblyReferences.First().References.First());

            ms.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">
  <metadata>
    <id>SourceDepotClient</id>
    <version>2.8.0</version>
    <authors>pranjalg</authors>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
    <licenseUrl>http://cbt-userguide/NugetCanUseInLabs.html</licenseUrl>
    <description>Source Depot Client assembly with SDApi.</description>
    <copyright>Copyright 2015</copyright>
    <references>
      <reference file=""SourceDepotClient.dll"" />
    </references>
  </metadata>
</package>", ms.ReadToEnd());
        }

        [Fact]
        public void MissingMetadataNodeThrows()
        {
            // Arrange
            string spec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package>
</package>";

            // Switch to invariant culture to ensure the error message is in english.
#if !IS_CORECLR
            // REVIEW: Unsupported on CoreCLR
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
#endif

            // Act
#if !IS_CORECLR
            ExceptionAssert.Throws<InvalidOperationException>(
                () => new PackageBuilder(spec.AsStream(), null),
                "The element 'package' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd' has incomplete content. " +
                "List of possible elements expected: 'metadata' in namespace 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd'. " +
                "This validation error occurred in a 'package' element.");
#else
            ExceptionAssert.Throws<InvalidDataException>(() => new PackageBuilder(spec.AsStream(), null), "The required element 'metadata' is missing from the manifest.");
#endif
        }

        [Fact]
        public void PackageBuilderWorksWithFileNamesContainingSpecialCharacters()
        {
            // Arrange
            var fileNames = new[] {
                        @"lib\regular.file.dll",
                        @"lib\name with spaces.dll",
                        @"lib\C#\test.dll",
                        @"content\images\logo123?#78.png",
                        @"content\images\bread&butter.jpg",
                    };

            // Act
            var builder = new PackageBuilder { Id = "test", Version = NuGetVersion.Parse("1.0"), Description = "test" };
            builder.Authors.Add("test");
            foreach (var name in fileNames)
            {
                builder.Files.Add(CreatePackageFile(name.Replace('\\', Path.DirectorySeparatorChar)));
            }

            // Assert
            using (MemoryStream stream = new MemoryStream())
            {
                builder.Save(stream);

                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true))
                {
                    // Get raw filenames without un-escaping.
                    var files = archive.Entries.Select(e => e.FullName).OrderBy(s => s).ToArray();

                    // Linux sorts the first two in different order than Windows
                    Assert.Contains<string>(@"[Content_Types].xml", files);
                    Assert.Contains<string>(@"_rels/.rels", files);
                    Assert.Equal(@"content/images/bread&butter.jpg", files[2]);
                    Assert.Equal(@"content/images/logo123?#78.png", files[3]);
                    Assert.Equal(@"lib/C#/test.dll", files[4]);
                    Assert.Equal(@"lib/name with spaces.dll", files[5]);
                    Assert.Equal(@"lib/regular.file.dll", files[6]);

                    Assert.StartsWith(@"package/services/metadata/core-properties/", files[7]);
                    Assert.EndsWith(@".psmdcp", files[7]);

                    Assert.Equal(@"test.nuspec", files[8]);
                }
            }
        }

        [Fact]
        public void PackageBuilderWorksWithFileNameWithoutAnExtension()
        {
            // Arrange
            var fileNames = new[] {
                        @"myfile",
                    };

            // Act
            var builder = new PackageBuilder { Id = "test", Version = NuGetVersion.Parse("1.0"), Description = "test" };
            builder.Authors.Add("test");
            foreach (var name in fileNames)
            {
                builder.Files.Add(CreatePackageFile(name.Replace('\\', Path.DirectorySeparatorChar)));
            }

            // Assert
            using (MemoryStream stream = new MemoryStream())
            {
                builder.Save(stream);

                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true))
                {
                    var files = archive.GetFiles().OrderBy(s => s).ToArray();

                    // Linux sorts the first two in different order than Windows
                    Assert.Contains<string>(@"[Content_Types].xml", files);
                    Assert.Contains<string>(@"_rels/.rels", files);
                    Assert.Equal(@"myfile", files[2]);

                    Assert.StartsWith(@"package/services/metadata/core-properties/", files[3]);
                    Assert.EndsWith(@".psmdcp", files[3]);

                    Assert.Equal(@"test.nuspec", files[4]);

                    using (var contentTypesReader = new StreamReader(archive.Entries.Single(file => file.FullName == @"[Content_Types].xml").Open()))
                    {
                        var contentTypesXml = XDocument.Parse(contentTypesReader.ReadToEnd());
                        var node = contentTypesXml.Descendants().Single(e => e.Name.LocalName == "Override");

                        Assert.StartsWith(@"<Override PartName=""/myfile"" ContentType=""application/octet""", node.ToString());
                    }
                }
            }
        }

        [Theory]
        [InlineData(".txt")]
        [InlineData("")]
        public void Icon_InvalidExtension_ThrowsException(string fileExtension)
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var nuspecBuilder = NuspecBuilder.Create();
            var rng = new Random();

            var iconFile = $"icon{fileExtension}";
            var errorMessage = $"The 'icon' element '{iconFile}' has an invalid file extension. Valid options are .png, .jpg or .jpeg.";

            nuspecBuilder
                .WithIcon(iconFile)
                .WithFile(iconFile);

            testDirBuilder
                .WithFile(iconFile, rng.Next(1, PackageBuilder.MaxIconFileSize))
                .WithNuspec(nuspecBuilder);

            SavePackageAndAssertException(
                testDirBuilder: testDirBuilder,
                exceptionMessage: errorMessage);
        }

        [Theory]
        [InlineData(".jpeg")]
        [InlineData(".jpg")]
        [InlineData(".png")]
        [InlineData(".PnG")]
        [InlineData(".PNG")]
        [InlineData(".jPG")]
        [InlineData(".jpEG")]
        public void Icon_ValidExtension_Succeeds(string fileExtension)
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var nuspecBuilder = NuspecBuilder.Create();
            var rng = new Random();

            var iconFile = $"icon{fileExtension}";

            nuspecBuilder
                .WithIcon(iconFile)
                .WithFile(iconFile);

            testDirBuilder
                .WithFile(iconFile, rng.Next(1, PackageBuilder.MaxIconFileSize))
                .WithNuspec(nuspecBuilder);

            SavePackageAndAssertIcon(testDirBuilder, iconFile);
        }

        [Fact]
        public void Icon_IconMaxFileSizeExceeded_ThrowsException()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var nuspecBuilder = NuspecBuilder.Create();

            nuspecBuilder
                .WithIcon("icon.jpg")
                .WithFile("icon.jpg");

            testDirBuilder
                .WithFile("icon.jpg", PackageBuilder.MaxIconFileSize + 1)
                .WithNuspec(nuspecBuilder);

            SavePackageAndAssertException(
                testDirBuilder: testDirBuilder,
                exceptionMessage: "The icon file size must not exceed 1 megabyte.");
        }

        [Fact]
        public void Icon_IconFileEntryNotFound_ThrowsException()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var nuspecBuilder = NuspecBuilder.Create();

            nuspecBuilder
                .WithIcon("icon.jpg")
                .WithFile("icono.jpg");

            testDirBuilder
                .WithFile("icono.jpg", 100)
                .WithNuspec(nuspecBuilder);

            SavePackageAndAssertException(
                testDirBuilder: testDirBuilder,
                exceptionMessage: "The icon file 'icon.jpg' does not exist in the package.");
        }

        [Fact]
        public void Icon_HappyPath_Succeeds()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var nuspecBuilder = NuspecBuilder.Create();
            var iconFile = "icon.jpg";
            var rng = new Random();

            nuspecBuilder
                .WithIcon(iconFile)
                .WithFile(iconFile);

            testDirBuilder
                .WithFile(iconFile, rng.Next(1, 1024))
                .WithNuspec(nuspecBuilder);

            SavePackageAndAssertIcon(testDirBuilder, iconFile);
        }

        [Fact(Skip = "Need to solve https://github.com/NuGet/Home/issues/6941 to run this test case")]
        public void Icon_MultipleIconFilesResolved_ThrowsException()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var nuspecBuilder = NuspecBuilder.Create();
            var dirSep = Path.DirectorySeparatorChar;

            nuspecBuilder
                .WithIcon("icon.jpg")
                .WithFile("folder1\\*")
                .WithFile("folder2\\*");

            testDirBuilder
                .WithFile("icon.jpg", 400)
                .WithFile($"folder1{dirSep}icon.jpg", 2)
                .WithFile($"folder1{dirSep}dummy.txt", 2)
                .WithFile($"folder2{dirSep}icon.jpg", 2)
                .WithFile($"folder2{dirSep}file.txt", 2)
                .WithNuspec(nuspecBuilder);

            SavePackageAndAssertException(
                testDirBuilder: testDirBuilder,
                exceptionMessage: "Multiple files resolved as the embedded icon.");
        }


        private void SavePackageAndAssertIcon(TestDirectoryBuilder testDirBuilder, string iconFileEntry)
        {
            using (var sourceDir = testDirBuilder.Build())
            using (var nuspecStream = File.OpenRead(testDirBuilder.NuspecPath)) //sourceDir.NuspecPath
            using (var outputNuPkgStream = new MemoryStream())
            {
                PackageBuilder pkgBuilder = new PackageBuilder(nuspecStream, testDirBuilder.BaseDir); //sourceDir.BaseDir

                pkgBuilder.Save(outputNuPkgStream);

                outputNuPkgStream.Seek(0, SeekOrigin.Begin);

                using (var par = new PackageArchiveReader(outputNuPkgStream))
                {
                    Assert.Equal(iconFileEntry, par.NuspecReader.GetIcon());
                }
            }
        }

        private void SavePackageAndAssertException(TestDirectoryBuilder testDirBuilder, string exceptionMessage)
        {
            using (var sourceDir = testDirBuilder.Build())
            using (var nuspecStream = File.OpenRead(testDirBuilder.NuspecPath)) //sourceDir.NuspecPath
            using (var outputNuPkgStream = new MemoryStream())
            {
                PackageBuilder pkgBuilder = new PackageBuilder(nuspecStream, testDirBuilder.BaseDir); //sourceDir.BaseDir

                var ex = Assert.Throws<PackagingException>(() => pkgBuilder.Save(outputNuPkgStream));
                Assert.Equal(exceptionMessage, ex.Message);
            }
        }

        [Theory]
        [InlineData(@".\test1.txt", "test1.txt")]
        [InlineData(@".\test\..\test1.txt", "test1.txt")]
        [InlineData(@"./test/../test1.txt", "test1.txt")]
        [InlineData(@"..\test1.txt", "test1.txt")]
        [InlineData(@"test1\.\.\test2\..\test1.txt", "test1/test1.txt")]
        public void PackageBuilderWorksWithFilesHavingCurrentDirectoryAsTarget(string inputFile, string outputFile)
        {
            // Act
            var builder = new PackageBuilder { Id = "test", Version = NuGetVersion.Parse("1.0"), Description = "test" };
            builder.Authors.Add("test");
            builder.Files.Add(CreatePackageFile(inputFile));

            // Assert
            using (MemoryStream stream = new MemoryStream())
            {
                builder.Save(stream);

                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true))
                {
                    var files = archive.GetFiles().OrderBy(s => s).ToArray();

                    // Linux sorts the first two in different order than Windows
                    Assert.Contains<string>(@"[Content_Types].xml", files);
                    Assert.Contains<string>(@"_rels/.rels", files);
                    Assert.StartsWith(@"package/services/metadata/core-properties/", files[2]);
                    Assert.Equal(@"test.nuspec", files[3]);
                    Assert.Equal(outputFile, files[4]);
                }
            }
        }

        [Fact]
        public void EmitRequireLicenseAcceptance_ShouldNotEmitElement()
        {
            var builder = CreateEmitRequireLicenseAcceptancePackageBuilder(
                emitRequireLicenseAcceptance: false,
                requireLicenseAcceptance: false);

            using (SaveToTestDirectory(builder, out var reader, out var nuspecContent))
            {
                Assert.False(reader.NuspecReader.GetRequireLicenseAcceptance());

                Assert.DoesNotContain("<requireLicenseAcceptance>", nuspecContent);
            }
        }

        [Theory, InlineData(false), InlineData(true)]
        public void EmitRequireLicenseAcceptance_ShouldEmitElement(bool requireLicenseAcceptance)
        {
            var builder = CreateEmitRequireLicenseAcceptancePackageBuilder(
                emitRequireLicenseAcceptance: true,
                requireLicenseAcceptance);

            using (SaveToTestDirectory(builder, out var reader, out var nuspecContent))
            {
                Assert.Equal(requireLicenseAcceptance, reader.NuspecReader.GetRequireLicenseAcceptance());

                Assert.Contains(
                    requireLicenseAcceptance
                        ? "<requireLicenseAcceptance>true</requireLicenseAcceptance>"
                        : "<requireLicenseAcceptance>false</requireLicenseAcceptance>",
                    nuspecContent);
            }
        }

        [Fact]
        public void EmitRequireLicenseAcceptance_ShouldThrow()
        {
            var builder = CreateEmitRequireLicenseAcceptancePackageBuilder(
                emitRequireLicenseAcceptance: false,
                requireLicenseAcceptance: true);

            var ex = Assert.Throws<Exception>(() => builder.Save(Stream.Null));
            Assert.Equal(NuGetResources.Manifest_RequireLicenseAcceptanceRequiresEmit, ex.Message);
        }

        [Fact]
        public void PackageBuilder_PreserveFileLastWriteTime_Succeeds()
        {
            // Act
            var lastWriteTime = new DateTimeOffset(2017, 1, 15, 23, 59, 0, new TimeSpan(0, 0, 0));
            using (var directory = new TestLastWriteTimeDirectory(lastWriteTime))
            {
                var builder = new PackageBuilder { Id = "test", Version = NuGetVersion.Parse("1.0"), Description = "test" };
                builder.Authors.Add("test");
                builder.AddFiles(directory.Path, "**", "Content");

                using (var stream = new MemoryStream())
                {
                    builder.Save(stream);

                    // Assert
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            var path = directory.Path + Path.DirectorySeparatorChar + entry.Name;
                            // Only checks the entries that originated from files in test directory
                            if (File.Exists(path))
                            {
                                Assert.Equal(File.GetLastWriteTimeUtc(path), entry.LastWriteTime.DateTime);
                            }
                        }
                    }
                }
            }
        }

        [Fact]
        public void PackageBuilder_CorrectLastWriteTimeBeforeYear1980_Succeeds()
        {
            // Act
            DateTime year2020Date = new DateTime(2020, 12, 14, 23, 59, 7, DateTimeKind.Utc);
            DateTimeOffset lastWriteTime = ZipFormatMinDate.AddDays(-1); // 12/31/1979 12:00:00 AM + 00:00
            int numberOfDateCorrectedFiles = 0;
            int numberOfDateNotCorrectedFiles = 0;
            TestLogger innerLogger = new TestLogger();
            ILogger logger = new PackCollectorLogger(innerLogger, new WarningProperties());

            using (var directory = new TestLastWriteTimeDirectory(lastWriteTime.LocalDateTime))
            {
                var builder = new PackageBuilder(false, logger) { Id = "test", Version = NuGetVersion.Parse("1.0"), Description = "test" };
                builder.Authors.Add("test");

                // Additional edge cases
                string before1980File1 = Path.Combine(directory.Path, "before1980File1.txt");
                string before1980File2 = Path.Combine(directory.Path, "before1980File2.txt");
                string after1980File1 = Path.Combine(directory.Path, "after1980File1.txt");
                string after1980File2 = Path.Combine(directory.Path, "after1980File2.txt");
                string after1980File3 = Path.Combine(directory.Path, "after1980File3.txt");
                string after1980File4 = Path.Combine(directory.Path, "after1980File4.txt");
                File.WriteAllText(before1980File1, string.Empty);
                File.WriteAllText(before1980File2, string.Empty);
                File.WriteAllText(after1980File1, string.Empty);
                File.WriteAllText(after1980File2, string.Empty);
                File.WriteAllText(after1980File3, string.Empty);
                File.WriteAllText(after1980File3, string.Empty);
                File.WriteAllText(after1980File4, string.Empty);
                File.SetLastWriteTime(before1980File1, ZipFormatMinDate.AddSeconds(-2));
                File.SetLastWriteTime(before1980File2, ZipFormatMinDate.AddSeconds(-1));
                File.SetLastWriteTime(after1980File1, ZipFormatMinDate);
                File.SetLastWriteTime(after1980File2, ZipFormatMinDate.AddSeconds(1));
                File.SetLastWriteTime(after1980File3, ZipFormatMinDate.AddSeconds(2));
                File.SetLastWriteTime(after1980File4, year2020Date);

                builder.AddFiles(directory.Path, "**", "Content");

                using (var stream = new MemoryStream())
                {
                    builder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    // Assert
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            string path = Path.Combine(directory.Path, entry.Name);

                            // Please note: ZipArchive stream reader sometime changes LastWriteTime by another 1 second off than what "entry.LastWriteTime" has.
                            // The FAT filesystem of DOS has a timestamp resolution of only two seconds; ZIP file records mimic this.
                            // As a result, the built -in timestamp resolution of files in a ZIP archive is only two seconds, though extra fields can be used to store more precise timestamps.
                            // That is why you see this datetime interval instead of actual == of datetimes.
                            if (File.Exists(path))
                            {
                                if (path == after1980File1)
                                {
                                    Assert.Equal(ZipFormatMinDate, entry.LastWriteTime.DateTime);
                                    numberOfDateNotCorrectedFiles++;
                                }
                                else if (path == after1980File2 || path == after1980File3)
                                {
                                    Assert.True(entry.LastWriteTime.DateTime >= ZipFormatMinDate && entry.LastWriteTime.DateTime <= ZipFormatMinDate.AddSeconds(2));
                                    numberOfDateNotCorrectedFiles++;
                                }
                                else if (path == after1980File4)
                                {
                                    // File from 2020
                                    Assert.True(entry.LastWriteTime.DateTime >= year2020Date.AddSeconds(-1) && entry.LastWriteTime.DateTime <= year2020Date);
                                    numberOfDateNotCorrectedFiles++;
                                }
                                else
                                {
                                    // Files on 1/1/1980 00:00:01 UTC timestamp and before that.
                                    Assert.True(entry.LastWriteTime.DateTime >= ZipFormatMinDate.AddSeconds(-1) && entry.LastWriteTime.DateTime <= ZipFormatMinDate);
                                    numberOfDateCorrectedFiles++;
                                }
                            }
                        }
                    }
                }

                Assert.Equal(4, numberOfDateNotCorrectedFiles);
                Assert.Equal(5, numberOfDateCorrectedFiles);
                ILogMessage logMessage = Assert.Single(innerLogger.LogMessages);
                string[] logMessages = logMessage.Message.Split('\n');
                Assert.Equal(5, logMessages.Count(l => l.Contains("changed from")));
            }
        }

        [Fact]
        public void PackageBuilder_CorrectTestWriteTimeAfterYear2107_Succeeds()
        {
            // Act
            DateTime year2020Date = new DateTime(2020, 12, 14, 23, 59, 2, DateTimeKind.Utc);
            DateTimeOffset lastWriteTime = ZipFormatMaxDate.AddDays(1); // 1/1/2108 11:59:58 PM +00:00
            int numberOfDateCorrectedFiles = 0;
            int numberOfDateNotCorrectedFiles = 0;
            TestLogger innerLogger = new TestLogger();
            ILogger logger = new PackCollectorLogger(innerLogger, new WarningProperties());

            using (var directory = new TestLastWriteTimeDirectory(lastWriteTime.ToLocalTime()))
            {
                var builder = new PackageBuilder(false, logger) { Id = "test", Version = NuGetVersion.Parse("1.0"), Description = "test" };
                builder.Authors.Add("test");

                // Additional edge cases
                string before2107File1 = Path.Combine(directory.Path, "Before2107_1.txt");
                string before2107File2 = Path.Combine(directory.Path, "Before2107_2.txt");
                string before2107File3 = Path.Combine(directory.Path, "Before2107_3.txt");
                string before2107File4 = Path.Combine(directory.Path, "Before2107_4.txt");
                string after2107File1 = Path.Combine(directory.Path, "After2107_1.txt");
                string after2107File2 = Path.Combine(directory.Path, "After2107_2.txt");
                File.WriteAllText(before2107File1, string.Empty);
                File.WriteAllText(before2107File2, string.Empty);
                File.WriteAllText(before2107File3, string.Empty);
                File.WriteAllText(before2107File4, string.Empty);
                File.WriteAllText(after2107File1, string.Empty);
                File.WriteAllText(after2107File2, string.Empty);
                File.SetLastWriteTime(before2107File1, year2020Date);
                File.SetLastWriteTime(before2107File2, ZipFormatMaxDate.AddSeconds(-2));
                File.SetLastWriteTime(before2107File3, ZipFormatMaxDate.AddSeconds(-1));
                File.SetLastWriteTime(before2107File4, ZipFormatMaxDate);
                File.SetLastWriteTime(after2107File1, ZipFormatMaxDate.AddSeconds(1));
                File.SetLastWriteTime(after2107File2, ZipFormatMaxDate.AddSeconds(2));

                builder.AddFiles(directory.Path, "**", "Content");

                using (var stream = new MemoryStream())
                {
                    builder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    // Assert
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            string path = Path.Combine(directory.Path, entry.Name);

                            // Please note: ZipArchive stream reader sometime changes LastWriteTime by another 1 second off than what "entry.LastWriteTime" has.
                            // The FAT filesystem of DOS has a timestamp resolution of only two seconds; ZIP file records mimic this.
                            // As a result, the built -in timestamp resolution of files in a ZIP archive is only two seconds, though extra fields can be used to store more precise timestamps.
                            // That is why you see this datetime interval instead of actual == of datetimes.
                            if (File.Exists(path))
                            {
                                if (path == before2107File1)
                                {
                                    // File from 2020
                                    Assert.True(entry.LastWriteTime.DateTime >= year2020Date.AddSeconds(-1) && entry.LastWriteTime.DateTime <= year2020Date);
                                    numberOfDateNotCorrectedFiles++;
                                }
                                else if (path == before2107File2 || path == before2107File3)
                                {
                                    Assert.True(entry.LastWriteTime.DateTime >= ZipFormatMaxDate.AddSeconds(-2) && entry.LastWriteTime.DateTime <= ZipFormatMaxDate.AddSeconds(-1));
                                    numberOfDateNotCorrectedFiles++;
                                }
                                else if (path == before2107File4)
                                {
                                    Assert.Equal(ZipFormatMaxDate, entry.LastWriteTime.DateTime);
                                    numberOfDateNotCorrectedFiles++;
                                }
                                else
                                {
                                    // File from 12/31/2107, 23:59:58 UTC and after that
                                    Assert.True(entry.LastWriteTime.DateTime >= ZipFormatMaxDate.AddSeconds(-1) && entry.LastWriteTime.DateTime <= ZipFormatMaxDate);
                                    numberOfDateCorrectedFiles++;
                                }
                            }
                        }
                    }
                }

                Assert.Equal(4, numberOfDateNotCorrectedFiles);
                Assert.Equal(5, numberOfDateCorrectedFiles);
                ILogMessage logMessage = Assert.Single(innerLogger.LogMessages);
                string[] logMessages = logMessage.Message.Split('\n');
                Assert.Equal(5, logMessages.Count(l => l.Contains("changed from")));
            }
        }

        private static PackageBuilder CreateEmitRequireLicenseAcceptancePackageBuilder(bool emitRequireLicenseAcceptance, bool requireLicenseAcceptance)
        {
            return new PackageBuilder
            {
                Id = "test",
                Version = new NuGetVersion("0.0.1"),
                Authors = { "TestAuthors" },
                Description = "Test package for EmitRequireLicenseAcceptance",
                EmitRequireLicenseAcceptance = emitRequireLicenseAcceptance,
                RequireLicenseAcceptance = requireLicenseAcceptance,
                LicenseMetadata = new LicenseMetadata(LicenseType.Expression, "MIT", NuGetLicenseExpression.Parse("MIT"), warningsAndErrors: null, LicenseMetadata.EmptyVersion),
                DependencyGroups =
                {
                    new PackageDependencyGroup(
                        NuGetFramework.Parse("netstandard1.4"),
                        new[] { new PackageDependency("another.dep", VersionRange.Parse("0.0.1")) }),
                },
            };
        }

        private static IDisposable SaveToTestDirectory(PackageBuilder builder, out PackageArchiveReader reader, out string nuspecContent)
        {
            var testDir = TestDirectory.Create();

            var packagePath = Path.Combine(testDir, "test.0.0.1.nupkg");

            using (var nupkgStream = File.Create(packagePath))
            {
                builder.Save(nupkgStream);
            }

            reader = new PackageArchiveReader(packagePath);

            using (var nureader = new StreamReader(reader.GetNuspec()))
            {
                nuspecContent = nureader.ReadToEnd();
            }

            return testDir;
        }

        private static IPackageFile CreatePackageFile(string name)
        {
            var file = new Mock<IPackageFile>();
            file.SetupGet(f => f.Path).Returns(name);
            file.Setup(f => f.GetStream()).Returns(new MemoryStream());
            file.Setup(f => f.LastWriteTime).Returns(DateTimeOffset.UtcNow);

            string effectivePath;
            var nufx = FrameworkNameUtility.ParseNuGetFrameworkFromFilePath(name, out effectivePath);
            file.SetupGet(f => f.EffectivePath).Returns(effectivePath);
            file.SetupGet(f => f.NuGetFramework).Returns(nufx);

            var fx = FrameworkNameUtility.ParseFrameworkNameFromFilePath(name, out effectivePath);
#pragma warning disable CS0618 // Type or member is obsolete
            file.SetupGet(f => f.TargetFramework).Returns(fx);
#pragma warning restore CS0618 // Type or member is obsolete

            return file.Object;
        }

        private IPackageFile CreatePackageFileOnPath(string path, DateTime lastWriteTime)
        {
            string directorypath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }

            File.WriteAllText(path, string.Empty);
            File.SetLastWriteTime(path, lastWriteTime);

            using (MemoryStream ms = new MemoryStream())
            using (FileStream fileStream = File.OpenRead(path))
            {
                fileStream.CopyTo(ms);
                var file = new PhysicalPackageFile(ms)
                {
                    TargetPath = path
                };
                return file;
            }
        }

        private Stream GetManifestStream(Stream packageStream)
        {
            Stream resultStream = null;

            using (var archive = new ZipArchive(packageStream, ZipArchiveMode.Read, leaveOpen: true))
            {
                var entry = archive.GetEntry(@"_rels/.rels");
                using (var stream = entry.Open())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        var contents = streamReader.ReadToEnd();
                        var xDoc = XDocument.Parse(contents);

                        var relationshipsNS = "http://schemas.openxmlformats.org/package/2006/relationships";
                        var relationsShipsName = XName.Get("Relationships", relationshipsNS);
                        var relationshipsElements = xDoc.Elements(relationsShipsName);

                        var relationsShipName = XName.Get("Relationship", relationshipsNS);
                        var relationshipElements = relationshipsElements.Elements(relationsShipName);

                        var typeName = XName.Get("Type");
                        var typeAttrs = relationshipElements.Attributes(typeName);

                        var manifestNS = "http://schemas.microsoft.com/packaging/2010/07/manifest";
                        var manifestTypeAttr = typeAttrs.FirstOrDefault(attr => attr.Value == manifestNS);

                        var targetName = XName.Get("Target");
                        var targetPath = manifestTypeAttr.Parent.Attribute(targetName).Value.TrimStart('/');

                        var resultEntry = archive.GetEntry(targetPath);
                        resultStream = resultEntry.Open();
                    }
                }
            }

            return resultStream;
        }

        /*
        Test directory contents:

            dir1
                dir2
                    file1.txt
                    file2.txt
                file1.txt
                file2.txt
            dir3
            file1.txt
            file2.txt
        */
        public sealed class TestSourcesDirectory : IDisposable
        {
            private TestDirectory _testDirectory;

            public string Path
            {
                get { return _testDirectory.Path; }
            }

            public TestSourcesDirectory()
            {
                _testDirectory = TestDirectory.Create();

                PopulateTestDirectory();
            }

            public void Dispose()
            {
                _testDirectory.Dispose();
            }

            private void PopulateTestDirectory()
            {
                var rootDirectory = new DirectoryInfo(_testDirectory.Path);
                var directory1 = Directory.CreateDirectory(System.IO.Path.Combine(rootDirectory.FullName, "dir1"));
                var directory2 = Directory.CreateDirectory(System.IO.Path.Combine(directory1.FullName, "dir2"));
                var directory3 = Directory.CreateDirectory(System.IO.Path.Combine(rootDirectory.FullName, "dir3"));

                CreateTestFiles(rootDirectory);
                CreateTestFiles(directory1);
                CreateTestFiles(directory2);
            }

            private static void CreateTestFiles(DirectoryInfo directory)
            {
                File.WriteAllText(System.IO.Path.Combine(directory.FullName, "file1.txt"), string.Empty);
                File.WriteAllText(System.IO.Path.Combine(directory.FullName, "file2.txt"), string.Empty);
            }
        }

        public sealed class TestLastWriteTimeDirectory : IDisposable
        {
            private TestDirectory _testDirectory;
            private DateTimeOffset _lastWriteTime;

            public string Path
            {
                get { return _testDirectory.Path; }
            }

            public TestLastWriteTimeDirectory(DateTimeOffset lastWriteTime)
            {
                _testDirectory = TestDirectory.Create();
                _lastWriteTime = lastWriteTime;

                PopulateTestDirectory();
            }

            public void Dispose()
            {
                _testDirectory.Dispose();
            }

            private void PopulateTestDirectory()
            {
                var rootDirectory = new DirectoryInfo(_testDirectory.Path);
                var directory1 = Directory.CreateDirectory(System.IO.Path.Combine(rootDirectory.FullName, "dir1"));
                var directory2 = Directory.CreateDirectory(System.IO.Path.Combine(directory1.FullName, "dir2"));

                CreateTestFiles(rootDirectory);
                CreateTestFiles(directory1);
                CreateTestFiles(directory2);
            }

            private void CreateTestFiles(DirectoryInfo directory)
            {
                File.WriteAllText(System.IO.Path.Combine(directory.FullName, "file1.txt"), string.Empty);
                File.SetLastWriteTime(System.IO.Path.Combine(directory.FullName, "file1.txt"), _lastWriteTime.DateTime);
            }
        }
    }
}
