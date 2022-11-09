// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Microsoft.Arcade.Common
{
    public interface IZipArchiveManager
    {
        /// <summary>
        /// Opens a given archive.
        /// </summary>
        /// <param name="archivePath">Path to the zip archive</param>
        /// <param name="mode">Access mode</param>
        ZipArchive OpenArchive(string archivePath, ZipArchiveMode mode);

        /// <summary>
        /// Loads an embedded resource and adds it to a target archive.
        /// </summary>
        /// <typeparam name="TAssembly">Type from the assembly where the resource is embedded (usually the caller)</typeparam>
        /// <param name="archivePath">Path to the archive where the file will be added</param>
        /// <param name="resourceName">Name of the embedded resource</param>
        /// <param name="targetFileName">New name of the file in the archive</param>
        Task AddResourceFileToArchive<TAssembly>(string archivePath, string resourceName, string targetFileName = null);

        /// <summary>
        /// Creates a file with given content in a given archive.
        /// </summary>
        /// <param name="archivePath">Path to the archive</param>
        /// <param name="targetFilename">New path of the file in the archive</param>
        /// <param name="content">Content of the file</param>
        Task AddContentToArchive(string archivePath, string targetFilename, Stream content);

        /// <summary>
        /// Creates a file with given content in a given archive.
        /// </summary>
        /// <param name="archivePath">Path to the archive</param>
        /// <param name="targetFilename">New path of the file in the archive</param>
        /// <param name="content">Content of the file</param>
        Task AddContentToArchive(string archivePath, string targetFilename, string content);

        /// <summary>
        /// Compresses a directory into an archive on a given path.
        /// </summary>
        /// <param name="directoryPath">The directory to archive</param>
        /// <param name="archivePath">Path where to create the archive</param>
        /// <param name="includeBaseDirectory">When true, includes top-level directory in the archive</param>
        void ArchiveDirectory(string directoryPath, string archivePath, bool includeBaseDirectory);

        /// <summary>
        /// Creates a new archive containing given file.
        /// </summary>
        /// <param name="filePath">File to archive</param>
        /// <param name="archivePath">Path where to create the archive</param>
        void ArchiveFile(string filePath, string archivePath);
    }
}
