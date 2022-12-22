// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class ProjectRetargetingHandler : IVsTrackProjectRetargetingEvents, IVsTrackBatchRetargetingEvents, IDisposable
    {
        private uint _cookieProjectRetargeting;
        private uint _cookieBatchRetargeting;
        private DTE _dte;
        private ISolutionManager _solutionManager;
        private IVsTrackProjectRetargeting _vsTrackProjectRetargeting;
        private readonly ErrorListProvider _errorListProvider;
        private IVsMonitorSelection _vsMonitorSelection;
        private string _platformRetargetingProject;

        private Lazy<ISolutionRestoreWorker> _solutionRestoreWorker;

        private const string NETCore45 = ".NETCore,Version=v4.5";
        private const string Windows80 = "Windows, Version=8.0";
        private const string NETCore451 = ".NETCore,Version=v4.5.1";
        private const string Windows81 = "Windows, Version=8.1";

        /// <summary>
        /// Constructs and Registers ("Advises") for Project retargeting events if the IVsTrackProjectRetargeting service is available
        /// Otherwise, it simply exits
        /// </summary>
        /// <param name="dte"></param>
        public ProjectRetargetingHandler(DTE dte, ISolutionManager solutionManager, IServiceProvider serviceProvider, IComponentModel componentModel, IVsTrackProjectRetargeting vsTrackProjectRetargeting, IVsMonitorSelection vsMonitorSelection)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (dte == null)
            {
                throw new ArgumentNullException(nameof(dte));
            }

            if (solutionManager == null)
            {
                throw new ArgumentNullException(nameof(solutionManager));
            }

            if (componentModel == null)
            {
                throw new ArgumentNullException(nameof(componentModel));
            }

            _vsMonitorSelection = vsMonitorSelection;
            Assumes.Present(_vsMonitorSelection);

            _solutionRestoreWorker = new Lazy<ISolutionRestoreWorker>(
                () => componentModel.GetService<ISolutionRestoreWorker>());

            if (vsTrackProjectRetargeting != null)
            {
                _errorListProvider = new ErrorListProvider(serviceProvider);
                _dte = dte;
                _solutionManager = solutionManager;
                _vsTrackProjectRetargeting = vsTrackProjectRetargeting;

                // Register for ProjectRetargetingEvents
                if (_vsTrackProjectRetargeting.AdviseTrackProjectRetargetingEvents(this, out _cookieProjectRetargeting) == VSConstants.S_OK)
                {
                    Debug.Assert(_cookieProjectRetargeting != 0);
                    _dte.Events.BuildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
                    _dte.Events.SolutionEvents.AfterClosing += SolutionEvents_AfterClosing;
                }
                else
                {
                    _cookieProjectRetargeting = 0;
                }

                // Register for BatchRetargetingEvents. Using BatchRetargetingEvents, we need to detect platform retargeting
                if (_vsTrackProjectRetargeting.AdviseTrackBatchRetargetingEvents(this, out _cookieBatchRetargeting) == VSConstants.S_OK)
                {
                    Debug.Assert(_cookieBatchRetargeting != 0);
                    if (_cookieProjectRetargeting == 0)
                    {
                        // Register for dte Events only if they are not already registered for
                        _dte.Events.BuildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
                        _dte.Events.SolutionEvents.AfterClosing += SolutionEvents_AfterClosing;
                    }
                }
                else
                {
                    _cookieBatchRetargeting = 0;
                }
            }
        }

        private void SolutionEvents_AfterClosing()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _errorListProvider.Tasks.Clear();
            });
        }

        private void BuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            // Clear the error list upon the first build action
            // Note that the retargeting error message is shown on the errorlistprovider this class creates
            // Hence, explicit clearing of the error list is required
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _errorListProvider.Tasks.Clear();

                if (Action != vsBuildAction.vsBuildActionClean)
                {
                    await ShowWarningsForPackageReinstallationAsync(_dte.Solution);
                }
            });
        }

        private async System.Threading.Tasks.Task ShowWarningsForPackageReinstallationAsync(Solution solution)
        {
            Debug.Assert(solution != null);

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            foreach (Project project in solution.Projects)
            {
                var nuGetProject = await EnvDTEProjectUtility.GetNuGetProjectAsync(project, _solutionManager);
                if (ProjectRetargetingUtility.IsProjectRetargetable(nuGetProject))
                {
                    var packageReferencesToBeReinstalled = ProjectRetargetingUtility.GetPackageReferencesMarkedForReinstallation(nuGetProject);
                    if (packageReferencesToBeReinstalled.Count > 0)
                    {
                        Debug.Assert(await ProjectRetargetingUtility.IsNuGetInUseAsync(project));
                        var projectHierarchy = await project.ToVsHierarchyAsync();
                        ShowRetargetingErrorTask(packageReferencesToBeReinstalled.Select(p => p.PackageIdentity.Id), projectHierarchy, TaskErrorCategory.Warning, TaskPriority.Normal);
                    }
                }
            }
        }

        private void ShowRetargetingErrorTask(IEnumerable<string> packagesToBeReinstalled, IVsHierarchy projectHierarchy, TaskErrorCategory errorCategory, TaskPriority priority)
        {
            Debug.Assert(packagesToBeReinstalled != null && packagesToBeReinstalled.Any());

            var errorText = string.Format(CultureInfo.CurrentCulture, Strings.ProjectUpgradeAndRetargetErrorMessage,
                    string.Join(", ", packagesToBeReinstalled));
            MessageHelper.ShowError(_errorListProvider, errorCategory, priority, errorText, projectHierarchy);
        }

        #region IVsTrackProjectRetargetingEvents
        int IVsTrackProjectRetargetingEvents.OnRetargetingAfterChange(string projRef, IVsHierarchy pAfterChangeHier, string fromTargetFramework, string toTargetFramework)
        {
            NuGetProject retargetedProject = null;
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _errorListProvider.Tasks.Clear();
                var project = VsHierarchyUtility.GetProjectFromHierarchy(pAfterChangeHier);
                retargetedProject = await EnvDTEProjectUtility.GetNuGetProjectAsync(project, _solutionManager);

                if (ProjectRetargetingUtility.IsProjectRetargetable(retargetedProject))
                {
                    var packagesToBeReinstalled = await ProjectRetargetingUtility.GetPackagesToBeReinstalled(retargetedProject);
                    if (packagesToBeReinstalled.Any())
                    {
                        ShowRetargetingErrorTask(packagesToBeReinstalled.Select(p => p.Id), pAfterChangeHier, TaskErrorCategory.Error, TaskPriority.High);
                    }
                    // NuGet/Home#4833 Baseline
                    // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014
                    ProjectRetargetingUtility.MarkPackagesForReinstallation(retargetedProject, packagesToBeReinstalled);
#pragma warning restore CS4014
                }
            });

            if (retargetedProject is LegacyPackageReferenceProject)
            {
                // trigger solution restore and don't wait for it to be complete and hold the UI thread
                System.Threading.Tasks.Task.Run(() => _solutionRestoreWorker.Value.ScheduleRestoreAsync(SolutionRestoreRequest.ByUserCommand(ExplicitRestoreReason.ProjectRetargeting), CancellationToken.None));
            }
            return VSConstants.S_OK;
        }

        int IVsTrackProjectRetargetingEvents.OnRetargetingBeforeChange(string projRef, IVsHierarchy pBeforeChangeHier, string currentTargetFramework, string newTargetFramework, out bool pCanceled, out string ppReasonMsg)
        {
            pCanceled = false;
            ppReasonMsg = null;
            return VSConstants.S_OK;
        }

        int IVsTrackProjectRetargetingEvents.OnRetargetingBeforeProjectSave(string projRef, IVsHierarchy pBeforeChangeHier, string currentTargetFramework, string newTargetFramework)
        {
            return VSConstants.S_OK;
        }

        int IVsTrackProjectRetargetingEvents.OnRetargetingCanceledChange(string projRef, IVsHierarchy pBeforeChangeHier, string currentTargetFramework, string newTargetFramework)
        {
            return VSConstants.S_OK;
        }

        int IVsTrackProjectRetargetingEvents.OnRetargetingFailure(string projRef, IVsHierarchy pHier, string fromTargetFramework, string toTargetFramework)
        {
            return VSConstants.S_OK;
        }
        #endregion

        #region IVsTrackBatchRetargetingEvents
        int IVsTrackBatchRetargetingEvents.OnBatchRetargetingBegin()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var project = _vsMonitorSelection.GetActiveProject();

                if (project != null)
                {
                    _platformRetargetingProject = null;
                    var frameworkName = project.GetTargetFrameworkString();
                    if (NETCore45.Equals(frameworkName, StringComparison.OrdinalIgnoreCase) || Windows80.Equals(frameworkName, StringComparison.OrdinalIgnoreCase))
                    {
                        _platformRetargetingProject = project.UniqueName;
                    }
                }
            });

            return VSConstants.S_OK;
        }


        int IVsTrackBatchRetargetingEvents.OnBatchRetargetingEnd()
        {
            NuGetProject nuGetProject = null;
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _errorListProvider.Tasks.Clear();
                if (_platformRetargetingProject != null)
                {
                    try
                    {
                        var project = _dte.Solution.Item(_platformRetargetingProject);

                        if (project != null)
                        {
                            nuGetProject = await EnvDTEProjectUtility.GetNuGetProjectAsync(project, _solutionManager);

                            if (ProjectRetargetingUtility.IsProjectRetargetable(nuGetProject))
                            {
                                var frameworkName = project.GetTargetFrameworkString();
                                if (NETCore451.Equals(frameworkName, StringComparison.OrdinalIgnoreCase) || Windows81.Equals(frameworkName, StringComparison.OrdinalIgnoreCase))
                                {
                                    var packagesToBeReinstalled = await ProjectRetargetingUtility.GetPackagesToBeReinstalled(nuGetProject);
                                    if (packagesToBeReinstalled.Count > 0)
                                    {
                                        // By asserting that NuGet is in use, we are also asserting that NuGet.VisualStudio.dll is already loaded
                                        // Hence, it is okay to call project.ToVsHierarchyAsync()
                                        Debug.Assert(await ProjectRetargetingUtility.IsNuGetInUseAsync(project));
                                        var projectHierarchy = await project.ToVsHierarchyAsync();
                                        ShowRetargetingErrorTask(packagesToBeReinstalled.Select(p => p.Id), projectHierarchy, TaskErrorCategory.Error, TaskPriority.High);
                                    }
                                    // NuGet/Home#4833 Baseline
                                    // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014
                                    ProjectRetargetingUtility.MarkPackagesForReinstallation(nuGetProject, packagesToBeReinstalled);
#pragma warning restore CS4014
                                }
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                        // If the solution does not contain a project named '_platformRetargetingProject', it will throw ArgumentException
                    }
                    _platformRetargetingProject = null;
                }
            });

            if (nuGetProject is LegacyPackageReferenceProject)
            {
                // trigger solution restore and don't wait for it to be complete and hold the UI thread
                System.Threading.Tasks.Task.Run(() => _solutionRestoreWorker.Value.ScheduleRestoreAsync(SolutionRestoreRequest.ByUserCommand(ExplicitRestoreReason.ProjectRetargeting), CancellationToken.None));
            }

            return VSConstants.S_OK;
        }
        #endregion

        public void Dispose()
        {
            // Nothing is initialized if _vsTrackProjectRetargeting is null. Check if it is not null
            if (_vsTrackProjectRetargeting != null)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    _errorListProvider.Dispose();
                    if (_cookieProjectRetargeting != 0)
                    {
                        _vsTrackProjectRetargeting.UnadviseTrackProjectRetargetingEvents(_cookieProjectRetargeting);
                    }

                    if (_cookieBatchRetargeting != 0)
                    {
                        _vsTrackProjectRetargeting.UnadviseTrackBatchRetargetingEvents(_cookieBatchRetargeting);
                    }

                    if (_cookieProjectRetargeting != 0 || _cookieBatchRetargeting != 0)
                    {
                        _dte.Events.BuildEvents.OnBuildBegin -= BuildEvents_OnBuildBegin;
                        _dte.Events.SolutionEvents.AfterClosing -= SolutionEvents_AfterClosing;
                    }
                }).PostOnFailure(nameof(ProjectRetargetingHandler));
            }
        }
    }
}
