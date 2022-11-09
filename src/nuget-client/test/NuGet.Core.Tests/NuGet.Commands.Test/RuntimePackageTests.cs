// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RuntimePackageTests
    {
        [Fact]
        public async Task RuntimePackage_RejectedPackagesAreNotMerged()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = CreateTestFolders())
            {
                var repository = Path.Combine(workingDir, "repository");
                var projectDir = Path.Combine(workingDir, "project");
                var packagesDir = Path.Combine(workingDir, "packages");

                var runtimeJsonX1 = @"{
                  ""runtimes"": {
                    ""unix"": {
                            ""packageX"": {
                                ""runtime.packageX"": ""1.0.0""
                            }
                          }
                        },
                ""supports"": {
                    ""x1.app"": {
                            ""uap10.0"": [
                                ""win10-x86""
                        ]
                    }
                   }
                  }";

                var runtimeJsonX2 = @"{
                  ""runtimes"": {
                    ""unix"": {
                            ""packageX"": {
                                ""runtime.packageX"": ""2.0.0""
                            }
                          }
                        },
                ""supports"": {
                    ""x2.app"": {
                            ""uap10.0"": [
                                ""win10-x86""
                        ]
                    }
                   }
                  }";

                var packages = new List<SimpleTestPackageContext>();

                // A -> X 1.0.0 -> runtime.X 1.0.0
                // B -> X 2.0.0 -> runtime.X 2.0.0

                var packageX1 = new SimpleTestPackageContext()
                {
                    Id = "packageX",
                    Version = "1.0.0",
                    RuntimeJson = runtimeJsonX1
                };

                var packageX2 = new SimpleTestPackageContext()
                {
                    Id = "packageX",
                    Version = "2.0.0",
                    RuntimeJson = runtimeJsonX2
                };

                var packageB = new SimpleTestPackageContext()
                {
                    Id = "packageB"
                };

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "packageA"
                };

                var packageX1Runtime = new SimpleTestPackageContext()
                {
                    Id = "runtime.packageX",
                    Version = "1.0.0"
                };

                var packageX2Runtime = new SimpleTestPackageContext()
                {
                    Id = "runtime.packageX",
                    Version = "2.0.0"
                };

                packageA.Dependencies.Add(packageX1);
                packageB.Dependencies.Add(packageX2);

                packages.Add(packageA);
                packages.Add(packageB);
                packages.Add(packageX1);
                packages.Add(packageX2);
                packages.Add(packageX1Runtime);
                packages.Add(packageX2Runtime);

                await SimpleTestPackageUtility.CreatePackagesAsync(packages, repository);

                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(repository));

                var configJson = JObject.Parse(@"{
                    ""runtimes"": {
                        ""unix"": {}
                    },
                    ""dependencies"": {
                        ""packageA"": ""1.0.0"",
                        ""packageB"": ""1.0.0""
                    },
                    ""frameworks"": {
                        ""_FRAMEWORK_"": {}
                    }
                }".Replace("_FRAMEWORK_", framework));

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

                var request = new TestRestoreRequest(spec, sources, packagesDir, logger);
                request.LockFilePath = Path.Combine(projectDir, "project.lock.json");

                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var runtimeGraph = result.RestoreGraphs.Single(graph => graph.RuntimeIdentifier == "unix").RuntimeGraph;

                var selectedRuntimeDependency = runtimeGraph
                    .Runtimes
                    .Single()
                    .Value
                    .RuntimeDependencySets
                    .Single()
                    .Value
                    .Dependencies
                    .Single();

                var runtimeDependencyVersion = selectedRuntimeDependency.Value.VersionRange.ToLegacyShortString();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("x2.app", runtimeGraph.Supports.Single().Key);
                Assert.Equal("2.0.0", runtimeDependencyVersion);
            }
        }

        [Fact]
        public async Task RuntimePackage_BasicRuntimePackageRestore()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "dotnet";

            using (var workingDir = CreateTestFolders())
            {
                var repository = Path.Combine(workingDir, "repository");
                var projectDir = Path.Combine(workingDir, "project");
                var packagesDir = Path.Combine(workingDir, "packages");

                CreateBasicLibPackage(repository, "packageA");
                CreateRuntimesPackage(repository);

                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(repository));

                var configJson = JObject.Parse(@"{
                ""supports"": {
                    ""net46.app"": {},
                    ""uwp.10.0.app"": {},
                    ""dnxcore50.app"": {}
                },
                ""dependencies"": {
                    ""packageA"": ""1.0.0"",
                    ""runtimes"": ""1.0.0""
                },
                ""frameworks"": {
                    ""_FRAMEWORK_"": {}
                }
            }".Replace("_FRAMEWORK_", framework));

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

                var request = new TestRestoreRequest(spec, sources, packagesDir, logger);
                request.LockFilePath = Path.Combine(projectDir, "project.lock.json");

                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success, logger.ShowMessages());
            }
        }

        private TestDirectory CreateTestFolders()
        {
            var workingDir = TestDirectory.Create();

            var repository = Path.Combine(workingDir, "repository");
            Directory.CreateDirectory(repository);
            var projectDir = Path.Combine(workingDir, "project");
            Directory.CreateDirectory(projectDir);
            var packagesDir = Path.Combine(workingDir, "packages");
            Directory.CreateDirectory(packagesDir);
            var globalDir = Path.Combine(workingDir, "globalPackages");
            Directory.CreateDirectory(globalDir);

            return workingDir;
        }

        private static void CreateRuntimesPackage(string repositoryDir)
        {
            CreateRuntimesPackage(repositoryDir, "runtimes", GetRuntimeJson());
        }

        private static void CreateRuntimesPackage(string repositoryDir, string packageId, string runtimeJson)
        {
            var file = Path.Combine(repositoryDir, packageId + ".1.0.0.nupkg");

            using (var zip = new ZipArchive(File.Create(file), ZipArchiveMode.Create))
            {
                zip.AddEntry("runtime.json", runtimeJson, Encoding.UTF8);

                zip.AddEntry(packageId + ".nuspec", @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package xmlns=""http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd"">
                        <metadata>
                        <id>" + packageId + @"</id>
                        <version>1.0.0</version>
                        <title />
                        </metadata>
                        </package>", Encoding.UTF8);
            }
        }

        private static void CreateBasicLibPackage(string repositoryDir, string packageId)
        {
            var file = Path.Combine(repositoryDir, packageId + ".1.0.0.nupkg");

            using (var zip = new ZipArchive(File.Create(file), ZipArchiveMode.Create))
            {
                zip.AddEntry("lib/net45/a.dll", new byte[] { 0 });
                zip.AddEntry("lib/uap10.0/a.dll", new byte[] { 0 });
                zip.AddEntry("lib/win8/a.dll", new byte[] { 0 });
                zip.AddEntry("lib/dotnet/a.dll", new byte[] { 0 });
                zip.AddEntry("lib/native/a.dll", new byte[] { 0 });

                zip.AddEntry(packageId + ".nuspec", @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package xmlns=""http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd"">
                        <metadata>
                        <id>" + packageId + @"</id>
                        <version>1.0.0</version>
                        <title />
                        </metadata>
                        </package>", Encoding.UTF8);
            }
        }

        private static string GetRuntimeJson()
        {
            return @"{
                ""supports"": {
                    ""uwp.10.0.app"": {
                            ""uap10.0"": [
                                ""win10-x86"",
                                ""win10-x86-aot"",
                                ""win10-x64"",
                                ""win10-x64-aot"",
                                ""win10-arm"",
                                ""win10-arm-aot""
                        ]
                    },
                    ""net46.app"": {
                        ""net46"": [
                            ""win-x86"",
                            ""win-x64""
                        ]
                    },
                    ""dnxcore50.app"": {
                        ""dnxcore50"": [
                            ""win7-x86"",
                            ""win7-x64""
                        ]
                    }
                }
            }";
        }
    }
}
