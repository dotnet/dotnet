// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Windows.Controls;
using System.Windows.Input;

namespace NuGet.PackageManagement.UI.Options
{
    /// <summary>
    /// Interaction logic for ConfigurationFilesControl.xaml
    /// </summary>
    public partial class ConfigurationFilesControl : UserControl
    {
        private ConfigurationFilesViewModel _viewModel;

        public ConfigurationFilesControl(ConfigurationFilesViewModel configurationFiles)
        {
            InitializeComponent();
            _viewModel = configurationFiles;
            DataContext = configurationFiles;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _openButton.Command.Execute(null);
        }

        internal void InitializeOnActivated()
        {
            _viewModel.SetConfigPaths();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            UIUtility.LaunchExternalLink(e.Uri);
            e.Handled = true;
        }
    }
}
