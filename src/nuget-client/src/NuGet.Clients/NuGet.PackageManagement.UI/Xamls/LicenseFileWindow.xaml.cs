// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
