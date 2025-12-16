// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Commands.Test;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Versioning;

namespace NuGet.Test
{
    public class TestPackageReferenceNuGetProject
        : BuildIntegratedNuGetProject
        , INuGetProjectServices
        , IProjectScriptHostService
        , IProjectSystemReferencesReader
    {
        public HashSet<PackageIdentity> ExecuteInitScriptAsyncCalls { get; }
            = new HashSet<PackageIdentity>(PackageIdentity.Comparer);

        public bool IsCacheEnabled { get; set; }

        public IProjectSystemCapabilities Capabilities => throw new NotImplementedException();

        public IProjectSystemReferencesReader ReferencesReader => this;

        public IProjectSystemReferencesService References => throw new NotImplementedException();

        public IProjectSystemService ProjectSystem => throw new NotImplementedException();

        public IProjectScriptHostService ScriptService => this;

        public override string ProjectName => _packageSpec.Name;

        public override string MSBuildProjectPath => _packageSpec.FilePath;

        private PackageSpec _packageSpec;

        public TestPackageReferenceNuGetProject(
            PackageSpec packageSpec,
            IMSBuildProjectSystem msbuildProjectSystem)
        {
            _packageSpec = packageSpec ?? throw new ArgumentNullException(nameof(packageSpec));
            if (packageSpec.TargetFrameworks.Count != 1)
            {
                throw new ArgumentException("The package spec needs exactly 1 target framework");
            }
            ProjectServices = this;
            InternalMetadata.Add(NuGetProjectMetadataKeys.TargetFramework, packageSpec.TargetFrameworks[0].FrameworkName);
            InternalMetadata.Add(NuGetProjectMetadataKeys.Name, packageSpec.Name);
            InternalMetadata.Add(NuGetProjectMetadataKeys.FullPath, packageSpec.FilePath);
            ProjectStyle = ProjectStyle.PackageReference;
        }

        public void AddProjectReference(TestPackageReferenceNuGetProject project)
        {
            _packageSpec = _packageSpec.WithTestProjectReference(project._packageSpec);
        }

        public void AddProjectReference(PackageSpec project)
        {
            _packageSpec = _packageSpec.WithTestProjectReference(project);
        }

        public override Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context)
        {
            if (IsCacheEnabled)
            {
                if (context.PackageSpecCache.TryGetValue(MSBuildProjectPath, out var cachedResult))
                {
                    return Task.FromResult<IReadOnlyList<PackageSpec>>([cachedResult]);
                }
            }

            return Task.FromResult<IReadOnlyList<PackageSpec>>([_packageSpec]);
        }

        public Task ExecutePackageScriptAsync(PackageIdentity packageIdentity, string packageInstallPath, string scriptRelativePath, INuGetProjectContext projectContext, bool throwOnFailure, CancellationToken token)
        {
            ExecuteInitScriptAsyncCalls.Add(packageIdentity);
            return Task.CompletedTask;
        }

        public Task<bool> ExecutePackageInitScriptAsync(PackageIdentity packageIdentity, string packageInstallPath, INuGetProjectContext projectContext, bool throwOnFailure, CancellationToken token)
        {
            ExecuteInitScriptAsyncCalls.Add(packageIdentity);
            return TaskResult.True;
        }

        public Task<IEnumerable<LibraryDependency>> GetPackageReferencesAsync(NuGetFramework targetFramework, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectRestoreReference>> GetProjectReferencesAsync(ILogger logger, CancellationToken token)
        {
            return Task.FromResult<IEnumerable<ProjectRestoreReference>>(_packageSpec.RestoreMetadata.TargetFrameworks[0].ProjectReferences);
        }

        public Task<IReadOnlyList<(string id, string[] metadata)>> GetItemsAsync(string itemTypeName, params string[] metadataNames)
        {
            throw new NotImplementedException();
        }

        public override Task<string> GetAssetsFilePathAsync()
        {
            return Task.FromResult(Path.Combine(_packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName));
        }

        public override Task<string> GetCacheFilePathAsync()
        {
            return Task.FromResult(NoOpRestoreUtilities.GetProjectCacheFilePath(_packageSpec.RestoreMetadata.OutputPath));
        }

        public override Task<string> GetAssetsFilePathOrNullAsync()
        {
            return Task.FromResult(Path.Combine(_packageSpec.RestoreMetadata.OutputPath, LockFileFormat.AssetsFileName));
        }

        public override Task AddFileToProjectAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public override Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context)
        {
            return Task.FromResult<(IReadOnlyList<PackageSpec>, IReadOnlyList<IAssetsLogMessage>)>(([_packageSpec], []));
        }

        public override Task<bool> InstallPackageAsync(string packageId, VersionRange range, INuGetProjectContext nuGetProjectContext, BuildIntegratedInstallationContext installationContext, CancellationToken token)
        {
            PackageSpecOperations.AddOrUpdateDependency(_packageSpec, new PackageDependency(packageId, range));
            return Task.FromResult(true);
        }

        public override Task<bool> UninstallPackageAsync(string packageId, BuildIntegratedInstallationContext installationContext, CancellationToken token)
        {
            PackageSpecOperations.RemoveDependency(_packageSpec, packageId);
            return Task.FromResult(true);
        }

        public override Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
        {
            PackageSpecOperations.RemoveDependency(_packageSpec, packageIdentity.Id);
            return Task.FromResult(true);
        }

        public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
        {
            return Task.FromResult<IEnumerable<PackageReference>>([.. _packageSpec.TargetFrameworks.SelectMany(
                e => e.Dependencies.Select(p =>
                new PackageReference(
                    new PackageIdentity(p.Name, p.LibraryRange.VersionRange.MinVersion),
                    e.FrameworkName)))]);
        }
    }
}
