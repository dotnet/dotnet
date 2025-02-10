// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.FuncTest
{
    public partial class RestoreCommandTests
    {
        // A -> - X - B/PrivateAssets=All -> C
        //        X -> D -> E-> B -> C
        // X, D & E
        [Fact]
        public async Task RestoreCommand_HigherLevelSuppressionsWin_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Setup project
            var A = ProjectTestHelpers.GetPackageSpec("A", pathContext.SolutionRoot, framework: "net5.0");
            var X = ProjectTestHelpers.GetPackageSpec("X", pathContext.SolutionRoot, framework: "net5.0");
            var B = ProjectTestHelpers.GetPackageSpec("B", pathContext.SolutionRoot, framework: "net5.0");
            var C = ProjectTestHelpers.GetPackageSpec("C", pathContext.SolutionRoot, framework: "net5.0");
            var D = ProjectTestHelpers.GetPackageSpec("D", pathContext.SolutionRoot, framework: "net5.0");
            var E = ProjectTestHelpers.GetPackageSpec("E", pathContext.SolutionRoot, framework: "net5.0");

            X = X.WithTestProjectReference(B, LibraryIncludeFlags.All);
            X = X.WithTestProjectReference(D);
            D = D.WithTestProjectReference(E);
            E = E.WithTestProjectReference(B);
            B = B.WithTestProjectReference(C);
            A = A.WithTestProjectReference(X);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, A, X, B, C, D, E);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("D");
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("E");
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("X");
        }

        // LightGbm (project) -> Microsoft.ML.LightGbm -> LightGBM 2.0.0 (package)
        // This should result in an error with the cycle
        [Fact]
        public async Task RestoreCommand_WithCycleContainingProject_FailsAndLogsMessage()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("Microsoft.ML.LightGbm", "4.0.1")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("LightGBM", "3.3.5"),
                    ]
                });

            PackageSpec projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
                "LightGbm",
                pathContext.SolutionRoot,
                @"
                {
                    ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""Microsoft.ML.LightGbm"": ""4.0.1""
                        }
                    }
                    }
                }");

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec);

            // Additional assert
            result.Success.Should().BeFalse();

            result.LogMessages.Should().HaveCount(1);

            IAssetsLogMessage logMessage = result.LogMessages.Single();
            logMessage.Code.Should().Be(NuGetLogCode.NU1108);
            logMessage.Message.Should().Contain("LightGbm -> Microsoft.ML.LightGbm 4.0.1 -> LightGBM (>= 3.3.5)");

            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("Microsoft.ML.LightGbm");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("4.0.1"));
        }

        // Project 1 -> a 1.0.0 -> b 1.0.0
        //           -> b 2.0.0
        // Does not install b 1.0.0 since it is eclipsed by a direct package reference
        [Fact]
        public async Task RestoreCommand_WithDirectDependency_DoesNotDownloadOtherVersions()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("a", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "1.0.0"),
                    ]
                },
                new SimpleTestPackageContext("b", "2.0.0"));

            var project1spec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
                "Project1",
                pathContext.SolutionRoot,
                @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"": ""1.0.0"",
                                ""b"": ""2.0.0""
                        }
                    }
                  }
                }");

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1spec);

            // Additional assert
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));

            result.RestoreGraphs.Should().HaveCount(1);
            result.RestoreGraphs.First().Install.Should().HaveCount(2);

            string.Join(" ", result.RestoreGraphs.First().Install.Select(i => $"{i.Library.Name}/{i.Library.Version}").OrderBy(i => i)).Should().Be("a/1.0.0 b/2.0.0");
        }

        // Project 1 -> X 1.0 -> B 2.0 -> E 1.0
        //           -> C 2.0 -> D 1.0 -> B 3.0
        // Expected: X 1.0, B 3.0, C 2.0, D 1.0
        [Fact]
        public async Task RestoreCommand_WithNewPackageEvictedByVersionBump()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "2.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("e", "1.0"),
                        ]
                    },
                ]
            };

            var packageC = new SimpleTestPackageContext("c", "2.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("d", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "3.0.0")
                            {
                                Dependencies =
                                [
                                    /*new SimpleTestPackageContext("e", "1.0.0")*/
                                ]
                            },
                        ]
                    },
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageC);

            var projectSpec = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"":  ""1.0.0"",
                                ""c"": ""2.0.0""
                        }
                    }
                  }
                }";
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, projectSpec));

            // Assert
            result.Success.Should().BeTrue();
            result.LogMessages.Should().HaveCount(0);

            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("3.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("2.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("d");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> Project 2 -> a (null)
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RestoreCommand_WithNullPackageVersion_AndRaisesErrorForOneProjectAndNotTheOther(bool isCPMEnabled)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageA = new SimpleTestPackageContext("a", "1.0.0");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            var project2spec = ProjectTestHelpers.GetPackageSpec("Project2",
                pathContext.SolutionRoot,
                framework: "net472");

            project2spec.TargetFrameworks[0] = new TargetFrameworkInformation(project2spec.TargetFrameworks[0])
            {
                Dependencies = [
                    new LibraryDependency(new LibraryRange(
                        "a",
                        versionRange: isCPMEnabled ? null : VersionRange.All,
                        LibraryDependencyTarget.PackageProjectExternal))
                    ]
            };

            var project1spec = ProjectTestHelpers.GetPackageSpec("Project1",
                pathContext.SolutionRoot,
                framework: "net472")
                .WithTestProjectReference(project2spec);

            project1spec.RestoreMetadata.CentralPackageVersionsEnabled = isCPMEnabled;
            project2spec.RestoreMetadata.CentralPackageVersionsEnabled = isCPMEnabled;

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1spec, project2spec);

            if (isCPMEnabled)
            {
                // Additional assert
                result.Success.Should().BeTrue();
                result.LogMessages.Should().BeEmpty();

                result.LockFile.Targets.Should().HaveCount(1);
                result.LockFile.Targets[0].Libraries.Should().HaveCount(1);
                result.LockFile.Targets[0].Libraries[0].Name.Should().Be("Project2");
                result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            }
            else
            {
                result.Success.Should().BeTrue();
                result.LogMessages.Select(e => e.Code).Should().BeEquivalentTo([NuGetLogCode.NU1602]);

                result.LockFile.Targets.Should().HaveCount(1);
                result.LockFile.Targets[0].Libraries.Should().HaveCount(2);
                result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
                result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

                result.LockFile.Targets[0].Libraries[1].Name.Should().Be("Project2");
                result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
            }
        }

        // Project 1 -> a 1.0.0 -> b 1.0.0
        //                      -> c 1.0.0 -> b 2.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngrade_RespectsDowngrade_AndRaisesWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                    new SimpleTestPackageContext("c", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "2.0.0")
                        ]
                    },
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            var project1spec = ProjectTestHelpers.GetPackageSpec("Project1",
                pathContext.SolutionRoot,
                framework: "net472",
                dependencyName: "a");

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1spec);

            // Additional assert
            result.Success.Should().BeTrue();
            result.LogMessages.Select(e => e.Code).Should().BeEquivalentTo([NuGetLogCode.NU1605]);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> a 1.0.0 -> b 1.0.0
        //                      -> c 1.0.0 -> -> d 1.0.0 -> b 2.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeAndDepthDifferenceMoreThanOne_RespectsDowngrade_AndRaisesWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                    new SimpleTestPackageContext("c", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("d", "1.0.0")
                            {
                                Dependencies =
                                [
                                    new SimpleTestPackageContext("b", "2.0.0"),
                                ]
                            },
                        ]
                    },
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            var project1spec = ProjectTestHelpers.GetPackageSpec("Project1",
                pathContext.SolutionRoot,
                framework: "net472",
                dependencyName: "a");

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1spec);

            // Additional assert
            result.Success.Should().BeTrue();
            result.LogMessages.Select(e => e.Code).Should().BeEquivalentTo([NuGetLogCode.NU1605]);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("d");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> a 1.0.0 -> b 1.0.0
        //                      -> c 1.0.0 -> b 2.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeAndMissingVersion_RespectsDowngrade_AndRaisesWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var packageB100 = new SimpleTestPackageContext("b", "1.0.0");
            var packageB200 = new SimpleTestPackageContext("b", "2.0.0");
            var packageC = new SimpleTestPackageContext("c", "1.0.0")
            {
                Dependencies =
                [
                    packageB200,
                ]
            };

            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    packageB100,
                    packageC,
                ]
            };

            await SimpleTestPackageUtility.CreatePackagesWithoutDependenciesAsync(
                pathContext.PackageSource,
                packageA,
                packageB200,
                packageC);

            var project1spec = ProjectTestHelpers.GetPackageSpec("Project1",
                pathContext.SolutionRoot,
                framework: "net472",
                dependencyName: "a");

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1spec);

            // Additional assert
            result.Success.Should().BeTrue();
            result.LogMessages.Should().HaveCount(1);
            result.LogMessages.Select(e => e.Code).Should().BeEquivalentTo([NuGetLogCode.NU1603]);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // P1 -> a 1.0.0 -> b 1.0.0
        //                  c 1.0.0 -> b 3.0.0
        //       d 1.0.0 -> b 2.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeAndTransitivePinning_ElevatesTransitiveToDirect_AndDoesNotRaiseWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                    new SimpleTestPackageContext("c", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "3.0.0")
                        ]
                    },
                ]
            };

            var packageD = new SimpleTestPackageContext("d", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "2.0.0"),
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageD);

            var projectSpec = @"
                {
                  ""restore"": {
                    ""centralPackageVersionsManagementEnabled"": true,
                    ""CentralPackageTransitivePinningEnabled"": true,
                  },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""d"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                },
                                ""a"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                },
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[1.0.0,)"",
                            ""c"": ""[1.0.0,)"",
                        }
                    }
                  }
                }";

            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, projectSpec));

            // Assert
            result.Success.Should().BeTrue();
            result.LogMessages.Should().BeEmpty();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("3.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("d");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> f 1.0.0 -> a 1.0.0 -> b 1.0.0
        //                                 -> c 1.0.0 -> b 2.0.0
        // Centrally managed versions f & a, f 1.0.0 and a 1.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeAndTransitivePinning_RespectsDowngrade_AndRaisesWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageF = new SimpleTestPackageContext("f", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("a", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("c", "1.0.0")
                            {
                                Dependencies =
                                [
                                    new SimpleTestPackageContext("b", "2.0.0"),
                                ]
                            },
                            new SimpleTestPackageContext("b", "1.0.0"),
                        ]
                    },
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageF);

            var project1 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""f"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                },
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[1.0.0,)"",
                            ""f"": ""[1.0.0,)"",
                        }
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec);

            result.Success.Should().BeTrue();
            result.LogMessages.Select(e => e.Code).Should().BeEquivalentTo([NuGetLogCode.NU1605]);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("f");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> f 1.0.0 -> a 1.0.0 -> b 1.0.0
        //                                 -> c 1.0.0 -> b 2.0.0
        // Centrally managed versions f & a, b, f 1.0.0, a 2.0.0 and b 1.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeAndTransitivePinningToDowngradedVersion_Fails()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageF = new SimpleTestPackageContext("f", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("a", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "1.0.0"),
                            new SimpleTestPackageContext("c", "1.0.0")
                            {
                                Dependencies =
                                [
                                    new SimpleTestPackageContext("b", "2.0.0")
                                ]
                            },
                        ]
                    },
                ]
            };

            var packageA200 = new SimpleTestPackageContext("a", "2.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                    new SimpleTestPackageContext("c", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "2.0.0"),
                        ]
                    },
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageF,
                packageA200);

            var project1 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""f"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                },
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[2.0.0,)"",
                            ""f"": ""[1.0.0,)"",
                            ""b"": ""[1.0.0,)"",
                        }
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);

            // Act & Assert
            (RestoreResult result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec);

            result.Success.Should().BeFalse();
            result.LogMessages.Select(e => e.Code).Should().BeEquivalentTo([NuGetLogCode.NU1109]);

            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("f");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> f 1.0.0 -> a 1.0.0 -> b 1.0.0
        //                                 -> c 1.0.0 -> b 2.0.0
        // Centrally managed versions f & a, f 1.0.0 and a 2.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeAndTransitivePinningToHigherVersion_RespectsDowngrade_AndRaisesWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageF = new SimpleTestPackageContext("f", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("a", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "1.0.0"),
                            new SimpleTestPackageContext("c", "1.0.0")
                            {
                                Dependencies =
                                [
                                    new SimpleTestPackageContext("b", "2.0.0"),
                                ]
                            },
                        ]
                    },
                ]
            };

            var packageA200 = new SimpleTestPackageContext("a", "2.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                    new SimpleTestPackageContext("c", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "2.0.0"),
                        ]
                    },
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageF,
                packageA200);

            var project1 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""f"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                },
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[2.0.0,)"",
                            ""f"": ""[1.0.0,)"",
                        }
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec);

            result.Success.Should().BeTrue();
            result.LogMessages.Select(e => e.Code).Should().BeEquivalentTo([NuGetLogCode.NU1605]);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("f");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> d 1.0.0 -> b 1.0.0
        // Project 1 -> a 1.0.0 -> b 1.0.0
        //                      -> c 1.0.0 -> b 2.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeWithMultipleAncestors_RespectsDowngrade_AndRaisesWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                    new SimpleTestPackageContext("c", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "2.0.0"),
                        ]
                    },
                ]
            };

            var packageD = new SimpleTestPackageContext("d", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageD);

            var projectSpec = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""d"": ""1.0.0"",
                                ""a"":  ""1.0.0""
                        }
                    }
                  }
                }";

            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, projectSpec));

            // Assert
            result.Success.Should().BeTrue();
            result.LogMessages.Select(e => e.Code).Should().BeEquivalentTo([NuGetLogCode.NU1605]);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("d");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> d 1.0.0 -> e 1.0.0 -> b 3.0.0
        // Project 1 -> a 1.0.0 -> b 1.0.0
        //                      -> c 1.0.0 -> b 3.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeWithMultipleAncestorsAndCousin_CousinDependencyWins()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                    new SimpleTestPackageContext("c", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "3.0.0")
                        ]
                    },
                ]
            };

            var packageD = new SimpleTestPackageContext("d", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("e", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "3.0.0"),
                        ]
                    }
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageD);

            var projectSpec = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"":  ""1.0.0"",
                                ""d"": ""1.0.0""
                        }
                    }
                  }
                }";

            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, projectSpec));

            // Assert
            result.Success.Should().BeTrue();
            result.LogMessages.Should().BeEmpty();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(5);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("3.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("d");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[4].Name.Should().Be("e");
            result.LockFile.Targets[0].Libraries[4].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project 1 -> d 1.0.0 -> b 2.0.0
        // Project 1 -> a 1.0.0 -> b 1.0.0
        //                      -> c 1.0.0 -> b 3.0.0
        [Fact]
        public async Task RestoreCommand_WithPackageDrivenDowngradeWithMultipleAncestorsAndCousin_RespectsDowngrade_AndDoesNotRaiseWarning()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                    new SimpleTestPackageContext("c", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("b", "3.0.0")
                        ]
                    },
                ]
            };

            var packageD = new SimpleTestPackageContext("d", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "2.0.0"),
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageD);

            var projectSpec = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""d"": ""1.0.0"",
                                ""a"": ""1.0.0""
                        }
                    }
                  }
                }";

            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, projectSpec));

            // Assert
            result.Success.Should().BeTrue();
            result.LogMessages.Should().BeEmpty();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("d");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        [Fact]
        public async Task RestoreCommand_WithPackageWithAMissingDependencyVersion_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesWithoutDependenciesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("a", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", null),
                    ],
                },
                new SimpleTestPackageContext("b", "2.0.0"));

            // Setup project
            var spec1 = @"
                {
                  ""runtimes"": {
                        ""win"": {}
                  },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""a"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            },
                            ""b"": {
                                ""version"": ""[2.0.0,)"",
                                ""target"": ""Package"",
                            }
                        }
                    }
                  }
                }";

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, spec1);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1);
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(2);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
        }

        // P1 -> P2 -> B 2.*-preview.*
        // P1 -> A -> B 2.0.0-preview.1
        [Theory]
        [InlineData("*-rc.*")]
        [InlineData("2.*-preview.*")]
        [InlineData("2.0.*-preview.*")]
        [InlineData("2.0.0.*-preview.*")]
        public async Task RestoreCommand_WithPrereleaseTransitiveVersion_VerifiesEquivalency(string floatingVersion)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var versionRange = VersionRange.Parse(floatingVersion);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("a", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "2.0.0-preview.1"),
                    ],
                },
                new SimpleTestPackageContext("b", "2.0.0"));

            // Setup project
            var spec1 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""a"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            },
                        }
                    }
                  }
                }";

            var spec2 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""b"": {
                                ""version"": """ + versionRange.ToString() + @""",
                                ""target"": ""Package"",
                            },
                        }
                    }
                  }
                }";

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, spec1);
            var project2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, spec2);
            project1 = project1.WithTestProjectReference(project2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1, project2);
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // P1 -> P2 -> A (VersionOverride) -> B
        // P1 -> P2 -> B (PrivateAssets) VersionOverride
        // P1 has 2.0.0 for both packages, P2 has 1.0.0 for both packages
        // Similar as RestoreCommand_WithVersionOverridePrivateAssetsAndTransitivePinningEnabled_VerifiesEquivalency, because of RestoreCommand_WithPrivateAssetsOfTransitivePackage_VerifiesEquivalency, the new algorithm is arguably more correct.
        [Fact]
        public async Task RestoreCommand_WithPrivateAssetsAndTransitivePinningEnabled_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                ]
            };
            var packageA200 = new SimpleTestPackageContext("a", "2.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "2.0.0"),
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageA200);

            var project1 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[2.0.0,)"",
                            ""b"": ""[2.0.0,)""
                        }
                    }
                  }
                }";

            var project2 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                },
                                ""b"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true,
                                    ""suppressParent"": ""All""
                                }
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[1.0.0,)"",
                            ""b"": ""[1.0.0,)""
                        }
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, project2);
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            (var result2, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec2);
            result2.LockFile.Targets.Should().HaveCount(1);
            result2.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result2.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result2.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result2.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result2.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // P1 -> P2 -> A 1.0.0 -> B 1.0.0
        // P1 -> P2 -> B (PrivateAssets) 1.0.0
        // P1 gets A and P2
        // P2 gets A & B
        [Fact]
        public async Task RestoreCommand_WithPrivateAssetsOfTransitivePackage_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            var project1 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                        }
                    }
                  }
                }";

            var project2 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                },
                                ""b"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""suppressParent"": ""All""
                                }
                        },
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, project2);
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            (var result2, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec2);
            result2.LockFile.Targets.Should().HaveCount(1);
            result2.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result2.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result2.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result2.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result2.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project1 -> Project2 -> (PrivateAssets) X 1.0.0
        [Fact]
        public async Task RestoreCommand_WithProjectReferenceWithSuppressedDependencies_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpec("Project1", pathContext.SolutionRoot, framework: "net5.0");
            var projectSpec2 = ProjectTestHelpers.GetPackageSpec("Project2", pathContext.SolutionRoot, framework: "net5.0", dependencyName: "a");
            projectSpec2.TargetFrameworks[0] = new TargetFrameworkInformation(projectSpec2.TargetFrameworks[0])
            {
                Dependencies =
                [
                    new LibraryDependency(projectSpec2.TargetFrameworks[0].Dependencies[0])
                    {
                        SuppressParent = LibraryIncludeFlags.All
                    },
                ]
            };
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        [Fact]
        public async Task RestoreCommand_WithRuntimeSpecificPackage_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("a", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "1.0.0"),
                    ],
                    RuntimeJson = @"{
                  ""runtimes"": {
                    ""win"": {
                            ""a"": {
                                ""runtime.a"": ""1.0.0""
                            }
                          }
                        }
                  }"
                },
                new SimpleTestPackageContext("runtime.a", "1.0.0"));

            var spec1 = @"
                {
                  ""runtimes"": {
                        ""win"": {}
                  },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""a"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                        }
                    }
                  }
                }";

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, spec1);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1);
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(2);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result.LockFile.Targets[1].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[1].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[1].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[1].Libraries[2].Name.Should().Be("runtime.a");
        }

        [Fact]
        public async Task RestoreCommand_WithRuntimeSpecificPackageAndALockFile_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("a", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "1.0.0"),
                    ],
                    RuntimeJson = @"{
                  ""runtimes"": {
                    ""win"": {
                            ""a"": {
                                ""runtime.a"": ""1.0.0""
                            }
                          }
                        }
                  }"
                },
                new SimpleTestPackageContext("runtime.a", "1.0.0"));

            var spec1 = @"
                {
                  ""runtimes"": {
                        ""win"": {}
                  },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""a"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                        }
                    }
                  }
                }";

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, spec1);
            project1.RestoreMetadata.RestoreLockProperties = new RestoreLockProperties("true", null, false);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1);
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(2);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result.LockFile.Targets[1].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[1].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[1].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[1].Libraries[2].Name.Should().Be("runtime.a");

            await result.CommitAsync(NullLogger.Instance, CancellationToken.None);
            File.Delete(result.LockFilePath); // Ensure restore happens again.

            // Act & Assert
            (var resultWithLockFile, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1);
        }

        // Project1 -> (project) ProjectAndPackage 1.0.0 -> Project3 -> Project4 -> PackageB 2.0.0
        //          -> packageA 1.0.0 -> PackageB 1.0.0
        //                            -> ProjectAndPackage 2.0.0
        [Theory]
        [InlineData("3.0.0")]
        [InlineData("0.9.0")]
        public async Task RestoreCommand_WithSameIdProjectAndPackage_PrefersProject_AndVerifiesEquivalency(string projectAndPackageVersion)
        {
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("packageA", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("packageB", "1.0.0"),
                    new SimpleTestPackageContext("ProjectAndPackage", projectAndPackageVersion),
                ]
            };
            var packageB = new SimpleTestPackageContext("packageB", "2.0.0");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageB);

            var rootProject = @"
        {
          ""frameworks"": {
            ""net472"": {
                ""dependencies"": {
                        ""packageA"": {
                            ""version"": ""[1.0.0,)"",
                            ""target"": ""Package"",
                        },
                }
            }
          }
        }";

            var leafProject = @"
        {
          ""version"": ""4.0.0"",
          ""frameworks"": {
            ""net472"": {
                ""dependencies"": {
                        ""packageB"": {
                            ""version"": ""[2.0.0,)"",
                            ""target"": ""Package"",
                        },
                }
            }
          }
        }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, rootProject);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpec("ProjectAndPackage", framework: "net472");
            var projectSpec3 = ProjectTestHelpers.GetPackageSpec("Project3", framework: "net472");
            var projectSpec4 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project4", pathContext.SolutionRoot, leafProject);
            projectSpec3 = projectSpec3.WithTestProjectReference(projectSpec4);
            projectSpec2 = projectSpec2.WithTestProjectReference(projectSpec3);
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2, projectSpec3, projectSpec4);
            result.LockFile.PackageSpec.RestoreMetadata.CentralPackageTransitivePinningEnabled.Should().BeFalse();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(5);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("packageA");
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("packageB");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project3");
            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("Project4");
            result.LockFile.Targets[0].Libraries[4].Name.Should().Be("ProjectAndPackage");
            result.LockFile.Targets[0].Libraries[4].Type.Should().Be("project");
        }

        // Project1 -> Project2 -> X 3.0.0
        //          -> Project3 -> X 2.0.0 (project)
        // Project is chosen, despite package having higher version.
        [Fact]
        public async Task RestoreCommand_WithSameTransitiveProjectPackageId_ChoosesProject_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("a", "3.0.0"));

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpec("Project1", pathContext.SolutionRoot, framework: "net5.0");
            var project2 = ProjectTestHelpers.GetPackageSpec("Project2", pathContext.SolutionRoot, framework: "net5.0");
            var project3 = ProjectTestHelpers.GetPackageSpec("Project3", pathContext.SolutionRoot, framework: "net5.0", dependencyName: "a", dependencyVersion: "3.0.0");

            var projectA = ProjectTestHelpers.GetPackageSpec("a", pathContext.SolutionRoot, framework: "net5.0");
            projectA.Version = new NuGetVersion("2.0.0");

            project2 = project2.WithTestProjectReference(projectA);
            project1 = project1.WithTestProjectReference(project2);
            project1 = project1.WithTestProjectReference(project3);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1, project2, project3, projectA);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[0].Type.Should().Be("project");
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project3");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project1 -> Project2 -> X 1.0.0
        //          -> Project3 -> X 2.0.0 (project)
        // Project is chosen, cause higher.
        [Fact]
        public async Task RestoreCommand_WithSameTransitiveProjectPackageId_ChoosesProjectWithHigherVersion_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("a", "1.0.0"));

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpec("Project1", pathContext.SolutionRoot, framework: "net5.0");
            var project2 = ProjectTestHelpers.GetPackageSpec("Project2", pathContext.SolutionRoot, framework: "net5.0");
            var project3 = ProjectTestHelpers.GetPackageSpec("Project3", pathContext.SolutionRoot, framework: "net5.0", dependencyName: "a");
            var projectA = ProjectTestHelpers.GetPackageSpec("a", pathContext.SolutionRoot, framework: "net5.0");
            projectA.Version = new NuGetVersion("2.0.0");

            project2 = project2.WithTestProjectReference(projectA);
            project1 = project1.WithTestProjectReference(project2);
            project1 = project1.WithTestProjectReference(project3);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1, project2, project3, projectA);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[0].Type.Should().Be("project");
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project3");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project1 -> Project2 -> X 1.0.0
        //          -> Project3 -> (PrivateAssets) X 2.0.0 (project)
        // Package is chosen, since project is suppressed
        [Fact]
        public async Task RestoreCommand_WithSameTransitiveProjectPackageId_SuppressedProject_ChoosesPackage_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("a", "1.0.0"));

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpec("Project1", pathContext.SolutionRoot, framework: "net5.0");
            var project2 = ProjectTestHelpers.GetPackageSpec("Project2", pathContext.SolutionRoot, framework: "net5.0");
            var project3 = ProjectTestHelpers.GetPackageSpec("Project3", pathContext.SolutionRoot, framework: "net5.0", dependencyName: "a");

            var projectA = ProjectTestHelpers.GetPackageSpec("a", pathContext.SolutionRoot, framework: "net5.0");
            projectA.Version = new NuGetVersion("2.0.0");

            project2 = project2.WithTestProjectReference(projectA, LibraryIncludeFlags.All);
            project1 = project1.WithTestProjectReference(project2);
            project1 = project1.WithTestProjectReference(project3);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1, project2, project3, projectA);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            //result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0")); // TODO NK - Unclear to me why suppressed project is getting selected.
            //result.LockFile.Targets[0].Libraries[0].Type.Should().Be("package");
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project3");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project1 -> Project2 -> (PrivateAssets) Project3 -> X 1.0.0
        // Project1 expects Project2
        // Project2 expects Project3 and X
        // Project3 expects X
        [Fact]
        public async Task RestoreCommand_WithSuppressedProjectReferences_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("a", "1.0.0");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpec("Project1", pathContext.SolutionRoot);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpec("Project2", pathContext.SolutionRoot);
            var projectSpec3 = ProjectTestHelpers.GetPackageSpec("Project3", pathContext.SolutionRoot, "net5.0", dependencyName: "a");
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);
            projectSpec2 = projectSpec2.WithTestProjectReference(projectSpec3, LibraryIncludeFlags.All); // With PrivateAssetsAll

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2, projectSpec3);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            (var result2, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec2, projectSpec3);
            result2.LockFile.Targets.Should().HaveCount(1);
            result2.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result2.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result2.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result2.LockFile.Targets[0].Libraries[1].Name.Should().Be("Project3");
            result2.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            (var result3, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec3);
            result3.LockFile.Targets.Should().HaveCount(1);
            result3.LockFile.Targets[0].Libraries.Should().HaveCount(1);
            result3.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result3.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // P1 -> P2 -> A [1.0.0] -> B 1.0.0
        // P1 -> C 1.0.0 -> B 2.0.0
        [Fact]
        public async Task RestoreCommand_WithTransitiveExactVersionCausingVersionConflict_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var versionRange = VersionRange.Parse("2.0.0");

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("a", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "[1.0.0]"),
                    ],
                },
                new SimpleTestPackageContext("c", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "2.0.0"),
                    ],
                }
                );

            // Setup project
            var spec1 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""c"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            },
                        }
                    }
                  }
                }";

            var spec2 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""a"": {
                                ""version"": """ + versionRange.ToString() + @""",
                                ""target"": ""Package"",
                            },
                        }
                    }
                  }
                }";

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, spec1);
            var project2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, spec2);
            project1 = project1.WithTestProjectReference(project2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1, project2);
            result.Success.Should().BeFalse();
        }

        // P1 -> P2 -> A [1.0.0] -> B 1.0.0
        // P1 -> C 1.0.0 -> B 2.0.0
        // P1 -> B 2.0.0
        [Fact]
        public async Task RestoreCommand_WithTransitiveExactVersionConflict_OverridenByDirectDependency_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var versionRange = VersionRange.Parse("2.0.0");

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("a", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "[1.0.0]"),
                    ],
                },
                new SimpleTestPackageContext("c", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "2.0.0"),
                    ],
                });

            // Setup project
            var spec1 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""c"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            },
                            ""c"": {
                                ""version"": ""[2.0.0,)"",
                                ""target"": ""Package"",
                            }
                        }
                    }
                  }
                }";

            var spec2 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""a"": {
                                ""version"": """ + versionRange.ToString() + @""",
                                ""target"": ""Package"",
                            },
                        }
                    }
                  }
                }";

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, spec1);
            var project2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, spec2);
            project1 = project1.WithTestProjectReference(project2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1, project2);
            result.Success.Should().BeFalse();
        }

        // Project1 -> A 1.0.*  -> C 1.0.0
        //             B 1.0.*  -> C 1.0.0
        //             Project2 -> C 1.0.*
        [Theory]
        [InlineData("*")]
        [InlineData("1.*")]
        [InlineData("1.0.*")]
        public async Task RestoreCommand_WithTransitiveFloatingVersion_ResolvesCorrectly(string floatingVersion)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var packageA = new SimpleTestPackageContext("A", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("C", "1.0.0")
                ]
            };

            var packageB = new SimpleTestPackageContext("B", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("C", "1.0.0")
                ]
            };

            var packageC101 = new SimpleTestPackageContext("C", "1.0.1");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageB,
                packageC101);

            var project2Spec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
                "Project2",
                pathContext.SolutionRoot,
                @"
                {
                  ""frameworks"": {
                    ""net472"": {
                      ""dependencies"": {
                        ""C"": """ + floatingVersion + @""",
                      }
                    }
                  }
                }");

            var project1Spec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(
                "Project1",
                pathContext.SolutionRoot,
                @"
                {
                  ""frameworks"": {
                    ""net472"": {
                      ""dependencies"": {
                        ""A"": ""1.0.*"",
                        ""B"": ""1.0.*""
                      }
                    }
                  }
                }")
                .WithTestProjectReference(project2Spec);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1Spec, project2Spec);

            // Additional assert
            result.Success.Should().BeTrue();
            result.LogMessages.Should().BeEmpty();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("A");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("B");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));

            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("C");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.1"));

            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // P1 -> P2 -> B *
        // P1 -> A -> B 1.0.0
        [Theory]
        [InlineData("*")]
        [InlineData("*-*")]
        [InlineData("*-rc.*")]
        [InlineData("2.*")]
        [InlineData("2.*-*")]
        [InlineData("2.0.*")]
        [InlineData("2.0.*-*")]
        public async Task RestoreCommand_WithTransitiveFloatingVersion_VerifiesEquivalency(string floatingVersion)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var versionRange = VersionRange.Parse(floatingVersion);

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("a", "1.0.0")
                {
                    Dependencies =
                    [
                        new SimpleTestPackageContext("b", "1.0.0"),
                    ],
                },
                new SimpleTestPackageContext("b", "2.0.0"));

            // Setup project
            var spec1 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""a"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            },
                        }
                    }
                  }
                }";

            var spec2 = @"
                {
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""b"": {
                                ""version"": """ + versionRange.ToString() + @""",
                                ""target"": ""Package"",
                            },
                        }
                    }
                  }
                }";

            // Setup project
            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, spec1);
            var project2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, spec2);
            project1 = project1.WithTestProjectReference(project2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, project1, project2);
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Project1 -> Project3 -> (project) ProjectAndPackage 1.0.0 -> Project4 -> PackageB 2.0.0
        //          -> packageA 1.0.0 -> PackageB 1.0.0
        //                            -> ProjectAndPackage 2.0.0
        [Theory]
        [InlineData("3.0.0")]
        [InlineData("0.9.0")]
        public async Task RestoreCommand_WithTransitiveSameIdProjectAndPackage_PrefersProject_AndVerifiesEquivalency(string projectAndPackageVersion)
        {
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("packageA", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("packageB", "1.0.0"),
                    new SimpleTestPackageContext("ProjectAndPackage", projectAndPackageVersion),
                ]
            };
            var packageB = new SimpleTestPackageContext("packageB", "2.0.0");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageB);

            var rootProject = @"
        {
          ""frameworks"": {
            ""net472"": {
                ""dependencies"": {
                        ""packageA"": {
                            ""version"": ""[1.0.0,)"",
                            ""target"": ""Package"",
                        },
                }
            }
          }
        }";

            var leafProject = @"
        {
          ""version"": ""4.0.0"",
          ""frameworks"": {
            ""net472"": {
                ""dependencies"": {
                        ""packageB"": {
                            ""version"": ""[2.0.0,)"",
                            ""target"": ""Package"",
                        },
                }
            }
          }
        }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, rootProject);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpec("Project3", framework: "net472");
            var projectSpec3 = ProjectTestHelpers.GetPackageSpec("ProjectAndPackage", framework: "net472");
            var projectSpec4 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project4", pathContext.SolutionRoot, leafProject);
            projectSpec3 = projectSpec3.WithTestProjectReference(projectSpec4);
            projectSpec2 = projectSpec2.WithTestProjectReference(projectSpec3);
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2, projectSpec3, projectSpec4);
            result.LockFile.PackageSpec.RestoreMetadata.CentralPackageTransitivePinningEnabled.Should().BeFalse();
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(5);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("packageA");
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("packageB");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project3");
            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("Project4");
            result.LockFile.Targets[0].Libraries[4].Name.Should().Be("ProjectAndPackage");
            result.LockFile.Targets[0].Libraries[4].Type.Should().Be("project");
        }

        // P1 -> P2 -> A (VersionOverride) -> B
        // P1 -> P2 -> B (VersionOverride)
        [Fact]
        public async Task RestoreCommand_WithVersionOverrideAndTransitivePinning_VerifiesEquivalency()
        {
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                ]
            };

            var packageA200 = new SimpleTestPackageContext("a", "2.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "2.0.0"),
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageA200);

            var project1 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[2.0.0,)"",
                            ""b"": ""[2.0.0,)""
                        }
                    }
                  }
                }";

            var project2 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionOverride"": ""[1.0.0, )"",
                                },
                                ""b"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionOverride"": ""[1.0.0, )"",
                                }
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[2.0.0,)"",
                            ""b"": ""[2.0.0,)""
                        }
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, project2);
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            (var result2, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec2);
            result2.LockFile.Targets.Should().HaveCount(1);
            result2.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result2.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result2.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result2.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result2.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // P1 -> P2 -> A (VersionOverride) 1.0.0
        // P1 -> P2 -> B (VersionOverride) 1.0.0
        // P1 and P2 centrally manages A to 2.0.0-preview.1, but not B. No pinning means, only the version override versions are used.
        [Fact]
        public async Task RestoreCommand_WithVersionOverrideOfNotCentrallyManagedPackage_VerifiesEquivalency()
        {
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                ]
            };
            var packageA200 = new SimpleTestPackageContext("a", "2.0.0-preview.1")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "2.0.0-preview.1"),
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageA200);

            var project1 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""2.0.0-preview.1"",
                        }
                    }
                  }
                }";

            var project2 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionOverride"": ""[1.0.0, )"",
                                },
                                ""b"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionOverride"": ""[1.0.0, )"",
                                }
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""2.0.0-preview.1"",
                        }
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, project2);
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            (var result2, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec2);
            result2.LockFile.Targets.Should().HaveCount(1);
            result2.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result2.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result2.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result2.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result2.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // P1 -> P2 -> A (VersionOverride) 1.0.0
        // P1 -> P2 -> B (VersionOverride) 1.0.0
        // P1 and P2 centrally manages A to 2.0.0-preview.1, but not B.
        [Fact]
        public async Task RestoreCommand_WithVersionOverrideOfNotCentrallyManagedPackageAndTransitivePinning_VerifiesEquivalency()
        {
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("c", "1.0.0")
                        ]
                    },
                ]
            };
            var packageA200 = new SimpleTestPackageContext("a", "2.0.0-preview.1")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "2.0.0-preview.1")
                    {
                        Dependencies =
                        [
                            new SimpleTestPackageContext("c", "2.0.0-preview.1")
                        ]
                    },
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageA200);

            var project1 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""2.0.0-preview.1"",
                        }
                    }
                  }
                }";

            var project2 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionOverride"": ""[1.0.0, )"",
                                },
                                ""b"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionOverride"": ""[1.0.0, )"",
                                }
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""2.0.0-preview.1"",
                        }
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, project2);
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(4);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0-preview.1"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0-preview.1"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("2.0.0-preview.1"));
            result.LockFile.Targets[0].Libraries[3].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[3].Version.Should().Be(new NuGetVersion("1.0.0"));

            (var result2, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec2);
            result2.LockFile.Targets.Should().HaveCount(1);
            result2.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result2.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result2.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result2.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result2.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
            result2.LockFile.Targets[0].Libraries[2].Name.Should().Be("c");
            result2.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // P1 -> P2 -> A (VersionOverride) -> B
        // P1 -> P2 -> B (PrivateAssets) VersionOverride
        // Pinning is enabled for both P1 and P2, and P1s versions are higher than those of P2 (otherwise it'd be a downgrade error).
        // P1 gets P2, A & B in the old algorithm, but per RestoreCommand_WithPrivateAssetsOfTransitivePackage_VerifiesEquivalency, it likely shouldn't, since if you disable pinning, B is suppressed.
        // It's very likely that the old algorithm has a bug, likely a consequence of how the behavior was implemented to begin with.
        // Similar as RestoreCommand_WithPrivateAssetsAndTransitivePinningEnabled_VerifiesEquivalency, because of RestoreCommand_WithPrivateAssetsOfTransitivePackage_VerifiesEquivalency, the new algorithm is arguably more correct.
        [Fact]
        public async Task RestoreCommand_WithVersionOverridePrivateAssetsAndTransitivePinningEnabled_VerifiesEquivalency()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Setup packages
            var packageA = new SimpleTestPackageContext("a", "1.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "1.0.0"),
                ]
            };
            var packageA200 = new SimpleTestPackageContext("a", "2.0.0")
            {
                Dependencies =
                [
                    new SimpleTestPackageContext("b", "2.0.0"),
                ]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageA200);

            var project1 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[2.0.0,)"",
                            ""b"": ""[2.0.0,)""
                        }
                    }
                  }
                }";

            var project2 = @"
                {
                    ""restore"": {
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true,
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""a"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionOverride"": ""[1.0.0, )"",
                                },
                                ""b"": {
                                    ""version"": ""[1.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionOverride"": ""[1.0.0, )"",
                                    ""suppressParent"": ""All""
                                }
                        },
                        ""centralPackageVersions"": {
                            ""a"": ""[2.0.0,)"",
                            ""b"": ""[2.0.0,)""
                        }
                    }
                  }
                }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1);
            var projectSpec2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, project2);
            projectSpec = projectSpec.WithTestProjectReference(projectSpec2);

            // Act & Assert
            (var result, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec, projectSpec2);
            result.LockFile.Targets.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries.Should().HaveCount(3);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("2.0.0"));
            result.LockFile.Targets[0].Libraries[2].Name.Should().Be("Project2");
            result.LockFile.Targets[0].Libraries[2].Version.Should().Be(new NuGetVersion("1.0.0"));

            (var result2, _) = await ValidateRestoreAlgorithmEquivalency(pathContext, projectSpec2);
            result2.LockFile.Targets.Should().HaveCount(1);
            result2.LockFile.Targets[0].Libraries.Should().HaveCount(2);
            result2.LockFile.Targets[0].Libraries[0].Name.Should().Be("a");
            result2.LockFile.Targets[0].Libraries[0].Version.Should().Be(new NuGetVersion("1.0.0"));
            result2.LockFile.Targets[0].Libraries[1].Name.Should().Be("b");
            result2.LockFile.Targets[0].Libraries[1].Version.Should().Be(new NuGetVersion("1.0.0"));
        }

        // Here's why package driven dependencies should flow.
        // Say we have P1 -> P2 -> P3 -> A 1.0.0 -> B 2.0.0
        //                            -> B 1.5.0
        // If downgrades didn't flow, the downgrade from project P3 would need to applied in P2 and P1. We should assume P3 did what they did intentionally.
        // Idea: Say downgrades flowed for projects, but not for packages.
        // Then, say you have a mono repo with the above structure. Say you split P1 in one repo and P2 and P3 in another repo.
        // The restore behavior for P2 and P3 for package B would be different than the one for Package A.
        // To promote package/project duality, downgrades must be equivalent for both.
        // Note that all of this downgrade logic should apply CPM or no CPM, VersionOverride or no VersionOverride, the rules are independent of how the version was specified.
        // Some of these behaviors are captured within tests, like package driven downgrade, we need to tests for the project one and the mixed one, the 2 repo case.

        // That leads to VersionOverride - VersionOverride as a concept is an input thing, not a resolver thing. When VersionOverride has a value, it's always translated to the Version property of the library dependency.
        // I think that there should be no VersionOverride checks in the DependencyGraphResolver, since the only reason is exists is
        // to raise a warning when you have VersionOverride but you have not enabled it, otherwise it would've never gone into the model.

        internal static PackageSpec[] DuplicateAndEnableLegacyAlgorithm(PackageSpec[] projects)
        {
            var result = new PackageSpec[projects.Length];
            for (int i = 0; i < projects.Length; i++)
            {
                var legacyResolverProject = projects[i].Clone();
                legacyResolverProject.RestoreMetadata.UseLegacyDependencyResolver = true;
                result[i] = legacyResolverProject;
            }

            return result;
        }

        internal static Task<RestoreResult> RunRestoreAsync(SimpleTestPathContext pathContext, params PackageSpec[] projects)
        {
            return new RestoreCommand(ProjectTestHelpers.CreateRestoreRequest(pathContext, new TestLogger(), projects)).ExecuteAsync();
        }

        internal static async Task<(RestoreResult, RestoreResult)> ValidateRestoreAlgorithmEquivalency(SimpleTestPathContext pathContext, params PackageSpec[] projects)
        {
            var legacyResolverProjects = DuplicateAndEnableLegacyAlgorithm(projects);

            RestoreResult result = await RunRestoreAsync(pathContext, projects);
            RestoreResult legacyResult = await RunRestoreAsync(pathContext, legacyResolverProjects);

            // Assert
            ValidateRestoreResults(result, legacyResult);
            return (result, legacyResult);
        }

        internal static void ValidateRestoreResults(RestoreResult newAlgorithmResult, RestoreResult legacyResult)
        {
            var leftPackageSpec = newAlgorithmResult.LockFile.PackageSpec;
            var rightPackageSpec = legacyResult.LockFile.PackageSpec;

            newAlgorithmResult.Success.Should().Be(legacyResult.Success);
            newAlgorithmResult.LockFile.PackageSpec = null;
            legacyResult.LockFile.PackageSpec = null;
            newAlgorithmResult.LockFile.Should().Be(legacyResult.LockFile);

            //Reset package specs
            newAlgorithmResult.LockFile.PackageSpec = leftPackageSpec;
            legacyResult.LockFile.PackageSpec = rightPackageSpec;
        }
    }
}
