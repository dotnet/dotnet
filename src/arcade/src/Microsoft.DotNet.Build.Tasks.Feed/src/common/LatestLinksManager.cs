// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.DotNet.Deployment.Tasks.Links.src;
using Microsoft.DotNet.VersionTools.BuildManifest;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class LatestLinksManager
    {
        private TaskLoggingHelper Logger { get; }
        private AkaMSLinkManager LinkManager { get; } = null;
        private string AkaMSClientId { get; }
        private string AkaMSClientSecret { get; }
        private string AkaMSTenant { get; }
        private string AkaMsOwners { get; }
        private string AkaMSCreatedBy { get; }
        private string AkaMSGroupOwner { get; }

        private static HashSet<string> AccountsWithCdns { get; } = new()
        {
            "dotnetcli.blob.core.windows.net", "dotnetbuilds.blob.core.windows.net",
        };

        public LatestLinksManager(
            string akaMSClientId,
            string akaMSClientSecret,
            string akaMSTenant,
            string akaMSGroupOwner,
            string akaMSCreatedBy,
            string akaMsOwners,
            TaskLoggingHelper logger)
        {
            Logger = logger;
            AkaMSClientId = akaMSClientId;
            AkaMSClientSecret = akaMSClientSecret;
            AkaMSTenant = akaMSTenant;
            AkaMSGroupOwner = akaMSGroupOwner;
            AkaMSCreatedBy = akaMSCreatedBy;
            AkaMsOwners = akaMsOwners;
            LinkManager = new AkaMSLinkManager(AkaMSClientId, AkaMSClientSecret, AkaMSTenant, Logger);
        }

        public async System.Threading.Tasks.Task CreateOrUpdateLatestLinksAsync(
            HashSet<string> assetsToPublish,
            TargetFeedConfig feedConfig,
            int expectedSuffixLength)
        {
            if (!feedConfig.LatestLinkShortUrlPrefixes.Any())
            {
                return;
            }

            string feedBaseUrl = feedConfig.SafeTargetURL;
            if (expectedSuffixLength != 0)
            {
                // Strip away the feed expected suffix (index.json)
                feedBaseUrl = feedBaseUrl.Substring(0, feedBaseUrl.Length - expectedSuffixLength);
            }
            if (!feedBaseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                feedBaseUrl += "/";
            }
            if (AccountsWithCdns.Any(account => feedBaseUrl.Contains(account)))
            {
                // The storage accounts are in a single datacenter in the US and thus download 
                // times can be painful elsewhere. The CDN helps with this therefore we point the target 
                // of the aka.ms links to the CDN.
                feedBaseUrl = feedBaseUrl.Replace(".blob.core.windows.net", ".azureedge.net");
            }

            Logger.LogMessage(MessageImportance.High, "\nThe following aka.ms links for blobs will be created:");
            IEnumerable<AkaMSLink> linksToCreate = assetsToPublish
                .Where(asset => !feedConfig.FilenamesToExclude.Contains(Path.GetFileName(asset)))
                .Select(asset =>
            {

                // blob path.
                string actualTargetUrl = feedBaseUrl + asset;

                List<AkaMSLink> newLinks = new List<AkaMSLink>();
                foreach (string shortUrlPrefix in feedConfig.LatestLinkShortUrlPrefixes)
                {
                    newLinks.Add(GetAkaMSLinkForAsset(shortUrlPrefix, feedBaseUrl, asset, feedConfig.Flatten));
                }

                return newLinks;
            })
                .SelectMany(links => links)
                .ToList();

            await LinkManager.CreateOrUpdateLinksAsync(linksToCreate, AkaMsOwners, AkaMSCreatedBy, AkaMSGroupOwner, true);
        }

        /// <sunnary>
        ///     Create the aka.ms link info
        /// </summary>
        /// <param name="shortUrlPrefix">aka.ms short url prefix</param>
        /// <param name="feedBaseUrl">Base feed url for the asset</param>
        /// <param name="asset">Asset</param>
        /// <param name="flatten">If we should only use the filename when creating the aka.ms link</param>
        /// <returns>The AkaMSLink object for the asset</returns>
        public AkaMSLink GetAkaMSLinkForAsset(string shortUrlPrefix, string feedBaseUrl, string asset, bool flatten)
        {
            // blob path.
            string actualTargetUrl = feedBaseUrl + asset;

            AkaMSLink newLink = new AkaMSLink
            {
                ShortUrl = GetLatestShortUrlForBlob(shortUrlPrefix, asset, flatten),
                TargetUrl = actualTargetUrl
            };

            Logger.LogMessage(MessageImportance.High, $"  aka.ms/{newLink.ShortUrl} -> {newLink.TargetUrl}");

            return newLink;
        }

        /// <summary>
        ///     Get the short url for a blob.
        /// </summary>
        /// <param name="latestLinkShortUrlPrefix">aka.ms short url prefix</param>
        /// <param name="asset">Asset</param>
        /// <param name="flatten">If we should only use the filename when creating the aka.ms link</param>
        /// <returns>Short url prefix for the blob.</returns>
        public string GetLatestShortUrlForBlob(string latestLinkShortUrlPrefix, string asset, bool flatten)
        {
            string blobIdWithoutVersions = VersionIdentifier.RemoveVersions(asset);

            if (flatten)
            {
                blobIdWithoutVersions = Path.GetFileName(blobIdWithoutVersions);
            }

            return Path.Combine(latestLinkShortUrlPrefix, blobIdWithoutVersions).Replace("\\", "/");
        }
    }
}
