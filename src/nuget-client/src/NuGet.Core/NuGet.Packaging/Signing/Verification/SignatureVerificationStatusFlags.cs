// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Signing
{
    [Flags]
    public enum SignatureVerificationStatusFlags
    {
        /// <summary>
        /// There was no error found
        /// </summary>
        NoErrors = 0,

        /// <summary>
        /// A signature was not found
        /// </summary>
        NoSignature = 1 << 0,

        /// <summary>
        /// Signer certificate was not found
        /// </summary>
        NoCertificate = 1 << 1,

        /// <summary>
        /// Multiple signatures were found
        /// </summary>
        MultipleSignatures = 1 << 2,

        /// <summary>
        /// A call to SignedCms.CheckSignature failed
        /// </summary>
        SignatureCheckFailed = 1 << 3,

        /// <summary>
        /// Signature algorithm is not supported
        /// </summary>
        SignatureAlgorithmUnsupported = 1 << 4,

        /// <summary>
        /// Public key does not conform with the requirements of the spec
        /// </summary>
        CertificatePublicKeyInvalid = 1 << 5,

        /// <summary>
        /// Signing certificate has lifetimeSigningEku
        /// </summary>
        HasLifetimeSigningEku = 1 << 6,

        /// <summary>
        /// Signer certificate's validity is in the future
        /// </summary>
        CertificateValidityInTheFuture = 1 << 7,

        /// <summary>
        /// Signing certificate has expired
        /// </summary>
        CertificateExpired = 1 << 8,

        /// <summary>
        /// Hashing algorithm is not supported
        /// </summary>
        HashAlgorithmUnsupported = 1 << 9,

        /// <summary>
        /// Message imprint uses a hash algorithm that is not supported
        /// </summary>
        MessageImprintUnsupportedAlgorithm = 1 << 10,

        /// <summary>
        /// Integrity check of the signature failed
        /// </summary>
        IntegrityCheckFailed = 1 << 11,

        /// <summary>
        /// Chain building failures.
        /// Some specific chain building failures (like revocation, revocation status unavailable, certificate expired, etc.)
        /// are not covered by this flag because they are covered specially by another status flag.
        /// </summary>
        ChainBuildingFailure = 1 << 12,

        /// <summary>
        /// Revocation information was unavailable or was offline for the signer certificate
        /// </summary>
        UnknownRevocation = 1 << 13,

        /// <summary>
        /// Signing certificate was revoked
        /// </summary>
        CertificateRevoked = 1 << 14,

        /// <summary>
        /// Signing certificate chains to a certificate untrusted by the computer performing the verification
        /// </summary>
        UntrustedRoot = 1 << 15,

        /// <summary>
        /// The Timestamp's generalized time was outside certificate's validity period
        /// </summary>
        GeneralizedTimeOutsideValidity = 1 << 16,

        /// <summary>
        /// A valid timestamp was not found.
        /// </summary>
        NoValidTimestamp = 1 << 17,

        /// <summary>
        /// Multiple timestamps were found.
        /// </summary>
        MultipleTimestamps = 1 << 18,

        /// <summary>
        /// Unknown build status.
        /// </summary>
        UnknownBuildStatus = 1 << 19,

        /// <summary>
        /// Flags which indicate that the signed package is suspect.
        /// </summary>
        Suspect = IntegrityCheckFailed |
            CertificateRevoked,

        /// <summary>
        /// Flags which indicate that the signed package is illegal.
        /// </summary>
        Illegal = NoCertificate |
            MultipleSignatures |
            SignatureCheckFailed |
            SignatureAlgorithmUnsupported |
            CertificatePublicKeyInvalid |
            HasLifetimeSigningEku |
            CertificateValidityInTheFuture |
            HashAlgorithmUnsupported |
            MessageImprintUnsupportedAlgorithm,

        /// <summary>
        /// Flags which indicate that the signed package is untrusted.
        /// </summary>
        Untrusted = NoSignature |
            CertificateExpired |
            ChainBuildingFailure |
            UnknownRevocation |
            UntrustedRoot |
            GeneralizedTimeOutsideValidity |
            UnknownBuildStatus
    }
}
