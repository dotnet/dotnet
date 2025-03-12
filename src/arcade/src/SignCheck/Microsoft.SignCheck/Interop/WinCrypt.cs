// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Microsoft.SignCheck.Interop
{
    // See WinCrypt.h
    // See https://support.microsoft.com/en-us/help/323809/how-to-get-information-from-authenticode-signed-executables
    public static class WinCrypt
    {
        //-------------------------------------------------------------------------
        // dwObjectType for CryptQueryObject
        //-------------------------------------------------------------------------
        public const int CERT_QUERY_OBJECT_FILE = 0x00000001;
        public const int CERT_QUERY_OBJECT_BLOB = 0x00000002;

        //-------------------------------------------------------------------------
        //dwContentType for CryptQueryObject
        //-------------------------------------------------------------------------
        //encoded single certificate
        public const int CERT_QUERY_CONTENT_CERT = 1;
        //encoded single CTL
        public const int CERT_QUERY_CONTENT_CTL = 2;
        //encoded single CRL
        public const int CERT_QUERY_CONTENT_CRL = 3;
        //serialized store
        public const int CERT_QUERY_CONTENT_SERIALIZED_STORE = 4;
        //serialized single certificate
        public const int CERT_QUERY_CONTENT_SERIALIZED_CERT = 5;
        //serialized single CTL
        public const int CERT_QUERY_CONTENT_SERIALIZED_CTL = 6;
        //serialized single CRL
        public const int CERT_QUERY_CONTENT_SERIALIZED_CRL = 7;
        //a PKCS#7 signed message
        public const int CERT_QUERY_CONTENT_PKCS7_SIGNED = 8;
        //a PKCS#7 message, such as enveloped message.  But it is not a signed message,
        public const int CERT_QUERY_CONTENT_PKCS7_UNSIGNED = 9;
        //a PKCS7 signed message embedded in a file
        public const int CERT_QUERY_CONTENT_PKCS7_SIGNED_EMBED = 10;
        //an encoded PKCS#10
        public const int CERT_QUERY_CONTENT_PKCS10 = 11;
        //an encoded PFX BLOB
        public const int CERT_QUERY_CONTENT_PFX = 12;
        //an encoded CertificatePair (contains forward and/or reverse cross certs)
        public const int CERT_QUERY_CONTENT_CERT_PAIR = 13;
        //an encoded PFX BLOB, which was loaded to phCertStore
        public const int CERT_QUERY_CONTENT_PFX_AND_LOAD = 14;

        //-------------------------------------------------------------------------
        //dwExpectedConentTypeFlags for CryptQueryObject
        //-------------------------------------------------------------------------
        //encoded single certificate
        public const int CERT_QUERY_CONTENT_FLAG_CERT = (1 << CERT_QUERY_CONTENT_CERT);
        //encoded single CTL
        public const int CERT_QUERY_CONTENT_FLAG_CTL = (1 << CERT_QUERY_CONTENT_CTL);
        //encoded single CRL
        public const int CERT_QUERY_CONTENT_FLAG_CRL = (1 << CERT_QUERY_CONTENT_CRL);
        //serialized store
        public const int CERT_QUERY_CONTENT_FLAG_SERIALIZED_STORE = (1 << CERT_QUERY_CONTENT_SERIALIZED_STORE);
        //serialized single certificate
        public const int CERT_QUERY_CONTENT_FLAG_SERIALIZED_CERT = (1 << CERT_QUERY_CONTENT_SERIALIZED_CERT);
        //serialized single CTL
        public const int CERT_QUERY_CONTENT_FLAG_SERIALIZED_CTL = (1 << CERT_QUERY_CONTENT_SERIALIZED_CTL);
        //serialized single CRL
        public const int CERT_QUERY_CONTENT_FLAG_SERIALIZED_CRL = (1 << CERT_QUERY_CONTENT_SERIALIZED_CRL);
        //an encoded PKCS#7 signed message
        public const int CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED = (1 << CERT_QUERY_CONTENT_PKCS7_SIGNED);
        //an encoded PKCS#7 message.  But it is not a signed message
        public const int CERT_QUERY_CONTENT_FLAG_PKCS7_UNSIGNED = (1 << CERT_QUERY_CONTENT_PKCS7_UNSIGNED);
        //the content includes an embedded PKCS7 signed message
        public const int CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED_EMBED = (1 << CERT_QUERY_CONTENT_PKCS7_SIGNED_EMBED);
        //an encoded PKCS#10
        public const int CERT_QUERY_CONTENT_FLAG_PKCS10 = (1 << CERT_QUERY_CONTENT_PKCS10);
        //an encoded PFX BLOB
        public const int CERT_QUERY_CONTENT_FLAG_PFX = (1 << CERT_QUERY_CONTENT_PFX);
        //an encoded CertificatePair (contains forward and/or reverse cross certs)
        public const int CERT_QUERY_CONTENT_FLAG_CERT_PAIR = (1 << CERT_QUERY_CONTENT_CERT_PAIR);
        //an encoded PFX BLOB, and we do want to load it (not included in
        //CERT_QUERY_CONTENT_FLAG_ALL)
        public const int CERT_QUERY_CONTENT_FLAG_PFX_AND_LOAD = (1 << CERT_QUERY_CONTENT_PFX_AND_LOAD);
        //content can be any type
        public const int CERT_QUERY_CONTENT_FLAG_ALL = (
            CERT_QUERY_CONTENT_FLAG_CERT |
            CERT_QUERY_CONTENT_FLAG_CTL |
            CERT_QUERY_CONTENT_FLAG_CRL |
            CERT_QUERY_CONTENT_FLAG_SERIALIZED_STORE |
            CERT_QUERY_CONTENT_FLAG_SERIALIZED_CERT |
            CERT_QUERY_CONTENT_FLAG_SERIALIZED_CTL |
            CERT_QUERY_CONTENT_FLAG_SERIALIZED_CRL |
            CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED |
            CERT_QUERY_CONTENT_FLAG_PKCS7_UNSIGNED |
            CERT_QUERY_CONTENT_FLAG_PKCS7_SIGNED_EMBED |
            CERT_QUERY_CONTENT_FLAG_PKCS10 |
            CERT_QUERY_CONTENT_FLAG_PFX |
            CERT_QUERY_CONTENT_FLAG_CERT_PAIR);

        //-------------------------------------------------------------------------
        //dwFormatType for CryptQueryObject
        //-------------------------------------------------------------------------
        //the content is in binary format
        public const int CERT_QUERY_FORMAT_BINARY = 1;
        //the content is base64 encoded
        public const int CERT_QUERY_FORMAT_BASE64_ENCODED = 2;
        //the content is ascii hex encoded with "{ASN}" prefix
        public const int CERT_QUERY_FORMAT_ASN_ASCII_HEX_ENCODED = 3;

        //-------------------------------------------------------------------------
        //dwExpectedFormatTypeFlags for CryptQueryObject
        //-------------------------------------------------------------------------
        //the content is in binary format
        public const int CERT_QUERY_FORMAT_FLAG_BINARY = (1 << CERT_QUERY_FORMAT_BINARY);
        //the content is base64 encoded
        public const int CERT_QUERY_FORMAT_FLAG_BASE64_ENCODED = (1 << CERT_QUERY_FORMAT_BASE64_ENCODED);
        //the content is ascii hex encoded with "{ASN}" prefix
        public const int CERT_QUERY_FORMAT_FLAG_ASN_ASCII_HEX_ENCODED = (1 << CERT_QUERY_FORMAT_ASN_ASCII_HEX_ENCODED);
        //the content can be of any format
        public const int CERT_QUERY_FORMAT_FLAG_ALL = (
            CERT_QUERY_FORMAT_FLAG_BINARY |
            CERT_QUERY_FORMAT_FLAG_BASE64_ENCODED |
            CERT_QUERY_FORMAT_FLAG_ASN_ASCII_HEX_ENCODED);

        //-------------------------------------------------------------------------
        //  Get parameter types and their corresponding data structure definitions.
        //-------------------------------------------------------------------------
        public const int CMSG_TYPE_PARAM = 1;
        public const int CMSG_CONTENT_PARAM = 2;
        public const int CMSG_BARE_CONTENT_PARAM = 3;
        public const int CMSG_INNER_CONTENT_TYPE_PARAM = 4;
        public const int CMSG_SIGNER_COUNT_PARAM = 5;
        public const int CMSG_SIGNER_INFO_PARAM = 6;
        public const int CMSG_SIGNER_CERT_INFO_PARAM = 7;
        public const int CMSG_SIGNER_HASH_ALGORITHM_PARAM = 8;
        public const int CMSG_SIGNER_AUTH_ATTR_PARAM = 9;
        public const int CMSG_SIGNER_UNAUTH_ATTR_PARAM = 10;
        public const int CMSG_CERT_COUNT_PARAM = 11;
        public const int CMSG_CERT_PARAM = 12;
        public const int CMSG_CRL_COUNT_PARAM = 13;
        public const int CMSG_CRL_PARAM = 14;
        public const int CMSG_ENVELOPE_ALGORITHM_PARAM = 15;
        public const int CMSG_RECIPIENT_COUNT_PARAM = 17;
        public const int CMSG_RECIPIENT_INDEX_PARAM = 18;
        public const int CMSG_RECIPIENT_INFO_PARAM = 19;
        public const int CMSG_HASH_ALGORITHM_PARAM = 20;
        public const int CMSG_HASH_DATA_PARAM = 21;
        public const int CMSG_COMPUTED_HASH_PARAM = 22;
        public const int CMSG_ENCRYPT_PARAM = 26;
        public const int CMSG_ENCRYPTED_DIGEST = 27;
        public const int CMSG_ENCODED_SIGNER = 28;
        public const int CMSG_ENCODED_MESSAGE = 29;
        public const int CMSG_VERSION_PARAM = 30;
        public const int CMSG_ATTR_CERT_COUNT_PARAM = 31;
        public const int CMSG_ATTR_CERT_PARAM = 32;
        public const int CMSG_CMS_RECIPIENT_COUNT_PARAM = 33;
        public const int CMSG_CMS_RECIPIENT_INDEX_PARAM = 34;
        public const int CMSG_CMS_RECIPIENT_ENCRYPTED_KEY_INDEX_PARAM = 35;
        public const int CMSG_CMS_RECIPIENT_INFO_PARAM = 36;
        public const int CMSG_UNPROTECTED_ATTR_PARAM = 37;
        public const int CMSG_SIGNER_CERT_ID_PARAM = 38;
        public const int CMSG_CMS_SIGNER_INFO_PARAM = 39;

        public const string SPC_INDIRECT_DATA_OBJID = "1.3.6.1.4.1.311.2.1.4";
        public const string szOID_RSA_signingTime = "1.2.840.113549.1.9.5";
        public const string szOID_RSA_counterSign = "1.2.840.113549.1.9.6";
        public const string szOID_RFC3161_counterSign = "1.3.6.1.4.1.311.3.3.1";
        public const string szOID_NESTED_SIGNATURE = "1.3.6.1.4.1.311.2.4.1";
        public const string szOID_TIMESTAMP_TOKEN = "1.2.840.113549.1.9.16.1.4";
        public const string szOID_SIGNATURE_TIMESTAMP_ATTRIBUTE = "1.2.840.113549.1.9.16.2.14"; // Defined in RFC 3161 Appendix A

        [DllImport("Crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptQueryObject(
            int dwObjectType,
            IntPtr pvObject,
            int dwExpectedContentTypeFlags,
            int dwExpectedFormatTypeFlags,
            int dwFlags,
            out int pdwMsgAndCertEncodingType,
            out int pdwContentType,
            out int pdwFormatType,
            ref IntPtr phCertStore,
            ref IntPtr phMsg,
            ref IntPtr ppvContext);

        [DllImport("Crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptMsgGetParam(
            IntPtr hCryptMsg,
            int dwParamType,
            int dwIndex,
            IntPtr pvData,
            ref int pcbData);

        [DllImport("Crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptMsgGetParam(
            IntPtr hCryptMsg,
            int dwParamType,
            int dwIndex,
            [In, Out] byte[] pvData,
            ref int pcbData);

        /// <summary>
        /// Validate the timestamp signature on a specified array of bytes.
        /// </summary>
        /// <param name="pbTSContentInfo">A buffer containing the timestamp content.</param>
        /// <param name="cbTSContentInfo">The size (in bytes) of the buffer (pbTSContentInfo).</param>
        /// <param name="pbData">A buffer on which to validate the timestamp signature.</param>
        /// <param name="cbData">The size (in bytes) of the buffer (pbData).</param>
        /// <param name="hAdditionalStore">The handle of an additional store to search for TSA certificates and CTLs. Can be null if no additional store is to be searched.</param>
        /// <param name="ppTsContext">Pointer to a <see cref="CRYPT_TIMESTAMP_CONTEXT"/> structure. Must be freed by calling CryptMemFree</param>
        /// <param name="ppTSSigner">Pointer to a <see cref="CERT_CONTEXT"/> that receives the signer certificate. Must be freed by calling CertFreeCertificateContext.</param>
        /// <param name="phStore">Point to a handle that receives the opened store. Must be freed by calling CertCloseStore.</param>
        /// <returns></returns>
        [DllImport("Crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]        
        public static extern bool CryptVerifyTimeStampSignature(
            [In] byte[] pbTSContentInfo,
            uint cbTSContentInfo,
            [In] byte[] pbData,
            uint cbData,
            IntPtr hAdditionalStore,
            [Out] out IntPtr ppTsContext,
            [Out] out IntPtr ppTSSigner,
            [Out] out IntPtr phStore);

        [DllImport("crypt32.dll")]
        internal static extern void CryptMemFree([In] IntPtr pv);

        [DllImport("crypt32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CertFreeCertificateContext([In] IntPtr pCertContext);

        [DllImport("crypt32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CertCloseStore([In] IntPtr hCertStore, [In] uint dwFlags);
    }

    // https://msdn.microsoft.com/en-us/7a06eae5-96d8-4ece-98cb-cf0710d2ddbd
    // This structure is heavily aliased based on its contextual use, so we'll just stick with a generic name
    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_INTEGER_BLOB
    {
        public uint cbData;
        public IntPtr pbData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_ALGORITHM_IDENTIFIER
    {
        public string pszObjId;
        public CRYPT_INTEGER_BLOB Parameters;
    }

    /// <summary>
    /// A structure containing the encoded and decoded representations of a certificate. The structure must be
    /// freed by calling CertFreeCertificateContext.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_CONTEXT
    {
        /// <summary>
        /// The type of encoding used. Message and certificate encoding types can be combined using bitwise-OR.
        /// </summary>
        public uint dwCertEncodingType;
        /// <summary>
        /// A buffer containing the encoded certificate.
        /// </summary>
        public byte[] pbCertEncoded;
        /// <summary>
        /// The size (in bytes) of the encoded certificate.
        /// </summary>
        public uint cbCertEncoded;
        /// <summary>
        /// 
        /// </summary>
        public IntPtr pCertInfo;
        /// <summary>
        /// A handle to the certificate store containing the certificate context 
        /// </summary>
        public IntPtr hCertStore;
    }

    /// <summary>
    /// A structure containing both encoded and decoded representations of a time stamp token.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_TIMESTAMP_CONTEXT
    {
        /// <summary>
        /// The size (in bytes) of the buffer (pbEncoded).
        /// </summary>
        public uint cbEncoded;
        /// <summary>
        /// A buffer containing ASN.1 encoded content.
        /// </summary>
        public IntPtr pbEncoded;
        /// <summary>
        /// Point to a <see cref="CRYPT_TIMESTAMP_INFO"/> structure.
        /// </summary>
        public IntPtr pTimeStamp;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_TIMESTAMP_INFO
    {
        public uint dwVersion;
        public string pszTSAPolicyId;
        public CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;
        public CRYPT_INTEGER_BLOB HashedMessage;
        public CRYPT_INTEGER_BLOB SerialNumber;
        public FILETIME ftTime;
        public IntPtr pvAccuracy;
        public bool fOrdering;
        public CRYPT_INTEGER_BLOB Nonce;
        public CRYPT_INTEGER_BLOB Tsa;
        public uint cExtension;
        public IntPtr rgExtension;
    }
}
