// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace NuGet.Packaging
{
    public class PackageSigningTelemetryEvent : TelemetryEvent
    {
        public PackageSignType PackageSignType => (PackageSignType)(base[nameof(PackageSignType)]!);

        public NuGetOperationStatus Status => (NuGetOperationStatus)(base[nameof(Status)]!);

        [Obsolete("This seems to be unused and will be removed in a future version.")]
        public string? ExtractionId => base[nameof(ExtractionId)] as string;

        public const string EventName = "SigningInformation";

        public PackageSigningTelemetryEvent() :
            base(EventName)
        {
        }

        public void SetResult(PackageSignType packageSignType, NuGetOperationStatus status)
        {
            base[nameof(PackageSignType)] = packageSignType;
            base[nameof(Status)] = status;
        }

        internal static (X509ChainStatusFlags DecisionRelevantFlags, X509ChainStatusFlags RejectionFlags, bool HasNonX509Rejection) SetRevocationInformation(
            TelemetryEvent telemetryEvent,
            IEnumerable<PackageVerificationResult> results,
            RevocationMode revocationMode)
        {
            X509ChainStatusFlags signerFlags = X509ChainStatusFlags.NoError;
            X509ChainStatusFlags timestampFlags = X509ChainStatusFlags.NoError;
            List<TelemetryEvent> chains = new();
            List<SignatureChainEvaluationStatus> evaluationStatuses = new();
            SignatureVerificationRejectionReasons rejectionReasons = SignatureVerificationRejectionReasons.None;
            X509ChainStatusFlags revocationRejectionFlags = X509ChainStatusFlags.NoError;
            X509ChainStatusFlags decisionRelevantFlags = X509ChainStatusFlags.NoError;
            X509ChainStatusFlags chainRejectionFlags = X509ChainStatusFlags.NoError;
            bool hasNonX509Rejection = false;

            foreach (PackageVerificationResult result in results)
            {
                hasNonX509Rejection |= result.HasNonX509Rejection;
                // Parsing-error results carry observations, not completed trust decisions.
                bool attributesRejection = result is SignedPackageVerificationResult &&
                    result.Trust != SignatureVerificationStatus.Valid;
                foreach (SignatureVerificationSummary summary in result.SignatureSummaries)
                {
                    signerFlags |= summary.SignerChainStatusFlags;
                    timestampFlags |= summary.TimestampChainStatusFlags;
                    SignatureVerificationRejectionReasons signerRejectionReasons = attributesRejection
                        ? summary.SignerRejectionReasons : SignatureVerificationRejectionReasons.None;
                    SignatureVerificationRejectionReasons timestampRejectionReasons = attributesRejection
                        ? summary.TimestampRejectionReasons : SignatureVerificationRejectionReasons.None;
                    X509ChainStatusFlags signerDecisionFlags = result is SignedPackageVerificationResult
                        ? summary.SignerDecisionRelevantChainStatusFlags : X509ChainStatusFlags.NoError;
                    X509ChainStatusFlags timestampDecisionFlags = result is SignedPackageVerificationResult
                        ? summary.TimestampDecisionRelevantChainStatusFlags : X509ChainStatusFlags.NoError;
                    X509ChainStatusFlags signerRejectionFlags = attributesRejection
                        ? summary.SignerChainRejectionFlags : X509ChainStatusFlags.NoError;
                    X509ChainStatusFlags timestampRejectionFlags = attributesRejection
                        ? summary.TimestampChainRejectionFlags : X509ChainStatusFlags.NoError;
                    decisionRelevantFlags |= signerDecisionFlags | timestampDecisionFlags;
                    chainRejectionFlags |= signerRejectionFlags | timestampRejectionFlags;
                    chains.Add(CreateChainTelemetry(summary, CertificateType.Signature,
                        summary.SignerChainStatusFlags, summary.SignerChainEvaluationStatus, signerRejectionReasons,
                        signerDecisionFlags, signerRejectionFlags));
                    chains.Add(CreateChainTelemetry(summary, CertificateType.Timestamp,
                        summary.TimestampChainStatusFlags, summary.TimestampChainEvaluationStatus, timestampRejectionReasons,
                        timestampDecisionFlags, timestampRejectionFlags));
                    if (signerRejectionReasons.HasFlag(SignatureVerificationRejectionReasons.RevocationUnavailable))
                    {
                        revocationRejectionFlags |= summary.SignerChainStatusFlags & CertificateChainUtility.RevocationUnavailableFlags;
                    }
                    if (timestampRejectionReasons.HasFlag(SignatureVerificationRejectionReasons.RevocationUnavailable))
                    {
                        revocationRejectionFlags |= summary.TimestampChainStatusFlags & CertificateChainUtility.RevocationUnavailableFlags;
                    }
                    evaluationStatuses.Add(summary.SignerChainEvaluationStatus);
                    evaluationStatuses.Add(summary.TimestampChainEvaluationStatus);
                }

                if (result is SignedPackageVerificationResult signedResult)
                {
                    if (result.Trust != SignatureVerificationStatus.Valid)
                    {
                        rejectionReasons |= signedResult.RejectionReasons == SignatureVerificationRejectionReasons.None
                            ? SignatureVerificationRejectionReasons.Unknown
                            : signedResult.RejectionReasons;
                    }
                }
                else if (result.RejectionReasons != SignatureVerificationRejectionReasons.None)
                {
                    rejectionReasons |= result.RejectionReasons;
                }
                else if (result.Trust != SignatureVerificationStatus.Valid)
                {
                    rejectionReasons |= result.Trust == SignatureVerificationStatus.Unknown
                        ? SignatureVerificationRejectionReasons.Unknown
                        : SignatureVerificationRejectionReasons.Other;
                }
            }

            telemetryEvent["RevocationMode"] = revocationMode.ToString();
            telemetryEvent["SignerRevocationStatusFlags"] = (int)(signerFlags & CertificateChainUtility.RevocationUnavailableFlags);
            telemetryEvent["TimestampRevocationStatusFlags"] = (int)(timestampFlags & CertificateChainUtility.RevocationUnavailableFlags);
            // False means no unavailable-status flag was observed, not that revocation information was available.
            // Interpret alongside evaluation status and final package acceptance.
            telemetryEvent["RevocationInformationUnavailable"] = ((signerFlags | timestampFlags) & CertificateChainUtility.RevocationUnavailableFlags) != 0;
            telemetryEvent["SignerChainStatusFlags"] = (int)signerFlags;
            telemetryEvent["TimestampChainStatusFlags"] = (int)timestampFlags;
            telemetryEvent["RevocationEvaluationStatus"] = GetEvaluationStatus(evaluationStatuses).ToString();
            telemetryEvent["VerificationRejectionReasons"] = (int)rejectionReasons;
            telemetryEvent["VerificationRevocationRejectionFlags"] = (int)revocationRejectionFlags;
            telemetryEvent["VerificationDecisionRelevantChainStatusFlags"] = (int)decisionRelevantFlags;
            telemetryEvent["VerificationChainRejectionFlags"] = (int)chainRejectionFlags;
            telemetryEvent["AcceptanceDecisionRelevantChainStatusFlags"] = (int)decisionRelevantFlags;
            telemetryEvent["AcceptanceChainRejectionFlags"] = (int)chainRejectionFlags;
            telemetryEvent["WarningPromotionChainStatusFlags"] = 0;
            telemetryEvent["VerificationHasNonX509Rejection"] = hasNonX509Rejection;
            telemetryEvent["AcceptanceHasNonX509Rejection"] = hasNonX509Rejection;
            telemetryEvent["WarningPromotionHasNonX509Rejection"] = false;
            telemetryEvent.ComplexData["SignatureChains"] = chains;
            return (decisionRelevantFlags, chainRejectionFlags, hasNonX509Rejection);
        }

        private static TelemetryEvent CreateChainTelemetry(
            SignatureVerificationSummary summary, CertificateType certificateType,
            X509ChainStatusFlags flags, SignatureChainEvaluationStatus evaluationStatus,
            SignatureVerificationRejectionReasons rejectionReasons,
            X509ChainStatusFlags decisionRelevantFlags, X509ChainStatusFlags chainRejectionFlags)
        {
            return new TelemetryEvent("SignatureChain")
            {
                ["SignatureType"] = summary.SignatureType.ToString(),
                ["SignaturePlacement"] = summary.Placement.ToString(),
                ["CertificateType"] = certificateType.ToString(),
                ["ChainStatusFlags"] = (int)flags,
                ["EvaluationStatus"] = evaluationStatus.ToString(),
                ["RejectionReasons"] = (int)rejectionReasons,
                ["DecisionRelevantChainStatusFlags"] = (int)decisionRelevantFlags,
                ["RejectionChainStatusFlags"] = (int)chainRejectionFlags
            };
        }

        private static SignatureChainEvaluationStatus GetEvaluationStatus(List<SignatureChainEvaluationStatus> statuses)
        {
            if (statuses.Contains(SignatureChainEvaluationStatus.Indeterminate))
            {
                return SignatureChainEvaluationStatus.Indeterminate;
            }

            if (statuses.Contains(SignatureChainEvaluationStatus.NotEvaluated))
            {
                return statuses.Contains(SignatureChainEvaluationStatus.Evaluated)
                    ? SignatureChainEvaluationStatus.PartiallyEvaluated
                    : SignatureChainEvaluationStatus.NotEvaluated;
            }

            return statuses.Contains(SignatureChainEvaluationStatus.Evaluated)
                ? SignatureChainEvaluationStatus.Evaluated
                : statuses.Count == 0 ? SignatureChainEvaluationStatus.NotEvaluated : SignatureChainEvaluationStatus.NotApplicable;
        }
    }
}
