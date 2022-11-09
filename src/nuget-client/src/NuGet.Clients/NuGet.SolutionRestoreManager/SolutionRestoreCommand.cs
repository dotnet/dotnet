// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.PackageManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Telemetry;
using Task = System.Threading.Tasks.Task;

namespace NuGet.SolutionRestoreManager
{
    /// <summary>
    /// Restore packages menu command handler.
    /// </summary>
    internal sealed class SolutionRestoreCommand
    {
        private static SolutionRestoreCommand _instance;

        private const int CommandId = PkgCmdIDList.cmdidRestorePackages;
        private static readonly Guid CommandSet = GuidList.guidNuGetDialogCmdSet;

        [Import]
        private Lazy<ISolutionManager> SolutionManager { get; set; }

        [Import]
        private Lazy<ISolutionRestoreWorker> SolutionRestoreWorker { get; set; }

        [Import]
        private Lazy<IConsoleStatus> ConsoleStatus { get; set; }

        private IVsMonitorSelection _vsMonitorSelection;
        private uint _solutionExistsAndFullyLoadedContextCookie;
        private uint _solutionNotBuildingAndNotDebuggingContextCookie;

        private Task _restoreTask = Task.CompletedTask;

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider _serviceProvider;

        private bool _isMEFInitialized;

        private SolutionRestoreCommand()
        {
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="serviceProvider">Owner package, not null.</param>
        public static async Task InitializeAsync(Microsoft.VisualStudio.Shell.IAsyncServiceProvider serviceProvider)
        {
            Assumes.Present(serviceProvider);

            _instance = new SolutionRestoreCommand();

            await _instance.SubscribeAsync(serviceProvider);
        }

        private async Task SubscribeAsync(Microsoft.VisualStudio.Shell.IAsyncServiceProvider serviceProvider)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            _serviceProvider = serviceProvider;

            var commandService = await serviceProvider.GetServiceAsync<IMenuCommandService, IMenuCommandService>(throwOnFailure: false);

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(
                OnRestorePackages, null, BeforeQueryStatusForPackageRestore, menuCommandId);

            commandService.AddCommand(menuItem);

            _vsMonitorSelection = await serviceProvider.GetServiceAsync<IVsMonitorSelection, IVsMonitorSelection>();
            Assumes.Present(_vsMonitorSelection);

            // get the solution not building and not debugging cookie
            var guidCmdUI = VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_guid;
            _vsMonitorSelection.GetCmdUIContextCookie(
                ref guidCmdUI, out _solutionExistsAndFullyLoadedContextCookie);

            guidCmdUI = VSConstants.UICONTEXT.SolutionExistsAndNotBuildingAndNotDebugging_guid;
            _vsMonitorSelection.GetCmdUIContextCookie(
                ref guidCmdUI, out _solutionNotBuildingAndNotDebuggingContextCookie);
        }

        private async Task InitializeMEFAsync()
        {
            var componentModel = await _serviceProvider.GetComponentModelAsync();
            componentModel.DefaultCompositionService.SatisfyImportsOnce(_instance);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void OnRestorePackages(object sender, EventArgs args)
        {
            if (!_isMEFInitialized)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await InitializeMEFAsync();
                });

                _isMEFInitialized = true;
            }

            if (_restoreTask.IsCompleted)
            {
                _restoreTask = NuGetUIThreadHelper.JoinableTaskFactory
                    .RunAsync(() => SolutionRestoreWorker.Value.ScheduleRestoreAsync(
                        SolutionRestoreRequest.ByUserCommand(ExplicitRestoreReason.RestoreSolutionPackages),
                        CancellationToken.None))
                    .Task;
            }
        }

        public void BeforeQueryStatusForPackageRestore(object sender, EventArgs args)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                if (!_isMEFInitialized)
                {
                    await InitializeMEFAsync();
                    _isMEFInitialized = true;
                }

                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var command = (OleMenuCommand)sender;

                // Enable the 'Restore NuGet Packages' dialog menu
                // - if the solution exists and not debugging and not building AND
                // - if the console is NOT busy executing a command, AND
                // - if the restore worker is not executing restore operation, AND
                // - if there are NuGetProjects. This means there are loaded, supported projects.
                command.Enabled =
                    IsSolutionExistsAndNotDebuggingAndNotBuilding() &&
                    _restoreTask.IsCompleted &&
                    !ConsoleStatus.Value.IsBusy &&
                    !SolutionRestoreWorker.Value.IsBusy &&
                    await SolutionManager.Value.DoesNuGetSupportsAnyProjectAsync();
            });
        }

        private bool IsSolutionExistsAndNotDebuggingAndNotBuilding()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var hr = _vsMonitorSelection.IsCmdUIContextActive(
                _solutionExistsAndFullyLoadedContextCookie, out var pfActive);

            if (ErrorHandler.Succeeded(hr) && pfActive > 0)
            {
                hr = _vsMonitorSelection.IsCmdUIContextActive(
                    _solutionNotBuildingAndNotDebuggingContextCookie, out pfActive);

                return ErrorHandler.Succeeded(hr) && pfActive > 0;
            }

            return false;
        }
    }
}
