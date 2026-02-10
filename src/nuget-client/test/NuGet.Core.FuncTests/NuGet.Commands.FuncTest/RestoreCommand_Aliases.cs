// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;

namespace NuGet.Commands.FuncTest
{
    [Collection(TestCollection.Name)]
    public class RestoreCommand_Aliases
    {
        // P1 (banana) -> X
        // P1 (apple) -> Y
        [Fact]
        public async Task RestoreCommand_WithAliasesOfSameFramework_RestoresCorrectPackages()
        {
            using var pathContext = new SimpleTestPathContext();
            var rootProject = @"
            {
              ""frameworks"": {
                ""apple"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""apple"",
                    ""dependencies"": {
                            ""x"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                },
                ""banana"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""banana"",
                    ""dependencies"": {
                            ""y"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                }
              }
            }";


            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, rootProject);
            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("x", "1.0.0"),
                new SimpleTestPackageContext("y", "1.0.0"));

            // Act & Assert
            var result = await RunRestoreAsync(pathContext, projectSpec);
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(2);
            result.LockFile.Targets[0].TargetAlias.Should().Be("apple");
            result.LockFile.Targets[0].Libraries.Should().HaveCount(1);
            result.LockFile.Targets[0].Libraries[0].Name.Should().Be("x");
            result.LockFile.Targets[1].TargetAlias.Should().Be("banana");
            result.LockFile.Targets[1].Libraries.Should().HaveCount(1);
            result.LockFile.Targets[1].Libraries[0].Name.Should().Be("y");
        }

        // P (apple)  -> Net472 package, with ATF, succeeds
        // P (banana) -> Net472 package, with ATF, fails
        [Fact]
        public async Task RestoreCommand_WithAliasesOfSameFramework_WithAssetTargetFallback_OneSucceedsOneFails()
        {
            using var pathContext = new SimpleTestPathContext();

            // Create a package that only supports net472
            var pkgNet472Only = new SimpleTestPackageContext("Net472Package", "1.0.0");
            pkgNet472Only.AddFile("lib/net472/Net472Package.dll");
            await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, pkgNet472Only);

            string apple = nameof(apple);
            string banana = nameof(banana);

