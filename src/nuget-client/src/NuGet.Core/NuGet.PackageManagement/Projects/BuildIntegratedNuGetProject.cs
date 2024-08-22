// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.ProjectManagement.Projects
{
    /// <summary>
    /// A NuGet integrated MSBuild project.k
    /// These projects contain a project.json or package references in CSProj
    /// </summary>
    [DebuggerDisplay("{ProjectName} [{ProjectStyle}]")]
    public abstract class BuildIntegratedNuGetProject
        : NuGetProject
        , INuGetIntegratedProject
        , IDependencyGraphProject
    {
        protected BuildIntegratedNuGetProject()
        {
        }

        /// <summary>
        /// Project name
        /// </summary>
        public abstract string ProjectName { get; }

        public abstract string MSBuildProjectPath { get; }

        /// <summary>
        /// Returns the path to the assets file or the lock file. Throws an exception if the assets file path cannot be
        /// determined.
        /// </summary>
        public abstract Task<string> GetAssetsFilePathAsync();

        public abstract Task<string> GetCacheFilePathAsync();

        /// <summary>
        /// Returns the path to the assets file or the lock file. Returns null if the assets file path cannot be
        /// determined.
        /// </summary>
        public abstract Task<string> GetAssetsFilePathOrNullAsync();

        /// <summary>
        /// Add specified file to Project system
        /// </summary>
        /// <param name="filePath">file to be added</param>
        /// <returns></returns>
        public abstract Task AddFileToProjectAsync(string filePath);

        public abstract Task<IReadOnlyList<PackageSpec>> GetPackageSpecsAsync(DependencyGraphCacheContext context);

        public abstract Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context);

        public abstract Task<bool> InstallPackageAsync(
            string packageId,
            VersionRange range,
            INuGetProjectContext nuGetProjectContext,
            BuildIntegratedInstallationContext installationContext,
            CancellationToken token);

        public override sealed Task<bool> InstallPackageAsync(
            PackageIdentity packageIdentity,
            DownloadResourceResult downloadResourceResult,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            throw new NotImplementedException("This API should not be called for BuildIntegratedNuGetProject.");
        }

        public abstract Task<bool> UninstallPackageAsync(
            string packageId,
            BuildIntegratedInstallationContext installationContext,
            CancellationToken token);
    }
}
