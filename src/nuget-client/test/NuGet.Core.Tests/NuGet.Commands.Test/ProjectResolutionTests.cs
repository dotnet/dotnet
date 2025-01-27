// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class ProjectResolutionTests
    {
        [Fact]
        public async Task ProjectResolution_ProjectFrameworkAssemblyReferences()
        {
            // Arrange
            var project1Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            var project2Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                   ""frameworkAssemblies"": {
                     ""ReferenceA"": """",
                     ""ReferenceB"": """",
                   },
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1);
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project2Json, "project2", specPath2);

                var logger = new TestLogger();

                var restoreContext = new RestoreArgs()
                {
                    Sources = new List<string>() { packageSource.FullName },
                    GlobalPackagesFolder = packagesDir.FullName,
                    Log = logger,
                    CacheContext = new SourceCacheContext()
                };

                // Modify specs for netcore
                spec2 = spec2.WithTestRestoreMetadata();
                spec1 = spec1.WithTestRestoreMetadata().WithTestProjectReference(spec2);

                var request = await ProjectTestHelpers.GetRequestAsync(restoreContext, spec1, spec2);

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse("net45"), null);
                var project2Lib = target.Libraries.Single(lib => lib.Name == "project2");
                var frameworkReferences = project2Lib.FrameworkAssemblies;

                // Assert
                Assert.True(result.Success);
                Assert.Equal(2, frameworkReferences.Count);
                Assert.Equal("ReferenceA", frameworkReferences[0]);
                Assert.Equal("ReferenceB", frameworkReferences[1]);
            }
        }

        [Fact]
        public async Task ProjectResolution_ProjectReferenceWithInCompatibleTFM()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            var project2Json = @"
            {
              ""version"": ""2.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net46"": {
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1);
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project2Json, "project2", specPath2);

                var logger = new TestLogger();

                var restoreContext = new RestoreArgs()
                {
                    Sources = new List<string>() { packageSource.FullName },
                    GlobalPackagesFolder = packagesDir.FullName,
                    Log = logger,
                    CacheContext = new SourceCacheContext()
                };

                // Modify specs for netcore
                spec2 = spec2.WithTestRestoreMetadata();
                spec1 = spec1.WithTestRestoreMetadata().WithTestProjectReference(spec2);

                var request = await ProjectTestHelpers.GetRequestAsync(restoreContext, spec1, spec2);

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetGraph = lockFile.GetTarget(NuGetFramework.Parse("net45"), null);
                var project2Lib = targetGraph.Libraries.FirstOrDefault(lib => lib.Name == "project2");

                var issue = result.CompatibilityCheckResults.SelectMany(ccr => ccr.Issues).Single();

                // Assert
                Assert.False(result.Success);
                Assert.Equal(0, result.GetAllUnresolved().Count);
                Assert.NotNull(project2Lib);
                Assert.Equal("Project project2 is not compatible with net45 (.NETFramework,Version=v4.5). Project project2 supports: net46 (.NETFramework,Version=v4.6)", issue.Format());
                Assert.Equal(1, logger.ErrorMessages.Count());

                Assert.Contains("Project project2 is not compatible with net45 (.NETFramework,Version=v4.5). Project project2 supports: net46 (.NETFramework,Version=v4.6)", logger.ErrorMessages.Last());

                // Incompatible projects are marked as Unsupported
                Assert.Equal(NuGetFramework.UnsupportedFramework.DotNetFrameworkName, project2Lib.Framework);
            }
        }

        [Fact]
        public async Task ProjectResolution_ProjectReferenceWithInCompatibleTFM_Multiple()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            var project2Json = @"
            {
              ""version"": ""2.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net46"": { },
                ""win81"": { }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1);
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project2Json, "project2", specPath2);

                var logger = new TestLogger();

                var restoreContext = new RestoreArgs()
                {
                    Sources = new List<string>() { packageSource.FullName },
                    GlobalPackagesFolder = packagesDir.FullName,
                    Log = logger,
                    CacheContext = new SourceCacheContext()
                };

                // Modify specs for netcore
                spec2 = spec2.WithTestRestoreMetadata();
                spec1 = spec1.WithTestRestoreMetadata().WithTestProjectReference(spec2);

                var request = await ProjectTestHelpers.GetRequestAsync(restoreContext, spec1, spec2);

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var project2Lib = lockFile.Libraries.FirstOrDefault(lib => lib.Name == "project2");

                var issue = result.CompatibilityCheckResults.SelectMany(ccr => ccr.Issues).Single();

                // Assert
                Assert.False(result.Success);
                Assert.Equal(0, result.GetAllUnresolved().Count);
                Assert.NotNull(project2Lib);
                Assert.Equal("Project project2 is not compatible with net45 (.NETFramework,Version=v4.5). Project project2 supports:\n  - net46 (.NETFramework,Version=v4.6)\n  - win81 (Windows,Version=v8.1)".Replace("\n", Environment.NewLine), issue.Format());
                Assert.Equal(1, logger.ErrorMessages.Count());
            }
        }

        [Fact]
        public async Task ProjectResolution_ProjectReferenceWithInCompatibleTFMNoRange()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            var project2Json = @"
            {
              ""version"": ""2.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net46"": {
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1);
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project2Json, "project2", specPath2);

                var logger = new TestLogger();

                var restoreContext = new RestoreArgs()
                {
                    Sources = new List<string>() { packageSource.FullName },
                    GlobalPackagesFolder = packagesDir.FullName,
                    Log = logger,
                    CacheContext = new SourceCacheContext()
                };

                // Modify specs for netcore
                spec2 = spec2.WithTestRestoreMetadata();
                spec1 = spec1.WithTestRestoreMetadata().WithTestProjectReference(spec2);

                var request = await ProjectTestHelpers.GetRequestAsync(restoreContext, spec1, spec2);

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var issue = result.CompatibilityCheckResults.SelectMany(ccr => ccr.Issues).Single();

                // Assert
                Assert.False(result.Success);
                Assert.Equal(0, result.GetAllUnresolved().Count);
                Assert.Equal("Project project2 is not compatible with net45 (.NETFramework,Version=v4.5). Project project2 supports: net46 (.NETFramework,Version=v4.6)", issue.Format());
                Assert.Equal(1, logger.ErrorMessages.Count());

                Assert.Contains("Project project2 is not compatible with net45 (.NETFramework,Version=v4.5). Project project2 supports: net46 (.NETFramework,Version=v4.6)", logger.ErrorMessages.Last());
            }
        }

        [Fact]
        public async Task ProjectResolution_ExternalReferenceWithNoProjectJson()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var msbuidPath1 = Path.Combine(project2.FullName, "project1.xproj");
                var msbuidPath2 = Path.Combine(project2.FullName, "project2.csproj");

                File.WriteAllText(msbuidPath1, string.Empty);
                File.WriteAllText(msbuidPath2, string.Empty);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger);

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project1",
                    spec1,
                    msbuidPath1,
                    new string[] { "project2" }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project2",
                    null,
                    msbuidPath2,
                    new string[] { }));

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.Targets.Single().Libraries.Single();

                // Assert
                Assert.True(result.Success);

                // External projects that did not provide a framework should leave the framework property out
                Assert.Null(targetLib.Framework);
            }
        }

        [Fact]
        public async Task ProjectResolution_HigherVersionForProjectReferences()
        {
            // Arrange
            var project1Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            var project2Json = @"
            {
              ""version"": ""2.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1);
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project2Json, "project2", specPath2);

                var logger = new TestLogger();

                var restoreContext = new RestoreArgs()
                {
                    Sources = new List<string>() { packageSource.FullName },
                    GlobalPackagesFolder = packagesDir.FullName,
                    Log = logger,
                    CacheContext = new SourceCacheContext()
                };

                // Modify specs for netcore
                spec2 = spec2.WithTestRestoreMetadata();
                spec1 = spec1.WithTestRestoreMetadata().WithTestProjectReference(spec2);

                var request = await ProjectTestHelpers.GetRequestAsync(restoreContext, spec1, spec2);
                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(0, result.GetAllUnresolved().Count);
                Assert.Equal(0, logger.Messages.Count(s => s.IndexOf("Dependency specified was") > -1));
            }
        }

        // CSProj does not resolve sibling folder
        [Fact]
        public async Task ProjectResolution_MSBuildProjectDoesNotResolveByDirectory()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""dependencies"": {
                ""project2"": ""1.0.0-*""
              },
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            var project2Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            var globalJson = @"
            {
                ""projects"": [
                    ""projects""
                ]
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);
                File.WriteAllText(Path.Combine(workingDir, "global.json"), globalJson);

                var project1CSProjPath = Path.Combine(project1.FullName, "project1.csproj");
                File.WriteAllText(project1CSProjPath, string.Empty);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project2Json, "project2", specPath2).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger);

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");

                request.ExternalProjects.Add(
                    new ExternalProjectReference("project1", spec1, project1CSProjPath, new string[] { }));

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.False(result.Success);
                Assert.Equal(1, result.GetAllUnresolved().Count);
                Assert.Equal("project2", result.GetAllUnresolved().Single().Name);
            }
        }
    }
}
