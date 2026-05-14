// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Arcade.Common
{
    public class ZipArchiveManager : IZipArchiveManager
    {
        public ZipArchive OpenArchive(string archivePath, ZipArchiveMode mode)
            => ZipFile.Open(archivePath, mode);

        public async Task AddResourceFileToArchive<TAssembly>(string archivePath, string resourceName, string targetFileName = null)
        {
            using Stream fileStream = GetResourceFileContent<TAssembly>(resourceName);
            await AddContentToArchive(archivePath, targetFileName ?? resourceName, fileStream);
        }

        public void ArchiveDirectory(string directoryPath, string archivePath, bool includeBaseDirectory)
            => ZipFile.CreateFromDirectory(directoryPath, archivePath, CompressionLevel.Fastest, includeBaseDirectory);

        public void ArchiveFile(string filePath, string archivePath)
        {
            bool archiveExists = File.Exists(archivePath);
            string entryName = Path.GetFileName(filePath);

            using FileStream fs = File.OpenWrite(archivePath);
            using ZipArchive zip = new(fs, archiveExists ? ZipArchiveMode.Update : ZipArchiveMode.Create, false);
                
            // Overwrite previous file, if any (otherwise dual records were created)
            if (archiveExists)
            {
                zip.Entries.FirstOrDefault(e => e.FullName == entryName)?.Delete();
            }

            zip.CreateEntryFromFile(filePath, entryName);
        }

        public Task AddContentToArchive(string archivePath, string targetFilename, string content)
            => AddContentToArchive(archivePath, targetFilename, new MemoryStream(Encoding.UTF8.GetBytes(content)));

        public async Task AddContentToArchive(string archivePath, string targetFilename, Stream content)
        {
            using FileStream archiveStream = new(archivePath, FileMode.Open);
            using ZipArchive archive = new(archiveStream, ZipArchiveMode.Update);

            // Overwrite previous file, if any (otherwise dual records were created)
            archive.Entries.FirstOrDefault(e => e.FullName == targetFilename)?.Delete();

            ZipArchiveEntry entry = archive.CreateEntry(targetFilename);
            using Stream targetStream = entry.Open();
            await content.CopyToAsync(targetStream);
        }

        public static Stream GetResourceFileContent<TAssembly>(string resourceFileName)
        {
            Assembly assembly = typeof(TAssembly).Assembly;
            return assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourceFileName}");
        }
    }
}
