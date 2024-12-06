// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#if IS_SIGNING_SUPPORTED
using System.Linq;
using NuGet.Common;
#endif

namespace NuGet.Packaging.Signing
{
    public class AllowListVerificationProvider : ISignatureVerificationProvider
    {
        private readonly IReadOnlyCollection<VerificationAllowListEntry> _allowList;
        private readonly string _emptyListErrorMessage;
        private readonly string _noMatchErrorMessage;
        private readonly bool _requireNonEmptyAllowList;

        public AllowListVerificationProvider(IReadOnlyCollection<VerificationAllowListEntry> allowList, bool requireNonEmptyAllowList = false, string emptyListErrorMessage = "", string noMatchErrorMessage = "")
        {
            _allowList = allowList;
            _requireNonEmptyAllowList = requireNonEmptyAllowList;

            _emptyListErrorMessage = string.IsNullOrEmpty(emptyListErrorMessage) ? Strings.DefaultError_EmptyAllowList : emptyListErrorMessage;
            _noMatchErrorMessage = string.IsNullOrEmpty(noMatchErrorMessage) ? Strings.DefaultError_NoMatchInAllowList : noMatchErrorMessage;
        }

        public Task<PackageVerificationResult> GetTrustResultAsync(ISignedPackageReader package, PrimarySignature signature, SignedPackageVerifierSettings settings, CancellationToken token)
        {
            return Task.FromResult(VerifyAllowList(package, signature, settings));
        }

#if IS_SIGNING_SUPPORTED
        private PackageVerificationResult VerifyAllowList(ISignedPackageReader package, PrimarySignature signature, SignedPackageVerifierSettings settings)
        {
            var treatIssuesAsErrors = !settings.AllowUntrusted;
            var status = SignatureVerificationStatus.Valid;
            var issues = new List<SignatureLog>();

            if (_allowList == null || _allowList.Count == 0)
            {
                if (_requireNonEmptyAllowList)
                {
                    status = SignatureVerificationStatus.Disallowed;
                    issues.Add(SignatureLog.Error(code: NuGetLogCode.NU3034, message: _emptyListErrorMessage));
                }
            }
            else if (!IsSignatureAllowed(signature, _allowList))
            {
                if (!settings.AllowUntrusted)
                {
                    status = SignatureVerificationStatus.Disallowed;
                }

                issues.Add(SignatureLog.Issue(fatal: treatIssuesAsErrors, code: NuGetLogCode.NU3034, message: _noMatchErrorMessage));
            }

            return new SignedPackageVerificationResult(status, signature, issues);
        }

        private bool IsSignatureAllowed(
            PrimarySignature signature,
            IReadOnlyCollection<VerificationAllowListEntry> allowList)
        {
            var primarySignatureCertificateFingerprintLookUp = new Dictionary<HashAlgorithmName, string>();
            var countersignatureCertificateFingerprintLookUp = new Dictionary<HashAlgorithmName, string>();
            var repositoryCountersignature = new Lazy<RepositoryCountersignature>(() => RepositoryCountersignature.GetRepositoryCountersignature(signature));

            foreach (var allowedEntry in allowList)
            {
                // Verify the certificate hash allow list objects
                var certificateHashEntry = allowedEntry as CertificateHashAllowListEntry;
                if (certificateHashEntry != null)
                {
                    if (certificateHashEntry.Placement.HasFlag(SignaturePlacement.PrimarySignature))
                    {
                        // Get information needed for allow list verification
                        var primarySignatureCertificateFingerprint = GetCertificateFingerprint(
                            signature,
                            certificateHashEntry.FingerprintAlgorithm,
                            primarySignatureCertificateFingerprintLookUp);

                        if (IsSignatureTargeted(certificateHashEntry.Target, signature) &&
                            StringComparer.OrdinalIgnoreCase.Equals(certificateHashEntry.Fingerprint, primarySignatureCertificateFingerprint))
                        {
                            if (ShouldVerifyOwners(certificateHashEntry as TrustedSignerAllowListEntry, signature as IRepositorySignature, out var allowedOwners, out var actualOwners))
                            {
                                if (allowedOwners.Intersect(actualOwners).Any())
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }

                    if (certificateHashEntry.Placement.HasFlag(SignaturePlacement.Countersignature))
                    {
                        if (repositoryCountersignature.Value != null)
                        {
                            // Get information needed for allow list verification
                            var countersignatureCertificateFingerprint = GetCertificateFingerprint(
                                repositoryCountersignature.Value,
                                certificateHashEntry.FingerprintAlgorithm,
                                countersignatureCertificateFingerprintLookUp);

                            if (IsSignatureTargeted(certificateHashEntry.Target, repositoryCountersignature.Value) &&
                                StringComparer.OrdinalIgnoreCase.Equals(certificateHashEntry.Fingerprint, countersignatureCertificateFingerprint))
                            {
                                if (ShouldVerifyOwners(certificateHashEntry as TrustedSignerAllowListEntry, repositoryCountersignature.Value, out var allowedOwners, out var actualOwners))
                                {
                                    if (allowedOwners.Intersect(actualOwners).Any())
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static bool ShouldVerifyOwners(TrustedSignerAllowListEntry entry, IRepositorySignature repoSignature, out IReadOnlyList<string> allowedOwners, out IReadOnlyList<string> actualOwners)
        {
            allowedOwners = null;
            actualOwners = null;

            if (entry != null && entry.Target.HasFlag(VerificationTarget.Repository) && entry.Owners != null && entry.Owners.Any() && repoSignature != null)
            {
                allowedOwners = entry.Owners ?? Enumerable.Empty<string>().ToList();
                actualOwners = repoSignature.PackageOwners ?? Enumerable.Empty<string>().ToList();

                return true;
            }

            return false;
        }

        private static bool IsSignatureTargeted(VerificationTarget target, Signature signature)
        {
            return (target.HasFlag(VerificationTarget.Author) && signature is AuthorPrimarySignature) ||
                (target.HasFlag(VerificationTarget.Repository) && signature is RepositoryPrimarySignature) ||
                (target.HasFlag(VerificationTarget.Repository) && signature is RepositoryCountersignature);
        }

        private static string GetCertificateFingerprint(
            Signature signature,
            HashAlgorithmName fingerprintAlgorithm,
            IDictionary<HashAlgorithmName, string> CertificateFingerprintLookUp)
        {
            if (!CertificateFingerprintLookUp.TryGetValue(fingerprintAlgorithm, out var fingerprintString))
            {
                fingerprintString = CertificateUtility.GetHashString(signature.SignerInfo.Certificate, fingerprintAlgorithm);
                CertificateFingerprintLookUp[fingerprintAlgorithm] = fingerprintString;
            }

            return fingerprintString;
        }

#else
        private PackageVerificationResult VerifyAllowList(ISignedPackageReader package, PrimarySignature signature, SignedPackageVerifierSettings settings)
        {
            throw new NotSupportedException();
        }
#endif
    }
}
