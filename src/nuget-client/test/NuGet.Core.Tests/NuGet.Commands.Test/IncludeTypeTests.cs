// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Protocol.Test;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility.Commands;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class IncludeTypeTests
    {
        [Fact]
        public async Task IncludeType_ProjectToProjectDeeperDependencyExcludedAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                ""dependencies"": {
                    ""packageX"": {
                        ""version"": ""1.0.0"",
                        ""suppressParent"": ""runtime,compile""
                    },
                    ""packageY"": {
                        ""version"": ""1.0.0"",
                        ""exclude"": ""runtime,compile"",
                        ""suppressParent"": ""runtime,compile""
                    }
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                var configJson1 = @"{
                ""dependencies"": {
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), "all", string.Empty);

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);
                result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(0, GetNonEmptyCount(targets["packageY"].CompileTimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectDeeperDependencyOverridesAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";
            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                ""dependencies"": {
                    ""packageX"": {
                        ""version"": ""1.0.0""
                    },
                    ""packageY"": {
                        ""version"": ""1.0.0"",
                        ""exclude"": ""runtime,compile""
                    }
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                var configJson1 = @"{
                ""dependencies"": {
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), "all", string.Empty);

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);
                result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].CompileTimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectDefaultFlowDoubleRestoreAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                ""dependencies"": {
                    ""packageX"": {
                        ""version"": ""1.0.0""
                    }
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                var configJson1 = @"{
                ""dependencies"": {
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), "all", string.Empty);

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);
                result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(0, GetNonEmptyCount(targets["packageX"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageX"].FrameworkAssemblies.Count);
                Assert.Equal(1, targets["packageX"].Dependencies.Count);

                Assert.Equal(0, GetNonEmptyCount(targets["packageY"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageY"].FrameworkAssemblies.Count);
                Assert.Equal(1, targets["packageY"].Dependencies.Count);

                Assert.Equal(0, GetNonEmptyCount(targets["packageZ"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageZ"].FrameworkAssemblies.Count);
                Assert.Equal(0, targets["packageZ"].Dependencies.Count);

                Assert.Equal(0, msbuildTargets["TestProject1"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectFlowAllDoubleRestoreAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                ""dependencies"": {
                    ""packageX"": {
                        ""version"": ""1.0.0"",
                        ""suppressParent"": ""none""
                    }
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                var configJson1 = @"{
                ""dependencies"": {
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), "all", string.Empty);

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);
                result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(2, GetNonEmptyCount(targets["packageX"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageX"].FrameworkAssemblies.Count);
                Assert.Equal(1, targets["packageX"].Dependencies.Count);

                Assert.Equal(2, GetNonEmptyCount(targets["packageY"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageY"].FrameworkAssemblies.Count);
                Assert.Equal(1, targets["packageY"].Dependencies.Count);

                Assert.Equal(2, GetNonEmptyCount(targets["packageZ"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageZ"].FrameworkAssemblies.Count);
                Assert.Equal(0, targets["packageZ"].Dependencies.Count);

                Assert.Equal(3, msbuildTargets["TestProject1"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectFlowAllAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                ""dependencies"": {
                    ""packageX"": {
                        ""version"": ""1.0.0"",
                        ""suppressParent"": ""none""
                    }
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                var configJson1 = @"{
                ""dependencies"": {
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), "all", string.Empty);

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(2, GetNonEmptyCount(targets["packageX"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageX"].FrameworkAssemblies.Count);
                Assert.Equal(1, targets["packageX"].Dependencies.Count);

                Assert.Equal(2, GetNonEmptyCount(targets["packageY"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageY"].FrameworkAssemblies.Count);
                Assert.Equal(1, targets["packageY"].Dependencies.Count);

                Assert.Equal(2, GetNonEmptyCount(targets["packageZ"].ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].RuntimeAssemblies));
                Assert.Equal(1, targets["packageZ"].FrameworkAssemblies.Count);
                Assert.Equal(0, targets["packageZ"].Dependencies.Count);

                Assert.Equal(3, msbuildTargets["TestProject1"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectsIntersectExcludesAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson3 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0"",
                            ""suppressParent"": ""compile,runtime""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                ""runtimes"": { ""any"": { } }
                }";

                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0"",
                            ""suppressParent"": ""native,runtime""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                  ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                    ""dependencies"": {
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                  ""runtimes"": { ""any"": { } }
                }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await TriangleProjectSetupAsync(workingDir, logger, configJson1, configJson2, configJson3);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(3, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());


                Assert.Equal(0, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageZ"].RuntimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageX"].RuntimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].CompileTimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].CompileTimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].CompileTimeAssemblies));
            }
        }

        [Fact]
        public async Task IncludeType_ExludeDependencyAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {

                var projectJson = @"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0""
                            },
                            ""packageB"": {
                                ""version"": ""1.0.0"",
                                ""exclude"": ""all""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }";

                await CreateAToBAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var a = targets["packageA"];
                var b = targets["packageB"];

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(1, GetNonEmptyCount(a.RuntimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(b.RuntimeAssemblies));
            }
        }

        [Fact]
        public async Task IncludeType_CircularDependencyIsHandledInFlattenAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": ""1.0.0""
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                 }";

                var x = new SimpleTestPackageContext()
                {
                    Id = "packageX",
                    Version = "1.0.0",
                    Include = "runtime"
                };

                var z = new SimpleTestPackageContext()
                {
                    Id = "packageZ",
                    Version = "1.0.0",
                    Include = "runtime"
                };

                z.Dependencies.Add(x);

                var y = new SimpleTestPackageContext()
                {
                    Id = "packageY",
                    Version = "1.0.0",
                    Include = "runtime"
                };

                y.Dependencies.Add(z);

                x.Dependencies.Add(y);

                var packages = new List<SimpleTestPackageContext>()
                {
                    x
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(packages, Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(1, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].RuntimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].RuntimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageY"].CompileTimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageZ"].CompileTimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].CompileTimeAssemblies));
            }
        }

        [Fact]
        public async Task IncludeType_UnifyDependencyEdgesAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageA"": ""1.0.0"",
                            ""packageB"": ""2.0.0""
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                 }";

                var z1 = new SimpleTestPackageContext()
                {
                    Id = "packageZ",
                    Version = "1.0.0",
                    Include = "runtime"
                };

                var y1 = new SimpleTestPackageContext()
                {
                    Id = "packageY",
                    Version = "1.0.0",
                    Include = "runtime"
                };
                y1.Dependencies.Add(z1);

                var z2 = new SimpleTestPackageContext()
                {
                    Id = "packageZ",
                    Version = "2.0.0",
                    Include = "compile"
                };

                var y2 = new SimpleTestPackageContext()
                {
                    Id = "packageY",
                    Version = "2.0.0",
                    Include = "compile"
                };

                y2.Dependencies.Add(z2);

                var a = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "1.0.0"
                };

                a.Dependencies.Add(y1);

                var b = new SimpleTestPackageContext()
                {
                    Id = "packageB",
                    Version = "2.0.0"
                };

                b.Dependencies.Add(y2);

                var packages = new List<SimpleTestPackageContext>()
                {
                    a,
                    b
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(packages, Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageZ"].RuntimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].CompileTimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].CompileTimeAssemblies));
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectWithBuildOverrideToExcludeAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": ""1.0.0""
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0"",
                            ""exclude"": ""build""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(3, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(3, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(0, msbuildTargets["TestProject1"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectWithBuildOverrideAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                    ""dependencies"": {
                        ""packageX"": ""1.0.0""
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(3, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(3, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(3, msbuildTargets["TestProject1"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectNoTransitiveBuildAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                ""dependencies"": {
                },
                ""frameworks"": {
                ""net46"": {}
                },
                ""runtimes"": { ""any"": { } }
            }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(3, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(3, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(0, msbuildTargets["TestProject1"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectNoTransitiveContentAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                    ""dependencies"": {
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(3, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(3, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.True(target.Libraries.Where(lib => lib.Type == LibraryType.Package).All(lib => IsEmptyFolder(lib.ContentFiles)));
            }
        }

        [Fact]
        public async Task IncludeType_ExcludedAndTransitivePackageAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0"",
                            ""suppressParent"": ""all""
                        },
                        ""packageZ"": ""1.0.0""
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                    ""dependencies"": {
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(1, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(1, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal("packageZ", target.Libraries.Single(lib => lib.Type == LibraryType.Package).Name);
                Assert.Equal(1, target.Libraries.Single(lib => lib.Type == LibraryType.Package).CompileTimeAssemblies.Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectReferenceWithBuildReferenceAndTopLevelAsync()
        {
            // Restore Project1
            // Project2 has only build dependencies
            // Project1 -> Project2 -(suppress: all)-> packageX -> packageY -> packageB

            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0"",
                            ""suppressParent"": ""all""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                    ""dependencies"": {
                        ""packageZ"": ""1.0.0""
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(1, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(1, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal("packageZ", target.Libraries.Single(lib => lib.Type == LibraryType.Package).Name);
                Assert.Equal(1, target.Libraries.Single(lib => lib.Type == LibraryType.Package).CompileTimeAssemblies.Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectReferenceWithBuildDependencyAsync()
        {
            // Restore Project1
            // Project2 has only build dependencies
            // Project1 -> Project2 -(suppress: all)-> packageX -> packageY -> packageB

            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0"",
                            ""suppressParent"": ""all""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                    ""dependencies"": {
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(0, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(0, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectReferenceWithBuildTypeDependencyAppliedAsync()
        {
            // Restore Project1
            // Project2 has only build dependencies
            // Project1 -> Project2 -(suppress: all)-> packageX -> packageY -> packageB

            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson2 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0"",
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson1 = @"{
                    ""dependencies"": {
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), "all", string.Empty);
                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);
                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");
                var dependencies = target.Libraries.Single(lib => lib.Name == "TestProject2").Dependencies;

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(3, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(3, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(1, dependencies.Count());
            }
        }

        [Fact]
        public async Task IncludeType_ProjectToProjectReferenceWithBuildTypeDependencyTopLevelAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var configJson1 = @"{
                    ""dependencies"": {
                        ""packageX"": {
                            ""version"": ""1.0.0"",
                            ""type"": ""build""
                        }
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var configJson2 = @"{
                    ""dependencies"": {
                    },
                    ""frameworks"": {
                    ""net46"": {}
                    },
                    ""runtimes"": { ""any"": { } }
                }";

                var repository = Path.Combine(workingDir, "repository");

                var contextY = new SimpleTestPackageContext()
                {
                    Id = "packageY"
                };

                var contextX = new SimpleTestPackageContext()
                {
                    Id = "packageX",
                    Dependencies = new List<SimpleTestPackageContext>() { contextY }
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(repository, contextX);
                await SimpleTestPackageUtility.CreateFullPackageAsync(repository, contextY);

                var result = await ProjectToProjectSetupAsync(workingDir, logger, configJson1, configJson2);
                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(2, target.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.Equal(2, result.LockFile.Libraries.Where(lib => lib.Type == LibraryType.Package).Count());
                Assert.True(target.Libraries.Any(lib => lib.Name == "packageX"));
                Assert.True(target.Libraries.Any(lib => lib.Name == "packageY"));
            }
        }

        [Fact]
        public async Task IncludeType_ProjectIncludesOnlyCompileAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0"",
                                ""include"": ""compile""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(0, GetNonEmptyCount(targets["packageX"].RuntimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageZ"].RuntimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].CompileTimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].CompileTimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].CompileTimeAssemblies));
            }
        }

        [Fact]
        public async Task IncludeType_ProjectOverrideNuspecExcludeAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0""
                            },
                            ""packageY"": ""1.0.0"",
                            ""packageZ"": ""1.0.0""
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                         ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), string.Empty, "build");

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(3, msbuildTargets["TestProject"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectOverrideNuspecExclude_UnderFrameworkAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""frameworks"": {
                            ""net46"": {
                                ""dependencies"": {
                                    ""packageX"": ""1.0.0"",
                                    ""packageY"": ""1.0.0"",
                                    ""packageZ"": ""1.0.0""
                                }
                            }
                        },
                    ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), string.Empty, "build");

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(3, msbuildTargets["TestProject"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_ProjectExcludesBuildFromAllAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {

                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0"",
                                ""exclude"": ""build""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                    ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(0, msbuildTargets["TestProject"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_NuspecExcludesBuildFromTransitiveDependenciesAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                    ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), string.Empty, "build");

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(1, msbuildTargets["TestProject"].Count);
                Assert.Equal("packageX", msbuildTargets["TestProject"].Single());
            }
        }

        [Fact]
        public async Task IncludeType_NuspecFlowsOnlyRuntimeTransitiveDependenciesAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                    ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), "runtime", string.Empty);

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].RuntimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageY"].RuntimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageZ"].RuntimeAssemblies));
                Assert.Equal(1, GetNonEmptyCount(targets["packageX"].CompileTimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageY"].CompileTimeAssemblies));
                Assert.Equal(0, GetNonEmptyCount(targets["packageZ"].CompileTimeAssemblies));
            }
        }

        [Fact]
        public async Task IncludeType_NuspecFlowsContentFromTransitiveDependenciesAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                    ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"), "contentFiles", string.Empty);

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(2, GetNonEmptyCount(targets["packageX"].ContentFiles));
                Assert.Equal(2, GetNonEmptyCount(targets["packageY"].ContentFiles));
                Assert.Equal(2, GetNonEmptyCount(targets["packageZ"].ContentFiles));
            }
        }

        [Fact]
        public async Task IncludeType_ProjectOverridesNoContentFromTransitiveDependenciesAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0""
                            },
                            ""packageB"": {
                                ""version"": ""1.0.0""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                    ""runtimes"": { ""any"": { } }
                    }";

                await CreateAToBAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var a = targets["packageA"];
                var b = targets["packageB"];

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(2, GetNonEmptyCount(a.ContentFiles));
                Assert.Equal(2, GetNonEmptyCount(b.ContentFiles));
            }
        }

        [Fact]
        public async Task IncludeType_NoContentFromTransitiveDependenciesAsync()
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                    ""runtimes"": { ""any"": { } }
                    }";

                await CreateAToBAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var a = targets["packageA"];
                var b = targets["packageB"];

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(2, GetNonEmptyCount(a.ContentFiles));
                Assert.Equal(0, GetNonEmptyCount(b.ContentFiles));

                Assert.Equal(3, b.ContentFiles.Single().Properties.Count);
                Assert.Equal("None", b.ContentFiles.Single().Properties["buildAction"]);
                Assert.Equal("False", b.ContentFiles.Single().Properties["copyToOutput"]);
                Assert.Equal("any", b.ContentFiles.Single().Properties["codeLanguage"]);
            }
        }

        [Theory]
        [InlineData(@"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0"",
                                ""suppressParent"": ""all""
                            },
                            ""packageB"": {
                                ""version"": ""1.0.0"",
                                ""suppressParent"": ""all"",
                                ""exclude"": ""contentFiles""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }")]
        [InlineData(@"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0""
                            },
                            ""packageB"": {
                                ""version"": ""1.0.0"",
                                ""exclude"": ""contentFiles""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }")]
        [InlineData(@"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0""
                            },
                            ""packageB"": {
                                ""version"": ""1.0.0"",
                                ""include"": ""build,compile,runtime,native""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }")]
        [InlineData(@"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0"",
                                ""include"": ""build,compile,runtime,native,contentfiles""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }")]
        [InlineData(@"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0"",
                                ""include"": ""all"",
                                ""exclude"": ""none""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }")]
        [InlineData(@"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0"",
                                ""exclude"": ""none""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }")]
        [InlineData(@"{
                        ""dependencies"": {
                            ""packageA"": {
                                ""version"": ""1.0.0"",
                                ""include"": ""all""
                            }
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }")]
        [InlineData(@"{
                        ""dependencies"": {
                            ""packageA"": ""1.0.0""
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                        ""runtimes"": { ""any"": { } }
                    }")]
        public async Task IncludeType_SingleProjectEquivalentToTheDefaultAsync(string projectJson)
        {
            // Arrange
            var logger = new TestLogger();
            var framework = "net46";

            using (var workingDir = TestDirectory.Create())
            {
                await CreateAToBAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var target = result.LockFile.GetTarget(NuGetFramework.Parse(framework), "any");

                var targets = target.Libraries.ToDictionary(lib => lib.Name);

                var a = targets["packageA"];
                var b = targets["packageB"];

                var msbuildTargets = GetInstalledTargets(workingDir);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(2, GetNonEmptyCount(a.ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(a.NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(a.RuntimeAssemblies));
                Assert.Equal(1, a.FrameworkAssemblies.Count);
                Assert.Equal(1, a.Dependencies.Count);

                Assert.Equal(0, GetNonEmptyCount(b.ContentFiles));
                Assert.Equal(1, GetNonEmptyCount(b.NativeLibraries));
                Assert.Equal(1, GetNonEmptyCount(b.RuntimeAssemblies));
                Assert.Equal(1, b.FrameworkAssemblies.Count);
                Assert.Equal(0, b.Dependencies.Count);

                Assert.Equal(2, msbuildTargets["TestProject"].Count);
            }
        }

        [Fact]
        public async Task IncludeType_TransitiveDependenciesWithTargetsAsync()
        {
            // Arrange
            var logger = new TestLogger();

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0""
                            },
                            ""packageY"": ""1.0.0"",
                            ""packageZ"": ""1.0.0""
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                         ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var msbuildTargets = GetInstalledTargets(workingDir);
                var buildTargets = msbuildTargets["TestProject"].ToList();

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(3, buildTargets.Count);
                Assert.Equal("packageZ", buildTargets[0]);
                Assert.Equal("packageY", buildTargets[1]);
                Assert.Equal("packageX", buildTargets[2]);
            }
        }

        [Fact]
        public async Task IncludeType_MultipleDependenciesWithTargetsAsync()
        {
            // Arrange
            var logger = new TestLogger();

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0""
                            },
                            ""packageY"": ""1.0.0"",
                            ""packageZ"": ""1.0.0"",
                            ""packageA"": ""1.0.0"",
                            ""packageB"": ""1.0.0""
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                         ""runtimes"": { ""any"": { } }
                 }";

                await CreateXYZAsync(Path.Combine(workingDir, "repository"));

                await CreateAToBAsync(Path.Combine(workingDir, "repository"));

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var msbuildTargets = GetInstalledTargets(workingDir);
                var buildTargets = msbuildTargets["TestProject"].ToList();

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(5, buildTargets.Count);
                Assert.Equal("packageZ", buildTargets[0]);
                Assert.Equal("packageY", buildTargets[1]);
                Assert.Equal("packageX", buildTargets[2]);
                Assert.Equal("packageB", buildTargets[3]);
                Assert.Equal("packageA", buildTargets[4]);
            }
        }

        [Fact]
        public async Task IncludeType_DependenciesWithTargetsAsync()
        {
            // Arrange
            var logger = new TestLogger();

            using (var workingDir = TestDirectory.Create())
            {
                var projectJson = @"{
                        ""dependencies"": {
                            ""packageX"": {
                                ""version"": ""1.0.0""
                            },
                            ""packageY"": ""1.0.0"",
                            ""packageZ"": ""1.0.0""
                        },
                        ""frameworks"": {
                            ""net46"": {}
                        },
                         ""runtimes"": { ""any"": { } }
                 }";

                await CreateXyzIndividuallyAsync(Path.Combine(workingDir, "repository"), string.Empty, string.Empty);

                // Act
                var result = await StandardSetupAsync(workingDir, logger, projectJson);

                var msbuildTargets = GetInstalledTargets(workingDir);
                var buildTargets = msbuildTargets["TestProject"].ToList();

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);

                Assert.Equal(3, buildTargets.Count);
                Assert.Equal("packageZ", buildTargets[0]);
                Assert.Equal("packageY", buildTargets[1]);
                Assert.Equal("packageX", buildTargets[2]);
            }
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.Compile)]
        [InlineData(LibraryIncludeFlags.Runtime)]
        public async Task IncludeType_FlowsIntoCentralTransitiveDependencies(LibraryIncludeFlags includeFlags)
        {
            // Arrange
            using (var tmpPath = new SimpleTestPathContext())
            {
                var packageA = new SimpleTestPackageContext { Id = "PackageA", Version = "1.0.0", };
                var logger = new TestLogger();
                var project1Directory = new DirectoryInfo(Path.Combine(tmpPath.SolutionRoot, "Project1"));
                var project2Directory = new DirectoryInfo(Path.Combine(tmpPath.SolutionRoot, "Project2"));
                var project3Directory = new DirectoryInfo(Path.Combine(tmpPath.SolutionRoot, "Project3"));

                var project3Spec = PackageReferenceSpecBuilder.Create("Project3", project3Directory.FullName)
                    .WithTargetFrameworks(new[]
                    {
                        new TargetFrameworkInformation
                        {
                            FrameworkName = NuGetFramework.Parse("net471"),
                            Dependencies = new List<LibraryDependency>(
                                new[]
                                {
                                    new LibraryDependency
                                    {
                                        LibraryRange = new LibraryRange("PackageA", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.All),
                                        VersionCentrallyManaged = true,
                                    },
                                }),
                            CentralPackageVersions = { new KeyValuePair<string, CentralPackageVersion>("PackageA", new CentralPackageVersion("PackageA", VersionRange.Parse("1.0.0"))) },
                        }
                    })
                    .WithCentralPackageVersionsEnabled()
                    .WithCentralPackageTransitivePinningEnabled()
                    .Build()
                    .WithTestRestoreMetadata();

                var project2Spec = PackageReferenceSpecBuilder.Create("Project2", project2Directory.FullName)
                    .WithTargetFrameworks(new[]
                    {
                        new TargetFrameworkInformation
                        {
                            FrameworkName = NuGetFramework.Parse("net471"),
                            Dependencies = new List<LibraryDependency>(),
                            CentralPackageVersions = { new KeyValuePair<string, CentralPackageVersion>("PackageA", new CentralPackageVersion("PackageA", VersionRange.Parse("1.0.0"))) },
                        }
                    })
                    .WithCentralPackageVersionsEnabled()
                    .WithCentralPackageTransitivePinningEnabled()
                    .Build()
                    .WithTestRestoreMetadata()
                    .WithTestProjectReference(project3Spec, privateAssets: (LibraryIncludeFlags.All & (~includeFlags)));


                var project1Spec = PackageReferenceSpecBuilder.Create("Project1", project1Directory.FullName)
                    .WithTargetFrameworks(new[]
                    {
                        new TargetFrameworkInformation
                        {
                            FrameworkName = NuGetFramework.Parse("net471"),
                            Dependencies = new List<LibraryDependency>(),
                            CentralPackageVersions = { new KeyValuePair<string, CentralPackageVersion>("PackageA", new CentralPackageVersion("PackageA", VersionRange.Parse("1.0.0"))) },
                        }
                    })
                    .WithCentralPackageVersionsEnabled()
                    .WithCentralPackageTransitivePinningEnabled()
                    .Build()
                    .WithTestRestoreMetadata()
                    .WithTestProjectReference(project2Spec);

                var restoreContext = new RestoreArgs()
                {
                    Sources = new List<string>() { tmpPath.PackageSource },
                    GlobalPackagesFolder = tmpPath.UserPackagesFolder,
                    Log = logger,
                    CacheContext = new TestSourceCacheContext(),
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(tmpPath.PackageSource, packageA);
                var request = await ProjectTestHelpers.GetRequestAsync(restoreContext, project1Spec, project2Spec, project3Spec);

                // Act
                var command1 = new RestoreCommand(request);
                var result1 = await command1.ExecuteAsync();
                var lockFile1 = result1.LockFile;

                // Assert
                Assert.True(result1.Success);
                Assert.Equal(includeFlags, lockFile1.CentralTransitiveDependencyGroups.Single().TransitiveDependencies.Single().IncludeType);

                var targetLibrary = lockFile1.Targets.Single().Libraries.Single(x => x.Name == packageA.Id);
                Assert.EndsWith((includeFlags & LibraryIncludeFlags.Compile) == LibraryIncludeFlags.Compile ? "a.dll" : "_._", targetLibrary.CompileTimeAssemblies.Single().Path);
                Assert.EndsWith((includeFlags & LibraryIncludeFlags.Runtime) == LibraryIncludeFlags.Runtime ? "a.dll" : "_._", targetLibrary.RuntimeAssemblies.Single().Path);
            }
        }

        private async Task<RestoreResult> ProjectToProjectSetupAsync(
            string workingDir,
            NuGet.Common.ILogger logger,
            string configJson1,
            string configJson2)
        {
            // Arrange
            var repository = Path.Combine(workingDir, "repository");
            Directory.CreateDirectory(repository);

            var projectDir = Path.Combine(workingDir, "project");
            Directory.CreateDirectory(projectDir);

            var packagesDir = Path.Combine(workingDir, "packages");
            Directory.CreateDirectory(packagesDir);

            var testProject1Dir = Path.Combine(projectDir, "TestProject1");
            Directory.CreateDirectory(testProject1Dir);

            var testProject2Dir = Path.Combine(projectDir, "TestProject2");
            Directory.CreateDirectory(testProject2Dir);

            var specPath1 = Path.Combine(testProject1Dir, "project.json");
            var spec1 = JsonPackageSpecReader.GetPackageSpec(configJson1, "TestProject1", specPath1).EnsureProjectJsonRestoreMetadata();

            using (var writer = new StreamWriter(File.OpenWrite(specPath1)))
            {
                writer.WriteLine(configJson1.ToString());
            }

            var specPath2 = Path.Combine(testProject2Dir, "project.json");
            var spec2 = JsonPackageSpecReader.GetPackageSpec(configJson2, "TestProject2", specPath2).EnsureProjectJsonRestoreMetadata();

            using (var writer = new StreamWriter(File.OpenWrite(specPath2)))
            {
                writer.WriteLine(configJson2.ToString());
            }

            var sources = new List<PackageSource>
            {
                new PackageSource(repository)
            };

            var request = new TestRestoreRequest(spec1, sources, packagesDir, logger)
            {
                LockFilePath = Path.Combine(testProject1Dir, "project.lock.json"),

                ExternalProjects = new List<ExternalProjectReference>()
            {
                new ExternalProjectReference(
                    "TestProject1",
                    spec1,
                    Path.Combine(testProject1Dir, "TestProject1.csproj"),
                    new string[] { "TestProject2" }),

                new ExternalProjectReference(
                    "TestProject2",
                    spec2,
                    Path.Combine(testProject1Dir, "TestProject2.csproj"),
                    Enumerable.Empty<string>())
            }
            };

            var command = new RestoreCommand(request);

            // Act
            var result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            return result;
        }

        private async Task<RestoreResult> TriangleProjectSetupAsync(
            string workingDir,
            NuGet.Common.ILogger logger,
            string configJson1,
            string configJson2,
            string configJson3)
        {
            // Arrange
            var repository = Path.Combine(workingDir, "repository");
            Directory.CreateDirectory(repository);
            var projectDir = Path.Combine(workingDir, "project");
            Directory.CreateDirectory(projectDir);
            var packagesDir = Path.Combine(workingDir, "packages");
            Directory.CreateDirectory(packagesDir);
            var testProject1Dir = Path.Combine(projectDir, "TestProject1");
            Directory.CreateDirectory(testProject1Dir);
            var testProject2Dir = Path.Combine(projectDir, "TestProject2");
            Directory.CreateDirectory(testProject2Dir);
            var testProject3Dir = Path.Combine(projectDir, "TestProject3");
            Directory.CreateDirectory(testProject3Dir);

            var specPath1 = Path.Combine(testProject1Dir, "project.json");
            var spec1 = JsonPackageSpecReader.GetPackageSpec(configJson1, "TestProject1", specPath1).EnsureProjectJsonRestoreMetadata();
            using (var writer = new StreamWriter(File.OpenWrite(specPath1)))
            {
                writer.WriteLine(configJson1);
            }

            var specPath2 = Path.Combine(testProject2Dir, "project.json");
            var spec2 = JsonPackageSpecReader.GetPackageSpec(configJson2, "TestProject2", specPath2).EnsureProjectJsonRestoreMetadata();
            using (var writer = new StreamWriter(File.OpenWrite(specPath2)))
            {
                writer.WriteLine(configJson2);
            }

            var specPath3 = Path.Combine(testProject3Dir, "project.json");
            var spec3 = JsonPackageSpecReader.GetPackageSpec(configJson3, "TestProject3", specPath3).EnsureProjectJsonRestoreMetadata();
            using (var writer = new StreamWriter(File.OpenWrite(specPath3)))
            {
                writer.WriteLine(configJson3);
            }

            var sources = new List<PackageSource>
            {
                new PackageSource(repository)
            };

            var request = new TestRestoreRequest(spec1, sources, packagesDir, logger)
            {
                LockFilePath = Path.Combine(testProject1Dir, "project.lock.json"),
                ExternalProjects = new List<ExternalProjectReference>()
            {
                new ExternalProjectReference("TestProject1", spec1, null, new string[] { "TestProject2", "TestProject3" }),
                new ExternalProjectReference("TestProject2", spec2, null, Enumerable.Empty<string>()),
                new ExternalProjectReference("TestProject3", spec3, null, Enumerable.Empty<string>())
            }
            };

            var command = new RestoreCommand(request);

            // Act
            var result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            return result;
        }

        private async Task<RestoreResult> StandardSetupAsync(
            string workingDir,
            NuGet.Common.ILogger logger,
            string configJson)
        {
            // Arrange
            var repository = Path.Combine(workingDir, "repository");
            Directory.CreateDirectory(repository);
            var projectDir = Path.Combine(workingDir, "project");
            Directory.CreateDirectory(projectDir);
            var packagesDir = Path.Combine(workingDir, "packages");
            Directory.CreateDirectory(packagesDir);
            var testProjectDir = Path.Combine(projectDir, "TestProject");
            Directory.CreateDirectory(testProjectDir);

            var sources = new List<PackageSource>
            {
                new PackageSource(repository)
            };

            var specPath = Path.Combine(testProjectDir, "project.json");
            var spec = JsonPackageSpecReader.GetPackageSpec(configJson, "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

            var request = new TestRestoreRequest(spec, sources, packagesDir, logger)
            {
                LockFilePath = Path.Combine(testProjectDir, "project.lock.json")
            };

            request.ExternalProjects.Add(
                new ExternalProjectReference(
                    "TestProject",
                    spec,
                    Path.Combine(testProjectDir, "TestProject.csproj"),
                    new string[] { }));


            var command = new RestoreCommand(request);

            // Act
            var result = await command.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            return result;
        }

        private static async Task CreateAToBAsync(string repositoryDir)
        {
            var b = new SimpleTestPackageContext()
            {
                Id = "packageB",
                Version = "1.0.0"
            };

            var a = new SimpleTestPackageContext()
            {
                Id = "packageA",
                Version = "1.0.0"
            };
            a.Dependencies.Add(b);

            var packages = new List<SimpleTestPackageContext>()
            {
                a
            };

            await SimpleTestPackageUtility.CreatePackagesAsync(packages, repositoryDir);
        }

        private static async Task CreateXYZAsync(string repositoryDir)
        {
            await CreateXYZAsync(repositoryDir, string.Empty, string.Empty);
        }

        private static async Task CreateXYZAsync(string repositoryDir, string include, string exclude)
        {
            var z = new SimpleTestPackageContext()
            {
                Id = "packageZ",
                Version = "1.0.0",
                Include = include,
                Exclude = exclude
            };

            var y = new SimpleTestPackageContext()
            {
                Id = "packageY",
                Version = "1.0.0",
                Include = include,
                Exclude = exclude
            };
            y.Dependencies.Add(z);

            var x = new SimpleTestPackageContext()
            {
                Id = "packageX",
                Version = "1.0.0",
                Include = include,
                Exclude = exclude
            };
            x.Dependencies.Add(y);

            var packages = new List<SimpleTestPackageContext>()
            {
                x
            };

            await SimpleTestPackageUtility.CreatePackagesAsync(packages, repositoryDir);
        }

        private static async Task CreateXyzIndividuallyAsync(string repositoryDir, string include, string exclude)
        {
            var z = new SimpleTestPackageContext()
            {
                Id = "packageZ",
                Version = "1.0.0",
                Include = include,
                Exclude = exclude
            };

            var y = new SimpleTestPackageContext()
            {
                Id = "packageY",
                Version = "1.0.0",
                Include = include,
                Exclude = exclude
            };

            var x = new SimpleTestPackageContext()
            {
                Id = "packageX",
                Version = "1.0.0",
                Include = include,
                Exclude = exclude
            };

            var packages = new List<SimpleTestPackageContext>()
            {
                x, y, z
            };

            await SimpleTestPackageUtility.CreatePackagesAsync(packages, repositoryDir);
        }

        private bool IsEmptyFolder(IEnumerable<LockFileItem> group)
        {
            return group.SingleOrDefault()?.Path.EndsWith("/_._") == true;
        }

        private int GetNonEmptyCount(IEnumerable<LockFileItem> group)
        {
            return group.Where(e => !e.Path.EndsWith("/_._")).Count();
        }

        private static Dictionary<string, HashSet<string>> GetInstalledTargets(string workingDir)
        {
            var result = new Dictionary<string, HashSet<string>>();
            var projectDir = new DirectoryInfo(Path.Combine(workingDir, "project"));

            foreach (var dir in projectDir.GetDirectories())
            {
                result.Add(dir.Name, new HashSet<string>());

                var targets = dir.GetFiles("*.nuget.targets").SingleOrDefault();

                if (targets != null)
                {
                    using (var stream = targets.OpenRead())
                    {
                        var xml = XDocument.Load(stream);

                        foreach (var package in xml.Descendants()
                            .Where(node => node.Name.LocalName == "Import")
                            .Select(node => node.Attribute(XName.Get("Project")))
                            .Select(file => Path.GetFileNameWithoutExtension(file.Value)))
                        {
                            result[dir.Name].Add(package);
                        }
                    }
                }
            }

            return result;
        }
    }
}
