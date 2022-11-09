// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// A service API providing methods of executing package scripts for the underlying project.
    /// </summary>
    public interface IProjectScriptHostService
    {
        /// <summary>
        /// Executes a package script in the project's context.
        /// </summary>
        /// <param name="packageIdentity">Package id</param>
        /// <param name="packageInstallPath">Package install path</param>
        /// <param name="scriptRelativePath">Script path relative to the package install path</param>
        /// <param name="projectContext">Project context</param>
        /// <param name="throwOnFailure">Flag to control error handling</param>
        /// <param name="token">A cancellation token</param>
        Task ExecutePackageScriptAsync(
            PackageIdentity packageIdentity,
            string packageInstallPath,
            string scriptRelativePath,
            INuGetProjectContext projectContext,
            bool throwOnFailure,
            CancellationToken token);

        /// <summary>
        /// Executes init.ps1 package script in the project's context.
        /// </summary>
        /// <param name="packageIdentity">Package id</param>
        /// <param name="packageInstallPath">Package files location</param>
        /// <param name="projectContext">Project context</param>
        /// <param name="throwOnFailure">Flag to control error handling</param>
        /// <param name="token">A cancellation token</param>
        /// <returns><code>true</code> if succeeded, otherwise - <code>false</code>.</returns>
        Task<bool> ExecutePackageInitScriptAsync(
            PackageIdentity packageIdentity,
            string packageInstallPath,
            INuGetProjectContext projectContext,
            bool throwOnFailure,
            CancellationToken token);
    }
}
