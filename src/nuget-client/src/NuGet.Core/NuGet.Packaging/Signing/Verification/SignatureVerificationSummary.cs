// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Timestamp used to validate certificate.
        /// </summary>
        public Timestamp Timestamp { get; }

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
            Timestamp timestamp,
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
            Timestamp timestamp,
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
