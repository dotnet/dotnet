// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    internal sealed class NuGetSourcesServiceWrapper : INuGetSourcesService
    {
        private INuGetSourcesService _service = NullNuGetSourcesService.Instance;
        private readonly object _syncObject = new object();

        public event EventHandler<IReadOnlyList<PackageSourceContextInfo>>? PackageSourcesChanged;

        internal INuGetSourcesService Service
        {
            get
            {
                lock (_syncObject)
                {
                    return _service;
                }
            }
        }

        public INuGetSourcesService Swap(INuGetSourcesService newService)
        {
            lock (_syncObject)
            {
                Service.PackageSourcesChanged -= OnPackageSourcesChanged;

                INuGetSourcesService oldService = _service;

                _service = newService ?? NullNuGetSourcesService.Instance;

                Service.PackageSourcesChanged += OnPackageSourcesChanged;

                return oldService;
            }
        }

        private void OnPackageSourcesChanged(object sender, IReadOnlyList<PackageSourceContextInfo> e)
        {
            PackageSourcesChanged?.Invoke(sender, e);
        }

        public void Dispose()
        {
            lock (_syncObject)
            {
                using (INuGetSourcesService? service = _service)
                {
                    _service = NullNuGetSourcesService.Instance;
                }
            }

            GC.SuppressFinalize(this);
        }

        public ValueTask<IReadOnlyList<PackageSourceContextInfo>> GetPackageSourcesAsync(CancellationToken cancellationToken)
        {
            return Service.GetPackageSourcesAsync(cancellationToken);
        }

        public ValueTask SavePackageSourceContextInfosAsync(IReadOnlyList<PackageSourceContextInfo> sources, CancellationToken cancellationToken)
        {
            return Service.SavePackageSourceContextInfosAsync(sources, cancellationToken);
        }

        public ValueTask<string?> GetActivePackageSourceNameAsync(CancellationToken cancellationToken)
        {
            return Service.GetActivePackageSourceNameAsync(cancellationToken);
        }

        private sealed class NullNuGetSourcesService : INuGetSourcesService
        {
            public event EventHandler<IReadOnlyList<PackageSourceContextInfo>>? PackageSourcesChanged { add { } remove { } }

            internal static NullNuGetSourcesService Instance { get; } = new NullNuGetSourcesService();

            public void Dispose() { }

            public ValueTask<IReadOnlyList<PackageSourceContextInfo>> GetPackageSourcesAsync(CancellationToken cancellationToken) => new ValueTask<IReadOnlyList<PackageSourceContextInfo>>(Array.Empty<PackageSourceContextInfo>());

            public ValueTask SavePackageSourceContextInfosAsync(IReadOnlyList<PackageSourceContextInfo> sources, CancellationToken cancellationToken) => new ValueTask();

            public ValueTask<string?> GetActivePackageSourceNameAsync(CancellationToken cancellationToken) => new ValueTask<string?>();
        }
    }
}
