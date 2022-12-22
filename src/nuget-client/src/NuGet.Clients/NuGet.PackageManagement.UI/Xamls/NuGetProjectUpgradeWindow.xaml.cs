// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for NuGetProjectUpgradeWindow.xaml
    /// </summary>
    public partial class NuGetProjectUpgradeWindow : DialogWindow
    {
        public NuGetProjectUpgradeWindow(NuGetProjectUpgradeWindowModel model)
        {
            DataContext = model;
            InitializeComponent();
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ExecuteOpenExternalLink(object sender, ExecutedRoutedEventArgs e)
        {
            var hyperlink = (Hyperlink)e.OriginalSource;
            if (hyperlink.NavigateUri != null)
            {
                Process.Start(hyperlink.NavigateUri.AbsoluteUri);
                e.Handled = true;
            }
        }

        private void PromoteAllToTopLevel_Checked(object sender, RoutedEventArgs e)
        {
            var model = DataContext as NuGetProjectUpgradeWindowModel;
            var checkBox = sender as CheckBox;
            foreach (var item in model.TransitiveDependencies)
            {
                item.InstallAsTopLevel = checkBox.IsChecked.GetValueOrDefault();
            }
        }
    }
}
