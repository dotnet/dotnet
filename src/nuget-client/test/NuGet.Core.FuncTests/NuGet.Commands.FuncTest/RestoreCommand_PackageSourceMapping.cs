// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Commands.Test;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Protocol.Test;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility.Commands;
using Xunit;

namespace NuGet.Commands.FuncTest
{
    [Collection(TestCollection.Name)]
    public class RestoreCommand_PackageSourceMapping
    {
        [Fact]
        public async Task RestoreCommand_WithPackageNamesacesConfiguredDownloadsPackageFromExpectedSource_Succeeds()
        {
            using var pathContext = new SimpleTestPathContext();

            const string packageA = "PackageA";
            const string packageB = "PackageB";
            const string version = "1.0.0";

            var packageA100 = new SimpleTestPackageContext
            {
                Id = packageA,
                Version = version,
            };

            var packageB100 = new SimpleTestPackageContext
            {
                Id = packageB,
                Version = version,
            };

            var projectSpec = PackageReferenceSpecBuilder.Create("Library1", pathContext.SolutionRoot)
            .WithTargetFrameworks(new[]
            {
                new TargetFrameworkInformation
                {
                    FrameworkName = NuGetFramework.Parse("net5.0"),
                    Dependencies = [
                        new LibraryDependency
                        {
                            LibraryRange = new LibraryRange(packageA, VersionRange.Parse(version),
                                LibraryDependencyTarget.Package)
                        },
                        new LibraryDependency
                        {
                            LibraryRange = new LibraryRange(packageB, VersionRange.Parse(version),
                                LibraryDependencyTarget.Package)
                        },
                    ]
                }
            })
            .Build();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA100,
                packageB100);

            var packageSource2 = Path.Combine(pathContext.WorkingDirectory, "source2");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                packageSource2,
                PackageSaveMode.Defaultv3,
                packageA100,
                packageB100);

            var log = new TestLogger();

            PackageSource[] sources = [new PackageSource(pathContext.PackageSource), new PackageSource(packageSource2)];

            //package source mapping configuration
            Dictionary<string, IReadOnlyList<string>> patterns = new();
            patterns.Add(packageSource2, new List<string>() { packageA });
            patterns.Add(pathContext.PackageSource, new List<string>() { packageB });
            PackageSourceMapping sourceMappingConfiguration = new(patterns);

            var request = new TestRestoreRequest(projectSpec,
                sources,
                pathContext.UserPackagesFolder,
                new TestSourceCacheContext(),
                sourceMappingConfiguration,
                log);

            var command = new RestoreCommand(request);
            var result = await command.ExecuteAsync();

            Assert.True(result.Success);

            var restoreGraph = result.RestoreGraphs.ElementAt(0);
            Assert.Equal(0, restoreGraph.Unresolved.Count);
            //packageA should be installed from source2
            restoreGraph.Install.Count.Should().Be(2);

