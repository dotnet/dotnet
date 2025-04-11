// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;
using Resx = NuGet.PackageManagement.UI;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for SolutionView.xaml. The DataContext is PackageSolutionDetailControlModel.
    /// </summary>
    public partial class SolutionView : UserControl
    {
        public event EventHandler<EventArgs> InstallButtonClicked;

        public event EventHandler<EventArgs> UninstallButtonClicked;

        // the list of columns that are sortable.
        private List<GridViewColumnHeader> _sortableColumns;
        private GridViewColumnHeader _requestedVersionColumn;

        public SolutionView()
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

            _projectList.SizeChanged += ListView_SizeChanged;
            ((GridView)_projectList.View).Columns.CollectionChanged += Columns_CollectionChanged;

            //Requested Version column may not be needed, but since saved Sorting Settings are being restored at initialization time,
            //we should go ahead and create the Header column for it here with its Sort property name.
            _requestedVersionColumn = new GridViewColumnHeader();
            SortableColumnHeaderAttachedProperties.SetSortPropertyName(_requestedVersionColumn, "RequestedVersion");

            _sortableColumns = new List<GridViewColumnHeader>
            {
                _projectColumnHeader,
                _installedVersionColumnHeader,
                _requestedVersionColumn,
                _packageLevelColumnHeader
            };

            SortByColumn(_projectColumnHeader);
        }

        private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is GridViewColumnCollection columnCollection)
            {
                for (int i = 0; i < columnCollection.Count; i++)
                {
                    if (columnCollection[i].Header is GridViewColumnHeader columnHeader)
                    {
                        // When the project list column header collection changes in any way, recalculate the tabindex
                        // of each of the columns so the tab order is in the order they appear on screen.
                        columnHeader.TabIndex = i;
                    }
                }
            }
        }

        private void SolutionView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Since this event will fire when closing PMUI, when the model is set to Project PMUI,
            //and when other WPF controls in the tree set their DataContext, this cast is more important than it may seem.
            if (e.NewValue is PackageSolutionDetailControlModel)
            {
                var model = e.NewValue as PackageSolutionDetailControlModel;
                if (model.IsRequestedVisible)
                {
                    GridViewColumn _versionColumn = CreateRequestedVersionColumn();
                    _gridProjects.Columns.Insert(2, _versionColumn);
                }
            }
        }

        private void UninstallButton_Clicked(object sender, RoutedEventArgs e)
        {
            UninstallButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void InstallButton_Clicked(object sender, RoutedEventArgs e)
        {
            InstallButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ColumnHeader_Clicked(object sender, RoutedEventArgs e)
        {
            var columnHeader = sender as GridViewColumnHeader;
            if (columnHeader == null)
            {
                return;
            }

            SortByColumn(columnHeader);
        }

        public void SaveSettings(UserSettings settings)
        {
            var sortDescription = _projectList.Items.SortDescriptions.FirstOrDefault();
            if (sortDescription != default(SortDescription))
            {
                settings.SortPropertyName = sortDescription.PropertyName;
                settings.SortDirection = sortDescription.Direction;
            }
        }

        public void RestoreUserSettings(UserSettings userSettings)
        {
            if (userSettings == null)
            {
                return;
            }

            // find the column to sort
            var sortColumn = _sortableColumns.FirstOrDefault(
                column =>
                {
                    return StringComparer.OrdinalIgnoreCase.Equals(
                        SortableColumnHeaderAttachedProperties.GetSortPropertyName(obj: column),
                        userSettings.SortPropertyName);
                });

            if (sortColumn == null)
            {
                return;
            }

            UpdateColumnSorting(sortColumn, new SortDescription(userSettings.SortPropertyName, userSettings.SortDirection));
        }

        private void SortByColumn(GridViewColumnHeader sortColumn)
        {
            var sortDescription = new SortDescription();
            sortDescription.PropertyName = SortableColumnHeaderAttachedProperties.GetSortPropertyName(sortColumn);
            var sortDir = SortableColumnHeaderAttachedProperties.GetSortDirectionProperty(sortColumn);

            sortDescription.Direction = sortDir == null
                ? ListSortDirection.Ascending
                : sortDir == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;

            UpdateColumnSorting(sortColumn, sortDescription);
        }

        private void UpdateColumnSorting(GridViewColumnHeader sortColumn, SortDescription sortDescription)
        {
            // Add new sort description
            _projectList.Items.SortDescriptions.Clear();
            _projectList.Items.SortDescriptions.Add(sortDescription);

            // Upate sorting info of the column to sort on
            SortableColumnHeaderAttachedProperties.SetSortDirectionProperty(obj: sortColumn, value: sortDescription.Direction);

            // Clear sort direction of other columns and update automation properties on all columns
            foreach (var column in _sortableColumns)
            {
                if (column == sortColumn)
                {
                    UpdateHeaderAutomationProperties(column);
                    continue;
                }

                SortableColumnHeaderAttachedProperties.RemoveSortDirectionProperty(obj: column);
                UpdateHeaderAutomationProperties(column);
            }
        }

        private void UpdateHeaderAutomationProperties(GridViewColumnHeader columnHeader)
        {
            var sortDir = SortableColumnHeaderAttachedProperties.GetSortDirectionProperty(columnHeader);
            string oldHelpText = AutomationProperties.GetHelpText(columnHeader);
            string newHelpText;
            if (sortDir == ListSortDirection.Ascending)
            {
                newHelpText = Resx.Resources.Accessibility_ColumnSortedAscendingHelpText;
            }
            else if (sortDir == ListSortDirection.Descending)
            {
                newHelpText = Resx.Resources.Accessibility_ColumnSortedDescendingHelpText;
            }
            else
            {
                newHelpText = Resx.Resources.Accessibility_ColumnNotSortedHelpText;
            }

            AutomationProperties.SetHelpText(columnHeader, newHelpText);
            var peer = UIElementAutomationPeer.FromElement(columnHeader);
            peer?.RaisePropertyChangedEvent(AutomationElementIdentifiers.HelpTextProperty, oldHelpText, newHelpText);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => CheckBoxSelectProjectsAsync(select: true))
                .PostOnFailure(nameof(SolutionView), nameof(CheckBox_Checked));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(() => CheckBoxSelectProjectsAsync(select: false))
                  .PostOnFailure(nameof(SolutionView), nameof(CheckBox_Unchecked));
        }

        private async Task CheckBoxSelectProjectsAsync(bool select)
        {
            var model = DataContext as PackageSolutionDetailControlModel;
            await model.SelectAllProjectsAsync(select);
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // adjust the width of the "project" column so that it takes
            // up all remaining width.
            var gridView = (GridView)_projectList.View;
            var width = _projectList.ActualWidth - 8;

            // If there are more than 7 projects the scrollbar will be visible
            if (_projectList.Items.Count > 7)
            {
                width = _projectList.ActualWidth - 3 * 8;
            }

            foreach (var column in gridView.Columns)
            {
                var header = (GridViewColumnHeader)column.Header;
                if (header.Name == "_installedVersionColumnHeader" && header.ActualWidth < 86)
                {
                    width -= 38;
                }
                width -= header.ActualWidth;
            }

            var newWidth = _projectColumnHeader.ActualWidth + width;
            if (newWidth > 0)
            {
                _projectColumn.SetValue(Canvas.WidthProperty, newWidth);
            }

            // this width adjustment is only done once.
            _projectList.SizeChanged -= ListView_SizeChanged;
        }

        private void SortableColumnHeader_SizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            // GridViewColumnHeader doesn't handle setting minwidth very well so we prevent it here.
            byte columnMinWidth = 60;
            string columnName = (sizeChangedEventArgs.Source as GridViewColumnHeader)?.Name;

            //"Installed" is a bit wider and can clip when the sorting indicator is applied.
            if (columnName == "_packageLevelColumnHeader")
            {
                columnMinWidth = 96;
            }
            if (columnName == "_installedVersionColumnHeader")
            {
                if (_warningIndicatorInstalledHeader.Visibility == Visibility.Visible)
                {
                    columnMinWidth = 84;
                }
                else
                {
                    columnMinWidth = 68;
                }
            }
            if (sizeChangedEventArgs.NewSize.Width <= columnMinWidth)
            {
                sizeChangedEventArgs.Handled = true;
                ((GridViewColumnHeader)sender).Column.Width = columnMinWidth;
            }
        }

        private void ProjectList_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            // toggle the selection state when user presses the space bar when focus is on the ListViewItem
            var packageInstallationInfo = _projectList.SelectedItem as PackageInstallationInfo;
            if (packageInstallationInfo != null
                && e.Key == Key.Space
                && ((ListViewItem)(_projectList.ItemContainerGenerator.ContainerFromItem(_projectList.SelectedItem))).IsFocused)
            {
                packageInstallationInfo.IsSelected = !packageInstallationInfo.IsSelected;
                e.Handled = true;
            }
        }

        private void SortableColumnHeader_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            var sortableColumnHeader = sender as GridViewColumnHeader;
            if (sortableColumnHeader != null && (e.Key == Key.Space || e.Key == Key.Enter))
            {
                SortByColumn(sortableColumnHeader);
                e.Handled = true;
            }
        }

        private GridViewColumn CreateRequestedVersionColumn()
        {
            //The header for this column is always created so that saved sorting settings can be restored at initialization time.
            //Now we really need this column, so add necessary properties.
            _requestedVersionColumn.SetValue(System.Windows.Automation.AutomationProperties.NameProperty, Resx.Resources.ColumnHeader_Requested);
            _requestedVersionColumn.Name = "_versionColumnHeader";
            _requestedVersionColumn.Click += ColumnHeader_Clicked;
            _requestedVersionColumn.Content = Resx.Resources.ColumnHeader_Requested;
            _requestedVersionColumn.HorizontalContentAlignment = HorizontalAlignment.Left;
            _requestedVersionColumn.SizeChanged += SortableColumnHeader_SizeChanged;
            _requestedVersionColumn.PreviewKeyUp += SortableColumnHeader_PreviewKeyUp;
            _requestedVersionColumn.Focusable = true;
            _requestedVersionColumn.IsTabStop = true;
            _requestedVersionColumn.SetResourceReference(FocusVisualStyleProperty, "ControlsFocusVisualStyle");

            var versionColumn = new GridViewColumn()
            {
                DisplayMemberBinding = new Binding("RequestedVersion"),
                Header = _requestedVersionColumn
            };

            return versionColumn;
        }

        private void ItemCheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            var itemCheckBox = sender as CheckBox;
            var itemContainer = itemCheckBox?.FindAncestor<ListViewItem>();
            if (itemContainer is null)
            {
                return;
            }

            var newValue = (e.RoutedEvent == CheckBox.CheckedEvent);
            var oldValue = !newValue; // Assume the state has actually toggled.
            AutomationPeer peer = UIElementAutomationPeer.FromElement(itemContainer);
            peer?.RaisePropertyChangedEvent(
                TogglePatternIdentifiers.ToggleStateProperty,
                oldValue ? ToggleState.On : ToggleState.Off,
                newValue ? ToggleState.On : ToggleState.Off);
        }

        private void Versions_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                    _versions.IsDropDownOpen = false;
                    base.OnPreviewKeyDown(e);
                    break;
                default:
                    base.OnPreviewKeyDown(e);
                    break;
            }
        }

        private void WarningIndicatorInstalledHeader_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // adjust the width of the "installed" and "project" column depending on the visibility of the warning indicator.
            if (_warningIndicatorInstalledHeader.Visibility == Visibility.Visible)
            {
                _installedVersionColumnHeader.Column.Width = 84;
                _projectColumn.Width -= 16;
            }
            else
            {
                _installedVersionColumnHeader.Column.Width = 68;
                _projectColumn.Width += 16;
            }
        }
    }
}
