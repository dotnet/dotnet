// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class ContentFilesMSBuildTests
    {
        [Fact]
        public async Task ContentFilesMSBuild_VerifyNoContentItemsForEmptyFolderAsync()
        {
            // Arrange
            var logger = new TestLogger();

            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var tfi = new List<TargetFrameworkInformation>
                {
                    new TargetFrameworkInformation()
                    {
                        FrameworkName = NuGetFramework.Parse("net462")
                    }
                };

                var spec = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "net46");
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("a", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var project = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, spec).Single();

                var packageA = new SimpleTestPackageContext("a");
                packageA.AddFile("contentFiles/any/any/_._");
                packageA.AddFile("contentFiles/cs/net45/_._");
                packageA.AddFile("contentFiles/cs/any/_._");

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageA);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var result = (await NETCoreRestoreTestUtility.RunRestore(
                    pathContext,
                    logger,
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    dgFile,
                    cacheContext)).Single();

                var props = XDocument.Load(project.PropsOutput);
                var itemGroups = props.Root.Elements(XName.Get("ItemGroup", "http://schemas.microsoft.com/developer/msbuild/2003")).ToArray();

                // Assert
                Assert.True(result.Success, logger.ShowErrors());
                Assert.Equal(1, itemGroups.Length); // The SourceRoot is the only expected item group.
            }
        }

        [Theory]
        [InlineData("contentFiles/any/any/x.txt", "'$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/cs/any/x.txt", "'$(Language)' == 'C#' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/fs/any/x.txt", "'$(Language)' == 'F#' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/vb/any/x.txt", "'$(Language)' == 'VB' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/ZzZ/any/x.txt", "'$(Language)' == 'ZZZ' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/1/any/x.txt", "'$(Language)' == '1' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        public async Task ContentFilesMSBuild_VerifyConditionForContentItemGroupWithoutCrossTargetingAsync(string file, string expected)
        {
            // Arrange
            var logger = new TestLogger();

            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var tfi = new List<TargetFrameworkInformation>
                {
                    new TargetFrameworkInformation()
                    {
                        FrameworkName = NuGetFramework.Parse("net462")
                    }
                };

                var spec = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "net46");
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("a", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var project = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, spec).Single();

                var packageA = new SimpleTestPackageContext("a");
                packageA.AddFile(file);

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageA);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var result = (await NETCoreRestoreTestUtility.RunRestore(
                    pathContext,
                    logger,
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    dgFile,
                    cacheContext)).Single();

                var props = XDocument.Load(project.PropsOutput);
                var itemGroups = props.Root.Elements(XName.Get("ItemGroup", "http://schemas.microsoft.com/developer/msbuild/2003")).ToArray();

                // Assert
                Assert.True(result.Success, logger.ShowErrors());
                Assert.Equal(2, itemGroups.Length); // SourceRoot is the first item group.
                Assert.EndsWith("x.txt", Path.GetFileName(itemGroups[1].Elements().Single().Attribute(XName.Get("Include")).Value));
                Assert.Equal(expected.Trim(), itemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());
            }
        }

        [Theory]
        [InlineData("contentFiles/any/any/x.txt", "'$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/cs/any/x.txt", "'$(Language)' != 'C#' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/cs/any/x.txt|contentFiles/cs/any/y.txt", "'$(Language)' != 'C#' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/cs/any/x.txt|contentFiles/fs/any/x.txt", "'$(Language)' != 'C#' AND '$(Language)' != 'F#' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        [InlineData("contentFiles/zz/any/x.txt|contentFiles/xx/any/x.txt|contentFiles/yy/any/x.txt", "'$(Language)' != 'XX' AND '$(Language)' != 'YY' AND '$(Language)' != 'ZZ' AND '$(ExcludeRestorePackageImports)' != 'true'")]
        public async Task ContentFilesMSBuild_VerifyConditionForFallbackContentItemGroupAsync(string files, string expected)
        {
            // Arrange
            var logger = new TestLogger();

            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var tfi = new List<TargetFrameworkInformation>
                {
                    new TargetFrameworkInformation()
                    {
                        FrameworkName = NuGetFramework.Parse("net462")
                    }
                };

                var spec = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "net46");
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("a", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var project = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, spec).Single();

                var packageA = new SimpleTestPackageContext("a");
                packageA.AddFile("contentFiles/any/any/anyMarker.txt");

                foreach (var file in files.Split('|'))
                {
                    packageA.AddFile(file);
                }

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageA);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var result = (await NETCoreRestoreTestUtility.RunRestore(
                    pathContext,
                    logger,
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    dgFile,
                    cacheContext)).Single();

                var props = XDocument.Load(project.PropsOutput);
                var itemGroups = props.Root.Elements(XName.Get("ItemGroup", "http://schemas.microsoft.com/developer/msbuild/2003")).ToArray();
                var group = itemGroups.Single(e => e.ToString().Contains("anyMarker.txt"));

                // Assert
                Assert.True(result.Success, logger.ShowErrors());
                Assert.Equal(expected.Trim(), group.Attribute(XName.Get("Condition")).Value.Trim());
            }
        }
    }
}
