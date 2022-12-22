// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED && IS_CORECLR
using System;
using System.Linq;
using System.Security.Cryptography;

namespace NuGet.Packaging.Signing
{
    internal sealed class Rfc3161TimestampTokenInfoNetstandard21Wrapper : IRfc3161TimestampTokenInfo
    {
        private System.Security.Cryptography.Pkcs.Rfc3161TimestampTokenInfo _rfc3161TimestampTokenInfo;

        public Rfc3161TimestampTokenInfoNetstandard21Wrapper(byte[] timestampTokenInfo)
        {
            bool success = System.Security.Cryptography.Pkcs.Rfc3161TimestampTokenInfo.TryDecode(
                new ReadOnlyMemory<byte>(timestampTokenInfo),
                out _rfc3161TimestampTokenInfo,
                out var _);

            if (!success)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }
        }

        public Rfc3161TimestampTokenInfoNetstandard21Wrapper(System.Security.Cryptography.Pkcs.Rfc3161TimestampTokenInfo timestampTokenInfo)
        {
            _rfc3161TimestampTokenInfo = timestampTokenInfo;
        }

        public string PolicyId => _rfc3161TimestampTokenInfo.PolicyId.ToString();

        public DateTimeOffset Timestamp => _rfc3161TimestampTokenInfo.Timestamp;

        public long? AccuracyInMicroseconds => _rfc3161TimestampTokenInfo.AccuracyInMicroseconds;

        public Oid HashAlgorithmId => _rfc3161TimestampTokenInfo.HashAlgorithmId;

        public bool HasMessageHash(byte[] hash)
        {
            if (hash == null)
            {
                return false;
            }

            var value = _rfc3161TimestampTokenInfo.GetMessageHash().ToArray();

            if (hash.Length != value.Length)
            {
                return false;
            }

            return value.SequenceEqual(hash);
        }

        public byte[] GetNonce()
        {
            ReadOnlyMemory<byte>? nonce = _rfc3161TimestampTokenInfo.GetNonce();
            if (nonce.HasValue)
            {
                return nonce.Value.ToArray();
            }
            return Array.Empty<byte>();
        }
    }
}
#endif
