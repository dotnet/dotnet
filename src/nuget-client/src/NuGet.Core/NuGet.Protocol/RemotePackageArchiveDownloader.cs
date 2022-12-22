// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// A remote package downloader.
    /// </summary>
    public sealed class RemotePackageArchiveDownloader : IPackageDownloader
    {
        private readonly SourceCacheContext _cacheContext;
        private string _destinationFilePath;
        private Func<Exception, Task<bool>> _handleExceptionAsync;
        private bool _isDisposed;
        private readonly ILogger _logger;
        private readonly PackageIdentity _packageIdentity;
        private Lazy<PackageArchiveReader> _packageReader;
        private readonly FindPackageByIdResource _resource;
        private SemaphoreSlim _throttle;

        /// <summary>
        /// Gets an asynchronous package content reader.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        public IAsyncPackageContentReader ContentReader
        {
            get
            {
                ThrowIfDisposed();

                return _packageReader.Value;
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

                return _packageReader.Value;
            }
        }

        public ISignedPackageReader SignedPackageReader
        {
            get
            {
                ThrowIfDisposed();

                return _packageReader.Value;
            }
        }

        public string Source { get; }

        /// <summary>
        /// Initializes a new <see cref="RemotePackageArchiveDownloader" /> class.
        /// </summary>
        /// <param name="source">A package source.</param>
        /// <param name="resource">A <see cref="FindPackageByIdResource" /> resource.</param>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="resource" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> is <c>null</c>.</exception>
        public RemotePackageArchiveDownloader(
            string source,
            FindPackageByIdResource resource,
            PackageIdentity packageIdentity,
            SourceCacheContext cacheContext,
            ILogger logger)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (cacheContext == null)
            {
                throw new ArgumentNullException(nameof(cacheContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _resource = resource;
            _packageIdentity = packageIdentity;
            _cacheContext = cacheContext;
            _logger = logger;
            _packageReader = new Lazy<PackageArchiveReader>(GetPackageReader);
            _handleExceptionAsync = exception => Task.FromResult(false);
            Source = source;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_packageReader.IsValueCreated)
                {
                    _packageReader.Value.Dispose();
                }

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

            try
            {
                if (_throttle != null)
                {
                    await _throttle.WaitAsync(cancellationToken);
                }

                cancellationToken.ThrowIfCancellationRequested();

                using (var destination = new FileStream(
                    destinationFilePath,
                    FileMode.Create,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite | FileShare.Delete,
                    bufferSize: 4096))
                {
                    var result = await _resource.CopyNupkgToStreamAsync(
                        _packageIdentity.Id,
                        _packageIdentity.Version,
                        destination,
                        _cacheContext,
                        _logger,
                        cancellationToken);

                    _destinationFilePath = destinationFilePath;

                    return result;
                }
            }
            catch (Exception ex)
            {
                if (!await _handleExceptionAsync(ex))
                {
                    throw;
                }
            }
            finally
            {
                _throttle?.Release();
            }

            return false;
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
        public Task<string> GetPackageHashAsync(string hashAlgorithm, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (string.IsNullOrEmpty(hashAlgorithm))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(hashAlgorithm));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var stream = GetDestinationStream())
            {
                var bytes = new CryptoHashProvider(hashAlgorithm).CalculateHash(stream);
                var packageHash = Convert.ToBase64String(bytes);

                return Task.FromResult(packageHash);
            }
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
            _throttle = throttle;
        }

        private PackageArchiveReader GetPackageReader()
        {
            var stream = GetDestinationStream();

            return new PackageArchiveReader(stream);
        }

        private FileStream GetDestinationStream()
        {
            ThrowIfDisposed();

            if (string.IsNullOrEmpty(_destinationFilePath))
            {
                throw new InvalidOperationException();
            }

            return new FileStream(
               _destinationFilePath,
               FileMode.Open,
               FileAccess.Read,
               FileShare.Read,
               bufferSize: 4096);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(RemotePackageArchiveDownloader));
            }
        }
    }
}
