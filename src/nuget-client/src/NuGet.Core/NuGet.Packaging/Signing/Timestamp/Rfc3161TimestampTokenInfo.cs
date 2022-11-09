// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
#if IS_DESKTOP
using System.Security.Cryptography.Pkcs;
#endif
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Represents an RFC3161 TSTInfo.
    /// This class should be removed once we can reference it throught the .NET Core framework.
    /// </summary>
    public sealed class Rfc3161TimestampTokenInfo : AsnEncodedData
    {
#if IS_DESKTOP
        public const string TimestampTokenInfoId = "1.2.840.113549.1.9.16.1.4";

        private TstInfo _decoded;

        private TstInfo Decoded
        {
            get
            {
                if (_decoded == null)
                    _decoded = TstInfo.Read(RawData);

                return _decoded;
            }
        }

        public Rfc3161TimestampTokenInfo(byte[] timestampTokenInfo)
            : base(TimestampTokenInfoId, timestampTokenInfo)
        {
        }

        internal Rfc3161TimestampTokenInfo(IntPtr pTsContext)
        {
            var context = (Rfc3161TimestampWin32.CRYPT_TIMESTAMP_CONTEXT)Marshal.PtrToStructure(pTsContext, typeof(Rfc3161TimestampWin32.CRYPT_TIMESTAMP_CONTEXT));
            byte[] encoded = new byte[context.cbEncoded];
            Marshal.Copy(context.pbEncoded, encoded, 0, context.cbEncoded);

            var cms = new SignedCms();

            cms.Decode(encoded);

            if (!string.Equals(cms.ContentInfo.ContentType.Value, Oids.TSTInfoContentType, StringComparison.Ordinal))
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            RawData = cms.ContentInfo.Content;

            _decoded = TstInfo.Read(RawData);
        }

        public int Version => Decoded.Version;

        public string PolicyId => Decoded.Policy.Value;

        public Oid HashAlgorithmId
        {
            get
            {
                Oid value = Decoded.MessageImprint.HashAlgorithm.Algorithm;

                return new Oid(value.Value, value.FriendlyName);
            }
        }

        public byte[] GetMessageHash()
        {
            return (byte[])Decoded.MessageImprint.HashedMessage.Clone();
        }

        public bool HasMessageHash(byte[] hash)
        {
            if (hash == null)
                return false;

            var value = Decoded.MessageImprint.HashedMessage;

            if (hash.Length != value.Length)
            {
                return false;
            }

            return value.SequenceEqual(hash);
        }

        /// <summary>
        /// Gets the serial number for the request in the big-endian byte order.
        /// </summary>
        public byte[] GetSerialNumber()
        {
            return (byte[])Decoded.SerialNumber.Clone();
        }

        public DateTimeOffset Timestamp => Decoded.GenTime;

        public long? AccuracyInMicroseconds => Decoded.Accuracy?.GetTotalMicroseconds();

        public bool IsOrdering => Decoded.Ordering;

        public byte[] GetNonce()
        {
            var nonce = (byte[])Decoded.Nonce?.Clone();

            if (nonce != null)
            {
                // Convert from big endian to little endian.
                Array.Reverse(nonce);
            }

            return nonce;
        }

        public byte[] GetTimestampAuthorityName()
        {
            return (byte[])Decoded.Tsa?.Clone();
        }

        public bool HasExtensions => Decoded.Extensions != null;

        public X509ExtensionCollection GetExtensions()
        {
            return ShallowCopy(Decoded.Extensions, preserveNull: false);
        }

        internal static X509ExtensionCollection ShallowCopy(X509ExtensionCollection existing, bool preserveNull)
        {
            if (preserveNull && existing == null)
                return null;

            var coll = new X509ExtensionCollection();

            if (existing == null)
                return coll;

            foreach (var extn in existing)
            {
                coll.Add(extn);
            }

            return coll;
        }

        public override void CopyFrom(AsnEncodedData asnEncodedData)
        {
            _decoded = null;
            base.CopyFrom(asnEncodedData);
        }

        internal static byte[] CopyFromNative(ref Rfc3161TimestampWin32.CRYPTOAPI_BLOB blob)
        {
            if (blob.cbData == 0)
                return null;

            byte[] answer = new byte[blob.cbData];
            Marshal.Copy(blob.pbData, answer, 0, answer.Length);
            return answer;
        }
#endif
    }
}
