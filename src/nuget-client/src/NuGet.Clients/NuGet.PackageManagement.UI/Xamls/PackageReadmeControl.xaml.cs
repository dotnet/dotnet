// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Markdown.Platform;
using Microsoft.VisualStudio.Shell;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for PackageReadmeControl.xaml
    /// </summary>
    public partial class PackageReadmeControl : UserControl
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private IMarkdownPreview _markdownPreview;
#pragma warning restore CS0618 // Type or member is obsolete

        public PackageReadmeControl()
        {
            InitializeComponent();
            _markdownPreview = MarkdownPreviewSingleton.GetInstance();
            descriptionMarkdownPreview.Content = _markdownPreview.VisualElement;
        }

        public ReadmePreviewViewModel ReadmeViewModel { get => (ReadmePreviewViewModel)DataContext; }

        private void ReadmeViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReadmePreviewViewModel.ReadmeMarkdown))
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(UpdateMarkdownAsync).PostOnFailure(nameof(PackageReadmeControl), nameof(ReadmeViewModel_PropertyChanged));
            }
        }

        private async Task UpdateMarkdownAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ReadmeViewModel.ReadmeMarkdown))
                {
                    await _markdownPreview.UpdateContentAsync(ReadmeViewModel.ReadmeMarkdown, ScrollHint.None);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                ReadmeViewModel.ErrorWithReadme = true;
                ReadmeViewModel.ReadmeMarkdown = string.Empty;
                await TelemetryUtility.PostFaultAsync(ex, nameof(ReadmePreviewViewModel));
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ReadmePreviewViewModel oldMetadata)
            {
                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await _markdownPreview.UpdateContentAsync("", ScrollHint.None);
                });
                oldMetadata.PropertyChanged -= ReadmeViewModel_PropertyChanged;
            }
            if (ReadmeViewModel is not null)
            {
                ReadmeViewModel.PropertyChanged += ReadmeViewModel_PropertyChanged;
            }
        }

        private void PackageReadmeControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ReadmeViewModel is not null)
            {
                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await _markdownPreview.UpdateContentAsync("", ScrollHint.None);
                });
                ReadmeViewModel.PropertyChanged -= ReadmeViewModel_PropertyChanged;
            }
        }

        private void PackageReadmeControl_Loaded(object sender, RoutedEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(UpdateMarkdownAsync);
        }
    }
}
