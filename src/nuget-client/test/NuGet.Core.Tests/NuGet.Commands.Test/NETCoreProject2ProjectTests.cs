// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class NETCoreProject2ProjectTests
    {
        [Theory]
        [InlineData("lib/netstandard1.6/b.dll", "bin/debug/b.dll")]
        [InlineData("lib/netstandard1.3/b.dll", "bin/debug/b.dll")]
        [InlineData("LIBANY", "bin/debug/b.dll")]
        [InlineData("lib/netstandard1.7/b.dll", "")]
        [InlineData("lib/net45/a.dll", "")]
        [InlineData("build/projectB.targets", "")]
        [InlineData("unknown/a.dll", "")]
        public async Task NETCoreProject2Project_VerifyLibFilesUnderCompile(string path, string expected)
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                var spec2 = NETCoreRestoreTestUtility.GetProject(projectName: "projectB", framework: "netstandard1.3");

                var specs = new[] { spec1, spec2 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // Link projects
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[1].ProjectPath,
                    ProjectUniqueName = spec2.RestoreMetadata.ProjectUniqueName,
                });

                var projectDir = Path.GetDirectoryName(spec2.RestoreMetadata.ProjectPath);
                var absolutePath = Path.Combine(projectDir, "bin", "debug", "b.dll");
                spec2.RestoreMetadata.Files.Add(new ProjectRestoreMetadataFile(path, Path.Combine(projectDir, absolutePath)));

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                dgFile.AddProject(spec1);
                dgFile.AddProject(spec2);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var lockFormat = new LockFileFormat();

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                var assetsFile = lockFormat.Read(Path.Combine(spec1.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName));

                var projectBTarget = assetsFile.Targets.Single().Libraries.Single(e => e.Type == "project");

                // Verify compile and runtime
                Assert.Equal(expected, string.Join("|", projectBTarget.CompileTimeAssemblies.Select(e => e.Path)));
                Assert.Equal(expected, string.Join("|", projectBTarget.RuntimeAssemblies.Select(e => e.Path)));
            }
        }

        /// <summary>
        /// Project graph:
        /// A -> B -> C -> D
        ///   -> X -> Y -> D
        ///   
        /// Restore A with various
        /// combinations of transitive edges.
        /// 
        /// Verify the compile assets returned to A.
        /// Verify all runtime assets are returned to A.
        /// </summary>
        [Theory]
        // Flow everything
        [InlineData("BCDXY", true, true, true, true, true, true)]
        // All transitive off
        [InlineData("BX", false, false, false, false, false, false)]
        // AB, BX has no impact on A
        [InlineData("BCDXY", false, true, true, true, true, true)]
        // BC off, D flows through X,Y
        [InlineData("BDXY", true, false, true, true, true, true)]
        // XY off, D flows through BC
        [InlineData("BCDX", true, true, true, true, false, true)]
        // BC off, XY off, D stops
        [InlineData("BX", true, false, true, true, false, true)]
        public async Task NETCoreProject2Project_VerifyCompileForTransitiveSettings(string expected, bool ab, bool bc, bool cd, bool ax, bool xy, bool yd)
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "A", framework: "netstandard1.6");
                var spec2 = NETCoreRestoreTestUtility.GetProject(projectName: "B", framework: "netstandard1.6");
                var spec3 = NETCoreRestoreTestUtility.GetProject(projectName: "C", framework: "netstandard1.6");
                var spec4 = NETCoreRestoreTestUtility.GetProject(projectName: "D", framework: "netstandard1.6");
                var spec5 = NETCoreRestoreTestUtility.GetProject(projectName: "X", framework: "netstandard1.6");
                var spec6 = NETCoreRestoreTestUtility.GetProject(projectName: "Y", framework: "netstandard1.6");

                var specs = new[] { spec1, spec2, spec3, spec4, spec5, spec6, };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // Link projects
                // A -> B
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[1].ProjectPath,
                    ProjectUniqueName = spec2.RestoreMetadata.ProjectUniqueName,
                    PrivateAssets = ab ? LibraryIncludeFlags.None : LibraryIncludeFlags.Compile
                });

                // B -> C
                spec2.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[2].ProjectPath,
                    ProjectUniqueName = spec3.RestoreMetadata.ProjectUniqueName,
                    PrivateAssets = bc ? LibraryIncludeFlags.None : LibraryIncludeFlags.Compile
                });

                // C -> D
                spec3.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[3].ProjectPath,
                    ProjectUniqueName = spec4.RestoreMetadata.ProjectUniqueName,
                    PrivateAssets = cd ? LibraryIncludeFlags.None : LibraryIncludeFlags.Compile
                });

                // A -> X
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[4].ProjectPath,
                    ProjectUniqueName = spec5.RestoreMetadata.ProjectUniqueName,
                    PrivateAssets = ax ? LibraryIncludeFlags.None : LibraryIncludeFlags.Compile
                });

                // X -> Y
                spec5.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[5].ProjectPath,
                    ProjectUniqueName = spec6.RestoreMetadata.ProjectUniqueName,
                    PrivateAssets = xy ? LibraryIncludeFlags.None : LibraryIncludeFlags.Compile
                });

                // Y -> D
                spec6.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[3].ProjectPath,
                    ProjectUniqueName = spec4.RestoreMetadata.ProjectUniqueName,
                    PrivateAssets = yd ? LibraryIncludeFlags.None : LibraryIncludeFlags.Compile
                });

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                foreach (var spec in specs)
                {
                    dgFile.AddProject(spec);

                    var projectDir = Path.GetDirectoryName(spec.RestoreMetadata.ProjectPath);
                    var absolutePath = Path.Combine(projectDir, "bin", "debug", "a.dll");
                    spec.RestoreMetadata.Files.Add(new ProjectRestoreMetadataFile("lib/netstandard1.6/a.dll", Path.Combine(projectDir, absolutePath)));
                }

                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var lockFormat = new LockFileFormat();

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                var assetsFile = lockFormat.Read(Path.Combine(spec1.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName));

                // Find all non _._ compile assets
                var flowingCompile = assetsFile.Targets.Single().Libraries
                    .Where(e => e.Type == "project")
                    .Where(e => e.CompileTimeAssemblies.Any(f => !f.Path.EndsWith("_._")))
                    .Select(e => e.Name)
                    .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);

                Assert.Equal(expected, string.Join("", flowingCompile));

                // Runtime should always flow
                var flowingRuntime = assetsFile.Targets.Single().Libraries
                    .Where(e => e.Type == "project")
                    .Where(e => e.RuntimeAssemblies.Any(f => !f.Path.EndsWith("_._")))
                    .Select(e => e.Name)
                    .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);

                Assert.Equal("BCDXY", string.Join("", flowingRuntime));
            }
        }

        [Fact]
        public async Task NETCoreProject2Project_IgnoreXproj()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));

                var spec = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                var specs = new[] { spec };

                var newDependencies = spec.TargetFrameworks.Single().Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });
                spec.TargetFrameworks[0] = new TargetFrameworkInformation(spec.TargetFrameworks[0]) { Dependencies = newDependencies };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, spec);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                packageX.Dependencies.Add(packageY);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                // Only add projectA
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var projectYRoot = Path.Combine(pathContext.SolutionRoot, "y");
                Directory.CreateDirectory(projectYRoot);
                var projectYJson = Path.Combine(projectYRoot, "project.json");

                var projectJsonContent = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'netstandard1.0': {
                                                    }
                                                  }
                                               }");

                File.WriteAllText(projectYJson, projectJsonContent.ToString());

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                // Verify only packages
                Assert.DoesNotContain(projects[0].AssetsFile.Libraries, e => e.Type != "package");
            }
        }

        [Fact]
        public async Task NETCoreProject2Project_ProjectReferenceOnlyUnderRestoreMetadata()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                var spec2 = NETCoreRestoreTestUtility.GetProject(projectName: "projectB", framework: "netstandard1.3");

                var specs = new[] { spec1, spec2 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // Link projects
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[1].ProjectPath,
                    ProjectUniqueName = spec2.RestoreMetadata.ProjectUniqueName,
                });

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                foreach (var spec in specs)
                {
                    dgFile.AddProject(spec);
                }

                // Restore only the first one
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                var targetLib = projects[0].AssetsFile
                    .Targets
                    .Single(e => e.TargetFramework.Equals(NuGetFramework.Parse("netstandard1.6")))
                    .Libraries
                    .Single(e => e.Name == "projectB");

                var libraryLib = projects[0].AssetsFile
                    .Libraries
                    .Single(e => e.Name == "projectB");

                Assert.Equal("projectB", targetLib.Name);
                Assert.Equal(NuGetFramework.Parse("netstandard1.3"), NuGetFramework.Parse(targetLib.Framework));
                Assert.Equal("1.0.0", targetLib.Version.ToNormalizedString());
                Assert.Equal("project", targetLib.Type);

                Assert.Equal("projectB", libraryLib.Name);
                Assert.Equal("project", libraryLib.Type);
                Assert.Equal("../projectB/projectB.csproj", libraryLib.MSBuildProject);
                Assert.Equal("../projectB/projectB.csproj", libraryLib.Path);
                Assert.Equal("1.0.0", libraryLib.Version.ToNormalizedString());
            }
        }

        [Fact]
        public async Task NETCoreProject2Project_ProjectReference_IgnoredForTFM()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                var spec2 = NETCoreRestoreTestUtility.GetProject(projectName: "projectB", framework: "netstandard1.3");

                var specs = new[] { spec1, spec2 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // Remove valid link
                spec1.RestoreMetadata.TargetFrameworks.Clear();

                // Add invalid link, net45 is not a project tfm
                spec1.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45")));
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[1].ProjectPath,
                    ProjectUniqueName = spec2.RestoreMetadata.ProjectUniqueName
                });

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                foreach (var spec in specs)
                {
                    dgFile.AddProject(spec);
                }

                // Restore only the first one
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.Equal(0, projects[0].AssetsFile.Libraries.Count);
            }
        }

        [Fact]
        public async Task NETCoreProject2Project_ProjectMissing()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                var spec2 = NETCoreRestoreTestUtility.GetProject(projectName: "projectB", framework: "netstandard1.3");

                var specs = new[] { spec1, spec2 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // Link projects
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[1].ProjectPath,
                    ProjectUniqueName = spec2.RestoreMetadata.ProjectUniqueName,
                });

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                // Only add projectA
                dgFile.AddProject(spec1);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.False(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.Contains("Unable to find project", string.Join(Environment.NewLine, logger.Messages));
                Assert.Contains(projects[1].ProjectPath, string.Join(Environment.NewLine, logger.Messages));
            }
        }

        [Fact]
        public async Task NETCoreProject2Project_ProjectFileDependencyGroupsForNETCore()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));

                var spec1 = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "netstandard1.6");
                var spec2 = NETCoreRestoreTestUtility.GetProject(projectName: "projectB", framework: "netstandard1.3");
                var spec3 = NETCoreRestoreTestUtility.GetProject(projectName: "projectC", framework: "netstandard1.0");

                var specs = new[] { spec1, spec2, spec3 };

                // Create fake projects, the real data is in the specs
                var projects = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, specs);

                // Link projects
                spec1.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[1].ProjectPath,
                    ProjectUniqueName = spec2.RestoreMetadata.ProjectUniqueName,
                });

                spec2.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Add(new ProjectRestoreReference()
                {
                    ProjectPath = projects[2].ProjectPath,
                    ProjectUniqueName = spec3.RestoreMetadata.ProjectUniqueName,
                });

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                foreach (var spec in specs)
                {
                    dgFile.AddProject(spec);
                }

                // Restore only the first one
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                // Act
                var summaries = await NETCoreRestoreTestUtility.RunRestore(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                var dependencies = projects[0].AssetsFile.ProjectFileDependencyGroups.SelectMany(e => e.Dependencies).ToArray();

                // Ensure ProjectC does not show up
                Assert.Equal(1, dependencies.Length);

                // Ensure ProjectC is in the libraries
                Assert.Equal(2, projects[0].AssetsFile.Libraries.Count);

                // Verify the project name is used not the path or unique name
                Assert.Equal("projectB >= 1.0.0", dependencies[0]);
            }
        }
    }
}
