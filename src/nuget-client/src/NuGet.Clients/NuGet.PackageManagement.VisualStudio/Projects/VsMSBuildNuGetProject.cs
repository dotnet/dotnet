// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft;
using Microsoft.VisualStudio.Shell;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// This is an implementation of <see cref="MSBuildNuGetProject"/> that has knowledge about interacting with DTE.
    /// Since the base class <see cref="MSBuildNuGetProject"/> is in the NuGet.Core solution, it does not have
    /// references to DTE.
    /// </summary>
    internal class VsMSBuildNuGetProject : MSBuildNuGetProject
    {
        public VsMSBuildNuGetProject(
            IVsProjectAdapter projectAdapter,
            IMSBuildProjectSystem msbuildNuGetProjectSystem,
            string folderNuGetProjectPath,
            string packagesConfigFolderPath,
            INuGetProjectServices projectServices)
            : base(
                msbuildNuGetProjectSystem,
                folderNuGetProjectPath,
                packagesConfigFolderPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Assumes.Present(projectAdapter);
            Assumes.Present(msbuildNuGetProjectSystem);
            Assumes.Present(projectServices);

            InternalMetadata.Add(NuGetProjectMetadataKeys.ProjectId, projectAdapter.ProjectId);
            InternalMetadata.Add(ProjectBuildProperties.NuGetAudit, projectAdapter.BuildProperties.GetPropertyValue(ProjectBuildProperties.NuGetAudit));
            InternalMetadata.Add(ProjectBuildProperties.NuGetAuditLevel, projectAdapter.BuildProperties.GetPropertyValue(ProjectBuildProperties.NuGetAuditLevel));

            ProjectServices = projectServices;
        }
    }
}
