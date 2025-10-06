// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// This class represents a plugin operations cache entry.
    /// It contains expiry logic, read/write/update logic.
    /// </summary>
    public sealed class PluginCacheEntry
    {
        /// <summary>
        /// Create a plugin cache entry.
        /// </summary>
        /// <param name="rootCacheFolder">The root cache folder, normally /localappdata/nuget/plugins-cache</param>
        /// <param name="pluginFilePath">The full plugin file path, which will be used to create a key for the folder created in the root folder itself </param>
        /// <param name="requestKey">A unique request key for the operation claims. Ideally the packageSourceRepository value of the PluginRequestKey. Example https://protected.package.feed/index.json, or Source-Agnostic</param>
        public PluginCacheEntry(string rootCacheFolder, string pluginFilePath, string requestKey)
        {
            RootFolder = Path.Combine(rootCacheFolder, CachingUtility.RemoveInvalidFileNameChars(CachingUtility.ComputeHash(pluginFilePath, addIdentifiableCharacters: false)));
            CacheFileName = Path.Combine(RootFolder, CachingUtility.RemoveInvalidFileNameChars(CachingUtility.ComputeHash(requestKey, addIdentifiableCharacters: false)) + ".dat");
            NewCacheFileName = CacheFileName + "-new";
        }

        internal TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(30);
        internal string CacheFileName { get; }
        private string RootFolder { get; }
        private string NewCacheFileName { get; }

        public IReadOnlyList<OperationClaim> OperationClaims { get; set; }

        /// <summary>
        /// Loads and processes the contet from the generated file if it exists.
        /// Even after this method is invoked, the operation claims might be null. 
        /// </summary>
        public void LoadFromFile()
        {
            Stream content = null;
            try
            {
                content = CachingUtility.ReadCacheFile(MaxAge, CacheFileName);
                if (content != null)
                {
                    ProcessContent(content);
                }
            }
            finally
            {
                content?.Dispose();
            }
        }

        private void ProcessContent(Stream content)
        {
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(content))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                OperationClaims = serializer.Deserialize<IReadOnlyList<OperationClaim>>(jsonTextReader);
            }
        }

        /// <summary>
        /// Updates the cache file with the current value in the operation claims if the operationn claims is not null.
        /// </summary>
        /// <returns>Task</returns>
        public async Task UpdateCacheFileAsync()
        {
            if (OperationClaims != null)
            {
                // Make sure the cache file directory is created before writing a file to it.
                Directory.CreateDirectory(RootFolder);

                // The update of a cached file is divided into two steps:
                // 1) Delete the old file.
                // 2) Create a new file with the same name.
                using (var fileStream = new FileStream(
                    NewCacheFileName,
                    FileMode.Create,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    CachingUtility.BufferSize))
                {
                    var json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(OperationClaims, Formatting.Indented));
                    await fileStream.WriteAsync(json, 0, json.Length);
                }

                if (File.Exists(CacheFileName))
                {
                    // Process B can perform deletion on an opened file if the file is opened by process A
                    // with FileShare.Delete flag. However, the file won't be actually deleted until A close it.
                    // This special feature can cause race condition, so we never delete an opened file.
                    if (!CachingUtility.IsFileAlreadyOpen(CacheFileName))
                    {
                        File.Delete(CacheFileName);
                    }
                }

                // If the destination file doesn't exist, we can safely perform moving operation.
                // Otherwise, moving operation will fail.
                if (!File.Exists(CacheFileName))
                {
                    File.Move(
                        NewCacheFileName,
                        CacheFileName);
                }
            }
        }
    }
}
