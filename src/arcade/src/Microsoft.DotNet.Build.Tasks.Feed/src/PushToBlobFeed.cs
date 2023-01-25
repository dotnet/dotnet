// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class PushToBlobFeed : MSBuildTaskBase
    {
        [Required]
        public string ExpectedFeedUrl { get; set; }

        [Required]
        public string AccountKey { get; set; }

        [Required]
        public ITaskItem[] ItemsToPush { get; set; }

        public bool Overwrite { get; set; }

        /// <summary>
        /// Enables idempotency when Overwrite is false.
        /// 
        /// false: (default) Attempting to upload an item that already exists fails.
        /// 
        /// true: When an item already exists, download the existing blob to check if it's
        /// byte-for-byte identical to the one being uploaded. If so, pass. If not, fail.
        /// </summary>
        public bool PassIfExistingItemIdentical { get; set; }

        public bool PublishFlatContainer { get; set; }

        public int MaxClients { get; set; } = 8;

        public bool SkipCreateContainer { get; set; } = false;

        public int UploadTimeoutInMinutes { get; set; } = 5;

        public string ManifestRepoUri { get; set; }

        public string ManifestBuildId { get; set; } = "no build id provided";

        public string ManifestBranch { get; set; }

        public string ManifestCommit { get; set; }

        public string[] ManifestBuildData { get; set; }

        public string AssetManifestPath { get; set; }

        public bool IsStableBuild { get; set; }

        public bool IsReleaseOnlyPackageVersion { get; set; }

        private IBuildModelFactory _buildModelFactory;
        private IPackageArtifactModelFactory _packageArtifactModelFactory;
        private IBlobArtifactModelFactory _blobArtifactModelFactory;

        public override void ConfigureServices(IServiceCollection collection)
        {
            collection.TryAddSingleton<IBuildModelFactory, BuildModelFactory>();
            collection.TryAddSingleton<ISigningInformationModelFactory, SigningInformationModelFactory>();
            collection.TryAddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>();
            collection.TryAddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>();
            collection.TryAddSingleton<INupkgInfoFactory, NupkgInfoFactory>();
            collection.TryAddSingleton<IPackageArchiveReaderFactory, PackageArchiveReaderFactory>();
            collection.TryAddSingleton<IFileSystem, FileSystem>();
            collection.TryAddSingleton(Log);
        }

        public bool ExecuteTask(IBuildModelFactory buildModelFactory,
            IBlobArtifactModelFactory blobArtifactModelFactory,
            IPackageArtifactModelFactory packageArtifactModelFactory)
        {
            _buildModelFactory = buildModelFactory;
            _packageArtifactModelFactory = packageArtifactModelFactory;
            _blobArtifactModelFactory = blobArtifactModelFactory;

            return ExecuteAsync().GetAwaiter().GetResult();
        }

        private async Task<bool> ExecuteAsync()
        {
            try
            {
                Log.LogMessage(MessageImportance.High, "Performing feed push...");

                if (!PublishFlatContainer)
                {
                    Log.LogError($"Blob feeds are no longer supported. PublishFlatContainer == false no longer supported.");
                }
                else if (ItemsToPush == null)
                {
                    Log.LogError($"No items to push. Please check ItemGroup ItemsToPush.");
                }
                else if (string.IsNullOrWhiteSpace(ExpectedFeedUrl) || string.IsNullOrWhiteSpace(AccountKey))
                {
                    Log.LogError($"{nameof(ExpectedFeedUrl)} / {nameof(AccountKey)} is not set properly.");
                }
                else if (string.IsNullOrWhiteSpace(AssetManifestPath))
                {
                    Log.LogError($"{nameof(AssetManifestPath)} is not set properly.");
                }
                else if (MaxClients <= 0)
                {
                    Log.LogError($"{nameof(MaxClients)} should be greater than zero.");
                }
                else if (UploadTimeoutInMinutes <= 0)
                {
                    Log.LogError($"{nameof(UploadTimeoutInMinutes)} should be greater than zero.");
                }

                if (Log.HasLoggedErrors)
                {
                    return false;
                }

                BlobFeedAction blobFeedAction = new BlobFeedAction(ExpectedFeedUrl, AccountKey, Log);
                var pushOptions = new PushOptions
                {
                    AllowOverwrite = Overwrite,
                    PassIfExistingItemIdentical = PassIfExistingItemIdentical
                };

                IEnumerable<BlobArtifactModel> blobArtifacts = Enumerable.Empty<BlobArtifactModel>();

                if (!SkipCreateContainer)
                {
                    await blobFeedAction.CreateContainerAsync(BuildEngine);
                }

                await blobFeedAction.PublishToFlatContainerAsync(ItemsToPush, 
                    MaxClients, 
                    pushOptions);
                blobArtifacts = ConcatBlobArtifacts(blobArtifacts, ItemsToPush);

                if (!(MSBuildListSplitter.GetNamedProperties(ManifestBuildData).ContainsKey("Location") || 
                    MSBuildListSplitter.GetNamedProperties(ManifestBuildData).ContainsKey("InitialAssetsLocation")))
                {
                    string[] locationAttribute = new string[] { $"Location={ExpectedFeedUrl}" };
                    ManifestBuildData = ManifestBuildData == null ? locationAttribute : ManifestBuildData.Concat(locationAttribute).ToArray();
                }

                _buildModelFactory.CreateBuildManifest(
                    blobArtifacts,
                    Enumerable.Empty<PackageArtifactModel>(),
                    AssetManifestPath,
                    ManifestRepoUri,
                    ManifestBuildId,
                    ManifestBranch,
                    ManifestCommit,
                    ManifestBuildData,
                    IsStableBuild,
                    PublishingInfraVersion.Legacy,
                    IsReleaseOnlyPackageVersion);
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }

        private IEnumerable<PackageArtifactModel> ConcatPackageArtifacts(
            IEnumerable<PackageArtifactModel> artifacts,
            IEnumerable<ITaskItem> items)
        {
            return artifacts.Concat(items
                .Where(i => !string.Equals(i.GetMetadata("ExcludeFromManifest"), "true", StringComparison.OrdinalIgnoreCase))
                .Select(_packageArtifactModelFactory.CreatePackageArtifactModel));
        }

        private IEnumerable<BlobArtifactModel> ConcatBlobArtifacts(
            IEnumerable<BlobArtifactModel> artifacts,
            IEnumerable<ITaskItem> items)
        {
            return artifacts.Concat(items
                .Where(i => !string.Equals(i.GetMetadata("ExcludeFromManifest"), "true", StringComparison.OrdinalIgnoreCase))
                .Select(i => _blobArtifactModelFactory.CreateBlobArtifactModel(i))
                .Where(blob => blob != null));
        }
    }
}
