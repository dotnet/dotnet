// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// An object whose disposal releases a held lock.
    /// </summary>
    internal sealed class AsyncLockReleaser : IDisposable
    {
        private readonly AsyncLockAwaiter _awaiter;
        private bool _isDisposed = false;

        public AsyncLockReleaser(AsyncLockAwaiter awaiter)
        {
            if (awaiter == null)
            {
                throw new ArgumentNullException(nameof(awaiter));
            }

            _awaiter = awaiter;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool _)
        {
            if (_isDisposed)
            {
                return;
            }

            _awaiter.Release();

            _isDisposed = true;
        }

        ~AsyncLockReleaser()
        {
            Dispose(false);
        }
    }
}
