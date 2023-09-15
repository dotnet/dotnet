// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Web.XmlTransform;
using NuGet.PackageManagement;
using static NuGet.Shared.XmlUtility;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// An XDT project file transformer.
    /// </summary>
    public class XdtTransformer : IPackageFileTransformer
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

            await PerformXdtTransformAsync(streamTaskFactory, targetPath, projectSystem, cancellationToken);
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="matchingFiles" />
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

            await PerformXdtTransformAsync(streamTaskFactory, targetPath, projectSystem, cancellationToken);
        }

        internal static async Task PerformXdtTransformAsync(
            Func<Task<Stream>> streamTaskFactory,
            string targetPath,
            IMSBuildProjectSystem msBuildNuGetProjectSystem,
            CancellationToken cancellationToken)
        {
            if (FileSystemUtility.FileExists(msBuildNuGetProjectSystem.ProjectFullPath, targetPath))
            {
                var content = await Preprocessor.ProcessAsync(streamTaskFactory, msBuildNuGetProjectSystem, cancellationToken);

                try
                {
                    using (var transformation = new XmlTransformation(content, isTransformAFile: false, logger: null))
                    {
                        using (var document = new XmlTransformableDocument())
                        {
                            // make sure we close the input stream immediately so that we can override 
                            // the file below when we save to it.
                            string path = FileSystemUtility.GetFullPath(msBuildNuGetProjectSystem.ProjectFullPath, targetPath);
                            using (FileStream fileStream = File.OpenRead(path))
                            {
                                using var xmlReader = XmlReader.Create(fileStream, GetXmlReaderSettings(LoadOptions.PreserveWhitespace));
                                document.Load(xmlReader);
                            }

                            var succeeded = transformation.Apply(document);
                            if (succeeded)
                            {
                                // save the result into a memoryStream first so that if there is any
                                // exception during document.Save(), the original file won't be truncated.
                                MSBuildNuGetProjectSystemUtility.AddFile(msBuildNuGetProjectSystem, targetPath, document.Save);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new InvalidDataException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.XdtError + " " + exception.Message,
                            targetPath,
                            msBuildNuGetProjectSystem.ProjectName),
                        exception);
                }
            }
        }
    }
}
