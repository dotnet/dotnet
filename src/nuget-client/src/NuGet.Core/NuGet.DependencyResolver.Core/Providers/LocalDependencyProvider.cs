// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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
    /// A local dependency provider.
    /// </summary>
    public class LocalDependencyProvider : IRemoteDependencyProvider
    {
        private readonly IDependencyProvider _dependencyProvider;

        /// <summary>
        /// Initializes a new <see cref="LocalDependencyProvider" /> class.
        /// </summary>
        /// <param name="dependencyProvider"></param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dependencyProvider" />
        /// is <see langword="null" />.</exception>
        public LocalDependencyProvider(IDependencyProvider dependencyProvider)
        {
            if (dependencyProvider == null)
            {
                throw new ArgumentNullException(nameof(dependencyProvider));
            }

            _dependencyProvider = dependencyProvider;
        }

        /// <summary>
        /// Gets a flag indicating whether or not the provider source is HTTP or HTTPS.
        /// </summary>
        public bool IsHttp { get; private set; }

        /// <summary>
        /// Gets the package source.
        /// </summary>
        /// <remarks>Optional. This will be <see langword="null" /> for project providers.</remarks>
        public PackageSource Source { get; private set; }

        public SourceRepository SourceRepository { get; private set; }

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
        public Task<LibraryIdentity> FindLibraryAsync(
            LibraryRange libraryRange,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (libraryRange == null)
            {
                throw new ArgumentNullException(nameof(libraryRange));
            }

            if (targetFramework == null)
            {
                throw new ArgumentNullException(nameof(targetFramework));
            }

            var library = _dependencyProvider.GetLibrary(libraryRange, targetFramework);

            if (library == null)
            {
                return TaskResult.Null<LibraryIdentity>();
            }

            return Task.FromResult(library.Identity);
        }

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
        public Task<LibraryDependencyInfo> GetDependenciesAsync(
            LibraryIdentity libraryIdentity,
            NuGetFramework targetFramework,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            if (libraryIdentity == null)
            {
                throw new ArgumentNullException(nameof(libraryIdentity));
            }

            if (targetFramework == null)
            {
                throw new ArgumentNullException(nameof(targetFramework));
            }

            var library = _dependencyProvider.GetLibrary(libraryIdentity, targetFramework);

            var dependencyInfo = LibraryDependencyInfo.Create(
                library.Identity,
                targetFramework,
                library.Dependencies);

            return Task.FromResult(dependencyInfo);
        }

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
        /// <exception cref="NotSupportedException">Thrown always.</exception>
        public Task<IPackageDownloader> GetPackageDownloaderAsync(
            PackageIdentity packageIdentity,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<IEnumerable<NuGetVersion>> GetAllVersionsAsync(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken token)
        {
            throw new NotImplementedException();
        }

    }
}
