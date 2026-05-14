// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Documents;
using Microsoft.VisualStudio.PlatformUI;
using NuGet.PackageManagement.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    public partial class DeprecatedFrameworkWindow : DialogWindow
    {
        private bool _initialized;
        private INuGetUIContext _uiContext;

        public DeprecatedFrameworkWindow(INuGetUIContext uiContext)
        {
            _initialized = false;
            _uiContext = uiContext;
            InitializeComponent();
            _doNotShowCheckBox.IsChecked = DotnetDeprecatedPrompt.GetDoNotShowPromptState();

            _initialized = true;
        }

        private void OnMigrationUrlNavigate(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink)sender;
            if (hyperlink != null
                && hyperlink.NavigateUri != null)
            {
                UIUtility.LaunchExternalLink(hyperlink.NavigateUri);
                e.Handled = true;
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void DoNotShowCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!_initialized)
            {
                return;
            }

            SaveDoNotShowPreviewWindowSetting(doNotshow: true);
        }

        private void DoNotShowCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_initialized)
            {
                return;
            }

            SaveDoNotShowPreviewWindowSetting(doNotshow: false);
        }

        private void SaveDoNotShowPreviewWindowSetting(bool doNotshow)
        {
            _uiContext.UserSettingsManager.ApplyShowDeprecatedFrameworkSetting(!doNotshow);

            DotnetDeprecatedPrompt.SaveDoNotShowPromptState(doNotshow);
        }
    }
}
