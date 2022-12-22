// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Threading;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class NuGetProjectManagerServiceState : INuGetProjectManagerServiceState
    {
        private readonly AsyncSemaphore _asyncSemaphore = new AsyncSemaphore(initialCount: 1);
        private PackageIdentity? _packageidentity;
        private readonly Dictionary<string, ResolvedAction> _resolvedActions = new Dictionary<string, ResolvedAction>();
        private SourceCacheContext? _sourceCacheContext;
        private bool _isDisposed;

        public AsyncSemaphore AsyncSemaphore
        {
            get
            {
                ThrowIfDisposed();

                return _asyncSemaphore;
            }
        }

        public PackageIdentity? PackageIdentity
        {
            get
            {
                ThrowIfDisposed();

                return _packageidentity;
            }
            set
            {
                ThrowIfDisposed();

                _packageidentity = value;
            }
        }

        public Dictionary<string, ResolvedAction> ResolvedActions
        {
            get
            {
                ThrowIfDisposed();

                return _resolvedActions;
            }
        }

        public SourceCacheContext? SourceCacheContext
        {
            get
            {
                ThrowIfDisposed();

                return _sourceCacheContext;
            }
            set
            {
                ThrowIfDisposed();

                _sourceCacheContext = value;
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _asyncSemaphore.Dispose();
                _sourceCacheContext?.Dispose();

                GC.SuppressFinalize(this);
            }

            _isDisposed = true;
        }

        public void Reset()
        {
            ThrowIfDisposed();

            _packageidentity = null;
            _resolvedActions.Clear();
            _sourceCacheContext?.Dispose();
            _sourceCacheContext = null;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(NuGetProjectManagerServiceState));
            }
        }
    }
}
