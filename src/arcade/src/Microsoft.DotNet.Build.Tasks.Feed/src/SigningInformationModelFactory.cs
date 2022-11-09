// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public interface ISigningInformationModelFactory
    {
        SigningInformationModel CreateSigningInformationModelFromItems(
            ITaskItem[] itemsToSign,
            ITaskItem[] strongNameSignInfo,
            ITaskItem[] fileSignInfo,
            ITaskItem[] fileExtensionSignInfo,
            ITaskItem[] certificatesSignInfo,
            IEnumerable<BlobArtifactModel> blobArtifacts,
            IEnumerable<PackageArtifactModel> packageArtifacts);
    }

    public class SigningInformationModelFactory : ISigningInformationModelFactory
    {
        private readonly TaskLoggingHelper _log;

        public SigningInformationModelFactory(TaskLoggingHelper logger)
        {
            _log = logger;
        }

        public SigningInformationModel CreateSigningInformationModelFromItems(
            ITaskItem[] itemsToSign,
            ITaskItem[] strongNameSignInfo,
            ITaskItem[] fileSignInfo,
            ITaskItem[] fileExtensionSignInfo,
            ITaskItem[] certificatesSignInfo,
            IEnumerable<BlobArtifactModel> blobArtifacts,
            IEnumerable<PackageArtifactModel> packageArtifacts)
        {
            List<ItemToSignModel> parsedItemsToSign = new();
            List<StrongNameSignInfoModel> parsedStrongNameSignInfo = new();
            List<FileSignInfoModel> parsedFileSignInfo = new();
            List<FileExtensionSignInfoModel> parsedFileExtensionSignInfoModel = new();
            List<CertificatesSignInfoModel> parsedCertificatesSignInfoModel = new();

            if (itemsToSign != null)
            {
                foreach (var itemToSign in itemsToSign)
                {
                    var fileName = Path.GetFileName(itemToSign.ItemSpec);
                    if (!blobArtifacts.Any(b => Path.GetFileName(b.Id).Equals(fileName, StringComparison.OrdinalIgnoreCase)) &&
                        !packageArtifacts.Any(p => $"{p.Id}.{p.Version}.nupkg".Equals(fileName, StringComparison.OrdinalIgnoreCase)))
                    {
                        _log.LogError($"Item to sign '{itemToSign}' was not found in the artifacts");
                    }
                    parsedItemsToSign.Add(new ItemToSignModel { Include = Path.GetFileName(fileName) });
                }
            }
            if (strongNameSignInfo != null)
            {
                foreach (var signInfo in strongNameSignInfo)
                {
                    var attributes = signInfo.CloneCustomMetadata() as IDictionary<string, string>;
                    parsedStrongNameSignInfo.Add(new StrongNameSignInfoModel { Include = Path.GetFileName(signInfo.ItemSpec), CertificateName = attributes["CertificateName"], PublicKeyToken = attributes["PublicKeyToken"] });
                }
            }
            if (fileSignInfo != null)
            {
                foreach (var signInfo in fileSignInfo)
                {
                    var attributes = signInfo.CloneCustomMetadata() as IDictionary<string, string>;
                    var fileSignInfoModel = new FileSignInfoModel { Include = Path.GetFileName(signInfo.ItemSpec), CertificateName = attributes["CertificateName"] };

                    if (attributes.TryGetValue("PublicKeyToken", out string publicKeyTokenValue))
                    {
                        fileSignInfoModel.PublicKeyToken = publicKeyTokenValue;
                    }
                    
                    if (attributes.TryGetValue("TargetFramework", out string targetFrameworkValue))
                    {
                        fileSignInfoModel.TargetFramework = targetFrameworkValue;
                    }

                    parsedFileSignInfo.Add(fileSignInfoModel);
                }
            }
            if (fileExtensionSignInfo != null)
            {
                foreach (var signInfo in fileExtensionSignInfo)
                {
                    var attributes = signInfo.CloneCustomMetadata() as IDictionary<string, string>;
                    parsedFileExtensionSignInfoModel.Add(new FileExtensionSignInfoModel { Include = signInfo.ItemSpec, CertificateName = attributes["CertificateName"] });
                }
            }
            if (certificatesSignInfo != null)
            {
                foreach (var signInfo in certificatesSignInfo)
                {
                    var attributes = signInfo.CloneCustomMetadata() as IDictionary<string, string>;
                    parsedCertificatesSignInfoModel.Add(new CertificatesSignInfoModel { Include = signInfo.ItemSpec, DualSigningAllowed = bool.Parse(attributes["DualSigningAllowed"]) });
                }
            }

            return new SigningInformationModel
            {
                ItemsToSign = parsedItemsToSign,
                StrongNameSignInfo = parsedStrongNameSignInfo,
                FileSignInfo = parsedFileSignInfo,
                FileExtensionSignInfo = parsedFileExtensionSignInfoModel,
                CertificatesSignInfo = parsedCertificatesSignInfoModel
            };
        }
    }
}
