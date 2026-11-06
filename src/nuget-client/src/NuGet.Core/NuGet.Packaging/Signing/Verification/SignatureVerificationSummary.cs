// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    public sealed class SignatureVerificationSummary
    {
        /// <summary>
        /// Type of the signature that was verified
        /// </summary>
        public SignatureType SignatureType { get; }

        /// <summary>
        /// Status of the verification
        /// </summary>
        public SignatureVerificationStatus Status { get; }

        /// <summary>
        /// Reasons for the status.
        /// </summary>
        public SignatureVerificationStatusFlags Flags { get; }

        internal X509ChainStatusFlags SignerChainStatusFlags { get; set; }

        internal X509ChainStatusFlags TimestampChainStatusFlags { get; set; }

        // Observations on retained chains include permitted conditions; rejection masks contain only enforced conditions.
        internal X509ChainStatusFlags SignerDecisionRelevantChainStatusFlags { get; set; }

        internal X509ChainStatusFlags TimestampDecisionRelevantChainStatusFlags { get; set; }

        internal X509ChainStatusFlags SignerChainRejectionFlags { get; set; }

        internal X509ChainStatusFlags TimestampChainRejectionFlags { get; set; }

        internal bool SignerHasNonX509Rejection { get; set; }

        internal bool TimestampHasNonX509Rejection { get; set; }

        internal SignatureChainEvaluationStatus SignerChainEvaluationStatus { get; set; }

        internal SignatureChainEvaluationStatus TimestampChainEvaluationStatus { get; set; }

        internal SignaturePlacement Placement { get; set; }

        internal SignatureVerificationRejectionReasons SignerRejectionReasons { get; set; }

        internal SignatureVerificationRejectionReasons TimestampRejectionReasons { get; set; }

        internal SignatureVerificationRejectionReasons RejectionReasons => SignerRejectionReasons | TimestampRejectionReasons;

        /// <summary>
        /// Timestamp used to validate certificate.
        /// </summary>
        public Timestamp? Timestamp { get; }

        /// <summary>
        /// Expiration Date and Time for signature
        /// </summary>
        /// <remarks>This field will only be set if the certificate is expired.</remarks>
        public DateTimeOffset? ExpirationTime { get; }

        public IEnumerable<SignatureLog> Issues { get; set; }

        public SignatureVerificationSummary(
            SignatureType signatureType,
            SignatureVerificationStatus status,
            SignatureVerificationStatusFlags flags,
            Timestamp? timestamp,
            DateTimeOffset? expirationTime,
            IEnumerable<SignatureLog> issues)
        {
            SignatureType = signatureType;
            Status = status;
            Flags = flags;
            Timestamp = timestamp;
            ExpirationTime = expirationTime;
            Issues = issues;
        }

        public SignatureVerificationSummary(
            SignatureType signatureType,
            SignatureVerificationStatus status,
            SignatureVerificationStatusFlags flags,
            Timestamp? timestamp,
            IEnumerable<SignatureLog> issues)
            : this(signatureType, status, flags, timestamp, expirationTime: null, issues: issues)
        {
        }

        public SignatureVerificationSummary(
            SignatureType signatureType,
            SignatureVerificationStatus status,
            SignatureVerificationStatusFlags flags,
            IEnumerable<SignatureLog> issues)
            : this(signatureType, status, flags, timestamp: null, expirationTime: null, issues: issues)
        {
        }
    }
}
