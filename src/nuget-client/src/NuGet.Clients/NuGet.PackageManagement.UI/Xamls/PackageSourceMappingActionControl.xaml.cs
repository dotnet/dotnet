// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Windows.Controls;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.PackageManagement.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for PackageSourceMappingActionControl.xaml
    /// </summary>
    public partial class PackageSourceMappingActionControl : UserControl
    {
        public PackageSourceMappingActionControl()
        {
            InitializeComponent();
        }

        private void SettingsButtonClicked(object sender, EventArgs e)
        {
            var viewModel = (PackageSourceMappingActionViewModel)DataContext;
            viewModel.UIController.LaunchNuGetOptionsDialog(OptionsPage.PackageSourceMapping);
        }
    }
}
