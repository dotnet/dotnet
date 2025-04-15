// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Build.Tasks.Feed.Model;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public abstract class SetupTargetFeedConfigBase
    {
        protected bool IsInternalBuild { get; set; }
        protected string RepositoryName { get; set; }
        protected string CommitSha { get; set; }
        protected bool PublishInstallersAndChecksums { get; set; }
        protected string InstallersTargetStaticFeed { get; set; }
        protected string InstallersAzureAccountKey { get; set; }
        protected string ChecksumsTargetStaticFeed { get; set; }
        protected string ChecksumsAzureAccountKey { get; set; }
        protected string AzureDevOpsStaticShippingFeed { get; set; }
        protected string AzureDevOpsStaticTransportFeed { get; set; }
        protected string AzureDevOpsStaticSymbolsFeed { get; set; }
        protected ImmutableList<string> LatestLinkShortUrlPrefixes { get; set; }
        protected string AzureDevOpsFeedsKey { get; set; }

        protected SetupTargetFeedConfigBase(
            bool isInternalBuild,
            string repositoryName,
            string commitSha,
            bool publishInstallersAndChecksums,
            string installersTargetStaticFeed,
            string installersAzureAccountKey,
            string checksumsTargetStaticFeed,
            string checksumsAzureAccountKey,
            string azureDevOpsStaticShippingFeed,
            string azureDevOpsStaticTransportFeed,
            string azureDevOpsStaticSymbolsFeed,
            ImmutableList<string> latestLinkShortUrlPrefixes,
            string azureDevOpsFeedsKey)
        {
            IsInternalBuild = isInternalBuild;
            RepositoryName = repositoryName;
            CommitSha = commitSha;
            PublishInstallersAndChecksums = publishInstallersAndChecksums;
            InstallersTargetStaticFeed = installersTargetStaticFeed;
            InstallersAzureAccountKey = installersAzureAccountKey;
            ChecksumsTargetStaticFeed = checksumsTargetStaticFeed;
            ChecksumsAzureAccountKey = checksumsAzureAccountKey;
            AzureDevOpsStaticShippingFeed = azureDevOpsStaticShippingFeed;
            AzureDevOpsStaticTransportFeed = azureDevOpsStaticTransportFeed;
            AzureDevOpsStaticSymbolsFeed = azureDevOpsStaticSymbolsFeed;
            LatestLinkShortUrlPrefixes = latestLinkShortUrlPrefixes;
            AzureDevOpsFeedsKey = azureDevOpsFeedsKey;
        }
        public abstract List<TargetFeedConfig> Setup();
    }
}
