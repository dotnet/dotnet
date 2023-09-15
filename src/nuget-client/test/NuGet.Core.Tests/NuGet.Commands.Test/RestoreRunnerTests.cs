// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RestoreRunnerTests
    {
        [Fact]
        public async Task RestoreRunner_BasicRestoreAsync()
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
                ""net45"": {
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var projectName = "project1";
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", projectName));

                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var dgPath = Path.Combine(project1.FullName, "project.dg");

                File.WriteAllText(specPath1, project1Json);

                var spec1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec(projectName, Path.Combine(workingDir, "projects"), project1Json);

                spec1.RestoreMetadata.Sources = new List<PackageSource> { new PackageSource(packageSource.FullName) };
                spec1.RestoreMetadata.PackagesPath = packagesDir.FullName;
                var dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(spec1);
                dgSpec.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                var logger = new TestLogger();
                var lockPath = Path.Combine(spec1.RestoreMetadata.OutputPath, "project.assets.json");

                var sourceRepos = sources.Select(source => Repository.Factory.GetCoreV3(source.Source)).ToList();

                var providerCache = new RestoreCommandProvidersCache();
                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        GlobalPackagesFolder = packagesDir.FullName,
                        Sources = new List<string>() { packageSource.FullName },
                        Log = logger,
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(providerCache, dgSpec)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    Assert.True(summary.Success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                    Assert.Equal(1, summary.FeedsUsed.Count);
                    Assert.True(File.Exists(lockPath), lockPath);
                    Assert.False(File.Exists(Path.Combine(project1.FullName, "project1.nuget.targets")));
                    Assert.False(File.Exists(Path.Combine(project1.FullName, "project1.nuget.props")));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_BasicRestore_VerifyFailureWritesFilesAsync()
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
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
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
                var spec1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), project1Json);
                spec1.RestoreMetadata.Sources = new List<PackageSource> { new PackageSource(packageSource.FullName) };
                spec1.RestoreMetadata.PackagesPath = packagesDir.FullName;
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec1);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                var logger = new TestLogger();
                var lockPath = Path.Combine(spec1.RestoreMetadata.OutputPath, "project.assets.json");

                var sourceRepos = sources.Select(source => Repository.Factory.GetCoreV3(source.Source)).ToList();

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("build/net45/x.targets");

                var packageY = new SimpleTestPackageContext("y");
                packageX.Dependencies.Add(packageY);

                var yPath = await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageY);
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageX);

                // y does not exist
                yPath.Delete();

                var providerCache = new RestoreCommandProvidersCache();

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        GlobalPackagesFolder = packagesDir.FullName,
                        Sources = new List<string>() { packageSource.FullName },
                        Log = logger,
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(providerCache, dgFile)
                        }
                    };

                    var targetsPath = Path.Combine(spec1.RestoreMetadata.OutputPath, "project1.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(spec1.RestoreMetadata.OutputPath, "project1.nuget.props");

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    var targets = TargetsUtility.GetMSBuildPackageImports(targetsPath);

                    // Assert
                    Assert.False(summary.Success);
                    Assert.True(File.Exists(lockPath), lockPath);
                    Assert.True(File.Exists(targetsPath));
                    Assert.False(File.Exists(propsPath));
                    Assert.Equal(1, targets.Count);
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_BasicRestore_VerifyFailureWritesFiles_NETCoreAsync()
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
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
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
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1);
                spec1.TargetFrameworks.Single().TargetAlias = "net45";
                spec1.RestoreMetadata = new ProjectRestoreMetadata
                {
                    OutputPath = Path.Combine(project1.FullName, "obj"),
                    ProjectStyle = ProjectStyle.PackageReference,
                    ProjectName = "project1",
                    ProjectPath = Path.Combine(project1.FullName, "project1.csproj")
                };
                spec1.RestoreMetadata.ProjectUniqueName = spec1.RestoreMetadata.ProjectPath;
                spec1.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45")) { TargetAlias = "net45" });
                spec1.RestoreMetadata.OriginalTargetFrameworks.Add("net45");
                spec1.FilePath = spec1.RestoreMetadata.ProjectPath;

                var dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(spec1);
                dgSpec.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                var logger = new TestLogger();
                var assetsPath = Path.Combine(project1.FullName, "obj", "project.assets.json");

                var sourceRepos = sources.Select(source => Repository.Factory.GetCoreV3(source.Source)).ToList();

                var providerCache = new RestoreCommandProvidersCache();

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        GlobalPackagesFolder = packagesDir.FullName,
                        Sources = new List<string>() { packageSource.FullName },
                        Log = logger,
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(providerCache, dgSpec)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    Assert.False(summary.Success);
                    Assert.True(File.Exists(assetsPath), assetsPath);
                    Assert.True(File.Exists(Path.Combine(project1.FullName, "obj", "project1.csproj.nuget.g.props")));
                    Assert.True(File.Exists(Path.Combine(project1.FullName, "obj", "project1.csproj.nuget.g.targets")));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_BasicRestoreWithConfigFileAsync()
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
                ""net45"": {
                }
              }
            }";

            var configFile = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""nuget.org"" value=""{0}"" />
    </packageSources>
