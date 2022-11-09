// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED && IS_CORECLR
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class Rfc3161TimestampTokenNetstandard21Wrapper : IRfc3161TimestampToken
    {
        private readonly System.Security.Cryptography.Pkcs.Rfc3161TimestampToken _rfc3161TimestampToken;
        public IRfc3161TimestampTokenInfo TokenInfo { get; }

        public Rfc3161TimestampTokenNetstandard21Wrapper(
            IRfc3161TimestampTokenInfo tstInfo,
            X509Certificate2 signerCertificate,
            X509Certificate2Collection additionalCerts,
            byte[] encoded)
        {
            bool success = System.Security.Cryptography.Pkcs.Rfc3161TimestampToken.TryDecode(
                new ReadOnlyMemory<byte>(encoded),
                out _rfc3161TimestampToken,
                out var _);

            if (!success)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            TokenInfo = new Rfc3161TimestampTokenInfoNetstandard21Wrapper(_rfc3161TimestampToken.TokenInfo);
        }

        public Rfc3161TimestampTokenNetstandard21Wrapper(
            System.Security.Cryptography.Pkcs.Rfc3161TimestampToken rfc3161TimestampToken)
        {
            _rfc3161TimestampToken = rfc3161TimestampToken;

            TokenInfo = new Rfc3161TimestampTokenInfoNetstandard21Wrapper(_rfc3161TimestampToken.TokenInfo);
        }

        public SignedCms AsSignedCms()
        {
            return _rfc3161TimestampToken.AsSignedCms();
        }
    }
}
#endif
