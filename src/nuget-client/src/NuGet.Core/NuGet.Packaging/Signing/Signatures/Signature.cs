// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
#endif
using NuGet.Common;
using HashAlgorithmName = NuGet.Common.HashAlgorithmName;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Package signature information.
    /// </summary>
    public abstract class Signature : ISignature
    {
#if IS_SIGNING_SUPPORTED
        private readonly Lazy<IReadOnlyList<Timestamp>> _timestamps;

        /// <summary>
        /// Indicates if this is an author or repository signature.
        /// </summary>
        public SignatureType Type { get; }

        /// <summary>
        /// Signature timestamps.
        /// </summary>
        public IReadOnlyList<Timestamp> Timestamps => _timestamps.Value;

        /// <summary>
        /// SignerInfo for this signature.
        /// </summary>
        public SignerInfo SignerInfo { get; }

        public abstract byte[] GetSignatureValue();

        private IDictionary<HashAlgorithmName, string> _signingCertificateFingerprintLookup;

        protected Signature(SignerInfo signerInfo, SignatureType type)
        {
            SignerInfo = signerInfo;
            Type = type;

            _timestamps = new Lazy<IReadOnlyList<Timestamp>>(() => GetTimestamps(SignerInfo, FriendlyName));

            if (Type != SignatureType.Unknown)
            {
                VerifySigningTimeAttribute(SignerInfo);
            }
        }

        protected abstract void ThrowForInvalidSignature();

        public virtual string FriendlyName => Strings.SignatureFriendlyName;

        /// <summary>
        /// Get a valid timestamp from the unsigned attributes if present
        /// </summary>
        /// <param name="settings">Specify what is allowed in the validation for timestamp</param>
        /// <param name="fingerprintAlgorithm">fingerprint algorithm for displaying timestamp's certificate information</param>
        /// <param name="issues">List of log messages.</param>
        /// <param name="verificationFlags">Flags that specify the status of the verification</param>
        /// <param name="validTimestamp">TTimestamp found in the signature that passes validation with the given <see cref="settings"/></param>
        /// <remarks>If <see cref="SignedPackageVerifierSettings.AllowNoTimestamp" /> is set to true this method return true with a <see cref="validTimestamp" /> set to null.</remarks>
        /// <returns>true if a valid timestamp was found</returns>
        internal bool TryGetValidTimestamp(
            SignedPackageVerifierSettings settings,
            HashAlgorithmName fingerprintAlgorithm,
            List<SignatureLog> issues,
            out SignatureVerificationStatusFlags verificationFlags,
            out Timestamp validTimestamp)
        {
            if (issues == null)
            {
                throw new ArgumentNullException(nameof(issues));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            verificationFlags = SignatureVerificationStatusFlags.NoErrors;
            validTimestamp = null;

            var timestamps = Timestamps;

            if (timestamps.Count == 0)
            {
                issues.Add(SignatureLog.Issue(!settings.AllowNoTimestamp, NuGetLogCode.NU3027, Strings.ErrorNoTimestamp));
                if (!settings.AllowNoTimestamp)
                {
                    verificationFlags |= SignatureVerificationStatusFlags.NoValidTimestamp;
                    return false;
                }
            }

            if (timestamps.Count > 1 && !settings.AllowMultipleTimestamps)
            {
                issues.Add(SignatureLog.Error(NuGetLogCode.NU3000, Strings.ErrorMultipleTimestamps));
                verificationFlags |= SignatureVerificationStatusFlags.MultipleTimestamps;
                return false;
            }

            var timestamp = timestamps.FirstOrDefault();
            if (timestamp != null)
            {
                verificationFlags |= timestamp.Verify(this, settings, fingerprintAlgorithm, issues);

                if (verificationFlags != SignatureVerificationStatusFlags.NoErrors &&
                    verificationFlags != SignatureVerificationStatusFlags.UnknownRevocation)
                {
                    return false;
                }

                validTimestamp = timestamp;
            }

            return true;
        }

        /// <summary>
        /// Verify if the signature object meets the specification trust and validity requirements.
        /// </summary>
        /// <param name="timestamp">Timestamp for this signature, if signature is not timestamped it can be null.</param>
        /// <param name="allowUntrusted">Setting that tells if a signature that does not meet any soft failure requirements can still be allowed. Used to know if warnings or errors should be logged for an issue.</param>
        /// <param name="allowUnknownRevocation">Setting that tells if unkown revocation is valid when building the chain.</param>
        /// <param name="allowUntrustedSelfSignedCertificate">Setting that tells if an untrusted self-signed certificate should be allowed as the signing certificate.</param>
        /// <param name="fingerprintAlgorithm">Algorithm used to calculate and display the certificate's fingerprint.</param>
        /// <param name="certificateExtraStore">Collection of certificates to help the chain building engine as an extra store.</param>
        /// <param name="issues">List of log messages.</param>
        /// <remarks>This is only public for ease of testing</remarks>
        /// <returns>Status of trust for signature.</returns>
        public virtual SignatureVerificationSummary Verify(
            Timestamp timestamp,
            SignatureVerifySettings settings,
            HashAlgorithmName fingerprintAlgorithm,
            X509Certificate2Collection certificateExtraStore)
        {
            settings = settings ?? SignatureVerifySettings.Default;
            var issues = new List<SignatureLog>();
            SignatureVerificationStatus status;

            var certificate = SignerInfo.Certificate;
            if (certificate == null)
            {
                issues.Add(SignatureLog.Issue(!settings.AllowIllegal, NuGetLogCode.NU3010, string.Format(CultureInfo.CurrentCulture, Strings.Verify_ErrorNoCertificate, FriendlyName)));

                status = settings.AllowIllegal ? SignatureVerificationStatus.Valid : SignatureVerificationStatus.Disallowed;
                return new SignatureVerificationSummary(Type, status, SignatureVerificationStatusFlags.NoCertificate, issues);
            }

            issues.Add(SignatureLog.InformationLog(string.Format(CultureInfo.CurrentCulture,
                Strings.VerificationCertDisplay,
                FriendlyName,
                $"{Environment.NewLine}")));

            // Debug log any errors
            issues.AddRange(CertificateUtility.X509Certificate2ToLogMessages(certificate, fingerprintAlgorithm));

            try
            {
                SignerInfo.CheckSignature(verifySignatureOnly: true);
            }
            catch (Exception e)
            {
                issues.Add(SignatureLog.Issue(!settings.AllowIllegal, NuGetLogCode.NU3012, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_SignatureVerificationFailed, FriendlyName)));
                issues.Add(SignatureLog.DebugLog(e.ToString()));
                status = settings.AllowIllegal ? SignatureVerificationStatus.Valid : SignatureVerificationStatus.Disallowed;

                return new SignatureVerificationSummary(Type, status, SignatureVerificationStatusFlags.SignatureCheckFailed, issues);
            }

            DateTimeOffset? expirationTime = null;
            var certificateFlags = VerificationUtility.ValidateSigningCertificate(certificate, !settings.AllowIllegal, FriendlyName, issues);
            if (certificateFlags != SignatureVerificationStatusFlags.NoErrors)
            {
                status = VerificationUtility.GetSignatureVerificationStatus(certificateFlags);
                return new SignatureVerificationSummary(Type, status, certificateFlags, timestamp, expirationTime, issues);
            }
            else
            {
                timestamp = timestamp ?? new Timestamp();
                SignatureVerificationStatusFlags flags = SignatureVerificationStatusFlags.NoErrors;
                using (X509ChainHolder chainHolder = X509ChainHolder.CreateForCodeSigning())
                {
                    IX509Chain chain = chainHolder.Chain2;

                    // This flag should only be set for verification scenarios, not signing.
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreNotTimeValid;

                    CertificateChainUtility.SetCertBuildChainPolicy(chain.ChainPolicy, certificateExtraStore, timestamp.UpperLimit.LocalDateTime, CertificateType.Signature);

                    if (settings.RevocationMode == RevocationMode.Offline)
                    {
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Offline;
                    }
                    else
                    {
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    }

                    var chainBuildingSucceeded = CertificateChainUtility.BuildCertificateChain(chain, certificate, out var chainStatuses);
                    string x509ChainString = CertificateUtility.X509ChainToString(chain.PrivateReference, fingerprintAlgorithm);

                    if (!string.IsNullOrWhiteSpace(x509ChainString))
                    {
                        issues.Add(SignatureLog.DetailedLog(x509ChainString));
                    }

                    var chainBuildingHasIssues = false;

                    if (!chainBuildingSucceeded && chainStatuses.Length == 0)
                    {
                        return new SignatureVerificationSummary(Type, SignatureVerificationStatus.Unknown, SignatureVerificationStatusFlags.UnknownBuildStatus, timestamp, issues);
                    }

                    var statusFlags = CertificateChainUtility.DefaultObservedStatusFlags;

                    IEnumerable<string> messages;
                    if (CertificateChainUtility.TryGetStatusAndMessage(chainStatuses, statusFlags, out messages))
                    {
                        foreach (var message in messages)
                        {
                            issues.Add(SignatureLog.Issue(!settings.AllowIllegal, NuGetLogCode.NU3012, string.Format(CultureInfo.CurrentCulture, Strings.VerifyChainBuildingIssue, FriendlyName, message)));
                        }

                        chainBuildingHasIssues = true;
                        flags |= SignatureVerificationStatusFlags.ChainBuildingFailure;
                    }

                    // For all the special cases, chain status list only has unique elements for each chain status flag present
                    // therefore if we are checking for one specific chain status we can use the first of the returned list
                    // if we are combining checks for more than one, then we have to use the whole list.
                    if (CertificateChainUtility.TryGetStatusAndMessage(chainStatuses, X509ChainStatusFlags.Revoked, out messages))
                    {
                        issues.Add(SignatureLog.Error(NuGetLogCode.NU3012, string.Format(CultureInfo.CurrentCulture, Strings.VerifyChainBuildingIssue, FriendlyName, messages.First())));
                        flags |= SignatureVerificationStatusFlags.CertificateRevoked;

                        return new SignatureVerificationSummary(Type, SignatureVerificationStatus.Suspect, flags, timestamp, issues);
                    }

                    if (CertificateChainUtility.TryGetStatusAndMessage(chainStatuses, X509ChainStatusFlags.UntrustedRoot, out messages))
                    {
                        if (settings.ReportUntrustedRoot)
                        {
                            SignatureUtility.LogAdditionalContext(chain, issues);

                            issues.Add(SignatureLog.Issue(!settings.AllowUntrusted, NuGetLogCode.NU3018, string.Format(CultureInfo.CurrentCulture, Strings.VerifyChainBuildingIssue_UntrustedRoot, FriendlyName)));
                        }

                        if (!settings.AllowUntrusted)
                        {
                            chainBuildingHasIssues = true;
                            flags |= SignatureVerificationStatusFlags.UntrustedRoot;
                        }
                    }

                    var offlineRevocationErrors = CertificateChainUtility.TryGetStatusAndMessage(chainStatuses, X509ChainStatusFlags.OfflineRevocation, out var _);
                    var unknownRevocationErrors = CertificateChainUtility.TryGetStatusAndMessage(chainStatuses, X509ChainStatusFlags.RevocationStatusUnknown, out var unknownRevocationStatusMessages);
                    if (offlineRevocationErrors || unknownRevocationErrors)
                    {
                        if (settings.ReportUnknownRevocation)
                        {
                            string unknownRevocationMessage = null;

                            if (unknownRevocationErrors)
                            {
                                unknownRevocationMessage = string.Format(CultureInfo.CurrentCulture, Strings.VerifyChainBuildingIssue, FriendlyName, unknownRevocationStatusMessages.First());
                            }

                            if (settings.RevocationMode == RevocationMode.Offline)
                            {
                                if (offlineRevocationErrors)
                                {
                                    issues.Add(SignatureLog.InformationLog(string.Format(CultureInfo.CurrentCulture, Strings.VerifyChainBuildingIssue, FriendlyName, Strings.VerifyCertTrustOfflineWhileRevocationModeOffline)));
                                }

                                if (unknownRevocationMessage != null)
                                {
                                    issues.Add(SignatureLog.InformationLog(unknownRevocationMessage));
                                }
                            }
                            else
                            {
                                if (offlineRevocationErrors)
                                {
                                    issues.Add(SignatureLog.Issue(!settings.AllowUnknownRevocation, NuGetLogCode.NU3018, string.Format(CultureInfo.CurrentCulture, Strings.VerifyChainBuildingIssue, FriendlyName, Strings.VerifyCertTrustOfflineWhileRevocationModeOnline)));
                                }

                                if (unknownRevocationMessage != null)
                                {
                                    issues.Add(SignatureLog.Issue(!settings.AllowUnknownRevocation, NuGetLogCode.NU3018, unknownRevocationMessage));
                                }
                            }
                        }

                        if (!settings.AllowUnknownRevocation)
                        {
                            chainBuildingHasIssues = true;
                            flags |= SignatureVerificationStatusFlags.UnknownRevocation;
                        }
                    }

                    if (!chainBuildingSucceeded)
                    {
                        // Debug log any errors
                        issues.Add(SignatureLog.DebugLog(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.VerifyError_InvalidCertificateChain,
                            FriendlyName,
                            string.Join(", ", chainStatuses.Select(x => x.Status.ToString())))));
                    }

                    var isSignatureTimeValid = Rfc3161TimestampVerificationUtility.ValidateSignerCertificateAgainstTimestamp(certificate, timestamp);
                    if (isSignatureTimeValid && !chainBuildingHasIssues)
                    {
                        return new SignatureVerificationSummary(Type, SignatureVerificationStatus.Valid, flags, timestamp, issues);
                    }
                    else if (!isSignatureTimeValid)
                    {
                        issues.Add(
                            SignatureLog.Issue(
                                !settings.AllowUntrusted,
                                NuGetLogCode.NU3037,
                                string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_SignatureNotTimeValid, FriendlyName)));

                        if (!settings.AllowUntrusted)
                        {
                            flags |= SignatureVerificationStatusFlags.CertificateExpired;
                        }

                        expirationTime = DateTime.SpecifyKind(certificate.NotAfter, DateTimeKind.Local);
                    }
                }

                status = VerificationUtility.GetSignatureVerificationStatus(flags);

                return new SignatureVerificationSummary(Type, status, flags, timestamp, expirationTime, issues);
            }
        }

        public string GetSigningCertificateFingerprint(HashAlgorithmName algorithm)
        {
            if (!Enum.IsDefined(typeof(HashAlgorithmName), algorithm))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.UnrecognizedEnumValue,
                        algorithm),
                    nameof(algorithm));
            }

            var certificate = SignerInfo.Certificate;
            if (certificate == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.Verify_ErrorNoCertificate, FriendlyName));
            }

            if (_signingCertificateFingerprintLookup == null)
            {
                _signingCertificateFingerprintLookup = new Dictionary<HashAlgorithmName, string>();
            }

            if (_signingCertificateFingerprintLookup.TryGetValue(algorithm, out var fingerprint))
            {
                return fingerprint;
            }

            fingerprint = CertificateUtility.GetHashString(certificate, algorithm);

            _signingCertificateFingerprintLookup.Add(algorithm, fingerprint);

            return fingerprint;
        }

        private void VerifySigningTimeAttribute(SignerInfo signerInfo)
        {
            var attribute = signerInfo.SignedAttributes.GetAttributeOrDefault(Oids.SigningTime);

            if (attribute == null)
            {
                ThrowForInvalidSignature();
            }
        }

        private static IReadOnlyList<Timestamp> GetTimestamps(SignerInfo signer, string signatureFriendlyName)
        {
            CryptographicAttributeObjectCollection unsignedAttributes = signer.UnsignedAttributes;

            var timestampList = new List<Timestamp>();

            foreach (CryptographicAttributeObject attribute in unsignedAttributes)
            {
                if (string.Equals(attribute.Oid.Value, Oids.SignatureTimeStampTokenAttribute, StringComparison.Ordinal))
                {
                    foreach (AsnEncodedData value in attribute.Values)
                    {
                        var timestampCms = new SignedCms();

                        timestampCms.Decode(value.RawData);

                        using (var certificates = SignatureUtility.GetTimestampCertificates(
                            timestampCms,
                            SigningSpecifications.V1,
                            signatureFriendlyName))
                        {
                            if (certificates == null || certificates.Count == 0)
                            {
                                throw new SignatureException(NuGetLogCode.NU3029, Strings.InvalidTimestampSignature);
                            }
                        }

                        timestampList.Add(new Timestamp(timestampCms));
                    }
                }
            }

            return timestampList;
        }
#endif
    }
}
