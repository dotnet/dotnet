// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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
    public class CompatilibityCheckerTests
    {
        [Theory]
        [InlineData("lib/net45/a.dll", false)]
        [InlineData("lib/netstandard1.5/a.dll", true)]
        [InlineData("lib/netstandard1.0/a.dll", true)]
        [InlineData("lib/netstandard1.6/a.dll", false)]
        [InlineData("build/packageA.targets", true)]
        [InlineData("build/packageA.props", true)]
        [InlineData("build/netstandard1.3/packageA.targets", true)]
        [InlineData("build/netstandard1.3/packageA.props", true)]
        [InlineData("build/netstandard1.3/packageB.targets", false)]
        [InlineData("build/netstandard1.3/packageB.props", false)]
        [InlineData("buildCrossTargeting/packageA.targets", true)]
        [InlineData("buildCrossTargeting/packageA.props", true)]
        [InlineData("buildCrossTargeting/packageB.targets", false)]
        [InlineData("buildCrossTargeting/packageB.props", false)]
        [InlineData("buildMultiTargeting/packageA.targets", true)]
        [InlineData("buildMultiTargeting/packageA.props", true)]
        [InlineData("buildMultiTargeting/packageB.targets", false)]
        [InlineData("buildMultiTargeting/packageB.props", false)]
        [InlineData("build/_._", true)]
        [InlineData("build/netstandard1.3/_._", true)]
        [InlineData("buildCrossTargeting/_._", true)]
        [InlineData("contentFiles/any/any/_._", true)]
        [InlineData("contentFiles/any/any/a.txt", true)]
        [InlineData("contentFiles/cs/netstandard/a.txt", true)]
        [InlineData("contentFiles/cs/net45/a.txt", false)]
        public async Task CompatilibityChecker_SingleFileVerifyCompatibilityAsync(string file, bool expected)
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
                        ""packageA"": {
                            ""version"": ""1.0.0""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile(file);

                // Ensure that the package doesn't pass due to no ref or lib files.
                packageA.AddFile("lib/net462/_._");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.Equal(expected, result.Success);

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);

                // Verify no compatibility issues
                Assert.Equal(expected, result.CompatibilityCheckResults.All(check => check.Success));
            }
        }

        [Fact]
        public async Task CompatilibityChecker_PackageCompatibility_VerifyAvailableFrameworksAsync()
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
                ""netstandard1.0"": {
                    ""dependencies"": {
                        ""packageA"": {
                            ""version"": ""1.0.0""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("lib/netstandard1.1/a.dll");
                packageA.AddFile("contentFiles/any/win81/a.dll");
                packageA.AddFile("ref/netstandard1.2/a.dll");
                packageA.AddFile("runtimes/win7-x86/lib/netstandard1.3/a.dll");
                packageA.AddFile("runtimes/win7-x86/native/a.dll");
                packageA.AddFile("runtimes/win8/lib/netstandard1.4/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.False(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);

                var issue = result.CompatibilityCheckResults.SelectMany(check => check.Issues).Single();

                Assert.Equal("Package packageA 1.0.0 is not compatible with netstandard1.0 (.NETStandard,Version=v1.0). Package packageA 1.0.0 supports:\n  - netstandard1.1 (.NETStandard,Version=v1.1)\n  - netstandard1.2 (.NETStandard,Version=v1.2)\n  - win81 (Windows,Version=v8.1)".Replace("\n", Environment.NewLine), issue.Format());
            }
        }

        [Fact]
        public async Task CompatilibityChecker_PackageCompatibility_VerifyNoAvailableFrameworksAsync()
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
                ""netstandard1.0"": {
                    ""dependencies"": {
                        ""packageA"": {
                            ""version"": ""1.0.0""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("ref/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.False(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);

                var issue = result.CompatibilityCheckResults.SelectMany(check => check.Issues).Single();

                Assert.Equal(@"Package packageA 1.0.0 is not compatible with netstandard1.0 (.NETStandard,Version=v1.0). Package packageA 1.0.0 does not support any target frameworks.".Replace("\n", Environment.NewLine), issue.Format());
            }
        }

        [Fact]
        public async Task CompatilibityChecker_PackageCompatibility_VerifyAvailableFrameworksIgnoresInvalidAssetsAsync()
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
                ""netstandard1.0"": {
                    ""dependencies"": {
                        ""packageA"": {
                            ""version"": ""1.0.0""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("lib/netstandard1.1/valid.dll");
                packageA.AddFile("lib/netstandard1.0/x86/invalid.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.False(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);

                var issue = result.CompatibilityCheckResults.SelectMany(check => check.Issues).Single();

                Assert.Equal("Package packageA 1.0.0 is not compatible with netstandard1.0 (.NETStandard,Version=v1.0). Package packageA 1.0.0 supports: netstandard1.1 (.NETStandard,Version=v1.1)", issue.Format());
            }
        }

        [Fact]
        public async Task CompatilibityChecker_PackageExcludeCompileRuntime_SuccessAsync()
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
                        ""packageA"": {
                            ""version"": ""1.0.0"",
                            ""exclude"": ""compile,runtime""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("lib/net45/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);

                // Verify no compatibility issues
                Assert.True(result.CompatibilityCheckResults.All(check => check.Success));
            }
        }

        [Fact]
        public async Task CompatilibityChecker_MissingRuntimeAssemblyCompileOnly_SuccessAsync()
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
                        ""packageA"": {
                            ""version"": ""1.0.0"",
                            ""include"": ""compile""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("ref/netstandard1.3/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);

                // Verify no compatibility issues
                Assert.True(result.CompatibilityCheckResults.All(check => check.Success));
            }
        }

        [Fact]
        public async Task CompatilibityChecker_MissingRuntimeAssembly_FailAsync()
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
                            ""version"": ""1.0.0""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("ref/netstandard1.3/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var failure = result.CompatibilityCheckResults.Single(r => !r.Success).Issues.Single();

                // Assert
                Assert.False(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);

                // Verify compatibility issue
                Assert.Equal("a", failure.AssemblyName);
                Assert.Equal("win7-x86", failure.RuntimeIdentifier);
            }
        }

        [Fact]
        public async Task CompatilibityChecker_MissingRuntimeAssembly_IgnoreAsync()
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
                            ""version"": ""1.0.0""
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
                request.ValidateRuntimeAssets = false;

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("ref/netstandard1.3/a.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var failures = result.CompatibilityCheckResults.Count(r => !r.Success);

                // Assert
                Assert.True(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);
                Assert.Equal(0, failures);
            }
        }

        [Fact]
        public async Task CompatilibityChecker_RuntimeFoundInAnotherPackage_SuccessAsync()
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
                        ""packageA"": {
                            ""version"": ""1.0.0""
                        },
                        ""packageB"": {
                            ""version"": ""1.0.0""
                        },
                        ""packageC"": {
                            ""version"": ""1.0.0""
                        },
                        ""packageD"": {
                            ""version"": ""1.0.0""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("ref/netstandard1.3/a.dll");
                packageA.AddFile("ref/netstandard1.3/b.dll");
                packageA.AddFile("ref/netstandard1.3/c.dll");

                var packageB = new SimpleTestPackageContext("packageB");
                packageB.AddFile("runtimes/win7-x86/lib/netstandard1.1/a.dll");

                var packageC = new SimpleTestPackageContext("packageC");
                packageC.AddFile("lib/netstandard1.1/b.ni.dll");

                var packageD = new SimpleTestPackageContext("packageD");
                packageD.AddFile("lib/netstandard1.1/c.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA, packageB, packageC, packageD);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(4, result.LockFile.Libraries.Count);

                // Verify no compatibility issues
                Assert.True(result.CompatibilityCheckResults.All(check => check.Success));
            }
        }

        [Fact]
        public async Task CompatilibityChecker_RuntimeFoundInSamePackage_SuccessAsync()
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
                        ""packageA"": {
                            ""version"": ""1.0.0""
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

                var packageA = new SimpleTestPackageContext("packageA");
                packageA.AddFile("ref/netstandard1.3/a.dll");
                packageA.AddFile("ref/netstandard1.3/b.dll");
                packageA.AddFile("ref/netstandard1.3/c.dll");
                packageA.AddFile("runtimes/win7-x86/lib/netstandard1.1/a.dll");
                packageA.AddFile("runtimes/win7-x86/lib/netstandard1.1/b.ni.dll");
                packageA.AddFile("runtimes/win7-x86/lib/netstandard1.1/c.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource.FullName, packageA);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success, logger.ShowErrors());

                // Verify both libraries were installed
                Assert.Equal(1, result.LockFile.Libraries.Count);

                // Verify no compatibility issues
                Assert.True(result.CompatibilityCheckResults.All(check => check.Success));
            }
        }
    }
}
