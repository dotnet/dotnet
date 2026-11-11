// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    public sealed class SignatureTrustAndValidityVerificationProvider : ISignatureVerificationProvider
    {
        private readonly HashAlgorithmName _fingerprintAlgorithm;
        private readonly IEnumerable<KeyValuePair<string, HashAlgorithmName>>? _allowUntrustedRootList;

        public SignatureTrustAndValidityVerificationProvider(IEnumerable<KeyValuePair<string, HashAlgorithmName>>? allowUntrustedRootList = null)
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
            return GetTrustResultAsync(package, signature, settings, token, observedSummaries: null);
        }

        internal Task<PackageVerificationResult> GetTrustResultAsync(
            ISignedPackageReader package,
            PrimarySignature signature,
            SignedPackageVerifierSettings settings,
            CancellationToken token,
            ICollection<SignatureVerificationSummary>? observedSummaries)
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

            PackageVerificationResult result = Verify(signature, settings, observedSummaries);

            return Task.FromResult(result);
        }

        private PackageVerificationResult Verify(
            PrimarySignature signature,
            SignedPackageVerifierSettings settings,
            ICollection<SignatureVerificationSummary>? observedSummaries)
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
            List<SignatureVerificationSummary> summaries = new();
            SignatureVerificationRejectionReasons rejectionReasons = SignatureVerificationRejectionReasons.None;
            bool hasNonX509Rejection = false;

            var verifySettings = new SignatureVerifySettings(
                allowIllegal: settings.AllowIllegal,
                allowUntrusted: settings.AllowUntrusted || isUntrustedRootAllowed,
                allowUnknownRevocation: settings.AllowUnknownRevocation,
                reportUnknownRevocation: settings.ReportUnknownRevocation,
                reportUntrustedRoot: !isUntrustedRootAllowed,
                revocationMode: settings.RevocationMode);

            SignatureVerificationSummary? primarySummary = null;

            if (settings.SignaturePlacement.HasFlag(SignaturePlacement.PrimarySignature) &&
                VerificationUtility.IsVerificationTarget(signature.Type, settings.VerificationTarget))
            {
                primarySummary = VerifyValidityAndTrust(signature, settings, verifySettings, certificateExtraStore);
                primarySummary.Placement = SignaturePlacement.PrimarySignature;
                summaries.Add(primarySummary);
                observedSummaries?.Add(primarySummary);

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
                        rejectionReasons |= SignatureVerificationRejectionReasons.Other;
                        hasNonX509Rejection = true;
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
                    countersignatureSummary.Placement = SignaturePlacement.Countersignature;
                    summaries.Add(countersignatureSummary);
                    observedSummaries?.Add(countersignatureSummary);

                    if (primarySummary == null)
                    {
                        status = countersignatureSummary.Status;
                    }
                    else
                    {
                        if (countersignatureSummary.Status == SignatureVerificationStatus.Valid)
                        {
                            bool primaryWasRejected = status != SignatureVerificationStatus.Valid;
                            if (IsSignatureExpired(primarySummary) && HasUntrustedRoot(primarySummary))
                            {
                                // Exclude the issue of the primary signature being untrusted since the repository countersignature fulfills the role of a trust anchor.
                                issues = issues.Where(log => log.Code != NuGetLogCode.NU3018);

                                if (countersignatureSummary.Timestamp != null &&
                                Rfc3161TimestampVerificationUtility.ValidateSignerCertificateAgainstTimestamp(signature.SignerInfo.Certificate!, countersignatureSummary.Timestamp))
                                {
                                    // Exclude the issue of the primary signature being expired since the repository countersignature fulfills the role of a trusted timestamp.
                                    issues = issues.Where(log => log.Code != NuGetLogCode.NU3037);

                                    status = SignatureVerificationStatus.Valid;
                                }
                            }
                            else if (IsSignatureExpired(primarySummary) &&
                                countersignatureSummary.Timestamp != null &&
                                Rfc3161TimestampVerificationUtility.ValidateSignerCertificateAgainstTimestamp(signature.SignerInfo.Certificate!, countersignatureSummary.Timestamp))
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
                            if (primaryWasRejected && status == SignatureVerificationStatus.Valid)
                            {
                                primarySummary.SignerDecisionRelevantChainStatusFlags = X509ChainStatusFlags.NoError;
                                primarySummary.TimestampDecisionRelevantChainStatusFlags = X509ChainStatusFlags.NoError;
                                primarySummary.SignerChainRejectionFlags = X509ChainStatusFlags.NoError;
                                primarySummary.TimestampChainRejectionFlags = X509ChainStatusFlags.NoError;
                                primarySummary.SignerHasNonX509Rejection = false;
                                primarySummary.TimestampHasNonX509Rejection = false;
                            }
                        }

                        // Both the primary signature and the repository countersignature were evaluated.
                        // The overall status should be the more severe status of the two.
                        status = (SignatureVerificationStatus)Math.Min((int)status, (int)countersignatureSummary.Status);
                    }

                    issues = issues.Concat(countersignatureSummary.Issues);
                }
            }

            return new SignedPackageVerificationResult(status, signature, issues)
            {
                SignatureSummaries = summaries,
                RejectionReasons = status == SignatureVerificationStatus.Valid
                    ? SignatureVerificationRejectionReasons.None
                    : summaries.Aggregate(rejectionReasons, (reasons, summary) => reasons | summary.RejectionReasons),
                HasNonX509Rejection = status != SignatureVerificationStatus.Valid &&
                    (hasNonX509Rejection || summaries.Any(summary => summary.SignerHasNonX509Rejection || summary.TimestampHasNonX509Rejection))
            };
        }

        private SignatureVerificationSummary GetTimestamp(
            Signature signature,
            SignedPackageVerifierSettings verifierSettings,
            out Timestamp? timestamp)
        {
            var issues = new List<SignatureLog>();
            SignatureVerificationStatus status;
            SignatureVerificationStatusFlags statusFlags;

            bool succeeded = signature.TryGetValidTimestamp(
                verifierSettings, _fingerprintAlgorithm, issues, out statusFlags, out timestamp,
                out X509ChainStatusFlags chainStatusFlags, out SignatureChainEvaluationStatus chainEvaluationStatus);

            status = VerificationUtility.GetSignatureVerificationStatus(statusFlags);

            if (!succeeded)
            {
                if (statusFlags == SignatureVerificationStatusFlags.NoValidTimestamp ||
                    statusFlags == SignatureVerificationStatusFlags.MultipleTimestamps)
                {
                    status = SignatureVerificationStatus.Disallowed;
                }
            }

            return new SignatureVerificationSummary(signature.Type, status, statusFlags, issues)
            {
                TimestampChainStatusFlags = chainStatusFlags,
                TimestampChainEvaluationStatus = chainEvaluationStatus
            };
        }

        private SignatureVerificationSummary VerifyValidityAndTrust(
            Signature signature,
            SignedPackageVerifierSettings verifierSettings,
            SignatureVerifySettings settings,
            X509Certificate2Collection certificateExtraStore)
        {
            Timestamp? timestamp;
            var timestampSummary = GetTimestamp(signature, verifierSettings, out timestamp);

            var status = signature.Verify(
            timestamp!,
            settings,
            _fingerprintAlgorithm,
            certificateExtraStore);

            bool timestampRejected = timestampSummary.Status != SignatureVerificationStatus.Valid && !verifierSettings.AllowIgnoreTimestamp;
            SignatureVerificationSummary summary = timestampRejected
                ? new SignatureVerificationSummary(
                    signature.Type,
                    SignatureVerificationStatus.Disallowed,
                    SignatureVerificationStatusFlags.NoValidTimestamp,
                    status.Issues.Concat(timestampSummary.Issues))
                : new SignatureVerificationSummary(
                status.SignatureType,
                status.Status,
                status.Flags,
                status.Timestamp,
                status.ExpirationTime,
                status.Issues.Concat(timestampSummary.Issues));
            summary.SignerChainStatusFlags = status.SignerChainStatusFlags;
            summary.SignerChainEvaluationStatus = status.SignerChainEvaluationStatus;
            summary.TimestampChainStatusFlags = timestampSummary.TimestampChainStatusFlags;
            summary.TimestampChainEvaluationStatus = timestampSummary.TimestampChainEvaluationStatus;
            summary.SignerRejectionReasons = GetRejectionReasons(status, verifierSettings.AllowUnknownRevocation);
            summary.TimestampRejectionReasons = timestampRejected
                ? GetRejectionReasons(timestampSummary, verifierSettings.AllowUnknownRevocation)
                : SignatureVerificationRejectionReasons.None;
            summary.SignerDecisionRelevantChainStatusFlags = status.SignerChainStatusFlags;
            summary.SignerChainRejectionFlags = GetChainRejectionFlags(
                status.SignerChainStatusFlags, status, verifierSettings.AllowUnknownRevocation);
            summary.SignerHasNonX509Rejection = HasNonX509Rejection(status, summary.SignerChainRejectionFlags);
            // An invalid optional timestamp was discarded; a selected or required timestamp participates in verification.
            if (timestamp != null || timestampRejected)
            {
                summary.TimestampDecisionRelevantChainStatusFlags = timestampSummary.TimestampChainStatusFlags;
                summary.TimestampChainRejectionFlags = timestampRejected
                    ? GetChainRejectionFlags(timestampSummary.TimestampChainStatusFlags, timestampSummary, verifierSettings.AllowUnknownRevocation)
                    : X509ChainStatusFlags.NoError;
                summary.TimestampHasNonX509Rejection = timestampRejected &&
                    HasNonX509Rejection(timestampSummary, summary.TimestampChainRejectionFlags);
            }
            return summary;
        }

        private static bool HasNonX509Rejection(SignatureVerificationSummary summary, X509ChainStatusFlags chainRejectionFlags)
        {
            if (summary.Status == SignatureVerificationStatus.Valid)
            {
                return false;
            }

            const SignatureVerificationStatusFlags chainFlags =
                SignatureVerificationStatusFlags.ChainBuildingFailure |
                SignatureVerificationStatusFlags.CertificateRevoked |
                SignatureVerificationStatusFlags.UntrustedRoot |
                SignatureVerificationStatusFlags.UnknownRevocation |
                SignatureVerificationStatusFlags.CertificateExpired |
                SignatureVerificationStatusFlags.UnknownBuildStatus;
            return (summary.Flags & ~chainFlags) != 0 ||
                (summary.Flags.HasFlag(SignatureVerificationStatusFlags.CertificateExpired) &&
                    !chainRejectionFlags.HasFlag(X509ChainStatusFlags.NotTimeValid));
        }

        private static X509ChainStatusFlags GetChainRejectionFlags(
            X509ChainStatusFlags observedFlags, SignatureVerificationSummary summary, bool allowUnknownRevocation)
        {
            if (summary.Status == SignatureVerificationStatus.Valid)
            {
                return X509ChainStatusFlags.NoError;
            }

            X509ChainStatusFlags enforcedFlags = X509ChainStatusFlags.NoError;
            if (summary.Flags.HasFlag(SignatureVerificationStatusFlags.ChainBuildingFailure))
            {
                enforcedFlags |= CertificateChainUtility.DefaultObservedStatusFlags;
            }
            if (summary.Flags.HasFlag(SignatureVerificationStatusFlags.CertificateRevoked))
            {
                enforcedFlags |= X509ChainStatusFlags.Revoked;
            }
            if (summary.Flags.HasFlag(SignatureVerificationStatusFlags.UntrustedRoot))
            {
                enforcedFlags |= X509ChainStatusFlags.UntrustedRoot;
            }
            if (!allowUnknownRevocation && summary.Flags.HasFlag(SignatureVerificationStatusFlags.UnknownRevocation))
            {
                enforcedFlags |= CertificateChainUtility.RevocationUnavailableFlags;
            }
            if (summary.Flags.HasFlag(SignatureVerificationStatusFlags.CertificateExpired))
            {
                enforcedFlags |= X509ChainStatusFlags.NotTimeValid;
            }
            return observedFlags & enforcedFlags;
        }

        private static SignatureVerificationRejectionReasons GetRejectionReasons(
            SignatureVerificationSummary summary, bool allowUnknownRevocation)
        {
            if (summary.Status == SignatureVerificationStatus.Valid)
            {
                return SignatureVerificationRejectionReasons.None;
            }

            SignatureVerificationRejectionReasons reasons = SignatureVerificationRejectionReasons.None;
            if (!allowUnknownRevocation && summary.Flags.HasFlag(SignatureVerificationStatusFlags.UnknownRevocation))
            {
                reasons |= SignatureVerificationRejectionReasons.RevocationUnavailable;
            }

            if (summary.Flags.HasFlag(SignatureVerificationStatusFlags.UnknownBuildStatus))
            {
                reasons |= SignatureVerificationRejectionReasons.Unknown;
            }

            if ((summary.Flags & ~(SignatureVerificationStatusFlags.UnknownRevocation | SignatureVerificationStatusFlags.UnknownBuildStatus)) != 0)
            {
                reasons |= SignatureVerificationRejectionReasons.Other;
            }

            return reasons == SignatureVerificationRejectionReasons.None
                ? SignatureVerificationRejectionReasons.Unknown
                : reasons;
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


    }
}
