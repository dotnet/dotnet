// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class Rfc3161TimestampTokenFactory
    {
        public static IRfc3161TimestampToken Create(
            IRfc3161TimestampTokenInfo tstInfo,
            X509Certificate2 signerCertificate,
            X509Certificate2Collection additionalCerts,
            byte[] encoded)
        {
            if (tstInfo == null)
            {
                throw new ArgumentNullException(nameof(tstInfo));
            }

            if (signerCertificate == null)
            {
                throw new ArgumentNullException(nameof(signerCertificate));
            }

            if (additionalCerts == null)
            {
                throw new ArgumentNullException(nameof(additionalCerts));
            }

            if (encoded == null)
            {
                throw new ArgumentNullException(nameof(encoded));
            }

            IRfc3161TimestampToken iRfc3161TimestampToken = null;
#if IS_DESKTOP
            iRfc3161TimestampToken = new Rfc3161TimestampTokenNet472Wrapper(
                tstInfo,
                signerCertificate,
                additionalCerts,
                encoded);
#else
            iRfc3161TimestampToken = new Rfc3161TimestampTokenNetstandard21Wrapper(
                tstInfo,
                signerCertificate,
                additionalCerts,
                encoded);
#endif
            return iRfc3161TimestampToken;
        }
    }
}
#endif
