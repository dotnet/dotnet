// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER
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
                    return new AzureStorageContainerAssetPublisher(new Uri(feedConfig.TargetURL), _log);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
#endif