            foreach (RemoteMatch match in restoreGraph.Install)
            {
                if (match.Library.Name == packageA)
                {
                    match.Provider.Source.Name.Should().Be(packageSource2);
                }
                else if (match.Library.Name == packageB)
                {
                    match.Provider.Source.Name.Should().Be(pathContext.PackageSource);
                }
            }
        }

        [Fact]
        public async Task RestoreCommand_WithPackageNamespacesConfiguredAndNoMatchingSourceForAPackage_Fails()
        {
            using var pathContext = new SimpleTestPathContext();

            const string packageA = "PackageA";
            const string packageB = "PackageB";
            const string version = "1.0.0";

            var packageA100 = new SimpleTestPackageContext
            {
                Id = packageA,
                Version = version,
            };

            var packageB100 = new SimpleTestPackageContext
            {
                Id = packageB,
                Version = version,
            };

            var projectSpec = PackageReferenceSpecBuilder.Create("Library1", pathContext.SolutionRoot)
            .WithTargetFrameworks(new[]
            {
                new TargetFrameworkInformation
                {
                    FrameworkName = NuGetFramework.Parse("net5.0"),
                    Dependencies = [
                        new LibraryDependency
                        {
                            LibraryRange = new LibraryRange(packageA, VersionRange.Parse(version),
                                LibraryDependencyTarget.Package)
                        },
                        new LibraryDependency
                        {
                            LibraryRange = new LibraryRange(packageB, VersionRange.Parse(version),
                                LibraryDependencyTarget.Package)
                        },
                    ]
                }
            })
            .Build();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA100,
                packageB100);

            var packageSource2 = Path.Combine(pathContext.WorkingDirectory, "source2");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                packageSource2,
                PackageSaveMode.Defaultv3,
                packageA100,
                packageB100);

            var sources = new[] { new PackageSource(pathContext.PackageSource),
                                                   new PackageSource(packageSource2) };
            var log = new TestLogger();

            //package source mapping configuration
            Dictionary<string, IReadOnlyList<string>> patterns = new();
            patterns.Add(packageSource2, new List<string>() { packageA });
            PackageSourceMapping sourceMappingConfiguration = new(patterns);

            var request = new TestRestoreRequest(projectSpec,
                sources,
                pathContext.UserPackagesFolder,
                new TestSourceCacheContext(),
                sourceMappingConfiguration,
                log);

            var command = new RestoreCommand(request);
            var result = await command.ExecuteAsync();

            Assert.False(result.Success);

            var restoreGraph = result.RestoreGraphs.ElementAt(0);
            Assert.Equal(1, restoreGraph.Unresolved.Count);

            Assert.Equal(1, log.Errors);
            log.ErrorMessages.TryPeek(out string message);
            Assert.Equal($"NU1100: Unable to resolve 'PackageB (>= 1.0.0)' for 'net5.0'. PackageSourceMapping is enabled, the following source(s) were not considered: {pathContext.PackageSource}, {packageSource2}.", message);

            //packageA should be installed from source2
            string packageASource = restoreGraph.Install.ElementAt(0).Provider.Source.Name;
            Assert.Equal(packageSource2, packageASource);
        }

        [Theory]
        [InlineData("Source2", "source2")]
        [InlineData("source2", "SouRce2")]
        public async Task RestoreCommand_WithPSMConfiguredWithDifferentCasing_DownloadsPackageFromExpectedSource_Succeeds(string packageSourceMappingKey, string packageSourcesKey)
        {
            using var pathContext = new SimpleTestPathContext();

            const string packageA = "PackageA";
            const string packageB = "PackageB";
            const string version = "1.0.0";

            var packageA100 = new SimpleTestPackageContext(packageA, version);
            var packageB100 = new SimpleTestPackageContext(packageB, version);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA100,
                packageB100);

            var packageSource2Path = Path.Combine(pathContext.WorkingDirectory, "additional-source");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                packageSource2Path,
                PackageSaveMode.Defaultv3,
                packageA100,
                packageB100);

            pathContext.Settings.AddSource(packageSourcesKey, packageSource2Path);
            pathContext.Settings.AddPackageSourceMapping(packageSourceMappingKey, packageA);
            pathContext.Settings.AddPackageSourceMapping(SimpleTestSettingsContext.DefaultPackageSourceName, packageB);

            pathContext.Settings.Save();

            var rootProject = @"
        {
          ""frameworks"": {
            ""net5.0"": {
                ""dependencies"": {
                        ""PackageA"": {
                            ""version"": ""[1.0.0,)"",
                            ""target"": ""Package"",
                        },
                        ""PackageB"": {
                            ""version"": ""[1.0.0,)"",
                            ""target"": ""Package"",
                        }
                },
            }
          }
        }";

            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Library1", pathContext.SolutionRoot, rootProject)
                .WithSettingsBasedRestoreMetadata(Settings.LoadDefaultSettings(pathContext.SolutionRoot));

            var result = await RunRestoreAsync(pathContext, projectSpec);

            result.Success.Should().BeTrue(because: string.Join(Environment.NewLine, result.LockFile.LogMessages.Select(e => e.ToString())));

            var restoreGraph = result.RestoreGraphs.ElementAt(0);
            restoreGraph.Unresolved.Should().BeEmpty();
            //packageA should be installed from source2
            restoreGraph.Install.Count.Should().Be(2);

            foreach (RemoteMatch match in restoreGraph.Install)
            {
                if (match.Library.Name == packageA)
                {
                    match.Provider.Source.Name.Should().Be(packageSourcesKey);
                }
                else if (match.Library.Name == packageB)
                {
                    match.Provider.Source.Name.Should().Be(SimpleTestSettingsContext.DefaultPackageSourceName);
                }
            }
        }

        internal static Task<RestoreResult> RunRestoreAsync(SimpleTestPathContext pathContext, params PackageSpec[] projects)
        {
            return new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, new TestLogger(), projects)).ExecuteAsync();
        }
    }
}
