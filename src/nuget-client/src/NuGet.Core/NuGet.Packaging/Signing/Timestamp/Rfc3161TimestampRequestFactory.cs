// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal sealed class Rfc3161TimestampRequestFactory
    {
        public static IRfc3161TimestampRequest Create(
            byte[] messageHash,
            HashAlgorithmName hashAlgorithm,
            Oid requestedPolicyId,
            byte[] nonce,
            bool requestSignerCertificates,
            X509ExtensionCollection extensions)
        {
            if (messageHash == null)
            {
                throw new ArgumentNullException(nameof(messageHash));
            }

            IRfc3161TimestampRequest iRfc3161TimestampRequest = null;
#if IS_DESKTOP
            iRfc3161TimestampRequest = new Rfc3161TimestampRequestNet472Wrapper(
                messageHash,
                hashAlgorithm,
                requestedPolicyId,
                nonce,
                requestSignerCertificates,
                extensions);
#else
            iRfc3161TimestampRequest = new Rfc3161TimestampRequestNetstandard21Wrapper(
                messageHash,
                hashAlgorithm,
                requestedPolicyId,
                nonce,
                requestSignerCertificates,
                extensions);
#endif
            return iRfc3161TimestampRequest;

        }
    }
}
#endif
