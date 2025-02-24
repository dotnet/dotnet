// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.ViewModels
{
    public sealed class ReadmePreviewViewModel : TitledPageViewModelBase, IDisposable
    {
        private bool _errorWithReadme;
        private INuGetPackageFileService _nugetPackageFileService;
        private string _rawReadme;
        private DetailedPackageMetadata _packageMetadata;
        private bool _canRenderLocalReadme;
        private bool _isBusy;
        private bool _disposed;
        private CancellationTokenSource _readmeLoadingCancellationTokenSource = new CancellationTokenSource();


#pragma warning disable CS0618 // Type or member is obsolete
        public ReadmePreviewViewModel(INuGetPackageFileService packageFileService, ItemFilter itemFilter, bool isReadmeFeatureEnabled)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            _nugetPackageFileService = packageFileService ?? throw new ArgumentNullException(nameof(packageFileService));
            _canRenderLocalReadme = CanRenderLocalReadme(itemFilter);
            _nugetPackageFileService = packageFileService;
            _errorWithReadme = false;
            _isBusy = false;
            _rawReadme = string.Empty;
            _packageMetadata = null;
            Title = Resources.Label_Readme_Tab;
            IsVisible = isReadmeFeatureEnabled;
        }

        public void SetVisibility(bool isVisible)
        {
            IsVisible = isVisible;
        }

        public bool IsReadmeReady { get => !IsBusy && !ErrorWithReadme; }

        public bool ErrorWithReadme
        {
            get => _errorWithReadme;
            set
            {
                SetAndRaisePropertyChanged(ref _errorWithReadme, value);
                RaisePropertyChanged(nameof(IsReadmeReady));
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetAndRaisePropertyChanged(ref _isBusy, value);
                RaisePropertyChanged(nameof(IsReadmeReady));
            }
        }

        public string ReadmeMarkdown
        {
            get => _rawReadme;
            set => SetAndRaisePropertyChanged(ref _rawReadme, value);
        }

        public async Task ItemFilterChangedAsync(ItemFilter filter)
        {
            var oldRenderLocalReadme = _canRenderLocalReadme;
            _canRenderLocalReadme = CanRenderLocalReadme(filter);
            if (_canRenderLocalReadme != oldRenderLocalReadme)
            {
                if (_packageMetadata != null)
                {
                    var newToken = ExchangeCancellationTokenSource();
                    await LoadReadmeAsync(newToken.Token);
                }
            }
        }

        public async Task SetPackageMetadataAsync(DetailedPackageMetadata packageMetadata)
        {
            if (ShouldUpdatePackageMetadata(packageMetadata))
            {
                var newToken = ExchangeCancellationTokenSource();
                _packageMetadata = packageMetadata;
                await LoadReadmeAsync(newToken.Token);
            }
        }

        // for testing purposes
        internal bool ShouldUpdatePackageMetadata(DetailedPackageMetadata packageMetadata)
        {
            return packageMetadata != null && (
                !string.Equals(packageMetadata.Id, _packageMetadata?.Id)
                || packageMetadata.Version != _packageMetadata?.Version
                || !string.Equals(packageMetadata.ReadmeFileUrl, _packageMetadata?.ReadmeFileUrl)
                || !string.Equals(packageMetadata.PackagePath, _packageMetadata?.PackagePath)
                );
        }

        private static bool CanRenderLocalReadme(ItemFilter filter)
        {
            return filter != ItemFilter.All;
        }

        private async Task LoadReadmeAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_packageMetadata.ReadmeFileUrl))
            {
                ReadmeMarkdown = _canRenderLocalReadme && !string.IsNullOrWhiteSpace(_packageMetadata.PackagePath) ? Resources.Text_NoReadme : string.Empty;
                IsVisible = !string.IsNullOrWhiteSpace(ReadmeMarkdown);
                ErrorWithReadme = false;
                return;
            }

            var readmeUrl = new Uri(_packageMetadata.ReadmeFileUrl);
            if (!_canRenderLocalReadme && readmeUrl.IsFile)
            {
                ReadmeMarkdown = string.Empty;
                IsVisible = false;
                ErrorWithReadme = false;
                return;
            }

            var readme = Resources.Text_NoReadme;
            try
            {
                IsBusy = true;
                ErrorWithReadme = false;
                await ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await TaskScheduler.Default;
                    using var readmeStream = await _nugetPackageFileService.GetReadmeAsync(readmeUrl, cancellationToken);
                    if (readmeStream is null)
                    {
                        return;
                    }

                    using StreamReader streamReader = new StreamReader(readmeStream);
                    readme = await streamReader.ReadToEndAsync();
                });
            }
            finally
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    ReadmeMarkdown = readme;
                    IsVisible = !string.IsNullOrWhiteSpace(readme);
                    ErrorWithReadme = false;
                    IsBusy = false;
                }
            }
        }

        private CancellationTokenSource ExchangeCancellationTokenSource()
        {
            var newCts = new CancellationTokenSource();
            var oldCts = Interlocked.Exchange(ref _readmeLoadingCancellationTokenSource, newCts);
            oldCts?.Cancel();
            oldCts?.Dispose();
            return newCts;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _readmeLoadingCancellationTokenSource?.Cancel();
                    _readmeLoadingCancellationTokenSource?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
