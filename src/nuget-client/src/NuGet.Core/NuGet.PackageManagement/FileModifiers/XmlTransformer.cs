// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// An XML project file transformer.
    /// </summary>
    public class XmlTransformer : IPackageFileTransformer
    {
        private readonly IDictionary<XName, Action<XElement, XElement>> _nodeActions;

        /// <summary>
        /// Initializes a new <see cref="XmlTransformer" /> class.
        /// </summary>
        /// <param name="nodeActions">A dictionary of XML node names to node actions.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nodeActions" />
        /// is <see langword="null" />.</exception>
        public XmlTransformer(IDictionary<XName, Action<XElement, XElement>> nodeActions)
        {
            if (nodeActions == null)
            {
                throw new ArgumentNullException(nameof(nodeActions));
            }

            _nodeActions = nodeActions;
        }

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

            // Get the xml fragment
            var xmlFragment = await GetXmlAsync(streamTaskFactory, projectSystem, cancellationToken);

            var transformDocument = MSBuildNuGetProjectSystemUtility.GetOrCreateDocument(xmlFragment.Name, targetPath, projectSystem);

            // Do a merge
            transformDocument.Root.MergeWith(xmlFragment, _nodeActions);

            MSBuildNuGetProjectSystemUtility.AddFile(projectSystem, targetPath, transformDocument.Save);
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

            // Get the xml snippet
            var xmlFragment = await GetXmlAsync(streamTaskFactory, projectSystem, cancellationToken);

            var document = XmlUtility.GetOrCreateDocument(xmlFragment.Name,
                projectSystem.ProjectFullPath,
                targetPath,
                projectSystem.NuGetProjectContext);

            // Merge the other xml elements into one element within this xml hierarchy (matching the config file path)
            var elements = new List<XElement>();

            foreach (var matchingFile in matchingFiles)
            {
                elements.Add(await GetXmlAsync(matchingFile, projectSystem, cancellationToken));
            }

            var mergedFragments = elements.Aggregate(
                new XElement(xmlFragment.Name),
                (left, right) => left.MergeWith(right, _nodeActions));

            // Take the difference of the xml and remove it from the main xml file
            document.Root.Except(xmlFragment.Except(mergedFragments));

            // Save the new content to the file system
            using (var fileStream = FileSystemUtility.CreateFile(
                projectSystem.ProjectFullPath,
                targetPath,
                projectSystem.NuGetProjectContext))
            {
                document.Save(fileStream);
            }
        }

        private static async Task<XElement> GetXmlAsync(
            InternalZipFileInfo packageFileInfo,
            IMSBuildProjectSystem projectSystem,
            CancellationToken cancellationToken)
        {
            string content;

            using var packageStream = File.OpenRead(packageFileInfo.ZipArchivePath);
            using var zipArchive = new ZipArchive(packageStream);

            var zipArchivePackageEntry = PathUtility.GetEntry(zipArchive, packageFileInfo.ZipArchiveEntryFullName);

            if (zipArchivePackageEntry == null)
            {
                throw new ArgumentException("internalZipFileInfo");
            }

            content = await Preprocessor.ProcessAsync(
                () => Task.FromResult(zipArchivePackageEntry.Open()),
                projectSystem,
                cancellationToken);

            return XElement.Parse(content, LoadOptions.PreserveWhitespace);
        }

        private static async Task<XElement> GetXmlAsync(
            Func<Task<Stream>> streamTaskFactory,
            IMSBuildProjectSystem projectSystem,
            CancellationToken cancellationToken)
        {
            var content = await Preprocessor.ProcessAsync(streamTaskFactory, projectSystem, cancellationToken);

            return XElement.Parse(content, LoadOptions.PreserveWhitespace);
        }
    }
}
