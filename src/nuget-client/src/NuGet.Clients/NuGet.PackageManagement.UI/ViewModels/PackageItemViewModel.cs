// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.PackageManagement.UI.ViewModels;
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
        private readonly IPackageVulnerabilityService _vulnerabilityService;
        private readonly PackageModel _packageModel;
        private List<NuGetVersion> _transitiveInstalledVersions;
        private List<PackageIdentity> _transitiveOrigins;

        public PackageItemViewModel(INuGetSearchService searchService, PackageModel packageModel, IPackageVulnerabilityService vulnerabilityService = default)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _searchService = searchService;
            _vulnerabilityService = vulnerabilityService;
            _packageModel = packageModel;
            _transitiveInstalledVersions = [];
            _transitiveOrigins = [];
        }

        // same URIs can reuse the bitmapImage that we've already used.
        private static readonly ObjectCache BitmapImageCache = MemoryCache.Default;

        private static readonly RequestCachePolicy RequestCacheIfAvailable = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);

        private static readonly ErrorFloodGate ErrorFloodGate = new ErrorFloodGate();

        private INuGetSearchService _searchService;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id => _packageModel.Id;

        public NuGetVersion Version => _packageModel.Version;

        public VersionRange AllowedVersions { get; set; }

        public VersionRange VersionOverride { get; set; }

        public IReadOnlyCollection<PackageSourceContextInfo> Sources { get; set; }

        public bool IncludePrerelease { get; set; }

        public ImmutableList<KnownOwnerViewModel> KnownOwnerViewModels { get; internal set; }

        public string Owner => string.Join(", ", _packageModel.OwnersList ?? []);

        public string Author => _packageModel.Authors;

        /// <summary>
        /// When a collection of <see cref="KnownOwnerViewModels"/> is available, this property returns the <see cref="PackageSearchMetadataContextInfo.Owners"/>
        /// string which contains the package owner name(s).
        /// If the collection exists but is empty, it's treated as there being no assigned owner for this package by returning an empty string.
        /// Otherwise, when there's no collection or no Owners string, it returns null.
        /// </summary>
        private string ByOwner
        {
            get
            {
                // Owners is only used when we have Known Owners.
                if (KnownOwnerViewModels == null)
                {
                    return null;
                }

                // Empty Known Owners is treated as there being no assigned owner for this package.
                if (KnownOwnerViewModels.IsEmpty)
                {
                    return string.Empty;
                }

                // Having Known Owners but with an empty Owners string is treated as there being no assigned owner for this package.
                if (string.IsNullOrWhiteSpace(Owner))
                {
                    return string.Empty;
                }

                return string.Format(CultureInfo.CurrentCulture, Resx.Text_ByOwner, Owner);
            }
        }

        public string ByAuthor => !string.IsNullOrWhiteSpace(_packageModel.Authors) ? string.Format(CultureInfo.CurrentCulture, Resx.Text_ByAuthor, _packageModel.Authors) : null;

        /// <summary>
        /// Fallback to <see cref="ByAuthor"/> only when <see cref="ByOwner"> is null.
        /// </summary>
        public string ByOwnerOrAuthor => ByOwner ?? ByAuthor;

        public string VulnerableVersionsString => string.Join(", ", VulnerableVersions.Keys);

        private readonly Dictionary<NuGetVersion, int> _vulnerableVersions = [];
        public Dictionary<NuGetVersion, int> VulnerableVersions => _vulnerableVersions;

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

        public long? DownloadCount => (_packageModel as RemotePackageModel)?.DownloadCount;

        public string Summary => _packageModel.Summary;

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

        public bool Recommended => (_packageModel as RecommendedPackageModel) is not null;

        public (string modelVersion, string vsixVersion)? RecommenderVersion => (_packageModel as RecommendedPackageModel)?.RecommenderVersion;

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

        public PackageDeprecationMetadataContextInfo DeprecationMetadata => (_packageModel as IDeprecationCapable)?.DeprecationMetadata;
        public bool IsPackageDeprecated => (_packageModel as IDeprecationCapable)?.IsDeprecated ?? false;

        public bool IsPackageVulnerable => (_packageModel as IVulnerableCapable)?.IsVulnerable ?? false || VulnerableVersions.Count > 0;

        public int VulnerabilityMaxSeverity
        {
            get
            {
                if (VulnerableVersions.Count > 0)
                {
                    return VulnerableVersions.Values.Max();
                }
                else
                {
                    return -1;
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

        public Uri IconUrl => _packageModel.IconUrl;

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
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
            return _detailedPackageSearchMetadata.Value;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
        }

        public AlternatePackageMetadataContextInfo AlternatePackage => (_packageModel as IDeprecationCapable)?.AlternatePackage;

        public IReadOnlyCollection<PackageVulnerabilityMetadataContextInfo> Vulnerabilities => (_packageModel as IVulnerableCapable)?.Vulnerabilities ?? [];

        public void UpdateTransitiveInfo(PackageSearchMetadataContextInfo metadataContextInfo)
        {
            if (metadataContextInfo.TransitiveOrigins == null)
            {
                return;
            }

            _transitiveInstalledVersions.Add(metadataContextInfo.Identity.Version);
            _transitiveOrigins.AddRange(metadataContextInfo.TransitiveOrigins);
            TransitiveToolTipMessage = string.Format(CultureInfo.CurrentCulture, Resources.PackageVersionWithTransitiveOrigins, string.Join(", ", _transitiveInstalledVersions), string.Join(", ", _transitiveOrigins));
            OnPropertyChanged(nameof(TransitiveToolTipMessage));
        }

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

            using (Stream stream = await _packageModel.GetIconAsync(CancellationToken.None))
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

        private async Task ReloadPackageVersionsAsync()
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
            catch (OperationCanceledException)
            {
                // UI requested cancellation
            }
            catch (TimeoutException)
            {
                // Our code throws a TimeoutException for HTTP timeouts
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
            await RunOperationAsync(async (cancellationToken) =>
            {
                await _packageModel.PopulateDataAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                if (_packageModel is IDeprecationCapable deprecationCapable)
                {
                    UpdateDeprecationInfo(deprecationCapable);
                }

                if (_packageModel is IVulnerableCapable vulnerableCapable)
                {
                    UpdateVulnerabilityInfo(vulnerableCapable);
                }
            });
        }

        private async Task RunOperationAsync(Func<CancellationToken, Task> func)
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            try
            {
                await func(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // UI requested cancellation.
            }
            catch (TimeoutException)
            {
                // Our code throws a TimeoutException for HTTP timeouts
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

        private async Task UpdatePackageMaxVulnerabilityAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken)
        {
            await RunOperationAsync(async (cancellationToken) =>
            {
                IVulnerableCapable vulnerabilityDatabaseCapability = new VulnerableDatabaseCapability(_vulnerabilityService, packageIdentity);
                // Use ShutdownToken to ensure the operation is canceled if it's still running when VS shuts down.
                await vulnerabilityDatabaseCapability.PopulateDataAsync(VsShellUtilities.ShutdownToken);
                cancellationToken.ThrowIfCancellationRequested();

                UpdateVulnerabilityInfo(vulnerabilityDatabaseCapability);
            });
        }

        private void SetVulnerabilityMaxSeverity(NuGetVersion version, int maxSeverity)
        {
            if (maxSeverity > -1)
            {
                if (VulnerableVersions.TryAdd(version, maxSeverity))
                {
                    OnPropertyChanged(nameof(VulnerabilityMaxSeverity));
                    OnPropertyChanged(nameof(VulnerableVersions));
                    OnPropertyChanged(nameof(VulnerableVersionsString));
                }

                OnPropertyChanged(nameof(Status));
            }
        }

        private void UpdateDeprecationInfo(IDeprecationCapable deprecationCapable)
        {
            if (deprecationCapable == null)
            {
                return;
            }

            OnPropertyChanged(nameof(IsPackageDeprecated));
            OnPropertyChanged(nameof(DeprecationMetadata));
            OnPropertyChanged(nameof(AlternatePackage));
        }

        private void UpdateVulnerabilityInfo(IVulnerableCapable vulnerableCapable)
        {
            if (vulnerableCapable == null)
            {
                return;
            }

            SetVulnerabilityMaxSeverity(Version, (int)vulnerableCapable.VulnerabilityMaxSeverity);
            OnPropertyChanged(nameof(IsPackageVulnerable));
            OnPropertyChanged(nameof(IsPackageWithWarnings));
            OnPropertyChanged(nameof(Vulnerabilities));
        }

        public void UpdateInstalledPackagesVulnerabilities(PackageIdentity packageIdentity)
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            NuGetUIThreadHelper.JoinableTaskFactory
                .RunAsync(() => UpdatePackageMaxVulnerabilityAsync(packageIdentity, cancellationToken))
                .PostOnFailure(nameof(PackageItemViewModel), nameof(UpdatePackageMaxVulnerabilityAsync));
        }

        public async Task UpdatePackageStatusAsync(IEnumerable<PackageCollectionItem> installedPackages, bool clearCache = false)
        {
            // Get the maximum version installed in any target project/solution
            InstalledVersion = installedPackages
                .GetPackageVersions(Id)
                .MaxOrDefault();

            if (clearCache && InstalledVersion != null)
            {
                _searchService.ClearFromCache(Id, Sources, IncludePrerelease);
            }

            await ReloadPackageVersionsAsync();
            await ReloadPackageMetadataAsync();

            OnPropertyChanged(nameof(Status));
        }

        public async Task UpdateTransitivePackageStatusAsync()
        {
            InstalledVersion = Version;

            // Transitive packages cannot be updated and can only be installed as top-level packages with their currently installed version.
            LatestVersion = InstalledVersion;

            await ReloadPackageMetadataAsync();

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

        public string PackagePath => (_packageModel as LocalPackageModel)?.PackagePath;

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
