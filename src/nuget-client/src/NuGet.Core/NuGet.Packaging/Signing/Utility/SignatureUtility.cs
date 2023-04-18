// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    public static class SignatureUtility
    {
        private const int SHA1HashLength = 20;

        private enum SigningCertificateRequirement
        {
            NoRequirement,
            OnlyV2,
            EitherOrBoth
        }

        /// <summary>
        /// Gets certificates in the certificate chain for the primary signature.
        /// </summary>
        /// <param name="primarySignature">The primary signature.</param>
        /// <returns>A non-empty, read-only list of X.509 certificates ordered from signing certificate to root.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="primarySignature" /> is <c>null</c>.</exception>
        /// <remarks>
        /// WARNING:  This method does not perform revocation, trust, or certificate validity checking.
        /// </remarks>
        public static IX509CertificateChain GetCertificateChain(PrimarySignature primarySignature)
        {
            if (primarySignature == null)
            {
                throw new ArgumentNullException(nameof(primarySignature));
            }

            return GetPrimarySignatureCertificates(
                primarySignature.SignedCms,
                primarySignature.SignerInfo,
                SigningSpecifications.V1,
                primarySignature.FriendlyName,
                includeChain: true);
        }

        /// <summary>
        /// Gets certificates in the certificate chain for the repository countersignature.
        /// </summary>
        /// <param name="primarySignature">The primary signature.</param>
        /// <param name="repositoryCountersignature">The repository countersignature.</param>
        /// <returns>A non-empty, read-only list of X.509 certificates ordered from signing certificate to root.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="primarySignature" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="repositoryCountersignature" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="repositoryCountersignature" /> is
        /// unrelated to <paramref name="primarySignature" />.</exception>
        /// <remarks>
        /// WARNING:  This method does not perform revocation, trust, or certificate validity checking.
        /// </remarks>
        public static IX509CertificateChain GetCertificateChain(
            PrimarySignature primarySignature,
            RepositoryCountersignature repositoryCountersignature)
        {
            if (primarySignature == null)
            {
                throw new ArgumentNullException(nameof(primarySignature));
            }

            if (repositoryCountersignature == null)
            {
                throw new ArgumentNullException(nameof(repositoryCountersignature));
            }

            if (!repositoryCountersignature.IsRelated(primarySignature))
            {
                throw new ArgumentException(Strings.UnrelatedSignatures, nameof(repositoryCountersignature));
            }

            return GetRepositoryCountersignatureCertificates(
                primarySignature.SignedCms,
                repositoryCountersignature.SignerInfo,
                SigningSpecifications.V1,
                includeChain: true);
        }

        internal static IX509CertificateChain GetCertificateChain(
            SignedCms signedCms,
            SignerInfo signerInfo,
            SigningSpecifications signingSpecifications,
            string signatureFriendlyName)
        {
            return GetPrimarySignatureCertificates(signedCms, signerInfo, signingSpecifications, signatureFriendlyName, includeChain: false);
        }

        private static IX509CertificateChain GetPrimarySignatureCertificates(
            SignedCms signedCms,
            SignerInfo signerInfo,
            SigningSpecifications signingSpecifications,
            string signatureFriendlyName,
            bool includeChain)
        {
            if (signedCms == null)
            {
                throw new ArgumentNullException(nameof(signedCms));
            }

            if (signerInfo == null)
            {
                throw new ArgumentNullException(nameof(signerInfo));
            }

            var errors = new Errors(
                noCertificate: NuGetLogCode.NU3010,
                noCertificateString: string.Format(CultureInfo.CurrentCulture, Strings.Verify_ErrorNoCertificate, signatureFriendlyName),
                invalidSignature: NuGetLogCode.NU3011,
                invalidSignatureString: Strings.InvalidPrimarySignature,
                chainBuildingFailed: NuGetLogCode.NU3018);

            var signatureType = AttributeUtility.GetSignatureType(signerInfo.SignedAttributes);
            SigningCertificateRequirement signingCertificateRequirement;
            bool isIssuerSerialRequired;

            switch (signatureType)
            {
                case SignatureType.Author:
                case SignatureType.Repository:
                    signingCertificateRequirement = SigningCertificateRequirement.OnlyV2;
                    isIssuerSerialRequired = true;
                    break;
                default:
                    signingCertificateRequirement = SigningCertificateRequirement.NoRequirement;
                    isIssuerSerialRequired = false;
                    break;
            }

            return GetCertificates(
                signedCms,
                signerInfo,
                signingCertificateRequirement,
                isIssuerSerialRequired,
                errors,
                signingSpecifications,
                CertificateType.Signature,
                includeChain);
        }

        /// <summary>
        /// Gets certificates in the certificate chain for a timestamp on the primary signature.
        /// </summary>
        /// <param name="primarySignature">The primary signature.</param>
        /// <returns>A non-empty, read-only list of X.509 certificates ordered from signing certificate to root.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="primarySignature" /> is <c>null</c>.</exception>
        /// <exception cref="SignatureException">Thrown if <paramref name="primarySignature" /> does not have a valid
        /// timestamp.</exception>
        /// <remarks>
        /// WARNING:  This method does not perform revocation, trust, or certificate validity checking.
        /// </remarks>
        public static IX509CertificateChain GetTimestampCertificateChain(
            PrimarySignature primarySignature)
        {
            if (primarySignature == null)
            {
                throw new ArgumentNullException(nameof(primarySignature));
            }

            var timestamp = primarySignature.Timestamps.FirstOrDefault();

            if (timestamp == null)
            {
                throw new SignatureException(NuGetLogCode.NU3000, Strings.PrimarySignatureHasNoTimestamp);
            }

            return GetTimestampCertificates(
                timestamp.SignedCms,
                SigningSpecifications.V1,
                primarySignature.FriendlyName,
                includeChain: true);
        }

        /// <summary>
        /// Gets certificates in the certificate chain for a timestamp on the repository countersignature.
        /// </summary>
        /// <param name="primarySignature">The primary signature.</param>
        /// <param name="repositoryCountersignature">The repository countersignature.</param>
        /// <returns>A non-empty, read-only list of X.509 certificates ordered from signing certificate to root.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="primarySignature" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="repositoryCountersignature" /> is <c>null</c>.</exception>
        /// <exception cref="SignatureException">Thrown if <paramref name="repositoryCountersignature" /> does not have a valid
        /// timestamp.</exception>
        /// <remarks>
        /// WARNING:  This method does not perform revocation, trust, or certificate validity checking.
        /// </remarks>
        public static IX509CertificateChain GetTimestampCertificateChain(
            PrimarySignature primarySignature,
            RepositoryCountersignature repositoryCountersignature)
        {
            if (primarySignature == null)
            {
                throw new ArgumentNullException(nameof(primarySignature));
            }

            if (repositoryCountersignature == null)
            {
                throw new ArgumentNullException(nameof(repositoryCountersignature));
            }

            if (!repositoryCountersignature.IsRelated(primarySignature))
            {
                throw new ArgumentException(Strings.UnrelatedSignatures, nameof(repositoryCountersignature));
            }

            var timestamp = repositoryCountersignature.Timestamps.FirstOrDefault();

            if (timestamp == null)
            {
                throw new SignatureException(NuGetLogCode.NU3000, Strings.RepositoryCountersignatureHasNoTimestamp);
            }

            return GetTimestampCertificates(
                timestamp.SignedCms,
                SigningSpecifications.V1,
                primarySignature.FriendlyName,
                includeChain: true);
        }

        private static IX509CertificateChain GetRepositoryCountersignatureCertificates(
            SignedCms signedCms,
            SignerInfo signerInfo,
            SigningSpecifications signingSpecifications,
            bool includeChain)
        {
            if (signedCms == null)
            {
                throw new ArgumentNullException(nameof(signedCms));
            }

            if (signerInfo == null)
            {
                throw new ArgumentNullException(nameof(signerInfo));
            }

            var errors = new Errors(
                noCertificate: NuGetLogCode.NU3034,
                noCertificateString: Strings.RepositoryCountersignatureHasNoCertificate,
                invalidSignature: NuGetLogCode.NU3031,
                invalidSignatureString: Strings.InvalidRepositoryCountersignature,
                chainBuildingFailed: NuGetLogCode.NU3035);

            var isIssuerSerialRequired = true;

            return GetCertificates(
                signedCms,
                signerInfo,
                SigningCertificateRequirement.OnlyV2,
                isIssuerSerialRequired,
                errors,
                signingSpecifications,
                CertificateType.Signature,
                includeChain);
        }

        public static bool HasRepositoryCountersignature(PrimarySignature primarySignature)
        {
            if (primarySignature == null)
            {
                throw new ArgumentNullException(nameof(primarySignature));
            }

            if (primarySignature is RepositoryPrimarySignature)
            {
                return false;
            }

            var counterSignatures = primarySignature.SignerInfo.CounterSignerInfos;

            foreach (var counterSignature in counterSignatures)
            {
                var countersignatureType = AttributeUtility.GetSignatureType(counterSignature.SignedAttributes);
                if (countersignatureType == SignatureType.Repository)
                {
                    return true;
                }
            }

            return false;
        }

        internal static void LogAdditionalContext(IX509Chain chain, List<SignatureLog> issues)
        {
            if (chain is null)
            {
                throw new ArgumentNullException(nameof(chain));
            }

            if (issues is null)
            {
                throw new ArgumentNullException(nameof(issues));
            }

            ILogMessage logMessage = chain.AdditionalContext;

            if (logMessage is not null)
            {
                SignatureLog issue = SignatureLog.Issue(
                    fatal: false,
                    logMessage.Code,
                    logMessage.Message);

                issues.Add(issue);
            }
        }

        internal static IX509CertificateChain GetTimestampCertificates(
            SignedCms signedCms,
            SigningSpecifications signingSpecifications,
            string signatureFriendlyName)
        {
            return GetTimestampCertificates(
              signedCms,
              signingSpecifications,
              signatureFriendlyName,
              includeChain: false);
        }

        private static IX509CertificateChain GetTimestampCertificates(
            SignedCms signedCms,
            SigningSpecifications signingSpecifications,
            string signatureFriendlyName,
            bool includeChain)
        {
            var errors = new Errors(
                noCertificate: NuGetLogCode.NU3020,
                noCertificateString: string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampNoCertificate, signatureFriendlyName),
                invalidSignature: NuGetLogCode.NU3021,
                invalidSignatureString: string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampInvalid, signatureFriendlyName),
                chainBuildingFailed: NuGetLogCode.NU3028);

            if (signedCms.SignerInfos.Count != 1)
            {
                throw new SignatureException(errors.InvalidSignature, errors.InvalidSignatureString);
            }

            const bool isIssuerSerialRequired = false;

            return GetCertificates(
                signedCms,
                signedCms.SignerInfos[0],
                SigningCertificateRequirement.EitherOrBoth,
                isIssuerSerialRequired,
                errors,
                signingSpecifications,
                CertificateType.Timestamp,
                includeChain);
        }

        private static IX509CertificateChain GetCertificates(
            SignedCms signedCms,
            SignerInfo signerInfo,
            SigningCertificateRequirement signingCertificateRequirement,
            bool isIssuerSerialRequired,
            Errors errors,
            SigningSpecifications signingSpecifications,
            CertificateType certificateType,
            bool includeChain)
        {
            if (signedCms == null)
            {
                throw new ArgumentNullException(nameof(signedCms));
            }

            if (signingSpecifications == null)
            {
                throw new ArgumentNullException(nameof(signingSpecifications));
            }

            if (signerInfo.Certificate == null)
            {
                throw new SignatureException(errors.NoCertificate, errors.NoCertificateString);
            }

            /*
                The signing-certificate and signing-certificate-v2 attributes are described in RFC 2634 and RFC 5035.

                Timestamps
                --------------------------------------------------
                RFC 3161 requires the signing-certificate attribute.  The RFC 5816 update introduces the newer
                signing-certificate-v2 attribute, which may replace or, for backwards compatibility, be present
                alongside the older signing-certificate attribute.

                The issuerSerial field is not required, but should be validated if present.


                Author and repository signatures
                --------------------------------------------------
                RFC 5126 (CAdES) requires that exactly one of either of these attributes be present, and also requires that
                the issuerSerial field be present.


                Validation
                --------------------------------------------------
                For author and repository signatures:

                    * the signing-certificate attribute must not be present
                    * the signing-certificate-v2 attribute be present
                    * the issuerSerial field must be present


                References:

                    "Signing Certificate Attribute Definition", RFC 2634 section 5.4 (https://tools.ietf.org/html/rfc2634#section-5.4)
                    "Certificate Identification", RFC 2634 section 5.4 (https://tools.ietf.org/html/rfc2634#section-5.4.1)
                    "Enhanced Security Services (ESS) Update: Adding CertID Algorithm Agility", RFC 5035 (https://tools.ietf.org/html/rfc5035)
                    "Request Format", RFC 3161 section 2.4.1 (https://tools.ietf.org/html/rfc3161#section-2.4.1)
                    "Signature of Time-Stamp Token", RFC 5816 section 2.2.1 (https://tools.ietf.org/html/rfc5816#section-2.2.1)
                    "Signing Certificate Reference Attributes", RFC 5126 section 5.7.3 (https://tools.ietf.org/html/rfc5126.html#section-5.7.3)
                    "ESS signing-certificate Attribute Definition", RFC 5126 section 5.7.3.1 (https://tools.ietf.org/html/rfc5126.html#section-5.7.3.1)
                    "ESS signing-certificate-v2 Attribute Definition", RFC 5126 section 5.7.3.2 (https://tools.ietf.org/html/rfc5126.html#section-5.7.3.2)
            */

            const string signingCertificateName = "signing-certificate";
            const string signingCertificateV2Name = "signing-certificate-v2";

            CryptographicAttributeObject signingCertificateAttribute = null;
            CryptographicAttributeObject signingCertificateV2Attribute = null;

            foreach (var attribute in signerInfo.SignedAttributes)
            {
                switch (attribute.Oid.Value)
                {
                    case Oids.SigningCertificate:
                        if (signingCertificateAttribute != null)
                        {
                            throw new SignatureException(
                                errors.InvalidSignature,
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.MultipleAttributesDisallowed,
                                    signingCertificateName));
                        }

                        if (attribute.Values.Count != 1)
                        {
                            throw new SignatureException(
                                errors.InvalidSignature,
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.ExactlyOneAttributeValueRequired,
                                    signingCertificateName));
                        }

                        signingCertificateAttribute = attribute;
                        break;

                    case Oids.SigningCertificateV2:
                        if (signingCertificateV2Attribute != null)
                        {
                            throw new SignatureException(
                                errors.InvalidSignature,
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.MultipleAttributesDisallowed,
                                    signingCertificateV2Name));
                        }

                        if (attribute.Values.Count != 1)
                        {
                            throw new SignatureException(
                                errors.InvalidSignature,
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Strings.ExactlyOneAttributeValueRequired,
                                    signingCertificateV2Name));
                        }

                        signingCertificateV2Attribute = attribute;
                        break;
                }
            }

            switch (signingCertificateRequirement)
            {
                case SigningCertificateRequirement.OnlyV2:
                    {
                        if (signingCertificateAttribute != null)
                        {
                            throw new SignatureException(errors.InvalidSignature, Strings.SigningCertificateAttributeMustNotBePresent);
                        }

                        if (signingCertificateV2Attribute == null)
                        {
                            throw new SignatureException(
                                errors.InvalidSignature,
                                string.Format(CultureInfo.CurrentCulture,
                                    Strings.ExactlyOneAttributeRequired,
                                    signingCertificateV2Name));
                        }
                    }
                    break;

                case SigningCertificateRequirement.EitherOrBoth:
                    if (signingCertificateAttribute == null && signingCertificateV2Attribute == null)
                    {
                        throw new SignatureException(errors.InvalidSignature, Strings.SigningCertificateV1OrV2AttributeMustBePresent);
                    }
                    break;
            }

            if (signingCertificateV2Attribute != null)
            {
                var reader = CreateDerSequenceReader(signingCertificateV2Attribute);
                var signingCertificateV2 = SigningCertificateV2.Read(reader);

                if (signingCertificateV2.Certificates.Count == 0)
                {
                    throw new SignatureException(errors.InvalidSignature, Strings.SigningCertificateV2Invalid);
                }

                foreach (var essCertIdV2 in signingCertificateV2.Certificates)
                {
                    if (!signingSpecifications.AllowedHashAlgorithmOids.Contains(
                        essCertIdV2.HashAlgorithm.Algorithm.Value,
                        StringComparer.Ordinal))
                    {
                        throw new SignatureException(errors.InvalidSignature, Strings.SigningCertificateV2UnsupportedHashAlgorithm);
                    }
                }

                if (!IsMatch(signerInfo.Certificate, signingCertificateV2.Certificates[0], errors, isIssuerSerialRequired))
                {
                    throw new SignatureException(errors.InvalidSignature, Strings.SigningCertificateV2CertificateNotFound);
                }
            }

            if (signingCertificateAttribute != null)
            {
                var reader = CreateDerSequenceReader(signingCertificateAttribute);
                var signingCertificate = SigningCertificate.Read(reader);

                if (signingCertificate.Certificates.Count == 0)
                {
                    throw new SignatureException(errors.InvalidSignature, Strings.SigningCertificateInvalid);
                }

                if (!IsMatch(signerInfo.Certificate, signingCertificate.Certificates[0]))
                {
                    throw new SignatureException(errors.InvalidSignature, Strings.SigningCertificateCertificateNotFound);
                }
            }

            IX509CertificateChain certificates = GetCertificateChain(
                signerInfo.Certificate,
                signedCms.Certificates,
                certificateType,
                includeChain);

            if (certificates == null || certificates.Count == 0)
            {
                throw new SignatureException(errors.ChainBuildingFailed, Strings.CertificateChainBuildFailed);
            }

            return certificates;
        }

        private static bool IsMatch(
            X509Certificate2 certificate,
            EssCertIdV2 essCertIdV2,
            Errors errors,
            bool isIssuerSerialRequired)
        {
            if (isIssuerSerialRequired)
            {
                if (essCertIdV2.IssuerSerial == null ||
                    essCertIdV2.IssuerSerial.GeneralNames.Count == 0)
                {
                    throw new SignatureException(errors.InvalidSignature, errors.InvalidSignatureString);
                }
            }

            if (essCertIdV2.IssuerSerial != null)
            {
                if (!AreSerialNumbersEqual(essCertIdV2.IssuerSerial, certificate))
                {
                    return false;
                }

                if (!AreGeneralNamesEqual(essCertIdV2.IssuerSerial, certificate))
                {
                    return false;
                }
            }

            var hashAlgorithmName = CryptoHashUtility.OidToHashAlgorithmName(essCertIdV2.HashAlgorithm.Algorithm.Value);
            var actualHash = CertificateUtility.GetHash(certificate, hashAlgorithmName);

            return essCertIdV2.CertificateHash.SequenceEqual(actualHash);
        }

        private static bool IsMatch(X509Certificate2 certificate, EssCertId essCertId)
        {
            if (essCertId.IssuerSerial != null)
            {
                if (!AreSerialNumbersEqual(essCertId.IssuerSerial, certificate))
                {
                    return false;
                }

                if (!AreGeneralNamesEqual(essCertId.IssuerSerial, certificate))
                {
                    return false;
                }
            }

            return essCertId.CertificateHash.Length == SHA1HashLength;
        }

        private static bool AreGeneralNamesEqual(IssuerSerial issuerSerial, X509Certificate2 certificate)
        {
            var generalName = issuerSerial.GeneralNames.FirstOrDefault();

            if (generalName != null &&
                generalName.DirectoryName != null)
            {
                return string.Equals(generalName.DirectoryName.Name, certificate.IssuerName.Name, StringComparison.Ordinal);
            }

            return true;
        }

        private static bool AreSerialNumbersEqual(IssuerSerial issuerSerial, X509Certificate2 certificate)
        {
            var certificateSerialNumber = certificate.GetSerialNumber();

            // Convert from little endian to big endian.
            Array.Reverse(certificateSerialNumber);

            return issuerSerial.SerialNumber.SequenceEqual(certificateSerialNumber);
        }

        private static IX509CertificateChain GetCertificateChain(
            X509Certificate2 certificate,
            X509Certificate2Collection extraStore,
            CertificateType certificateType,
            bool includeCertificatesAfterSigningCertificate)
        {
            if (!includeCertificatesAfterSigningCertificate)
            {
                return new X509CertificateChain() { certificate };
            }

            using (X509ChainHolder chainHolder = certificateType == CertificateType.Signature
                ? X509ChainHolder.CreateForCodeSigning() : X509ChainHolder.CreateForTimestamping())
            {
                IX509Chain chain = chainHolder.Chain2;

                chain.ChainPolicy.ExtraStore.AddRange(extraStore);

                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

                CertificateChainUtility.BuildWithPolicy(chain, certificate);

                if (chain.ChainStatus.Any(chainStatus =>
                    chainStatus.Status.HasFlag(X509ChainStatusFlags.Cyclic) ||
                    chainStatus.Status.HasFlag(X509ChainStatusFlags.PartialChain) ||
                    chainStatus.Status.HasFlag(X509ChainStatusFlags.NotSignatureValid)))
                {
                    return null;
                }

                return CertificateChainUtility.GetCertificateChain(chain.PrivateReference);
            }
        }

        private static DerSequenceReader CreateDerSequenceReader(CryptographicAttributeObject attribute)
        {
            if (attribute.Values.Count != 1)
            {
                throw new SignatureException(string.Format(CultureInfo.CurrentCulture, Strings.SignatureContainsInvalidAttribute, attribute.Oid.Value));
            }

            return new DerSequenceReader(attribute.Values[0].RawData);
        }

        private sealed class Errors
        {
            internal NuGetLogCode NoCertificate { get; }
            internal string NoCertificateString { get; }
            internal NuGetLogCode InvalidSignature { get; }
            internal string InvalidSignatureString { get; }
            internal NuGetLogCode ChainBuildingFailed { get; }

            internal Errors(
                NuGetLogCode noCertificate,
                string noCertificateString,
                NuGetLogCode invalidSignature,
                string invalidSignatureString,
                NuGetLogCode chainBuildingFailed)
            {
                NoCertificate = noCertificate;
                NoCertificateString = noCertificateString;
                InvalidSignature = invalidSignature;
                InvalidSignatureString = invalidSignatureString;
                ChainBuildingFailed = chainBuildingFailed;
            }
        }
    }
}
#endif
