// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Show/hide policy for the Package Manager UI "packages have vulnerabilities" InfoBar.
    /// Owns the scenario state (session suppression) and the InfoBar content; the actual hosting
    /// is delegated to <see cref="PackageManagerInfoBarService"/>.
    /// </summary>
    internal sealed class PackageManagerVulnerabilitiesInfoBar
    {
        internal const string InfoBarKey = "vulnerabilities";

        private readonly PackageManagerInfoBarService _infoBarService;
        private readonly Func<FixVulnerabilitiesSource, CancellationToken, Task> _launchFixVulnerabilitiesAsync;

        // The user dismissed the InfoBar with the close ('x') button; don't show it again for this window's session.
        private bool _userClosed;

        // The vulnerable count currently reflected in the visible InfoBar, so we only rebuild it when it changes.
        private int _lastShownCount;

        public PackageManagerVulnerabilitiesInfoBar(PackageManagerInfoBarService infoBarService, Func<FixVulnerabilitiesSource, CancellationToken, Task> launchFixVulnerabilitiesAsync)
        {
            _infoBarService = infoBarService ?? throw new ArgumentNullException(nameof(infoBarService));
            _launchFixVulnerabilitiesAsync = launchFixVulnerabilitiesAsync ?? throw new ArgumentNullException(nameof(launchFixVulnerabilitiesAsync));
        }

        /// <summary>
        /// Shows the InfoBar when there are vulnerable packages installed, and hides it when there are none.
        /// Reflects the current count and updates in place when it changes. Idempotent: safe to call on every refresh.
        /// </summary>
        public async Task UpdateAsync(int vulnerablePackagesCount)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (_userClosed)
            {
                return;
            }

            if (vulnerablePackagesCount <= 0)
            {
                _infoBarService.HideInfoBar(InfoBarKey);
                _lastShownCount = 0;
                return;
            }

            if (_infoBarService.IsInfoBarVisible(InfoBarKey))
            {
                if (vulnerablePackagesCount != _lastShownCount)
                {
                    _infoBarService.UpdateInfoBar(InfoBarKey, GetInfoBarModel(vulnerablePackagesCount));
                }
            }
            else
            {
                _infoBarService.ShowInfoBar(InfoBarKey, GetInfoBarModel(vulnerablePackagesCount), OnActionClicked, OnClosed);
            }

            _lastShownCount = vulnerablePackagesCount;
        }

        internal static InfoBarModel GetInfoBarModel(int vulnerablePackagesCount)
        {
            IVsInfoBarTextSpan[] textSpans =
            [
                new InfoBarTextSpan(UIUtility.GetInstalledVulnerablePackagesWarningText(vulnerablePackagesCount) + " "),
                new InfoBarHyperlink(Resources.InfoBar_FixVulnerabilitiesWithCopilot),
            ];

            return new InfoBarModel(
                textSpans,
                KnownMonikers.StatusWarning,
                isCloseButtonVisible: true);
        }

        private void OnActionClicked(IVsInfoBarActionItem actionItem)
        {
            // Only action item is the "Fix with GitHub Copilot" hyperlink.
            NuGetUIThreadHelper.JoinableTaskFactory
                .RunAsync(() => _launchFixVulnerabilitiesAsync(FixVulnerabilitiesSource.PackageManagerInfoBar, CancellationToken.None))
                .PostOnFailure(nameof(PackageManagerVulnerabilitiesInfoBar));
        }

        private void OnClosed()
        {
            _userClosed = true;
        }
    }
}
