// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI.Options
{
    [Guid("2819C3B6-FC75-4CD5-8C77-877903DE864C")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class PackageSourceOptionsPage : OptionsPageBase
    {
        private PackageSourcesOptionsControl _optionsWindow;

        protected override void OnActivate(CancelEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            base.OnActivate(e);
            PackageSourcesControl.Font = VsShellUtilities.GetEnvironmentFont(ServiceProvider.GlobalProvider);
            DoCancelableOperationWithProgressUI(() =>
            {
                // Normally we shouldn't wrap JTF around BrokeredCalls but this is in a cancelable operation already
                NuGetUIThreadHelper.JoinableTaskFactory.Run(async () => await OnActivateAsync(e, CancellationToken));

            }, Resources.PackageSourceOptions_OnActivated);
        }

        private async Task OnActivateAsync(CancelEventArgs e, CancellationToken cancellationToken)
        {
            await PackageSourcesControl.InitializeOnActivatedAsync(cancellationToken);
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            // Do not need to call base.OnApply() here.
            DoCancelableOperationWithProgressUI(() =>
            {
                // Normally we shouldn't wrap JTF around BrokeredCalls but this is in a cancelable operation already
                NuGetUIThreadHelper.JoinableTaskFactory.Run(async () => await OnApplyAsync(e, CancellationToken));
            }, Resources.PackageSourceOptions_OnApply);
        }

        private async Task OnApplyAsync(PageApplyEventArgs e, CancellationToken cancellationToken)
        {
            bool wasApplied = await PackageSourcesControl.ApplyChangedSettingsAsync(cancellationToken);

            if (!wasApplied)
            {
                e.ApplyBehavior = ApplyKind.CancelNoNavigate;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            PackageSourcesControl.ClearSettings();
            base.OnClosed(e);
        }

        private PackageSourcesOptionsControl PackageSourcesControl
        {
            get
            {
                if (_optionsWindow == null)
                {
                    _optionsWindow = new PackageSourcesOptionsControl(AsyncServiceProvider.GlobalProvider);
                    _optionsWindow.Location = new Point(0, 0);
                }

                return _optionsWindow;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get { return PackageSourcesControl; }
        }
    }
}
