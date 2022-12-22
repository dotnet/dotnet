using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Arcade.Common;

namespace Microsoft.DotNet.Helix.Client
{
    internal class DirectoryPayload : IPayload
    {
        private static readonly IHelpers s_helpers = new Helpers();

        public DirectoryPayload(string directory, string archiveEntryPrefix)
        {
            ArchiveEntryPrefix = archiveEntryPrefix;
            DirectoryInfo = new DirectoryInfo(directory);
            if (!DirectoryInfo.Exists)
            {
                throw new DirectoryNotFoundException($"The directory '{directory}' was not found.");
            }
        }

        private const int CacheExpiryHours = 5;
        public DirectoryInfo DirectoryInfo { get; }

        public string NormalizedDirectoryPath => s_helpers.RemoveTrailingSlash(DirectoryInfo.FullName);

        public string ArchiveEntryPrefix { get; }

        public Task<string> UploadAsync(IBlobContainer payloadContainer, Action<string> log, CancellationToken cancellationToken)
            => Task.FromResult(
                s_helpers.DirectoryMutexExec(
                    () => DoUploadAsync(payloadContainer, log, cancellationToken),
                    NormalizedDirectoryPath));

        private async Task<string> DoUploadAsync(IBlobContainer payloadContainer, Action<string> log, CancellationToken cancellationToken)
        {
            await Task.Yield();
            string basePath = NormalizedDirectoryPath;

            var alreadyUploadedFile = new FileInfo(basePath + ".payload");
            if (alreadyUploadedFile.Exists && IsUpToDate(alreadyUploadedFile))
            {
                log?.Invoke($"Using previously uploaded payload for {basePath}");
                return File.ReadAllText(alreadyUploadedFile.FullName);
            }

            log?.Invoke($"Uploading payload for {basePath}");
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    foreach (FileInfo file in DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                    {
                        string relativePath =
                            file.FullName.Substring(basePath.Length + 1); // +1 prevents it from including the leading backslash
                        string zipEntryName = relativePath.Replace('\\', '/'); // Normalize slashes

                        if (!string.IsNullOrEmpty(ArchiveEntryPrefix))
                        {
                            zipEntryName = ArchiveEntryPrefix + "/" + zipEntryName;
                        }

                        zip.CreateEntryFromFile(file.FullName, zipEntryName);
                    }
                }

                stream.Position = 0;
                Uri zipUri = await payloadContainer.UploadFileAsync(stream, $"{Guid.NewGuid()}.zip", log, cancellationToken);
                File.WriteAllText(alreadyUploadedFile.FullName, zipUri.AbsoluteUri);
                return zipUri.AbsoluteUri;
            }
        }

        private bool IsUpToDate(FileInfo alreadyUploadedFile)
        {
            if (alreadyUploadedFile.LastWriteTimeUtc.AddHours(CacheExpiryHours) < DateTime.UtcNow)
            {
                return false;
            }

            var newestFileWriteTime = DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories)
                .Select(file => file.LastWriteTimeUtc)
                .Max();
            return alreadyUploadedFile.LastWriteTimeUtc >= newestFileWriteTime;
        }
    }
}
