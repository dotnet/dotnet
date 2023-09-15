// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;

namespace NuGet.Packaging
{
    /// <summary>
    /// A package downloader.
    /// </summary>
    public interface IPackageDownloader : IDisposable
    {
        /// <summary>
        /// Gets an asynchronous package content reader.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        IAsyncPackageContentReader ContentReader { get; }

        /// <summary>
        /// Gets an asynchronous package core reader.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        IAsyncPackageCoreReader CoreReader { get; }

        ISignedPackageReader SignedPackageReader { get; }

        string Source { get; }

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
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<bool> CopyNupkgFileToAsync(string destinationFilePath, CancellationToken cancellationToken);

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
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<string> GetPackageHashAsync(string hashAlgorithm, CancellationToken cancellationToken);

        /// <summary>
        /// Sets an exception handler for package downloads.
        /// </summary>
        /// <remarks>The exception handler returns a task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />
        /// indicating whether or not the exception was handled.  To handle an exception and stop its
        /// propagation, the task should return <see langword="true" />.  Otherwise, the exception will be rethrown.</remarks>
        /// <param name="handleExceptionAsync">An exception handler.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="handleExceptionAsync" />
        /// is <see langword="null" />.</exception>
        void SetExceptionHandler(Func<Exception, Task<bool>> handleExceptionAsync);

        /// <summary>
        /// Sets a throttle for package downloads.
        /// </summary>
        /// <param name="throttle">A throttle.  Can be <see langword="null" />.</param>
        void SetThrottle(SemaphoreSlim throttle);
    }
}
