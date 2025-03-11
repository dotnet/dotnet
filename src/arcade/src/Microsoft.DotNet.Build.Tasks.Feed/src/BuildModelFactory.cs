// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public interface IBuildModelFactory
    {
        BuildModel CreateModel(
            ITaskItem[] artifacts,
            ArtifactVisibility artifactVisibilitiesToInclude,
            string buildId,
            string[] manifestBuildData,
            string repoUri,
            string repoBranch,
            string repoCommit,
            string repoOrigin,
            bool isStableBuild,
            PublishingInfraVersion publishingVersion,
            bool isReleaseOnlyPackageVersion);

        BuildModel ManifestFileToModel(string assetManifestPath);
        BuildModel CreateMergedModel(List<BuildModel> models);
    }

    public class BuildModelFactory : IBuildModelFactory
    {
        private readonly IBlobArtifactModelFactory _blobArtifactModelFactory;
        private readonly IPdbArtifactModelFactory _pdbArtifactModelFactory;
        private readonly IPackageArtifactModelFactory _packageArtifactModelFactory;
        private readonly IFileSystem _fileSystem;
        private readonly TaskLoggingHelper _log;

        public BuildModelFactory(
            IBlobArtifactModelFactory blobArtifactModelFactory,
            IPdbArtifactModelFactory pdbArtifactModelFactory,
            IPackageArtifactModelFactory packageArtifactModelFactory,
            IFileSystem fileSystem,
            TaskLoggingHelper logger)
        {
            _blobArtifactModelFactory = blobArtifactModelFactory;
            _pdbArtifactModelFactory = pdbArtifactModelFactory;
            _packageArtifactModelFactory = packageArtifactModelFactory;
            _fileSystem = fileSystem;
            _log = logger;
        }

        private const string AssetsVirtualDir = "assets/";

        private static readonly string AzureDevOpsHostPattern = @"dev\.azure\.com\";

        private readonly Regex LegacyRepositoryUriPattern = new Regex(
            @"^https://(?<account>[a-zA-Z0-9]+)\.visualstudio\.com/");

        private const string ArtifactKindMetadata = "Kind";

        public BuildModel CreateModel(
            ITaskItem[] artifacts,
            ArtifactVisibility artifactVisibilitiesToInclude,
            string buildId,
            string[] manifestBuildData,
            string repoUri,
            string repoBranch,
            string repoCommit,
            string repoOrigin,
            bool isStableBuild,
            PublishingInfraVersion publishingVersion,
            bool isReleaseOnlyPackageVersion)
        {
            if (artifacts == null)
            {
                throw new ArgumentNullException(nameof(artifacts));
            }

            // Filter out artifacts that are excluded from the manifest
            var itemsToPushNoExcludes = artifacts.
                Where(i => !string.Equals(i.GetMetadata("ExcludeFromManifest"), "true", StringComparison.OrdinalIgnoreCase));

            // Verify that Kind is set on all items
            bool missingKind = false;
            foreach (var item in itemsToPushNoExcludes)
            {
                if (string.IsNullOrEmpty(item.GetMetadata("Kind")))
                {
                    _log.LogError($"Missing 'Kind' property on artifact {item.ItemSpec}. Possible values are 'Blob', 'PDB', 'Package'.");
                    missingKind = true;
                }
            }

            if (missingKind)
            {
                return null;
            }

            // Split the non-excluded items into the different artifact types based on the Kind metadata,
            // where the visibility of the artifact should be included in the manifest,
            // and create the corresponding artifact models.

            var blobArtifacts = itemsToPushNoExcludes
                .Where(i => i.GetMetadata(ArtifactKindMetadata).Equals(nameof(ArtifactKind.Blob), StringComparison.OrdinalIgnoreCase))
                .Select(i => _blobArtifactModelFactory.CreateBlobArtifactModel(i, i.GetMetadata("RepoOrigin") is string origin and not "" ? origin : repoOrigin))
                .Where(b => artifactVisibilitiesToInclude.HasFlag(b.Visibility));

            var packageArtifacts = itemsToPushNoExcludes
                .Where(i => i.GetMetadata(ArtifactKindMetadata).Equals(nameof(ArtifactKind.Package), StringComparison.OrdinalIgnoreCase))
                .Select(i => _packageArtifactModelFactory.CreatePackageArtifactModel(i, i.GetMetadata("RepoOrigin") is string origin and not "" ? origin : repoOrigin))
                .Where(b => artifactVisibilitiesToInclude.HasFlag(b.Visibility));

            var pdbArtifacts = itemsToPushNoExcludes
                .Where(i => i.GetMetadata(ArtifactKindMetadata).Equals(nameof(ArtifactKind.Pdb), StringComparison.OrdinalIgnoreCase))
                .Select(i => _pdbArtifactModelFactory.CreatePdbArtifactModel(i, i.GetMetadata("RepoOrigin") is string origin and not "" ? origin : repoOrigin))
                .Where(b => artifactVisibilitiesToInclude.HasFlag(b.Visibility));

            return CreateModel(
                blobArtifacts,
                packageArtifacts,
                pdbArtifacts,
                buildId,
                manifestBuildData,
                repoUri,
                repoBranch,
                repoCommit,
                isStableBuild,
                publishingVersion,
                isReleaseOnlyPackageVersion);
        }
                    
        private BuildModel CreateModel( 
            IEnumerable<BlobArtifactModel> blobArtifacts,
            IEnumerable<PackageArtifactModel> packageArtifacts,
            IEnumerable<PdbArtifactModel> pdbArtifacts,
            string manifestBuildId,
            string[] manifestBuildData,
            string manifestRepoName,
            string manifestBranch,
            string manifestCommit,
            bool isStableBuild,
            PublishingInfraVersion publishingVersion,
            bool isReleaseOnlyPackageVersion)
        {
            var attributes = MSBuildListSplitter.GetNamedProperties(manifestBuildData);
            if (!ManifestBuildDataHasLocationInformation(attributes))
            {
                _log.LogError("Missing 'location' property from ManifestBuildData");
            }

            NormalizeUrisInBuildData(attributes);

            BuildModel buildModel = new BuildModel(
                    new BuildIdentity
                    {
                        Attributes = attributes,
                        Name = manifestRepoName,
                        BuildId = manifestBuildId,
                        Branch = manifestBranch,
                        Commit = manifestCommit,
                        IsStable = isStableBuild,
                        PublishingVersion = publishingVersion,
                        IsReleaseOnlyPackageVersion = isReleaseOnlyPackageVersion
                    });

            buildModel.Artifacts.Blobs.AddRange(blobArtifacts);
            buildModel.Artifacts.Packages.AddRange(packageArtifacts);
            buildModel.Artifacts.Pdbs.AddRange(pdbArtifacts);
            return buildModel;
        }

        public BuildModel ManifestFileToModel(string assetManifestPath)
        {
            try
            {
                return BuildModel.Parse(XElement.Load(assetManifestPath));
            }
            catch (Exception e)
            {
                _log.LogError($"Could not parse asset manifest file: {assetManifestPath}");
                _log.LogErrorFromException(e);
                return null;
            }
        }

        private bool ManifestBuildDataHasLocationInformation(IDictionary<string, string> attributes)
        {
            return attributes.ContainsKey("Location") || attributes.ContainsKey("InitialAssetsLocation");
        }

        private void NormalizeUrisInBuildData(IDictionary<string, string> attributes)
        {
            foreach(var attribute in attributes.ToList())
            {
                attributes[attribute.Key] = NormalizeAzureDevOpsUrl(attribute.Value);
            }
        }

        /// <summary>
        // If repoUri includes the user in the account we remove it from URIs like
        // https://dnceng@dev.azure.com/dnceng/internal/_git/repo
        // If the URL host is of the form "dnceng.visualstudio.com" like
        // https://dnceng.visualstudio.com/internal/_git/repo we replace it to "dev.azure.com/dnceng"
        // for consistency
        /// </summary>
        /// <param name="repoUri">The original url</param>
        /// <returns>Transformed url</returns>
        private string NormalizeAzureDevOpsUrl(string repoUri)
        {
            if (Uri.TryCreate(repoUri, UriKind.Absolute, out Uri parsedUri))
            {
                if (!string.IsNullOrEmpty(parsedUri.UserInfo))
                {
                    repoUri = repoUri.Replace($"{parsedUri.UserInfo}@", string.Empty);
                }

                Match m = LegacyRepositoryUriPattern.Match(repoUri);

                if (m.Success)
                {
                    string replacementUri = $"{Regex.Unescape(AzureDevOpsHostPattern)}/{m.Groups["account"].Value}";
                    repoUri = repoUri.Replace(parsedUri.Host, replacementUri);
                }
            }

            return repoUri;
        }

        /// <summary>
        /// Merges multiple BuildModels into a single BuildModel.
        /// If there is only one model, the original is returned.
        /// </summary>
        /// <param name="models">Manifests</param>
        /// <returns>Build model with the contents of all manifests. Please note, the identity and assets may be ref-equal</returns>
        public BuildModel CreateMergedModel(List<BuildModel> models)
        {
            if (models == null || models.Count == 0)
            {
                _log.LogError("No manifests to merge.");
                return null;
            }

            // If there is only one manifest, return it.
            if (models.Count == 1)
            {
                return models.First();
            }

            // Use the first manifest as the reference identity.
            BuildModel reference = models.First();
            var refIdentity = reference.Identity;

            // Validate that all manifests have identical build identity properties.
            foreach (BuildModel manifest in models)
            {
                // Compare the identities of the manifests.
                var identity = manifest.Identity;
                if (!string.Equals(refIdentity.AzureDevOpsAccount, identity.AzureDevOpsAccount, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(refIdentity.AzureDevOpsBranch, identity.AzureDevOpsBranch, StringComparison.OrdinalIgnoreCase) ||
                    refIdentity.AzureDevOpsBuildDefinitionId != identity.AzureDevOpsBuildDefinitionId ||
                    refIdentity.AzureDevOpsBuildId != identity.AzureDevOpsBuildId ||
                    !string.Equals(refIdentity.AzureDevOpsBuildNumber, identity.AzureDevOpsBuildNumber, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(refIdentity.AzureDevOpsProject, identity.AzureDevOpsProject, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(refIdentity.AzureDevOpsRepository, identity.AzureDevOpsRepository, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(refIdentity.Branch, identity.Branch, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(refIdentity.BuildId, identity.BuildId, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(refIdentity.InitialAssetsLocation, identity.InitialAssetsLocation, StringComparison.OrdinalIgnoreCase) ||
                    refIdentity.IsReleaseOnlyPackageVersion != identity.IsReleaseOnlyPackageVersion ||
                    refIdentity.IsStable != identity.IsStable ||
                    !string.Equals(refIdentity.ProductVersion, identity.ProductVersion, StringComparison.OrdinalIgnoreCase) ||
                    refIdentity.PublishingVersion != identity.PublishingVersion ||
                    !string.Equals(refIdentity.VersionStamp, identity.VersionStamp, StringComparison.OrdinalIgnoreCase))
                {
                    _log.LogError("Build identity properties are not identical across manifests.");
                    return null;
                }
            }

            // Create a new BuildModel with the shared identity.
            BuildModel mergedModel = new BuildModel(refIdentity);

            // Concatenate artifacts from all manifests.
            foreach (BuildModel manifest in models)
            {
                mergedModel.Artifacts.Blobs.AddRange(manifest.Artifacts.Blobs);
                mergedModel.Artifacts.Packages.AddRange(manifest.Artifacts.Packages);
                mergedModel.Artifacts.Pdbs.AddRange(manifest.Artifacts.Pdbs);
            }

            return mergedModel;
        }
    }
}
