// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

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
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.SolutionRestoreManager
{
    [Export(typeof(IVulnerabilitiesNotificationService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class VulnerablePackagesInfoBar : IVulnerabilitiesNotificationService, IVsInfoBarUIEvents
    {
        private IAsyncServiceProvider _asyncServiceProvider = AsyncServiceProvider.GlobalProvider;
        private IVsInfoBarUIElement? _infoBarUIElement;
        private bool _infoBarVisible = false; // InfoBar is currently being displayed in the Solution Explorer
        private bool _wasInfoBarClosed = false; // InfoBar was closed by the user, using the 'x'(close) in the InfoBar
        private bool _wasInfoBarHidden = false; // InfoBar was hid, this is caused because there are no more vulnerabilities to address
        private uint? _eventCookie; // To hold the connection cookie

        [Import]
        private Lazy<IPackageManagerLaunchService>? PackageManagerLaunchService { get; set; }

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
            PackageManagerLaunchService?.Value.LaunchSolutionPackageManager();
        }

        protected InfoBarModel GetInfoBarModel()
        {
            IEnumerable<IVsInfoBarTextSpan> textSpans = new IVsInfoBarTextSpan[]
            {
                new InfoBarTextSpan(Resources.InfoBar_TextMessage + " "),
                new InfoBarHyperlink(Resources.InfoBar_HyperlinkMessage)
            };

            return new InfoBarModel(
                textSpans,
                KnownMonikers.StatusWarning);
        }
    }
}
