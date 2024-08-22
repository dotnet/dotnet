// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public static class HttpCacheUtility
    {
        private const int BufferSize = 8192;

        public static HttpCacheResult InitializeHttpCacheResult(
            string httpCacheDirectory,
            Uri sourceUri,
            string cacheKey,
            HttpSourceCacheContext context)
        {
            // When the MaxAge is TimeSpan.Zero, this means the caller is passing is using a temporary directory for
            // cache files, instead of the global HTTP cache used by default. Additionally, the cleaning up of this
            // directory is the responsibility of the caller.
            if (context.MaxAge > TimeSpan.Zero)
            {
                var baseFolderName = CachingUtility.RemoveInvalidFileNameChars(CachingUtility.ComputeHash(sourceUri.OriginalString));
                var baseFileName = CachingUtility.RemoveInvalidFileNameChars(cacheKey) + ".dat";
                var cacheFolder = Path.Combine(httpCacheDirectory, baseFolderName);
                var cacheFile = Path.Combine(cacheFolder, baseFileName);
                var newCacheFile = cacheFile + "-new";

                return new HttpCacheResult(
                    context.MaxAge,
                    newCacheFile,
                    cacheFile);
            }
            else
            {
                var temporaryFile = Path.Combine(context.RootTempFolder, Path.GetRandomFileName());
                var newTemporaryFile = Path.Combine(context.RootTempFolder, Path.GetRandomFileName());

                return new HttpCacheResult(
                    context.MaxAge,
                    newTemporaryFile,
                    temporaryFile);
            }
        }

        public static async Task CreateCacheFileAsync(
            HttpCacheResult result,
            HttpResponseMessage response,
            Action<Stream> ensureValidContents,
            CancellationToken cancellationToken)
        {
            // Get the cache file directories, so we can make sure they are created before writing
            // files to them.
            var newCacheFileDirectory = Path.GetDirectoryName(result.NewFile);
            var cacheFileDirectory = Path.GetDirectoryName(result.CacheFile);

            // Make sure the new cache file directory is created before writing a file to it.
            Directory.CreateDirectory(newCacheFileDirectory);

            // The update of a cached file is divided into two steps:
            // 1) Delete the old file.
            // 2) Create a new file with the same name.
            using (var fileStream = new FileStream(
                result.NewFile,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None,
                BufferSize))
            {
#if NETCOREAPP2_0_OR_GREATER
                using (var networkStream = await response.Content.ReadAsStreamAsync(cancellationToken))
#else
                using (var networkStream = await response.Content.ReadAsStreamAsync())
#endif
                {
                    await networkStream.CopyToAsync(fileStream, BufferSize, cancellationToken);
                }

                // Validate the content before putting it into the cache.
                if (ensureValidContents != null)
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    ensureValidContents.Invoke(fileStream);
                }
            }

            if (File.Exists(result.CacheFile))
            {
                // Process B can perform deletion on an opened file if the file is opened by process A
                // with FileShare.Delete flag. However, the file won't be actually deleted until A close it.
                // This special feature can cause race condition, so we never delete an opened file.
                if (!CachingUtility.IsFileAlreadyOpen(result.CacheFile))
                {
                    File.Delete(result.CacheFile);
                }
            }

            // Make sure the cache file directory is created before moving or writing a file to it.
            if (cacheFileDirectory != newCacheFileDirectory)
            {
                Directory.CreateDirectory(cacheFileDirectory);
            }

            // If the destination file doesn't exist, we can safely perform moving operation.
            // Otherwise, moving operation will fail.
            if (!File.Exists(result.CacheFile))
            {
                File.Move(
                    result.NewFile,
                    result.CacheFile);
            }

            // Even the file deletion operation above succeeds but the file is not actually deleted,
            // we can still safely read it because it means that some other process just updated it
            // and we don't need to update it with the same content again.
            result.Stream = new FileStream(
                result.CacheFile,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read | FileShare.Delete,
                BufferSize);
        }
    }
}