            // Apple alias with AssetTargetFallback (should succeed)
            // Banana alias without AssetTargetFallback (should fail)
            var rootProject = @"
            {
              ""frameworks"": {
                ""apple"": {
                    ""framework"": ""net9.0"",
                    ""targetAlias"": ""apple"",
                    ""assetTargetFallback"": true,
                    ""imports"": [ ""net472"" ],
                    ""warn"": true,
                    ""dependencies"": {
                            ""Net472Package"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                },
                ""banana"": {
                    ""framework"": ""net9.0"",
                    ""targetAlias"": ""banana"",
                    ""dependencies"": {
                            ""Net472Package"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                }
              }
            }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, rootProject);

            // Act
            var result = await RunRestoreAsync(pathContext, projectSpec);

            // Assert - restore should fail overall due to banana
            result.Success.Should().BeFalse();
            result.LockFile.Targets.Should().HaveCount(2);

            // Apple alias should succeed with the package
            var appleTarget = result.LockFile.GetTarget(apple, null);
            appleTarget.Should().NotBeNull();
            appleTarget.Libraries.Should().ContainSingle(e => e.Name!.Equals("Net472Package"));

            // Banana alias should have no compatible assets
            var bananaTarget = result.LockFile.GetTarget(banana, null);
            bananaTarget.Should().NotBeNull();
            var bananaLib = bananaTarget.Libraries.Single(e => e.Name!.Equals("Net472Package"));
            bananaLib.CompileTimeAssemblies.Should().BeEmpty();
            bananaLib.RuntimeAssemblies.Should().BeEmpty();

            // Verify log messages
            result.LockFile.LogMessages.Should().HaveCount(2);
            result.LockFile.LogMessages[0].Level.Should().Be(LogLevel.Warning);
            result.LockFile.LogMessages[0].Code.Should().Be(NuGetLogCode.NU1701);
            result.LockFile.LogMessages[0].TargetGraphs.Should().BeEquivalentTo([apple]);

            result.LockFile.LogMessages[1].Level.Should().Be(LogLevel.Error);
            result.LockFile.LogMessages[1].Code.Should().Be(NuGetLogCode.NU1202);
            result.LockFile.LogMessages[1].TargetGraphs.Should().BeEquivalentTo([banana]);
        }

        // P (apple)  -> Net472 package -> Transitive Net472 package, with ATF, succeeds
        // P (banana) -> Net472 package, with ATF, fails
        [Fact]
        public async Task RestoreCommand_WithAliasesOfSameFramework_WithAssetTargetFallback_TransitiveDependenciesFlowCorrectly()
        {
            using var pathContext = new SimpleTestPathContext();

            // Create a transitive dependency package that only supports net472
            var pkgTransitive = new SimpleTestPackageContext("TransitiveNet472Package", "1.0.0");
            pkgTransitive.AddFile("lib/net472/TransitiveNet472Package.dll");

            // Create a top-level package that only supports net472 and depends on the transitive package
            var pkgNet472WithDependency = new SimpleTestPackageContext("Net472PackageWithDependency", "1.0.0");
            pkgNet472WithDependency.AddFile("lib/net472/Net472PackageWithDependency.dll");
            pkgNet472WithDependency.PerFrameworkDependencies.Add(CommonFrameworks.Net472, [pkgTransitive]);

            await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, pkgNet472WithDependency, pkgTransitive);

            string apple = nameof(apple);
            string banana = nameof(banana);

            // Apple alias with AssetTargetFallback (should succeed)
            // Banana alias without AssetTargetFallback (should fail)
            var rootProject = @"
            {
              ""frameworks"": {
                ""apple"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""apple"",
                    ""assetTargetFallback"": true,
                    ""imports"": [ ""net472"" ],
                    ""warn"": true,
                    ""dependencies"": {
                            ""Net472PackageWithDependency"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                },
                ""banana"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""banana"",
                    ""dependencies"": {
                            ""Net472PackageWithDependency"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                }
              }
            }";

            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, rootProject);

            // Act
            var result = await RunRestoreAsync(pathContext, projectSpec);

            // Assert - restore should fail overall due to banana
            result.Success.Should().BeFalse();
            result.LockFile.Targets.Should().HaveCount(2);

            // Apple alias should succeed with both the package and its transitive dependency
            var appleTarget = result.LockFile.GetTarget(apple, null);
            appleTarget.Should().NotBeNull();
            appleTarget.Libraries.Should().Contain(e => e.Name!.Equals("Net472PackageWithDependency"));
            appleTarget.Libraries.Should().Contain(e => e.Name!.Equals("TransitiveNet472Package"));

            // Verify the dependency relationship
            var appleTopLevelLib = appleTarget.Libraries.First(e => e.Name!.Equals("Net472PackageWithDependency"));
            appleTopLevelLib.Dependencies.Should().Contain(d => d.Id.Equals("TransitiveNet472Package"));

            // Banana alias should have no compatible assets for either package
            var bananaTarget = result.LockFile.GetTarget(banana, null);
            bananaTarget.Should().NotBeNull();
            bananaTarget.Libraries.Should().HaveCount(1);
            bananaTarget.Libraries[0].CompileTimeAssemblies.Should().BeEmpty();
            bananaTarget.Libraries[0].RuntimeAssemblies.Should().BeEmpty();

            // Verify log messages
            result.LockFile.LogMessages.Should().HaveCount(3);
            result.LockFile.LogMessages[0].Level.Should().Be(LogLevel.Warning);
            result.LockFile.LogMessages[0].Code.Should().Be(NuGetLogCode.NU1701);

            result.LockFile.LogMessages[1].Level.Should().Be(LogLevel.Warning);
            result.LockFile.LogMessages[1].Code.Should().Be(NuGetLogCode.NU1701);

            result.LockFile.LogMessages[2].Level.Should().Be(LogLevel.Error);
            result.LockFile.LogMessages[2].Code.Should().Be(NuGetLogCode.NU1202);
        }

        // P (apple) -> Project2 (apple) -> Package A
        // P (banana) -> Project2 (banana) -> Package B
        [Fact]
        public async Task RestoreCommand_WithAliasesOfSameFrameworkAndProjectReferences_TransitivePackageDependenciesFlowCorrectly()
        {
            using var pathContext = new SimpleTestPathContext();

            // Create packages
            var pkgA = new SimpleTestPackageContext("PackageA", "1.0.0");
            var pkgB = new SimpleTestPackageContext("PackageB", "1.0.0");
            await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, pkgA, pkgB);

            string apple = nameof(apple);
            string banana = nameof(banana);

            // Create Project2 spec with different package dependencies per alias
            var project2Spec = @"
            {
              ""frameworks"": {
                ""apple"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""apple"",
                    ""dependencies"": {
                            ""PackageA"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                },
                ""banana"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""banana"",
                    ""dependencies"": {
                            ""PackageB"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                }
              }
            }";

            var project2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, project2Spec);

            // Create Project1 spec that references Project2
            var project1Spec = @"
            {
              ""frameworks"": {
                ""apple"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""apple"",
                    ""dependencies"": {
                    }
                },
                ""banana"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""banana"",
                    ""dependencies"": {
                    }
                }
              }
            }";

            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1Spec);
            project1 = project1.WithTestProjectReference(project2);

