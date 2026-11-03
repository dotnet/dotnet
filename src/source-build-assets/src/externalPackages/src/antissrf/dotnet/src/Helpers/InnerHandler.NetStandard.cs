#if !NET5_0_OR_GREATER

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Security.AntiSSRF
{
    internal sealed class InnerHandler : HttpClientHandler
    {
        private readonly AntiSSRFPolicy _policy;

        internal InnerHandler(AntiSSRFPolicy policy) : base()
        {
            _policy = policy;
            AllowAutoRedirect = false;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri is null)
                throw new AntiSSRFException($"No URI was specified.");

            string host = request.RequestUri.Host;

            bool isIPAddressHost = IPAddress.TryParse(host, out IPAddress? parsedIpAddress);
            IPAddress[] resolvedIPs;
            if (isIPAddressHost && parsedIpAddress is not null)
            {
                resolvedIPs = new[] { parsedIpAddress };
            }
            else
            {
                resolvedIPs = await Dns.GetHostAddressesAsync(host).ConfigureAwait(false);
                if (resolvedIPs.Length == 0)
                    throw new AntiSSRFException($"DNS lookup failed for {host}.");
            }

            if (!_policy.IsNetworkConnectionAllowed(resolvedIPs))
                throw new AntiSSRFException($"The connection to {host} is not allowed per policy.");

            if (!isIPAddressHost)
            {
                request.Headers.Host = host;

                if (resolvedIPs[0].AddressFamily == AddressFamily.InterNetwork || resolvedIPs[0].AddressFamily == AddressFamily.InterNetworkV6)
                {
                    try
                    {
                        var uriBuilder = new UriBuilder(request.RequestUri)
                        {
                            Host = resolvedIPs[0].ToString()
                        };
                        request.RequestUri = uriBuilder.Uri;
                    }
                    catch
                    {
                        throw new AntiSSRFException($"Unsupported URI format");
                    }
                }
                else
                {
                    throw new AntiSSRFException($"Unsupported AddressFamily: {resolvedIPs[0].AddressFamily}");
                }
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}

#endif
