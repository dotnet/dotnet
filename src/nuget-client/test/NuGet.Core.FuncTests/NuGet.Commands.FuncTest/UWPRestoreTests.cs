// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Commands.Test;
using NuGet.Configuration;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
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
            var sources = new List<PackageSource>();
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));

            using (var packagesDir = TestDirectory.Create())
            using (var projectDir = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
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

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath)
                    .EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(NullSettings.Instance, logger);
                var request = new TestRestoreRequest(spec, sources, packagesDir, cacheContext, clientPolicyContext, logger)
                {
                    LockFilePath = Path.Combine(projectDir, "project.lock.json")
                };

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
                Assert.Equal(118, result.GetAllInstalled().Count);
            }
        }

        [Fact]
        public async Task UWPRestore_VerifySameResultWhenRestoringWithLocalPackages()
        {
            // Arrange
            var sources = new List<PackageSource>();
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));

            using (var packagesDir = TestDirectory.Create())
            using (var projectDir = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
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

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec, sources, packagesDir, cacheContext, logger);
                request.LockFilePath = Path.Combine(projectDir, "project.lock.json");

                var lockFileFormat = new LockFileFormat();
                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();
                var result2 = await command.ExecuteAsync();

                // Assert
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(result.LockFile, result2.LockFile);
            }
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
            var sources = new List<PackageSource>();
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));

            using (var packagesDir = TestDirectory.Create())
            using (var projectDir = TestDirectory.Create())
            using (var cacheContext = new SourceCacheContext())
            {
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

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec, sources, packagesDir, cacheContext, logger);
                request.LockFilePath = Path.Combine(projectDir, "project.lock.json");

                var lockFileFormat = new LockFileFormat();
                var command = new RestoreCommand(request);

                var expectedStream = GetResource("NuGet.Commands.FuncTest.compiler.resources.uwpBlankAppV2.json");

                JObject expectedJson = null;

                using (var reader = new StreamReader(expectedStream))
                {
                    expectedJson = JObject.Parse(reader.ReadToEnd());
                }

                // Act
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var lockFileJson = JObject.Parse(File.ReadAllText(request.LockFilePath));
                RemovePackageFolders(lockFileJson);
                RemoveRestoreSection(lockFileJson);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(118, result.GetAllInstalled().Count);
                Assert.Equal(expectedJson.ToString(), lockFileJson.ToString());
            }
        }

        // Verify that File > New Project > Blank UWP App can restore without errors or warnings.
        [Fact]
        public async Task UWPRestore_BlankUWPAppV1()
        {
            // Arrange
            var sources = new List<PackageSource>();
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));

            using (var packagesDir = TestDirectory.Create())
            using (var projectDir = TestDirectory.Create())
            {
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

                var specPath = Path.Combine(projectDir, "TestProject", "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(configJson.ToString(), "TestProject", specPath).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec, sources, packagesDir, logger);
                request.LockFilePath = Path.Combine(projectDir, "project.lock.json");

                // Set the lock file version to v1 to force a downgrade
                request.LockFileVersion = 1;

                var lockFileFormat = new LockFileFormat();
                var command = new RestoreCommand(request);

                var expectedStream = GetResource("NuGet.Commands.FuncTest.compiler.resources.uwpBlankAppV1.json");

                JObject expectedJson = null;

                using (var reader = new StreamReader(expectedStream))
                {
                    expectedJson = JObject.Parse(reader.ReadToEnd());
                }

                // Act
                var result = await command.ExecuteAsync();
                await result.CommitAsync(logger, CancellationToken.None);

                var lockFileJson = JObject.Parse(File.ReadAllText(request.LockFilePath));
                RemovePackageFolders(lockFileJson);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(0, result.CompatibilityCheckResults.Sum(checkResult => checkResult.Issues.Count));
                Assert.Equal(0, logger.Errors);
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(118, result.GetAllInstalled().Count);
                Assert.Equal(expectedJson.ToString(), lockFileJson.ToString());
            }
        }

        // Verify that File > New Project > Class Library (Portable) can restore without errors or warnings.
        [Fact]
        public async Task UWPRestore_ModernPCL()
        {
            // Arrange
            var sources = new List<PackageSource>();
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));

            using (var packagesDir = TestDirectory.Create())
            using (var projectDir = TestDirectory.Create())
            {
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
                Assert.True(0 == logger.Errors, logger.ShowMessages());
                Assert.Equal(0, logger.Warnings);
                Assert.Equal(86, result.GetAllInstalled().Count);
            }
        }

        // Verify that installing all Office 365 services into a UWP app restores without errors.
        [Fact]
        public async Task UWPRestore_UWPAppWithOffice365Packages()
        {
            // Arrange
            var sources = new List<PackageSource>();
            sources.Add(new PackageSource("https://api.nuget.org/v3/index.json"));

            using (var packagesDir = TestDirectory.Create())
            using (var projectDir = TestDirectory.Create())
            {
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
                Assert.Equal(140, result.GetAllInstalled().Count);
            }
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
