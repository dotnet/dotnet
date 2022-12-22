// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED && IS_DESKTOP
using System;
using System.Security.Cryptography;

namespace NuGet.Packaging.Signing
{
    internal sealed class Rfc3161TimestampTokenInfoNet472Wrapper : IRfc3161TimestampTokenInfo
    {
        private Rfc3161TimestampTokenInfo _rfc3161TimestampTokenInfo;

        public Rfc3161TimestampTokenInfoNet472Wrapper(byte[] timestampTokenInfo)
        {
            _rfc3161TimestampTokenInfo = new Rfc3161TimestampTokenInfo(timestampTokenInfo);
        }

        public Rfc3161TimestampTokenInfoNet472Wrapper(Rfc3161TimestampTokenInfo timestampTokenInfo)
        {
            _rfc3161TimestampTokenInfo = timestampTokenInfo;
        }

        public string PolicyId => _rfc3161TimestampTokenInfo.PolicyId;

        public DateTimeOffset Timestamp => _rfc3161TimestampTokenInfo.Timestamp;

        public long? AccuracyInMicroseconds => _rfc3161TimestampTokenInfo.AccuracyInMicroseconds;

        public Oid HashAlgorithmId => _rfc3161TimestampTokenInfo.HashAlgorithmId;

        public bool HasMessageHash(byte[] hash)
        {
            return _rfc3161TimestampTokenInfo.HasMessageHash(hash);
        }

        public byte[] GetNonce()
        {
            return _rfc3161TimestampTokenInfo.GetNonce();
        }
    }
}
#endif
