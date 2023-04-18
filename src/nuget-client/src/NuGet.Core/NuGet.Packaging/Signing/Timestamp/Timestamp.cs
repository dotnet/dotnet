// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography.Pkcs;
#endif

namespace NuGet.Packaging.Signing
{
    public sealed class Timestamp
    {
#if IS_SIGNING_SUPPORTED

        /// <summary>
        /// Upper limit of Timestamp.
        /// </summary>
        public DateTimeOffset UpperLimit { get; }

        /// <summary>
        /// Lower limit of Timestamp.
        /// </summary>
        public DateTimeOffset LowerLimit { get; }

        /// <summary>
        /// Time timestamp was created by the Time Stamp Authority.
        /// </summary>
        public DateTimeOffset GeneralizedTime { get; }

        /// <summary>
        /// A SignedCms object holding the timestamp and SignerInfo.
        /// </summary>
        public SignedCms SignedCms { get; }

        /// <summary>
        /// SignerInfo for this timestamp.
        /// </summary>
        public SignerInfo SignerInfo => SignedCms.SignerInfos[0];

        /// <summary>
        /// Timestamp token info for this timestamp.
        /// </summary>
        internal IRfc3161TimestampTokenInfo TstInfo { get; }

        /// <summary>
        /// Default constructor. Limits are set to current time.
        /// </summary>
        public Timestamp()
        {
            GeneralizedTime = DateTimeOffset.Now;
            UpperLimit = GeneralizedTime;
            LowerLimit = GeneralizedTime;
        }

        /// <summary>
        /// SignedCms containing a time stamp authority token reponse
        /// </summary>
        /// <param name="timestampCms">SignedCms from Time Stamp Authority</param>
        public Timestamp(SignedCms timestampCms)
        {
            SignedCms = timestampCms ?? throw new ArgumentNullException(nameof(timestampCms));

            if (Rfc3161TimestampVerificationUtility.TryReadTSTInfoFromSignedCms(timestampCms, out var tstInfo))
            {
                try
                {
                    SignedCms.CheckSignature(verifySignatureOnly: true);
                }
                catch (Exception ex)
                {
                    throw new TimestampException(NuGetLogCode.NU3021, Strings.VerifyError_TimestampSignatureValidationFailed, ex);
                }

                TstInfo = tstInfo;
                GeneralizedTime = tstInfo.Timestamp;

                var accuracyInMilliseconds = Rfc3161TimestampVerificationUtility.GetAccuracyInMilliseconds(tstInfo);
                UpperLimit = tstInfo.Timestamp.AddMilliseconds(accuracyInMilliseconds);
                LowerLimit = tstInfo.Timestamp.AddMilliseconds(-accuracyInMilliseconds);
            }
            else
            {
                throw new TimestampException(NuGetLogCode.NU3021, Strings.VerifyError_TimestampSignatureValidationFailed);
            }
        }

        /// <summary>
        /// Verify if the timestamp object meets the specification requirements.
        /// </summary>
        /// <param name="signature">Signature which this timestamp is for.</param>
        /// <param name="allowIgnoreTimestamp">Setting that tells if a timestamp can be ignored if it doesn't meet the requirements. Used to know if warnings or errors should be logged for an issue.</param>
        /// <param name="allowUnknownRevocation">Setting that tells if unkown revocation is valid when building the chain.</param>
        /// <param name="issues">List of log messages.</param>
        /// <returns>true if the timestamp meets the requierements, false otherwise.</returns>
        internal SignatureVerificationStatusFlags Verify(
            Signature signature,
            SignedPackageVerifierSettings settings,
            HashAlgorithmName fingerprintAlgorithm,
            List<SignatureLog> issues)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var flags = SignatureVerificationStatusFlags.NoErrors;

            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }
            if (issues == null)
            {
                throw new ArgumentNullException(nameof(issues));
            }

