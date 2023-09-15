// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using NuGet.Versioning;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for ProjectView.xaml. Its DataContext is PackageDetailControlModel.
    /// </summary>
    public partial class ProjectView : UserControl
    {
        public event EventHandler<EventArgs> InstallButtonClicked;
        public event EventHandler<EventArgs> UninstallButtonClicked;

        public ProjectView()
        {
            InitializeComponent();

            // Change ItemContainerStyle of the _versions combobox so that
            // for a null value, a separator is generated.
            var dataTrigger = new DataTrigger();
            dataTrigger.Binding = new Binding();
            dataTrigger.Value = null;
            dataTrigger.Setters.Add(new Setter(TemplateProperty, FindResource("SeparatorControlTemplate")));

            // make sure the separator can't be selected thru keyboard navigation.
            dataTrigger.Setters.Add(new Setter(IsEnabledProperty, false));

            var style = new Style(typeof(ComboBoxItem), _versions.ItemContainerStyle);

            style.Triggers.Add(dataTrigger);
            _versions.ItemContainerStyle = style;
        }

        private TextBox _textBox;

        private TextBox TextBox
        {
            get
            {
                if (_textBox == null) _textBox = _versions.Template.FindName("PART_EditableTextBox", _versions) as TextBox;
                return _textBox;
            }
            set
            {
                _textBox = value;
            }
        }

        private PackageDetailControlModel _packageDetailControlModel;

        private PackageDetailControlModel PackageDetailControlModel
        {
            get
            {
                if (_packageDetailControlModel == null) _packageDetailControlModel = (PackageDetailControlModel)DataContext;
                return _packageDetailControlModel;
            }
            set
            {
                _packageDetailControlModel = value;
            }
        }

        private DetailControlModel _detailModel;

        private DetailControlModel DetailModel
        {
            get
            {
                if (_detailModel == null) _detailModel = (DetailControlModel)DataContext;
                return _detailModel;
            }
            set
            {
                _detailModel = value;
            }
        }

        private void Versions_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (PackageDetailControlModel.IsProjectPackageReference)
            {
                switch (e.Key)
                {
                    case Key.Tab:
                    case Key.Enter:
                        _versions.IsDropDownOpen = false;
                        break;
                    case Key.Escape:
                        _versions.IsDropDownOpen = false;
                        _versions.SelectedIndex = -1;
                        break;
                    case Key.Space:
                        e.Handled = true;
                        break;
                    case Key.Down:
                        if (_versions.SelectedIndex < _versions.Items.Count - 1)
                        {
                            // Handle null separator
                            if (_versions.Items[_versions.SelectedIndex + 1] == null)
                            {
                                _versions.SelectedIndex = _versions.SelectedIndex + 2;
                            }
                            else
                            {
                                _versions.SelectedIndex++;
                            }

                            e.Handled = true;
                        }
                        break;
                    case Key.Up:
                        if (_versions.SelectedIndex > 0)
                        {
                            // Handle null separator
                            if (_versions.Items[_versions.SelectedIndex - 1] == null)
                            {
                                _versions.SelectedIndex = _versions.SelectedIndex - 2;
                            }
                            else
                            {
                                _versions.SelectedIndex--;
                            }

                            e.Handled = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Tab:
                        _versions.IsDropDownOpen = false;
                        break;
                    default:
                        base.OnPreviewKeyDown(e);
                        break;
                }
            }
        }

        private void Versions_KeyUp(object sender, KeyEventArgs e)
        {
            if (PackageDetailControlModel.IsProjectPackageReference)
            {
                string comboboxText = _versions.Text;
                IEnumerable<NuGetVersion> versions = DetailModel.Versions.Where(v => v != null).Select(v => v.Version);

                switch (e.Key)
                {
                    case Key.Enter:
                        VersionRange userRequestedVersionRange = null;
                        bool isAVersion = _versions.Items.CurrentItem != null;
                        bool isUserInputValidNuGetVersionRange = _versions.SelectedIndex >= 0 ? VersionRange.TryParse(comboboxText, out userRequestedVersionRange) : false;

                        if (_versions.SelectedIndex >= 0 && isAVersion)
                        {
                            // Check if the user typed a custom version
                            if (isUserInputValidNuGetVersionRange)
                            {
                                // Search for the best version
                                NuGetVersion rangeBestVersion = userRequestedVersionRange.FindBestMatch(versions);
                                DisplayVersion selectedVersion = (DisplayVersion)_versions.Items.CurrentItem;
                                bool isBestOption = rangeBestVersion.ToString() == selectedVersion.Version.ToString();
                                if (isBestOption)
                                {
                                    // Add vulnerable/deprecated labels to non floating/range versions
                                    if (userRequestedVersionRange.OriginalString.StartsWith("(", StringComparison.OrdinalIgnoreCase) ||
                                        userRequestedVersionRange.OriginalString.StartsWith("[", StringComparison.OrdinalIgnoreCase) ||
                                        userRequestedVersionRange.IsFloating)
                                    {
                                        PackageDetailControlModel.SelectedVersion = new DisplayVersion(userRequestedVersionRange, rangeBestVersion, additionalInfo: null);
                                    }
                                    else
                                    {
                                        PackageDetailControlModel.SelectedVersion = new DisplayVersion(userRequestedVersionRange, rangeBestVersion, additionalInfo: null, isVulnerable: selectedVersion.IsVulnerable, isDeprecated: selectedVersion.IsDeprecated);
                                    }
                                    _versions.Text = PackageDetailControlModel.SelectedVersion.ToString();
                                }
                                else
                                {
                                    PackageDetailControlModel.SelectedVersion = _versions.Items[_versions.SelectedIndex] as DisplayVersion;
                                    _versions.Text = PackageDetailControlModel.SelectedVersion.ToString();
                                }
                            }
                            else
                            {
                                PackageDetailControlModel.SelectedVersion = _versions.Items[_versions.SelectedIndex] as DisplayVersion;
                                _versions.Text = PackageDetailControlModel.SelectedVersion.ToString();
                            }

                            TextBox.SelectionStart = 0;
                            TextBox.SelectionLength = int.MaxValue;
                            _versions.IsDropDownOpen = false;
                        }

                        e.Handled = true;
                        break;
                    case Key.Down:
                    case Key.Up:
                    case Key.Space:
                    case Key.Tab:
                        e.Handled = true;
                        break;
                    default:
                        if (PackageDetailControlModel.PreviousSelectedVersion != comboboxText)
                        {
                            PackageDetailControlModel.PreviousSelectedVersion = comboboxText;
                            var selectionStart = TextBox.SelectionStart;

                            PackageDetailControlModel.UserInput = comboboxText; // Update the variable so the filter refreshes
                            SetComboboxCurrentVersion(comboboxText, versions);

                            _versions.Text = comboboxText;
                            TextBox.SelectionStart = selectionStart;

                            break;
                        }

                        base.OnKeyUp(e);
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Tab:
                        e.Handled = true;
                        break;
                    default:
                        base.OnKeyUp(e);
                        break;
                }
            }
        }

        private void SetComboboxCurrentVersion(string comboboxText, IEnumerable<NuGetVersion> versions)
        {
            NuGetVersion matchVersion = null;
            VersionRange userRange = null;

            // Get the best version from the range if the user typed a custom version
            bool userTypedAValidVersionRange = VersionRange.TryParse(comboboxText, out userRange);
            if (userTypedAValidVersionRange)
            {
                matchVersion = userRange.FindBestMatch(versions);
            }

            if (matchVersion == null && userRange == null)
            {
                return;
            }

            // If the selected version is not the correct one, deselect a version so Install/Update button is disabled.
            if (_versions.SelectedIndex != -1 && matchVersion?.ToString() != _versions.Items[_versions.SelectedIndex].ToString())
            {
                _versions.SelectedIndex = -1;
            }

            // Automatically select the item when the input or custom range text matches it
            for (int i = 0; i < _versions.Items.Count; i++)
            {
                DisplayVersion currentItem = _versions.Items[i] as DisplayVersion;
                if (currentItem != null && // null, this represent a bar in UI in the Versions combobox 
                    (comboboxText.Trim() == currentItem.Version.ToNormalizedString() || matchVersion?.ToString() == currentItem.Version.ToNormalizedString())) // trim extra spaces and compare versions without labels
                {
                    _versions.SelectedIndex = i; // This is the "select" effect in the dropdown
                    PackageDetailControlModel.SelectedVersion = new DisplayVersion(userRange, matchVersion, additionalInfo: null, isDeprecated: currentItem.IsDeprecated, isVulnerable: currentItem.IsVulnerable);
                }
            }
        }

        private void UninstallButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (UninstallButtonClicked != null)
            {
                UninstallButtonClicked(this, EventArgs.Empty);
            }
        }

        private void InstallButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (InstallButtonClicked != null)
            {
                InstallButtonClicked(this, EventArgs.Empty);
            }
        }

        private void Versions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PackageDetailControlModel.PreviousSelectedVersion = e.AddedItems.Count > 0 && e.AddedItems[0] != null ? e.AddedItems[0].ToString() : string.Empty;

            return;
        }
    }
}
