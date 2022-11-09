// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Commands;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Common;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace NuGet.VisualStudio
{
    public static class EnvDteProjectExtensions
    {
        public static async Task<IVsHierarchy> ToVsHierarchyAsync(this EnvDTE.Project project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IVsHierarchy hierarchy;

            // Get the vs solution
            var solution = await ServiceLocator.GetGlobalServiceAsync<SVsSolution, IVsSolution>();
            int hr = solution.GetProjectOfUniqueName(project.GetUniqueName(), out hierarchy);

            if (hr != VSConstants.S_OK)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return hierarchy;
        }

        public static async Task<string[]> GetProjectTypeGuidsAsync(this EnvDTE.Project project)
        {
            Verify.ArgumentIsNotNull(project, nameof(project));

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Get the vs hierarchy as an IVsAggregatableProject to get the project type guids
            IVsHierarchy hierarchy = await ToVsHierarchyAsync(project);
            return VsHierarchyUtility.GetProjectTypeGuidsFromHierarchy(hierarchy);
        }

        #region Constants

        public const string WebConfig = "web.config";
        public const string AppConfig = "app.config";
        public const string FullPath = "FullPath";
        public const string ProjectDirectory = "ProjectDirectory";

        #endregion // Constants

        #region Get Project Information

        /// <summary>
        /// Returns the full path including the project file name.
        /// </summary>
        public static string GetFullProjectPath(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Debug.Assert(envDTEProject != null);
            if (IsUnloaded(envDTEProject))
            {
                // Find the project file path from the UniqueName which contains the file path 
                // relative to the solution directory for unloaded projects.
                var solutionDirectory = Path.GetDirectoryName(envDTEProject.DTE.Solution.FullName);
                return Path.Combine(solutionDirectory, envDTEProject.UniqueName);
            }

            // FullName
            var fullName = GetPotentialFullPathOrNull(envDTEProject.FullName);

            if (!string.IsNullOrEmpty(fullName))
            {
                return fullName;
            }

            // FullPath
            var fullPath = GetPotentialFullPathOrNull(GetPropertyValue<string>(envDTEProject, FullPath));

            if (!string.IsNullOrEmpty(fullPath))
            {
                return fullPath;
            }

            return null;
        }

        /// <summary>
        /// Returns the full path of the project directory.
        /// </summary>
        /// <param name="envDTEProject">The project.</param>
        /// <returns>The full path of the project directory.</returns>
        public static async Task<string> GetFullPathAsync(this EnvDTE.Project envDTEProject)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Assert(envDTEProject != null);
            if (IsUnloaded(envDTEProject))
            {
                // To get the directory of an unloaded project, we use the UniqueName property,
                // which is the path of the project file relative to the solution directory.
                var solutionDirectory = Path.GetDirectoryName(envDTEProject.DTE.Solution.FullName);
                var projectFileFullPath = Path.Combine(solutionDirectory, envDTEProject.UniqueName);
                return Path.GetDirectoryName(projectFileFullPath);
            }

            // Attempt to determine the project path using the available EnvDTE.Project properties.
            // Project systems using async load such as CPS may not have all properties populated 
            // for start up scenarios such as VS Templates. In these cases we need to fallback 
            // until we can find one containing the full path.

            // For website projects, always read FullPath from properties list
            if (await IsWebProjectAsync(envDTEProject))
            {
                // FullPath
                var fullProjectPath = GetPropertyValue<string>(envDTEProject, FullPath);

                if (!string.IsNullOrEmpty(fullProjectPath))
                {
                    return fullProjectPath;
                }
            }

            // FullName
            if (!string.IsNullOrEmpty(envDTEProject.FullName))
            {
                return Path.GetDirectoryName(envDTEProject.FullName);
            }

            // C++ projects do not have FullPath property, but do have ProjectDirectory one.
            var projectDirectory = GetPropertyValue<string>(envDTEProject, ProjectDirectory);

            if (!string.IsNullOrEmpty(projectDirectory))
            {
                return projectDirectory;
            }

            // FullPath
            var fullPath = GetPropertyValue<string>(envDTEProject, FullPath);

            if (!string.IsNullOrEmpty(fullPath))
            {
                // Some Project System implementations (JS Windows Store app) return the project
                // file as FullPath. We only need the parent directory
                return Path.GetDirectoryName(fullPath);
            }

            Debug.Fail("Unable to find the project path");

            return null;
        }

        public static bool IsUnloaded(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return VsProjectTypes.UnloadedProjectTypeGuid.Equals(envDTEProject.Kind, StringComparison.OrdinalIgnoreCase);
        }

        private static T GetPropertyValue<T>(this EnvDTE.Project envDTEProject, string propertyName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Debug.Assert(envDTEProject != null);
            if (envDTEProject.Properties == null)
            {
                // this happens in unit tests
                return default;
            }

            try
            {
                var property = envDTEProject.Properties.Item(propertyName);
                if (property != null)
                {
                    return (T)property.Value;
                }
            }
            catch (ArgumentException)
            {
            }
            catch (InvalidCastException)
            {
            }
            return default;
        }

        /// <summary>
        /// Returns the full path of the packages config file associated with the project.
        /// </summary>
        /// <param name="envDTEProject">The project.</param>
        public static async Task<string> GetPackagesConfigFullPathAsync(this EnvDTE.Project envDTEProject)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Assert(envDTEProject != null);
            var projectDirectory = await GetFullPathAsync(envDTEProject);

            var packagesConfigFullPath = Path.Combine(
                projectDirectory ?? string.Empty,
                ProjectManagement.Constants.PackageReferenceFile);

            return packagesConfigFullPath;
        }

        public static string GetName(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string name = envDTEProject.Name;
            if (IsJavaScriptProject(envDTEProject))
            {
                // The JavaScript project initially returns a "(loading..)" suffix to the project Name.
                // Need to get rid of it for the rest of NuGet to work properly.
                // TODO: Follow up with the VS team to see if this will be fixed eventually
                const string suffix = " (loading...)";
                if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    name = name.Substring(0, name.Length - suffix.Length);
                }
            }
            return name;
        }

        public static string GetUniqueName(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (IsWixProject(envDTEProject))
            {
                // Wix project doesn't offer UniqueName property
                return envDTEProject.FullName;
            }

            try
            {
                return envDTEProject.UniqueName;
            }
            catch (COMException)
            {
                return envDTEProject.FullName;
            }
        }

        /// <summary>
        /// Returns the unique name of the specified project including all solution folder names containing it.
        /// </summary>
        /// <remarks>
        /// This is different from the DTE Project.UniqueName property, which is the absolute path to the project file.
        /// </remarks>
        public static async Task<string> GetCustomUniqueNameAsync(this EnvDTE.Project envDTEProject)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (IsWebSite(envDTEProject))
            {
                // website projects always have unique name
                return envDTEProject.Name;
            }
            var nameParts = new Stack<string>();

            var cursor = envDTEProject;
            nameParts.Push(GetName(cursor));

            // walk up till the solution root
            while (cursor.ParentProjectItem != null
                   && cursor.ParentProjectItem.ContainingProject != null)
            {
                cursor = cursor.ParentProjectItem.ContainingProject;
                nameParts.Push(GetName(cursor));
            }

            return string.Join("\\", nameParts);
        }

        /// <summary>
        /// Determine the project framework string based on the project properties.
        /// </summary>
        public static string GetTargetFrameworkString(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (envDTEProject == null)
            {
                return null;
            }

            var projectPath = GetFullProjectPath(envDTEProject);
            var platformIdentifier = GetPropertyValue<string>(envDTEProject, ProjectBuildProperties.TargetPlatformIdentifier);
            var platformVersion = GetPropertyValue<string>(envDTEProject, ProjectBuildProperties.TargetPlatformVersion);
            var platformMinVersion = GetPropertyValue<string>(envDTEProject, ProjectBuildProperties.TargetPlatformMinVersion);
            var targetFrameworkMoniker = GetPropertyValue<string>(envDTEProject, ProjectBuildProperties.TargetFrameworkMoniker);
            var isManagementPackProject = IsManagementPackProject(envDTEProject);
            var isXnaWindowsPhoneProject = IsXnaWindowsPhoneProject(envDTEProject);

            // Projects supporting TargetFramework and TargetFrameworks are detected before
            // this check. The values can be passed as null here.
            var frameworkStrings = MSBuildProjectFrameworkUtility.GetProjectFrameworkStrings(
                projectFilePath: projectPath,
                targetFrameworks: null,
                targetFramework: null,
                targetFrameworkMoniker: targetFrameworkMoniker,
                targetPlatformIdentifier: platformIdentifier,
                targetPlatformVersion: platformVersion,
                targetPlatformMinVersion: platformMinVersion,
                isManagementPackProject: isManagementPackProject,
                isXnaWindowsPhoneProject: isXnaWindowsPhoneProject);

            return frameworkStrings.FirstOrDefault();
        }

        // TODO: Return null for library projects
        public static async Task<string> GetConfigurationFileAsync(this EnvDTE.Project envDTEProject)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return await IsWebProjectAsync(envDTEProject) ? WebConfig : AppConfig;
        }

        private class PathComparer : IEqualityComparer<string>
        {
            public static readonly PathComparer Default = new PathComparer();

            public bool Equals(string x, string y)
            {
                return Path.GetFileName(x).Equals(Path.GetFileName(y), StringComparison.OrdinalIgnoreCase);
            }

            [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Existing behavior.")]
            public int GetHashCode(string obj)
            {
                return Path.GetFileName(obj).ToLowerInvariant().GetHashCode();
            }
        }

        /// <summary>
        /// A DTE specific helper method that validates a path to ensure that it 
        /// could be for a file as opposed to a URL or other invalid path, and
        /// not for a directory. This is used to help determine if a value returned
        /// from DTE is a directory or file, since the file may still be in 
        /// memory and not yet written to disk File.Exists will not work.
        /// </summary>
        private static string GetPotentialFullPathOrNull(string path)
        {
            string fullPath = null;

            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    // Attempt to get the full path
                    fullPath = Path.GetFullPath(path);

                    // Some project systems may return a directory for the file path. 
                    // Directories usually exist even when the in-memory files have not yet 
                    // been written, so we can try to detect obvious non-files here.
                    // WebSites and Win JS projects can return a directory instead of the project file path.
                    if (Directory.Exists(fullPath))
                    {
                        // Ignore directories
                        fullPath = null;
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentException
                || ex is NotSupportedException
                || ex is PathTooLongException
                || ex is SecurityException)
            {
                // Ignore invalid paths
                // This can occur if the path was a URL
            }

            return fullPath;
        }

        #endregion // Get Project Information

        #region Check Project Types

        public static bool IsJavaScriptProject(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return envDTEProject != null && VsProjectTypes.JsProjectTypeGuid.Equals(envDTEProject.Kind, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsManagementPackProject(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return envDTEProject != null && VsProjectTypes.ManagementPackProjectTypeGuid.Equals(envDTEProject.Kind, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsXnaWindowsPhoneProject(this EnvDTE.Project envDTEProject)
        {
            // XNA projects will have this property set
            const string xnaPropertyValue = "Microsoft.Xna.GameStudio.CodeProject.WindowsPhoneProjectPropertiesExtender.XnaRefreshLevel";
            return envDTEProject != null &&
                   "Windows Phone OS 7.1".Equals(GetPropertyValue<string>(envDTEProject, xnaPropertyValue), StringComparison.OrdinalIgnoreCase);
        }

        private static async Task<bool> IsWebProjectAsync(this EnvDTE.Project envDTEProject)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            string[] types = await envDTEProject.GetProjectTypeGuidsAsync();
            return types.Contains(VsProjectTypes.WebSiteProjectTypeGuid, StringComparer.OrdinalIgnoreCase) ||
                   types.Contains(VsProjectTypes.WebApplicationProjectTypeGuid, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsWebSite(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return envDTEProject.Kind != null && envDTEProject.Kind.Equals(VsProjectTypes.WebSiteProjectTypeGuid, StringComparison.OrdinalIgnoreCase);
        }

        public static async Task<bool> IsWindowsStoreAppAsync(this EnvDTE.Project envDTEProject)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            string[] types = await envDTEProject.GetProjectTypeGuidsAsync();
            return types.Contains(VsProjectTypes.WindowsStoreProjectTypeGuid, StringComparer.OrdinalIgnoreCase);
        }

        private static bool IsWixProject(this EnvDTE.Project envDTEProject)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return envDTEProject.Kind != null && envDTEProject.Kind.Equals(VsProjectTypes.WixProjectTypeGuid, StringComparison.OrdinalIgnoreCase);
        }

        #endregion // Check Project Types
    }
}
