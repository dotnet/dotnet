// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED && IS_DESKTOP
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NuGet.Packaging.Signing
{
    internal sealed class Rfc3161TimestampRequestNet472Wrapper : IRfc3161TimestampRequest
    {
        private readonly Rfc3161TimestampRequest _rfc3161TimestampRequest;

        public Rfc3161TimestampRequestNet472Wrapper(
            byte[] messageHash,
            HashAlgorithmName hashAlgorithm,
            Oid requestedPolicyId,
            byte[] nonce,
            bool requestSignerCertificates,
            X509ExtensionCollection extensions)
        {
            _rfc3161TimestampRequest = new Rfc3161TimestampRequest(
                messageHash,
                hashAlgorithm,
                requestedPolicyId,
                nonce,
                requestSignerCertificates,
                extensions);
        }

        public Task<IRfc3161TimestampToken> SubmitRequestAsync(Uri timestampUri, TimeSpan timeout)
        {
            return Task.FromResult(_rfc3161TimestampRequest.SubmitRequest(timestampUri, timeout));
        }

        public byte[] GetNonce()
        {
            return _rfc3161TimestampRequest.GetNonce();
        }
    }
}
#endif
