// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if !IS_CORECLR
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    public class StsAuthenticationHandler : DelegatingHandler
    {
        // Only one source may prompt at a time
        private readonly static SemaphoreSlim _credentialPromptLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Response header that specifies the WSTrust13 Windows Transport endpoint.
        /// </summary>
        public const string STSEndPointHeader = "X-NuGet-STS-EndPoint";

        /// <summary>
        /// Response header that specifies the realm to authenticate for. In most cases this would be the gallery we are going up against.
        /// </summary>
        public const string STSRealmHeader = "X-NuGet-STS-Realm";

        /// <summary>
        /// Request header that contains the SAML token.
        /// </summary>
        public const string STSTokenHeader = "X-NuGet-STS-Token";

        private readonly Uri _baseUri;

        private readonly TokenStore _tokenStore;

        private readonly Func<string, string, string> _tokenFactory;

        public StsAuthenticationHandler(Configuration.PackageSource packageSource, TokenStore tokenStore)
            : this(packageSource, tokenStore, tokenFactory: (endpoint, realm) => AcquireSTSToken(endpoint, realm))
        {
        }

        public StsAuthenticationHandler(Configuration.PackageSource packageSource, TokenStore tokenStore, Func<string, string, string> tokenFactory)
        {
            if (packageSource == null)
            {
                throw new ArgumentNullException(nameof(packageSource));
            }

            _baseUri = packageSource.SourceUri;

            if (tokenStore == null)
            {
                throw new ArgumentNullException(nameof(tokenStore));
            }

            _tokenStore = tokenStore;

            if (tokenFactory == null)
            {
                throw new ArgumentNullException(nameof(tokenFactory));
            }

            _tokenFactory = tokenFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            bool shouldRetry = false;

            do
            {
                // Clean up any previous responses
                if (response != null)
                {
                    response.Dispose();
                }

                // keep the token store version
                var cacheVersion = _tokenStore.Version;

                using (var req = request.Clone())
                {
                    PrepareSTSRequest(req);

                    response = await base.SendAsync(req, cancellationToken);
                }

                if (!shouldRetry && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    try
                    {
                        await _credentialPromptLock.WaitAsync();

                        if (cacheVersion != _tokenStore.Version)
                        {
                            // retry the request with updated credentials
                            shouldRetry = true;
                        }
                        else
                        {
                            shouldRetry = TryRetrieveSTSToken(response);
                        }
                    }
                    finally
                    {
                        _credentialPromptLock.Release();
                    }
                }
                else
                {
                    shouldRetry = false;
                }

            } while (shouldRetry);

            return response;
        }

        /// <summary>
        /// Adds the SAML token as a header to the request if it is already cached for this source.
        /// </summary>
        private void PrepareSTSRequest(HttpRequestMessage request)
        {
            var STSToken = _tokenStore.GetToken(_baseUri);

            if (!string.IsNullOrEmpty(STSToken))
            {
                request.Headers.TryAddWithoutValidation(STSTokenHeader, STSToken);
            }
        }

        /// <summary>
        /// Attempts to retrieve a SAML token if the response indicates that server requires STS-based auth.
        /// </summary>
        public bool TryRetrieveSTSToken(HttpResponseMessage response)
        {
            var endpoint = GetHeader(response, STSEndPointHeader);
            var realm = GetHeader(response, STSRealmHeader);
            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(realm))
            {
                // The server does not conform to NuGet STS-auth requirements.
                return false;
            }

            var STSToken = _tokenStore.GetToken(_baseUri);

            if (string.IsNullOrEmpty(STSToken))
            {
                var rawStsToken = _tokenFactory.Invoke(endpoint, realm);
                if (rawStsToken != null)
                {
                    STSToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawStsToken));
                    _tokenStore.AddToken(_baseUri, STSToken);
                    return true;
                }
            }

            return false;
        }

        private static string AcquireSTSToken(string endpoint, string realm)
        {
            var binding = new WS2007HttpBinding(SecurityMode.Transport);

            using var factory = new WSTrustChannelFactory(binding, endpoint) { TrustVersion = TrustVersion.WSTrust13 };

            var endPointReference = new EndpointReference(realm);
            var requestToken = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,
                AppliesTo = endPointReference
            };

            var channel = factory.CreateChannel();
            var responseToken = channel.Issue(requestToken) as GenericXmlSecurityToken;
            return responseToken?.TokenXml.OuterXml;
        }

        private static string GetHeader(HttpResponseMessage response, string header)
        {
            IEnumerable<string> values;
            if (response.Headers.TryGetValues(header, out values))
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}
#endif
