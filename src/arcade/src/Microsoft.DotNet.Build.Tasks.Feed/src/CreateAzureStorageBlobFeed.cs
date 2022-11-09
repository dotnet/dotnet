// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Storage.Blobs.Models;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.CloudTestTasks;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MSBuild = Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class CreateAzureStorageBlobFeed : MSBuild.Task
    {
        [Output]
        public string TargetFeedURL { get; set; }

        [Required]
        public string RepositoryName { get; set; }

        [Required]
        public string CommitSha { get; set; }

        [Required]
        public string AzureStorageFeedsBaseUrl { get; set; }

        [Required]
        public string AzureStorageAccountName { get; set; }

        [Required]
        public string AzureStorageAccountKey { get; set; }

        [Required]
        public bool IsInternal { get; set; }

        private const string baseUrlRegex = @"https:\/\/[^/]+/container/(?<containername>[^/]+).*";

        public override bool Execute()
        {
            return ExecuteAsync().GetAwaiter().GetResult();
        }

        private async Task<bool> ExecuteAsync()
        {
            try
            {
                string accessId = IsInternal ? "int" : "pub";
                string baseFeedName = $"darc-{accessId}-{RepositoryName}-{CommitSha}";
                string versionedFeedName = baseFeedName;
                bool needsUniqueName = false;
                int subVersion = 0;
                var containerName = string.Empty;

                Log.LogMessage(MessageImportance.High, $"Creating a new Azure Storage internal feed ...");

                Match m = Regex.Match(AzureStorageFeedsBaseUrl, baseUrlRegex);
                if (m.Success)
                {
                    containerName = m.Groups["containername"].Value;
                }
                else
                {
                    Log.LogError($"Could not parse {nameof(AzureStorageFeedsBaseUrl)} to extract the container name: '{AzureStorageFeedsBaseUrl}'");
                    return false;
                }

                AzureStorageUtils azUtils = new AzureStorageUtils(AzureStorageAccountName, AzureStorageAccountKey, containerName);

                // Create container if it doesn't already exist
                if (!await azUtils.CheckIfContainerExistsAsync())
                {
                    PublicAccessType permissions = IsInternal ? PublicAccessType.None : PublicAccessType.BlobContainer;
                    await azUtils.CreateContainerAsync(permissions);
                }

                // Create folder inside the container. Note that AzureStorage requires a folder
                // to have at least one file.
                do
                {
                    if (await azUtils.CheckIfBlobExistsAsync($"{versionedFeedName}/index.json"))
                    {
                        versionedFeedName = $"{baseFeedName}-{++subVersion}";
                        needsUniqueName = true;
                    }
                    else
                    {
                        baseFeedName = versionedFeedName;
                        needsUniqueName = false;
                    }
                } while (needsUniqueName);

                // Initialize the feed using sleet
                SleetSource sleetSource = new SleetSource()
                {
                    Name = baseFeedName,
                    Type = "azure",
                    BaseUri = $"{AzureStorageFeedsBaseUrl}{baseFeedName}",
                    AccountName = AzureStorageAccountName,
                    Container = containerName,
                    FeedSubPath = $"{baseFeedName}",
                    ConnectionString = $"DefaultEndpointsProtocol=https;AccountName={AzureStorageAccountName};AccountKey={AzureStorageAccountKey};EndpointSuffix=core.windows.net"
                };

                BlobFeedAction bfAction = new BlobFeedAction(sleetSource, AzureStorageAccountKey, Log);
                await bfAction.InitAsync();

                TargetFeedURL = $"{AzureStorageFeedsBaseUrl}{baseFeedName}";

                Log.LogMessage(MessageImportance.High, $"Feed '{TargetFeedURL}' created successfully!");
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
