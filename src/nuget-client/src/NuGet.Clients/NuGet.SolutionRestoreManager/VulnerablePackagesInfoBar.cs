// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.PackageManagement;
using NuGet.PackageManagement.Telemetry;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.SolutionRestoreManager
{
    [Export(typeof(IVulnerabilitiesNotificationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class VulnerablePackagesInfoBar : IVulnerabilitiesNotificationService, IVsInfoBarUIEvents
    {
        private IAsyncServiceProvider _asyncServiceProvider = AsyncServiceProvider.GlobalProvider;
        internal IVsInfoBarUIElement? _infoBarUIElement;
        internal bool _infoBarVisible = false; // InfoBar is currently being displayed in the Solution Explorer
        internal bool _wasInfoBarClosed = false; // InfoBar was closed by the user, using the 'x'(close) in the InfoBar
        internal bool _wasInfoBarHidden = false; // InfoBar was hid, this is caused because there are no more vulnerabilities to address
        private uint? _eventCookie; // To hold the connection cookie
        private IVsInfoBarActionItem? _launchPackageManagerActionItem;
        private IVsInfoBarActionItem? _fixVulnerabilitiesActionItem;

        private Lazy<IPackageManagerLaunchService>? PackageManagerLaunchService { get; }
        private Lazy<IFixVulnerabilitiesService>? FixVulnerabilitiesService { get; }
        private ISolutionManager? SolutionManager { get; }

        [ImportingConstructor]
        public VulnerablePackagesInfoBar(ISolutionManager solutionManager, Lazy<IPackageManagerLaunchService> packageManagerLaunchService, Lazy<IFixVulnerabilitiesService> fixVulnerabilitiesService)
        {
            SolutionManager = solutionManager;
            PackageManagerLaunchService = packageManagerLaunchService;
            FixVulnerabilitiesService = fixVulnerabilitiesService;
            SolutionManager.SolutionClosed += OnSolutionClosed;
        }

        private void OnSolutionClosed(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_infoBarVisible)
            {
                _infoBarUIElement?.Close();
            }
            // Reset all the state to defaults, since the solution is closed.
            _wasInfoBarHidden = false;
            _wasInfoBarClosed = false;
            _infoBarVisible = false;
        }

        public async Task ReportVulnerabilitiesAsync(bool hasVulnerabilitiesInSolution, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // If the infoBar was closed, don't show it for the rest of the VS session
            // if the infobar is visible and there are vulnerabilities, no work needed
            // if the infobar is not visible and there are no vulnerabilities, no work needed
            if (_wasInfoBarClosed || (hasVulnerabilitiesInSolution == _infoBarVisible))
            {
                return;
            }

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // Hide the InfoBar if Vulnerabilities were fixed
            if (!hasVulnerabilitiesInSolution && _infoBarVisible)
            {
                _wasInfoBarHidden = true;
                _infoBarUIElement?.Close();
                return;
            }

            try
            {
                await CreateInfoBar();

                _infoBarVisible = true;
                _wasInfoBarHidden = false;
            }
            catch (Exception ex)
            {
                await TelemetryUtility.PostFaultAsync(ex, nameof(VulnerablePackagesInfoBar));
                return;
            }
        }

        private async Task CreateInfoBar()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Initialize the InfoBar host in the SolutionExplorer window
            IVsInfoBarHost? infoBarHost;
            IVsUIShell? uiShell = await _asyncServiceProvider.GetServiceAsync<SVsUIShell, IVsUIShell>(throwOnFailure: true);
            int windowFrameCode = uiShell!.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fFindFirst, VSConstants.StandardToolWindows.SolutionExplorer, out var windowFrame);
            if (ErrorHandler.Failed(windowFrameCode))
            {
                Exception exception = new Exception(string.Format(CultureInfo.CurrentCulture, "Unable to find Solution Explorer window. HRRESULT {0}", windowFrameCode));
                await TelemetryUtility.PostFaultAsync(exception, nameof(VulnerablePackagesInfoBar));
                return;
            }

            object tempObject;
            int hostBarCode = windowFrame.GetProperty((int)__VSFPROPID7.VSFPROPID_InfoBarHost, out tempObject);
            if (ErrorHandler.Failed(hostBarCode))
            {
                Exception exception = new Exception(string.Format(CultureInfo.CurrentCulture, "Unable to find InfoBarHost. HRRESULT {0}", hostBarCode));
                await TelemetryUtility.PostFaultAsync(exception, nameof(VulnerablePackagesInfoBar));
                return;
            }

            infoBarHost = (IVsInfoBarHost)tempObject;

            // Create the VulnerabilitiesFound InfoBar
            IVsInfoBarUIFactory? infoBarFactory = await _asyncServiceProvider.GetServiceAsync<SVsInfoBarUIFactory, IVsInfoBarUIFactory>(throwOnFailure: false);
            if (infoBarFactory == null)
            {
                NullReferenceException exception = new NullReferenceException(nameof(infoBarFactory));
                await TelemetryUtility.PostFaultAsync(exception, nameof(VulnerablePackagesInfoBar));
                return;
            }

            InfoBarModel infoBarModel = GetInfoBarModel();

            _infoBarUIElement = infoBarFactory.CreateInfoBar(infoBarModel);
            _infoBarUIElement.Advise(this, out uint cookie);
            _eventCookie = cookie;

            infoBarHost.AddInfoBar(_infoBarUIElement);
        }

        public void OnClosed(IVsInfoBarUIElement infoBarUIElement)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_eventCookie.HasValue)
            {
                infoBarUIElement?.Unadvise(_eventCookie.Value);
                infoBarUIElement?.Close();
                _eventCookie = null;
            }

            _infoBarVisible = false;

            if (!_wasInfoBarHidden)
            {
                _wasInfoBarClosed = true;
            }
        }

        public void OnActionItemClicked(IVsInfoBarUIElement infoBarUIElement, IVsInfoBarActionItem actionItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (actionItem == _launchPackageManagerActionItem)
            {
                PackageManagerLaunchService?.Value.LaunchSolutionPackageManager();
                var evt = NavigatedTelemetryEvent.CreateWithVulnerabilityInfoBarManagePackages();
                TelemetryActivity.EmitTelemetryEvent(evt);
            }
            else if (actionItem == _fixVulnerabilitiesActionItem)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    if (FixVulnerabilitiesService == null)
                    {
                        return;
                    }

                    await FixVulnerabilitiesService.Value.LaunchFixVulnerabilitiesAsync(CancellationToken.None);
                }).PostOnFailure(nameof(VulnerablePackagesInfoBar));
            }
        }

        protected InfoBarModel GetInfoBarModel()
        {
            _launchPackageManagerActionItem = new InfoBarHyperlink(Resources.InfoBar_HyperlinkMessage);
            _fixVulnerabilitiesActionItem = new InfoBarHyperlink(Resources.InfoBar_HyperlinkFixVulnerabilitiesWithCopilot);

            IEnumerable<IVsInfoBarTextSpan> textSpans =
            [
                new InfoBarTextSpan(Resources.InfoBar_TextMessage + " "),
                _launchPackageManagerActionItem,
                new InfoBarTextSpan(" | "),
                _fixVulnerabilitiesActionItem
            ];

            return new InfoBarModel(
                textSpans,
                KnownMonikers.StatusWarning);
        }
    }
}
