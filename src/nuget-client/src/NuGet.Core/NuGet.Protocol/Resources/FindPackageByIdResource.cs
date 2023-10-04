// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// A resource capable of fetching packages, package versions and package dependency information.
    /// </summary>
    public abstract class FindPackageByIdResource : INuGetResource
    {
        /// <summary>
        /// Asynchronously gets all package versions for a package ID.
        /// </summary>
        /// <param name="id">A package ID.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{NuGetVersion}" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public abstract Task<IEnumerable<NuGetVersion>> GetAllVersionsAsync(
            string id,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets dependency information for a specific package.
        /// </summary>
        /// <param name="id">A package id.</param>
        /// <param name="version">A package version.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{NuGetVersion}" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="version" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public abstract Task<FindPackageByIdDependencyInfo> GetDependencyInfoAsync(
            string id,
            NuGetVersion version,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously copies a .nupkg to a stream.
        /// </summary>
        /// <param name="id">A package ID.</param>
        /// <param name="version">A package version.</param>
        /// <param name="destination">A destination stream.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="bool" /> indicating whether or not the .nupkg file was copied.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="version" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="destination" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public abstract Task<bool> CopyNupkgToStreamAsync(
            string id,
            NuGetVersion version,
            Stream destination,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously gets a package downloader for a package identity.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an <see cref="IPackageDownloader" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public abstract Task<IPackageDownloader> GetPackageDownloaderAsync(
            PackageIdentity packageIdentity,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously check if exact package (id/version) exists at this source.
        /// </summary>
        /// <param name="id">A package id.</param>
        /// <param name="version">A package version.</param>
        /// <param name="cacheContext">A source cache context.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{NuGetVersion}" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="version" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cacheContext" /> <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger" /> <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public abstract Task<bool> DoesPackageExistAsync(
            string id,
            NuGetVersion version,
            SourceCacheContext cacheContext,
            ILogger logger,
            CancellationToken cancellationToken);

        /// <summary>
        /// Read dependency info from a nuspec.
        /// </summary>
        /// <remarks>This also verifies minClientVersion.</remarks>
        protected static FindPackageByIdDependencyInfo GetDependencyInfo(NuspecReader reader)
        {
            // Since this is the first place a package is read after selecting it as the best version
            // check the minClientVersion here to verify we are okay to read this package.
            MinClientVersionUtility.VerifyMinClientVersion(reader);

            // Create dependency info
            return new FindPackageByIdDependencyInfo(
                reader.GetIdentity(),
                reader.GetDependencyGroups(),
                reader.GetFrameworkAssemblyGroups());
        }
    }
}
