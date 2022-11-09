// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;

namespace NuGetVSExtension
{
    public sealed class NuGetSearchTask : IVsSearchTask
    {
        private readonly NuGetSearchProvider _provider;
        private readonly IVsSearchProviderCallback _searchCallback;
        private readonly OleMenuCommand _managePackageDialogCommand;
        private readonly OleMenuCommand _managePackageForSolutionDialogCommand;

        public NuGetSearchTask(NuGetSearchProvider provider, uint cookie, IVsSearchQuery searchQuery, IVsSearchProviderCallback searchCallback, OleMenuCommand managePackageDialogCommand, OleMenuCommand managePackageForSolutionDialogCommand)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (searchQuery == null)
            {
                throw new ArgumentNullException(nameof(searchQuery));
            }
            if (searchCallback == null)
            {
                throw new ArgumentNullException(nameof(searchCallback));
            }
            if (managePackageDialogCommand == null)
            {
                throw new ArgumentNullException(nameof(managePackageDialogCommand));
            }
            if (managePackageForSolutionDialogCommand == null)
            {
                throw new ArgumentNullException(nameof(managePackageForSolutionDialogCommand));
            }
            _provider = provider;
            _searchCallback = searchCallback;
            _managePackageDialogCommand = managePackageDialogCommand;
            _managePackageForSolutionDialogCommand = managePackageForSolutionDialogCommand;

            SearchQuery = searchQuery;
            Id = cookie;
            ErrorCode = 0;

            SetStatus(VsSearchTaskStatus.Created);
        }

        public int ErrorCode { get; private set; }

        public uint Id { get; private set; }

        public IVsSearchQuery SearchQuery { get; private set; }

        public uint Status { get; private set; }

        public void Start()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                SetStatus(VsSearchTaskStatus.Started);

                SetStatus(VsSearchTaskStatus.Completed);
                var supportedManagePackageCommand = GetSupportedManagePackageCommand();

                if (!string.IsNullOrEmpty(SearchQuery.SearchString)
                    && null != supportedManagePackageCommand)
                {
                    var result = new NuGetStaticSearchResult(SearchQuery.SearchString, _provider, supportedManagePackageCommand);
                    _searchCallback.ReportResult(this, result);
                    _searchCallback.ReportComplete(this, 1);
                }
                else
                {
                    _searchCallback.ReportComplete(this, 0);
                }
            });
        }

        public void Stop()
        {
            SetStatus(VsSearchTaskStatus.Stopped);
        }

        private void SetStatus(VsSearchTaskStatus taskStatus)
        {
            Status = (uint)taskStatus;
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "Just to make TeamCity build happy. We don't see any FxCop issue when built locally.")]
        private OleMenuCommand GetSupportedManagePackageCommand()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            // Call QueryStatus for _managePackageDialogCommand and _managePackageForSolutionDialogCommand below
            // to refresh the visibility of the command which is used to determine whether search results should be displayed or not.
            // The following API QueryStatusCommand returns S_OK if successful
            // int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText);
            // Note that both the commands belong to the commandGroup. That is, the first parameter GUID representing the cmdGroup
            // is the same for both the commands. Hence, it is possible to query for their status in a single call

            OLECMD[] cmd = new OLECMD[2];
            cmd[0].cmdID = (uint)_managePackageDialogCommand.CommandID.ID;
            cmd[1].cmdID = (uint)_managePackageForSolutionDialogCommand.CommandID.ID;
            Guid guid = _managePackageDialogCommand.CommandID.Guid;
            int result = ((IOleCommandTarget)_provider.MenuCommandService).QueryStatus(pguidCmdGroup: ref guid, cCmds: 2u, prgCmds: cmd, pCmdText: (IntPtr)null);

            // At this point, if result == S_OK, the visibility of the commands are up to date and can be used confidently
            if (result == VSConstants.S_OK
                && _managePackageDialogCommand.Visible
                && _managePackageDialogCommand.Enabled)
            {
                return _managePackageDialogCommand;
            }

            if (result == VSConstants.S_OK
                && _managePackageForSolutionDialogCommand.Visible
                && _managePackageForSolutionDialogCommand.Enabled)
            {
                return _managePackageForSolutionDialogCommand;
            }

            return null;
        }

        private enum VsSearchTaskStatus : uint
        {
            Completed = 2,
            Created = 0,
            Error = 4,
            Started = 1,
            Stopped = 3
        }
    }
}
