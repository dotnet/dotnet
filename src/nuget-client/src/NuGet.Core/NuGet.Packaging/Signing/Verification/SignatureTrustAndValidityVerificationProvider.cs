// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public sealed class SignatureTrustAndValidityVerificationProvider : ISignatureVerificationProvider
    {
        private readonly HashAlgorithmName _fingerprintAlgorithm;
        private readonly IEnumerable<KeyValuePair<string, HashAlgorithmName>> _allowUntrustedRootList;

        public SignatureTrustAndValidityVerificationProvider(IEnumerable<KeyValuePair<string, HashAlgorithmName>> allowUntrustedRootList = null)
        {
            _fingerprintAlgorithm = HashAlgorithmName.SHA256;
            _allowUntrustedRootList = allowUntrustedRootList;
        }

        public Task<PackageVerificationResult> GetTrustResultAsync(
            ISignedPackageReader package,
            PrimarySignature signature,
            SignedPackageVerifierSettings settings,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var result = Verify(signature, settings);

            return Task.FromResult(result);
        }

#if IS_SIGNING_SUPPORTED
        private PackageVerificationResult Verify(
            PrimarySignature signature,
            SignedPackageVerifierSettings settings)
        {
            var certificateExtraStore = signature.SignedCms.Certificates;
            var repositoryCountersignatureExists = SignatureUtility.HasRepositoryCountersignature(signature);
            var isRepositoryCountersignatureVerificationRequested = settings.VerificationTarget.HasFlag(VerificationTarget.Repository) &&
                settings.SignaturePlacement.HasFlag(SignaturePlacement.Countersignature);
            var allowDeferralToRepositoryCountersignature = isRepositoryCountersignatureVerificationRequested &&
                repositoryCountersignatureExists;
            var status = SignatureVerificationStatus.Unknown;
            var issues = Enumerable.Empty<SignatureLog>();
            var isUntrustedRootAllowed = IsUntrustedRootAllowed(signature);

            var verifySettings = new SignatureVerifySettings(
                allowIllegal: settings.AllowIllegal,
                allowUntrusted: settings.AllowUntrusted || isUntrustedRootAllowed,
                allowUnknownRevocation: settings.AllowUnknownRevocation,
                reportUnknownRevocation: settings.ReportUnknownRevocation,
                reportUntrustedRoot: !isUntrustedRootAllowed,
                revocationMode: settings.RevocationMode);

            SignatureVerificationSummary primarySummary = null;

            if (settings.SignaturePlacement.HasFlag(SignaturePlacement.PrimarySignature) &&
                VerificationUtility.IsVerificationTarget(signature.Type, settings.VerificationTarget))
            {
                primarySummary = VerifyValidityAndTrust(signature, settings, verifySettings, certificateExtraStore);

                issues = issues.Concat(primarySummary.Issues);

                status = primarySummary.Status;
            }

            Debug.Assert(isRepositoryCountersignatureVerificationRequested != (settings.RepositoryCountersignatureVerificationBehavior == SignatureVerificationBehavior.Never));

            bool shouldVerifyRepositoryCountersignature;

            switch (settings.RepositoryCountersignatureVerificationBehavior)
            {
                case SignatureVerificationBehavior.IfExists:
                    shouldVerifyRepositoryCountersignature = isRepositoryCountersignatureVerificationRequested &&
                        repositoryCountersignatureExists;
                    break;

                case SignatureVerificationBehavior.IfExistsAndIsNecessary:
                    // The repository countersignature should be evaluated if settings allow it, if a repository countersignature exists
                    // and if either settings only allow a repository countersignature to be evaluated or the primary signature has some
                    // validation/trust issues that may benefit from a repository countersignature fallback.
                    shouldVerifyRepositoryCountersignature = isRepositoryCountersignatureVerificationRequested &&
                        repositoryCountersignatureExists &&
                        (primarySummary == null ||
                            (primarySummary != null &&
                            (HasUntrustedRoot(primarySummary) || IsSignatureExpired(primarySummary))));
                    break;

                case SignatureVerificationBehavior.Always:
                    shouldVerifyRepositoryCountersignature = isRepositoryCountersignatureVerificationRequested;
                    break;

                case SignatureVerificationBehavior.Never:
                    shouldVerifyRepositoryCountersignature = false;
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (shouldVerifyRepositoryCountersignature)
            {
                var countersignature = RepositoryCountersignature.GetRepositoryCountersignature(signature);

                if (countersignature == null)
                {
                    if (settings.RepositoryCountersignatureVerificationBehavior == SignatureVerificationBehavior.Always)
                    {
                        issues = issues.Concat(new[] { SignatureLog.Error(NuGetLogCode.NU3038, Strings.NoRepositoryCountersignature) });
                        status = SignatureVerificationStatus.Disallowed;
                    }
                }
                else
                {
                    isUntrustedRootAllowed = IsUntrustedRootAllowed(countersignature);

                    verifySettings = new SignatureVerifySettings(
                        allowIllegal: settings.AllowIllegal,
                        allowUntrusted: settings.AllowUntrusted || isUntrustedRootAllowed,
                        allowUnknownRevocation: settings.AllowUnknownRevocation,
                        reportUnknownRevocation: settings.ReportUnknownRevocation,
                        reportUntrustedRoot: !isUntrustedRootAllowed,
                        revocationMode: settings.RevocationMode);

                    var countersignatureSummary = VerifyValidityAndTrust(countersignature, settings, verifySettings, certificateExtraStore);

                    if (primarySummary == null)
                    {
                        status = countersignatureSummary.Status;
                    }
                    else
                    {
                        if (countersignatureSummary.Status == SignatureVerificationStatus.Valid)
                        {
                            if (IsSignatureExpired(primarySummary) && HasUntrustedRoot(primarySummary))
                            {
                                // Exclude the issue of the primary signature being untrusted since the repository countersignature fulfills the role of a trust anchor.
                                issues = issues.Where(log => log.Code != NuGetLogCode.NU3018);

                                if (countersignatureSummary.Timestamp != null &&
                                Rfc3161TimestampVerificationUtility.ValidateSignerCertificateAgainstTimestamp(signature.SignerInfo.Certificate, countersignatureSummary.Timestamp))
                                {
                                    // Exclude the issue of the primary signature being expired since the repository countersignature fulfills the role of a trusted timestamp.
                                    issues = issues.Where(log => log.Code != NuGetLogCode.NU3037);

                                    status = SignatureVerificationStatus.Valid;
                                }
                            }
                            else if (IsSignatureExpired(primarySummary) &&
                                countersignatureSummary.Timestamp != null &&
                                Rfc3161TimestampVerificationUtility.ValidateSignerCertificateAgainstTimestamp(signature.SignerInfo.Certificate, countersignatureSummary.Timestamp))
                            {
                                // Exclude the issue of the primary signature being expired since the repository countersignature fulfills the role of a trusted timestamp.
                                issues = issues.Where(log => log.Code != NuGetLogCode.NU3037);

                                status = SignatureVerificationStatus.Valid;
                            }
                            else if (HasUntrustedRoot(primarySummary))
                            {
                                // Exclude the issue of the primary signature being untrusted since the repository countersignature fulfills the role of a trust anchor.
                                issues = issues.Where(log => log.Code != NuGetLogCode.NU3018);

                                status = SignatureVerificationStatus.Valid;
                            }
                        }

                        // Both the primary signature and the repository countersignature were evaluated.
                        // The overall status should be the more severe status of the two.
                        status = (SignatureVerificationStatus)Math.Min((int)status, (int)countersignatureSummary.Status);
                    }

                    issues = issues.Concat(countersignatureSummary.Issues);
                }
            }

            return new SignedPackageVerificationResult(status, signature, issues);
        }

        private SignatureVerificationSummary GetTimestamp(
            Signature signature,
            SignedPackageVerifierSettings verifierSettings,
            out Timestamp timestamp)
        {
            var issues = new List<SignatureLog>();
            SignatureVerificationStatus status;
            SignatureVerificationStatusFlags statusFlags;

            var succeeded = signature.TryGetValidTimestamp(verifierSettings, _fingerprintAlgorithm, issues, out statusFlags, out timestamp);

            status = VerificationUtility.GetSignatureVerificationStatus(statusFlags);

            if (!succeeded)
            {
                if (statusFlags == SignatureVerificationStatusFlags.NoValidTimestamp ||
                    statusFlags == SignatureVerificationStatusFlags.MultipleTimestamps)
                {
                    status = SignatureVerificationStatus.Disallowed;
                }
            }

            return new SignatureVerificationSummary(signature.Type, status, statusFlags, issues);
        }

        private SignatureVerificationSummary VerifyValidityAndTrust(
            Signature signature,
            SignedPackageVerifierSettings verifierSettings,
            SignatureVerifySettings settings,
            X509Certificate2Collection certificateExtraStore)
        {
            Timestamp timestamp;
            var timestampSummary = GetTimestamp(signature, verifierSettings, out timestamp);

            var status = signature.Verify(
            timestamp,
            settings,
            _fingerprintAlgorithm,
            certificateExtraStore);

            if (timestampSummary.Status != SignatureVerificationStatus.Valid && !verifierSettings.AllowIgnoreTimestamp)
            {
                return new SignatureVerificationSummary(
                    signature.Type,
                    SignatureVerificationStatus.Disallowed,
                    SignatureVerificationStatusFlags.NoValidTimestamp,
                    status.Issues.Concat(timestampSummary.Issues));
            }

            return new SignatureVerificationSummary(
                status.SignatureType,
                status.Status,
                status.Flags,
                status.Timestamp,
                status.ExpirationTime,
                status.Issues.Concat(timestampSummary.Issues));
        }

        private bool IsUntrustedRootAllowed(Signature signature)
        {
            if (_allowUntrustedRootList != null)
            {
                foreach (var allowUntrustedRoot in _allowUntrustedRootList)
                {
                    try
                    {
                        var signingCertificateFingerprint = signature.GetSigningCertificateFingerprint(allowUntrustedRoot.Value);

                        if (string.Equals(allowUntrustedRoot.Key, signingCertificateFingerprint, StringComparison.Ordinal))
                        {
                            return true;
                        }
                    }
                    // if the exception is InvalidOperationException it means the signature is invalid
                    catch (InvalidOperationException)
                    {
                        return false;
                    }
                    // any other exception means the entry is invalid, therefore we just skip it
                    catch { }
                }
            }

            return false;
        }

        private static bool HasUntrustedRoot(SignatureVerificationSummary summary)
        {
            return summary.SignatureType != SignatureType.Repository &&
                summary.Status != SignatureVerificationStatus.Valid &&
                summary.Flags.HasFlag(SignatureVerificationStatusFlags.UntrustedRoot);
        }

        private static bool IsSignatureExpired(SignatureVerificationSummary summary)
        {
            return summary.SignatureType != SignatureType.Repository && summary.ExpirationTime.HasValue;
        }

#else
        private PackageVerificationResult Verify(
            PrimarySignature signature,
            SignedPackageVerifierSettings settings)
        {
            throw new NotSupportedException();
        }
#endif
    }
}
