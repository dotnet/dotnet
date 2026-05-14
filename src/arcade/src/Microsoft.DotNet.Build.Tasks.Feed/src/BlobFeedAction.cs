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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NuGet.Packaging.Core;
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
            }
            else
            {
                throw new Exception("Unable to parse expected feed. Please check ExpectedFeedUrl.");
            }
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

        public async Task CreateContainerAsync(IBuildEngine buildEngine)
        {
            Log.LogMessage($"Creating container {ContainerName}...");

            CreateAzureContainer createContainer = new CreateAzureContainerIfNotExists
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
        }
    }
}
