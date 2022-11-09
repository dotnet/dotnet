// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.CloudTestTasks;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NuGet.Packaging.Core;
using Sleet;
using MSBuild = Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    sealed class BlobFeedAction
    {
        private MSBuild.TaskLoggingHelper Log;
        private static readonly CancellationTokenSource TokenSource = new CancellationTokenSource();
        private static readonly CancellationToken CancellationToken = TokenSource.Token;
        private const string feedRegex = @"(?<feedurl>https:\/\/(?<accountname>[^\.-]+)(?<domain>[^\/]*)\/((?<token>[a-zA-Z0-9+\/]*?\/\d{4}-\d{2}-\d{2})\/)?(?<containername>[^\/]+)\/(?<relativepath>.*\/)?)index\.json";
        private string feedUrl;
        private SleetSource source;
        private bool hasToken = false;

        public string AccountName { get; }
        public string AccountKey { get; }
        public string ContainerName { get; }
        public string RelativePath { get; }

        public BlobFeedAction(string expectedFeedUrl, string accountKey, MSBuild.TaskLoggingHelper Log)
        {
            // This blob feed action regex is custom because of the way that NuGet handles query strings (it doesn't)
            // Instead of encoding the query string containing the SAS at the end of the URL we encode it at the beginning.
            // As a result, we can't parse this feed url like a traditional feed url.  When this changes, this code could be simplified and
            // BlobUriParser could be used instead.
            this.Log = Log;
            Match m = Regex.Match(expectedFeedUrl, feedRegex);
            if (m.Success)
            {
                AccountKey = accountKey;
                AccountName = m.Groups["accountname"].Value;
                ContainerName = m.Groups["containername"].Value;
                RelativePath = m.Groups["relativepath"].Value;

                feedUrl = m.Groups["feedurl"].Value;
                hasToken = !string.IsNullOrEmpty(m.Groups["token"].Value);

                source = new SleetSource
                {
                    Name = ContainerName,
                    Type = "azure",
                    Path = feedUrl,
                    Container = ContainerName,
                    FeedSubPath = RelativePath,
                    ConnectionString = $"DefaultEndpointsProtocol=https;AccountName={AccountName};AccountKey={AccountKey};EndpointSuffix=core.windows.net"
                };
            }
            else
            {
                throw new Exception("Unable to parse expected feed. Please check ExpectedFeedUrl.");
            }
        }

        public BlobFeedAction(SleetSource sleetSource, string accountKey, MSBuild.TaskLoggingHelper log)
        {
            ContainerName = sleetSource.Container;
            RelativePath = sleetSource.FeedSubPath;
            AccountName = sleetSource.AccountName;
            AccountKey = accountKey;
            hasToken = true;
            Log = log;
            source = sleetSource;
        }

        public async Task<bool> PushToFeedAsync(
            IEnumerable<string> items,
            PushOptions options)
        {
            if (IsSanityChecked(items))
            {
                if (CancellationToken.IsCancellationRequested)
                {
                    Log.LogError("Task PushToFeed cancelled");
                    CancellationToken.ThrowIfCancellationRequested();
                }

                await PushItemsToFeedAsync(items, options);
            }

            return !Log.HasLoggedErrors;
        }

        public async Task<bool> PushItemsToFeedAsync(
            IEnumerable<string> items,
            PushOptions options)
        {
            Log.LogMessage(MessageImportance.Low, $"START pushing items to feed");

            if (!items.Any())
            {
                Log.LogMessage("No items to push found in the items list.");
                return true;
            }

            try
            {
                return await PushAsync(items, options);
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
            }

            return !Log.HasLoggedErrors;
        }

        public async Task PublishToFlatContainerAsync(IEnumerable<ITaskItem> taskItems, int maxClients,
            PushOptions pushOptions)
        {
            if (taskItems.Any())
            {
                using (var clientThrottle = new SemaphoreSlim(maxClients, maxClients))
                {
                    await System.Threading.Tasks.Task.WhenAll(taskItems.Select(
                        item => { return UploadAssetAsync(item, pushOptions, clientThrottle); }
                    ));
                }
            }
        }

        public async Task UploadAssetAsync(
            ITaskItem item,
            PushOptions options,
            SemaphoreSlim clientThrottle = null)
        {
            string relativeBlobPath = item.GetMetadata("RelativeBlobPath");

            if (string.IsNullOrEmpty(relativeBlobPath))
            {
                string fileName = Path.GetFileName(item.ItemSpec);
                string recursiveDir = item.GetMetadata("RecursiveDir");
                relativeBlobPath = $"{recursiveDir}{fileName}";
            }

            if (!string.IsNullOrEmpty(relativeBlobPath))
            {
                relativeBlobPath = $"{RelativePath}{relativeBlobPath}".Replace("\\", "/");

                if (relativeBlobPath.StartsWith("//"))
                {
                    Log.LogError(
                        $"Item '{item.ItemSpec}' RelativeBlobPath contains virtual directory " +
                        $"without name (double forward slash): '{relativeBlobPath}'");
                    return;
                }

                Log.LogMessage($"Uploading {relativeBlobPath}");

                if (clientThrottle != null)
                {
                    await clientThrottle.WaitAsync();
                }

                try
                {
                    AzureStorageUtils blobUtils = new AzureStorageUtils(AccountName, AccountKey, ContainerName);

                    if (!options.AllowOverwrite && await blobUtils.CheckIfBlobExistsAsync(relativeBlobPath))
                    {
                        if (options.PassIfExistingItemIdentical)
                        {
                            if (!await blobUtils.IsFileIdenticalToBlobAsync(item.ItemSpec, relativeBlobPath))
                            {
                                Log.LogError(
                                    $"Item '{item}' already exists with different contents " +
                                    $"at '{relativeBlobPath}'");
                            }
                        }
                        else
                        {
                            Log.LogError($"Item '{item}' already exists at '{relativeBlobPath}'");
                        }
                    }
                    else
                    {
                        using (FileStream stream =
                            new FileStream(item.ItemSpec, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            Log.LogMessage($"Uploading {item} to {relativeBlobPath}.");
                            await blobUtils.UploadBlockBlobAsync(item.ItemSpec, relativeBlobPath, stream);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Log.LogError(
                        $"Unable to upload to {relativeBlobPath} in Azure Storage account {AccountName}/{ContainerName} due to {exc}.");
                    throw;
                }
                finally
                {
                    if (clientThrottle != null)
                    {
                        clientThrottle.Release();
                    }
                }
            }
            else
            {
                Log.LogError($"Relative blob path is empty.");
            }
        }

        public async Task CreateContainerAsync(IBuildEngine buildEngine, bool publishFlatContainer)
        {
            Log.LogMessage($"Creating container {ContainerName}...");

            CreateAzureContainer createContainer = new CreateAzureContainer
            {
                AccountKey = AccountKey,
                AccountName = AccountName,
                ContainerName = ContainerName,
                FailIfExists = false,
                IsPublic = !hasToken,
                BuildEngine = buildEngine
            };

            await createContainer.ExecuteAsync();

            Log.LogMessage($"Creating container {ContainerName} succeeded!");

            if (!publishFlatContainer)
            {
                try
                {
                    bool result = await InitAsync();

                    if (result)
                    {
                        Log.LogMessage($"Initializing sub-feed {source.FeedSubPath} succeeded!");
                    }
                    else
                    {
                        throw new Exception($"Initializing sub-feed {source.FeedSubPath} failed!");
                    }
                }
                catch (Exception e)
                {
                    Log.LogErrorFromException(e);
                }
            }
        }

        public async Task<ISet<PackageIdentity>> GetPackageIdentitiesAsync()
        {
            using (var fileCache = CreateFileCache())
            {
                var context = new SleetContext
                {
                    LocalSettings = GetSettings(),
                    Log = new SleetLogger(Log, NuGet.Common.LogLevel.Verbose),
                    Source = GetAzureFileSystem(fileCache),
                    Token = CancellationToken
                };
                context.SourceSettings = await FeedSettingsUtility.GetSettingsOrDefault(
                    context.Source,
                    context.Log,
                    context.Token);

                var packageIndex = new PackageIndex(context);

                return await packageIndex.GetPackagesAsync();
            }
        }

        private bool IsSanityChecked(IEnumerable<string> items)
        {
            Log.LogMessage(MessageImportance.Low, $"START checking sanitized items for feed");
            foreach (var item in items)
            {
                if (items.Any(s => Path.GetExtension(item) != ".nupkg"))
                {
                    Log.LogError($"{item} is not a nupkg");
                    return false;
                }
            }
            List<string> duplicates = items.GroupBy(x => x)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key).ToList();
            if (duplicates.Count > 0)
            {
                Log.LogError($"Duplicates found: {string.Join(", ", duplicates)}");
                return false;
            }
            Log.LogMessage(MessageImportance.Low, $"DONE checking for sanitized items for feed");
            return true;
        }

        private LocalSettings GetSettings()
        {
            SleetSettings sleetSettings = new SleetSettings()
            {
                Sources = new List<SleetSource>
                    {
                       source
                    }
            };

            var jsonSerializer = JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });

            LocalSettings settings = new LocalSettings
            {
                Json = JObject.FromObject(sleetSettings, jsonSerializer)
            };

            return settings;
        }

        private ISleetFileSystem GetAzureFileSystem(LocalCache fileCache)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(source.ConnectionString);
                return new AzureFileSystem(fileCache, new Uri(source.Path), new Uri(source.Path), storageAccount, source.Name, source.FeedSubPath);
            }
            catch
            {
                return FileSystemFactory.CreateFileSystem(GetSettings(), fileCache, source.Name);
            }
        }

        public async Task<bool> PushAsync(IEnumerable<string> items, PushOptions options)
        {
            LocalSettings settings = GetSettings();
            SleetLogger log = new SleetLogger(Log, NuGet.Common.LogLevel.Verbose);
            var packagesToPush = items.ToList();

            // Create a new cache to be used once a lock is obtained.
            using (var fileCache = CreateFileCache())
            {
                var lockedFileSystem = GetAzureFileSystem(fileCache);

                return await PushCommand.RunAsync(
                    settings,
                    lockedFileSystem,
                    packagesToPush,
                    force: options.AllowOverwrite,
                    skipExisting: !options.AllowOverwrite,
                    log: log);
            }
        }

        public async Task<bool> InitAsync()
        {
            AzureStorageUtils blobUtils = new AzureStorageUtils(AccountName, AccountKey, ContainerName);

            if (!await blobUtils.CheckIfContainerExistsAsync())
            {
                throw new Exception($"The informed container for the feed '{ContainerName}' doesn't exist!");
            }

            using (var fileCache = CreateFileCache())
            {
                LocalSettings settings = GetSettings();
                var fileSystem = FileSystemFactory.CreateFileSystem(settings, fileCache, source.Name);
                bool result = await InitCommand.RunAsync(settings, fileSystem, enableCatalog: false, enableSymbols: false, log: new SleetLogger(Log, NuGet.Common.LogLevel.Verbose), token: CancellationToken);
                return result;
            }
        }

        private static LocalCache CreateFileCache()
        {
            // By default a folder is created inside %temp% to store the cache, to 
            // change this location pass a folder path to the LocalCache constructor.
            // Passing PerfTracker in so a summary is logged at the end of publishing.
            return new LocalCache(new PerfTracker());
        }
    }
}
