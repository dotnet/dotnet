// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    /// <summary>
    /// Factory helpers to create CPS and Legacy PackageReference test projects
    /// </summary>
    internal static class ProjectFactories
    {
        internal static CpsPackageReferenceProject CreateCpsPackageReferenceProject(string projectName, string projectFullPath, IProjectSystemCache projectSystemCache)
        {
            var projectServices = new TestProjectSystemServices();

            return new CpsPackageReferenceProject(
                    projectName: projectName,
                    projectUniqueName: projectName,
                    projectFullPath: projectFullPath,
                    projectSystemCache: projectSystemCache,
                    unconfiguredProject: null,
                    projectServices: projectServices,
                    projectId: projectName);
        }

        internal static LegacyPackageReferenceProject CreateLegacyPackageReferenceProject(TestDirectory testDirectory, string projectId, IVsProjectThreadingService threadingService, LibraryDependency[] pkgDependencies)
        {
            var framework = NuGetFramework.Parse("netstandard13");
            IVsProjectAdapter projectAdapter = CreateProjectAdapter(testDirectory);

            var projectServices = new TestProjectSystemServices();
            projectServices.SetupInstalledPackages(
                framework,
                pkgDependencies);

            var testProject = new LegacyPackageReferenceProject(
                projectAdapter,
                projectId,
                projectServices,
                threadingService);

            return testProject;
        }

        internal static LegacyPackageReferenceProject CreateLegacyPackageReferenceProject(TestDirectory testDirectory, string projectId, string range, IVsProjectThreadingService threadingService)
        {
            var onedep = new[]
            {
                new LibraryDependency
                {
                    LibraryRange = new LibraryRange(
                        "packageA",
                        VersionRange.Parse(range),
                        LibraryDependencyTarget.Package)
                }
            };

            return CreateLegacyPackageReferenceProject(testDirectory: testDirectory, projectId: projectId, threadingService: threadingService, pkgDependencies: onedep);
        }

        internal static IVsProjectAdapter CreateProjectAdapter(string fullPath)
        {
            var projectBuildProperties = new Mock<IVsProjectBuildProperties>();
            return CreateProjectAdapter(fullPath, projectBuildProperties);
        }

        internal static IVsProjectAdapter CreateProjectAdapter(string fullPath, Mock<IVsProjectBuildProperties> projectBuildProperties)
        {
            var projectAdapter = CreateProjectAdapter(projectBuildProperties);

            projectAdapter
                .Setup(x => x.FullProjectPath)
                .Returns(Path.Combine(fullPath, "foo.csproj"));
            projectAdapter
                .Setup(x => x.GetTargetFramework())
                .Returns(NuGetFramework.Parse("netstandard13"));

            var testMSBuildProjectExtensionsPath = Path.Combine(fullPath, "obj");
            Directory.CreateDirectory(testMSBuildProjectExtensionsPath);
            projectAdapter
                .Setup(x => x.GetMSBuildProjectExtensionsPath())
                .Returns(testMSBuildProjectExtensionsPath);

            return projectAdapter.Object;
        }

        internal static Mock<IVsProjectAdapter> CreateProjectAdapter(Mock<IVsProjectBuildProperties> projectBuildProperties)
        {
            var projectAdapter = new Mock<IVsProjectAdapter>();

            projectAdapter
                .SetupGet(x => x.ProjectName)
                .Returns("TestProject");

            projectAdapter
                .Setup(x => x.Version)
                .Returns("1.0.0");

            projectAdapter
                .Setup(x => x.BuildProperties)
                .Returns(projectBuildProperties.Object);

            return projectAdapter;
        }

        internal static ProjectNames GetTestProjectNames(string projectPath, string projectUniqueName)
        {
            var projectNames = new ProjectNames(
            fullName: projectPath,
            uniqueName: projectUniqueName,
            shortName: projectUniqueName,
            customUniqueName: projectUniqueName,
            projectId: Guid.NewGuid().ToString());
            return projectNames;
        }

        internal static PackageSpec GetPackageSpec(string projectName, string packageSpecFullPath, string version)
        {
            string referenceSpec = $@"
                {{
                    ""frameworks"":
                    {{
                        ""net5.0"":
                        {{
                            ""dependencies"":
                            {{
                                ""packageA"":
                                {{
                                    ""version"": ""{version}"",
                                    ""target"": ""Package""
                                }},
                            }}
                        }}
                    }}
                }}";
            return JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, packageSpecFullPath).WithTestRestoreMetadata();
        }

        internal static async Task CreatePackagesAsync(SimpleTestPathContext rootDir, string packageAVersion = "2.15.3", string packageBVersion = "1.0.0")
        {
            await SimpleTestPackageUtility.CreateFullPackageAsync(rootDir.PackageSource, "PackageB", packageBVersion);
            await SimpleTestPackageUtility.CreateFullPackageAsync(rootDir.PackageSource, "PackageA", packageAVersion,
                new Packaging.Core.PackageDependency[]
                {
                    new Packaging.Core.PackageDependency("PackageB", VersionRange.Parse(packageBVersion))
                });
        }

        internal static CpsPackageReferenceProject PrepareCpsRestoredProject(PackageSpec packageSpec, IProjectSystemCache projectSystemCache = null)
        {
            var projectCache = projectSystemCache ?? new ProjectSystemCache();
            CpsPackageReferenceProject project = CreateCpsPackageReferenceProject(packageSpec.Name, packageSpec.FilePath, projectCache);
            UpdateProjectSystemCache(projectCache, packageSpec, project);

            return project;
        }

        internal static void UpdateProjectSystemCache(IProjectSystemCache projectCache, PackageSpec packageSpec, NuGetProject project)
        {
            ProjectNames projectNames = GetTestProjectNames(packageSpec.FilePath, packageSpec.Name);
            DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpec);
            projectCache.AddProjectRestoreInfo(projectNames, dgSpec, new List<IAssetsLogMessage>());
            projectCache.AddProject(projectNames, Mock.Of<IVsProjectAdapter>(), project);
        }

        internal static async Task RestorePackageSpecsAsync(SimpleTestPathContext rootDir, ITestOutputHelper output = null, params PackageSpec[] packageSpecs)
        {
            var logger = output == null ? new TestLogger() : new TestLogger(output);
            var restoreContext = new RestoreArgs()
            {
                Sources = new List<string>() { rootDir.PackageSource },
                GlobalPackagesFolder = rootDir.UserPackagesFolder,
                Log = logger,
                CacheContext = new SourceCacheContext(),
            };

            DependencyGraphSpec dgSpec = ProjectTestHelpers.GetDGSpecForAllProjects(packageSpecs);
            var dgProvider = new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgSpec);

            foreach (RestoreSummaryRequest request in await dgProvider.CreateRequests(restoreContext))
            {
                var command = new RestoreCommand(request.Request);
                RestoreResult restoreResult = await command.ExecuteAsync();
                await restoreResult.CommitAsync(logger, CancellationToken.None); // Force assets file creation

                Assert.True(restoreResult.Success);
            }
        }
    }
}
