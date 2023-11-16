// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Threading;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.UI.Utility
{
    internal sealed class NuGetSearchServiceReconnector : IDisposable
    {
        private IServiceBroker _serviceBroker;
        private INuGetSearchService _service;
        private JoinableTaskFactory _jtf;
        private ManagedNuGetSearchService _object;

        // This token source should be used only for dealing with "internal" concerns of this class. When calling
        // methods on INuGetSearchService, it's the caller's responsibility not to dispose this class while a
        // method call is pending.
        private CancellationTokenSource _disposedTokenSource;

        private NuGetSearchServiceReconnector(IServiceBroker serviceBroker, JoinableTaskFactory jtf, INuGetSearchService initialService)
        {
            if (serviceBroker == null)
            {
                throw new ArgumentNullException(nameof(serviceBroker));
            }

            if (jtf == null)
            {
                throw new ArgumentNullException(nameof(jtf));
            }

            if (initialService == null)
            {
                throw new ArgumentNullException(nameof(initialService));
            }

            _disposedTokenSource = new CancellationTokenSource();
            _jtf = jtf;
            _serviceBroker = serviceBroker;
            _service = initialService;

            _serviceBroker.AvailabilityChanged += AvailabilityChanged;

            _object = new ManagedNuGetSearchService(this);
        }

        public static async Task<NuGetSearchServiceReconnector> CreateAsync(IServiceBroker serviceBroker, JoinableTaskFactory jtf, CancellationToken cancellationToken)
        {
            if (serviceBroker == null)
            {
                throw new ArgumentNullException(nameof(serviceBroker));
            }

            if (jtf == null)
            {
                throw new ArgumentNullException(nameof(jtf));
            }

#pragma warning disable ISB001 // Dispose of proxies - ownership is being transferred to the instance.
            var initialService = await serviceBroker.GetProxyAsync<INuGetSearchService>(NuGetServices.SearchService, cancellationToken);
#pragma warning restore ISB001 // Dispose of proxies
            return new NuGetSearchServiceReconnector(serviceBroker, jtf, initialService);
        }

        private void AvailabilityChanged(object sender, BrokeredServicesChangedEventArgs e)
        {
            _jtf.RunAsync(AvailabilityChangedAsync)
                .PostOnFailure(typeof(NuGetSearchServiceReconnector).FullName, nameof(AvailabilityChangedAsync));
        }

        internal async Task AvailabilityChangedAsync()
        {
            try
            {
                _service?.Dispose();
                _service = await _serviceBroker.GetProxyAsync<INuGetSearchService>(NuGetServices.SearchService, _disposedTokenSource.Token);
            }
            catch (OperationCanceledException) when (_disposedTokenSource.Token.IsCancellationRequested)
            {
                // Expected
            }
        }

        public void Dispose()
        {
            // This class does not own the lifetime of _serviceBroker, so don't dispose it.
            _serviceBroker.AvailabilityChanged -= AvailabilityChanged;

            _disposedTokenSource.Cancel();
            _disposedTokenSource.Dispose();

            _object.Dispose();

            _service?.Dispose();
        }

        public INuGetSearchService Object => _object;

        private class ManagedNuGetSearchService : INuGetSearchService
        {
            NuGetSearchServiceReconnector _parent;

            public ManagedNuGetSearchService(NuGetSearchServiceReconnector service)
            {
                _parent = service;
            }

            public ValueTask<SearchResultContextInfo> ContinueSearchAsync(CancellationToken cancellationToken)
            {
                return _parent._service.ContinueSearchAsync(cancellationToken);
            }

            public void Dispose()
            {
                // do not dispose `_parent`, it's the responsibility of the object owning the instance.
            }

            public Task<IReadOnlyList<SourceRepository>> GetAllPackageFoldersAsync(IReadOnlyCollection<IProjectContextInfo> projectContextInfos, CancellationToken cancellationToken)
            {
                return _parent._service.GetAllPackageFoldersAsync(projectContextInfos, cancellationToken);
            }

            public ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>> GetAllPackagesAsync(
                IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
                IReadOnlyCollection<PackageSourceContextInfo> packageSources,
                IReadOnlyCollection<string> targetFrameworks,
                SearchFilter searchFilter,
                NuGet.VisualStudio.Internal.Contracts.ItemFilter itemFilter,
                bool isSolution,
                CancellationToken cancellationToken)
            {
                return _parent._service.GetAllPackagesAsync(projectContextInfos, packageSources, targetFrameworks, searchFilter, itemFilter, isSolution, cancellationToken);
            }

            public ValueTask<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo)> GetPackageMetadataAsync(PackageIdentity identity, IReadOnlyCollection<PackageSourceContextInfo> packageSources, bool includePrerelease, CancellationToken cancellationToken)
            {
                return _parent._service.GetPackageMetadataAsync(identity, packageSources, includePrerelease, cancellationToken);
            }

            public ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>> GetPackageMetadataListAsync(string id, IReadOnlyCollection<PackageSourceContextInfo> packageSources, bool includePrerelease, bool includeUnlisted, CancellationToken cancellationToken)
            {
                return _parent._service.GetPackageMetadataListAsync(id, packageSources, includePrerelease, includeUnlisted, cancellationToken);
            }

            public ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(PackageIdentity identity, IReadOnlyCollection<PackageSourceContextInfo> packageSources, bool includePrerelease, CancellationToken cancellationToken)
            {
                return _parent._service.GetPackageVersionsAsync(identity, packageSources, includePrerelease, cancellationToken);
            }

            public ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(PackageIdentity identity, IReadOnlyCollection<PackageSourceContextInfo> packageSources, bool includePrerelease, bool isTransitive, CancellationToken cancellationToken)
            {
                return _parent._service.GetPackageVersionsAsync(identity, packageSources, includePrerelease, isTransitive, cancellationToken);
            }

            public ValueTask<IReadOnlyCollection<VersionInfoContextInfo>> GetPackageVersionsAsync(PackageIdentity identity, IReadOnlyCollection<PackageSourceContextInfo> packageSources, bool includePrerelease, bool isTransitive, IEnumerable<IProjectContextInfo> projects, CancellationToken cancellationToken)
            {
                return _parent._service.GetPackageVersionsAsync(identity, packageSources, includePrerelease, isTransitive, projects, cancellationToken);
            }

            public ValueTask<int> GetTotalCountAsync(
                int maxCount,
                IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
                IReadOnlyCollection<PackageSourceContextInfo> packageSources,
                IReadOnlyCollection<string> targetFrameworks,
                SearchFilter searchFilter,
                NuGet.VisualStudio.Internal.Contracts.ItemFilter itemFilter,
                bool isSolution,
                CancellationToken cancellationToken)
            {
                return _parent._service.GetTotalCountAsync(maxCount, projectContextInfos, packageSources, targetFrameworks, searchFilter, itemFilter, isSolution, cancellationToken);
            }

            public ValueTask<SearchResultContextInfo> RefreshSearchAsync(CancellationToken cancellationToken)
            {
                return _parent._service.RefreshSearchAsync(cancellationToken);
            }

            public ValueTask<SearchResultContextInfo> SearchAsync(
                IReadOnlyCollection<IProjectContextInfo> projectContextInfos,
                IReadOnlyCollection<PackageSourceContextInfo> packageSources,
                IReadOnlyCollection<string> targetFrameworks,
                string searchText,
                SearchFilter searchFilter,
                NuGet.VisualStudio.Internal.Contracts.ItemFilter itemFilter,
                bool isSolution,
                bool useRecommender,
                CancellationToken cancellationToken)
            {
                return _parent._service.SearchAsync(projectContextInfos, packageSources, targetFrameworks, searchText, searchFilter, itemFilter, isSolution, useRecommender, cancellationToken);
            }
        }
    }
}
