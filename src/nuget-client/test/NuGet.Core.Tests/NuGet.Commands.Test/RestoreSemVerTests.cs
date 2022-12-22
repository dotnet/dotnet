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
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RestoreSemVerTests
    {
        // Float on release label
        [Fact]
        public async Task RestoreSemVer_RestorePackageFloatOnReleaseLabelAsync()
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
                        ""packageA"": ""1.0.0-alpha.1.*""
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

                var packages = new List<SimpleTestPackageContext>();

                var package1 = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0-alpha.1.10.1"
                };
                package1.AddFile("lib/netstandard1.5/a.dll");
                packages.Add(package1);

                var package2 = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0-alpha.1.11.1"
                };
                package2.AddFile("lib/netstandard1.5/a.dll");
                packages.Add(package2);

                var package3 = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0-alpha.1.9.2"
                };
                package3.AddFile("lib/netstandard1.5/a.dll");
                packages.Add(package3);

                var package4 = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0-alpha.1"
                };
                package4.AddFile("lib/netstandard1.5/a.dll");
                packages.Add(package4);

                var package5 = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0-beta.1.99"
                };
                package5.AddFile("lib/netstandard1.5/a.dll");
                packages.Add(package5);

                var package6 = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0-alpha.2.99"
                };
                package6.AddFile("lib/netstandard1.5/a.dll");
                packages.Add(package6);

                var package7 = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0-alpha.1.2+skip"
                };
                package7.AddFile("lib/netstandard1.5/a.dll");
                packages.Add(package7);

                await SimpleTestPackageUtility.CreatePackagesAsync(packages, packageSource.FullName);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.5/a.dll", compile.Path);
                Assert.Equal("1.0.0-alpha.1.11.1", targetLib.Version.ToNormalizedString());
            }
        }

        // Restore 1.0.0-alpha.1.2.3
        [Fact]
        public async Task RestoreSemVer_RestorePackageWithMultipleReleaseLabelsAsync()
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
                        ""packageA"": ""1.0.0-alpha.1.2.3""
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

                var packageAContext = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0-alpha.1.2.3"
                };

                packageAContext.AddFile("lib/netstandard1.5/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.5/a.dll", compile.Path);
                Assert.Equal("1.0.0-alpha.1.2.3", targetLib.Version.ToNormalizedString());
            }
        }

        // Restore 1.0.0+security.fix
        [Fact]
        public async Task RestoreSemVer_RestorePackageWithMetadata_IdenticalAsync()
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
                        ""packageA"": ""1.0.0+security.fix""
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

                var packageAContext = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0+security.fix"
                };

                packageAContext.AddFile("lib/netstandard1.5/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.5/a.dll", compile.Path);
                Assert.Equal("1.0.0", targetLib.Version.ToNormalizedString());
            }
        }

        // Restore 1.0.0+security.fix where the package does not contain +security.fix
        [Fact]
        public async Task RestoreSemVer_RestorePackageWithMetadata_MetadataOnlyInProjectJsonAsync()
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
                        ""packageA"": ""1.0.0+security.fix""
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

                var packageAContext = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0"
                };

                packageAContext.AddFile("lib/netstandard1.5/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.5/a.dll", compile.Path);
                Assert.Equal("1.0.0", targetLib.Version.ToNormalizedString());
            }
        }

        // Restore 1.0.0+security.fix where the project does not contain +security.fix
        [Fact]
        public async Task RestoreSemVer_RestorePackageWithMetadata_MetadataOnlyInPackageAsync()
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

                var packageAContext = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0+security.fix"
                };

                packageAContext.AddFile("lib/netstandard1.5/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.5/a.dll", compile.Path);
                Assert.Equal("1.0.0", targetLib.Version.ToNormalizedString());
            }
        }

        [Fact]
        public async Task RestoreSemVer_RestorePackageWithMetadata_DifferentMetadataDoesNotMatterAsync()
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
                        ""packageA"": ""1.0.0+1""
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

                var packageAContext = new SimpleTestPackageContext("packageA")
                {
                    Version = "1.0.0+B"
                };

                packageAContext.AddFile("lib/netstandard1.5/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.5/a.dll", compile.Path);
                Assert.Equal("1.0.0", targetLib.Version.ToNormalizedString());
            }
        }
    }
}
