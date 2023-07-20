// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Runtime.Caching;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.UI.Utility;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using Resx = NuGet.PackageManagement.UI.Resources;

namespace NuGet.PackageManagement.UI
{
    // This is the model class behind the package items in the infinite scroll list.
    // Some of its properties, such as Latest Version, Status, are fetched on-demand in the background.
    public sealed class PackageItemViewModel : INotifyPropertyChanged, ISelectableItem, IDisposable
    {
        internal const int DecodePixelWidth = 32;

        private readonly CancellationTokenSource _cancellationTokenSource;

        public PackageItemViewModel(IReconnectingNuGetSearchService searchService)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _searchService = searchService;
        }

        // same URIs can reuse the bitmapImage that we've already used.
        private static readonly ObjectCache BitmapImageCache = MemoryCache.Default;

        private static readonly RequestCachePolicy RequestCacheIfAvailable = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);

        private static readonly ErrorFloodGate ErrorFloodGate = new ErrorFloodGate();

        private IReconnectingNuGetSearchService _searchService;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }

        public NuGetVersion Version { get; set; }

        public VersionRange AllowedVersions { get; set; }

        public VersionRange VersionOverride { get; set; }

        public IReadOnlyCollection<PackageSourceContextInfo> Sources { get; set; }

        public bool IncludePrerelease { get; set; }

        private string _author;
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
                OnPropertyChanged(nameof(ByAuthor));
            }
        }

        public string ByAuthor
        {
            get
            {
                return _author != null ? string.Format(CultureInfo.CurrentCulture, Resx.Text_ByAuthor, _author) : null;
            }
        }

        /// <summary>
        /// The installed version of the package.
        /// </summary>
        private NuGetVersion _installedVersion;
        public NuGetVersion InstalledVersion
        {
            get
            {
                return _installedVersion;
            }
            set
            {
                if (!VersionEquals(_installedVersion, value))
                {
                    _installedVersion = value;
                    OnPropertyChanged(nameof(InstalledVersion));
                    OnPropertyChanged(nameof(IsInstalledAndTransitive));
                    OnPropertyChanged(nameof(IsLatestInstalled));

                    // update tool tip
                    if (_installedVersion != null)
                    {
                        var displayVersion = new DisplayVersion(_installedVersion, string.Empty);
                        InstalledVersionToolTip = string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ToolTip_InstalledVersion,
                            displayVersion);
                    }
                    else
                    {
                        InstalledVersionToolTip = null;
                    }
                }
            }
        }

        /// <summary>
        /// The version that can be installed or updated to. It is null
        /// if the installed version is already the latest.
        /// </summary>
        private NuGetVersion _latestVersion;
        public NuGetVersion LatestVersion
        {
            get
            {
                return _latestVersion;
            }
            set
            {
                if (!VersionEquals(_latestVersion, value))
                {
                    _latestVersion = value;
                    OnPropertyChanged(nameof(IsNotInstalled));
                    OnPropertyChanged(nameof(IsUpdateAvailable));
                    OnPropertyChanged(nameof(LatestVersion));
                    OnPropertyChanged(nameof(IsUninstalledOrTransitive));

                    // update tool tip
                    if (_latestVersion != null)
                    {
                        var displayVersion = new DisplayVersion(_latestVersion, string.Empty);
                        string toolTipText = PackageLevel == PackageLevel.Transitive ? Resources.ToolTip_TransitiveDependencyVersion : Resources.ToolTip_LatestVersion;
                        LatestVersionToolTip = string.Format(
                            CultureInfo.CurrentCulture,
                            toolTipText,
                            displayVersion);
                    }
                    else
                    {
                        LatestVersionToolTip = null;
                    }
                }
            }
        }

        /// <summary>
        /// True if the package is AutoReferenced
        /// </summary>
        private bool _autoReferenced;
        public bool AutoReferenced
        {
            get
            {
                return _autoReferenced;
            }
            set
            {
                _autoReferenced = value;
                OnPropertyChanged(nameof(AutoReferenced));
            }
        }

        private string _installedVersionToolTip;

        public string InstalledVersionToolTip
        {
            get
            {
                return _installedVersionToolTip;
            }
            set
            {
                _installedVersionToolTip = value;
                OnPropertyChanged(nameof(InstalledVersionToolTip));
            }
        }

        private string _latestVersionToolTip;

        public string LatestVersionToolTip
        {
            get
            {
                return _latestVersionToolTip;
            }
            set
            {
                _latestVersionToolTip = value;
                OnPropertyChanged(nameof(LatestVersionToolTip));
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        private bool VersionEquals(NuGetVersion v1, NuGetVersion v2)
        {
            if (v1 == null && v2 == null)
            {
                return true;
            }

            if (v1 == null)
            {
                return false;
            }

            return v1.Equals(v2, VersionComparison.Default);
        }

        private long? _downloadCount;

        public long? DownloadCount
        {
            get
            {
                return _downloadCount;
            }
            set
            {
                _downloadCount = value;
                OnPropertyChanged(nameof(DownloadCount));
            }
        }

        public string Summary { get; set; }

        private PackageStatus _status;
        public PackageStatus Status
        {
            get
            {
                return _status;
            }

            private set
            {
                bool refresh = _status != value;
                _status = value;

                if (refresh)
                {
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(IsLatestInstalled));
                    OnPropertyChanged(nameof(IsUpdateAvailable));
                    OnPropertyChanged(nameof(IsUninstallable));
                    OnPropertyChanged(nameof(IsNotInstalled));
                    OnPropertyChanged(nameof(IsUninstalledOrTransitive));
                }
            }
        }

        // If the values that help calculate this property change, make sure you raise OnPropertyChanged for IsNotInstalled
        // in all those properties.
        public bool IsNotInstalled
        {
            get
            {
                return (Status == PackageStatus.NotInstalled && LatestVersion != null);
            }
        }

        public bool IsUninstalledOrTransitive => (Status == PackageStatus.NotInstalled && LatestVersion != null) || PackageLevel == PackageLevel.Transitive;

        public bool IsInstalledAndTransitive => PackageLevel == PackageLevel.Transitive || InstalledVersion != null;

        // If the values that help calculate this property change, make sure you raise OnPropertyChanged for IsUninstallable
        // in all those properties.
        public bool IsUninstallable
        {
            get
            {
                return (Status == PackageStatus.Installed || Status == PackageStatus.UpdateAvailable);
            }
        }

        // If the values that help calculate this property change, make sure you raise OnPropertyChanged for IsLatestInstalled
        // in all those properties.
        public bool IsLatestInstalled
        {
            get
            {
                return (Status == PackageStatus.Installed && InstalledVersion != null);
            }
        }

        // If the values that help calculate this property change, make sure you raise OnPropertyChanged for IsUpdateAvailable
        // in all those properties.
        public bool IsUpdateAvailable
        {
            get
            {
                return (Status == PackageStatus.UpdateAvailable && LatestVersion != null);
            }
        }

        private bool _recommended;
        public bool Recommended
        {
            get { return _recommended; }
            set
            {
                if (_recommended != value)
                {
                    _recommended = value;
                    OnPropertyChanged(nameof(Recommended));
                }
            }
        }

        private (string modelVersion, string vsixVersion)? _recommenderVersion;
        public (string modelVersion, string vsixVersion)? RecommenderVersion
        {
            get { return _recommenderVersion; }
            set
            {
                _recommenderVersion = value;
                OnPropertyChanged(nameof(RecommenderVersion));
            }
        }

        private bool _prefixReserved;
        public bool PrefixReserved
        {
            get { return _prefixReserved; }
            set
            {
                if (_prefixReserved != value)
                {
                    _prefixReserved = value;
                    OnPropertyChanged(nameof(PrefixReserved));
                }
            }
        }

        private bool _isPackageDeprecated;
        public bool IsPackageDeprecated
        {
            get { return _isPackageDeprecated; }
            set
            {
                if (_isPackageDeprecated != value)
                {
                    _isPackageDeprecated = value;
                    OnPropertyChanged(nameof(IsPackageDeprecated));
                    OnPropertyChanged(nameof(IsPackageWithWarnings));
                }
            }
        }

        public bool IsPackageVulnerable
        {
            get => VulnerabilityMaxSeverity > -1;
        }

        private int _vulnerabilityMaxSeverity = -1;
        public int VulnerabilityMaxSeverity
        {
            get { return _vulnerabilityMaxSeverity; }
            set
            {
                if (_vulnerabilityMaxSeverity != value)
                {
                    _vulnerabilityMaxSeverity = value;
                    OnPropertyChanged(nameof(VulnerabilityMaxSeverity));
                    OnPropertyChanged(nameof(IsPackageVulnerable));
                    OnPropertyChanged(nameof(IsPackageWithWarnings));
                }
            }
        }

        public bool IsPackageWithWarnings
        {
            get => IsPackageDeprecated || IsPackageVulnerable;
        }

        private bool _isPackageWithNetworkErrors;
        public bool IsPackageWithNetworkErrors
        {
            get => _isPackageWithNetworkErrors;
            set
            {
                if (IsPackageWithNetworkErrors != value)
                {
                    _isPackageWithNetworkErrors = value;
                    OnPropertyChanged(nameof(IsPackageWithNetworkErrors));
                }
            }
        }

        private Uri _iconUrl;
        public Uri IconUrl
        {
            get { return _iconUrl; }
            set
            {
                _iconUrl = value;
                OnPropertyChanged(nameof(IconUrl));
            }
        }

        private IconBitmapStatus _bitmapStatus;

        public IconBitmapStatus BitmapStatus
        {
            get { return _bitmapStatus; }
            set
            {
                if (_bitmapStatus != value)
                {
                    _bitmapStatus = value;
                    OnPropertyChanged(nameof(BitmapStatus));
                }
            }
        }

        private BitmapSource _iconBitmap;
        public BitmapSource IconBitmap
        {
            get
            {
                if (_iconBitmap == null)
                {
                    if (BitmapStatus == IconBitmapStatus.None)
                    {
                        (BitmapSource iconBitmap, IconBitmapStatus nextStatus) = GetInitialIconBitmapAndStatus();

                        BitmapStatus = nextStatus;
                        _iconBitmap = iconBitmap;
                        if (BitmapStatus == IconBitmapStatus.NeedToFetch)
                        {
                            BitmapStatus = IconBitmapStatus.Fetching;
                            NuGetUIThreadHelper.JoinableTaskFactory
                                .RunAsync(FetchIconAsync)
                                .PostOnFailure(nameof(PackageItemViewModel), nameof(IconBitmap));
                        }
                    }
                }

                return _iconBitmap;
            }
            set
            {
                if (_iconBitmap != value)
                {
                    _iconBitmap = value;
                    OnPropertyChanged(nameof(IconBitmap));
                }
            }
        }

        private string _transitiveToolTipMessage;

        public string TransitiveToolTipMessage
        {
            get => _transitiveToolTipMessage;
            set
            {
                if (_transitiveToolTipMessage != value)
                {
                    _transitiveToolTipMessage = value;
                    OnPropertyChanged(nameof(TransitiveToolTipMessage));
                }
            }
        }

        private PackageLevel _packageLevel;
        public PackageLevel PackageLevel
        {
            get => _packageLevel;
            set
            {
                if (_packageLevel != value)
                {
                    _packageLevel = value;
                    OnPropertyChanged(nameof(PackageLevel));
                    OnPropertyChanged(nameof(IsUninstalledOrTransitive));
                    OnPropertyChanged(nameof(IsInstalledAndTransitive));
                }
            }
        }

        public async Task<IReadOnlyCollection<VersionInfoContextInfo>> GetVersionsAsync()
        {
            var identity = new PackageIdentity(Id, Version);
            var isTransitive = PackageLevel == PackageLevel.Transitive;
            return await _searchService.GetPackageVersionsAsync(identity, Sources, IncludePrerelease, isTransitive, _cancellationTokenSource.Token);
        }

        public async Task<IReadOnlyCollection<VersionInfoContextInfo>> GetVersionsAsync(IEnumerable<IProjectContextInfo> projects)
        {
            var identity = new PackageIdentity(Id, Version);
            var isTransitive = PackageLevel == PackageLevel.Transitive;
            return await _searchService.GetPackageVersionsAsync(identity, Sources, IncludePrerelease, isTransitive, projects, _cancellationTokenSource.Token);
        }

        // This Lazy/AsyncLazy is just because DetailControlModel calls GetDetailedPackageSearchMetadataAsync directly,
        // and there are tests that don't mock IServiceBroker and INuGetSearchService. It's called via a jtf.RunAsync that is
        // not awaited. By keeping this AsyncLazy, we ensure that the exception is thrown in an async continuation. Whereas
        // if we get rid of it and have GetDetailedPackageSearchMetadataAsync call _searchService directly, then the exception
        // will not be thrown in a continuation, and the test will fail.
        private Lazy<Task<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo)>> _detailedPackageSearchMetadata =>
            new Common.AsyncLazy<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo)>(async () =>
            {
                var identity = new PackageIdentity(Id, Version);
                return await _searchService.GetPackageMetadataAsync(identity, Sources, IncludePrerelease, _cancellationTokenSource.Token);
            });
        public Task<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo)> GetDetailedPackageSearchMetadataAsync()
        {
            return _detailedPackageSearchMetadata.Value;
        }

        private PackageDeprecationMetadataContextInfo _deprecationMetadata;
        public PackageDeprecationMetadataContextInfo DeprecationMetadata
        {
            get => _deprecationMetadata;
            set
            {
                if (_deprecationMetadata != value)
                {
                    _deprecationMetadata = value;
                    OnPropertyChanged(nameof(DeprecationMetadata));
                }
            }
        }

        public IEnumerable<PackageVulnerabilityMetadataContextInfo> Vulnerabilities { get; set; }

        private (BitmapSource, IconBitmapStatus) GetInitialIconBitmapAndStatus()
        {
            BitmapSource imageBitmap = null;
            IconBitmapStatus status;

            if (IconUrl == null)
            {
                imageBitmap = Images.DefaultPackageIcon;
                status = IconBitmapStatus.DefaultIcon;
            }
            else if (!IconUrl.IsAbsoluteUri)
            {
                imageBitmap = Images.DefaultPackageIcon;
                status = IconBitmapStatus.DefaultIconDueToRelativeUri;
            }
            else
            {
                string cacheKey = GenerateKeyFromIconUri(IconUrl);
                var cachedBitmapImage = BitmapImageCache.Get(cacheKey) as BitmapSource;
                if (cachedBitmapImage != null)
                {
                    imageBitmap = cachedBitmapImage;
                    status = IconBitmapStatus.MemoryCachedIcon;
                }
                else
                {
                    // Some people run on networks with internal NuGet feeds, but no access to the package images on the internet.
                    // This is meant to detect that kind of case, and stop spamming the network, so the app remains responsive.
                    if (ErrorFloodGate.HasTooManyNetworkErrors)
                    {
                        imageBitmap = Images.DefaultPackageIcon;
                        status = IconBitmapStatus.DefaultIconDueToNullStream;
                    }
                    else
                    {
                        imageBitmap = Images.DefaultPackageIcon;
                        status = IconBitmapStatus.NeedToFetch;
                    }
                }
            }

            return (imageBitmap, status);
        }

        private static bool IsHandleableBitmapEncodingException(Exception ex)
        {
            return ex is ArgumentException ||
                ex is COMException ||
                ex is FileFormatException ||
                ex is InvalidOperationException ||
                ex is NotSupportedException ||
                ex is OutOfMemoryException ||
                ex is IOException ||
                ex is UnauthorizedAccessException;
        }

        private async Task FetchIconAsync()
        {
            await TaskScheduler.Default;

            Assumes.NotNull(IconUrl);

            using (Stream stream = await PackageFileService.GetPackageIconAsync(new PackageIdentity(Id, Version), CancellationToken.None))
            {
                if (stream != null)
                {
                    var iconBitmapImage = new BitmapImage();
                    iconBitmapImage.BeginInit();

                    // BitmapImage can download on its own from URIs, but in order
                    // to support downloading on a worker thread, we need to download the image
                    // data and put into a memorystream. Then have the BitmapImage decode the
                    // image from the memorystream.
                    using (var memoryStream = new MemoryStream())
                    {
                        // Cannot call CopyToAsync as we'll get an InvalidOperationException due to CheckAccess() in next line.
                        stream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        iconBitmapImage.StreamSource = memoryStream;

                        try
                        {
                            FinalizeBitmapImage(iconBitmapImage);
                            iconBitmapImage.Freeze();
                            IconBitmap = iconBitmapImage;
                            BitmapStatus = IconBitmapStatus.FetchedIcon;
                        }
                        catch (Exception ex) when (IsHandleableBitmapEncodingException(ex))
                        {
                            IconBitmap = Images.DefaultPackageIcon;
                            BitmapStatus = IconBitmapStatus.DefaultIconDueToDecodingError;
                        }
                    }
                }
                else
                {
                    ErrorFloodGate.ReportBadNetworkError();
                    if (BitmapStatus == IconBitmapStatus.Fetching)
                    {
                        BitmapStatus = IconBitmapStatus.DefaultIconDueToNullStream;
                    }
                }

                ErrorFloodGate.ReportAttempt();

                if (IconBitmap != null)
                {
                    string cacheKey = GenerateKeyFromIconUri(IconUrl);
                    AddToCache(cacheKey, IconBitmap);
                }
            }
        }

        private static void FinalizeBitmapImage(BitmapImage iconBitmapImage)
        {
            // Default cache policy: Per MSDN, satisfies a request for a resource either by using the cached copy of the resource or by sending a request
            // for the resource to the server. The action taken is determined by the current cache policy and the age of the content in the cache.
            // This is the cache level that should be used by most applications.
            iconBitmapImage.UriCachePolicy = RequestCacheIfAvailable;

            // Instead of scaling larger images and keeping larger image in memory, this makes it so we scale it down, and throw away the bigger image.
            // Only need to set this on one dimension, to preserve aspect ratio
            iconBitmapImage.DecodePixelWidth = DecodePixelWidth;

            // Workaround for https://github.com/dotnet/wpf/issues/3503
            iconBitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;

            iconBitmapImage.CacheOption = BitmapCacheOption.OnLoad;

            iconBitmapImage.EndInit();
        }

        private static string GenerateKeyFromIconUri(Uri iconUrl)
        {
            return iconUrl == null ? string.Empty : iconUrl.ToString();
        }

        private static void AddToCache(string cacheKey, BitmapSource iconBitmapImage)
        {
            var policy = new CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromMinutes(10),
            };
            BitmapImageCache.Set(cacheKey, iconBitmapImage, policy);
        }

        private async System.Threading.Tasks.Task ReloadPackageVersionsAsync()
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            try
            {
                IReadOnlyCollection<VersionInfoContextInfo> packageVersions = await GetVersionsAsync();

                // filter package versions based on allowed versions in packages.config
                packageVersions = packageVersions.Where(v => AllowedVersions.Satisfies(v.Version)).ToList();
                NuGetVersion result = packageVersions
                    .Select(p => p.Version)
                    .MaxOrDefault();

                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                LatestVersion = result;
                Status = GetPackageStatus(LatestVersion, InstalledVersion, AutoReferenced);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // UI requested cancellation
            }
            catch (TaskCanceledException)
            {
                // HttpClient throws TaskCanceledExceptions for HTTP timeouts
                try
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
                    IsPackageWithNetworkErrors = true;
                }
                catch (OperationCanceledException)
                {
                    // if cancellationToken cancelled before the above is scheduled on UI thread, don't log fault telemetry
                }
            }
        }

        private async Task ReloadPackageMetadataAsync()
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            try
            {
                var identity = new PackageIdentity(Id, Version);
                (PackageSearchMetadataContextInfo packageMetadata, PackageDeprecationMetadataContextInfo deprecationMetadata) =
                    await _searchService.GetPackageMetadataAsync(identity, Sources, IncludePrerelease, cancellationToken);

                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                DeprecationMetadata = deprecationMetadata;
                IsPackageDeprecated = deprecationMetadata != null;
                VulnerabilityMaxSeverity = packageMetadata?.Vulnerabilities?.FirstOrDefault()?.Severity ?? -1;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // UI requested cancellation.
            }
            catch (TaskCanceledException)
            {
                // HttpClient throws TaskCanceledExceptions for HTTP timeouts
                try
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
                    IsPackageWithNetworkErrors = true;
                }
                catch (OperationCanceledException)
                {
                    // if cancellationToken cancelled before the above is scheduled on UI thread, don't log fault telemetry
                }
            }
        }

        public void UpdatePackageStatus(IEnumerable<PackageCollectionItem> installedPackages)
        {
            // Get the maximum version installed in any target project/solution
            InstalledVersion = installedPackages
                .GetPackageVersions(Id)
                .MaxOrDefault();

            // Set auto referenced to true any reference for the given id contains the flag.
            AutoReferenced = installedPackages.IsAutoReferenced(Id);

            NuGetUIThreadHelper.JoinableTaskFactory
                .RunAsync(ReloadPackageVersionsAsync)
                .PostOnFailure(nameof(PackageItemViewModel), nameof(ReloadPackageVersionsAsync));

            NuGetUIThreadHelper.JoinableTaskFactory
                .RunAsync(ReloadPackageMetadataAsync)
                .PostOnFailure(nameof(PackageItemViewModel), nameof(ReloadPackageMetadataAsync));

            OnPropertyChanged(nameof(Status));
        }

        public void UpdateTransitivePackageStatus(NuGetVersion installedVersion)
        {
            InstalledVersion = installedVersion ?? throw new ArgumentNullException(nameof(installedVersion)); ;

            // Transitive packages cannot be updated and can only be installed as top-level packages with their currently installed version.
            LatestVersion = installedVersion;

            OnPropertyChanged(nameof(Status));
        }

        private static PackageStatus GetPackageStatus(
            NuGetVersion latestAvailableVersion,
            NuGetVersion installedVersion,
            bool autoReferenced)
        {
            var status = PackageStatus.NotInstalled;

            if (autoReferenced)
            {
                status = PackageStatus.AutoReferenced;
            }
            else if (installedVersion != null)
            {
                status = PackageStatus.Installed;

                if (VersionComparer.VersionRelease.Compare(installedVersion, latestAvailableVersion) < 0)
                {
                    status = PackageStatus.UpdateAvailable;
                }
            }

            return status;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string PackagePath { get; set; }
        public INuGetPackageFileService PackageFileService { get; internal set; }

        public override string ToString()
        {
            return Id;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            // Don't dispose _searchService. It's a shared instance.
        }
    }
}
