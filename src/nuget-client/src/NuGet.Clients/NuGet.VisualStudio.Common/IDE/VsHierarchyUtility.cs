// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using TaskExpandedNodes = System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<NuGet.VisualStudio.VsHierarchyItem>>>;

namespace NuGet.VisualStudio
{
    public static class VsHierarchyUtility
    {
        private const string VsWindowKindSolutionExplorer = "3AE79031-E1BC-11D0-8F78-00A0C9110057";

        public static string GetProjectPath(IVsHierarchy project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var format = (IPersistFileFormat)project;
            format.GetCurFile(out string projectPath, out uint _);
            return projectPath;
        }

        public static bool IsSupportedByGuid(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ErrorHandler.Succeeded(hierarchy.GetGuidProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_TypeGuid, out Guid pguid)))
            {
                var guid = pguid.ToString("B", provider: null);
                if (ProjectType.IsUnsupported(guid) || !ProjectType.IsSupported(guid))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static bool IsNuGetSupported(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (IsProjectCapabilityCompliant(hierarchy))
            {
                return true;
            }
            return IsSupportedByGuid(hierarchy) && !HasUnsupportedProjectCapability(hierarchy);
        }

        /// <summary>Check if this project appears to support NuGet.</summary>
        /// <param name="hierarchy">IVsHierarchy representing the project in the solution.</param>
        /// <returns>True if NuGet should enable this project, false if NuGet should ignore the project.</returns>
        /// <remarks>The project may be packages.config or PackageReference. This method does not tell you which.</remarks>
        public static bool IsProjectCapabilityCompliant(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // NOTE: (AssemblyReferences + DeclaredSourceItems + UserSourceItems) exists solely for compatibility reasons
            // with existing custom CPS-based projects that existed before "PackageReferences" capability was introduced.
            return hierarchy.IsCapabilityMatch(IDE.ProjectCapabilities.SupportsNuGet);
        }

        public static bool HasUnsupportedProjectCapability(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // This is true for shared projects in universal apps
            return hierarchy.IsCapabilityMatch(ProjectCapabilities.SharedAssetsProject);
        }

        /// <summary>
        /// Gets the project type guids from the hierarchy.
        /// </summary>
        /// <param name="hierarchy">hierarchy for a project</param>
        /// <returns>The project type guids of all the types in the hierarchy.</returns>
        public static string[] GetProjectTypeGuidsFromHierarchy(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var aggregatableProject = hierarchy as IVsAggregatableProject;
            if (aggregatableProject != null)
            {
                string projectTypeGuids;
                var hr = aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);
                ErrorHandler.ThrowOnFailure(hr);

                return projectTypeGuids.Split(';');
            }

            if (ErrorHandler.Succeeded(hierarchy.GetGuidProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_TypeGuid, out Guid pguid)))
            {
                return new string[] { pguid.ToString("B", provider: null) };
            }
            return Array.Empty<string>();
        }

        /// <summary>
        /// Check for CPS capability in IVsHierarchy.
        /// </summary>
        /// <remarks>This does not mean the project also supports PackageReference!</remarks>
        public static bool IsCPSCapabilityCompliant(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return hierarchy.IsCapabilityMatch(IDE.ProjectCapabilities.Cps);
        }

        /// <summary>
        /// Gets the EnvDTE.Project instance from IVsHierarchy
        /// </summary>
        /// <param name="pHierarchy">pHierarchy is the IVsHierarchy instance from which the project instance is obtained</param>
        public static EnvDTE.Project GetProjectFromHierarchy(IVsHierarchy pHierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Set it to null to avoid unassigned local variable warning
            EnvDTE.Project project = null;
            object projectObject;

            if (pHierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out projectObject) >= 0)
            {
                project = (EnvDTE.Project)projectObject;
            }

            return project;
        }

        public static async TaskExpandedNodes GetAllExpandedNodesAsync()
        {
            // this operation needs to execute on UI thread
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = await ServiceLocator.GetGlobalServiceAsync<SDTE, DTE>();
            var projects = dte.Solution.Projects;

            var results = new Dictionary<string, ISet<VsHierarchyItem>>(StringComparer.OrdinalIgnoreCase);
            foreach (var project in projects.Cast<Project>())
            {
                var expandedNodes =
                    await GetExpandedProjectHierarchyItemsAsync(project);
                Debug.Assert(!results.ContainsKey(project.GetUniqueName()));
                results[project.GetUniqueName()] =
                    new HashSet<VsHierarchyItem>(expandedNodes);
            }
            return results;
        }

        public static async Task CollapseAllNodesAsync(IDictionary<string, ISet<VsHierarchyItem>> ignoreNodes)
        {
            Common.Verify.ArgumentIsNotNull(ignoreNodes, nameof(ignoreNodes));

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = await ServiceLocator.GetGlobalServiceAsync<SDTE, DTE>();
            var projects = dte.Solution.Projects;

            foreach (var project in projects.Cast<Project>())
            {
                ISet<VsHierarchyItem> expandedNodes;
                if (ignoreNodes.TryGetValue(project.GetUniqueName(), out expandedNodes)
                    &&
                    expandedNodes != null)
                {
                    await CollapseProjectHierarchyItemsAsync(project, expandedNodes);
                }
            }
        }

        /// <summary>
        /// Gets all the hierarchies of all the NuGet compatible projects
        /// </summary>
        /// <param name="vsSolution">The VS Solution instance. Must not be <see cref="null"/>. </param>
        /// <returns>Hierarchies of the NuGet compatible projects</returns>
        /// <remarks>This method assumes the solution is open.</remarks>
        public static List<IVsHierarchy> GetAllLoadedProjects(IVsSolution vsSolution)
        {
            Assumes.NotNull(vsSolution);
            ThreadHelper.ThrowIfNotOnUIThread();

            var compatibleProjectHierarchies = new List<IVsHierarchy>();

            var hr = vsSolution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION, Guid.Empty, out IEnumHierarchies ppenum);
            ErrorHandler.ThrowOnFailure(hr);

            IVsHierarchy[] hierarchies = new IVsHierarchy[1];
            while ((ppenum.Next((uint)hierarchies.Length, hierarchies, out uint fetched) == VSConstants.S_OK) && (fetched == (uint)hierarchies.Length))
            {
                var hierarchy = hierarchies[0];
                if (IsNuGetSupported(hierarchy))
                {
                    compatibleProjectHierarchies.Add(hierarchy);
                }
            }

            return compatibleProjectHierarchies;
        }

        public static bool AreAnyLoadedProjectsNuGetCompatible(IVsSolution vsSolution)
        {
            Assumes.NotNull(vsSolution);
            ThreadHelper.ThrowIfNotOnUIThread();

            var hr = vsSolution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION, Guid.Empty, out IEnumHierarchies ppenum);
            ErrorHandler.ThrowOnFailure(hr);

            IVsHierarchy[] hierarchies = new IVsHierarchy[1];
            while ((ppenum.Next((uint)hierarchies.Length, hierarchies, out uint fetched) == VSConstants.S_OK) && (fetched == (uint)hierarchies.Length))
            {
                var hierarchy = hierarchies[0];
                if (IsNuGetSupported(hierarchy))
                {
                    return true;
                }
            }
            return false;
        }

        private static async Task<ICollection<VsHierarchyItem>> GetExpandedProjectHierarchyItemsAsync(EnvDTE.Project project)
        {
            var projectHierarchyItem = await VsHierarchyItem.FromDteProjectAsync(project);
            var solutionExplorerWindow = await GetSolutionExplorerHierarchyWindowAsync();

            if (solutionExplorerWindow == null)
            {
                // If the solution explorer is collapsed since opening VS, this value is null. In such a case, simply exit early.
                return Array.Empty<VsHierarchyItem>();
            }

            var expandedItems = new List<VsHierarchyItem>();

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // processCallback return values: 
            //     0   continue, 
            //     1   don't recurse into, 
            //    -1   stop
            await projectHierarchyItem.WalkDepthFirstAsync(
                fVisible: true,
                processCallbackAsync:
                    async (VsHierarchyItem vsItem, object callerObject) =>
                    {
                        if (await IsVsHierarchyItemExpandedAsync(vsItem, solutionExplorerWindow))
                        {
                            expandedItems.Add(vsItem);
                        }
                        return (0, null);
                    },
                callerObject: null);

            return expandedItems;
        }

        private static async Task CollapseProjectHierarchyItemsAsync(EnvDTE.Project project, ISet<VsHierarchyItem> ignoredHierarcyItems)
        {
            var projectHierarchyItem = await VsHierarchyItem.FromDteProjectAsync(project);
            var solutionExplorerWindow = await GetSolutionExplorerHierarchyWindowAsync();

            if (solutionExplorerWindow == null)
            {
                // If the solution explorer is collapsed since opening VS, this value is null. In such a case, simply exit early.
                return;
            }

            // processCallback return values:
            //     0   continue, 
            //     1   don't recurse into, 
            //    -1   stop
            await projectHierarchyItem.WalkDepthFirstAsync(
                fVisible: true,
                processCallbackAsync:
                    async (VsHierarchyItem currentHierarchyItem, object callerObject) =>
                    {
                        if (!ignoredHierarcyItems.Contains(currentHierarchyItem))
                        {
                            await CollapseVsHierarchyItemAsync(currentHierarchyItem, solutionExplorerWindow);
                        }
                        return (0, null);
                    },
                callerObject: null);
        }

        private static async Task CollapseVsHierarchyItemAsync(VsHierarchyItem vsHierarchyItem, IVsUIHierarchyWindow vsHierarchyWindow)
        {
            if (vsHierarchyItem == null
                || vsHierarchyWindow == null)
            {
                return;
            }

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            vsHierarchyWindow.ExpandItem(AsVsUIHierarchy(vsHierarchyItem), vsHierarchyItem.VsItemID, EXPANDFLAGS.EXPF_CollapseFolder);
        }

        private static async Task<bool> IsVsHierarchyItemExpandedAsync(VsHierarchyItem hierarchyItem, IVsUIHierarchyWindow uiWindow)
        {
            if (!await hierarchyItem.IsExpandableAsync())
            {
                return false;
            }

            const uint expandedStateMask = (uint)__VSHIERARCHYITEMSTATE.HIS_Expanded;
            uint itemState;

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            uiWindow.GetItemState(AsVsUIHierarchy(hierarchyItem), hierarchyItem.VsItemID, expandedStateMask, out itemState);
            return ((__VSHIERARCHYITEMSTATE)itemState == __VSHIERARCHYITEMSTATE.HIS_Expanded);
        }

        private static IVsUIHierarchy AsVsUIHierarchy(VsHierarchyItem hierarchyItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return hierarchyItem.VsHierarchy as IVsUIHierarchy;
        }

        private static async Task<IVsUIHierarchyWindow> GetSolutionExplorerHierarchyWindowAsync()
        {
            var serviceProvider = await ServiceLocator.GetServiceProviderAsync();
            return VsShellUtilities.GetUIHierarchyWindow(
                serviceProvider,
                new Guid(VsWindowKindSolutionExplorer));
        }
    }
}
