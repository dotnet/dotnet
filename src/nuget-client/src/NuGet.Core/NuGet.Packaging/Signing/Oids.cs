// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Signing
{
    public static class Oids
    {
        // RFC 5652 "signing-time" attribute https://tools.ietf.org/html/rfc5652#section-11.3
        public const string SigningTime = "1.2.840.113549.1.9.5";

        // RFC 5652 "countersignature" attribute https://tools.ietf.org/html/rfc5652#section-11.4
        public const string Countersignature = "1.2.840.113549.1.9.6";

        // RFC 3161 appendix A (https://tools.ietf.org/html/rfc3161#page-20)
        public const string SignatureTimeStampTokenAttribute = "1.2.840.113549.1.9.16.2.14";

        // RFC 8017 appendix B.1 (https://tools.ietf.org/html/rfc8017#appendix-B.1).
        public const string Sha1 = "1.3.14.3.2.26";

        // RFC 8017 appendix B.1 (https://tools.ietf.org/html/rfc8017#appendix-B.1).
        public const string Sha256 = "2.16.840.1.101.3.4.2.1";

        // RFC 8017 appendix B.1 (https://tools.ietf.org/html/rfc8017#appendix-B.1).
        public const string Sha384 = "2.16.840.1.101.3.4.2.2";

        // RFC 8017 appendix B.1 (https://tools.ietf.org/html/rfc8017#appendix-B.1).
        public const string Sha512 = "2.16.840.1.101.3.4.2.3";

        // RFC 4055 "sha256WithRSAEncryption" (https://tools.ietf.org/html/rfc4055#section-5)
        public const string Sha256WithRSAEncryption = "1.2.840.113549.1.1.11";

        // RFC 4055 "sha384WithRSAEncryption" (https://tools.ietf.org/html/rfc4055#section-5)
        public const string Sha384WithRSAEncryption = "1.2.840.113549.1.1.12";

        // RFC 4055 "sha512WithRSAEncryption" (https://tools.ietf.org/html/rfc4055#section-5)
        public const string Sha512WithRSAEncryption = "1.2.840.113549.1.1.13";

        // RFC 5280 codeSigning attribute, https://tools.ietf.org/html/rfc5280#section-4.2.1.12
        public const string CodeSigningEku = "1.3.6.1.5.5.7.3.3";

        // RFC 5652 "id-data" https://tools.ietf.org/html/rfc5652#section-4
        public const string Pkcs7Data = "1.2.840.113549.1.7.1";

        // ETSI TS 102 023 v1.2.2 http://www.etsi.org/deliver/etsi_ts/102000_102099/102023/01.02.02_60/ts_102023v010202p.pdf
        public const string BaselineTimestampPolicy = "0.4.0.2023.1.1";

        // RFC 3280 "id-kp-timeStamping" https://tools.ietf.org/html/rfc3280.html#section-4.2.1.13
        public const string TimeStampingEku = "1.3.6.1.5.5.7.3.8";

        // RFC 2459 "id-ce-extKeyUsage" https://tools.ietf.org/html/rfc3280.html#section-4.2.1.13
        public const string EnhancedKeyUsage = "2.5.29.37";

        // RFC 3161 "id-ct-TSTInfo" https://tools.ietf.org/html/rfc3161#section-2.4.2
        public const string TSTInfoContentType = "1.2.840.113549.1.9.16.1.4";

        // XCN_OID_KP_LIFETIME_SIGNING https://msdn.microsoft.com/en-us/library/windows/desktop/aa378132(v=vs.85).aspx
        public const string LifetimeSigningEku = "1.3.6.1.4.1.311.10.3.13";

        // RFC 5126 "commitment-type-indication" https://tools.ietf.org/html/rfc5126.html#section-5.11.1
        public const string CommitmentTypeIndication = "1.2.840.113549.1.9.16.2.16";

        // RFC 5126 "id-cti-ets-proofOfOrigin" https://tools.ietf.org/html/rfc5126.html#section-5.11.1
        public const string CommitmentTypeIdentifierProofOfOrigin = "1.2.840.113549.1.9.16.6.1";

        // RFC 5126 "id-cti-ets-proofOfReceipt" https://tools.ietf.org/html/rfc5126.html#section-5.11.1
        public const string CommitmentTypeIdentifierProofOfReceipt = "1.2.840.113549.1.9.16.6.2";

        // RFC 2634 "signing-certificate" http://tools.ietf.org/html/rfc2634#section-5.4
        public const string SigningCertificate = "1.2.840.113549.1.9.16.2.12";

        // RFC 5126 "signing-certificate-v2" https://tools.ietf.org/html/rfc5126.html#page-34
        public const string SigningCertificateV2 = "1.2.840.113549.1.9.16.2.47";

        // RFC 5280 "id-ce-authorityKeyIdentifier" https://tools.ietf.org/html/rfc5280#section-4.2.1.1
        public const string AuthorityKeyIdentifier = "2.5.29.35";

        // RFC 5280 "id-ce-subjectKeyIdentifier" https://tools.ietf.org/html/rfc5280#section-4.2.1.2
        public const string SubjectKeyIdentifier = "2.5.29.14";

        // RFC 5280 "anyPolicy" https://tools.ietf.org/html/rfc5280#section-4.2.1.4
        public const string AnyPolicy = "2.5.29.32.0";

        // RFC 5280 "id-qt-cps" https://tools.ietf.org/html/rfc5280#section-4.2.1.4
        public const string IdQtCps = "1.3.6.1.5.5.7.2.1";

        // RFC 5280 "id-qt-unotice" https://tools.ietf.org/html/rfc5280#section-4.2.1.4
        public const string IdQtUnotice = "1.3.6.1.5.5.7.2.2";

        public const string NuGetV3ServiceIndexUrl = "1.3.6.1.4.1.311.84.2.1.1.1";

        public const string NuGetPackageOwners = "1.3.6.1.4.1.311.84.2.1.1.2";
    }
}
