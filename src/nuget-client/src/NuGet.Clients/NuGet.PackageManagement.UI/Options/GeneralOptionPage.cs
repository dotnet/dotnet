// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace NuGet.PackageManagement.UI.Options
{
    [Guid("0F052CF7-BF62-4743-B190-87FA4D49421E")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class GeneralOptionPage : OptionsPageBase
    {
        private GeneralOptionControl _optionsWindow;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get { return GeneralControl; }
        }

        protected override void OnClosed(EventArgs e)
        {
            GeneralControl.OnClosed();
            base.OnClosed(e);
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            base.OnActivate(e);

            GeneralControl.Font = VsShellUtilities.GetEnvironmentFont(ServiceProvider.GlobalProvider);
            GeneralControl.OnActivated();
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);
            if (!GeneralControl.OnApply()) e.ApplyBehavior = ApplyKind.Cancel;
        }

        private GeneralOptionControl GeneralControl
        {
            get
            {
                if (_optionsWindow == null)
                {
                    _optionsWindow = new GeneralOptionControl
                    {
                        Location = new Point(0, 0)
                    };
                }

                return _optionsWindow;
            }
        }
    }
}
