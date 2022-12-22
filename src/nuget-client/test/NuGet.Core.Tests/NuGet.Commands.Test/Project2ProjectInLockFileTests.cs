// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class Project2ProjectTests
    {
        [Fact]
        public async Task Project2ProjectInLockFile_VerifySnapshotVersions()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var projectJson = @"
            {
                ""version"": ""2.0.0"",
                ""dependencies"": {
                },
                ""frameworks"": {
                    ""net45"": {}
                }
            }";

            var project2Json = @"
            {
              ""version"": ""2.0.0-*"",
              ""description"": ""Proj2 Class Library"",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""dependencies"": {
                ""project3"": ""2.0.0-*""
              },
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            var project3Json = @"
            {
              ""version"": ""2.0.0-*"",
              ""description"": ""Proj3 Class Library"",
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

            using (var packagesDir = TestDirectory.Create())
            using (var workingDir = TestDirectory.Create())
            {
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                var project3 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project3"));
                project1.Create();
                project2.Create();
                project3.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), projectJson);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);
                File.WriteAllText(Path.Combine(project3.FullName, "project.json"), project3Json);
                File.WriteAllText(Path.Combine(workingDir, "global.json"), globalJson);

                File.WriteAllText(Path.Combine(project1.FullName, "project1.csproj"), string.Empty);
                File.WriteAllText(Path.Combine(project2.FullName, "project2.xproj"), string.Empty);
                File.WriteAllText(Path.Combine(project2.FullName, "project3.xproj"), string.Empty);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var specPath3 = Path.Combine(project3.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(projectJson, "project1", specPath1).EnsureProjectJsonRestoreMetadata();
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project2Json, "project2", specPath2).EnsureProjectJsonRestoreMetadata();
                var spec3 = JsonPackageSpecReader.GetPackageSpec(project3Json, "project3", specPath3).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir, logger);
                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project1",
                    spec1,
                    Path.Combine(project1.FullName, "project1.csproj"),
                    new string[] { "project2" }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project2",
                    spec2,
                    Path.Combine(project2.FullName, "project2.xproj"),
                    new string[] { "project3" }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project3",
                    spec3,
                    Path.Combine(project2.FullName, "project3.xproj"),
                    new string[] { }));


                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");
                var format = new LockFileFormat();

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var lockFile = format.Read(request.LockFilePath, logger);

                var project2Lib = lockFile.GetLibrary("project2", NuGetVersion.Parse("2.0.0"));
                var project3Lib = lockFile.GetLibrary("project3", NuGetVersion.Parse("2.0.0"));

                var project2Target = lockFile.GetTarget(FrameworkConstants.CommonFrameworks.Net45, runtimeIdentifier: null)
                    .Libraries
                    .Single(lib => lib.Name == "project2");

                var project3Target = lockFile.GetTarget(FrameworkConstants.CommonFrameworks.Net45, runtimeIdentifier: null)
                    .Libraries
                    .Single(lib => lib.Name == "project3");

                // Assert
                Assert.True(result.Success);

                Assert.Equal("2.0.0", project2Target.Version.ToString());
                Assert.Equal("2.0.0", project3Target.Version.ToString());
                Assert.Equal("2.0.0", project2Lib.Version.ToString());
                Assert.Equal("2.0.0", project3Lib.Version.ToString());

                Assert.Equal("2.0.0", project2Target.Dependencies.Single().VersionRange.ToLegacyShortString());
            }
        }

        [Fact]
        public async Task Project2ProjectInLockFile_PackageReferenceConflict()
        {
            // Arrange
            var sources = new List<PackageSource>()
            {
                new PackageSource("https://api.nuget.org/v3/index.json")
            };

            var project1Json = @"
            {
                ""version"": ""1.0.0"",
                ""dependencies"": {
                },
                ""frameworks"": {
                    ""net45"": {}
                }
            }";

            var project2Json = @"
            {
                ""version"": ""1.0.0"",
                ""dependencies"": {
                    ""Microsoft.VisualBasic"": ""10.0.0""
                },
                ""frameworks"": {
                    ""net45"": {}
                }
            }";

            using (var packagesDir = TestDirectory.Create())
            using (var workingDir = TestDirectory.Create())
            {
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                project1.Create();
                project2.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);

                File.WriteAllText(Path.Combine(project1.FullName, "project1.csproj"), string.Empty);
                File.WriteAllText(Path.Combine(project2.FullName, "project2.csproj"), string.Empty);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project2Json, "project2", specPath2).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir, logger);
                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project1",
                    spec1,
                    Path.Combine(project1.FullName, "project1.csproj"),
                    new string[] { "project2" }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project2",
                    spec2,
                    Path.Combine(project2.FullName, "project2.csproj"),
                    new string[] { }));

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");
                var format = new LockFileFormat();

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success, logger.ShowMessages());
            }
        }

        [Fact]
        public async Task Project2ProjectInLockFile_VerifyP2PWithNonProjectJsonReference()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var projectJson = @"
            {
                ""version"": ""1.0.0"",
                ""dependencies"": {
                },
                ""frameworks"": {
                    ""net45"": {}
                }
            }";

            using (var packagesDir = TestDirectory.Create())
            using (var workingDir = TestDirectory.Create())
            {
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                var project3 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project3"));
                project1.Create();
                project2.Create();
                project3.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), projectJson);

                File.WriteAllText(Path.Combine(project1.FullName, "project1.csproj"), string.Empty);
                File.WriteAllText(Path.Combine(project2.FullName, "project2.csproj"), string.Empty);
                File.WriteAllText(Path.Combine(project3.FullName, "project3.csproj"), string.Empty);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(projectJson, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir, logger);
                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project1",
                    spec1,
                    Path.Combine(project1.FullName, "project1.xproj"),
                    new string[] { "project2" }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project2",
                    null,
                    Path.Combine(project2.FullName, "project2.csproj"),
                    new string[] { "project3" }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project3",
                    null,
                    Path.Combine(project3.FullName, "project3.csproj"),
                    new string[] { }));

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");
                var format = new LockFileFormat();

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var lockFile = format.Read(request.LockFilePath, logger);

                var project1Lib = lockFile.GetLibrary("project1", NuGetVersion.Parse("1.0.0"));
                var project2Lib = lockFile.GetLibrary("project2", NuGetVersion.Parse("1.0.0"));
                var project3Lib = lockFile.GetLibrary("project3", NuGetVersion.Parse("1.0.0"));

                var project2Target = lockFile.GetTarget(FrameworkConstants.CommonFrameworks.Net45, runtimeIdentifier: null)
                    .Libraries
                    .Where(lib => lib.Name == "project2")
                    .Single();

                var project3Target = lockFile.GetTarget(FrameworkConstants.CommonFrameworks.Net45, runtimeIdentifier: null)
                    .Libraries
                    .Where(lib => lib.Name == "project3")
                    .Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(2, lockFile.Libraries.Count);

                Assert.Equal("project", project2Lib.Type);
                Assert.Equal("project", project3Lib.Type);

                Assert.Null(project2Lib.Path);
                Assert.Null(project3Lib.Path);

                Assert.Equal("../project2/project2.csproj", project2Lib.MSBuildProject);
                Assert.Equal("../project3/project3.csproj", project3Lib.MSBuildProject);

                Assert.Equal(1, project2Target.Dependencies.Count);
                Assert.Equal("project3", project2Target.Dependencies.Single().Id);
                Assert.Equal("1.0.0", project2Target.Dependencies.Single().VersionRange.ToLegacyShortString());

                Assert.Equal(0, project3Target.Dependencies.Count);

                Assert.Null(project2Target.Framework);
                Assert.Null(project3Target.Framework);
            }
        }

        [Fact]
        public async Task Project2ProjectInLockFile_VerifyProjectsUnderProjectFileDependencyGroups_External()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var projectJson = @"
            {
                ""version"": ""1.0.0"",
                ""dependencies"": {
                },
                ""frameworks"": {
                    ""net45"": {}
                }
            }";

            using (var packagesDir = TestDirectory.Create())
            using (var workingDir = TestDirectory.Create())
            {
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                var project3 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project3"));
                project1.Create();
                project2.Create();
                project3.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), projectJson);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), projectJson);
                File.WriteAllText(Path.Combine(project3.FullName, "project.json"), projectJson);

                File.WriteAllText(Path.Combine(project1.FullName, "project1.csproj"), string.Empty);
                File.WriteAllText(Path.Combine(project2.FullName, "project2.csproj"), string.Empty);
                File.WriteAllText(Path.Combine(project3.FullName, "project3.csproj"), string.Empty);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var specPath3 = Path.Combine(project3.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(projectJson, "project1", specPath1).EnsureProjectJsonRestoreMetadata();
                var spec2 = JsonPackageSpecReader.GetPackageSpec(projectJson, "project2", specPath2).EnsureProjectJsonRestoreMetadata();
                var spec3 = JsonPackageSpecReader.GetPackageSpec(projectJson, "project3", specPath3).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir, logger);
                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project1",
                    spec1,
                    Path.Combine(project1.FullName, "project1.xproj"),
                    new string[] { "project2" }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project2",
                    spec2,
                    Path.Combine(project2.FullName, "project2.csproj"),
                    new string[] { "project3" }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                    "project3",
                    spec3,
                    Path.Combine(project3.FullName, "project3.csproj"),
                    new string[] { }));

                request.LockFilePath = Path.Combine(project1.FullName, "project.lock.json");
                var format = new LockFileFormat();

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var lockFile = format.Read(request.LockFilePath, logger);

                var project1Lib = lockFile.GetLibrary("project1", NuGetVersion.Parse("1.0.0"));
                var project2Lib = lockFile.GetLibrary("project2", NuGetVersion.Parse("1.0.0"));
                var project3Lib = lockFile.GetLibrary("project3", NuGetVersion.Parse("1.0.0"));

                var project2Target = lockFile.GetTarget(FrameworkConstants.CommonFrameworks.Net45, runtimeIdentifier: null)
                    .Libraries
                    .Where(lib => lib.Name == "project2")
                    .Single();

                var project3Target = lockFile.GetTarget(FrameworkConstants.CommonFrameworks.Net45, runtimeIdentifier: null)
                    .Libraries
                    .Where(lib => lib.Name == "project3")
                    .Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(2, lockFile.Libraries.Count);

                Assert.Equal("project", project2Lib.Type);
                Assert.Equal("project", project3Lib.Type);

                Assert.Equal("../project2/project.json", project2Lib.Path);
                Assert.Equal("../project3/project.json", project3Lib.Path);

                Assert.Equal("../project2/project2.csproj", project2Lib.MSBuildProject);
                Assert.Equal("../project3/project3.csproj", project3Lib.MSBuildProject);

                Assert.Equal(1, project2Target.Dependencies.Count);
                Assert.Equal("project3", project2Target.Dependencies.Single().Id);
                Assert.Equal("1.0.0", project2Target.Dependencies.Single().VersionRange.ToLegacyShortString());

                Assert.Equal(0, project3Target.Dependencies.Count);

                Assert.Equal(".NETFramework,Version=v4.5", project2Target.Framework);
                Assert.Equal(".NETFramework,Version=v4.5", project3Target.Framework);
            }
        }

        [Fact]
        public async Task Project2ProjectInLockFile_VerifyProjectsReferencesInLibAndTargets()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
                ""version"": ""1.0.0"",
                ""frameworks"": {
                    ""net45"": {}
                }
            }";

            var project2Json = @"
            {
                ""version"": ""1.0.0"",
                ""frameworks"": {
                    ""net45"": {}
                }
            }";

            var project3Json = @"
            {
                ""version"": ""1.0.0"",
                ""dependencies"": {
                },
                ""frameworks"": {
                    ""net45"": {}
                }
            }";

            using (var packagesDir = TestDirectory.Create())
            using (var workingDir = TestDirectory.Create())
            {
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                var project3 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project3"));
                project1.Create();
                project2.Create();
                project3.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(project2.FullName, "project.json"), project2Json);
                File.WriteAllText(Path.Combine(project3.FullName, "project.json"), project3Json);

                var project1Path = Path.Combine(project1.FullName, "project1.csproj");
                var project2Path = Path.Combine(project2.FullName, "project2.csproj");
                var project3Path = Path.Combine(project3.FullName, "project3.csproj");

                File.WriteAllText(project1Path, string.Empty);
                File.WriteAllText(project2Path, string.Empty);
                File.WriteAllText(project3Path, string.Empty);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var specPath2 = Path.Combine(project2.FullName, "project.json");
                var specPath3 = Path.Combine(project3.FullName, "project.json");

                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1);
                var spec2 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project2", specPath2);
                var spec3 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project3", specPath3);

                var logger = new TestLogger();

                var restoreContext = new RestoreArgs()
                {
                    GlobalPackagesFolder = packagesDir,
                    Log = logger,
                    CacheContext = new SourceCacheContext()
                };

                // Modify specs for netcore
                spec3 = spec3.WithTestRestoreMetadata();
                spec3.FilePath = project3Path;
                spec3.RestoreMetadata.ProjectPath = project3Path;
                spec3.RestoreMetadata.ProjectUniqueName = project3Path;

                spec2 = spec2.WithTestRestoreMetadata().WithTestProjectReference(spec3);
                spec2.FilePath = project2Path;
                spec2.RestoreMetadata.ProjectPath = project2Path;
                spec2.RestoreMetadata.ProjectUniqueName = project2Path;

                spec1 = spec1.WithTestRestoreMetadata().WithTestProjectReference(spec2);
                spec1.FilePath = project1Path;
                spec1.RestoreMetadata.ProjectPath = project1Path;
                spec1.RestoreMetadata.ProjectUniqueName = project1Path;

                var request = await ProjectTestHelpers.GetRequestAsync(restoreContext, spec1, spec2, spec3);

                request.LockFilePath = Path.Combine(project1.FullName, "project.assets.json");
                var format = new LockFileFormat();

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                Assert.True(result.Success, logger.ShowMessages());

                var lockFile = format.Read(request.LockFilePath, logger);

                var project1Lib = lockFile.GetLibrary("project1", NuGetVersion.Parse("1.0.0"));
                var project2Lib = lockFile.GetLibrary("project2", NuGetVersion.Parse("1.0.0"));
                var project3Lib = lockFile.GetLibrary("project3", NuGetVersion.Parse("1.0.0"));

                var project2Target = lockFile.GetTarget(FrameworkConstants.CommonFrameworks.Net45, runtimeIdentifier: null)
                    .Libraries
                    .Where(lib => lib.Name == "project2")
                    .Single();

                var project3Target = lockFile.GetTarget(FrameworkConstants.CommonFrameworks.Net45, runtimeIdentifier: null)
                    .Libraries
                    .Where(lib => lib.Name == "project3")
                    .Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(2, lockFile.Libraries.Count);

                Assert.Equal("project", project2Lib.Type);
                Assert.Equal("project", project3Lib.Type);

                Assert.Equal("../project2/project2.csproj", project2Lib.Path);
                Assert.Equal("../project3/project3.csproj", project3Lib.Path);

                Assert.Equal("../project2/project2.csproj", project2Lib.MSBuildProject);
                Assert.Equal("../project3/project3.csproj", project3Lib.MSBuildProject);

                Assert.Equal(1, project2Target.Dependencies.Count);
                Assert.Equal("project3", project2Target.Dependencies.Single().Id);
                Assert.Equal("1.0.0", project2Target.Dependencies.Single().VersionRange.ToLegacyShortString());

                Assert.Equal(0, project3Target.Dependencies.Count);

                Assert.Equal(".NETFramework,Version=v4.5", project2Target.Framework);
                Assert.Equal(".NETFramework,Version=v4.5", project3Target.Framework);
            }
        }
    }
}
