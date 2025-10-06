// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Packaging.Signing
{
    public static class RepositorySignatureInfoUtility
    {
        /// <summary>
        /// Gets SignedPackageVerifierSettings from a given RepositorySignatureInfo.
        /// </summary>
        /// <param name="repoSignatureInfo">RepositorySignatureInfo to be used.</param>
        /// <param name="fallbackSettings">SignedPackageVerifierSettings to be used if RepositorySignatureInfo is unavailable.</param>
        /// <returns>SignedPackageVerifierSettings based on the RepositorySignatureInfo and SignedPackageVerifierSettings.</returns>
        public static SignedPackageVerifierSettings GetSignedPackageVerifierSettings(
            RepositorySignatureInfo repoSignatureInfo,
            SignedPackageVerifierSettings fallbackSettings)
        {
            if (fallbackSettings == null)
            {
                throw new ArgumentNullException(nameof(fallbackSettings));
            }

            if (repoSignatureInfo == null)
            {
                return fallbackSettings;
            }
            else
            {
                // Allow unsigned only if the common settings allow it and repository does not have all packages signed
                var allowUnsigned = fallbackSettings.AllowUnsigned && !repoSignatureInfo.AllRepositorySigned;

                // Allow untrusted only if the common settings allow it and repository does not have all packages signed
                var allowUntrusted = fallbackSettings.AllowUntrusted && !repoSignatureInfo.AllRepositorySigned;

                return new SignedPackageVerifierSettings(
                    allowUnsigned,
                    fallbackSettings.AllowIllegal,
                    allowUntrusted,
                    fallbackSettings.AllowIgnoreTimestamp,
                    fallbackSettings.AllowMultipleTimestamps,
                    fallbackSettings.AllowNoTimestamp,
                    fallbackSettings.AllowUnknownRevocation,
                    fallbackSettings.ReportUnknownRevocation,
                    fallbackSettings.VerificationTarget,
                    fallbackSettings.SignaturePlacement,
                    fallbackSettings.RepositoryCountersignatureVerificationBehavior,
                    fallbackSettings.RevocationMode);
            }
        }

        public static IReadOnlyCollection<CertificateHashAllowListEntry> GetRepositoryAllowList(IEnumerable<IRepositoryCertificateInfo> repositoryCertificateInfos)
        {
            HashSet<CertificateHashAllowListEntry> repositoryAllowedCertificates = null;

            if (repositoryCertificateInfos != null)
            {
                repositoryAllowedCertificates = new HashSet<CertificateHashAllowListEntry>();

                foreach (var certInfo in repositoryCertificateInfos)
                {
                    foreach (var hashAlgorithm in SigningSpecifications.V1.AllowedHashAlgorithms)
                    {
                        var fingerprint = certInfo.Fingerprints[hashAlgorithm.ConvertToOidString()];

                        if (!string.IsNullOrEmpty(fingerprint))
                        {
                            repositoryAllowedCertificates.Add(new CertificateHashAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, fingerprint, hashAlgorithm));
                        }
                    }
                }
            }

            return repositoryAllowedCertificates;
        }
    }
}
