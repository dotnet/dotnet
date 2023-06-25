// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A package downloader that delegates to a plugin.
    /// </summary>
    public sealed class PluginPackageDownloader : IPackageDownloader
    {
        private Func<Exception, Task<bool>> _handleExceptionAsync;
        private bool _isDisposed;
        private readonly PackageIdentity _packageIdentity;
        private readonly PluginPackageReader _packageReader;
        private readonly string _packageSourceRepository;
        private readonly IPlugin _plugin;

        /// <summary>
        /// Gets an asynchronous package content reader.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        public IAsyncPackageContentReader ContentReader
        {
            get
            {
                ThrowIfDisposed();

                return _packageReader;
            }
        }

        /// <summary>
        /// Gets an asynchronous package core reader.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        public IAsyncPackageCoreReader CoreReader
        {
            get
            {
                ThrowIfDisposed();

                return _packageReader;
            }
        }

        public ISignedPackageReader SignedPackageReader
        {
            get
            {
                ThrowIfDisposed();

                return _packageReader;
            }
        }

        public string Source => _packageSourceRepository;

        /// <summary>
        /// Initializes a new <see cref="PluginPackageDownloader" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="packageReader">A plugin package reader.</param>
        /// <param name="packageSourceRepository">A package source repository location.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageReader" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="packageSourceRepository" />
        /// is either <c>null</c> or an empty string.</exception>
        public PluginPackageDownloader(
            IPlugin plugin,
            PackageIdentity packageIdentity,
            PluginPackageReader packageReader,
            string packageSourceRepository)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (packageReader == null)
            {
                throw new ArgumentNullException(nameof(packageReader));
            }

            if (string.IsNullOrEmpty(packageSourceRepository))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageSourceRepository));
            }

            _plugin = plugin;
            _packageIdentity = packageIdentity;
            _packageReader = packageReader;
            _packageSourceRepository = packageSourceRepository;
            _handleExceptionAsync = exception => TaskResult.False;
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _packageReader.Dispose();
                _plugin.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Asynchronously copies a .nupkg to a target file path.
        /// </summary>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />
        /// indicating whether or not the copy was successful.</returns>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="destinationFilePath" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task<bool> CopyNupkgFileToAsync(string destinationFilePath, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (string.IsNullOrEmpty(destinationFilePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(destinationFilePath));
            }

            cancellationToken.ThrowIfCancellationRequested();

            string filePath = null;

            try
            {
                filePath = await _packageReader.CopyNupkgAsync(destinationFilePath, cancellationToken);
            }
            catch (Exception ex)
            {
                if (!await _handleExceptionAsync(ex))
                {
                    throw;
                }
            }

            return !string.IsNullOrEmpty(filePath);
        }

        /// <summary>
        /// Asynchronously gets a package hash.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="string" />
        /// representing the package hash.</returns>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="hashAlgorithm" />
        /// is either <c>null</c> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task<string> GetPackageHashAsync(string hashAlgorithm, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (string.IsNullOrEmpty(hashAlgorithm))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(hashAlgorithm));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var request = new GetPackageHashRequest(
                _packageSourceRepository,
                _packageIdentity.Id,
                _packageIdentity.Version.ToNormalizedString(),
                hashAlgorithm);

            var response = await _plugin.Connection.SendRequestAndReceiveResponseAsync<GetPackageHashRequest, GetPackageHashResponse>(
                MessageMethod.GetPackageHash,
                request,
                cancellationToken);

            if (response != null && response.ResponseCode == MessageResponseCode.Success)
            {
                return response.Hash;
            }

            return null;
        }

        /// <summary>
        /// Sets an exception handler for package downloads.
        /// </summary>
        /// <remarks>The exception handler returns a task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />
        /// indicating whether or not the exception was handled.  To handle an exception and stop its
        /// propagation, the task should return <c>true</c>.  Otherwise, the exception will be rethrown.</remarks>
        /// <param name="handleExceptionAsync">An exception handler.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="handleExceptionAsync" />
        /// is <c>null</c>.</exception>
        public void SetExceptionHandler(Func<Exception, Task<bool>> handleExceptionAsync)
        {
            if (handleExceptionAsync == null)
            {
                throw new ArgumentNullException(nameof(handleExceptionAsync));
            }

            _handleExceptionAsync = handleExceptionAsync;
        }

        /// <summary>
        /// Sets a throttle for package downloads.
        /// </summary>
        /// <param name="throttle">A throttle.  Can be <c>null</c>.</param>
        public void SetThrottle(SemaphoreSlim throttle)
        {
            // Do nothing.  Plugins are not implemented on macOS.
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(PluginPackageDownloader));
            }
        }
    }
}
