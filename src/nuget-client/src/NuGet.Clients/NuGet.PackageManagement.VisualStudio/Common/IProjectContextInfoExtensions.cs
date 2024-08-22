// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class IProjectContextInfoExtensions
    {
        public static async ValueTask<bool> IsUpgradeableAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(projectContextInfo);
            Assumes.NotNull(serviceBroker);

            cancellationToken.ThrowIfCancellationRequested();

            using (INuGetProjectUpgraderService? projectUpgrader = await serviceBroker.GetProxyAsync<INuGetProjectUpgraderService>(
                NuGetServices.ProjectUpgraderService,
                cancellationToken: cancellationToken))
            {
                Assumes.NotNull(projectUpgrader);

                return await projectUpgrader.IsProjectUpgradeableAsync(projectContextInfo.ProjectId, cancellationToken);
            }
        }

        public static async ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>> GetInstalledPackagesAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(projectContextInfo);
            Assumes.NotNull(serviceBroker);

            cancellationToken.ThrowIfCancellationRequested();

            using (INuGetProjectManagerService projectManager = await GetProjectManagerAsync(serviceBroker, cancellationToken))
            {
                return await projectManager.GetInstalledPackagesAsync(new string[] { projectContextInfo.ProjectId }, cancellationToken);
            }
        }

        public static async ValueTask<IInstalledAndTransitivePackages> GetInstalledAndTransitivePackagesAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken) => await GetInstalledAndTransitivePackagesAsync(projectContextInfo, serviceBroker, includeTransitiveOrigins: false, cancellationToken);

        public static async ValueTask<IInstalledAndTransitivePackages> GetInstalledAndTransitivePackagesAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            bool includeTransitiveOrigins,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(projectContextInfo);
            Assumes.NotNull(serviceBroker);

            cancellationToken.ThrowIfCancellationRequested();

            IInstalledAndTransitivePackages projectPackages;
            using (INuGetProjectManagerService projectManager = await GetProjectManagerAsync(serviceBroker, cancellationToken))
            {
                projectPackages = await projectManager.GetInstalledAndTransitivePackagesAsync(new string[] { projectContextInfo.ProjectId }, includeTransitiveOrigins, cancellationToken);
            }

            return projectPackages;
        }

        /// <summary>
        /// Get packageFolders section from assets file in a PackageReference project
        /// </summary>
        /// <param name="projectContextInfo">A project</param>
        /// <param name="serviceBroker">Service Broker to gather data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection with all package folders listed in assets file or empty collection if none</returns>
        /// <exception cref="ArgumentNullException">If any argument is null</exception>
        /// <remarks><see cref="NuGetProjectManagerService.GetPackageFoldersAsync(IReadOnlyCollection{string}, CancellationToken)"/></remarks>
        public static async ValueTask<IReadOnlyCollection<string>> GetPackageFoldersAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
            if (projectContextInfo == null)
            {
                throw new ArgumentNullException(nameof(projectContextInfo));
            }
            if (serviceBroker == null)
            {
                throw new ArgumentNullException(nameof(serviceBroker));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using INuGetProjectManagerService projectManager = await GetProjectManagerAsync(serviceBroker, cancellationToken);
            return await projectManager.GetPackageFoldersAsync(new string[] { projectContextInfo.ProjectId }, cancellationToken);
        }

        public static async ValueTask<IReadOnlyCollection<NuGetFramework>> GetTargetFrameworksAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(projectContextInfo);
            Assumes.NotNull(serviceBroker);

            cancellationToken.ThrowIfCancellationRequested();

            using (INuGetProjectManagerService projectManager = await GetProjectManagerAsync(serviceBroker, cancellationToken))
            {
                return await projectManager.GetTargetFrameworksAsync(new string[] { projectContextInfo.ProjectId }, cancellationToken);
            }
        }

        public static async ValueTask<bool> IsCentralPackageManagementEnabledAsync(this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(projectContextInfo);
            Assumes.NotNull(serviceBroker);

            cancellationToken.ThrowIfCancellationRequested();

            using (INuGetProjectManagerService projectManager = await GetProjectManagerAsync(serviceBroker, cancellationToken))
            {
                return await projectManager.IsCentralPackageManagementEnabledAsync(projectContextInfo.ProjectId, cancellationToken);
            }
        }

        public static async ValueTask<IProjectMetadataContextInfo> GetMetadataAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(projectContextInfo);
            Assumes.NotNull(serviceBroker);

            cancellationToken.ThrowIfCancellationRequested();

            using (INuGetProjectManagerService projectManager = await GetProjectManagerAsync(serviceBroker, cancellationToken))
            {
                return await projectManager.GetMetadataAsync(projectContextInfo.ProjectId, cancellationToken);
            }
        }

        public static async ValueTask<string?> GetUniqueNameOrNameAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
            IProjectMetadataContextInfo metadata = await GetMetadataAsync(projectContextInfo, serviceBroker, cancellationToken);

            return metadata.UniqueName ?? metadata.Name;
        }

        public static async ValueTask<(bool, string?)> TryGetInstalledPackageFilePathAsync(
            this IProjectContextInfo projectContextInfo,
            IServiceBroker serviceBroker,
            PackageIdentity packageIdentity,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(projectContextInfo);
            Assumes.NotNull(serviceBroker);
            Assumes.NotNull(packageIdentity);

            cancellationToken.ThrowIfCancellationRequested();

            using (INuGetProjectManagerService projectManager = await GetProjectManagerAsync(serviceBroker, cancellationToken))
            {
                return await projectManager.TryGetInstalledPackageFilePathAsync(
                    projectContextInfo.ProjectId,
                    packageIdentity,
                    cancellationToken);
            }
        }

        private static async ValueTask<INuGetProjectManagerService> GetProjectManagerAsync(
            IServiceBroker serviceBroker,
            CancellationToken cancellationToken)
        {
#pragma warning disable ISB001 // Dispose of proxies
            INuGetProjectManagerService? projectManager = await serviceBroker.GetProxyAsync<INuGetProjectManagerService>(
                 NuGetServices.ProjectManagerService,
                 cancellationToken: cancellationToken);
#pragma warning restore ISB001 // Dispose of proxies

            Assumes.NotNull(projectManager);

            return projectManager;
        }
    }
}
