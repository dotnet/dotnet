// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if IS_DESKTOP
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Class representing a Rfc3161TimestampRequest.
    /// This class should be removed once we can reference it throught the .NET Core framework.
    /// </summary>
    internal sealed class Rfc3161TimestampRequest : AsnEncodedData
    {
        private class DataType
        {
            internal int _version;
            internal byte[] _hash;
            internal Oid _hashAlgorithm;
            internal Oid _requestedPolicyId;
            internal byte[] _nonce;
            internal bool _requestSignerCertificate;
            internal X509ExtensionCollection _extensions;
        }

        private DataType _data;

        private DataType Data
        {
            get
            {
                if (_data == null)
                    _data = Decode(RawData);

                return _data;
            }
        }

        public Rfc3161TimestampRequest(byte[] encodedRequest)
            : base(encodedRequest)
        {
        }

        public Rfc3161TimestampRequest(
            byte[] messageHash,
            HashAlgorithmName hashAlgorithm,
            Oid requestedPolicyId = null,
            byte[] nonce = null,
            bool requestSignerCertificates = false,
            X509ExtensionCollection extensions = null)
        {
            if (messageHash == null)
                throw new ArgumentNullException(nameof(messageHash));

            int expectedSize;
            string algorithmIdentifier;

            if (!ResolveAlgorithm(hashAlgorithm, out expectedSize, out algorithmIdentifier))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hashAlgorithm),
                    hashAlgorithm,
                    "Hash algorithm is not supported by this method");
            }

            if (messageHash.Length != expectedSize)
            {
                throw new ArgumentException("Hash is not the correct size for the identified algorithm", nameof(messageHash));
            }

            if (requestedPolicyId != null && !Rfc3161TimestampUtils.IsLegalOid(requestedPolicyId.Value))
            {
                throw new ArgumentException("Value is not a legal object identifier", nameof(requestedPolicyId));
            }

            if (nonce != null && nonce.Length == 0)
            {
                throw new ArgumentException("Nonce must be null or non-empty", nameof(nonce));
            }

            var data = new DataType
            {
                _version = 1,
                _hash = (byte[])messageHash.Clone(),
                _hashAlgorithm = OpportunisticOid(algorithmIdentifier),
                _nonce = (byte[])nonce?.Clone(),
                _requestSignerCertificate = requestSignerCertificates,
                _extensions = Rfc3161TimestampTokenInfo.ShallowCopy(extensions, preserveNull: true),
            };

            if (requestedPolicyId != null)
            {
                data._requestedPolicyId = new Oid(requestedPolicyId.Value, requestedPolicyId.FriendlyName);
            }

            RawData = Encode(data);
        }

        private static Oid OpportunisticOid(string oidValue, OidGroup group = OidGroup.HashAlgorithm)
        {
            if (oidValue == null)
                return null;

            try
            {
                return Oid.FromOidValue(oidValue, group);
            }
            catch (CryptographicException)
            {
                return new Oid(oidValue, oidValue);
            }
        }

        public Rfc3161TimestampRequest(
            byte[] messageHash,
            Oid hashAlgorithmId,
            Oid requestedPolicyId = null,
            byte[] nonce = null,
            bool requestSignerCertificates = false,
            X509ExtensionCollection extensions = null)
        {
            if (messageHash == null)
                throw new ArgumentNullException(nameof(messageHash));
            if (hashAlgorithmId == null)
                throw new ArgumentNullException(nameof(hashAlgorithmId));
            if (!Rfc3161TimestampUtils.IsLegalOid(hashAlgorithmId.Value))
                throw new ArgumentException("Value is not a legal object identifier", nameof(hashAlgorithmId));

            if (requestedPolicyId != null && !Rfc3161TimestampUtils.IsLegalOid(requestedPolicyId.Value))
            {
                throw new ArgumentException("Value is not a legal object identifier", nameof(requestedPolicyId));
            }

            if (nonce != null && nonce.Length == 0)
            {
                throw new ArgumentException("Nonce must be null or non-empty", nameof(nonce));
            }

            DataType data = new DataType
            {
                _version = 1,
                _hash = (byte[])messageHash.Clone(),
                _hashAlgorithm = new Oid(hashAlgorithmId.Value, hashAlgorithmId.FriendlyName),
                _nonce = (byte[])nonce?.Clone(),
                _requestSignerCertificate = requestSignerCertificates,
                _extensions = Rfc3161TimestampTokenInfo.ShallowCopy(extensions, preserveNull: true),
            };

            if (requestedPolicyId != null)
            {
                data._requestedPolicyId = new Oid(requestedPolicyId.Value, requestedPolicyId.FriendlyName);
            }

            _data = data;
            RawData = Encode(data);
        }

        public int Version => Data._version;

        public byte[] GetMessageHash() => (byte[])Data._hash.Clone();

        public Oid HashAlgorithmId => new Oid(Data._hashAlgorithm.Value, Data._hashAlgorithm.FriendlyName);

        public Oid RequestedPolicyId => new Oid(Data._requestedPolicyId.Value, Data._requestedPolicyId.FriendlyName);

        public byte[] GetNonce() => (byte[])Data._nonce?.Clone();

        public bool RequestSignerCertificate => Data._requestSignerCertificate;

        public bool HasExtensions => Data._extensions?.Count > 0;

        public X509ExtensionCollection GetExtensions() =>
            Rfc3161TimestampTokenInfo.ShallowCopy(Data._extensions, preserveNull: false);

        public unsafe IRfc3161TimestampToken SubmitRequest(Uri timestampUri, TimeSpan timeout)
        {
            if (timestampUri == null)
                throw new ArgumentNullException(nameof(timestampUri));
            if (!timestampUri.IsAbsoluteUri)
                throw new ArgumentException("Absolute URI required", nameof(timestampUri));
            if (timestampUri.Scheme != Uri.UriSchemeHttp && timestampUri.Scheme != Uri.UriSchemeHttps)
                throw new ArgumentException("HTTP/HTTPS required", nameof(timestampUri));

            IntPtr requestedPolicyPtr = IntPtr.Zero;
            IntPtr pTsContext = IntPtr.Zero;
            IntPtr pTsSigner = IntPtr.Zero;
            IntPtr hStore = IntPtr.Zero;

            const Rfc3161TimestampWin32.CryptRetrieveTimeStampFlags flags =
                Rfc3161TimestampWin32.CryptRetrieveTimeStampFlags.TIMESTAMP_VERIFY_CONTEXT_SIGNATURE |
                Rfc3161TimestampWin32.CryptRetrieveTimeStampFlags.TIMESTAMP_DONT_HASH_DATA;

            try
            {
                requestedPolicyPtr = Marshal.StringToHGlobalAnsi(Data._requestedPolicyId?.Value);

                Rfc3161TimestampWin32.CRYPT_TIMESTAMP_PARA para = new Rfc3161TimestampWin32.CRYPT_TIMESTAMP_PARA()
                {
                    fRequestCerts = Data._requestSignerCertificate,
                    pszTSAPolicyId = requestedPolicyPtr,
                };

                if (Data._extensions?.Count > 0)
                    throw new NotImplementedException();

                fixed (byte* pbNonce = Data._nonce)
                {
                    if (Data._nonce != null)
                    {
                        para.Nonce.cbData = (uint)Data._nonce.Length;
                        para.Nonce.pbData = (IntPtr)pbNonce;
                    }

                    if (!Rfc3161TimestampWin32.CryptRetrieveTimeStamp(
                        timestampUri.AbsoluteUri,
                        flags,
                        (int)timeout.TotalMilliseconds,
                        Data._hashAlgorithm.Value,
                        ref para,
                        Data._hash,
                        Data._hash.Length,
                        ref pTsContext,
                        ref pTsSigner,
                        ref hStore))
                    {
                        throw new CryptographicException(Marshal.GetLastWin32Error());
                    }
                }

                var content = (Rfc3161TimestampWin32.CRYPT_TIMESTAMP_CONTEXT)Marshal.PtrToStructure(pTsContext, typeof(Rfc3161TimestampWin32.CRYPT_TIMESTAMP_CONTEXT));
                byte[] encoded = new byte[content.cbEncoded];
                Marshal.Copy(content.pbEncoded, encoded, 0, content.cbEncoded);

                var tstInfo = new Rfc3161TimestampTokenInfoNet472Wrapper(new Rfc3161TimestampTokenInfo(pTsContext));
                X509Certificate2 signerCert = new X509Certificate2(pTsSigner);

                using (X509Store extraCerts = new X509Store(hStore))
                {
                    X509Certificate2Collection additionalCertsColl = new X509Certificate2Collection();

                    foreach (var cert in extraCerts.Certificates)
                    {
                        if (!signerCert.Equals(cert))
                        {
                            additionalCertsColl.Add(cert);
                        }
                    }

                    return Rfc3161TimestampTokenFactory.Create(
                        tstInfo,
                        signerCert,
                        additionalCertsColl,
                        encoded);
                }
            }
            finally
            {
                if (hStore != IntPtr.Zero)
                    Rfc3161TimestampWin32.CertCloseStore(hStore, 0);

                if (pTsSigner != IntPtr.Zero)
                    Rfc3161TimestampWin32.CertFreeCertificateContext(pTsSigner);

                if (pTsContext != IntPtr.Zero)
                    Rfc3161TimestampWin32.CryptMemFree(pTsContext);

                if (requestedPolicyPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(requestedPolicyPtr);
            }
        }

        private static unsafe byte[] Encode(DataType data)
        {
            IntPtr algorithmOidPtr = IntPtr.Zero;
            IntPtr policyOidPtr = IntPtr.Zero;
            IntPtr encodedDataPtr = IntPtr.Zero;

            try
            {
                algorithmOidPtr = Marshal.StringToHGlobalAnsi(data._hashAlgorithm.Value);
                policyOidPtr = Marshal.StringToHGlobalAnsi(data._requestedPolicyId?.Value);

                Rfc3161TimestampWin32.CRYPT_TIMESTAMP_REQUEST request = new Rfc3161TimestampWin32.CRYPT_TIMESTAMP_REQUEST
                {
                    dwVersion = data._version,
                    fCertReq = data._requestSignerCertificate,
                    pszTSAPolicyId = policyOidPtr,
                };

                request.HashAlgorithm.pszOid = algorithmOidPtr;
                request.HashedMessage.cbData = (uint)data._hash.Length;
                request.Nonce.cbData = (uint)(data._nonce?.Length ?? 0);

                fixed (byte* hashPtr = data._hash)
                fixed (byte* noncePtr = data._nonce)
                {
                    request.HashedMessage.pbData = (IntPtr)hashPtr;
                    request.Nonce.pbData = (IntPtr)noncePtr;

                    uint cbEncoded = 0;

                    if (!Rfc3161TimestampWin32.CryptEncodeObjectEx(
                        Rfc3161TimestampWin32.CryptEncodingTypes.X509_ASN_ENCODING,
                        Rfc3161TimestampWin32.TIMESTAMP_REQUEST,
                        (IntPtr)(&request),
                        Rfc3161TimestampWin32.CryptEncodeObjectFlags.CRYPT_ENCODE_ALLOC_FLAG,
                        IntPtr.Zero,
                        (IntPtr)(&encodedDataPtr),
                        ref cbEncoded))
                    {
                        throw new CryptographicException(Marshal.GetLastWin32Error());
                    }

                    byte[] encoded = new byte[cbEncoded];
                    Marshal.Copy(encodedDataPtr, encoded, 0, (int)cbEncoded);
                    return encoded;
                }
            }
            finally
            {
                if (algorithmOidPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(algorithmOidPtr);

                if (policyOidPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(policyOidPtr);

                if (encodedDataPtr != IntPtr.Zero)
                    Rfc3161TimestampWin32.LocalFree(encodedDataPtr);
            }
        }

        private static unsafe DataType Decode(byte[] rawData)
        {
            fixed (byte* pbData = rawData)
            {
                IntPtr decodedPtr = IntPtr.Zero;
                int cbStruct = 0;

                try
                {
                    if (!Rfc3161TimestampWin32.CryptDecodeObjectEx(
                        Rfc3161TimestampWin32.CryptEncodingTypes.X509_ASN_ENCODING,
                        Rfc3161TimestampWin32.TIMESTAMP_REQUEST,
                        (IntPtr)pbData,
                        rawData.Length,
                        Rfc3161TimestampWin32.CryptDecodeObjectFlags.CRYPT_DECODE_ALLOC_FLAG |
                            Rfc3161TimestampWin32.CryptDecodeObjectFlags.CRYPT_DECODE_NOCOPY_FLAG |
                            Rfc3161TimestampWin32.CryptDecodeObjectFlags.CRYPT_DECODE_NO_SIGNATURE_BYTE_REVERSAL_FLAG,
                        IntPtr.Zero,
                        (IntPtr)(&decodedPtr),
                        ref cbStruct))
                    {
                        throw new CryptographicException(Marshal.GetLastWin32Error());
                    }

                    var request = (Rfc3161TimestampWin32.CRYPT_TIMESTAMP_REQUEST)Marshal.PtrToStructure(decodedPtr, typeof(Rfc3161TimestampWin32.CRYPT_TIMESTAMP_REQUEST));

                    DataType dataType = new DataType
                    {
                        _version = request.dwVersion,
                        _hashAlgorithm = OpportunisticOid(Marshal.PtrToStringAnsi(request.HashAlgorithm.pszOid)),
                        _requestedPolicyId = OpportunisticOid(Marshal.PtrToStringAnsi(request.pszTSAPolicyId), OidGroup.Policy),
                        _hash = Rfc3161TimestampTokenInfo.CopyFromNative(ref request.HashedMessage),
                        _nonce = Rfc3161TimestampTokenInfo.CopyFromNative(ref request.Nonce),
                        _requestSignerCertificate = request.fCertReq
                    };

                    if (request.cExtension != 0)
                        throw new NotImplementedException();

                    return dataType;
                }
                finally
                {
                    if (decodedPtr != IntPtr.Zero)
                        Rfc3161TimestampWin32.LocalFree(decodedPtr);
                }
            }
        }

        private static bool ResolveAlgorithm(
            HashAlgorithmName hashAlgorithm,
            out int expectedSizeInBytes,
            out string algorithmIdentifier)
        {
            if (hashAlgorithm == HashAlgorithmName.SHA256)
            {
                expectedSizeInBytes = 256 / 8;
                algorithmIdentifier = "2.16.840.1.101.3.4.2.1";
            }
            else if (hashAlgorithm == HashAlgorithmName.SHA384)
            {
                expectedSizeInBytes = 384 / 8;
                algorithmIdentifier = "2.16.840.1.101.3.4.2.2";
            }
            else if (hashAlgorithm == HashAlgorithmName.SHA512)
            {
                expectedSizeInBytes = 512 / 8;
                algorithmIdentifier = "2.16.840.1.101.3.4.2.3";
            }
            else
            {
                expectedSizeInBytes = 0;
                algorithmIdentifier = null;
                return false;
            }

            Debug.Assert(expectedSizeInBytes > 0 && expectedSizeInBytes <= 512 / 8);
            Debug.Assert(!string.IsNullOrEmpty(algorithmIdentifier));
            return true;
        }

        public override void CopyFrom(AsnEncodedData asnEncodedData)
        {
            _data = null;
            base.CopyFrom(asnEncodedData);
        }
    }
}
#endif
