// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public static class VerificationUtility
    {
        public static SignatureVerificationStatus GetSignatureVerificationStatus(SignatureVerificationStatusFlags flags)
        {
            if (flags == SignatureVerificationStatusFlags.NoErrors)
            {
                return SignatureVerificationStatus.Valid;
            }

            if ((flags & SignatureVerificationStatusFlags.Suspect) != 0)
            {
                return SignatureVerificationStatus.Suspect;
            }

            // If the only flags are these known ones, return disallowed.
            if ((flags & ~(SignatureVerificationStatusFlags.Illegal |
                SignatureVerificationStatusFlags.Untrusted |
                SignatureVerificationStatusFlags.NoValidTimestamp |
                SignatureVerificationStatusFlags.MultipleTimestamps)) == 0)
            {
                return SignatureVerificationStatus.Disallowed;
            }

            return SignatureVerificationStatus.Unknown;
        }

        public static bool IsVerificationTarget(SignatureType signatureType, VerificationTarget target)
        {
            switch (signatureType)
            {
                case SignatureType.Unknown:
                    return target.HasFlag(VerificationTarget.Unknown);

                case SignatureType.Author:
                    return target.HasFlag(VerificationTarget.Author);

                case SignatureType.Repository:
                    return target.HasFlag(VerificationTarget.Repository);

                default:
                    throw new NotImplementedException();
            }
        }

        internal static SignatureVerificationStatusFlags ValidateSigningCertificate(X509Certificate2 certificate, bool treatIssuesAsErrors, string signatureFriendlyName, List<SignatureLog> issues)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (issues == null)
            {
                throw new ArgumentNullException(nameof(issues));
            }

            var validationFlags = SignatureVerificationStatusFlags.NoErrors;

            if (!CertificateUtility.IsSignatureAlgorithmSupported(certificate))
            {
                issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3013, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_CertificateHasUnsupportedSignatureAlgorithm, signatureFriendlyName)));
                validationFlags |= SignatureVerificationStatusFlags.SignatureAlgorithmUnsupported;
            }

            if (!CertificateUtility.IsCertificatePublicKeyValid(certificate))
            {
                issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3014, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_CertificateFailsPublicKeyLengthRequirement, signatureFriendlyName)));
                validationFlags |= SignatureVerificationStatusFlags.CertificatePublicKeyInvalid;
            }

            if (CertificateUtility.HasExtendedKeyUsage(certificate, Oids.LifetimeSigningEku))
            {
                issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3015, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_CertificateHasLifetimeSigningEKU, signatureFriendlyName)));
                validationFlags |= SignatureVerificationStatusFlags.HasLifetimeSigningEku;
            }

            if (CertificateUtility.IsCertificateValidityPeriodInTheFuture(certificate))
            {
                issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3017, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_CertificateNotYetValid, signatureFriendlyName)));
                validationFlags |= SignatureVerificationStatusFlags.CertificateValidityInTheFuture;
            }

            return validationFlags;
        }

#if IS_SIGNING_SUPPORTED
        internal static SignatureVerificationStatusFlags ValidateTimestamp(Timestamp timestamp, Signature signature, bool treatIssuesAsErrors, List<SignatureLog> issues, SigningSpecifications spec)
        {
            if (timestamp == null)
            {
                throw new ArgumentNullException(nameof(timestamp));
            }
            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }
            if (issues == null)
            {
                throw new ArgumentNullException(nameof(issues));
            }

            // Default to specification v1
            spec = spec ?? SigningSpecifications.V1;

            var validationFlags = SignatureVerificationStatusFlags.NoErrors;
            var signerInfo = timestamp.SignerInfo;

            if (timestamp.SignerInfo.Certificate != null)
            {
                try
                {
                    signerInfo.CheckSignature(verifySignatureOnly: true);
                }
                catch (Exception e)
                {
                    issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3021, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampSignatureValidationFailed, signature.FriendlyName)));
                    issues.Add(SignatureLog.DebugLog(e.ToString()));
                    validationFlags |= SignatureVerificationStatusFlags.SignatureCheckFailed;
                }

                if (!CertificateUtility.IsSignatureAlgorithmSupported(signerInfo.Certificate))
                {
                    issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3022, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampUnsupportedSignatureAlgorithm, signature.FriendlyName)));
                    validationFlags |= SignatureVerificationStatusFlags.SignatureAlgorithmUnsupported;
                }

                if (!CertificateUtility.IsCertificatePublicKeyValid(signerInfo.Certificate))
                {
                    issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3023, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampCertificateFailsPublicKeyLengthRequirement, signature.FriendlyName)));
                    validationFlags |= SignatureVerificationStatusFlags.CertificatePublicKeyInvalid;
                }

                if (!spec.AllowedHashAlgorithmOids.Contains(signerInfo.DigestAlgorithm.Value))
                {
                    issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3024, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampSignatureUnsupportedDigestAlgorithm, signature.FriendlyName)));
                    validationFlags |= SignatureVerificationStatusFlags.HashAlgorithmUnsupported;
                }

                try
                {
                    var hashAlgorithm = CryptoHashUtility.OidToHashAlgorithmName(timestamp.TstInfo.HashAlgorithmId.Value);
                    var signatureValue = signature.GetSignatureValue();
                    var messageHash = hashAlgorithm.ComputeHash(signatureValue);

                    if (!timestamp.TstInfo.HasMessageHash(messageHash))
                    {
                        issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3019, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampIntegrityCheckFailed, signature.FriendlyName)));
                        validationFlags |= SignatureVerificationStatusFlags.IntegrityCheckFailed;
                    }
                }
                catch
                {
                    // If the hash algorithm is not supported OidToHashAlgorithmName will throw
                    issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3030, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampMessageImprintUnsupportedHashAlgorithm, signature.FriendlyName)));
                    validationFlags |= SignatureVerificationStatusFlags.MessageImprintUnsupportedAlgorithm;
                }

                if (CertificateUtility.IsCertificateValidityPeriodInTheFuture(signerInfo.Certificate))
                {
                    issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3025, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampNotYetValid, signature.FriendlyName)));
                    validationFlags |= SignatureVerificationStatusFlags.CertificateValidityInTheFuture;
                }

                if (!CertificateUtility.IsDateInsideValidityPeriod(signerInfo.Certificate, timestamp.GeneralizedTime))
                {
                    issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3036, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampGeneralizedTimeInvalid, signature.FriendlyName)));
                    validationFlags |= SignatureVerificationStatusFlags.GeneralizedTimeOutsideValidity;
                }
            }
            else
            {
                issues.Add(SignatureLog.Issue(treatIssuesAsErrors, NuGetLogCode.NU3020, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampNoCertificate, signature.FriendlyName)));
                validationFlags |= SignatureVerificationStatusFlags.NoCertificate;
            }

            return validationFlags;
        }
#endif
    }
}
