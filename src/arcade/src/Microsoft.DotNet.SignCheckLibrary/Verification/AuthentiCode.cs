// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using Microsoft.SignCheck.Interop;

namespace Microsoft.SignCheck.Verification
{
    public static class AuthentiCode
    {
        public static bool IsSigned(string path, SignatureVerificationResult svr, ISecurityInfoProvider securityInfoProvider) =>
            string.IsNullOrEmpty(path) ? false : IsSignedInternal(path, svr, securityInfoProvider);

        public static IEnumerable<Timestamp> GetTimestamps(string path, ISecurityInfoProvider securityInfoProvider) =>
            string.IsNullOrEmpty(path) ? Enumerable.Empty<Timestamp>() : GetTimestampsInternal(path, securityInfoProvider);

        private static bool IsSignedInternal(string path, SignatureVerificationResult svr, ISecurityInfoProvider securityInfoProvider)
        {
            if (securityInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(securityInfoProvider));
            }

            try
            {
                SignedCms signedCms = securityInfoProvider.ReadSecurityInfo(path);
                if (signedCms == null)
                {
                    return false;
                }

                if (signedCms.ContentInfo.ContentType.Value != WinCrypt.SPC_INDIRECT_DATA_OBJID)
                {
                    throw new CryptographicException($"Invalid content type: {signedCms.ContentInfo.ContentType.Value}");
                }

                SignerInfoCollection signerInfos = signedCms.SignerInfos;
                SignerInfo signerInfo = GetPrimarySignerInfo(signerInfos);

                // Check the signatures
                signerInfo.CheckSignature(signedCms.Certificates, true);
                signedCms.CheckSignature(signedCms.Certificates, true);

                return true;
            }
            catch (Exception ex)
            {
                svr.AddDetail(DetailKeys.Error, ex.Message);
                return false;
            }
        }

        private static IEnumerable<Timestamp> GetTimestampsInternal(string path, ISecurityInfoProvider securityInfoProvider)
        {
            if (securityInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(securityInfoProvider));
            }

            SignedCms signedCms = securityInfoProvider.ReadSecurityInfo(path);
            if (signedCms == null)
            {
                return Enumerable.Empty<Timestamp>();
            }

            return ExtractTimestamps(signedCms);
        }

        private static SignerInfo GetPrimarySignerInfo(SignerInfoCollection signerInfos)
        {
            int signerCount = signerInfos.Count;
            if (signerCount != 1)
            {
                throw new CryptographicException($"Invalid number of signers: {signerCount}. Expected 1.");
            }

            return signerInfos[0];
        }

        private static IEnumerable<Timestamp> ExtractTimestamps(SignedCms signedCms)
        {
            var timestamps = new List<Timestamp>();
            // Timestamp information can be stored in multiple sections.
            // A single SHA1 stores the timestamp as a counter sign in the unsigned attributes
            // Multiple authenticode signatures will store additional information as a nested signature
            // In the case of SHA2 signatures, we need to find and decode the timestamp token (RFC3161).
            // Luckily NuGet implemented a proper TST and DER parser to decode this
            foreach (SignerInfo signerInfo in signedCms.SignerInfos)
            {
                foreach (CryptographicAttributeObject unsignedAttribute in signerInfo.UnsignedAttributes)
                {
                    if (String.Equals(unsignedAttribute.Oid.Value, WinCrypt.szOID_RSA_counterSign, StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (SignerInfo counterSign in signerInfo.CounterSignerInfos)
                        {
                            foreach (CryptographicAttributeObject signedAttribute in counterSign.SignedAttributes)
                            {
                                if (String.Equals(signedAttribute.Oid.Value, WinCrypt.szOID_RSA_signingTime, StringComparison.OrdinalIgnoreCase))
                                {
                                    var st = (Pkcs9SigningTime)signedAttribute.Values[0];
                                    X509Certificate2 cert = counterSign.Certificate;

                                    var timeStamp = new Timestamp
                                    {
                                        SignedOn = st.SigningTime.ToLocalTime(),
                                        EffectiveDate = Convert.ToDateTime(cert.GetEffectiveDateString()).ToLocalTime(),
                                        ExpiryDate = Convert.ToDateTime(cert.GetExpirationDateString()).ToLocalTime(),
                                        SignatureAlgorithm = cert.SignatureAlgorithm.FriendlyName
                                    };

                                    timestamps.Add(timeStamp);
                                }
                            }
                        }
                    }
                    else if (String.Equals(unsignedAttribute.Oid.Value, WinCrypt.szOID_RFC3161_counterSign, StringComparison.OrdinalIgnoreCase))
                    {
                        timestamps.AddRange(GetTimestampsFromCounterSignature(unsignedAttribute.Values[0]));
                    }
                    else if (String.Equals(unsignedAttribute.Oid.Value, WinCrypt.szOID_NESTED_SIGNATURE, StringComparison.OrdinalIgnoreCase))
                    {
                        var nestedSignature = new Pkcs9AttributeObject(unsignedAttribute.Values[0]);
                        SignedCms nestedSignatureMessage = new SignedCms();
                        nestedSignatureMessage.Decode(nestedSignature.RawData);

                        foreach (SignerInfo nestedSignerInfo in nestedSignatureMessage.SignerInfos)
                        {
                            foreach (CryptographicAttributeObject nestedUnsignedAttribute in nestedSignerInfo.UnsignedAttributes)
                            {
                                if (String.Equals(nestedUnsignedAttribute.Oid.Value, WinCrypt.szOID_RFC3161_counterSign, StringComparison.OrdinalIgnoreCase))
                                {
                                    timestamps.AddRange(GetTimestampsFromCounterSignature(nestedUnsignedAttribute.Values[0]));
                                }
                            }
                        }
                    }
                }
            }

            return timestamps;
        }

        private static IEnumerable<Timestamp> GetTimestampsFromCounterSignature(AsnEncodedData unsignedAttribute)
        {
            var timestamps = new List<Timestamp>();
            var rfc3161CounterSignature = new Pkcs9AttributeObject(unsignedAttribute);
            SignedCms rfc3161Message = new SignedCms();
            rfc3161Message.Decode(rfc3161CounterSignature.RawData);

            foreach (SignerInfo rfc3161SignerInfo in rfc3161Message.SignerInfos)
            {
                if (String.Equals(rfc3161Message.ContentInfo.ContentType.Value, WinCrypt.szOID_TIMESTAMP_TOKEN, StringComparison.OrdinalIgnoreCase))
                {
                    var timestampToken = NuGet.Packaging.Signing.TstInfo.Read(rfc3161Message.ContentInfo.Content);

                    var timeStamp = new Timestamp
                    {
                        SignedOn = timestampToken.GenTime.LocalDateTime,
                        EffectiveDate = Convert.ToDateTime(rfc3161SignerInfo.Certificate.GetEffectiveDateString()).ToLocalTime(),
                        ExpiryDate = Convert.ToDateTime(rfc3161SignerInfo.Certificate.GetExpirationDateString()).ToLocalTime(),
                        SignatureAlgorithm = rfc3161SignerInfo.Certificate.SignatureAlgorithm.FriendlyName
                    };

                    timestamps.Add(timeStamp);
                }
            }

            return timestamps;
        }
    }
}
