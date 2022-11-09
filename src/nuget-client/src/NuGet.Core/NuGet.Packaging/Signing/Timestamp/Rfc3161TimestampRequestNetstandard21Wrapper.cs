// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED && IS_CORECLR
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NuGet.Packaging.Signing
{
    internal sealed class Rfc3161TimestampRequestNetstandard21Wrapper : IRfc3161TimestampRequest
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly System.Security.Cryptography.Pkcs.Rfc3161TimestampRequest _rfc3161TimestampRequest;

        public Rfc3161TimestampRequestNetstandard21Wrapper(
            byte[] messageHash,
            HashAlgorithmName hashAlgorithm,
            Oid requestedPolicyId,
            byte[] nonce,
            bool requestSignerCertificates,
            X509ExtensionCollection extensions)
        {
            _rfc3161TimestampRequest = System.Security.Cryptography.Pkcs.Rfc3161TimestampRequest.CreateFromHash(
                new ReadOnlyMemory<byte>(messageHash),
                hashAlgorithm,
                requestedPolicyId,
                nonce,
                requestSignerCertificates,
                extensions);
        }

        public async Task<IRfc3161TimestampToken> SubmitRequestAsync(Uri timestampUri, TimeSpan timeout)
        {
            if (timestampUri == null)
            {
                throw new ArgumentNullException(nameof(timestampUri));
            }

            if (!timestampUri.IsAbsoluteUri)
            {
                throw new ArgumentException(
                    Strings.AnAbsoluteUriIsRequired, nameof(timestampUri));
            }

            if (timestampUri.Scheme != Uri.UriSchemeHttp && timestampUri.Scheme != Uri.UriSchemeHttps)
            {
                throw new ArgumentException(
                    Strings.HttpOrHttpsIsRequired, nameof(timestampUri));
            }

            using (var content = new ReadOnlyMemoryContent(_rfc3161TimestampRequest.Encode()))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/timestamp-query");
                using (HttpResponseMessage httpResponse = await HttpClient.PostAsync(timestampUri, content))
                {
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new CryptographicException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.TimestampServiceRespondedError,
                                (int)httpResponse.StatusCode,
                                httpResponse.ReasonPhrase));
                    }

                    var data = await httpResponse.Content.ReadAsByteArrayAsync();

                    System.Security.Cryptography.Pkcs.Rfc3161TimestampToken response = _rfc3161TimestampRequest.ProcessResponse(data, out var _);

                    var timestampToken = new Rfc3161TimestampTokenNetstandard21Wrapper(response);

                    return timestampToken;
                }
            }
        }

        public byte[] GetNonce()
        {
            ReadOnlyMemory<byte>? normalizedNonce = _rfc3161TimestampRequest.GetNonce();
            return normalizedNonce.HasValue ? normalizedNonce.Value.ToArray() : null;
        }
    }
}
#endif
