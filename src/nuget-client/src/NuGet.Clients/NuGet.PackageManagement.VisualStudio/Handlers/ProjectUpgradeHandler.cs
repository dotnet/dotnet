// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class ProjectUpgradeHandler : IVsSolutionEvents, IVsSolutionEventsProjectUpgrade, IDisposable
    {
        private uint _cookie;
        private IVsSolution2 _vsSolution2;
        private ISolutionManager _solutionManager;

        /// <summary>
        /// Constructs and Registers ("Advises") for Project retargeting events if the IVsSolutionEvents service is available
        /// Otherwise, it simply exits
        /// </summary>
        public ProjectUpgradeHandler(ISolutionManager solutionManager, IVsSolution2 vsSolution2)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (solutionManager == null)
            {
                throw new ArgumentNullException(nameof(solutionManager));
            }

            if (vsSolution2 != null)
            {
                _vsSolution2 = vsSolution2;
                _solutionManager = solutionManager;
                if (vsSolution2.AdviseSolutionEvents(this, out _cookie) == VSConstants.S_OK)
                {
                    Debug.Assert(_cookie != 0);
                }
                else
                {
                    _cookie = 0;
                }
            }
        }

        #region IVsSolutionEventsProjectUpgrade
        int IVsSolutionEventsProjectUpgrade.OnAfterUpgradeProject(IVsHierarchy pHierarchy, uint fUpgradeFlag, string bstrCopyLocation, SYSTEMTIME stUpgradeTime, IVsUpgradeLogger pLogger)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                Debug.Assert(pHierarchy != null);

                var upgradedProject = VsHierarchyUtility.GetProjectFromHierarchy(pHierarchy);
                var upgradedNuGetProject = await EnvDTEProjectUtility.GetNuGetProjectAsync(upgradedProject, _solutionManager);

                if (ProjectRetargetingUtility.IsProjectRetargetable(upgradedNuGetProject))
                {
                    var packagesToBeReinstalled = await ProjectRetargetingUtility.GetPackagesToBeReinstalled(upgradedNuGetProject);

                    if (packagesToBeReinstalled.Any())
                    {
                        pLogger.LogMessage((int)__VSUL_ERRORLEVEL.VSUL_ERROR, upgradedProject.Name, upgradedProject.Name,
                            string.Format(CultureInfo.CurrentCulture, Strings.ProjectUpgradeAndRetargetErrorMessage, string.Join(", ", packagesToBeReinstalled.Select(p => p.Id))));
                    }
                }
            });

            return VSConstants.S_OK;
        }
        #endregion

        #region IVsSolutionEvents (mandatory but unused implementation)
        int IVsSolutionEvents.OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }
        #endregion


        public void Dispose()
        {
            if (_cookie != 0 && _vsSolution2 != null)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    _vsSolution2.UnadviseSolutionEvents(_cookie);
                    _cookie = VSConstants.VSCOOKIE_NIL;
                }).PostOnFailure(nameof(ProjectUpgradeHandler));
            }
        }
    }
}
