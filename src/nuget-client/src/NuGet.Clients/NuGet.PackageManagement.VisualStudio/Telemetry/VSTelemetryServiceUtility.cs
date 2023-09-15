// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.Telemetry
{
    /// <summary>
    /// Some utility apis for telemetry operations.
    /// </summary>
    public static class VSTelemetryServiceUtility
    {
        public static readonly Lazy<string> NuGetVersion
            = new Lazy<string>(() => ClientVersionUtility.GetNuGetAssemblyVersion());

        /// <summary>
        /// Create ActionTelemetryEvent instance.
        /// </summary>
        /// <param name="projects"></param>
        /// <param name="operationType"></param>
        /// <param name="source"></param>
        /// <param name="startTime"></param>
        /// <param name="status"></param>
        /// <param name="statusMessage"></param>
        /// <param name="packageCount"></param>
        /// <param name="endTime"></param>
        /// <param name="duration"></param>
        /// <param name="isPackageSourceMappingEnabled"></param>
        /// <returns></returns>
        public static VSActionsTelemetryEvent GetActionTelemetryEvent(
            string operationId,
            IEnumerable<NuGetProject> projects,
            NuGetProjectActionType operationType,
            OperationSource source,
            DateTimeOffset startTime,
            NuGetOperationStatus status,
            int packageCount,
            double duration,
            bool isPackageSourceMappingEnabled)
        {
            var sortedProjects = projects.OrderBy(
                project => project.GetMetadata<string>(NuGetProjectMetadataKeys.UniqueName));

            var projectIds = sortedProjects.Select(
                project => project.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId)).ToArray();

            return new VSActionsTelemetryEvent(
                operationId,
                projectIds,
                operationType,
                source,
                startTime,
                status,
                packageCount,
                DateTimeOffset.Now,
                duration,
                isPackageSourceMappingEnabled: isPackageSourceMappingEnabled);
        }

        public static async Task<ProjectTelemetryEvent> GetProjectTelemetryEventAsync(NuGetProject nuGetProject)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }
            string projectUniqueName = string.Empty;
            ProjectTelemetryEvent returnValue = null;

            try
            {
                // Get the project details.
                projectUniqueName = nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.UniqueName);
                string projectId = nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId);
                NuGetProjectType projectType = GetProjectType(nuGetProject);
                bool isUpgradable = await NuGetProjectUpgradeUtility.IsNuGetProjectUpgradeableAsync(nuGetProject);
                string fullPath = nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.FullPath);

                returnValue = new ProjectTelemetryEvent(
                    NuGetVersion.Value,
                    projectId,
                    projectType,
                    isUpgradable,
                    fullPath);
            }
            catch (Exception ex)
            {
                // ArgumentException means project metadata is empty
                // DTE exceptions could mean VS process has a severe failure
                string message =
                    $"Failed to emit project information for project '{projectUniqueName}'. Exception:" +
                    Environment.NewLine +
                    ex.ToString();

                ActivityLog.LogWarning(ExceptionHelper.LogEntrySource, message);
                Debug.Fail(message);

                await TelemetryUtility.PostFaultAsync(ex, nameof(VSTelemetryServiceUtility), nameof(GetProjectTelemetryEventAsync));
            }

            return returnValue;
        }

        public static NuGetProjectType GetProjectType(NuGetProject nuGetProject)
        {
            NuGetProjectType projectType = NuGetProjectType.Unknown;

            if (nuGetProject is MSBuildNuGetProject msbuildProject)
            {
                if (msbuildProject.DoesPackagesConfigExists())
                {
                    projectType = NuGetProjectType.PackagesConfig;
                }
                else
                {
                    projectType = NuGetProjectType.UnconfiguredNuGetType;
                }
            }
            else if (nuGetProject is CpsPackageReferenceProject)
            {
                projectType = NuGetProjectType.CPSBasedPackageRefs;
            }
            else if (nuGetProject is LegacyPackageReferenceProject)
            {
                projectType = NuGetProjectType.LegacyProjectSystemWithPackageRefs;
            }
            else if (nuGetProject is ProjectJsonNuGetProject)
            {
                projectType = NuGetProjectType.UwpProjectJson;
            }

            return projectType;
        }

        public static string NormalizePackageId(string packageId)
        {
            return packageId?.ToLowerInvariant() ?? "(empty package id)";
        }
    }
}
