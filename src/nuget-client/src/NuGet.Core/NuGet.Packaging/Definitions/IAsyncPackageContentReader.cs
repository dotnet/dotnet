// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Packaging
{
    /// <summary>
    /// An asynchronous package content reader.
    /// </summary>
    public interface IAsyncPackageContentReader
    {
        /// <summary>
        /// Asynchronously returns all framework references found in the nuspec.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<FrameworkSpecificGroup>> GetFrameworkItemsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously returns all items under the build folder.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<FrameworkSpecificGroup>> GetBuildItemsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously returns all items under the tools folder.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<FrameworkSpecificGroup>> GetToolItemsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously returns all items found in the content folder.
        /// </summary>
        /// <remarks>
        /// Some legacy behavior has been dropped here due to the mix of content folders and target framework
        /// folders here.
        /// </remarks>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<FrameworkSpecificGroup>> GetContentItemsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously returns all lib items without any filtering.
        /// </summary>
        /// <remarks>Use GetReferenceItemsAsync(...) for the filtered list.</remarks>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<FrameworkSpecificGroup>> GetLibItemsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously returns lib items + filtering based on the nuspec and other NuGet rules.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{FrameworkSpecificGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<FrameworkSpecificGroup>> GetReferenceItemsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously returns package dependencies.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{PackageDependencyGroup}" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<IEnumerable<PackageDependencyGroup>> GetPackageDependenciesAsync(CancellationToken cancellationToken);
    }
}
