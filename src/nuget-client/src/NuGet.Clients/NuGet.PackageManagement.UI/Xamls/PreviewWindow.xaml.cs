// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using NuGet.PackageManagement.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : DialogWindow
    {
        private bool _initialized;
        private INuGetUIContext _uiContext;

        public PreviewWindow(INuGetUIContext uiContext)
        {
            _initialized = false;
            _uiContext = uiContext;
            InitializeComponent();
            _doNotShowCheckBox.IsChecked = IsDoNotShowPreviewWindowEnabled();
            var copyBindings = ApplicationCommands.Copy.InputGestures;
            foreach (KeyGesture gesture in copyBindings)
            {
                InputBindings.Add(
                    new KeyBinding(
                        ApplicationCommands.Copy,
                        gesture
                    )
                );
            }
            _initialized = true;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CopyToClipboard()
        {
            var windowModel = _changesItems.DataContext as PreviewWindowModel;
            Clipboard.SetText(windowModel.ToString());
        }

        private void CopyButtonClicked(object sender, RoutedEventArgs e)
        {
            CopyToClipboard();
        }

        private void ExecuteCopyToClipboard(object sender, ExecutedRoutedEventArgs e)
        {
            CopyToClipboard();
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
            _uiContext.UserSettingsManager.ApplyShowPreviewSetting(!doNotshow);
            RegistrySettingUtility.SetBooleanSetting(
                Constants.DoNotShowPreviewWindowRegistryName,
                doNotshow);
        }

        public static bool IsDoNotShowPreviewWindowEnabled()
        {
            return RegistrySettingUtility.GetBooleanSetting(
                Constants.DoNotShowPreviewWindowRegistryName);
        }
    }
}
