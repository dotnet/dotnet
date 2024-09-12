// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

#if IS_SIGNING_SUPPORTED
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
#endif

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// A provider for RFC 3161 timestamps
    /// https://tools.ietf.org/html/rfc3161
    /// </summary>
    public class Rfc3161TimestampProvider : ITimestampProvider
    {
        // Url to an RFC 3161 timestamp server
        private readonly Uri _timestamperUrl;
#if IS_SIGNING_SUPPORTED
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10);
#endif
        public Rfc3161TimestampProvider(Uri timeStampServerUrl)
        {
#if IS_SIGNING_SUPPORTED
            // Uri.UriSchemeHttp and Uri.UriSchemeHttps are not available in netstandard 1.3
            if (!string.Equals(timeStampServerUrl.Scheme, Uri.UriSchemeHttp, StringComparison.Ordinal) &&
                !string.Equals(timeStampServerUrl.Scheme, Uri.UriSchemeHttps, StringComparison.Ordinal))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.TimestampFailureInvalidHttpScheme,
                    timeStampServerUrl,
                    nameof(Uri.UriSchemeHttp),
                    nameof(Uri.UriSchemeHttps)));
            }
#endif
            _timestamperUrl = timeStampServerUrl ?? throw new ArgumentNullException(nameof(timeStampServerUrl));
        }

