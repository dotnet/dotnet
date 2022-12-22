// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using SB = Microsoft.VisualStudio.Shell.ServiceBroker;
using Task = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio.OnlineEnvironment.Client
{
    internal class RestoreCommandHandler
    {
        private readonly JoinableTaskFactory _joinableTaskFactory;
        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private CancellationTokenSource _cancelBuildToken;

        public RestoreCommandHandler(JoinableTaskFactory joinableTaskFactory, IAsyncServiceProvider asyncServiceProvider)
        {
            _joinableTaskFactory = joinableTaskFactory ?? throw new ArgumentNullException(nameof(joinableTaskFactory));
            _asyncServiceProvider = asyncServiceProvider ?? throw new ArgumentNullException(nameof(asyncServiceProvider));
        }

        public void RunSolutionRestore()
        {
            _joinableTaskFactory.RunAsync(RunSolutionRestoreAsync).PostOnFailure(nameof(RestoreCommandHandler));
        }

        private async Task RunSolutionRestoreAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            _cancelBuildToken = cancellationTokenSource;

            try
            {
                await GetAndActivatePackageManagerOutputWindowAsync();

                await TaskScheduler.Default;
                SB.IBrokeredServiceContainer serviceContainer = await _asyncServiceProvider.GetServiceAsync<SB.SVsBrokeredServiceContainer, SB.IBrokeredServiceContainer>();
                IServiceBroker serviceBroker = serviceContainer.GetFullAccessServiceBroker();

                INuGetSolutionService nugetSolutionService = await serviceBroker.GetProxyAsync<INuGetSolutionService>(NuGetServices.SolutionService);

                try
                {
                    await nugetSolutionService.RestoreSolutionAsync(cancellationTokenSource.Token);
                }
                finally
                {
                    (nugetSolutionService as IDisposable)?.Dispose();
                }
            }
            catch (Exception e)
            {
                // Only log to the activity log for now
                // TODO: https://github.com/NuGet/Home/issues/9352
                ActivityLog.LogError("NuGet Package Manager", e.Message);
            }
            finally
            {
                cancellationTokenSource.Dispose();
                _cancelBuildToken = null;
            }
        }

        public bool IsRestoreActionInProgress()
        {
            return _cancelBuildToken != null;
        }

        private async Task GetAndActivatePackageManagerOutputWindowAsync()
        {
            var outputWindow = await _asyncServiceProvider.GetServiceAsync<SVsOutputWindow, IVsOutputWindow>();
            var vsUIShell = await _asyncServiceProvider.GetServiceAsync<SVsUIShell, IVsUIShell>();

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // create the Package Manager pane within the Output window
            var hr = outputWindow.CreatePane(
                ref GuidList.GuidNuGetOutputWindowPaneGuid,
                Resources.OutputConsolePaneName,
                fInitVisible: 1,
                fClearWithSolution: 0);
            ErrorHandler.ThrowOnFailure(hr);

            IVsOutputWindowPane pane;
            hr = outputWindow.GetPane(
                ref GuidList.GuidNuGetOutputWindowPaneGuid,
                out pane);
            ErrorHandler.ThrowOnFailure(hr);

            Guid outputToolWindow = VSConstants.StandardToolWindows.Output;

            vsUIShell.FindToolWindow(0,
                ref outputToolWindow,
                out var toolWindow);
            toolWindow?.Show();

            pane.Activate();
        }
    }
}