</configuration>
";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);
                File.WriteAllText(Path.Combine(workingDir, "NuGet.Config"), string.Format(configFile, packageSource.FullName));

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), project1Json);
                var configPath = Path.Combine(workingDir, "NuGet.Config");

                var dgFile = new DependencyGraphSpec();
                spec1.RestoreMetadata.ConfigFilePaths = new List<string> { configPath };
                spec1.RestoreMetadata.Sources = new List<PackageSource> { new PackageSource(packageSource.FullName) };
                spec1.RestoreMetadata.PackagesPath = packagesDir.FullName;

                dgFile.AddProject(spec1);
                dgFile.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                var logger = new TestLogger();
                var lockPath = Path.Combine(spec1.RestoreMetadata.OutputPath, "project.assets.json");

                var providerCache = new RestoreCommandProvidersCache();

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        GlobalPackagesFolder = spec1.RestoreMetadata.PackagesPath,
                        ConfigFile = configPath,
                        Log = logger,
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(new List<PackageSource>())),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>
                        {
                            new DependencyGraphSpecRequestProvider(providerCache, dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    Assert.True(summary.Success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                    Assert.Equal(1, summary.FeedsUsed.Count);
                    Assert.True(File.Exists(lockPath), lockPath);
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_RestoreWithExternalFileAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var targetFrameworkInfo1 = new TargetFrameworkInformation
            {
                FrameworkName = NuGetFramework.Parse("net45"),
                TargetAlias = "net45",
            };
            var frameworks1 = new[] { targetFrameworkInfo1 };

            var targetFrameworkInfo2 = new TargetFrameworkInformation
            {
                FrameworkName = NuGetFramework.Parse("net45"),
                TargetAlias = "net45",
            };
            var frameworks2 = new[] { targetFrameworkInfo2 };

            // Create two net45 projects
            var spec1 = new PackageSpec(frameworks1)
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectUniqueName = "project1",
                    ProjectName = "project1",
                    ProjectStyle = ProjectStyle.PackageReference
                }
            };
            spec1.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

            var spec2 = new PackageSpec(frameworks2)
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectUniqueName = "project2",
                    ProjectName = "project2",
                    ProjectStyle = ProjectStyle.PackageReference
                }
            };
            spec2.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

            var specs = new[] { spec1, spec2 };

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var projPath1 = Path.Combine(project1.FullName, "project1.csproj");
                var projPath2 = Path.Combine(project2.FullName, "project2.csproj");
                File.WriteAllText(projPath1, string.Empty);
                File.WriteAllText(projPath2, string.Empty);

                spec1.RestoreMetadata.ProjectPath = projPath1;
                spec1.FilePath = projPath1;
                spec1.Name = "project1";
                spec2.RestoreMetadata.ProjectPath = projPath2;
                spec2.FilePath = projPath1;
                spec2.Name = "project2";

                var logger = new TestLogger();
                var objPath1 = Path.Combine(project1.FullName, "obj");
                var objPath2 = Path.Combine(project2.FullName, "obj");

                spec1.RestoreMetadata.OutputPath = objPath1;
                spec2.RestoreMetadata.OutputPath = objPath2;

                spec1.RestoreMetadata.OriginalTargetFrameworks.Add("net45");
                spec2.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

                var lockPath1 = Path.Combine(objPath1, "project.assets.json");
                var lockPath2 = Path.Combine(objPath2, "project.assets.json");

                // Link projects
                spec1.TargetFrameworks.Single().Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange()
                    {
                        Name = "project2",
                        TypeConstraint = LibraryDependencyTarget.ExternalProject
                    }
                });

                spec1.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45")));
                spec1.RestoreMetadata.TargetFrameworks
                    .Single()
                    .ProjectReferences
                    .Add(new ProjectRestoreReference()
                    {
                        ProjectPath = projPath2,
                        ProjectUniqueName = "project2"
                    });

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                foreach (var spec in specs)
                {
                    dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);
                    dgFile.AddProject(spec);
                }

                var dgPath = Path.Combine(workingDir, "input.dg");
                dgFile.Save(dgPath);

                var sourceRepos = sources.Select(source => Repository.Factory.GetCoreV3(source.Source)).ToList();

                var providerCache = new RestoreCommandProvidersCache();

                var restoreContext = new RestoreArgs()
                {
                    CacheContext = new SourceCacheContext(),
                    DisableParallel = true,
                    GlobalPackagesFolder = packagesDir.FullName,
                    Sources = new List<string>() { packageSource.FullName },
                    Log = logger,
                    CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                    RequestProviders = new List<IRestoreRequestProvider>()
                    {
                        new DependencyGraphFileRequestProvider(providerCache)
                    }
                };

                // add file path as input
                restoreContext.Inputs.Add(dgPath);

                // Act
                var summaries = await RestoreRunner.RunAsync(restoreContext);
                var success = summaries.All(s => s.Success);

                var lockFormat = new LockFileFormat();
                var lockFile1 = lockFormat.Read(lockPath1);
                var project2Lib = lockFile1.Libraries.First();

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.True(File.Exists(lockPath1), lockPath1);
                Assert.True(File.Exists(lockPath2), lockPath2);
                Assert.Equal("project2", project2Lib.Name);
            }
        }

        [Fact]
        public async Task RestoreRunner_RestoreWithExternalFile_NetCoreOutputAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var targetFrameworkInfo1 = new TargetFrameworkInformation
            {
                FrameworkName = NuGetFramework.Parse("net45"),
                TargetAlias = "net45",
            };
            var frameworks1 = new[] { targetFrameworkInfo1 };

            var targetFrameworkInfo2 = new TargetFrameworkInformation
            {
                FrameworkName = NuGetFramework.Parse("net45"),
                TargetAlias = "net45",
            };
            var frameworks2 = new[] { targetFrameworkInfo2 };

            // Create two net45 projects
            var spec1 = new PackageSpec(frameworks1)
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectUniqueName = "project1",
                    ProjectName = "project1",
                    ProjectStyle = ProjectStyle.PackageReference
                }
            };
            spec1.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

            var spec2 = new PackageSpec(frameworks2)
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectUniqueName = "project2",
                    ProjectName = "project2",
                    ProjectStyle = ProjectStyle.PackageReference
                }
            };
            spec2.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

            var specs = new[] { spec1, spec2 };

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project2 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project2"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                project2.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var projPath1 = Path.Combine(project1.FullName, "project1.csproj");
                var projPath2 = Path.Combine(project2.FullName, "project2.csproj");
                File.WriteAllText(projPath1, string.Empty);
                File.WriteAllText(projPath2, string.Empty);

                spec1.RestoreMetadata.ProjectPath = projPath1;
                spec1.FilePath = projPath1;
                spec1.Name = "project1";
                spec2.RestoreMetadata.ProjectPath = projPath2;
                spec2.FilePath = projPath1;
                spec2.Name = "project2";

                var logger = new TestLogger();
                var objPath1 = Path.Combine(project1.FullName, "obj");
                var objPath2 = Path.Combine(project2.FullName, "obj");

                spec1.RestoreMetadata.OutputPath = objPath1;
                spec2.RestoreMetadata.OutputPath = objPath2;

                spec1.RestoreMetadata.OriginalTargetFrameworks.Add("net45");
                spec2.RestoreMetadata.OriginalTargetFrameworks.Add("net45");

                var lockPath1 = Path.Combine(objPath1, "project.assets.json");
                var lockPath2 = Path.Combine(objPath2, "project.assets.json");

                // Link projects
                spec1.TargetFrameworks.Single().Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange()
                    {
                        Name = "project2",
                        TypeConstraint = LibraryDependencyTarget.ExternalProject
                    }
                });

                spec1.RestoreMetadata.TargetFrameworks.Add(new ProjectRestoreMetadataFrameworkInfo(NuGetFramework.Parse("net45")) { TargetAlias = "net45" });
                spec1.RestoreMetadata.TargetFrameworks
                    .Single()
                    .ProjectReferences
                    .Add(new ProjectRestoreReference()
                    {
                        ProjectPath = projPath2,
                        ProjectUniqueName = "project2"
                    });

                // Create dg file
                var dgFile = new DependencyGraphSpec();

                foreach (var spec in specs)
                {
                    dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);
                    dgFile.AddProject(spec);
                }

                var dgPath = Path.Combine(workingDir, "input.dg");
                dgFile.Save(dgPath);

                var sourceRepos = sources.Select(source => Repository.Factory.GetCoreV3(source.Source)).ToList();

                var providerCache = new RestoreCommandProvidersCache();

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        GlobalPackagesFolder = packagesDir.FullName,
                        Sources = new List<string>() { packageSource.FullName },
                        Inputs = new List<string>() { dgPath },
                        Log = logger,
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        RequestProviders = new List<IRestoreRequestProvider>()
                        {
                            new DependencyGraphFileRequestProvider(providerCache)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var success = summaries.All(s => s.Success);

                    var lockFormat = new LockFileFormat();
                    var lockFile1 = lockFormat.Read(lockPath1);
                    var project2Lib = lockFile1.Libraries.First();

                    // Assert
                    Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                    Assert.True(File.Exists(lockPath1), lockPath1);
                    Assert.True(File.Exists(lockPath2), lockPath2);
                    Assert.Equal("project2", project2Lib.Name);
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_RestoreWithRuntimeAsync()
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
                ""net45"": {
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
                var spec1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), project1Json);

                spec1.RestoreMetadata.Sources = new List<PackageSource> { new PackageSource(packageSource.FullName) };
                spec1.RestoreMetadata.PackagesPath = packagesDir.FullName;
                var dgSpec = new DependencyGraphSpec();
                dgSpec.AddProject(spec1);
                dgSpec.AddRestore(spec1.RestoreMetadata.ProjectUniqueName);

                var logger = new TestLogger();
                var lockPath1 = Path.Combine(spec1.RestoreMetadata.OutputPath, "project.assets.json");

                var sourceRepos = sources.Select(source => Repository.Factory.GetCoreV3(source.Source)).ToList();

                var providerCache = new RestoreCommandProvidersCache();

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        GlobalPackagesFolder = spec1.RestoreMetadata.PackagesPath,
                        Sources = new List<string>() { packageSource.FullName },
                        Log = logger,
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                             new DependencyGraphSpecRequestProvider(providerCache, dgSpec)
                        }
                    };

                    restoreContext.Runtimes.Add("linux-x86");

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var success = summaries.All(s => s.Success);

                    var lockFormat = new LockFileFormat();
                    var lockFile = lockFormat.Read(lockPath1);

                    // Assert
                    Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                    Assert.True(lockFile.Targets.Any(graph => graph.RuntimeIdentifier == "linux-x86"));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_BasicPackageDownloadRestoreAsync()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""downloadDependencies"": [
                       {""name"" : ""y"", ""version"" : ""[1.0.0, )""}
                    ]
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageX);

                // set up package download
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageY);

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.True(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count); // Only X is written in the libraries section.
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);
                    Assert.Equal(0, lockFile.LogMessages.Count);
                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().Name);
                    Assert.True(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "y", "1.0.0"))); // Y is installed
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_MultiplePackageDownloadRestoreAsync()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""downloadDependencies"": [
                       {""name"" : ""y"", ""version"" : ""[1.0.0];[2.0.0]""}
                    ]
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                });

                // set up package download
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                });

                var packageY =
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "2.0.0"
                });

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.True(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count); // Only X is written in the libraries section.
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);
                    Assert.Equal(0, lockFile.LogMessages.Count);
                    Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Count);
                    Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().Name);
                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Last().Name);
                    Assert.True(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "y", "1.0.0"))); // Y 1.0.0 is installed
                    Assert.True(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "y", "2.0.0"))); // Y 2.0.0 is installed
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_PackageDownloadUnresolvedAsync()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""downloadDependencies"": [
                     { ""name"": ""y"", ""version"": ""[2.0.0]"" }
                    ]
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageX);

                // set up package download
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageY);

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.False(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count); // Only X is written in the libraries section.
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);

                    Assert.Equal(1, lockFile.LogMessages.Count);
                    var logMessage = lockFile.LogMessages.First();
                    Assert.Equal(LogLevel.Error, logMessage.Level);
                    Assert.Equal(NuGetLogCode.NU1102, logMessage.Code);
                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().Name);
                    Assert.False(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "y", "1.0.0"))); // Y is not installed
                }
            }
        }
        [Fact]
        public async Task RestoreRunner_PackageReferenceAndPackageDownloadBothLogErrors()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""downloadDependencies"": [
                     { ""name"": ""y"", ""version"": ""[2.0.0]"" }
                    ]
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.5.0"
                };
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageX);

                // set up package download
                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageY);

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.False(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count); // Only X is written in the libraries section.
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);

                    Assert.Equal(2, lockFile.LogMessages.Count);

                    var logMessage = lockFile.LogMessages.First();
                    Assert.Equal(LogLevel.Warning, logMessage.Level);
                    Assert.Equal(NuGetLogCode.NU1603, logMessage.Code);
                    Assert.Equal(1, logMessage.TargetGraphs.Count);

                    var logMessageDD = lockFile.LogMessages.Last();
                    Assert.Equal(LogLevel.Error, logMessageDD.Level);
                    Assert.Equal(NuGetLogCode.NU1102, logMessageDD.Code);
                    Assert.Equal(1, logMessageDD.TargetGraphs.Count);

                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().Name);
                    Assert.False(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "y", "1.0.0"))); // Y is not installed
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_MultiTfmPackageDownloadRestoreAsync()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""downloadDependencies"": [
                       {""name"" : ""y"", ""version"" : ""[1.0.0]""},
                    ]
                },
                ""net46"": {
                    ""downloadDependencies"": [
                       {""name"" : ""z"", ""version"" : ""[1.0.0]""},
                    ]
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                });

                // set up package download
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                });

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                });

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.True(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count); // Only X is written in the libraries section.
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);
                    Assert.Equal(0, lockFile.LogMessages.Count);
                    Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.Count);
                    Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                    Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Count);

                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().Name);
                    Assert.Equal("z", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);
                    Assert.True(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "y", "1.0.0"))); // Y 1.0.0 is installed
                    Assert.True(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "z", "1.0.0"))); // Z 1.0.0 is installed
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_MultiTfmPackageDownloadUnresolved_BothTfmsLogErrorsAsync()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""downloadDependencies"": [
                       {""name"" : ""y"", ""version"" : ""[1.0.0]""},
                    ]
                },
                ""net46"": {
                    ""downloadDependencies"": [
                       {""name"" : ""y"", ""version"" : ""[1.0.0]""},
                    ]
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                });

                // set up package download
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.5.0"
                });

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.False(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count); // Only X is written in the libraries section.
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);


                    Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.Count);
                    Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                    Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Count);
                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().Name);
                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);

                    Assert.Equal(1, lockFile.LogMessages.Count);

                    var logMessage = lockFile.LogMessages.First();
                    Assert.Equal(LogLevel.Error, logMessage.Level);
                    Assert.Equal(NuGetLogCode.NU1102, logMessage.Code);
                    Assert.Equal(2, logMessage.TargetGraphs.Count);

                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().Name);
                    Assert.False(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "y", "1.0.0"))); // Y is not installed
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_MultiTfmPackageDownloadRestore_OnlyMatchingPackagesDownloadedAsync()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""downloadDependencies"": [
                       {""name"" : ""y"", ""version"" : ""[1.0.0]""},
                    ]
                },
                ""net46"": {
                    ""downloadDependencies"": [
                       {""name"" : ""z"", ""version"" : ""[1.0.0]""},
                       {""name"" : ""f"", ""version"" : ""[2.0.0]""},
                    ]
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                });

                // set up package download
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                });

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "z",
                    Version = "1.0.0"
                });

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.False(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count); // Only X is written in the libraries section.
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);
                    Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.Count);
                    Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                    Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Count);
                    Assert.Equal(1, lockFile.LogMessages.Count);

                    var logMessage = lockFile.LogMessages.First();
                    Assert.Equal(LogLevel.Error, logMessage.Level);
                    Assert.Equal(NuGetLogCode.NU1101, logMessage.Code);
                    Assert.Equal(1, logMessage.TargetGraphs.Count);

                    Assert.Equal("y", lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.First().Name);
                    Assert.Equal("f", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);
                    Assert.Equal("z", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Last().Name);
                    Assert.True(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "y", "1.0.0"))); // Y 1.0.0 is installed
                    Assert.True(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "z", "1.0.0"))); // Z 1.0.0 is installed
                    Assert.False(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "f", "1.0.0"))); // F 1.0.0 is not installed
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_MultiTfmPDandPR_LogsWarningsAsync()
        {
            // The scenario here is having the same package be resolved for PD and PR download. If warnings are needed, they are raised for both PD and PR.
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                },
                ""net46"": {
                    ""downloadDependencies"": [
                       {""name"" : ""x"", ""version"" : ""[1.5.0]""},
                    ]
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.5.0"
                });

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.True(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));

                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count);
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);
                    Assert.Equal(2, lockFile.PackageSpec.TargetFrameworks.Count);
                    Assert.Equal(0, lockFile.PackageSpec.TargetFrameworks.First().DownloadDependencies.Count);
                    Assert.Equal(1, lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.Count);
                    Assert.Equal("x", lockFile.PackageSpec.TargetFrameworks.Last().DownloadDependencies.First().Name);

                    Assert.Equal(1, lockFile.LogMessages.Count);
                    var logMessage = lockFile.LogMessages.First();
                    Assert.Equal(LogLevel.Warning, logMessage.Level);
                    Assert.Equal(NuGetLogCode.NU1603, logMessage.Code); // bumped up version.
                    Assert.Equal(1, logMessage.TargetGraphs.Count);
                    Assert.True(Directory.Exists(Path.Combine(globalPackagesFolder.FullName, "x", "1.5.0"))); // x is installed
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_FrameworkReferenceIsWrittenToAssetsFile()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""net45"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""frameworkReferences"": {
                        ""a"" : {
                            ""privateAssets"" : ""none""
                        }
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageX);

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.True(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count); // Only X is written in the libraries section.
                    Assert.Equal("x", lockFile.Targets.First().Libraries.First().Name);
                    Assert.Equal(0, lockFile.LogMessages.Count);
                    Assert.Equal("a", lockFile.PackageSpec.TargetFrameworks.First().FrameworkReferences.Single().Name);
                    Assert.Equal("none", FrameworkDependencyFlagsUtils.GetFlagString(lockFile.PackageSpec.TargetFrameworks.First().FrameworkReferences.Single().PrivateAssets));
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_FrameworkReferenceIsProjectToPackageTransitive()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""netcoreapp3.0"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                // set up project1
                var projectSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec.RestoreMetadata.Sources = sources;
                projectSpec.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;
                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec);
                dgFile.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);
                // set up the packages
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.UseDefaultRuntimeAssemblies = false;
                packageX.AddFile("lib/netcoreapp3.0/packageX.dll");
                packageX.FrameworkReferences.Add(NuGetFramework.Parse("netcoreapp3.0"), new string[] { "Microsoft.WindowsDesktop.App|WinForms" });

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.UseDefaultRuntimeAssemblies = false;
                packageY.AddFile("lib/netcoreapp3.0/packageY.dll");
                packageY.FrameworkReferences.Add(NuGetFramework.Parse("netcoreapp3.0"), new string[] { "Microsoft.WindowsDesktop.App|WPF" });

                packageX.Dependencies.Add(packageY);

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageX);
                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageY);

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    // Assert
                    var assetsFilePath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.True(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(2, lockFile.Libraries.Count);

                    Assert.Equal(1, lockFile.Targets.Count);
                    var xTarget = lockFile.Targets.Single().Libraries.First();
                    var yTarget = lockFile.Targets.Single().Libraries.Last();
                    Assert.Equal("x", xTarget.Name);
                    Assert.Equal("y", yTarget.Name);
                    Assert.Equal("Microsoft.WindowsDesktop.App|WinForms", xTarget.FrameworkReferences.Single());
                    Assert.Equal("Microsoft.WindowsDesktop.App|WPF", yTarget.FrameworkReferences.Single());

                    Assert.Equal(0, lockFile.LogMessages.Count);

                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_FrameworkReferenceIsProjectToProjectTransitive()
        {
            // Arrange
            var project1 = "project1";
            var packageSpec = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""netcoreapp3.0"": {

                }
              }
            }";

            var project2 = "project2";
            var packageSpec2 = @"
            {
              ""version"": ""1.0.0"",
              ""frameworks"": {
                ""netcoreapp3.0"": {
                    ""dependencies"": {
                        ""x"": ""1.0.0""
                    },
                    ""frameworkReferences"": {
                        ""Microsoft.WindowsDesktop.App|WPF"" : {
                            ""privateAssets"" : ""none""
                        }
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                // set up the folders
                var globalPackagesFolder = new DirectoryInfo(Path.Combine(workingDir, "globalPackages")); globalPackagesFolder.Create();
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource")); packageSource.Create();
                var project1Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project1)); project1Folder.Create();
                var project2Folder = new DirectoryInfo(Path.Combine(workingDir, "projects", project2)); project2Folder.Create();

                // set up project1
                var projectSpec1 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project1", Path.Combine(workingDir, "projects"), packageSpec);
                var sources = new List<PackageSource>() { new PackageSource(packageSource.FullName) };
                projectSpec1.RestoreMetadata.Sources = sources;
                projectSpec1.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;

                // set up project2
                var projectSpec2 = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project2", Path.Combine(workingDir, "projects"), packageSpec2);
                projectSpec2.RestoreMetadata.Sources = sources;
                projectSpec2.RestoreMetadata.PackagesPath = globalPackagesFolder.FullName;

                // link the projects
                projectSpec1
                    .RestoreMetadata
                    .TargetFrameworks.Single()
                    .ProjectReferences
                    .Add(new ProjectRestoreReference()
                    {
                        ProjectPath = projectSpec2.FilePath,
                        ProjectUniqueName = projectSpec2.RestoreMetadata.ProjectUniqueName
                    }
                    );

                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(projectSpec1);
                dgFile.AddProject(projectSpec2);
                dgFile.AddRestore(projectSpec1.RestoreMetadata.ProjectUniqueName);
                dgFile.AddRestore(projectSpec2.RestoreMetadata.ProjectUniqueName);

                // set up the packages
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.UseDefaultRuntimeAssemblies = false;
                packageX.AddFile("lib/netcoreapp3.0/packageX.dll");
                packageX.FrameworkReferences.Add(NuGetFramework.Parse("netcoreapp3.0"), new string[] { "Microsoft.WindowsDesktop.App|WinForms" });

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageX);
                var logger = new TestLogger();
                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = logger,
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        }
                    };

                    // Act
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    Assert.True(summaries.All(e => e.Success), string.Join(Environment.NewLine, logger.Messages));

                    // Assert project 2
                    var assetsFilePath2 = Path.Combine(projectSpec2.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath2 = Path.Combine(projectSpec2.RestoreMetadata.OutputPath, $"{project2}.csproj.nuget.g.targets");
                    var propsPath2 = Path.Combine(projectSpec2.RestoreMetadata.OutputPath, $"{project2}.csproj.nuget.g.targets");

                    Assert.True(File.Exists(assetsFilePath2), assetsFilePath2);
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath2, NullLogger.Instance);
                    Assert.Equal(1, lockFile.Libraries.Count);

                    Assert.Equal(1, lockFile.Targets.Count);
                    var xTarget2 = lockFile.Targets.Single().Libraries.Single();
                    Assert.Equal("x", xTarget2.Name);
                    Assert.Equal("Microsoft.WindowsDesktop.App|WinForms", xTarget2.FrameworkReferences.Single());
                    Assert.Equal(0, lockFile.LogMessages.Count);
                    Assert.Equal("Microsoft.WindowsDesktop.App|WPF", lockFile.PackageSpec.TargetFrameworks.Single().FrameworkReferences.Single().Name);
                    Assert.True(File.Exists(targetsPath2));
                    Assert.True(File.Exists(propsPath2));


                    // Assert project 1
                    var assetsFilePath = Path.Combine(projectSpec1.RestoreMetadata.OutputPath, "project.assets.json");
                    var targetsPath = Path.Combine(projectSpec1.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");
                    var propsPath = Path.Combine(projectSpec1.RestoreMetadata.OutputPath, $"{project1}.csproj.nuget.g.targets");

                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);
                    Assert.Equal(2, lockFile.Libraries.Count);

                    Assert.Equal(1, lockFile.Targets.Count);
                    var xTarget = lockFile.Targets.Single().Libraries.First();
                    var project2Target = lockFile.Targets.Single().Libraries.Last();
                    Assert.Equal("x", xTarget.Name);
                    Assert.Equal(project2, project2Target.Name);
                    Assert.Equal("Microsoft.WindowsDesktop.App|WinForms", xTarget.FrameworkReferences.Single());
                    Assert.Equal("Microsoft.WindowsDesktop.App|WPF", project2Target.FrameworkReferences.Single());
                    Assert.Equal(0, lockFile.LogMessages.Count);
                    Assert.True(File.Exists(targetsPath));
                    Assert.True(File.Exists(propsPath));
                }
            }
        }

        [Fact]
        public async Task RestoreRunner_ExecuteAndCommit_ProjectAssetsIsNotCommittedIfNotChanged()
        {
            var assetsFileName = "project.assets.json";
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var sources = new List<PackageSource> { new PackageSource(pathContext.PackageSource) };

                var project1Json = @"
                {
                  ""version"": ""1.0.0"",
                    ""restore"": {
                                    ""projectUniqueName"": ""TestProject"",
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": true
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""[2.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                }
                        },
                        ""centralPackageVersions"": {
                            ""packageA"": ""[2.0.0,)"",
                            ""packageB"": ""[2.0.0,)""
                        }
                    }
                  }
                }";

                var packageA_Version200 = new SimpleTestPackageContext("packageA", "2.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");

                packageA_Version200.Dependencies.Add(packageB_Version200);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    NuGet.Packaging.PackageSaveMode.Defaultv3,
                    packageA_Version200,
                    packageB_Version200
                    );

                // set up the project
                var spec = JsonPackageSpecReader.GetPackageSpec(project1Json, projectName, Path.Combine(projectPath, $"{projectName}.json")).WithTestRestoreMetadata();

                spec.RestoreMetadata.Sources = sources;
                spec.RestoreMetadata.PackagesPath = pathContext.UserPackagesFolder;

                // set up the dg spec.
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);

                using (var cacheContext = new SourceCacheContext())
                {
                    var restoreContext = new RestoreArgs()
                    {
                        CacheContext = cacheContext,
                        DisableParallel = true,
                        Log = new TestLogger(),
                        CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                        PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                        {
                            new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                        },
                        AllowNoOp = false,
                    };

                    // Act + Assert
                    var summaries = await RestoreRunner.RunAsync(restoreContext);
                    var summary = summaries.Single();

                    var assetsFilePath = Path.Combine(spec.BaseDirectory, assetsFileName);

                    Assert.True(summary.Success);
                    Assert.True(File.Exists(assetsFilePath), assetsFilePath);
                    var assetsFileTimeStamp = File.GetCreationTime(assetsFilePath);

                    // restore again, the assets file should not be changed
                    // the result should not be noop as requested by "AllowNoOp"
                    summaries = await RestoreRunner.RunAsync(restoreContext);
                    summary = summaries.Single();

                    var assetsFileTimeStampSecondRestore = File.GetCreationTime(assetsFilePath);
                    Assert.Equal(assetsFileTimeStamp.ToString(), assetsFileTimeStampSecondRestore.ToString());
                    Assert.False(summary.NoOpRestore);

                    // Other verifications
                    var lockFile = LockFileUtilities.GetLockFile(assetsFilePath, NullLogger.Instance);

                    Assert.Equal(2, lockFile.Libraries.Count);
                    Assert.Equal(1, lockFile.Targets.Count);
                    Assert.Equal(2, lockFile.Targets.First().Libraries.Count);
                    Assert.Equal(1, lockFile.CentralTransitiveDependencyGroups.Count);

                    var centralTransitiveDep = lockFile.CentralTransitiveDependencyGroups.First().TransitiveDependencies.FirstOrDefault();
                    Assert.Equal(1, lockFile.CentralTransitiveDependencyGroups.First().TransitiveDependencies.Count());
                    Assert.True(centralTransitiveDep.VersionCentrallyManaged);
                    Assert.Equal("packageB", centralTransitiveDep.Name);
                    Assert.Equal("[2.0.0, )", centralTransitiveDep.LibraryRange.VersionRange.ToNormalizedString());
                    Assert.Equal(LibraryDependencyReferenceType.Transitive, centralTransitiveDep.ReferenceType);
                    Assert.Equal(0, lockFile.LogMessages.Count);
                }
            }
        }
    }
}
