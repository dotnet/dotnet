// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.ProjectSystem;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGet.VisualStudio;

namespace Test.Utility
{
    public class TestCpsPackageReferenceProject
        : CpsPackageReferenceProject
        , IProjectScriptHostService
        , IProjectSystemReferencesReader
    {
        public HashSet<PackageIdentity> ExecuteInitScriptAsyncCalls { get; }
            = new HashSet<PackageIdentity>(PackageIdentity.Comparer);

        public List<TestExternalProjectReference> ProjectReferences { get; }
            = new List<TestExternalProjectReference>();

        public bool IsCacheEnabled { get; set; }

        public bool IsNu1605Error { get; set; }

        public HashSet<PackageSource> ProjectLocalSources { get; set; } = new HashSet<PackageSource>();

        public string AssetsFilePath { get; }

        private PackageSpec _packageSpec;

        private TestCpsPackageReferenceProject(
            string projectName,
            string projectUniqueName,
            string projectFullPath,
            IProjectSystemCache projectSystemCache,
            UnconfiguredProject unconfiguredProject,
            INuGetProjectServices projectServices,
            string projectId,
            string assetsFilePath,
            PackageSpec packageSpec)
            : base(projectName, projectUniqueName, projectFullPath, projectSystemCache, unconfiguredProject, projectServices, projectId)
        {
            ProjectServices = projectServices;
            AssetsFilePath = assetsFilePath;
            _packageSpec = packageSpec;
        }

        public static TestCpsPackageReferenceProject CreateTestCpsPackageReferenceProject(
            string projectName, string projectFullPath, IProjectSystemCache projectSystemCache,
            TestProjectSystemServices projectServices = null, string assetsFilePath = null, PackageSpec packageSpec = null)
        {
            projectServices = projectServices == null ? new TestProjectSystemServices() : projectServices;

            return new TestCpsPackageReferenceProject(
                    projectName: projectName,
                    projectUniqueName: projectName,
                    projectFullPath: projectFullPath,
                    projectSystemCache: projectSystemCache,
                    unconfiguredProject: null,
                    projectServices: projectServices,
                    projectId: projectName,
                    assetsFilePath,
                    packageSpec);
        }

        public static void AddProjectDetailsToCache(IProjectSystemCache projectCache, DependencyGraphSpec parentDependencyGraphSpec, TestCpsPackageReferenceProject parentPackageReferenceProject, ProjectNames parentProjectNames)
        {
            projectCache.AddProjectRestoreInfo(parentProjectNames, parentDependencyGraphSpec, new List<IAssetsLogMessage>());
            projectCache.AddProject(parentProjectNames, vsProjectAdapter: (new Mock<IVsProjectAdapter>()).Object, parentPackageReferenceProject).Should().BeTrue();
        }

        public override string MSBuildProjectPath => base.MSBuildProjectPath;

        public override string ProjectName => base.ProjectName;

        public override async Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context)
        {
            IReadOnlyList<PackageSpec> packageSpecs;

            if (_packageSpec != null)
            {
                packageSpecs = new List<PackageSpec>() { _packageSpec };
            }
            else
            {
                packageSpecs = await base.GetPackageSpecsAsync(context);
            }

            if (IsNu1605Error)
            {
                foreach (var packageSpec in packageSpecs)
                {
                    if (packageSpec?.RestoreMetadata != null)
                    {
                        var allWarningsAsErrors = false;
                        var noWarn = new HashSet<NuGetLogCode>();
                        var warnAsError = new HashSet<NuGetLogCode>();

                        if (packageSpec.RestoreMetadata.ProjectWideWarningProperties != null)
                        {
                            var warningProperties = packageSpec.RestoreMetadata.ProjectWideWarningProperties;
                            allWarningsAsErrors = warningProperties.AllWarningsAsErrors;
                            warnAsError.AddRange<NuGetLogCode>(warningProperties.WarningsAsErrors);
                            noWarn.AddRange<NuGetLogCode>(warningProperties.NoWarn);
                        }

                        warnAsError.Add(NuGetLogCode.NU1605);
                        noWarn.Remove(NuGetLogCode.NU1605);

                        var warningsNotAsErrors = new HashSet<NuGetLogCode>();

                        packageSpec.RestoreMetadata.ProjectWideWarningProperties = new WarningProperties(warnAsError, noWarn, allWarningsAsErrors, warningsNotAsErrors);

                        packageSpec?.RestoreMetadata.Sources.AddRange(new List<PackageSource>(ProjectLocalSources));
                    }
                }
            }

            return packageSpecs;
        }

