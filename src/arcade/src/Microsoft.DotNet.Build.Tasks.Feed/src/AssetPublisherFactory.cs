// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER
using Azure;
using Azure.Identity;
using System;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Feed.Model;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class AssetPublisherFactory
    {
        private readonly TaskLoggingHelper _log;

        public AssetPublisherFactory(TaskLoggingHelper log)
        {
            _log = log;
        }

        public virtual IAssetPublisher CreateAssetPublisher(TargetFeedConfig feedConfig, PublishArtifactsInManifestBase task)
        {
            switch (feedConfig.Type)
            {
                case FeedType.AzDoNugetFeed:
                    return new AzureDevOpsNugetFeedAssetPublisher(_log, feedConfig.TargetURL, feedConfig.Token, task);
                case FeedType.AzureStorageContainer:
                    // If there is a SAS URI specified, use that. Otherwise use the default azure credential
                    if (!string.IsNullOrEmpty(feedConfig.Token))
                    {
                        return new AzureStorageContainerAssetSasCredentialPublisher(
                            new Uri(feedConfig.TargetURL),
                            new AzureSasCredential(new Uri(feedConfig.Token).Query),
                            _log);
                    }
                    else
                    {
                        return new AzureStorageContainerAssetTokenCredentialPublisher(
                            new Uri(feedConfig.TargetURL),
                            new DefaultAzureCredential(),
                            _log);
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
#endif
