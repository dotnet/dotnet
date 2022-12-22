// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
#endif
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing.Utility;

namespace NuGet.Packaging.Signing
{
    internal class NativeCms : IDisposable
    {
        private readonly SafeCryptMsgHandle _handle;

        private struct RepositoryCounterSignerInfo
        {
            public uint dwUnauthAttrIndex;
            public CRYPT_ATTRIBUTE_STRING UnauthAttr;
            public CMSG_SIGNER_INFO SignerInfo;
        }

        private NativeCms(SafeCryptMsgHandle handle)
        {
            _handle = handle;
        }

        internal byte[] GetPrimarySignatureSignatureValue()
        {
            return GetByteArrayAttribute(CMSG_GETPARAM_TYPE.CMSG_ENCRYPTED_DIGEST, index: 0);
        }

        private byte[] GetByteArrayAttribute(CMSG_GETPARAM_TYPE param, uint index)
        {
            uint valueLength = 0;

            NativeUtility.ThrowIfFailed(NativeMethods.CryptMsgGetParam(
                _handle,
                param,
                index,
                null,
                ref valueLength));

            var data = new byte[(int)valueLength];

            NativeUtility.ThrowIfFailed(NativeMethods.CryptMsgGetParam(
                _handle,
                param,
                index,
                data,
                ref valueLength));

            return data;
        }

        internal byte[] GetRepositoryCountersignatureSignatureValue()
        {
            using (var retainer = new HeapBlockRetainer())
            {
                var repositoryCountersignature = GetRepositoryCountersignature(retainer);

                if (repositoryCountersignature == null)
                {
                    return null;
                }

                var countersignatureSignatureValue = new byte[repositoryCountersignature.Value.SignerInfo.EncryptedHash.cbData];

                Marshal.Copy(
                    repositoryCountersignature.Value.SignerInfo.EncryptedHash.pbData,
                    countersignatureSignatureValue,
                    startIndex: 0,
                    length: countersignatureSignatureValue.Length);

                return countersignatureSignatureValue;
            }
        }

