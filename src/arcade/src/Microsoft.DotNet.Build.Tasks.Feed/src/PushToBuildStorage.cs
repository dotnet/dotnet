// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class PushToBuildStorage : MSBuildTaskBase
    {
        [Required]
        public ITaskItem[] ItemsToPush { get; set; }

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

        /// <summary>
        /// Represents where assets should be copied locally, either for staging for upload
        /// or for propagation to another phase of the VMR build.
        /// </summary>
        public string AssetsLocalStorageDir { get; set; }

        /// <summary>
        /// Represents where shipping packages should be copied locally, either for staging for upload
        /// or for propagation to another phase of the VMR build.
        /// </summary>
        public string ShippingPackagesLocalStorageDir { get; set; }

        /// <summary>
        /// Represents where nonshipping packages should be copied locally, either for staging for upload
        /// or for propagation to another phase of the VMR build.
        /// </summary>
        public string NonShippingPackagesLocalStorageDir { get; set; }

        /// <summary>
        /// Represents where asset manifests should be copied locally, either for staging for upload
        /// or for propagation to another phase of the VMR build.
        /// </summary>
        public string AssetManifestsLocalStorageDir { get; set; }

        /// <summary>
        /// Represents where pdb artifacts should be copied locally, either for staging for upload
        /// or for propagation to another phase of the VMR build.
        /// 
        /// NOTE: In non-VMR builds, this represents the location of the PDBs that are copied
        /// to before uploading to the PDBArtifacts dir.
        /// </summary>
        public string PdbArtifactsLocalStorageDir { get; set; }

        public bool PushToLocalStorage { get; set; }

        /// <summary>
        /// The final path for any packages published to <see cref="ShippingPackagesLocalStorageDir"/>
        /// or <see cref="NonShippingPackagesLocalStorageDir"/> should have the artifact's RepoOrigin
        /// appended as a subfolder to the published path.
        /// </summary>
        public bool PreserveRepoOrigin { get; set; }

        /// <summary>
        /// The visibility of the artifacts to put in the manifest.
        /// </summary>
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

        public bool PublishManifestOnly { get; set; } = false;

        public override void ConfigureServices(IServiceCollection collection)
        {
            collection.TryAddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>();
            collection.TryAddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>();
            collection.TryAddSingleton<IPdbArtifactModelFactory, PdbArtifactModelFactory>();
            collection.TryAddSingleton<IBuildModelFactory, BuildModelFactory>();
            collection.TryAddSingleton<IFileSystem, FileSystem>();
            collection.TryAddSingleton<IPackageArchiveReaderFactory, PackageArchiveReaderFactory>();
            collection.TryAddSingleton<INupkgInfoFactory, NupkgInfoFactory>();
            collection.TryAddSingleton(Log);
        }

        public bool ExecuteTask(IFileSystem fileSystem,
            IBlobArtifactModelFactory blobArtifactModelFactory,
            IPackageArtifactModelFactory packageArtifactModelFactory,
            IPdbArtifactModelFactory pdbArtifactModelFactory,
            IBuildModelFactory buildModelFactory)
        {
            try
            {
                if (PushToLocalStorage)
                {
                    if (!PublishManifestOnly)
                    {
                        if (string.IsNullOrEmpty(AssetsLocalStorageDir) ||
                            string.IsNullOrEmpty(ShippingPackagesLocalStorageDir) ||
                            string.IsNullOrEmpty(NonShippingPackagesLocalStorageDir) ||
                            string.IsNullOrEmpty(PdbArtifactsLocalStorageDir))
                        {
                            throw new Exception($"AssetsLocalStorageDir, ShippingPackagesLocalStorageDir, NonShippingPackagesLocalStorageDir and PdbArtifactsLocalStorageDir need to be specified if PublishToLocalStorage is set to true");
                        }
                    }
                    if (string.IsNullOrEmpty(AssetManifestsLocalStorageDir))
                    {
                        throw new Exception($"AssetManifestsLocalStorageDir needs to be specified if PublishToLocalStorage is set to true");
                    }

                    Log.LogMessage(MessageImportance.High, "Performing push to local artifacts storage.");
                }
                else
                {
                    Log.LogMessage(MessageImportance.High, "Performing push to Azure DevOps artifacts storage.");
                }

                if (ItemsToPush == null)
                {
                    Log.LogError($"No items to push. Please check ItemGroup ItemsToPush.");
                }
                else
                {
                    PublishingInfraVersion targetPublishingVersion = PublishingInfraVersion.Latest;

                    if (!string.IsNullOrEmpty(PublishingVersion))
                    {
                        if (!Enum.TryParse(PublishingVersion, ignoreCase: true, out targetPublishingVersion))
                        {
                            Log.LogError($"Could not parse publishing infra version '{PublishingVersion}'");
                        }
                    }

                    var artifactVisibilities = GetVisibilitiesToPublish(ArtifactVisibilitiesToPublish);

                    var buildModel = buildModelFactory.CreateModel(
                        ItemsToPush,
                        artifactVisibilities,
                        ManifestBuildId,
                        ManifestBuildData,
                        !string.IsNullOrEmpty(ManifestRepoName) ? ManifestRepoName : ManifestRepoUri,
                        ManifestBranch,
                        ManifestCommit,
                        ManifestRepoOrigin,
                        IsStableBuild,
                        targetPublishingVersion,
                        IsReleaseOnlyPackageVersion);

                    if (buildModel == null)
                    {
                        Log.LogError($"Failed to construct build model from input artifacts.");
                        return false;
                    }

                    if (buildModel.Artifacts.Pdbs.Any() && string.IsNullOrEmpty(PdbArtifactsLocalStorageDir))
                    {
                        throw new Exception($"PdbArtifactsLocalStorageDir must be specified.");
                    }

                    if (!PublishManifestOnly)
                    {
                        foreach (var package in buildModel.Artifacts.Packages)
                        {
                            if (!fileSystem.FileExists(package.OriginalFile))
                            {
                                Log.LogError($"Could not find file {package.OriginalFile}.");
                                continue;
                            }

                            PushToLocalStorageOrAzDO(package);
                        }

                        foreach (var blobArtifact in buildModel.Artifacts.Blobs)
                        {
                            if (!fileSystem.FileExists(blobArtifact.OriginalFile))
                            {
                                Log.LogError($"Could not find file {blobArtifact.OriginalFile}.");
                                continue;
                            }

                            PushToLocalStorageOrAzDO(blobArtifact);
                        }

                        foreach (var pdbArtifact in buildModel.Artifacts.Pdbs)
                        {
                            if (!fileSystem.FileExists(pdbArtifact.OriginalFile))
                            {
                                Log.LogError($"Could not find file {pdbArtifact.OriginalFile}.");
                                continue;
                            }
                            PushToLocalStorageOrAzDO(pdbArtifact);
                        }

                        if (!PushToLocalStorage && buildModel.Artifacts.Pdbs.Any())
                        {
                            // Upload the full set of PDBs
                            Log.LogMessage(MessageImportance.High,
                                $"##vso[artifact.upload containerfolder=PdbArtifacts;artifactname=PdbArtifacts]{PdbArtifactsLocalStorageDir}");
                        }
                    }

                    // Write the manifest, then create an artifact for it.
                    Log.LogMessage(MessageImportance.High, $"Writing build manifest file '{AssetManifestPath}'...");
                    fileSystem.WriteToFile(AssetManifestPath, buildModel.ToXml().ToString(SaveOptions.DisableFormatting));

                    // Generate an artifact for the asset manifest and push it to storage.
                    AssetManifestModel assetManifestModel = new AssetManifestModel
                    {
                        OriginalFile = AssetManifestPath,
                        Id = Path.GetFileName(AssetManifestPath)
                    };
                    PushToLocalStorageOrAzDO(assetManifestModel);
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e, true);
            }

            return !Log.HasLoggedErrors;
        }

        private void PushToLocalStorageOrAzDO(ArtifactModel artifactModel)
        {
            string path = artifactModel.OriginalFile;

            if (PushToLocalStorage)
            {
                string filename = Path.GetFileName(path);
                switch (artifactModel)
                {
                    case AssetManifestModel _:
                        Directory.CreateDirectory(AssetManifestsLocalStorageDir);
                        CopyFileAsHardLinkIfPossible(path, Path.Combine(AssetManifestsLocalStorageDir, filename), true);
                        break;

                    case PackageArtifactModel _:
                    {
                        string packageDestinationPath = artifactModel.NonShipping
                            ? NonShippingPackagesLocalStorageDir
                            : ShippingPackagesLocalStorageDir;

                        if (PreserveRepoOrigin)
                        {
                            packageDestinationPath = Path.Combine(packageDestinationPath, artifactModel.RepoOrigin);
                        }

                        Directory.CreateDirectory(packageDestinationPath);
                        CopyFileAsHardLinkIfPossible(path, Path.Combine(packageDestinationPath, filename), true);
                        break;
                    }

                    case BlobArtifactModel _:
                        string relativeBlobPath = artifactModel.Id;
                        string blobDestinationPath = Path.Combine(
                                                    AssetsLocalStorageDir,
                                                    string.IsNullOrEmpty(relativeBlobPath) ? filename : relativeBlobPath);

                        Directory.CreateDirectory(Path.GetDirectoryName(blobDestinationPath));
                        CopyFileAsHardLinkIfPossible(path, blobDestinationPath, true);
                        break;

                    case PdbArtifactModel _:
                        string relativePdbPath = artifactModel.Id;
                        string pdbDestinationPath = Path.Combine(
                                                    PdbArtifactsLocalStorageDir,
                                                    string.IsNullOrEmpty(relativePdbPath) ? filename : relativePdbPath);

                        Directory.CreateDirectory(Path.GetDirectoryName(pdbDestinationPath));
                        CopyFileAsHardLinkIfPossible(path, pdbDestinationPath, true);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(artifactModel));
                }
            }
            else
            {
                // Push to AzDO artifacts storage

                switch (artifactModel)
                {
                    case AssetManifestModel _:
                        Log.LogMessage(MessageImportance.High,
                            $"##vso[artifact.upload containerfolder=AssetManifests;artifactname=AssetManifests]{path}");
                        break;

                    case PackageArtifactModel _:
                        Log.LogMessage(MessageImportance.High,
                            $"##vso[artifact.upload containerfolder=PackageArtifacts;artifactname=PackageArtifacts]{path}");
                        break;

                    case BlobArtifactModel _:
                        Log.LogMessage(MessageImportance.High,
                            $"##vso[artifact.upload containerfolder=BlobArtifacts;artifactname=BlobArtifacts]{path}");
                        break;

                    case PdbArtifactModel _:
                        string pdbArtifactTarget = Path.Combine(PdbArtifactsLocalStorageDir, artifactModel.Id);
                        Directory.CreateDirectory(Path.GetDirectoryName(pdbArtifactTarget));
                        // Copy the PDB artifact to the temp local dir.
                        File.Copy(path, pdbArtifactTarget, false);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(artifactModel));
                }
            }
        }

        private static ArtifactVisibility GetVisibilitiesToPublish(ITaskItem[] allowedVisibilities)
        {
            if (allowedVisibilities is null || allowedVisibilities.Length == 0)
            {
                return ArtifactVisibility.External;
            }

            ArtifactVisibility visibility = 0;

            foreach (var item in allowedVisibilities)
            {
                if (Enum.TryParse(item.ItemSpec, true, out ArtifactVisibility parsedVisibility))
                {
                    visibility |= parsedVisibility;
                }
                else
                {
                    throw new ArgumentException($"Invalid visibility: {item.ItemSpec}");
                }
            }

            return visibility;
        }

        // The below method implementation is copied from msbuild's Copy task and adjusted.
        private void CopyFileAsHardLinkIfPossible(string sourceFileName, string destFileName, bool overwrite)
        {
            FileInfo destFile = new(destFileName);

            if (UseHardlinksIfPossible)
            {
                // NativeMethods.MakeHardLink cannot overwrite an existing file or link
                // so we need to delete the existing entry before we create the hard link.
                if (destFile.Exists && !destFile.IsReadOnly)
                {
                    try
                    {
                        File.Delete(destFile.FullName);
                    }
                    catch (Exception ex) when (IsIoRelatedException(ex))
                    {
                    }
                }

                Log.LogMessage(MessageImportance.Normal, $"Creating hard link to copy \"{sourceFileName}\" to \"{destFileName}\".");

                string errorMessage = string.Empty;
                if (!NativeMethods.MakeHardLink(destFileName, sourceFileName, ref errorMessage))
                {
                    Log.LogMessage(MessageImportance.Normal, $"Could not use a link to copy \"{sourceFileName}\" to \"{destFileName}\". Copying the file instead. {errorMessage}");
                    File.Copy(sourceFileName, destFileName, overwrite);
                }
            }
            else
            {
                File.Copy(sourceFileName, destFileName, overwrite);
            }

            // If the destinationFile file exists, then make sure it's read-write.
            // The File.Copy command copies attributes, but our copy needs to
            // leave the file writeable.
            if (new FileInfo(sourceFileName).IsReadOnly)
            {
                // Ensure the read-only attribute on the specified file is off, so
                // the file is writeable.
                if (destFile.Exists)
                {
                    if (destFile.IsReadOnly)
                    {
                        Log.LogMessage(MessageImportance.Low, $"Removing read-only attribute from \"{destFile.FullName}\".");
                        File.SetAttributes(destFile.FullName, FileAttributes.Normal);
                    }
                }
            }

            // Determine whether the exception is file-IO related.
            static bool IsIoRelatedException(Exception e)
            {
                // These all derive from IOException
                //     DirectoryNotFoundException
                //     DriveNotFoundException
                //     EndOfStreamException
                //     FileLoadException
                //     FileNotFoundException
                //     PathTooLongException
                //     PipeException
                return e is UnauthorizedAccessException
                    || e is NotSupportedException
                    || (e is ArgumentException && !(e is ArgumentNullException))
                    || e is SecurityException
                    || e is IOException;
            }
        }
    }
}
