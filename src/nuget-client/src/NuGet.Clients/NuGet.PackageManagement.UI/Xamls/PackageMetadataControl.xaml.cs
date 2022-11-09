// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.UI
{

    /// <summary>
    /// Interaction logic for PackageMetadataControl.xaml
    /// </summary>
    public partial class PackageMetadataControl : UserControl
    {
        public PackageMetadataControl()
        {
            InitializeComponent();

            Visibility = Visibility.Collapsed;
            DataContextChanged += PackageMetadataControl_DataContextChanged;
        }

        private void ViewLicense_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DetailedPackageMetadata metadata)
            {
                var window = new LicenseFileWindow()
                {
                    DataContext = new LicenseFileData
                    {
                        LicenseHeader = string.Format(CultureInfo.CurrentCulture, UI.Resources.WindowTitle_LicenseFileWindow, metadata.Id),
                        LicenseText = new FlowDocument(new Paragraph(new Run(UI.Resources.LicenseFile_Loading)))
                    }
                };

                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    string content = await PackageLicenseUtilities.GetEmbeddedLicenseAsync(new Packaging.Core.PackageIdentity(metadata.Id, metadata.Version), CancellationToken.None);

                    var flowDoc = new FlowDocument();
                    flowDoc.Blocks.AddRange(PackageLicenseUtilities.GenerateParagraphs(content));
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    (window.DataContext as LicenseFileData).LicenseText = flowDoc;
                }).PostOnFailure(nameof(PackageMetadataControl), nameof(ViewLicense_Click));

                window.ShowModal();
            }
        }

        private void PackageMetadataControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is DetailedPackageMetadata)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        // capture each item as it is selected, so we can unselect when treeview lostfocus
        private void OnItemSelected(object sender, RoutedEventArgs e)
        {
            _dependencies.Tag = e.OriginalSource;
        }

        private void TreeView_LostFocus(object sender, RoutedEventArgs e)
        {
            // hide focus highlight when treeview lostfocus
            if (_dependencies.SelectedItem != null)
            {
                TreeViewItem selectedTVI = _dependencies.Tag as TreeViewItem;
                if (selectedTVI != null)
                {
                    selectedTVI.IsSelected = false;
                }
            }
        }
    }
}
