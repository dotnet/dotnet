// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RuntimeTargetsTests
    {
        [Fact]
        public async Task RestoreTargets_RestoreWithNoRuntimesAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandard1.5"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "packageA"
                };

                packageA.AddFile("lib/a.dll");
                packageA.AddFile("lib/netstandard1.5/a.dll");
                packageA.AddFile("lib/netstandard1.5/en-us/a.resource.dll");
                packageA.AddFile("native/a.dll");
                packageA.AddFile("ref/netstandard1.5/a.dll");
                packageA.AddFile("contentFiles/any/any/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.Targets.Single(graph => graph.RuntimeIdentifier == null).Libraries.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1, lockFile.Libraries.Count);
                Assert.Equal(0, targetLib.RuntimeTargets.Count);
            }
        }

        [Fact]
        public async Task RestoreTargets_RestoreWithRuntimesAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandardapp1.5"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };
                request.RequestedRuntimes.Add("win7-x86");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "packageA"
                };

                packageA.AddFile("lib/netstandard1.5/a.dll");
                packageA.AddFile("native/a.dll");
                packageA.AddFile("runtimes/unix/native/a.dll");
                packageA.AddFile("runtimes/unix/lib/netstandard1.5/a.dll");
                packageA.AddFile("runtimes/win7/lib/netstandard1.5/a.dll");
                packageA.AddFile("runtimes/win7-x86/lib/netstandard1.5/a.dll");
                packageA.AddFile("runtimes/win7-x86/lib/netstandard1.5/en-us/a.resources.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var format = new LockFileFormat();
                var lockFile = format.Read(request.LockFilePath);

                var targetLib = lockFile.Targets.Single(graph => graph.RuntimeIdentifier == null).Libraries.Single();
                var ridTargetLib = lockFile.Targets.Single(graph => graph.RuntimeIdentifier != null).Libraries.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(5, targetLib.RuntimeTargets.Count);

                Assert.Equal("runtimes/unix/lib/netstandard1.5/a.dll", targetLib.RuntimeTargets[0].Path);
                Assert.Equal("runtime", targetLib.RuntimeTargets[0].Properties["assetType"]);
                Assert.Equal("unix", targetLib.RuntimeTargets[0].Properties["rid"]);

                Assert.Equal("runtimes/win7-x86/lib/netstandard1.5/en-us/a.resources.dll", targetLib.RuntimeTargets[3].Path);
                Assert.Equal("resource", targetLib.RuntimeTargets[3].Properties["assetType"]);
                Assert.Equal("win7-x86", targetLib.RuntimeTargets[3].Properties["rid"]);

                Assert.Equal("runtimes/unix/native/a.dll", targetLib.RuntimeTargets[1].Path);
                Assert.Equal("native", targetLib.RuntimeTargets[1].Properties["assetType"]);
                Assert.Equal("unix", targetLib.RuntimeTargets[1].Properties["rid"]);

                // This section does not exist for RID graphs
                Assert.Equal(0, ridTargetLib.RuntimeTargets.Count);
            }
        }

        [Fact]
        public async Task RestoreTargets_RestoreWithRuntimes_ExcludeAllAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandardapp1.5"": {
                    ""dependencies"": {
                        ""packageA"": {
                            ""version"": ""1.0.0"",
                            ""exclude"": ""all""
                        }
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };
                request.RequestedRuntimes.Add("win7-x86");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "packageA"
                };

                packageA.AddFile("lib/netstandard1.5/a.dll");
                packageA.AddFile("native/a.dll");
                packageA.AddFile("runtimes/unix/native/a.dll");
                packageA.AddFile("runtimes/unix/lib/netstandard1.5/a.dll");
                packageA.AddFile("runtimes/win7/lib/netstandard1.5/a.dll");
                packageA.AddFile("runtimes/win7-x86/lib/netstandard1.5/a.dll");
                packageA.AddFile("runtimes/win7-x86/lib/netstandard1.5/en-us/a.resources.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var format = new LockFileFormat();
                var lockFile = format.Read(request.LockFilePath);

                var targetLib = lockFile.Targets.Single(graph => graph.RuntimeIdentifier == null).Libraries.Single();
                var ridTargetLib = lockFile.Targets.Single(graph => graph.RuntimeIdentifier != null).Libraries.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(3, targetLib.RuntimeTargets.Count);

                Assert.Equal("runtimes/unix/lib/netstandard1.5/_._", targetLib.RuntimeTargets[0].Path);
                Assert.Equal("runtime", targetLib.RuntimeTargets[0].Properties["assetType"]);
                Assert.Equal("unix", targetLib.RuntimeTargets[0].Properties["rid"]);

                Assert.Equal("runtimes/win7-x86/lib/netstandard1.5/en-us/_._", targetLib.RuntimeTargets[2].Path);
                Assert.Equal("resource", targetLib.RuntimeTargets[2].Properties["assetType"]);
                Assert.Equal("win7-x86", targetLib.RuntimeTargets[2].Properties["rid"]);

                Assert.Equal("runtimes/unix/native/_._", targetLib.RuntimeTargets[1].Path);
                Assert.Equal("native", targetLib.RuntimeTargets[1].Properties["assetType"]);
                Assert.Equal("unix", targetLib.RuntimeTargets[1].Properties["rid"]);

                // This section does not exist for RID graphs
                Assert.Equal(0, ridTargetLib.RuntimeTargets.Count);
            }
        }

        [Fact]
        public async Task RestoreTargets_RestoreWithRuntimesAndNearestFrameworkAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""uap10.0"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };
                request.RequestedRuntimes.Add("win7-x86");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "packageA"
                };

                packageA.AddFile("runtimes/unix/lib/netstandard1.1/a.dll");
                packageA.AddFile("runtimes/unix/lib/netstandard1.2/a.dll");
                packageA.AddFile("runtimes/unix/lib/netstandard2.0/a.dll");

                packageA.AddFile("runtimes/win7/lib/netstandard1.3/a.dll");
                packageA.AddFile("runtimes/win7/lib/netstandard1.4/a.dll");

                packageA.AddFile("runtimes/win81-x86/lib/win81/a.dll");
                packageA.AddFile("runtimes/win81-x86/lib/win8/a.dll");

                packageA.AddFile("runtimes/win-any/lib/net45/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var format = new LockFileFormat();
                var lockFile = format.Read(request.LockFilePath);

                var targetLib = lockFile.Targets.Single(graph => graph.RuntimeIdentifier == null).Libraries.Single();
                var ridTargetLib = lockFile.Targets.Single(graph => graph.RuntimeIdentifier != null).Libraries.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(3, targetLib.RuntimeTargets.Count);

                Assert.Equal("runtimes/unix/lib/netstandard1.2/a.dll", targetLib.RuntimeTargets[0].Path);
                Assert.Equal("runtimes/win7/lib/netstandard1.4/a.dll", targetLib.RuntimeTargets[1].Path);
                Assert.Equal("runtimes/win81-x86/lib/win81/a.dll", targetLib.RuntimeTargets[2].Path);

                // This section does not exist for RID graphs
                Assert.Equal(0, ridTargetLib.RuntimeTargets.Count);
            }
        }
    }
}
