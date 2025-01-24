// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class PushToBuildStorage : MSBuildTaskBase
    {
        [Required]
        public ITaskItem[] ItemsToPush { get; set; }

        public string AssetsTemporaryDirectory { get; set; }

        public bool PublishFlatContainer { get; set; }

        public string ManifestRepoName { get; set; }

        public string ManifestRepoUri { get; set; }

        public string ManifestBuildId { get; set; } = "no build id provided";

        public string ManifestBranch { get; set; }

        public string ManifestCommit { get; set; }

        /// <summary>
        /// Indicates the source of the artifacts. For a VMR build, the `ManifestRepoName` is dotnet/dotnet,
        /// while the `ManifestRepoOrigin` corresponds to the actual product repository.
        /// </summary>
        public string ManifestRepoOrigin { get; set; }

        public string[] ManifestBuildData { get; set; }

        public string AzureDevOpsCollectionUri { get; set; }

        public string AzureDevOpsProject { get; set; }

        public int AzureDevOpsBuildId { get; set; }

        public ITaskItem[] ItemsToSign { get; set; }

        public ITaskItem[] StrongNameSignInfo { get; set; }

        public ITaskItem[] FileSignInfo { get; set; }

        public ITaskItem[] FileExtensionSignInfo { get; set; }

        public ITaskItem[] CertificatesSignInfo { get; set; }

        public string AssetManifestPath { get; set; }

        public bool IsStableBuild { get; set; }

        public bool IsReleaseOnlyPackageVersion { get; set; }

        public string AssetsLocalStorageDir { get; set; }

        public string ShippingPackagesLocalStorageDir { get; set; }

        public string NonShippingPackagesLocalStorageDir { get; set; }

        public string AssetManifestsLocalStorageDir { get; set; }

        public bool PushToLocalStorage { get; set; }

        public ITaskItem[] ArtifactVisibilitiesToPublish { get; set; }

        /// <summary>
        /// Which version should the build manifest be tagged with.
        /// By default he latest version is used.
        /// </summary>
        public string PublishingVersion { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to use hard links for the copied files
        /// rather than copy the files, if it's possible to do so.
        /// </summary>
        public bool UseHardlinksIfPossible { get; set; } = true;

        public enum ItemType
        {
            AssetManifest = 0,
            PackageArtifact,
            BlobArtifact
        }

        public override void ConfigureServices(IServiceCollection collection)
        {
            collection.TryAddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>();
            collection.TryAddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>();
            collection.TryAddSingleton<IBuildModelFactory, BuildModelFactory>();
            collection.TryAddSingleton<IFileSystem, FileSystem>();
            collection.TryAddSingleton<IPackageArchiveReaderFactory, PackageArchiveReaderFactory>();
            collection.TryAddSingleton<INupkgInfoFactory, NupkgInfoFactory>();
            collection.TryAddSingleton(Log);
        }

        public bool ExecuteTask(IFileSystem fileSystem,
            IBlobArtifactModelFactory blobArtifactModelFactory,
            IPackageArtifactModelFactory packageArtifactModelFactory,
            IBuildModelFactory buildModelFactory)
        {
            try
            {
                if (PushToLocalStorage)
                {
                    if (string.IsNullOrEmpty(AssetsLocalStorageDir) ||
                        string.IsNullOrEmpty(ShippingPackagesLocalStorageDir) ||
                        string.IsNullOrEmpty(NonShippingPackagesLocalStorageDir) ||
                        string.IsNullOrEmpty(AssetManifestsLocalStorageDir))
                    {
                        throw new Exception($"AssetsLocalStorageDir, ShippingPackagesLocalStorageDir, NonShippingPackagesLocalStorageDir and AssetManifestsLocalStorageDir need to be specified if PublishToLocalStorage is set to true");
                    }

                    Log.LogMessage(MessageImportance.High, "Performing push to local artifacts storage.");
                }
                else
                {
                    Log.LogMessage(MessageImportance.High, "Performing push to Azure DevOps artifacts storage.");
                }

                if (!string.IsNullOrWhiteSpace(AssetsTemporaryDirectory))
                {
                    Log.LogMessage(MessageImportance.High, $"It's no longer necessary to specify a value for the {nameof(AssetsTemporaryDirectory)} property. " +
                        $"Please consider patching your code to not use it.");
                }

                if (ItemsToPush == null)
                {
                    Log.LogError($"No items to push. Please check ItemGroup ItemsToPush.");
                }
                else
                {
                    IEnumerable<BlobArtifactModel> blobArtifacts = Enumerable.Empty<BlobArtifactModel>();
                    IEnumerable<PackageArtifactModel> packageArtifacts = Enumerable.Empty<PackageArtifactModel>();

                    var itemsToPushNoExcludes = ItemsToPush.
                        Where(i => !string.Equals(i.GetMetadata("ExcludeFromManifest"), "true", StringComparison.OrdinalIgnoreCase));

                    if (PublishFlatContainer)
                    {
                        // Act as if %(PublishFlatContainer) were true for all items.
                        blobArtifacts = itemsToPushNoExcludes
                            .Select(i => blobArtifactModelFactory.CreateBlobArtifactModel(i, ManifestRepoOrigin));
                        foreach (var blobItem in itemsToPushNoExcludes)
                        {
                            if (!fileSystem.FileExists(blobItem.ItemSpec))
                            {
                                Log.LogError($"Could not find file {blobItem.ItemSpec}.");
                                continue;
                            }

                            PushToLocalStorageOrAzDO(ItemType.BlobArtifact, blobItem);
                        }
                    }
                    else
                    {
                        ITaskItem[] symbolItems = itemsToPushNoExcludes
                            .Where(i => i.ItemSpec.EndsWith("symbols.nupkg"))
                            .Select(i =>
                            {
                                string fileName = Path.GetFileName(i.ItemSpec);
                                i.SetMetadata("RelativeBlobPath", $"{AssetsVirtualDir}symbols/{fileName}");
                                return i;
                            })
                            .ToArray();

                        var blobItems = itemsToPushNoExcludes
                            .Where(i =>
                            {
                                var isFlatString = i.GetMetadata("PublishFlatContainer");
                                if (!string.IsNullOrEmpty(isFlatString) &&
                                    bool.TryParse(isFlatString, out var isFlat))
                                {
                                    return isFlat;
                                }

                                return false;
                            })
                            .Union(symbolItems)
                            .ToArray();

                        ITaskItem[] packageItems = itemsToPushNoExcludes
                            .Except(blobItems)
                            .ToArray();

                        foreach (var packagePath in packageItems)
                        {
                            if (!fileSystem.FileExists(packagePath.ItemSpec))
                            {
                                Log.LogError($"Could not find file {packagePath.ItemSpec}.");
                                continue;
                            }

                            PushToLocalStorageOrAzDO(ItemType.PackageArtifact, packagePath);
                        }

                        foreach (var blobItem in blobItems)
                        {
                            if (!fileSystem.FileExists(blobItem.ItemSpec))
                            {
                                Log.LogError($"Could not find file {blobItem.ItemSpec}.");
                                continue;
                            }

                            PushToLocalStorageOrAzDO(ItemType.BlobArtifact, blobItem);
                        }

                        packageArtifacts = packageItems.Select(
                            i => packageArtifactModelFactory.CreatePackageArtifactModel(i, ManifestRepoOrigin));
                        blobArtifacts = blobItems.Select(
                                i => blobArtifactModelFactory.CreateBlobArtifactModel(i, ManifestRepoOrigin))
                            .Where(blob => blob != null);
                    }

                    ArtifactVisibility[] visibilitiesToPublish = GetVisibilitiesToPublish(ArtifactVisibilitiesToPublish);

                    packageArtifacts = packageArtifacts.Where(p => visibilitiesToPublish.Contains(p.Visibility));
                    blobArtifacts = blobArtifacts.Where(b => visibilitiesToPublish.Contains(b.Visibility));

                    PublishingInfraVersion targetPublishingVersion = PublishingInfraVersion.Latest;

                    if (!string.IsNullOrEmpty(PublishingVersion))
                    {
                        if (!Enum.TryParse(PublishingVersion, ignoreCase: true, out targetPublishingVersion))
                        {
                            Log.LogError($"Could not parse publishing infra version '{PublishingVersion}'");
                        }
                    }

                    buildModelFactory.CreateBuildManifest(
                        blobArtifacts,
                        packageArtifacts,
                        AssetManifestPath,
                        !string.IsNullOrEmpty(ManifestRepoName) ? ManifestRepoName : ManifestRepoUri,
                        ManifestBuildId,
                        ManifestBranch,
                        ManifestCommit,
                        ManifestBuildData,
                        IsStableBuild,
                        targetPublishingVersion,
                        IsReleaseOnlyPackageVersion);

                    PushToLocalStorageOrAzDO(ItemType.AssetManifest, new TaskItem(AssetManifestPath));
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }

        private void PushToLocalStorageOrAzDO(ItemType itemType, ITaskItem item)
        {
            string path = item.ItemSpec;

            if (PushToLocalStorage)
            {
                string filename = Path.GetFileName(path);
                switch (itemType)
                {
                    case ItemType.AssetManifest:
                        Directory.CreateDirectory(AssetManifestsLocalStorageDir);
                        CopyFileAsHardLinkIfPossible(path, Path.Combine(AssetManifestsLocalStorageDir, filename), true);
                        break;

                    case ItemType.PackageArtifact:
                        if (string.Equals(item.GetMetadata("IsShipping"), "true", StringComparison.OrdinalIgnoreCase))
                        {
                            Directory.CreateDirectory(ShippingPackagesLocalStorageDir);
                            CopyFileAsHardLinkIfPossible(path, Path.Combine(ShippingPackagesLocalStorageDir, filename), true);
                        }
                        else
                        {
                            Directory.CreateDirectory(NonShippingPackagesLocalStorageDir);
                            CopyFileAsHardLinkIfPossible(path, Path.Combine(NonShippingPackagesLocalStorageDir, filename), true);
                        }
                        break;

                    case ItemType.BlobArtifact:
                        string relativeBlobPath = item.GetMetadata("RelativeBlobPath");
                        string destinationPath = Path.Combine(
                                                    AssetsLocalStorageDir,
                                                    string.IsNullOrEmpty(relativeBlobPath) ? filename : relativeBlobPath);

                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                        CopyFileAsHardLinkIfPossible(path, destinationPath, true);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }
            }
            else
            {
                // Push to AzDO artifacts storage

                switch (itemType)
                {
                    case ItemType.AssetManifest:
                        Log.LogMessage(MessageImportance.High,
                            $"##vso[artifact.upload containerfolder=AssetManifests;artifactname=AssetManifests]{path}");
                        break;

                    case ItemType.PackageArtifact:
                        Log.LogMessage(MessageImportance.High,
                            $"##vso[artifact.upload containerfolder=PackageArtifacts;artifactname=PackageArtifacts]{path}");
                        break;

                    case ItemType.BlobArtifact:
                        Log.LogMessage(MessageImportance.High,
                            $"##vso[artifact.upload containerfolder=BlobArtifacts;artifactname=BlobArtifacts]{path}");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemType));
                }
            }
        }

        private static ArtifactVisibility[] GetVisibilitiesToPublish(ITaskItem[] allowedVisibilities)
        {
            if (allowedVisibilities is null || allowedVisibilities.Length == 0)
            {
                return [ArtifactVisibility.External];
            }

            return allowedVisibilities.Select(item => (ArtifactVisibility)Enum.Parse(typeof(ArtifactVisibility), item.ItemSpec)).ToArray();
        }

        private void CopyFileAsHardLinkIfPossible(string sourceFileName, string destFileName, bool overwrite)
        {
            if (UseHardlinksIfPossible)
            {
                Log.LogMessage(MessageImportance.Normal, "Creating hard link to copy \"{0}\" to \"{1}\".", sourceFileName, destFileName);

                string errorMessage = string.Empty;
                if (NativeMethods.MakeHardLink(destFileName, sourceFileName, ref errorMessage))
                {
                    return;
                }

                Log.LogMessage(MessageImportance.Normal, "Could not use a link to copy \"{0}\" to \"{1}\". Copying the file instead. {2}", sourceFileName, destFileName, errorMessage);
            }

            File.Copy(sourceFileName, destFileName, overwrite);
        }
    }
}
