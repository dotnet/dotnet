// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Core;
using Azure.Identity;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Feed.src
{
    public class PublishSignedAssets : PublishArtifactsInManifestBase
    {
        private static readonly string AzureDevOpsScope = "499b84ac-1321-427f-aa17-267ca6975798/.default";

        /// <summary>
        /// Required token to publishe packages to the feeds
        /// </summary>
        public string AzureDevOpsPersonalAccessToken { get; set; }

        /// <summary>
        /// The name of the feed for "shipping" packages
        /// </summary>
        [Required]
        public string ShippingFeedName { get; set; }

        /// <summary>
        /// The name of the feed for the "nonshipping" packages
        /// </summary>
        [Required]
        public string NonShippingFeedName { get; set; }

        /// <summary>
        /// Folder which contains the "shipping" assets
        /// </summary>
        [Required]
        public string ShippingAssetsFolder { get; set; }

        /// <summary>
        /// Folder which contains the "nonshipping" assets
        /// </summary>
        [Required]
        public string NonShippingAssetsFolder { get; set; }

        public override bool Execute()
        {
            return ExecuteAsync().GetAwaiter().GetResult();
        }

        public override async Task<bool> ExecuteAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(AzureDevOpsPersonalAccessToken))
                {
                    AzureDevOpsPersonalAccessToken = (await new AzureCliCredential().GetTokenAsync(new TokenRequestContext(new[] { AzureDevOpsScope }))).Token;
                }

                // Push shipping packages
                await PushPackagesToFeed(ShippingAssetsFolder, ShippingFeedName);

                // Push nonshipping packages
                await PushPackagesToFeed(NonShippingAssetsFolder, NonShippingFeedName);
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }

        private async Task PushPackagesToFeed(string assetsFolder, string feedUrl)
        {
            string packagesFolder = Path.Combine(assetsFolder, "packages");

            TargetFeedConfig targetFeedConfig = new TargetFeedConfig(TargetFeedContentType.Package, feedUrl, FeedType.AzDoNugetFeed, AzureDevOpsPersonalAccessToken);
            HashSet<PackageIdentity> packagesToPublish = new HashSet<PackageIdentity>(
                Directory.GetFiles(packagesFolder).Select(packagePath =>
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(packagePath, FileMode.Open)))
                    {
                        PackageArchiveReader packageReader = new PackageArchiveReader(reader.BaseStream);
                        return packageReader.NuspecReader.GetIdentity();
                    }
                }));

            await PushNugetPackagesAsync<PackageIdentity>(packagesToPublish, targetFeedConfig, 5,
                async (feed, httpClient, package, feedAccount, feedVisibility, feedName) =>
                {
                    string localPackagePath = Path.Combine(packagesFolder, $"{package.Id}.{package.Version}.nupkg");

                    if (!File.Exists(localPackagePath))
                    {
                        Log.LogError($"Could not locate '{package.Id}.{package.Version}' at '{localPackagePath}'");
                        return;
                    }

                    await PushNugetPackageAsync(feed, httpClient, localPackagePath, package.Id, package.Version.ToString(), feedAccount, feedVisibility, feedName);
                });
        }

        public PublishSignedAssets() : base()
        {
        }
    }
}
