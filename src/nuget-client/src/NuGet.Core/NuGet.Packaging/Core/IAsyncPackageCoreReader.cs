// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Versioning;

namespace NuGet.Packaging.Core
{
    /// <summary>
    /// A basic asynchronous package reader that provides the identity, min client version, and file access.
    /// </summary>
    /// <remarks>Higher level concepts used for normal development nupkgs should go at a higher level</remarks>
    public interface IAsyncPackageCoreReader : IDisposable
    {
        /// <summary>
        /// Asynchronously gets the identity of the package.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="PackageIdentity" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<PackageIdentity> GetIdentityAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets the minimum client version needed to consume the package.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="NuGetVersion" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<NuGetVersion> GetMinClientVersionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets zero or more package types from the .nuspec.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IReadOnlyList{PackageType}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IReadOnlyList<PackageType>> GetPackageTypesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously returns a file stream from the package.
        /// </summary>
        /// <param name="path">The file path in the package.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="Stream" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<Stream> GetStreamAsync(string path, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets all files in the package.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{String}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<string>> GetFilesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets files in a folder in the package.
        /// </summary>
        /// <param name="folder">A folder path in the package.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{String}" /> for files under the specified folder.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<string>> GetFilesAsync(string folder, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets a nuspec stream.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="Stream" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<Stream> GetNuspecAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets a nuspec file path.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="string" />
        /// representing the nuspec file path.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<string> GetNuspecFileAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously copies files from a package to a new location.
        /// </summary>
        /// <param name="destination">The destination path to copy to.</param>
        /// <param name="packageFiles">The package files to copy.</param>
        /// <param name="extractFile">A package file extraction delegate.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns am
        /// <see cref="IEnumerable{String}" /> for the copied file paths.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<string>> CopyFilesAsync(
            string destination,
            IEnumerable<string> packageFiles,
            ExtractPackageFileDelegate extractFile,
            ILogger logger,
            CancellationToken cancellationToken);
    }
}
