// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for PackageManagementFormatWindow.xaml
    /// </summary>
    public partial class PackageManagementFormatWindow : DialogWindow
    {
        private INuGetUIContext _uiContext;

        public PackageManagementFormatWindow(INuGetUIContext uiContext)
        {
            _uiContext = uiContext;
            InitializeComponent();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            var selectedFormat = DataContext as PackageManagementFormat;

            if (selectedFormat != null)
            {
                selectedFormat.ApplyChanges();
            }

            DialogResult = true;
        }

        private void ExecuteOpenExternalLink(object sender, ExecutedRoutedEventArgs e)
        {
            var hyperlink = e.OriginalSource as Hyperlink;
            if (hyperlink != null
                && hyperlink.NavigateUri != null)
            {
                UIUtility.LaunchExternalLink(hyperlink.NavigateUri);
                e.Handled = true;
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            _uiContext.OptionsPageActivator.ActivatePage(OptionsPage.General, null);
        }

    }
}
