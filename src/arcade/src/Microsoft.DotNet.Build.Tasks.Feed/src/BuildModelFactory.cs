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
        void CreateBuildManifest(
            IEnumerable<BlobArtifactModel> blobArtifacts,
            IEnumerable<PackageArtifactModel> packageArtifacts,
            string assetManifestPath,
            string manifestRepoName,
            string manifestBuildId,
            string manifestBranch,
            string manifestCommit,
            string[] manifestBuildData,
            bool isStableBuild,
            PublishingInfraVersion publishingVersion,
            bool isReleaseOnlyPackageVersion,
            SigningInformationModel signingInformationModel = null);

        BuildModel CreateModelFromItems(
            ITaskItem[] artifacts,
            ITaskItem[] itemsToSign,
            ITaskItem[] strongNameSignInfo,
            ITaskItem[] fileSignInfo,
            ITaskItem[] fileExtensionSignInfo,
            ITaskItem[] certificatesSignInfo,
            string buildId,
            string[] manifestBuildData,
            string repoUri,
            string repoBranch,
            string repoCommit,
            bool isStableBuild,
            PublishingInfraVersion publishingVersion,
            bool isReleaseOnlyPackageVersion);

        BuildModel ManifestFileToModel(string assetManifestPath);
    }

    public class BuildModelFactory : IBuildModelFactory
    {
        private readonly ISigningInformationModelFactory _signingInformationModelFactory;
        private readonly IBlobArtifactModelFactory _blobArtifactModelFactory;
        private readonly IPackageArtifactModelFactory _packageArtifactModelFactory;
        private readonly IFileSystem _fileSystem;
        private readonly TaskLoggingHelper _log;

        public BuildModelFactory(
            ISigningInformationModelFactory signingInformationModelFactory,
            IBlobArtifactModelFactory blobArtifactModelFactory,
            IPackageArtifactModelFactory packageArtifactModelFactory,
            IFileSystem fileSystem,
            TaskLoggingHelper logger)
        {
            _signingInformationModelFactory = signingInformationModelFactory;
            _blobArtifactModelFactory = blobArtifactModelFactory;
            _packageArtifactModelFactory = packageArtifactModelFactory;
            _fileSystem = fileSystem;
            _log = logger;
        }

        private const string AssetsVirtualDir = "assets/";

        private static readonly string AzureDevOpsHostPattern = @"dev\.azure\.com\";

        private readonly Regex LegacyRepositoryUriPattern = new Regex(
            @"^https://(?<account>[a-zA-Z0-9]+)\.visualstudio\.com/");

        /// <summary>
        /// Create a build manifest for packages, blobs, and associated signing information
        /// </summary>
        /// <param name="log">MSBuild log helper</param>
        /// <param name="blobArtifacts">Collection of blobs</param>
        /// <param name="packageArtifacts">Collection of packages</param>
        /// <param name="assetManifestPath">Asset manifest file that should be written</param>
        /// <param name="manifestRepoName">Repository name</param>
        /// <param name="manifestBuildId">Azure devops build id</param>
        /// <param name="manifestBranch">Name of the branch that was built</param>
        /// <param name="manifestCommit">Commit that was built</param>
        /// <param name="manifestBuildData">Additional build data properties</param>
        /// <param name="isStableBuild">True if the build is stable, false otherwise.</param>
        /// <param name="publishingVersion">Publishing version in use.</param>
        /// <param name="isReleaseOnlyPackageVersion">True if this repo uses release-only package versions</param>
        /// <param name="signingInformationModel">Signing information.</param>
        public void CreateBuildManifest(
            IEnumerable<BlobArtifactModel> blobArtifacts,
            IEnumerable<PackageArtifactModel> packageArtifacts,
            string assetManifestPath,
            string manifestRepoName,
            string manifestBuildId,
            string manifestBranch,
            string manifestCommit,
            string[] manifestBuildData,
            bool isStableBuild,
            PublishingInfraVersion publishingVersion,
            bool isReleaseOnlyPackageVersion,
            SigningInformationModel signingInformationModel = null)
        {
            BuildModel model = CreateModel(
                blobArtifacts,
                packageArtifacts,
                manifestBuildId,
                manifestBuildData,
                manifestRepoName,
                manifestBranch,
                manifestCommit,
                isStableBuild,
                publishingVersion,
                isReleaseOnlyPackageVersion,
                signingInformationModel: signingInformationModel);

            _log.LogMessage(MessageImportance.High, $"Writing build manifest file '{assetManifestPath}'...");
            _fileSystem.WriteToFile(assetManifestPath, model.ToXml().ToString(SaveOptions.DisableFormatting));
        }

        public BuildModel CreateModelFromItems(
            ITaskItem[] artifacts,
            ITaskItem[] itemsToSign,
            ITaskItem[] strongNameSignInfo,
            ITaskItem[] fileSignInfo,
            ITaskItem[] fileExtensionSignInfo,
            ITaskItem[] certificatesSignInfo,
            string buildId,
            string[] manifestBuildData,
            string repoUri,
            string repoBranch,
            string repoCommit,
            bool isStableBuild,
            PublishingInfraVersion publishingVersion,
            bool isReleaseOnlyPackageVersion)
        {
            if (artifacts == null)
            {
                throw new ArgumentNullException(nameof(artifacts));
            }

            var blobArtifacts = new List<BlobArtifactModel>();
            var packageArtifacts = new List<PackageArtifactModel>();

            foreach (var artifact in artifacts)
            {
                if (string.Equals(artifact.GetMetadata("ExcludeFromManifest"), "true", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var isSymbolsPackage = GeneralUtils.IsSymbolPackage(artifact.ItemSpec);

                if (artifact.ItemSpec.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase) && !isSymbolsPackage)
                {
                    packageArtifacts.Add(_packageArtifactModelFactory.CreatePackageArtifactModel(artifact));
                }
                else
                {
                    if (isSymbolsPackage)
                    {
                        string fileName = Path.GetFileName(artifact.ItemSpec);
                        artifact.SetMetadata("RelativeBlobPath", $"{AssetsVirtualDir}symbols/{fileName}");
                    }

                    blobArtifacts.Add(_blobArtifactModelFactory.CreateBlobArtifactModel(artifact));
                }
            }

            var signingInfoModel = _signingInformationModelFactory.CreateSigningInformationModelFromItems(
                itemsToSign, strongNameSignInfo, fileSignInfo, fileExtensionSignInfo,
                certificatesSignInfo, blobArtifacts, packageArtifacts);

            var buildModel = CreateModel(
                blobArtifacts,
                packageArtifacts,
                buildId,
                manifestBuildData,
                repoUri,
                repoBranch,
                repoCommit,
                isStableBuild,
                publishingVersion,
                isReleaseOnlyPackageVersion,
                signingInformationModel: signingInfoModel);
            return buildModel;
        }

        private BuildModel CreateModel(
            IEnumerable<BlobArtifactModel> blobArtifacts,
            IEnumerable<PackageArtifactModel> packageArtifacts,
            string manifestBuildId,
            string[] manifestBuildData,
            string manifestRepoName,
            string manifestBranch,
            string manifestCommit,
            bool isStableBuild,
            PublishingInfraVersion publishingVersion,
            bool isReleaseOnlyPackageVersion,
            SigningInformationModel signingInformationModel = null)
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
            buildModel.SigningInformation = signingInformationModel;
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
    }
}
