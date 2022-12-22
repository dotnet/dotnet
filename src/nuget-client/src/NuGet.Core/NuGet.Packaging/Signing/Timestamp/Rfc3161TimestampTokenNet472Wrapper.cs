// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED && IS_DESKTOP
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class Rfc3161TimestampTokenNet472Wrapper : IRfc3161TimestampToken
    {
        private readonly Rfc3161TimestampToken _rfc3161TimestampToken;

        public Rfc3161TimestampTokenNet472Wrapper(
            IRfc3161TimestampTokenInfo tstInfo,
            X509Certificate2 signerCertificate,
            X509Certificate2Collection additionalCerts,
            byte[] encoded)
        {
            _rfc3161TimestampToken = new Rfc3161TimestampToken(
                tstInfo,
                signerCertificate,
                additionalCerts,
                encoded);
        }

        public IRfc3161TimestampTokenInfo TokenInfo => _rfc3161TimestampToken.TokenInfo;

        public SignedCms AsSignedCms()
        {
            return _rfc3161TimestampToken.AsSignedCms();
        }
    }
}
#endif