        private unsafe RepositoryCounterSignerInfo? GetRepositoryCountersignature(HeapBlockRetainer retainer)
        {
            const uint primarySignerInfoIndex = 0;
            uint unsignedAttributeCount = 0;
            var pointer = IntPtr.Zero;

            NativeUtility.ThrowIfFailed(NativeMethods.CryptMsgGetParam(
                _handle,
                CMSG_GETPARAM_TYPE.CMSG_SIGNER_UNAUTH_ATTR_PARAM,
                primarySignerInfoIndex,
                pointer,
                ref unsignedAttributeCount));

            if (unsignedAttributeCount == 0)
            {
                return null;
            }

            pointer = retainer.Alloc((int)unsignedAttributeCount);

            NativeUtility.ThrowIfFailed(NativeMethods.CryptMsgGetParam(
                _handle,
                CMSG_GETPARAM_TYPE.CMSG_SIGNER_UNAUTH_ATTR_PARAM,
                primarySignerInfoIndex,
                pointer,
                ref unsignedAttributeCount));

            var unsignedAttributes = MarshalUtility.PtrToStructure<CRYPT_ATTRIBUTES>(pointer);
            int sizeOfCryptAttributeString = MarshalUtility.SizeOf<CRYPT_ATTRIBUTE_STRING>();
            int sizeOfCryptIntegerBlob = MarshalUtility.SizeOf<CRYPT_INTEGER_BLOB>();

            for (uint i = 0; i < unsignedAttributes.cAttr; ++i)
            {

                var attributePointer = new IntPtr(
                    (long)unsignedAttributes.rgAttr + (i * sizeOfCryptAttributeString));
                var attribute = MarshalUtility.PtrToStructure<CRYPT_ATTRIBUTE_STRING>(attributePointer);

                if (!string.Equals(attribute.pszObjId, Oids.Countersignature, StringComparison.Ordinal))
                {
                    continue;
                }

                for (var j = 0; j < attribute.cValue; ++j)
                {
                    var attributeValuePointer = new IntPtr(
                        (long)attribute.rgValue + (j * sizeOfCryptIntegerBlob));
                    var attributeValue = MarshalUtility.PtrToStructure<CRYPT_INTEGER_BLOB>(attributeValuePointer);
                    uint cbSignerInfo = 0;

                    NativeUtility.ThrowIfFailed(NativeMethods.CryptDecodeObject(
                        CMSG_ENCODING.Any,
                        new IntPtr(NativeMethods.PKCS7_SIGNER_INFO),
                        attributeValue.pbData,
                        attributeValue.cbData,
                        dwFlags: 0,
                        pvStructInfo: IntPtr.Zero,
                        pcbStructInfo: new IntPtr(&cbSignerInfo)));

                    var counterSignerInfoPointer = retainer.Alloc((int)cbSignerInfo);

                    NativeUtility.ThrowIfFailed(NativeMethods.CryptDecodeObject(
                        CMSG_ENCODING.Any,
                        new IntPtr(NativeMethods.PKCS7_SIGNER_INFO),
                        attributeValue.pbData,
                        attributeValue.cbData,
                        dwFlags: 0,
                        pvStructInfo: counterSignerInfoPointer,
                        pcbStructInfo: new IntPtr(&cbSignerInfo)));

                    var counterSignerInfo = MarshalUtility.PtrToStructure<CMSG_SIGNER_INFO>(counterSignerInfoPointer);

                    if (IsRepositoryCounterSignerInfo(counterSignerInfo))
                    {
                        return new RepositoryCounterSignerInfo()
                        {
                            dwUnauthAttrIndex = i,
                            UnauthAttr = attribute,
                            SignerInfo = counterSignerInfo
                        };
                    }
                }
            }

            return null;
        }

