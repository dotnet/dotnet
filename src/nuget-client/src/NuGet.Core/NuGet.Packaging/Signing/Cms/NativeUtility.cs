// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using NuGet.Packaging.Signing.Utility;
#endif

namespace NuGet.Packaging.Signing
{
    internal static class NativeUtility
    {
        internal static void SafeFree(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        internal static void ThrowIfFailed(bool result)
        {
            if (!result)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

#if IS_SIGNING_SUPPORTED
        internal static SignedCms NativeSign(CmsSigner cmsSigner, byte[] data, CngKey privateKey)
        {
            using (var hb = new HeapBlockRetainer())
            {
                var certificateBlobs = new BLOB[cmsSigner.Certificates.Count];

                for (var i = 0; i < cmsSigner.Certificates.Count; ++i)
                {
                    var cert = cmsSigner.Certificates[i];
                    var context = MarshalUtility.PtrToStructure<CERT_CONTEXT>(cert.Handle);

                    certificateBlobs[i] = new BLOB() { cbData = context.cbCertEncoded, pbData = context.pbCertEncoded };
                }

                byte[] encodedData;
                var signerInfo = CreateSignerInfo(cmsSigner, privateKey, hb);

                var signedInfo = new CMSG_SIGNED_ENCODE_INFO();
                signedInfo.cbSize = Marshal.SizeOf(signedInfo);
                signedInfo.cSigners = 1;

                using (var signerInfoHandle = new SafeLocalAllocHandle(Marshal.AllocHGlobal(Marshal.SizeOf(signerInfo))))
                {
                    Marshal.StructureToPtr(signerInfo, signerInfoHandle.DangerousGetHandle(), fDeleteOld: false);

                    signedInfo.rgSigners = signerInfoHandle.DangerousGetHandle();
                    signedInfo.cCertEncoded = certificateBlobs.Length;

                    using (var certificatesHandle = new SafeLocalAllocHandle(Marshal.AllocHGlobal(Marshal.SizeOf(certificateBlobs[0]) * certificateBlobs.Length)))
                    {
                        for (var i = 0; i < certificateBlobs.Length; ++i)
                        {
                            Marshal.StructureToPtr(certificateBlobs[i], new IntPtr(certificatesHandle.DangerousGetHandle().ToInt64() + Marshal.SizeOf(certificateBlobs[i]) * i), fDeleteOld: false);
                        }

                        signedInfo.rgCertEncoded = certificatesHandle.DangerousGetHandle();

                        var hMsg = NativeMethods.CryptMsgOpenToEncode(
                            CMSG_ENCODING.Any,
                            dwFlags: 0,
                            dwMsgType: NativeMethods.CMSG_SIGNED,
                            pvMsgEncodeInfo: ref signedInfo,
                            pszInnerContentObjID: null,
                            pStreamInfo: IntPtr.Zero);

                        ThrowIfFailed(!hMsg.IsInvalid);

                        ThrowIfFailed(NativeMethods.CryptMsgUpdate(
                            hMsg,
                            data,
                            (uint)data.Length,
                            fFinal: true));

                        uint valueLength = 0;

                        ThrowIfFailed(NativeMethods.CryptMsgGetParam(
                            hMsg,
                            CMSG_GETPARAM_TYPE.CMSG_CONTENT_PARAM,
                            dwIndex: 0,
                            pvData: null,
                            pcbData: ref valueLength));

                        encodedData = new byte[(int)valueLength];

                        ThrowIfFailed(NativeMethods.CryptMsgGetParam(
                            hMsg,
                            CMSG_GETPARAM_TYPE.CMSG_CONTENT_PARAM,
                            dwIndex: 0,
                            pvData: encodedData,
                            pcbData: ref valueLength));
                    }
                }

                var cms = new SignedCms();

                cms.Decode(encodedData);

                return cms;
            }
        }

        internal unsafe static CMSG_SIGNER_ENCODE_INFO CreateSignerInfo(
            CmsSigner cmsSigner,
            CngKey privateKey,
            HeapBlockRetainer hb)
        {
            var signerInfo = new CMSG_SIGNER_ENCODE_INFO();

            signerInfo.cbSize = (uint)Marshal.SizeOf(signerInfo);
            signerInfo.pCertInfo = MarshalUtility.PtrToStructure<CERT_CONTEXT>(cmsSigner.Certificate.Handle).pCertInfo;
            signerInfo.hCryptProvOrhNCryptKey = privateKey.Handle.DangerousGetHandle();
            signerInfo.HashAlgorithm.pszObjId = cmsSigner.DigestAlgorithm.Value;

            if (cmsSigner.SignerIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier)
            {
                var certContextHandle = IntPtr.Zero;

                try
                {
                    certContextHandle = NativeMethods.CertDuplicateCertificateContext(cmsSigner.Certificate.Handle);

                    uint cbData = 0;
                    var pbData = IntPtr.Zero;

                    ThrowIfFailed(NativeMethods.CertGetCertificateContextProperty(
                        certContextHandle,
                        NativeMethods.CERT_KEY_IDENTIFIER_PROP_ID,
                        pbData,
                        ref cbData));

                    if (cbData > 0)
                    {
                        pbData = hb.Alloc((int)cbData);

                        ThrowIfFailed(NativeMethods.CertGetCertificateContextProperty(
                            certContextHandle,
                            NativeMethods.CERT_KEY_IDENTIFIER_PROP_ID,
                            pbData,
                            ref cbData));

                        signerInfo.SignerId.dwIdChoice = NativeMethods.CERT_ID_KEY_IDENTIFIER;
                        signerInfo.SignerId.KeyId.cbData = cbData;
                        signerInfo.SignerId.KeyId.pbData = pbData;
                    }
                }
                finally
                {
                    if (certContextHandle != IntPtr.Zero)
                    {
                        NativeMethods.CertFreeCertificateContext(certContextHandle);
                    }
                }
            }

            if (cmsSigner.SignedAttributes.Count != 0)
            {
                signerInfo.cAuthAttr = cmsSigner.SignedAttributes.Count;

                checked
                {
                    int sizeOfCryptAttribute = MarshalUtility.SizeOf<CRYPT_ATTRIBUTE>();
                    int sizeOfCryptIntegerBlob = MarshalUtility.SizeOf<CRYPT_INTEGER_BLOB>();
                    var attributesArray = (CRYPT_ATTRIBUTE*)hb.Alloc(sizeOfCryptAttribute * cmsSigner.SignedAttributes.Count);
                    var currentAttribute = attributesArray;

                    foreach (var attribute in cmsSigner.SignedAttributes)
                    {
                        currentAttribute->pszObjId = hb.AllocAsciiString(attribute.Oid.Value);
                        currentAttribute->cValue = (uint)attribute.Values.Count;
                        currentAttribute->rgValue = hb.Alloc(sizeOfCryptIntegerBlob);

                        foreach (var value in attribute.Values)
                        {
                            var attrData = value.RawData;

                            if (attrData.Length > 0)
                            {
                                var blob = (CRYPT_INTEGER_BLOB*)currentAttribute->rgValue;

                                blob->cbData = (uint)attrData.Length;
                                blob->pbData = hb.Alloc(value.RawData.Length);

                                Marshal.Copy(attrData, 0, blob->pbData, attrData.Length);
                            }
                        }

                        currentAttribute++;
                    }

                    signerInfo.rgAuthAttr = new IntPtr(attributesArray);
                }
            }

            return signerInfo;
        }
#endif
    }
}
