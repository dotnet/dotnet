// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    internal sealed class NuGetSolutionManagerServiceWrapper : INuGetSolutionManagerService
    {
        private INuGetSolutionManagerService _service = NullNuGetSolutionManagerService.Instance;
        private readonly object _syncObject = new object();

        public event EventHandler<string>? AfterNuGetCacheUpdated;
        public event EventHandler<IProjectContextInfo>? AfterProjectRenamed;
        public event EventHandler<IProjectContextInfo>? ProjectAdded;
        public event EventHandler<IProjectContextInfo>? ProjectRemoved;
        public event EventHandler<IProjectContextInfo>? ProjectRenamed;
        public event EventHandler<IProjectContextInfo>? ProjectUpdated;

        internal INuGetSolutionManagerService Service
        {
            get
            {
                lock (_syncObject)
                {
                    return _service;
                }
            }
        }

        public void Dispose()
        {
            lock (_syncObject)
            {
                using (INuGetSolutionManagerService? service = _service)
                {
                    _service = NullNuGetSolutionManagerService.Instance;
                }
            }

            GC.SuppressFinalize(this);
        }

        public INuGetSolutionManagerService Swap(INuGetSolutionManagerService newService)
        {
            lock (_syncObject)
            {
                UnregisterEventHandlers();

                INuGetSolutionManagerService oldService = _service;

                _service = newService ?? NullNuGetSolutionManagerService.Instance;

                RegisterEventHandlers();

                return oldService;
            }
        }

        public ValueTask<string> GetSolutionDirectoryAsync(CancellationToken cancellationToken)
        {
            return Service.GetSolutionDirectoryAsync(cancellationToken);
        }

        private void RegisterEventHandlers()
        {
            _service.AfterNuGetCacheUpdated += OnAfterNuGetCacheUpdated;
            _service.AfterProjectRenamed += OnAfterProjectRenamed;
            _service.ProjectAdded += OnProjectAdded;
            _service.ProjectRemoved += OnProjectRemoved;
            _service.ProjectRenamed += OnProjectRenamed;
            _service.ProjectUpdated += OnProjectUpdated;
        }

        private void UnregisterEventHandlers()
        {
            _service.AfterNuGetCacheUpdated -= OnAfterNuGetCacheUpdated;
            _service.AfterProjectRenamed -= OnAfterProjectRenamed;
            _service.ProjectAdded -= OnProjectAdded;
            _service.ProjectRemoved -= OnProjectRemoved;
            _service.ProjectRenamed -= OnProjectRenamed;
            _service.ProjectUpdated -= OnProjectUpdated;
        }

        private void OnAfterNuGetCacheUpdated(object sender, string e)
        {
            AfterNuGetCacheUpdated?.Invoke(this, e);
        }

        private void OnAfterProjectRenamed(object sender, IProjectContextInfo e)
        {
            AfterProjectRenamed?.Invoke(this, e);
        }

        private void OnProjectAdded(object sender, IProjectContextInfo e)
        {
            ProjectAdded?.Invoke(this, e);
        }

        private void OnProjectRemoved(object sender, IProjectContextInfo e)
        {
            ProjectRemoved?.Invoke(this, e);
        }

        private void OnProjectRenamed(object sender, IProjectContextInfo e)
        {
            ProjectRenamed?.Invoke(this, e);
        }

        private void OnProjectUpdated(object sender, IProjectContextInfo e)
        {
            ProjectUpdated?.Invoke(this, e);
        }

        private sealed class NullNuGetSolutionManagerService : INuGetSolutionManagerService
        {
            public event EventHandler<string> AfterNuGetCacheUpdated { add { } remove { } }
            public event EventHandler<IProjectContextInfo> AfterProjectRenamed { add { } remove { } }
            public event EventHandler<IProjectContextInfo> ProjectAdded { add { } remove { } }
            public event EventHandler<IProjectContextInfo> ProjectRemoved { add { } remove { } }
            public event EventHandler<IProjectContextInfo> ProjectRenamed { add { } remove { } }
            public event EventHandler<IProjectContextInfo> ProjectUpdated { add { } remove { } }

            internal static NullNuGetSolutionManagerService Instance { get; } = new NullNuGetSolutionManagerService();

            public void Dispose() { }

            public ValueTask<string> GetSolutionDirectoryAsync(CancellationToken cancellationToken) => new ValueTask<string>();
        }
    }
}
