// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using Microsoft.VisualStudio.PlatformUI;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for LicenseFileWindow.xaml
    /// </summary>
    public partial class LicenseFileWindow : DialogWindow
    {
        public LicenseFileWindow()
        {
            InitializeComponent();
            Closing += new CancelEventHandler(Window_ClosingAction);
        }

        // We need to unparent the flow document to allow a follow-up click to view the same license work.
        private void Window_ClosingAction(object sender, CancelEventArgs e)
        {
            DataContext = null;
        }
    }
}