#if IS_SIGNING_SUPPORTED

        /// <summary>
        /// Timestamps data present in the TimestampRequest.
        /// </summary>
        public async Task<PrimarySignature> TimestampSignatureAsync(PrimarySignature primarySignature, TimestampRequest request, ILogger logger, CancellationToken token)
        {
            SignedCms timestampCms = await GetTimestampAsync(request, logger, token);
            using (ICms signatureCms = CmsFactory.Create(primarySignature.GetBytes()))
            {
                if (request.Target == SignaturePlacement.Countersignature)
                {
                    signatureCms.AddTimestampToRepositoryCountersignature(timestampCms);
                }
                else
                {
                    signatureCms.AddTimestamp(timestampCms);
                }
                return PrimarySignature.Load(signatureCms.Encode());
            }
        }

        /// <summary>
        /// Timestamps data present in the TimestampRequest.
        /// </summary>
        internal async Task<SignedCms> GetTimestampAsync(TimestampRequest request, ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            // Allows us to track the request.
            var nonce = GenerateNonce();
            var rfc3161TimestampRequest = Rfc3161TimestampRequestFactory.Create(
                request.HashedMessage,
                request.HashAlgorithm.ConvertToSystemSecurityHashAlgorithmName(),
                requestedPolicyId: null,
                nonce: nonce,
                requestSignerCertificates: true,
                extensions: null);

            // Request a timestamp
            // The response status need not be checked here as lower level api will throw if the response is invalid
            IRfc3161TimestampToken timestampToken = await rfc3161TimestampRequest.SubmitRequestAsync(
                _timestamperUrl,
                RequestTimeout);

            // quick check for response validity
            var normalizedNonce = rfc3161TimestampRequest.GetNonce();
            ValidateTimestampResponse(normalizedNonce, request.HashedMessage, timestampToken);

            var timestampCms = timestampToken.AsSignedCms();
            ValidateTimestampCms(request.SigningSpecifications, timestampCms, timestampToken);

            // If the timestamp signed CMS already has a complete chain for the signing certificate,
            // it's ready to be added to the signature to be timestamped.
            // However, a timestamp service is not required to include all certificates in a complete
            // chain for the signing certificate in the SignedData.certificates collection.
            // Some timestamp services include all certificates except the root in the
            // SignedData.certificates collection.
            var signerInfo = timestampCms.SignerInfos[0];

            using (var chain = CertificateChainUtility.GetCertificateChain(
                signerInfo.Certificate,
                timestampCms.Certificates,
                logger,
                CertificateType.Timestamp))
            {
                return EnsureCertificatesInCertificatesCollection(timestampCms, chain);
            }
        }

        private static SignedCms EnsureCertificatesInCertificatesCollection(
            SignedCms timestamp,
            IReadOnlyList<X509Certificate2> chain)
        {
            using (ICms timestampCms = CmsFactory.Create(timestamp.Encode()))
            {
                timestampCms.AddCertificates(
                    chain.Where(certificate => !timestamp.Certificates.Contains(certificate)));

                var bytes = timestampCms.Encode();
                var updatedCms = new SignedCms();

                updatedCms.Decode(bytes);

                return updatedCms;
            }
        }

        private static void ValidateTimestampCms(SigningSpecifications spec, SignedCms timestampCms, IRfc3161TimestampToken timestampToken)
        {
            var signerInfo = timestampCms.SignerInfos[0];
            try
            {
                signerInfo.CheckSignature(verifySignatureOnly: true);
            }
            catch (Exception e)
            {
                throw new TimestampException(NuGetLogCode.NU3021, Strings.SignError_TimestampSignatureValidationFailed, e);
            }

            if (signerInfo.Certificate == null)
            {
                throw new TimestampException(NuGetLogCode.NU3020, Strings.SignError_TimestampNoCertificate);
            }

            if (!CertificateUtility.IsSignatureAlgorithmSupported(signerInfo.Certificate))
            {
                var certificateSignatureAlgorithm = GetNameOrOidString(signerInfo.Certificate.SignatureAlgorithm);

                var supportedSignatureAlgorithms = string.Join(", ", spec.AllowedSignatureAlgorithms);

                var errorMessage = string.Format(CultureInfo.CurrentCulture,
                    Strings.TimestampCertificateUnsupportedSignatureAlgorithm,
                    certificateSignatureAlgorithm,
                    supportedSignatureAlgorithms);

                throw new TimestampException(NuGetLogCode.NU3022, errorMessage);
            }

            if (!CertificateUtility.IsCertificatePublicKeyValid(signerInfo.Certificate))
            {
                throw new TimestampException(NuGetLogCode.NU3023, Strings.SignError_TimestampCertificateFailsPublicKeyLengthRequirement);
            }

            if (!spec.AllowedHashAlgorithmOids.Contains(signerInfo.DigestAlgorithm.Value))
            {
                var digestAlgorithm = GetNameOrOidString(signerInfo.DigestAlgorithm);

                var supportedSignatureAlgorithms = string.Join(", ", spec.AllowedHashAlgorithms);

                var errorMessage = string.Format(CultureInfo.CurrentCulture,
                    Strings.TimestampSignatureUnsupportedDigestAlgorithm,
                    digestAlgorithm,
                    supportedSignatureAlgorithms);

                throw new TimestampException(NuGetLogCode.NU3024, errorMessage);
            }

            if (CertificateUtility.IsCertificateValidityPeriodInTheFuture(signerInfo.Certificate))
            {
                throw new TimestampException(NuGetLogCode.NU3025, Strings.SignError_TimestampNotYetValid);
            }

            if (!CertificateUtility.IsDateInsideValidityPeriod(signerInfo.Certificate, timestampToken.TokenInfo.Timestamp))
            {
                throw new TimestampException(NuGetLogCode.NU3036, Strings.SignError_TimestampGeneralizedTimeInvalid);
            }
        }

        private static void ValidateTimestampResponse(byte[] nonce, byte[] messageHash, IRfc3161TimestampToken timestampToken)
        {
            var tokenNonce = timestampToken.TokenInfo.GetNonce();
            if (tokenNonce == null || !nonce.SequenceEqual(tokenNonce))
            {
                throw new TimestampException(NuGetLogCode.NU3026, Strings.TimestampFailureNonceMismatch);
            }

            if (!timestampToken.TokenInfo.HasMessageHash(messageHash))
            {
                throw new TimestampException(NuGetLogCode.NU3019, Strings.SignError_TimestampIntegrityCheckFailed);
            }
        }

        /// <summary>
        /// Returns the FriendlyName of an Oid. If FriendlyName is null, then the Oid string is returned.
        /// </summary>
        private static string GetNameOrOidString(Oid oid)
        {
            return oid.FriendlyName?.ToUpper(CultureInfo.InvariantCulture) ?? oid.Value;
        }

        private static byte[] GenerateNonce()
        {
            var nonce = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(nonce);
            }

            EnsureValidNonce(nonce);

            return nonce;
        }

        /// <summary>
        /// Non-private for testing purposes only.
        /// </summary>
        internal static void EnsureValidNonce(byte[] nonce)
        {
#if IS_DESKTOP
            // Eventually, CryptEncodeObjectEx(...) is called on a CRYPT_TIMESTAMP_REQUEST with this nonce,
            // and CryptEncodeObjectEx(...) interprets the nonce as a little endian, DER-encoded integer value
            // (without tag and length), and may even strip leading bytes from the big endian representation
            // of the byte sequence to achieve proper integer DER encoding.
            //
            // If the nonce is changed after the client generates it, the timestamp server would receive
            // and return a nonce that does not agree with the client's original nonce.
            //
            // To ensure this does not happen, ensure that the most significant byte in the little
            // endian byte sequence is in the 0x01-0x7F range; clear that byte's most significant bit
            // and set that byte's least significant bit.

            nonce[nonce.Length - 1] &= 0x7f;
            nonce[nonce.Length - 1] |= 0x01;
#else
            // Per documentation on Rfc3161TimestampRequest.CreateFromHash(...) the nonce "value is interpreted
            // as an unsigned big-endian integer and may be normalized to the encoding format."  Clear the sign bit on
            // the most significant byte to ensure the nonce represents an unsigned big endian integer.
            nonce[0] &= 0x7f;
#endif
        }

#else

        /// <summary>
        /// Timestamp a signature.
        /// </summary>
        public Task<PrimarySignature> TimestampSignatureAsync(PrimarySignature primarySignature, TimestampRequest timestampRequest, ILogger logger, CancellationToken token)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
