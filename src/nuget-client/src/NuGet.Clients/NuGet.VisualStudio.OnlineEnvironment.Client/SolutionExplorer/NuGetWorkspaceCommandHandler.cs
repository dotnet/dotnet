// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Workspace.VSIntegration.UI;

namespace NuGet.VisualStudio.OnlineEnvironment.Client
{
    /// <summary>
    /// Extends the Solution Explorer in online environment scenarios.
    /// </summary>
    internal class NuGetWorkspaceCommandHandler : IWorkspaceCommandHandler
    {
        private readonly RestoreCommandHandler _restoreCommandHandler;
        private readonly PackageManagerUICommandHandler _packageManagerUICommandHandler;

        internal NuGetWorkspaceCommandHandler(JoinableTaskContext taskContext, IAsyncServiceProvider asyncServiceProvider)
        {
            if (taskContext == null)
            {
                throw new ArgumentNullException(nameof(taskContext));
            }

            _restoreCommandHandler = new RestoreCommandHandler(taskContext.Factory, asyncServiceProvider);
            _packageManagerUICommandHandler = new PackageManagerUICommandHandler(taskContext.Factory, asyncServiceProvider);
        }

        /// <summary>
        /// The command handlers priority. If there are multiple handlers for a given node
        /// then they are called in order of decreasing priority.
        /// </summary>
        public int Priority => 2000;

        /// <summary>
        /// Whether or not this handler should be ignored when multiple nodes are selected.
        /// </summary>
        public bool IgnoreOnMultiselect => true;

        public int Exec(List<WorkspaceVisualNodeBase> selection, Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == CommandGroup.NuGetOnlineEnvironmentsClientProjectCommandSetGuid)
            {
                var nCmdIDInt = (int)nCmdID;

                switch (nCmdIDInt)
                {
                    case PkgCmdIDList.CmdidRestorePackages:
                        if (IsSolutionOnlySelection(selection))
                        {
                            _restoreCommandHandler.RunSolutionRestore();
                            return VSConstants.S_OK;
                        }
                        break;
                    case PkgCmdIDList.CmdIdManageProjectUI:
                        if (IsSupportedProjectSelection(selection))
                        {
                            _packageManagerUICommandHandler.OpenPackageManagerUI(selection.Single());
                            return VSConstants.S_OK;
                        }
                        break;
                    default:
                        break;
                }
            }
            return (int)Constants.OLECMDERR_E_NOTSUPPORTED;
        }

        public bool QueryStatus(List<WorkspaceVisualNodeBase> selection, Guid pguidCmdGroup, uint nCmdID, ref uint cmdf, ref string customTitle)
        {
            bool handled = false;

            if (pguidCmdGroup == CommandGroup.NuGetOnlineEnvironmentsClientProjectCommandSetGuid)
            {
                var nCmdIDInt = (int)nCmdID;
                {
                    switch (nCmdIDInt)
                    {
                        case PkgCmdIDList.CmdidRestorePackages:
                            if (IsSolutionOnlySelection(selection))
                            {
                                var isRestoreActionInProgress = _restoreCommandHandler.IsRestoreActionInProgress();
                                cmdf = (uint)((isRestoreActionInProgress ? 0 : OLECMDF.OLECMDF_ENABLED) | OLECMDF.OLECMDF_SUPPORTED);
                                handled = true;
                            }
                            break;
                        case PkgCmdIDList.CmdIdManageProjectUI:
                            if (IsSupportedProjectSelection(selection))
                            {
                                var isPackageManagerUISupported = _packageManagerUICommandHandler.IsPackageManagerUISupported(selection.Single());
                                cmdf = (uint)(isPackageManagerUISupported ?
                                    (OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED) :
                                    OLECMDF.OLECMDF_INVISIBLE);
                                handled = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return handled;
        }

        private static bool IsSupportedProjectSelection(List<WorkspaceVisualNodeBase> selection)
        {
            if (selection != null &&
                selection.Count.Equals(1))
            {
                // We support every item representing a project
                // - we don't have the ability to do a capabilities check, because we don't have enough information.
                string fileExtension;
                try
                {
                    fileExtension = Path.GetExtension(selection.Single().NodeMoniker);
                }
                catch (ArgumentException)
                {
                    return false;
                }

                if (fileExtension == null)
                {
                    return false;
                }

                // We do not know if the project is supported
                return fileExtension.EndsWith("proj", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private static bool IsSolutionOnlySelection(List<WorkspaceVisualNodeBase> selection)
        {
            return selection != null &&
                selection.Count.Equals(1) &&
                selection.First().NodeMoniker.Equals(string.Empty, StringComparison.Ordinal);
        }
    }
}