            var treatIssueAsError = !settings.AllowIgnoreTimestamp;
            var timestamperCertificate = SignerInfo.Certificate;
            if (timestamperCertificate == null)
            {
                flags |= SignatureVerificationStatusFlags.NoCertificate;

                issues.Add(SignatureLog.Issue(treatIssueAsError, NuGetLogCode.NU3020, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampNoCertificate, signature.FriendlyName)));
                return flags;
            }

            flags |= VerificationUtility.ValidateTimestamp(this, signature, treatIssueAsError, issues, SigningSpecifications.V1);
            if (flags == SignatureVerificationStatusFlags.NoErrors)
            {
                issues.Add(SignatureLog.InformationLog(string.Format(CultureInfo.CurrentCulture, Strings.TimestampValue, GeneralizedTime.LocalDateTime.ToString(CultureInfo.CurrentCulture)) + Environment.NewLine));

                issues.Add(SignatureLog.InformationLog(string.Format(CultureInfo.CurrentCulture,
                    Strings.VerificationTimestamperCertDisplay,
                    signature.FriendlyName,
                    $"{Environment.NewLine}{CertificateUtility.X509Certificate2ToString(timestamperCertificate, fingerprintAlgorithm)}")));

                var certificateExtraStore = SignedCms.Certificates;

                using (X509ChainHolder chainHolder = X509ChainHolder.CreateForTimestamping())
                {
                    IX509Chain chain = chainHolder.Chain2;

                    // This flag should only be set for verification scenarios, not signing.
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreNotTimeValid;

                    CertificateChainUtility.SetCertBuildChainPolicy(chain.ChainPolicy, certificateExtraStore, DateTime.Now, CertificateType.Timestamp);

                    if (settings.RevocationMode == RevocationMode.Offline)
                    {
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Offline;
                    }
                    else
                    {
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    }

                    var chainBuildSucceed = CertificateChainUtility.BuildCertificateChain(chain, timestamperCertificate, out var chainStatusList);
                    string x509ChainString = CertificateUtility.X509ChainToString(chain.PrivateReference, fingerprintAlgorithm);

                    if (!string.IsNullOrWhiteSpace(x509ChainString))
                    {
                        issues.Add(SignatureLog.DetailedLog(x509ChainString));
                    }

                    if (chainBuildSucceed)
                    {
                        return flags;
                    }

                    var chainBuildingHasIssues = false;
                    IEnumerable<string> messages;

                    var timestampInvalidCertificateFlags = CertificateChainUtility.DefaultObservedStatusFlags;

                    if (CertificateChainUtility.TryGetStatusAndMessage(chainStatusList, timestampInvalidCertificateFlags, out messages))
                    {
                        foreach (var message in messages)
                        {
                            issues.Add(SignatureLog.Issue(treatIssueAsError, NuGetLogCode.NU3028, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampVerifyChainBuildingIssue, signature.FriendlyName, message)));
                        }

                        flags |= SignatureVerificationStatusFlags.ChainBuildingFailure;
                        chainBuildingHasIssues = true;
                    }

                    // For all the special cases, chain status list only has unique elements for each chain status flag present
                    // therefore if we are checking for one specific chain status we can use the first of the returned list
                    // if we are combining checks for more than one, then we have to use the whole list.

                    if (CertificateChainUtility.TryGetStatusAndMessage(chainStatusList, X509ChainStatusFlags.UntrustedRoot, out messages))
                    {
                        SignatureUtility.LogAdditionalContext(chain, issues);

                        issues.Add(SignatureLog.Error(NuGetLogCode.NU3028, string.Format(CultureInfo.CurrentCulture, Strings.VerifyTimestampChainBuildingIssue_UntrustedRoot, signature.FriendlyName)));

                        flags |= SignatureVerificationStatusFlags.UntrustedRoot;
                        chainBuildingHasIssues = true;
                    }

                    if (CertificateChainUtility.TryGetStatusAndMessage(chainStatusList, X509ChainStatusFlags.Revoked, out messages))
                    {
                        issues.Add(SignatureLog.Error(NuGetLogCode.NU3028, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampVerifyChainBuildingIssue, signature.FriendlyName, messages.First())));
                        flags |= SignatureVerificationStatusFlags.CertificateRevoked;

                        return flags;
                    }

                    var offlineRevocationErrors = CertificateChainUtility.TryGetStatusAndMessage(chainStatusList, X509ChainStatusFlags.OfflineRevocation, out var _);
                    var unknownRevocationErrors = CertificateChainUtility.TryGetStatusAndMessage(chainStatusList, X509ChainStatusFlags.RevocationStatusUnknown, out var unknownRevocationStatusMessages);
                    if (offlineRevocationErrors || unknownRevocationErrors)
                    {
                        if (treatIssueAsError)
                        {
                            string unknownRevocationMessage = null;

                            if (unknownRevocationErrors)
                            {
                                unknownRevocationMessage = string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampVerifyChainBuildingIssue, signature.FriendlyName, unknownRevocationStatusMessages.First());
                            }

                            if (settings.RevocationMode == RevocationMode.Offline)
                            {
                                if (offlineRevocationErrors)
                                {
                                    issues.Add(SignatureLog.InformationLog(string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampVerifyChainBuildingIssue, signature.FriendlyName, Strings.VerifyCertTrustOfflineWhileRevocationModeOffline)));
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
                                    issues.Add(SignatureLog.Issue(!settings.AllowUnknownRevocation, NuGetLogCode.NU3028, string.Format(CultureInfo.CurrentCulture, Strings.VerifyError_TimestampVerifyChainBuildingIssue, signature.FriendlyName, Strings.VerifyCertTrustOfflineWhileRevocationModeOnline)));
                                }

                                if (unknownRevocationMessage != null)
                                {
                                    issues.Add(SignatureLog.Issue(!settings.AllowUnknownRevocation, NuGetLogCode.NU3028, unknownRevocationMessage));
                                }

                            }
                        }

                        if (!chainBuildingHasIssues && (settings.AllowIgnoreTimestamp || settings.AllowUnknownRevocation))
                        {
                            return flags;
                        }

                        flags |= SignatureVerificationStatusFlags.UnknownRevocation;
                        chainBuildingHasIssues = true;
                    }

                    // Debug log any errors
                    issues.Add(
                        SignatureLog.DebugLog(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                $"{signature.FriendlyName}'s timestamp",
                                Strings.VerifyError_InvalidCertificateChain,
                                string.Join(", ", chainStatusList.Select(x => x.Status.ToString())))));
                }
            }

            return flags;
        }
#endif
    }
}
