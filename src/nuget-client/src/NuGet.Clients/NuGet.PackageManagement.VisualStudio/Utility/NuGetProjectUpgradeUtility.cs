// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class NuGetProjectUpgradeUtility
    {
        private static readonly HashSet<string> UpgradeableProjectTypes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                VsProjectTypes.CsharpProjectTypeGuid,
                VsProjectTypes.VbProjectTypeGuid,
                VsProjectTypes.FsharpProjectTypeGuid
            };

        private static readonly HashSet<string> UnupgradeableProjectTypes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                VsProjectTypes.CppProjectTypeGuid,
                VsProjectTypes.WebApplicationProjectTypeGuid,
                VsProjectTypes.WebSiteProjectTypeGuid
            };

        public static async Task<bool> IsNuGetProjectUpgradeableAsync(NuGetProject nuGetProject, Project envDTEProject = null, bool needsAPackagesConfig = true)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (nuGetProject == null && envDTEProject == null)
            {
                return false;
            }

            nuGetProject = nuGetProject ?? await GetNuGetProject(envDTEProject);

            if (nuGetProject == null)
            {
                return false;
            }

            // check if current project is packages.config based or not
            var msBuildNuGetProject = nuGetProject as MSBuildNuGetProject;
            if (msBuildNuGetProject == null || (!msBuildNuGetProject.PackagesConfigNuGetProject.PackagesConfigExists() && needsAPackagesConfig))
            {
                return false;
            }

            // this further check if current project system supports VSProject4 or not which is essential to skip
            // projects like c++ which currently doesn't support VSProject4 implementation for PackageReference
            if (!msBuildNuGetProject.ProjectServices.Capabilities.SupportsPackageReferences)
            {
                return false;
            }

            if (envDTEProject == null)
            {
                var vsmsBuildNuGetProjectSystem =
                    msBuildNuGetProject.ProjectSystem as VsMSBuildProjectSystem;
                if (vsmsBuildNuGetProjectSystem == null)
                {
                    return false;
                }
                envDTEProject = vsmsBuildNuGetProjectSystem.VsProjectAdapter.Project;
            }

            if (!await EnvDTEProjectUtility.IsSupportedAsync(envDTEProject))
            {
                return false;
            }

            return await IsProjectPackageReferenceCompatibleAsync(envDTEProject);
        }

        private static async Task<NuGetProject> GetNuGetProject(Project envDTEProject)
        {
            var solutionManager = await ServiceLocator.GetComponentModelServiceAsync<IVsSolutionManager>();

            var projectSafeName = await envDTEProject.GetCustomUniqueNameAsync();
            var nuGetProject = await solutionManager.GetNuGetProjectAsync(projectSafeName);
            return nuGetProject;
        }

        private static async Task<bool> IsProjectPackageReferenceCompatibleAsync(Project project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var projectGuids = await project.GetProjectTypeGuidsAsync();

            if (projectGuids.Any(t => UnupgradeableProjectTypes.Contains(t)))
            {
                return false;
            }

            // Project is supported language, and not an unsupported type
            return UpgradeableProjectTypes.Contains(project.Kind) &&
                   projectGuids.All(projectTypeGuid => !ProjectType.IsUnsupported(projectTypeGuid));
        }

        public static bool IsPackagesConfigSelected(IVsMonitorSelection vsMonitorSelection)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var selectedFileName = GetSelectedFileName(vsMonitorSelection);
            return !string.IsNullOrEmpty(selectedFileName) && Path.GetFileName(selectedFileName).Equals("packages.config", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetSelectedFileName(IVsMonitorSelection vsMonitorSelection)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (vsMonitorSelection == null)
            {
                return string.Empty;
            }

            var hHierarchy = IntPtr.Zero;
            var hContainer = IntPtr.Zero;
            try
            {
                IVsMultiItemSelect multiItemSelect;
                uint itemId;
                if (vsMonitorSelection.GetCurrentSelection(out hHierarchy, out itemId, out multiItemSelect, out hContainer) != 0)
                {
                    return string.Empty;
                }
                if (itemId >= VSConstants.VSITEMID_SELECTION)
                {
                    return string.Empty;
                }
                var hierarchy = Marshal.GetUniqueObjectForIUnknown(hHierarchy) as IVsHierarchy;
                if (hierarchy == null)
                {
                    return string.Empty;
                }
                string fileName;
                hierarchy.GetCanonicalName(itemId, out fileName);
                return fileName;
            }
            finally
            {
                if (hHierarchy != IntPtr.Zero)
                {
                    Marshal.Release(hHierarchy);
                }
                if (hContainer != IntPtr.Zero)
                {
                    Marshal.Release(hContainer);
                }
            }
        }
    }
}