            // Act
            var result = await RunRestoreAsync(pathContext, project1, project2);

            // Assert
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(2);

            // Apple alias should have Project2 and PackageA transitively
            var appleTarget = result.LockFile.GetTarget(apple, null);
            appleTarget.Should().NotBeNull();
            appleTarget.Libraries.Should().Contain(e => e.Name!.Equals("Project2"));
            appleTarget.Libraries.Should().Contain(e => e.Name!.Equals("PackageA"));
            appleTarget.Libraries.Should().NotContain(e => e.Name!.Equals("PackageB"));

            // Banana alias should have Project2 and PackageB transitively
            var bananaTarget = result.LockFile.GetTarget(banana, null);
            bananaTarget.Should().NotBeNull();
            bananaTarget.Libraries.Should().Contain(e => e.Name!.Equals("Project2"));
            bananaTarget.Libraries.Should().NotContain(e => e.Name!.Equals("PackageA"));
            bananaTarget.Libraries.Should().Contain(e => e.Name!.Equals("PackageB"));
        }

        // P (apple) -> Project2 
        // P (banana) -> Project3
        [Fact]
        public async Task RestoreCommand_WithAliasesOfSameFramework_MultipleProjectReferencesFlowCorrectly()
        {
            using var pathContext = new SimpleTestPathContext();

            string apple = nameof(apple);
            string banana = nameof(banana);

            // Create Project2 spec - no dependencies
            var project2Spec = @"
            {
              ""frameworks"": {
                ""net10.0"": {
                    ""framework"": ""net10.0""
                }
              }
            }";

            var project2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project2", pathContext.SolutionRoot, project2Spec);

            // Create Project3 spec - no dependencies
            var project3Spec = @"
            {
              ""frameworks"": {
                ""net10.0"": {
                    ""framework"": ""net10.0""
                }
              }
            }";

            var project3 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project3", pathContext.SolutionRoot, project3Spec);

            // Create Project1 spec that references Project2 for apple alias and Project3 for banana alias
            var project1Spec = @"
            {
              ""frameworks"": {
                ""apple"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""apple"",
                    ""dependencies"": {
                    }
                },
                ""banana"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""banana"",
                    ""dependencies"": {
                    }
                }
              }
            }";

            var project1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, project1Spec);

            // Add Project2 reference only for apple alias
            project1 = project1.WithTestProjectReferenceByAlias(project2, apple);

            // Add Project3 reference only for banana alias  
            project1 = project1.WithTestProjectReferenceByAlias(project3, banana);

            // Act
            var result = await RunRestoreAsync(pathContext, project1, project2, project3);

            // Assert
            result.Success.Should().BeTrue();
            result.LockFile.Targets.Should().HaveCount(2);

            // Apple alias should have Project2 but not Project3
            var appleTarget = result.LockFile.GetTarget(apple, null);
            appleTarget.Should().NotBeNull();
            appleTarget.Libraries.Should().Contain(e => e.Name!.Equals("Project2"));
            appleTarget.Libraries.Should().NotContain(e => e.Name!.Equals("Project3"));

            // Banana alias should have Project3 but not Project2
            var bananaTarget = result.LockFile.GetTarget(banana, null);
            bananaTarget.Should().NotBeNull();
            bananaTarget.Libraries.Should().NotContain(e => e.Name!.Equals("Project2"));
            bananaTarget.Libraries.Should().Contain(e => e.Name!.Equals("Project3"));
        }

        [Theory]
        [InlineData("10.0.200", "10.0.300")]
        [InlineData("10.0.300", "10.0.300-preview.0.26109")]
        [InlineData("11.0.100", "11.0.100-preview.2.26103")]
        public async Task RestoreCommand_WithAliasesOfSameFramework_AndIncompatibleSDKAnalysisLevelAndSDKVersionCombinations_FailsWithNU1018(string sdkAnalysisLevel, string sdkVersion)
        {
            using var pathContext = new SimpleTestPathContext();

            RestoreResult result = await RunSimpleAliasedRestoreWithSDKAnalysisLevelAndSDKVersion(pathContext, sdkAnalysisLevel, sdkVersion);

            result.Success.Should().BeFalse();
            result.LockFile.LogMessages.Should().HaveCount(1);
            result.LockFile.LogMessages[0].Code.Should().Be(NuGetLogCode.NU1018);
        }

        [Theory]
        [InlineData("10.0.300", "10.0.300")]
        [InlineData("10.0.300", "10.0.400")]
        [InlineData("10.0.300", null)] // Null value means we let it be.
        [InlineData("10.0.300", "10.0.300-preview.0.26110")] // This is a dummy value that'll need to be updated once we have a real one.
        [InlineData("11.0.100", "11.0.100-preview.2.26104")]
        [InlineData("11.0.100", "11.0.100")]
        [InlineData("11.0.100", "11.0.101")]
        public async Task RestoreCommand_WithAliasesOfSameFramework_AndValidSDKAnalysisLevelAndSDKVersionCombinations_Succeeds(string sdkAnalysisLevel, string sdkVersion)
        {
            using var pathContext = new SimpleTestPathContext();

            RestoreResult result = await RunSimpleAliasedRestoreWithSDKAnalysisLevelAndSDKVersion(pathContext, sdkAnalysisLevel, sdkVersion);

            result.Success.Should().BeTrue();
        }

        private static async Task<RestoreResult> RunSimpleAliasedRestoreWithSDKAnalysisLevelAndSDKVersion(SimpleTestPathContext pathContext, string sdkAnalysisLevel, string sdkVersion)
        {
            // Setup packages
            var packageA = new SimpleTestPackageContext("packageA", "1.0.0")
            {
                Dependencies = [new SimpleTestPackageContext("packageB", "1.0.0")]
            };

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA);

            var rootProject = @"
            {
              ""frameworks"": {
                ""apple"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""apple"",
                    ""dependencies"": {
                            ""x"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                },
                ""banana"": {
                    ""framework"": ""net10.0"",
                    ""targetAlias"": ""banana"",
                    ""dependencies"": {
                            ""y"": {
                                ""version"": ""[1.0.0,)"",
                                ""target"": ""Package"",
                            }
                    }
                }
              }
            }";


            // Setup project
            var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("Project1", pathContext.SolutionRoot, rootProject);
            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("x", "1.0.0"),
                new SimpleTestPackageContext("y", "1.0.0"));

            projectSpec.RestoreMetadata.SdkAnalysisLevel = NuGetVersion.Parse(sdkAnalysisLevel);
            projectSpec.RestoreMetadata.UsingMicrosoftNETSdk = true;
            projectSpec.RestoreSettings.SdkVersion = sdkVersion != null ? NuGetVersion.Parse(sdkVersion) : null;

            // Act & Assert
            return await RunRestoreAsync(pathContext, projectSpec);
        }

        internal static Task<RestoreResult> RunRestoreAsync(SimpleTestPathContext pathContext, params PackageSpec[] projects)
        {
            return RunRestoreAsync(pathContext, forceEvaluate: false, new TestLogger(), projects);
        }

        internal static Task<RestoreResult> RunRestoreAsync(SimpleTestPathContext pathContext, bool forceEvaluate, TestLogger logger, params PackageSpec[] projects)
        {
            var request = ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, projects);
            request.RestoreForceEvaluate = forceEvaluate;
            return new RestoreCommand(request).ExecuteAsync();
        }
    }
}
