// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.Packaging.Signing
{
    internal static class NativeMethods
    {
        internal const uint PKCS_ATTRIBUTE = 22;
        internal const uint PKCS7_SIGNER_INFO = 500;
        internal const int ERROR_MORE_DATA = 234;
        internal const uint CMSG_SIGNED = 2;
        internal const uint CERT_KEY_IDENTIFIER_PROP_ID = 20;
        internal const uint CERT_ID_KEY_IDENTIFIER = 2;

        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern SafeCryptMsgHandle CryptMsgOpenToEncode(
            CMSG_ENCODING dwMsgEncodingType,
            uint dwFlags,
            uint dwMsgType,
            ref CMSG_SIGNED_ENCODE_INFO pvMsgEncodeInfo,
            string pszInnerContentObjID,
            IntPtr pStreamInfo
        );

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380228(v=vs.85).aspx
        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern SafeCryptMsgHandle CryptMsgOpenToDecode(
            CMSG_ENCODING dwMsgEncodingType,
            CMSG_OPENTODECODE_FLAGS dwFlags,
            uint dwMsgType,
            IntPtr hCryptProv,
            IntPtr pRecipientInfo,
            IntPtr pStreamInfo);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/aa380221(v=vs.85).aspx
        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool CryptMsgCountersign(
            SafeCryptMsgHandle hCryptMsg,
            uint dwIndex,
            int cCountersigners,
            CMSG_SIGNER_ENCODE_INFO rgCountersigners);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380219(v=vs.85).aspx
        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool CryptMsgClose(IntPtr hCryptMsg);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380231(v=vs.85).aspx
        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool CryptMsgUpdate(
            SafeCryptMsgHandle hCryptMsg,
            byte[] pbData,
            uint cbData,
            bool fFinal);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380227(v=vs.85).aspx
        [DllImport("crypt32.dll", SetLastError = true)]
        public static extern bool CryptMsgGetParam(
            SafeCryptMsgHandle hCryptMsg,
            CMSG_GETPARAM_TYPE dwParamType,
            uint dwIndex,
            byte[] pvData,
            ref uint pcbData);

        [DllImport("crypt32.dll", SetLastError = true)]
        internal static extern bool CryptMsgGetParam(
            SafeCryptMsgHandle hCryptMsg,
            CMSG_GETPARAM_TYPE dwParamType,
            uint dwIndex,
            IntPtr pvData,
            ref uint pcbData);

        [DllImport("crypt32.dll", SetLastError = true)]
        internal static extern bool CryptDecodeObject(
            CMSG_ENCODING dwCertEncodingType,
            IntPtr lpszStructType,
            IntPtr pbEncoded,
            uint cbEncoded,
            uint dwFlags,
            IntPtr pvStructInfo,
            IntPtr pcbStructInfo);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa380220(v=vs.85).aspx
        [DllImport("crypt32.dll", SetLastError = true)]
        internal static extern bool CryptMsgControl(
            SafeCryptMsgHandle hCryptMsg,
            uint dwFlags,
            CMSG_CONTROL_TYPE dwCtrlType,
            IntPtr pvCtrlPara);

        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa379922(v=vs.85).aspx
        [DllImport("crypt32.dll", SetLastError = true)]
        internal static extern bool CryptEncodeObjectEx(
            CMSG_ENCODING dwCertEncodingType,
            IntPtr lpszStructType,
            IntPtr pvStructInfo,
            uint dwFlags,
            IntPtr pEncodePara,
            IntPtr pvEncoded,
            ref uint pcbEncoded);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool CryptReleaseContext(IntPtr hProv, int dwFlags);

        [DllImport("crypt32.dll", SetLastError = true)]
        internal static extern IntPtr CertDuplicateCertificateContext(IntPtr pCertContext);

        [DllImport("crypt32.dll", SetLastError = true)]
        internal static extern bool CertFreeCertificateContext(IntPtr pCertContext);

        [DllImport("crypt32.dll", SetLastError = true)]
        internal extern static bool CertGetCertificateContextProperty(
            IntPtr pCertContext,
            uint dwPropId,
            IntPtr pvData,
            ref uint pcbData);

        internal static int GetHRForWin32Error(int err)
        {
            if ((err & 0x80000000) == 0x80000000)
            {
                return err;
            }

            return (err & 0x0000FFFF) | unchecked((int)0x80070000);
        }
    }

    internal sealed class SafeCryptMsgHandle : SafeHandle
    {
        internal static SafeCryptMsgHandle InvalidHandle => new SafeCryptMsgHandle(IntPtr.Zero);

        public override bool IsInvalid => handle == IntPtr.Zero;

        private SafeCryptMsgHandle()
            : base(IntPtr.Zero, ownsHandle: true)
        {
        }

        internal SafeCryptMsgHandle(IntPtr handle)
            : base(handle, ownsHandle: true)
        {
        }

        internal SafeCryptMsgHandle(IntPtr handle, bool ownsHandle)
            : base(handle, ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CryptMsgClose(handle);
        }
    }

    internal sealed class SafeLocalAllocHandle : SafeHandle
    {
        public override bool IsInvalid { get { return handle == IntPtr.Zero; } }

        internal SafeLocalAllocHandle(IntPtr handle) : base(handle, ownsHandle: true)
        {
        }

        protected override bool ReleaseHandle()
        {
            if (!IsInvalid)
            {
                Marshal.FreeHGlobal(handle);

                handle = IntPtr.Zero;
            }

            return true;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CRYPT_INTEGER_BLOB
    {
        internal uint cbData;
        internal IntPtr pbData;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct CRYPT_ATTRIBUTES
    {
        internal uint cAttr;
        internal /*CRYPT_ATTRIBUTE*/ IntPtr rgAttr;
    }

    // http://msdn.microsoft.com/en-us/library/windows/desktop/aa381139(v=vs.85).aspx
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CRYPT_ATTRIBUTE
    {
        internal IntPtr pszObjId;
        internal uint cValue;
        internal IntPtr rgValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CRYPT_ATTRIBUTE_STRING
    {
        [MarshalAs(UnmanagedType.LPStr)]
        internal string pszObjId;
        internal uint cValue;
        internal IntPtr rgValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA
    {
        internal uint cbSize;
        internal uint dwSignerIndex;
        internal CRYPT_INTEGER_BLOB BLOB;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA
    {
        internal uint cbSize;
        internal uint dwSignerIndex;
        internal uint dwUnauthAttrIndex;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct CMSG_SIGNER_INFO
    {
        internal uint dwVersion;
        internal Rfc3161TimestampWin32.CRYPTOAPI_BLOB Issuer;
        internal Rfc3161TimestampWin32.CRYPTOAPI_BLOB SerialNumber;
        internal Rfc3161TimestampWin32.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;
        internal Rfc3161TimestampWin32.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;
        internal Rfc3161TimestampWin32.CRYPTOAPI_BLOB EncryptedHash;
        internal CRYPT_ATTRIBUTES AuthAttrs;
        internal CRYPT_ATTRIBUTES UnauthAttrs;
    }

    [Flags]
    internal enum CMSG_OPENTODECODE_FLAGS : uint
    {
        // Source: wincrypt.h
        None = 0,
        CMSG_DETACHED_FLAG = 0x00000004,
        CMSG_CRYPT_RELEASE_CONTEXT_FLAG = 0x00008000
    }

    [Flags]
    internal enum CMSG_ENCODING : uint
    {
        // Source: wincrypt.h
        X509_ASN_ENCODING = 0x00000001,
        PKCS_7_ASN_ENCODING = 0x00010000,
        Any = X509_ASN_ENCODING | PKCS_7_ASN_ENCODING
    }

    internal enum CMSG_CONTROL_TYPE : uint
    {
        CMSG_CTRL_VERIFY_SIGNATURE = 1,
        CMSG_CTRL_DECRYPT = 2,
        CMSG_CTRL_VERIFY_HASH = 5,
        CMSG_CTRL_ADD_SIGNER = 6,
        CMSG_CTRL_DEL_SIGNER = 7,
        CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR = 8,
        CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR = 9,
        CMSG_CTRL_ADD_CERT = 10,
        CMSG_CTRL_DEL_CERT = 11,
        CMSG_CTRL_ADD_CRL = 12,
        CMSG_CTRL_DEL_CRL = 13,
        CMSG_CTRL_ADD_ATTR_CERT = 14,
        CMSG_CTRL_DEL_ATTR_CERT = 15,
        CMSG_CTRL_KEY_TRANS_DECRYPT = 16,
        CMSG_CTRL_KEY_AGREE_DECRYPT = 17,
        CMSG_CTRL_MAIL_LIST_DECRYPT = 18,
        CMSG_CTRL_VERIFY_SIGNATURE_EX = 19,
        CMSG_CTRL_ADD_CMS_SIGNER_INFO = 20,
        CMSG_CTRL_ENABLE_STRONG_SIGNATURE = 21
    }

    internal enum CMSG_GETPARAM_TYPE : uint
    {
        // Source: wincrypt.h
        CMSG_TYPE_PARAM = 1,
        CMSG_CONTENT_PARAM = 2,
        CMSG_BARE_CONTENT_PARAM = 3,
        CMSG_INNER_CONTENT_TYPE_PARAM = 4,
        CMSG_SIGNER_COUNT_PARAM = 5,
        CMSG_SIGNER_INFO_PARAM = 6,
        CMSG_SIGNER_CERT_INFO_PARAM = 7,
        CMSG_SIGNER_HASH_ALGORITHM_PARAM = 8,
        CMSG_SIGNER_AUTH_ATTR_PARAM = 9,
        CMSG_SIGNER_UNAUTH_ATTR_PARAM = 10,
        CMSG_CERT_COUNT_PARAM = 11,
        CMSG_CERT_PARAM = 12,
        CMSG_CRL_COUNT_PARAM = 13,
        CMSG_CRL_PARAM = 14,
        CMSG_ENVELOPE_ALGORITHM_PARAM = 15,
        CMSG_RECIPIENT_COUNT_PARAM = 17,
        CMSG_RECIPIENT_INDEX_PARAM = 18,
        CMSG_RECIPIENT_INFO_PARAM = 19,
        CMSG_HASH_ALGORITHM_PARAM = 20,
        CMSG_HASH_DATA_PARAM = 21,
        CMSG_COMPUTED_HASH_PARAM = 22,
        CMSG_ENCRYPT_PARAM = 26,
        CMSG_ENCRYPTED_DIGEST = 27,
        CMSG_ENCODED_SIGNER = 28,
        CMSG_ENCODED_MESSAGE = 29,
        CMSG_VERSION_PARAM = 30,
        CMSG_ATTR_CERT_COUNT_PARAM = 31,
        CMSG_ATTR_CERT_PARAM = 32,
        CMSG_CMS_RECIPIENT_COUNT_PARAM = 33,
        CMSG_CMS_RECIPIENT_INDEX_PARAM = 34,
        CMSG_CMS_RECIPIENT_ENCRYPTED_KEY_INDEX_PARAM = 35,
        CMSG_CMS_RECIPIENT_INFO_PARAM = 36,
        CMSG_UNPROTECTED_ATTR_PARAM = 37,
        CMSG_SIGNER_CERT_ID_PARAM = 38,
        CMSG_CMS_SIGNER_INFO_PARAM = 39
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CMSG_SIGNER_ENCODE_INFO
    {
        internal uint cbSize;
        internal IntPtr pCertInfo;
        internal IntPtr hCryptProvOrhNCryptKey;
        internal int dwKeySpec;
        internal CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;
        internal IntPtr pvHashAuxInfo;
        internal int cAuthAttr;
        internal IntPtr rgAuthAttr;
        internal int cUnauthAttr;
        internal IntPtr rgUnauthAttr;
        internal CERT_ID SignerId;
        internal CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;
        internal IntPtr pvHashEncryptionAuxInfo;

        public void Dispose()
        {
            if (!hCryptProvOrhNCryptKey.Equals(IntPtr.Zero)) { NativeMethods.CryptReleaseContext(hCryptProvOrhNCryptKey, 0); }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CRYPT_ALGORITHM_IDENTIFIER
    {
        public string pszObjId;
        public CRYPT_INTEGER_BLOB Parameters;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BLOB
    {
        public uint cbData;
        public IntPtr pbData;

        public void Dispose()
        {
            NativeUtility.SafeFree(pbData);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CERT_ID
    {
        internal uint dwIdChoice;
        internal BLOB KeyId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CMSG_SIGNED_ENCODE_INFO
    {
        internal int cbSize;
        internal int cSigners;
        internal IntPtr rgSigners;
        internal int cCertEncoded;
        internal IntPtr rgCertEncoded;
        internal int cCrlEncoded;
        internal IntPtr rgCrlEncoded;
        internal int cAttrCertEncoded;
        internal IntPtr rgAttrCertEncoded;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CERT_CONTEXT
    {
        public uint dwCertEncodingType;
        public IntPtr pbCertEncoded;
        public uint cbCertEncoded;
        public IntPtr pCertInfo;
        public IntPtr hCertStore;
    }
}