        private static bool IsRepositoryCounterSignerInfo(CMSG_SIGNER_INFO counterSignerInfo)
        {
            var signedAttributes = counterSignerInfo.AuthAttrs;
            int sizeOfCryptAttributeString = MarshalUtility.SizeOf<CRYPT_ATTRIBUTE_STRING>();
            for (var i = 0; i < signedAttributes.cAttr; ++i)
            {
                var signedAttributePointer = new IntPtr(
                    (long)signedAttributes.rgAttr + (i * sizeOfCryptAttributeString));
                var signedAttribute = MarshalUtility.PtrToStructure<CRYPT_ATTRIBUTE_STRING>(signedAttributePointer);
                if (string.Equals(signedAttribute.pszObjId, Oids.CommitmentTypeIndication, StringComparison.Ordinal) &&
                    IsRepositoryCounterSignerInfo(signedAttribute))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsRepositoryCounterSignerInfo(CRYPT_ATTRIBUTE_STRING commitmentTypeIndicationAttribute)
        {
            int sizeOfCryptIntegerBlob = MarshalUtility.SizeOf<CRYPT_INTEGER_BLOB>();

            for (var i = 0; i < commitmentTypeIndicationAttribute.cValue; ++i)
            {
                var attributeValuePointer = new IntPtr(
                    (long)commitmentTypeIndicationAttribute.rgValue + (i * sizeOfCryptIntegerBlob));
                var attributeValue = MarshalUtility.PtrToStructure<CRYPT_INTEGER_BLOB>(attributeValuePointer);
                var bytes = new byte[attributeValue.cbData];

                Marshal.Copy(attributeValue.pbData, bytes, startIndex: 0, length: bytes.Length);

                var commitmentTypeIndication = CommitmentTypeIndication.Read(bytes);

                if (string.Equals(
                    commitmentTypeIndication.CommitmentTypeId.Value,
                    Oids.CommitmentTypeIdentifierProofOfReceipt,
                    StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        internal static NativeCms Decode(byte[] input)
        {
            var handle = NativeMethods.CryptMsgOpenToDecode(
                CMSG_ENCODING.Any,
                CMSG_OPENTODECODE_FLAGS.None,
                0U,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);

            if (handle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            if (!NativeMethods.CryptMsgUpdate(handle, input, (uint)input.Length, fFinal: true))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return new NativeCms(handle);
        }

        internal void AddCertificates(IEnumerable<X509Certificate2> certificates)
        {
            foreach (var cert in certificates)
            {
                byte[] encodedCert = cert.RawData;
                using (var hb = new HeapBlockRetainer())
                {
                    var unmanagedCert = hb.Alloc(encodedCert.Length);
                    Marshal.Copy(encodedCert, 0, unmanagedCert, encodedCert.Length);
                    var blob = new CRYPT_INTEGER_BLOB()
                    {
                        cbData = (uint)encodedCert.Length,
                        pbData = unmanagedCert
                    };

                    var unmanagedBlob = hb.Alloc(Marshal.SizeOf(blob));
                    Marshal.StructureToPtr(blob, unmanagedBlob, fDeleteOld: false);

                    if (!NativeMethods.CryptMsgControl(
                        _handle,
                        dwFlags: 0,
                        dwCtrlType: CMSG_CONTROL_TYPE.CMSG_CTRL_ADD_CERT,
                        pvCtrlPara: unmanagedBlob))
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }
                }
            }
        }

#if IS_SIGNING_SUPPORTED
        internal unsafe void AddCountersignature(CmsSigner cmsSigner, CngKey privateKey)
        {
            using (var hb = new HeapBlockRetainer())
            {
                var signerInfo = NativeUtility.CreateSignerInfo(cmsSigner, privateKey, hb);

                NativeUtility.ThrowIfFailed(NativeMethods.CryptMsgCountersign(
                    _handle,
                    dwIndex: 0,
                    cCountersigners: 1,
                    rgCountersigners: signerInfo));

                AddCertificates(cmsSigner.Certificates.OfType<X509Certificate2>());
            }
        }

        internal unsafe void AddTimestampToRepositoryCountersignature(SignedCms timestamp)
        {
            using (var hb = new HeapBlockRetainer())
            {
                var repositoryCountersignature = GetRepositoryCountersignature(hb);
                if (repositoryCountersignature == null)
                {
                    throw new SignatureException(Strings.Error_NotOneRepositoryCounterSignature);
                }

                // Remove repository countersignature from message
                var countersignatureDelAttr = new CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA()
                {
                    dwSignerIndex = 0,
                    dwUnauthAttrIndex = repositoryCountersignature.Value.dwUnauthAttrIndex
                };

                countersignatureDelAttr.cbSize = (uint)Marshal.SizeOf(countersignatureDelAttr);
                var unmanagedCountersignatureDelAttr = hb.Alloc(Marshal.SizeOf(countersignatureDelAttr));
                Marshal.StructureToPtr(countersignatureDelAttr, unmanagedCountersignatureDelAttr, fDeleteOld: false);

                if (!NativeMethods.CryptMsgControl(
                    _handle,
                    dwFlags: 0,
                    dwCtrlType: CMSG_CONTROL_TYPE.CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR,
                    pvCtrlPara: unmanagedCountersignatureDelAttr))
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                // Add timestamp attribute to existing unsigned attributes
                var signerInfo = repositoryCountersignature.Value.SignerInfo;
                var unauthAttrCount = signerInfo.UnauthAttrs.cAttr + 1;

                var sizeOfCryptAttribute = MarshalUtility.SizeOf<CRYPT_ATTRIBUTE>();
                var attributesArray = (CRYPT_ATTRIBUTE*)hb.Alloc((int)(sizeOfCryptAttribute * unauthAttrCount));
                var currentAttribute = attributesArray;

                // Copy existing unsigned attributes
                for (var i = 0; i < unauthAttrCount - 1; ++i)
                {
                    var existingAttributePointer = new IntPtr(
                         (long)signerInfo.UnauthAttrs.rgAttr + (i * sizeOfCryptAttribute));
                    var existingAttribute = MarshalUtility.PtrToStructure<CRYPT_ATTRIBUTE>(existingAttributePointer);

                    currentAttribute->pszObjId = existingAttribute.pszObjId;
                    currentAttribute->cValue = existingAttribute.cValue;
                    currentAttribute->rgValue = existingAttribute.rgValue;

                    currentAttribute++;
                }

                // Add timestamp attribute
                *currentAttribute = GetCryptAttributeForData(timestamp.Encode(), Oids.SignatureTimeStampTokenAttribute, hb);

                signerInfo.UnauthAttrs = new CRYPT_ATTRIBUTES()
                {
                    cAttr = unauthAttrCount,
                    rgAttr = new IntPtr(attributesArray)
                };

                // Encode signer info
                var unmanagedSignerInfo = hb.Alloc(Marshal.SizeOf(signerInfo));
                Marshal.StructureToPtr(signerInfo, unmanagedSignerInfo, fDeleteOld: false);

                uint encodedLength = 0;
                if (!NativeMethods.CryptEncodeObjectEx(
                    CMSG_ENCODING.Any,
                    lpszStructType: new IntPtr(NativeMethods.PKCS7_SIGNER_INFO),
                    pvStructInfo: unmanagedSignerInfo,
                    dwFlags: 0,
                    pEncodePara: IntPtr.Zero,
                    pvEncoded: IntPtr.Zero,
                    pcbEncoded: ref encodedLength))
                {
                    var err = Marshal.GetLastWin32Error();
                    if (err != NativeMethods.ERROR_MORE_DATA)
                    {
                        Marshal.ThrowExceptionForHR(NativeMethods.GetHRForWin32Error(err));
                    }
                }

                var unmanagedEncoded = hb.Alloc((int)encodedLength);
                if (!NativeMethods.CryptEncodeObjectEx(
                    CMSG_ENCODING.Any,
                    lpszStructType: new IntPtr(NativeMethods.PKCS7_SIGNER_INFO),
                    pvStructInfo: unmanagedSignerInfo,
                    dwFlags: 0,
                    pEncodePara: IntPtr.Zero,
                    pvEncoded: unmanagedEncoded,
                    pcbEncoded: ref encodedLength))
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                var encondedSignerBlob = new CRYPT_INTEGER_BLOB()
                {
                    cbData = encodedLength,
                    pbData = unmanagedEncoded
                };

                var unmanagedBlob = hb.Alloc(Marshal.SizeOf(encondedSignerBlob));
                Marshal.StructureToPtr(encondedSignerBlob, unmanagedBlob, fDeleteOld: false);

                var signerInfoAttr = new CRYPT_ATTRIBUTE()
                {
                    pszObjId = hb.AllocAsciiString(Oids.Countersignature),
                    cValue = 1,
                    rgValue = unmanagedBlob
                };

                // Create add unauth for signer info
                var signerInfoAddAttr = CreateUnsignedAddAttribute(signerInfoAttr, hb);

                // Add repository countersignature back to message
                signerInfoAddAttr.cbSize = (uint)Marshal.SizeOf(signerInfoAddAttr);
                var unmanagedSignerInfoAddAttr = hb.Alloc(Marshal.SizeOf(signerInfoAddAttr));

                Marshal.StructureToPtr(signerInfoAddAttr, unmanagedSignerInfoAddAttr, fDeleteOld: false);

                if (!NativeMethods.CryptMsgControl(
                    _handle,
                    dwFlags: 0,
                    dwCtrlType: CMSG_CONTROL_TYPE.CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR,
                    pvCtrlPara: unmanagedSignerInfoAddAttr))
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }
        }

        internal unsafe void AddTimestamp(SignedCms timestamp)
        {
            using (var hb = new HeapBlockRetainer())
            {
                var timestampAttr = GetCryptAttributeForData(timestamp.Encode(), Oids.SignatureTimeStampTokenAttribute, hb);
                var timestampAddAttr = CreateUnsignedAddAttribute(timestampAttr, hb);

                timestampAddAttr.cbSize = (uint)Marshal.SizeOf(timestampAddAttr);
                var unmanagedTimestampAddAttr = hb.Alloc(Marshal.SizeOf(timestampAddAttr));
                Marshal.StructureToPtr(timestampAddAttr, unmanagedTimestampAddAttr, fDeleteOld: false);

                if (!NativeMethods.CryptMsgControl(
                    _handle,
                    dwFlags: 0,
                    dwCtrlType: CMSG_CONTROL_TYPE.CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR,
                    pvCtrlPara: unmanagedTimestampAddAttr))
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }
        }
#endif

        private static CRYPT_ATTRIBUTE GetCryptAttributeForData(byte[] data, string attributeOid, HeapBlockRetainer hb)
        {
            var unmanagedData = hb.Alloc(data.Length);
            Marshal.Copy(data, 0, unmanagedData, data.Length);
            var blob = new CRYPT_INTEGER_BLOB()
            {
                cbData = (uint)data.Length,
                pbData = unmanagedData
            };

            var unmanagedBlob = hb.Alloc(Marshal.SizeOf(blob));
            Marshal.StructureToPtr(blob, unmanagedBlob, fDeleteOld: false);

            var attr = new CRYPT_ATTRIBUTE()
            {
                pszObjId = hb.AllocAsciiString(attributeOid),
                cValue = 1,
                rgValue = unmanagedBlob
            };

            return attr;
        }

        private static unsafe CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA CreateUnsignedAddAttribute(CRYPT_ATTRIBUTE attr, HeapBlockRetainer hb)
        {
            var unmanagedAttr = hb.Alloc(Marshal.SizeOf(attr));
            Marshal.StructureToPtr(attr, unmanagedAttr, fDeleteOld: false);

            uint encodedLength = 0;
            if (!NativeMethods.CryptEncodeObjectEx(
                CMSG_ENCODING.Any,
                lpszStructType: new IntPtr(NativeMethods.PKCS_ATTRIBUTE),
                pvStructInfo: unmanagedAttr,
                dwFlags: 0,
                pEncodePara: IntPtr.Zero,
                pvEncoded: IntPtr.Zero,
                pcbEncoded: ref encodedLength))
            {
                var err = Marshal.GetLastWin32Error();
                if (err != NativeMethods.ERROR_MORE_DATA)
                {
                    Marshal.ThrowExceptionForHR(NativeMethods.GetHRForWin32Error(err));
                }
            }

            var unmanagedEncoded = hb.Alloc((int)encodedLength);
            if (!NativeMethods.CryptEncodeObjectEx(
                CMSG_ENCODING.Any,
                lpszStructType: new IntPtr(NativeMethods.PKCS_ATTRIBUTE),
                pvStructInfo: unmanagedAttr,
                dwFlags: 0,
                pEncodePara: IntPtr.Zero,
                pvEncoded: unmanagedEncoded,
                pcbEncoded: ref encodedLength))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            var addAttr = new CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA()
            {
                dwSignerIndex = 0,
                BLOB = new CRYPT_INTEGER_BLOB()
                {
                    cbData = encodedLength,
                    pbData = unmanagedEncoded
                }
            };
            return addAttr;
        }

        internal byte[] Encode()
        {
            return GetByteArrayAttribute(CMSG_GETPARAM_TYPE.CMSG_ENCODED_MESSAGE, index: 0);
        }

        public void Dispose()
        {
            if (!_handle.IsInvalid)
            {
                _handle.Dispose();
            }
        }
    }
}
