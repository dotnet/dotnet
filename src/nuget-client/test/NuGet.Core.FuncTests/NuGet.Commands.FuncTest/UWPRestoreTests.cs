// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NuGet.Commands.Test;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.FuncTest
{
    [Collection(TestCollection.Name)]
    public class UWPRestoreTests
    {
        // Verify that a v1 lock file can be parsed without crashing.
        [Fact]
        public void UWPRestore_ReadV1LockFile()
        {
            // Arrange
            var expectedStream = GetResource("NuGet.Commands.FuncTest.compiler.resources.uwpBlankAppV1Original.json");

            LockFile lockFile = null;

            using (var reader = new StreamReader(expectedStream))
            {
                var format = new LockFileFormat();

                // Act
                lockFile = format.Parse(reader.ReadToEnd(), "c:\\project.lock.json");
            }

            // Assert
            Assert.NotNull(lockFile);
        }

        [Fact]
        public void UWPRestore_ReadLockFileRoundTrip()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var expectedStream = GetResource("NuGet.Commands.FuncTest.compiler.resources.uwpBlankAppV2.json");

                JObject json = null;
                var format = new LockFileFormat();

                using (var reader = new StreamReader(expectedStream))
                {
                    json = JObject.Parse(reader.ReadToEnd());
                }

                var path = Path.Combine(workingDir, "project.lock.json");

                // Act
                var lockFile = format.Parse(json.ToString(), path);

                format.Write(path, lockFile);
                var jsonOutput = JObject.Parse(File.ReadAllText(path));

                // Assert
                Assert.Equal(json.ToString(), jsonOutput.ToString());
            }
        }

        [Fact]
        public async Task UWPRestore_VerifySatellitePackagesAreCompatibleInPCL()
        {
            // Arrange
            var sources = new List<PackageSource>();
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));

            using (var packagesDir = TestDirectory.Create())
            using (var projectDir = TestDirectory.Create())
            {
                var configJson = JObject.Parse(@"{
                  ""dependencies"": {
                    ""Microsoft.AspNet.Mvc.de"": ""5.2.3""
                  },
                  ""frameworks"": {
                    ""net46"": {
                    }
                  }
                }");

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec, sources, packagesDir, logger);
                request.LockFilePath = Path.Combine(projectDir, "project.lock.json");

                var lockFileFormat = new LockFileFormat();
                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(5, result.GetAllInstalled().Count);
            }
        }

        // Verify that UWP packages are still compatible after excluding their contents.
        [Fact]
        public async Task UWPRestore_BlankUWPAppWithExcludes()
        {
            // Arrange
            List<PackageSource> sources = [new PackageSource("https://api.nuget.org/v3/index.json")];

            using var pathContext = new SimpleTestPathContext();
            var configJson = JObject.Parse(@"{
                  ""dependencies"": {
                    ""Microsoft.NETCore.UniversalWindowsPlatform"": {
                        ""version"": ""5.0.0"",
                        ""exclude"": ""build,runtime,compile,native""
                     }
                  },
                  ""frameworks"": {
                    ""uap10.0"": {}
                  },
                  ""runtimes"": {
                    ""win10-arm"": {},
                    ""win10-arm-aot"": {},
                    ""win10-x86"": {},
                    ""win10-x86-aot"": {},
                    ""win10-x64"": {},
                    ""win10-x64-aot"": {}
                  }
                }");


            var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", Path.Combine(pathContext.SolutionRoot, "TestProject", "project.json")).EnsureProjectJsonRestoreMetadata();
            spec.RestoreMetadata.Sources = sources;

            (var mainResult, var legacyResult) = await RestoreCommandTests.ValidateRestoreAlgorithmEquivalency(pathContext, spec);

            // Assert
            Assert.Equal(0, mainResult.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
            Assert.Equal(0, legacyResult.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
            Assert.Equal(0, mainResult.LockFile.LogMessages.Count);
            Assert.Equal(94, mainResult.LockFile.Targets[0].Libraries.Count);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UWPRestore_VerifySameResultWhenRestoringWithLocalPackages(bool useLegacyAlgorithm)
        {
            // Arrange
            List<PackageSource> sources = [new PackageSource("https://api.nuget.org/v3/index.json")];

            using var pathContext = new SimpleTestPathContext();

            var configJson = JObject.Parse(@"{
                ""runtimes"": {
                    ""win7-x86"": { }
                    },
                ""frameworks"": {
                ""dnxcore50"": {
                    ""dependencies"": {
                    ""Microsoft.NETCore"": ""5.0.1-beta-23225""
                    },
                    ""imports"": ""portable-net451+win81""
                }
                }
            }");

            var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", Path.Combine(pathContext.SolutionRoot, "TestProject", "project.json")).EnsureProjectJsonRestoreMetadata();
            spec.RestoreMetadata.Sources = sources;
            spec.RestoreMetadata.UseLegacyDependencyResolver = useLegacyAlgorithm;

            var logger = new TestLogger();
            var request = ProjectTestHelpers.CreateRestoreRequest(pathContext, logger, spec);

            var command = new RestoreCommand(request);

            // Act
            var result = await command.ExecuteAsync();
            var result2 = await command.ExecuteAsync();

            // Assert
            logger.ErrorMessages.Should().HaveCount(0); // TODO NK: NU1203 - potentially a consequence of direct dependency wins chnages. 73 vs 78 files - Actual discrepancy
            logger.WarningMessages.Should().HaveCount(0);
            Assert.Equal(result.LockFile, result2.LockFile);
        }

        [Fact]
        public async Task UWPRestore_SystemDependencyVersionConflict()
        {
            // Arrange
            var sources = new List<PackageSource>();
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));

            using (var packagesDir = TestDirectory.Create())
            using (var projectDir = TestDirectory.Create())
            {
                var configJson = JObject.Parse(@"{
                  ""dependencies"": {
                    ""System.Text.Encoding"": ""4.0.10"",
                    ""System.Collections"": ""4.0.11-beta-23225""
                  },
                  ""frameworks"": {
                    ""uap10.0"": {}
                  }
                }");

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec, sources, packagesDir, logger);
                request.LockFilePath = Path.Combine(projectDir, "project.lock.json");

                var lockFileFormat = new LockFileFormat();
                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var lockFileJson = JObject.Parse(File.ReadAllText(request.LockFilePath));

                // Assert
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
            }
        }

        // Verify that File > New Project > Blank UWP App can restore without errors or warnings.
        [Fact]
        public async Task UWPRestore_BlankUWPApp()
        {
            // Arrange
            List<PackageSource> sources = [new PackageSource("https://api.nuget.org/v3/index.json")];

            using var pathContext = new SimpleTestPathContext();

            var configJson = JObject.Parse(@"{
                  ""dependencies"": {
                    ""Microsoft.NETCore.UniversalWindowsPlatform"": ""5.0.0""
                  },
                  ""frameworks"": {
                    ""uap10.0"": {}
                  },
                  ""runtimes"": {
                    ""win10-arm"": {},
                    ""win10-arm-aot"": {},
                    ""win10-x86"": {},
                    ""win10-x86-aot"": {},
                    ""win10-x64"": {},
                    ""win10-x64-aot"": {}
                  }
                }");

            var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", Path.Combine(pathContext.SolutionRoot, "TestProject", "project.json")).EnsureProjectJsonRestoreMetadata();
            spec.RestoreMetadata.Sources = sources;

            var lockFileFormat = new LockFileFormat();
            var expectedStream = GetResource("NuGet.Commands.FuncTest.compiler.resources.uwpBlankAppV2.json");

            JObject expectedJson = null;

            using (var reader = new StreamReader(expectedStream))
            {
                expectedJson = JObject.Parse(reader.ReadToEnd());
            }

            // Act
            (var mainResult, var legacyResult) = await RestoreCommandTests.ValidateRestoreAlgorithmEquivalency(pathContext, spec);
            await mainResult.CommitAsync(new TestLogger(), CancellationToken.None);

            var lockFileJson = JObject.Parse(File.ReadAllText(mainResult.LockFilePath));
            RemovePackageFolders(lockFileJson);
            RemoveRestoreSection(lockFileJson);

            // Assert
            Assert.True(mainResult.Success);
            Assert.Equal(0, mainResult.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
            Assert.Equal(0, mainResult.LockFile.LogMessages.Count);
            Assert.Equal(expectedJson.ToString(), lockFileJson.ToString());
            Assert.Equal(94, mainResult.LockFile.Targets[0].Libraries.Count);
        }

        // Verify that File > New Project > Blank UWP App can restore without errors or warnings.
        [Fact]
        public async Task UWPRestore_BlankUWPAppV1()
        {
            // Arrange
            List<PackageSource> sources = [new PackageSource("https://api.nuget.org/v3/index.json")];

            using var pathContext = new SimpleTestPathContext();

            var configJson = JObject.Parse(@"{
                  ""dependencies"": {
                    ""Microsoft.NETCore.UniversalWindowsPlatform"": ""5.0.0""
                  },
                  ""frameworks"": {
                    ""uap10.0"": {}
                  },
                  ""runtimes"": {
                    ""win10-arm"": {},
                    ""win10-arm-aot"": {},
                    ""win10-x86"": {},
                    ""win10-x86-aot"": {},
                    ""win10-x64"": {},
                    ""win10-x64-aot"": {}
                  }
                }");

            var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", Path.Combine(pathContext.SolutionRoot, "TestProject", "project.json")).EnsureProjectJsonRestoreMetadata();
            spec.RestoreMetadata.Sources = sources;

            var lockFileFormat = new LockFileFormat();
            var expectedStream = GetResource("NuGet.Commands.FuncTest.compiler.resources.uwpBlankAppV1.json");

            JObject expectedJson = null;

            using (var reader = new StreamReader(expectedStream))
            {
                expectedJson = JObject.Parse(reader.ReadToEnd());
            }

            // Act
            (var mainResult, var legacyResult) = await ValidateRestoreAlgorithmEquivalency(pathContext, spec);
            await mainResult.CommitAsync(new TestLogger(), CancellationToken.None);

            var lockFileJson = JObject.Parse(File.ReadAllText(mainResult.LockFilePath));
            RemovePackageFolders(lockFileJson);

            // Assert
            Assert.True(mainResult.Success);
            Assert.Equal(0, mainResult.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
            Assert.Equal(0, mainResult.LockFile.LogMessages.Count);
            Assert.Equal(expectedJson.ToString(), lockFileJson.ToString());
            Assert.Equal(94, mainResult.LockFile.Targets[0].Libraries.Count);

            static async Task<(RestoreResult, RestoreResult)> ValidateRestoreAlgorithmEquivalency(SimpleTestPathContext pathContext, params PackageSpec[] projects)
            {
                var legacyResolverProjects = RestoreCommandTests.DuplicateAndEnableLegacyAlgorithm(projects);

                RestoreResult result = await RunRestoreAsync(pathContext, projects);
                RestoreResult legacyResult = await RunRestoreAsync(pathContext, legacyResolverProjects);

                // Assert
                RestoreCommandTests.ValidateRestoreResults(result, legacyResult);
                return (result, legacyResult);

                static Task<RestoreResult> RunRestoreAsync(SimpleTestPathContext pathContext, params PackageSpec[] projects)
                {
                    var request = ProjectTestHelpers.CreateRestoreRequest(pathContext, new TestLogger(), projects);
                    request.LockFileVersion = 1;
                    return new RestoreCommand(request).ExecuteAsync();
                }
            }
        }

        // Verify that File > New Project > Class Library (Portable) can restore without errors or warnings.
        [Fact]
        public async Task UWPRestore_ModernPCL()
        {
            // Arrange
            List<PackageSource> sources = [new PackageSource("https://api.nuget.org/v3/index.json")];

            using var pathContext = new SimpleTestPathContext();

            var configJson = JObject.Parse(@"{
                  ""supports"": {
                    ""net46.app"": { },
                    ""uwp.10.0.app"": { },
                    ""dnxcore50.app"": { }
                        },
                  ""dependencies"": {
                    ""Microsoft.NETCore"": ""5.0.0"",
                    ""Microsoft.NETCore.Portable.Compatibility"": ""1.0.0""
                  },
                  ""frameworks"": {
                    ""dotnet"": {
                      ""imports"": ""portable-net452+win81""
                    }
                  }
                }");

            var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", Path.Combine(pathContext.SolutionRoot, "TestProject", "project.json")).EnsureProjectJsonRestoreMetadata();
            spec.RestoreMetadata.Sources = sources;

            (var mainResult, var legacyResult) = await RestoreCommandTests.ValidateRestoreAlgorithmEquivalency(pathContext, spec);

            //result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count).Should().Be(0);
            //logger.ErrorMessages.Should().BeEmpty();
            //logger.WarningMessages.Should().BeEmpty(); // TODO NK: Actual issue, something about compatibility profiles.
            //result.GetAllInstalled().Should().HaveCount(86);
        }


        // Verify that installing all Office 365 services into a UWP app restores without errors.
        [Fact]
        public async Task UWPRestore_UWPAppWithOffice365Packages()
        {
            // Arrange
            List<PackageSource> sources = [new PackageSource("https://api.nuget.org/v3/index.json")];

            using var pathContext = new SimpleTestPathContext();
            var configJson = JObject.Parse(@"{
                ""dependencies"": {
                ""Microsoft.ApplicationInsights"": ""1.0.0"",
                ""Microsoft.ApplicationInsights.PersistenceChannel"": ""1.0.0"",
                ""Microsoft.ApplicationInsights.WindowsApps"": ""1.0.0"",
                ""Microsoft.Azure.ActiveDirectory.GraphClient"": ""2.0.6"",
                ""Microsoft.IdentityModel.Clients.ActiveDirectory"": ""2.14.201151115"",
                ""Microsoft.NETCore.UniversalWindowsPlatform"": ""5.0.0"",
                ""Microsoft.Office365.Discovery"": ""1.0.22"",
                ""Microsoft.Office365.OutlookServices"": ""1.0.35"",
                ""Microsoft.Office365.SharePoint"": ""1.0.22""
                },
                ""frameworks"": {
                ""uap10.0"": {}
                },
                ""runtimes"": {
                ""win10-arm"": {},
                ""win10-arm-aot"": {},
                ""win10-x86"": {},
                ""win10-x86-aot"": {},
                ""win10-x64"": {},
                ""win10-x64-aot"": {}
                }
            }");
            var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", Path.Combine(pathContext.SolutionRoot, "TestProject", "project.json")).EnsureProjectJsonRestoreMetadata();
            spec.RestoreMetadata.Sources = sources;

            (var mainResult, var legacyResult) = await RestoreCommandTests.ValidateRestoreAlgorithmEquivalency(pathContext, spec);
        }

        private Stream GetResource(string name)
        {
            return GetType().GetTypeInfo().Assembly.GetManifestResourceStream(name);
        }

        private void RemovePackageFolders(JObject json)
        {
            json.Remove("packageFolders");
        }

        private void RemoveRestoreSection(JObject json)
        {
            json.Remove("restore");
            json.Value<JObject>("project").Remove("restore");
        }
    }
}
