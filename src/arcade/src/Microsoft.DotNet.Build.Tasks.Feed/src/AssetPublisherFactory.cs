// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER
using Azure;
using System;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Azure.Core;
using System.Collections.Concurrent;
using Microsoft.DotNet.ArcadeAzureIntegration;

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
                            GetAzureTokenCredential(task.ManagedIdentityClientId),
                            _log);
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private ConcurrentDictionary<string, TokenCredential> _tokenCredentialsPerManagedIdentity = new ConcurrentDictionary<string, TokenCredential>(-1, 10);

        private TokenCredential GetAzureTokenCredential(string managedIdentityClientId)
        {
            TokenCredential tokenCredential = _tokenCredentialsPerManagedIdentity.GetOrAdd(managedIdentityClientId ?? string.Empty, static (mi) =>
                new DefaultIdentityTokenCredential(
                    new DefaultIdentityTokenCredentialOptions
                    {
                        ManagedIdentityClientId = string.IsNullOrEmpty(mi) ? null : mi
                    }
                )
            );
            return tokenCredential;
        }
    }
}
#endif
