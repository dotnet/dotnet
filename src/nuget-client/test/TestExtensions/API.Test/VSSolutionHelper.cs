// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace API.Test
{
    public static class VSSolutionHelper
    {
        private static UpdateSolutionEventHandler _solutionEventHandler = new UpdateSolutionEventHandler();

        private static uint _updateSolutionEventsCookie;

        private static IVsSolutionBuildManager _solutionBuildManager;

        internal static async Task<EnvDTE80.Solution2> GetDTESolutionAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = ServiceLocator.GetDTE();
            var dte2 = (EnvDTE80.DTE2)dte;
            var solution2 = dte2.Solution as EnvDTE80.Solution2;

            return solution2;
        }

        public static void WaitForSolutionLoad()
        {
            ThreadHelper.ThrowIfOnUIThread();

            using (var mre = new ManualResetEvent(false))
            {
                KnownUIContexts.SolutionExistsAndFullyLoadedContext.WhenActivated(() => mre.Set());
                mre.WaitOne();
            }
        }

        public static string GetSolutionFullName()
        {
            return ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                var solutionFullName = await GetSolutionFullNameAsync();
                return solutionFullName;
            });
        }

        private static async Task<string> GetSolutionFullNameAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await GetDTESolutionAsync();
            var solutionFullName = solution2.FullName;
            return solutionFullName;
        }

        public static void CreateSolution(string solutionDirectory, string name)
        {
            Utils.ThrowStringArgException(solutionDirectory, nameof(solutionDirectory));
            Utils.ThrowStringArgException(name, nameof(name));

            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await CreateSolutionAsync(solutionDirectory, name);
            });
        }

        private static async Task CreateSolutionAsync(string solutionDirectory, string name)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await GetDTESolutionAsync();
            solution2.Create(solutionDirectory, name);

            var solutionPath = Path.Combine(solutionDirectory, name);
            solution2.SaveAs(solutionPath);
        }

        private static string SolutionNameFormat = "Solution_{0}";
        public static void CreateNewSolution(string outputPath, string solutionName)
        {
            Utils.ThrowStringArgException(outputPath, nameof(outputPath));
            Utils.ThrowStringArgException(solutionName, nameof(solutionName));

            var solutionDir = Path.Combine(outputPath, solutionName);

            Directory.CreateDirectory(solutionDir);

            CreateSolution(solutionDir, solutionName);
        }

        public static void CreateNewSolution(string outputPath)
        {
            Utils.ThrowStringArgException(outputPath, nameof(outputPath));

            var id = Utils.GetNewGUID();
            var solutionName = string.Format(SolutionNameFormat, id);

            CreateNewSolution(outputPath, solutionName);
        }

        public static void EnsureSolution(string outputPath)
        {
            Utils.ThrowStringArgException(outputPath, nameof(outputPath));

            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await EnsureSolutionAsync(outputPath);
            });
        }

        public static async Task EnsureSolutionAsync(string outputPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var isSolutionAvailable = await IsSolutionAvailableAsync();
            if (!isSolutionAvailable)
            {
                CreateNewSolution(outputPath);
            }
        }

        public static bool IsSolutionAvailable()
        {
            return ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                var isSolutionAvailable = await IsSolutionAvailableAsync();
                return isSolutionAvailable;
            });
        }

        public static async Task<bool> IsSolutionAvailableAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await GetDTESolutionAsync();
            return await IsSolutionAvailableAsync(solution2);
        }

        public static async Task<bool> IsSolutionAvailableAsync(EnvDTE80.Solution2 solution2)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            return solution2 != null && solution2.IsOpen;
        }

        public static void CloseSolution()
        {
            ThreadHelper.JoinableTaskFactory.Run(() => CloseSolutionAsync());
        }

        private static async Task CloseSolutionAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await GetDTESolutionAsync();
            var isSolutionAvailable = await IsSolutionAvailableAsync(solution2);
            if (isSolutionAvailable)
            {
                solution2.Close();
            }
        }

        public static void OpenSolution(string solutionFile)
        {
            Utils.ThrowStringArgException(solutionFile, nameof(solutionFile));

            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await OpenSolutionAsync(solutionFile);
            });
        }

        private static async Task OpenSolutionAsync(string solutionFile)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await GetDTESolutionAsync();
            solution2.Open(solutionFile);
        }

        public static void SaveAsSolution(string solutionFile)
        {
            Utils.ThrowStringArgException(solutionFile, nameof(solutionFile));

            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await SaveAsSolutionAsync(solutionFile);
            });
        }

        private static async Task SaveAsSolutionAsync(string solutionFile)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await GetDTESolutionAsync();
            solution2.SaveAs(solutionFile);
        }

        public static async Task<EnvDTE.SolutionBuild> GetSolutionBuildAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await VSSolutionHelper.GetDTESolutionAsync();
            var isSolutionAvailable = await VSSolutionHelper.IsSolutionAvailableAsync(solution2);

            if (!isSolutionAvailable)
            {
                throw new ArgumentException("Solution is not available");
            }

            return solution2.SolutionBuild;
        }

        public static void BuildSolution()
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await BuildSolutionAsync();
            });
        }

        private static async Task BuildSolutionAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solutionBuild = await GetSolutionBuildAsync();

            solutionBuild.Build(WaitForBuildToFinish: true);
        }

        public static void CleanSolution()
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await CleanSolutionAsync();
            });
        }

        private static async Task CleanSolutionAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solutionBuild = await GetSolutionBuildAsync();

            solutionBuild.Clean(WaitForCleanToFinish: true);
        }

        public static void RebuildSolution()
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await RebuildSolutionAsync();
            });
        }

        private static async Task RebuildSolutionAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            Assumes.Present(_solutionBuildManager);

            var buildFlags = (uint)(VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_BUILD | VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_CLEAN);

            _solutionBuildManager.StartSimpleUpdateSolutionConfiguration(buildFlags, (uint)VSSOLNBUILDQUERYRESULTS.VSSBQR_CONTDEPLOYONERROR_QUERY_NO, 1);
        }

        public static void AdviseSolutionEvents()
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await AdviseSolutionEventsAsync();
            });
        }

        private static async Task AdviseSolutionEventsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            _solutionBuildManager = ServiceLocator.GetService<SVsSolutionBuildManager, IVsSolutionBuildManager>();
            Assumes.Present(_solutionBuildManager);

            _solutionBuildManager.AdviseUpdateSolutionEvents(_solutionEventHandler, out _updateSolutionEventsCookie);
        }

        public static void UnadviseSolutionEvents()
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await UnadviseSolutionEventsAsync();
            });
        }

        private static async Task UnadviseSolutionEventsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (_updateSolutionEventsCookie != 0)
            {
                _solutionBuildManager?.UnadviseUpdateSolutionEvents(_updateSolutionEventsCookie);
                _updateSolutionEventsCookie = 0;
            }
        }

        public static void WaitUntilRebuildCompleted()
        {
            while (!_solutionEventHandler.isOperationCompleted)
            {
                Thread.Sleep(100);
            }
        }

        public static async Task<EnvDTE.Project> GetSolutionFolderProjectAsync(EnvDTE80.Solution2 solution2, string solutionFolderName)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solutionFolderParts = solutionFolderName.Split('\\');
            var solutionFolderProject = await GetSolutionFolderProjectAsync(solution2.Projects, solutionFolderParts, 0);

            if (solutionFolderProject == null)
            {
                throw new ArgumentException("No corresponding solution folder exists", nameof(solutionFolderName));
            }

            var solutionFolder = solutionFolderProject.Object as EnvDTE80.SolutionFolder;
            if (solutionFolder == null)
            {
                throw new ArgumentException("Not a valid solution folder", nameof(solutionFolderName));
            }

            return solutionFolderProject;
        }

        private static async Task<EnvDTE.Project> GetSolutionFolderProjectAsync(
            IEnumerable projectItems,
            string[] solutionFolderParts,
            int level)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (solutionFolderParts == null)
            {
                throw new ArgumentNullException(nameof(solutionFolderParts));
            }

            if (solutionFolderParts.Length == 0)
            {
                throw new ArgumentException("solution folder parts cannot be null", nameof(solutionFolderParts));
            }

            if (projectItems == null || level >= solutionFolderParts.Length)
            {
                return null;
            }

            var solutionFolderName = solutionFolderParts[level];
            EnvDTE.Project solutionFolderProject = null;

            foreach (var item in projectItems)
            {
                // Item could be a project or a projectItem
                var project = item as EnvDTE.Project;

                if (project == null)
                {
                    var projectItem = item as EnvDTE.ProjectItem;
                    if (projectItem != null)
                    {
                        project = projectItem.SubProject;
                    }
                }

                if (project != null)
                {
                    if (project.Name.StartsWith(solutionFolderName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (solutionFolderParts.Length == level + 1)
                        {
                            solutionFolderProject = project;
                            break;
                        }
                        else
                        {
                            solutionFolderProject
                                = await GetSolutionFolderProjectAsync(project.ProjectItems, solutionFolderParts, level + 1);
                        }
                    }
                }
            }

            return solutionFolderProject;
        }

        public static void NewSolutionFolder(string outputPath, string folderPath)
        {
            Utils.ThrowStringArgException(outputPath, nameof(outputPath));
            Utils.ThrowStringArgException(folderPath, nameof(folderPath));

            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NewSolutionFolderAsync(outputPath, folderPath);
            });
        }

        private static async Task NewSolutionFolderAsync(string outputPath, string folderPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await GetDTESolutionAsync();



            var newSolutionFolderIndex = folderPath.LastIndexOf('\\');

            if (newSolutionFolderIndex == -1)
            {
                // Create solution folder at solution level
                await EnsureSolutionAsync(outputPath);
                solution2.AddSolutionFolder(folderPath);
                return;
            }
            else
            {
                // Get solution folder project object for parent
                var parentName = folderPath.Substring(0, newSolutionFolderIndex);
                var solutionFolderName = folderPath.Substring(newSolutionFolderIndex + 1);

                var parentProject = await GetSolutionFolderProjectAsync(solution2, parentName);
                var parentSolutionFolder = (EnvDTE80.SolutionFolder)parentProject.Object;
                parentSolutionFolder.AddSolutionFolder(solutionFolderName);
            }
        }

        public static void RenameSolutionFolder(string folderPath, string newName)
        {
            Utils.ThrowStringArgException(folderPath, nameof(folderPath));
            Utils.ThrowStringArgException(newName, nameof(newName));

            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await RenameSolutionFolderAsync(folderPath, newName);
            });
        }

        private static async Task RenameSolutionFolderAsync(string folderPath, string newName)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution2 = await GetDTESolutionAsync();

            var solutionFolderProject = await GetSolutionFolderProjectAsync(solution2, folderPath);
            solutionFolderProject.Name = newName;
        }

        public static async Task<EnvDTE.Project> GetProjectAsync(EnvDTE80.Solution2 solution2, string projectName)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return solution2
                .Projects
                .Cast<EnvDTE.Project>()
                .FirstOrDefault(project =>
                    {
                        ThreadHelper.ThrowIfNotOnUIThread(); return project.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase);
                    });
        }

        public static async Task<EnvDTE.Project> GetProjectAsync(
            EnvDTE.Project solutionFolderProject,
            string projectName)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var projectItem = solutionFolderProject.ProjectItems
                .Cast<EnvDTE.ProjectItem>()
                .FirstOrDefault(project =>
                    {
                        ThreadHelper.ThrowIfNotOnUIThread(); return project.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase);
                    });

            return projectItem?.Object as EnvDTE.Project;
        }

        private static IEnumerable<IVsHierarchy> EnumerateLoadedProjects(IVsSolution vsSolution)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IEnumHierarchies enumHierarchies;
            var guid = Guid.Empty;
            ErrorHandler.ThrowOnFailure(vsSolution.GetProjectEnum(
                (uint)__VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION, ref guid, out enumHierarchies));

            // Loop all projects found
            if (enumHierarchies != null)
            {
                // Loop projects found
                var hierarchy = new IVsHierarchy[1];
                uint fetched = 0;
                while (enumHierarchies.Next(1, hierarchy, out fetched) == VSConstants.S_OK && fetched > 0)
                {
                    yield return hierarchy[0];
                }
            }
        }

        public static async Task<IVsHierarchy> FindProjectAsync(string fullName)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var vsSolution = ServiceLocator.GetService<SVsSolution, IVsSolution>();

            foreach (var project in EnumerateLoadedProjects(vsSolution))
            {
                ErrorHandler.ThrowOnFailure(project.GetCanonicalName(
                    VSConstants.VSITEMID_ROOT, out string projectPath));
                if (projectPath.Equals(fullName, StringComparison.OrdinalIgnoreCase))
                {
                    return project;
                }
            }

            return null;
        }

        public static async Task<IVsHierarchy> FindProjectByUniqueNameAsync(string uniqueName)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var vsSolution = ServiceLocator.GetService<SVsSolution, IVsSolution>();

            var hr = vsSolution.GetProjectOfUniqueName(uniqueName, out var project);
            if (ErrorHandler.Succeeded(hr))
            {
                return project;
            }

            return null;
        }
    }

    public sealed class UpdateSolutionEventHandler : IVsUpdateSolutionEvents
    {
        public bool isOperationCompleted;

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            isOperationCompleted = false;
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            isOperationCompleted = true;
            return VSConstants.S_OK;
        }

        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Cancel()
        {
            isOperationCompleted = true;
            return VSConstants.S_OK;
        }

        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }
    }
}