        public Task ExecutePackageScriptAsync(PackageIdentity packageIdentity, string packageInstallPath, string scriptRelativePath, INuGetProjectContext projectContext, bool throwOnFailure, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExecutePackageInitScriptAsync(PackageIdentity packageIdentity, string packageInstallPath, INuGetProjectContext projectContext, bool throwOnFailure, CancellationToken token)
        {
            ExecuteInitScriptAsyncCalls.Add(packageIdentity);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<LibraryDependency>> GetPackageReferencesAsync(NuGetFramework targetFramework, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectRestoreReference>> GetProjectReferencesAsync(ILogger logger, CancellationToken token)
        {
            var projectRefs = ProjectReferences.Select(e => new ProjectRestoreReference()
            {
                ProjectUniqueName = e.MSBuildProjectPath,
                ProjectPath = e.MSBuildProjectPath,
            });

            return Task.FromResult(projectRefs);
        }

        public override Task PreProcessAsync(INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            return base.PreProcessAsync(nuGetProjectContext, token);
        }

        public override Task PostProcessAsync(INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            return base.PostProcessAsync(nuGetProjectContext, token);
        }

        public override Task<string> GetAssetsFilePathAsync()
        {
            if (AssetsFilePath != null)
            {
                return Task.FromResult(AssetsFilePath);
            }
            return base.GetAssetsFilePathAsync();
        }

        public override Task<string> GetAssetsFilePathOrNullAsync()
        {
            return base.GetAssetsFilePathOrNullAsync();
        }

        public override Task AddFileToProjectAsync(string filePath)
        {
            return base.AddFileToProjectAsync(filePath);
        }

        public override Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context)
        {
            if (_packageSpec != null)
            {
                return Task.FromResult<(IReadOnlyList<PackageSpec>, IReadOnlyList<IAssetsLogMessage>)>((new List<PackageSpec>() { _packageSpec },
                    new List<IAssetsLogMessage>()));
            }

            return base.GetPackageSpecsAndAdditionalMessagesAsync(context);
        }

        public override async Task<bool> InstallPackageAsync(string packageId, VersionRange range, INuGetProjectContext nuGetProjectContext, BuildIntegratedInstallationContext installationContext, CancellationToken token)
        {
            var dependency = new LibraryDependency
            {
                LibraryRange = new LibraryRange(
                    name: packageId,
                    versionRange: range,
                    typeConstraint: LibraryDependencyTarget.Package),
                SuppressParent = installationContext.SuppressParent,
                IncludeType = installationContext.IncludeType
            };

            await ProjectServices.References.AddOrUpdatePackageReferenceAsync(dependency, token);

            return true;
        }

        public override async Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            await ProjectServices.References.RemovePackageReferenceAsync(packageIdentity.Id);

            return true;
        }

        public override async Task<bool> UninstallPackageAsync(
            string packageId,
            BuildIntegratedInstallationContext installationContext,
            CancellationToken token)
        {
            await ProjectServices.References.RemovePackageReferenceAsync(packageId);

            return true;
        }

        public override Task<string> GetCacheFilePathAsync()
        {
            return base.GetCacheFilePathAsync();
        }

        public Task<IReadOnlyList<(string id, string[] metadata)>> GetItemsAsync(string itemTypeName, params string[] metadataNames)
        {
            throw new NotImplementedException();
        }
    }
}
