// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.DependencyResolver
{
    /// <summary>
    /// A remote dependency provider.
    /// </summary>
    public interface IRemoteDependencyProvider
    {
        /// <summary>
        /// Gets a flag indicating whether or not the provider source is HTTP or HTTPS.
        /// </summary>
        bool IsHttp { get; }

        /// <summary>
        /// Gets the package source.
        /// </summary>
        /// <remarks>Optional. This will be <see langword="null" /> for project providers.</remarks>
        PackageSource Source { get; }

        /// <summary>
        /// Gets the source repository.
        /// </summary>
        /// <remarks>Optional. This will be <see langword="null" /> for project providers.</remarks>
        SourceRepository SourceRepository { get; }

        /// <summary>
        /// Asynchronously discovers all versions of a package from a source and selects the best match.
        /// </summary>
        /// <remarks>This does not download the package.</remarks>
        /// <param name="libraryRange">A library range.</param>
        /// <param name="targetFramework">A target framework.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="LibraryIdentity" />
        /// instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="libraryRange" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="targetFramework" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<LibraryIdentity> FindLibraryAsync(
            LibraryRange libraryRange,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets package dependencies.
        /// </summary>
        /// <param name="libraryIdentity">A library identity.</param>
        /// <param name="targetFramework">A target framework.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="LibraryDependencyInfo" />
        /// instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="libraryIdentity" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="targetFramework" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<LibraryDependencyInfo> GetDependenciesAsync(
            LibraryIdentity libraryIdentity,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets a package downloader.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="IPackageDownloader" />
        /// instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IPackageDownloader> GetPackageDownloaderAsync(
            PackageIdentity packageIdentity,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken);

        Task<IEnumerable<NuGetVersion>> GetAllVersionsAsync(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken token);
    }
}
