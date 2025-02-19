// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Markdown.Platform;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.UI.ViewModels;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Interaction logic for PackageReadmeControl.xaml
    /// </summary>
    public partial class PackageReadmeControl : UserControl, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private IMarkdownPreview _markdownPreview;
#pragma warning restore CS0618 // Type or member is obsolete
        private bool _isDisposed = false;
        private CancellationTokenSource _markdownRenderingCancellationTokenSource;
        private bool _isBusy = true;

        public PackageReadmeControl()
        {
            InitializeComponent();
            _markdownRenderingCancellationTokenSource = new CancellationTokenSource();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Added to allow the view to display the loading spinner while the markdown is being rendered.
        /// The markdown rendering occurs on the view so the view should have some control over this property.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy || ReadmeViewModel.IsBusy;
        }

        /// <summary>
        /// Added to allow the view to hide the README while the markdown is being rendered
        /// The markdown rendering occurs on the view so the view should have some control over this property.
        /// </summary>
        public bool IsReadmeReady
        {
            get => !_isBusy && ReadmeViewModel.IsReadmeReady;
        }

        public ReadmePreviewViewModel ReadmeViewModel { get => (ReadmePreviewViewModel)DataContext; }

        public bool Initialize(IEditorOptionsFactoryService options)
        {
            if (options is null)
            {
                return false;
            }
            if (_markdownPreview is null)
            {
                // This class is marked as obsolete because the api hasn't been finalized, however we want to use IMarkdownPreview to maintain a centralized way of rendering markdown into html.
#pragma warning disable CS0618 // Type or member is obsolete
                var previewBuilder = new PreviewBuilder();
#pragma warning restore CS0618 // Type or member is obsolete
                previewBuilder.EditorOptions = options.GlobalOptions;
                previewBuilder.IsVsToolWindow = true;
                _markdownPreview = previewBuilder.Build();
                descriptionMarkdownPreview.Content = _markdownPreview?.VisualElement;
            }
            return true;
        }

        private void ReadmeViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReadmePreviewViewModel.ReadmeMarkdown))
            {
                if (!string.IsNullOrWhiteSpace(ReadmeViewModel.ReadmeMarkdown))
                {
                    CancelAndExchangeToken();
                    UpdateMarkdownAsync(ReadmeViewModel.ReadmeMarkdown, _markdownRenderingCancellationTokenSource.Token).PostOnFailure(nameof(PackageReadmeControl));
                }
            }
            if (e.PropertyName == nameof(ReadmePreviewViewModel.IsBusy))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
            if (e.PropertyName == nameof(ReadmePreviewViewModel.IsReadmeReady))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsReadmeReady)));
            }
        }

        private async Task UpdateMarkdownAsync(string markdown, CancellationToken token)
        {
            UpdateBusy(true);
            if (_markdownPreview is not null)
            {
                await TaskScheduler.Default;
                var success = await _markdownPreview.UpdateContentAsync(markdown, ScrollHint.None, token).PostOnFailureAsync(nameof(PackageReadmeControl));
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(token);
                if (!success)
                {
                    ReadmeViewModel.ErrorWithReadme = true;
                    ReadmeViewModel.ReadmeMarkdown = string.Empty;
                }
            }
            UpdateBusy(false);
        }

        private void UpdateBusy(bool isBusy)
        {
            _isBusy = isBusy;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsReadmeReady)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ReadmePreviewViewModel oldMetadata)
            {
                CancelAndExchangeToken();
                UpdateMarkdownAsync("", _markdownRenderingCancellationTokenSource.Token).PostOnFailure(nameof(PackageReadmeControl));
                oldMetadata.PropertyChanged -= ReadmeViewModel_PropertyChanged;
            }
            if (ReadmeViewModel is not null)
            {
                ReadmeViewModel.PropertyChanged += ReadmeViewModel_PropertyChanged;
            }
        }

        private void PackageReadmeControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ReadmeViewModel.ReadmeMarkdown))
            {
                CancelAndExchangeToken();
                UpdateMarkdownAsync(ReadmeViewModel.ReadmeMarkdown, _markdownRenderingCancellationTokenSource.Token).PostOnFailure(nameof(PackageReadmeControl));
            }
        }

        private void CancelAndExchangeToken()
        {
            var newToken = new CancellationTokenSource();
            var oldSource = Interlocked.Exchange(ref _markdownRenderingCancellationTokenSource, newToken);
            oldSource?.Cancel();
            oldSource?.Dispose();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _markdownRenderingCancellationTokenSource.Cancel();
                _markdownRenderingCancellationTokenSource.Dispose();
                _markdownPreview?.Dispose();
            }
        }
    }
}
