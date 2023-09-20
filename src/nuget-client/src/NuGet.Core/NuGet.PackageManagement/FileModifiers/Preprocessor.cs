// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Simple token replacement system for content files.
    /// </summary>
    public class Preprocessor : IPackageFileTransformer
    {
        /// <summary>
        /// Asynchronously transforms a file.
        /// </summary>
        /// <param name="streamTaskFactory">A stream task factory.</param>
        /// <param name="targetPath">A path to the file to be transformed.</param>
        /// <param name="projectSystem">The project where this change is taking place.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="streamTaskFactory" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="projectSystem" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task TransformFileAsync(
            Func<Task<Stream>> streamTaskFactory,
            string targetPath,
            IMSBuildProjectSystem projectSystem,
            CancellationToken cancellationToken)
        {
            if (streamTaskFactory == null)
            {
                throw new ArgumentNullException(nameof(streamTaskFactory));
            }

            if (projectSystem == null)
            {
                throw new ArgumentNullException(nameof(projectSystem));
            }

            cancellationToken.ThrowIfCancellationRequested();

            await MSBuildNuGetProjectSystemUtility.TryAddFileAsync(
                projectSystem,
                targetPath,
                async () => StreamUtility.StreamFromString(await ProcessAsync(streamTaskFactory, projectSystem, cancellationToken)),
                cancellationToken);
        }

        /// <summary>
        /// Asynchronously reverses the transform on the targetPath, using all the potential source of change.
        /// </summary>
        /// <param name="streamTaskFactory">A factory for accessing the file to be reverted from the nupkg being uninstalled.</param>
        /// <param name="targetPath">A path to the file to be reverted.</param>
        /// <param name="matchingFiles">Other files in other packages that may have changed the <paramref name="targetPath" />.</param>
        /// <param name="projectSystem">The project where this change is taking place.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="streamTaskFactory" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="projectSystem" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task RevertFileAsync(
            Func<Task<Stream>> streamTaskFactory,
            string targetPath,
            IEnumerable<InternalZipFileInfo> matchingFiles,
            IMSBuildProjectSystem projectSystem,
            CancellationToken cancellationToken)
        {
            if (streamTaskFactory == null)
            {
                throw new ArgumentNullException(nameof(streamTaskFactory));
            }

            if (projectSystem == null)
            {
                throw new ArgumentNullException(nameof(projectSystem));
            }

            cancellationToken.ThrowIfCancellationRequested();

            await MSBuildNuGetProjectSystemUtility.DeleteFileSafeAsync(
                targetPath,
                async () => StreamUtility.StreamFromString(await ProcessAsync(streamTaskFactory, projectSystem, cancellationToken)),
                projectSystem,
                cancellationToken);
        }

        internal static Task<string> ProcessAsync(
            Func<Task<Stream>> streamTaskFactory,
            IMSBuildProjectSystem projectSystem,
            CancellationToken cancellationToken)
        {
            return Common.Preprocessor.ProcessAsync(
                streamTaskFactory,
                propName => projectSystem.GetPropertyValue(propName),
                cancellationToken);
        }
    }
}
