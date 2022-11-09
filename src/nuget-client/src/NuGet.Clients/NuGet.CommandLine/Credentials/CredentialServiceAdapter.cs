// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
extern alias CoreV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using NuGet.Configuration;

namespace NuGet.Credentials
{
    /// <summary>
    /// Wraps a CredentialService to match the older v2 NuGet.ICredentialProvider interface
    /// </summary>
    public class CredentialServiceAdapter : CoreV2.NuGet.ICredentialProvider
    {
        private readonly ICredentialService _credentialService;
        private IDictionary<Uri, Uri> _endpoints;

        public CredentialServiceAdapter(ICredentialService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            _credentialService = service;
        }

        /// <summary>
        /// Initializes endpoint-to-source uri mapping
        /// </summary>
        /// <param name="endpoints">List of endpoint mapping entries</param>
        public void SetEndpoints(IEnumerable<KeyValuePair<Configuration.PackageSource, string>> endpoints)
        {
            _endpoints = endpoints
                .Where(kv => kv.Key.IsHttp)
                .ToDictionary(
                    kv => new Uri(kv.Value),
                    kv => kv.Key.SourceUri);
        }

        public ICredentials GetCredentials(Uri uri, IWebProxy proxy, CoreV2.NuGet.CredentialType credentialType, bool retrying)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            // NuGetCore calls the adapter with a "list" endpoint uri.
            // It may be different from a source uri, for instance when v3 source advertises v2 search endpoint.
            // If endpoints mapping is supplied, retrieve the source uri and use it to acquire credentials
            if (_endpoints != null && _endpoints.ContainsKey(uri))
            {
                uri = _endpoints[uri];
            }

            var type = credentialType == CoreV2.NuGet.CredentialType.ProxyCredentials ?
                CredentialRequestType.Proxy : CredentialRequestType.Unauthorized;

            var task = _credentialService.GetCredentialsAsync(
                uri,
                proxy,
                type,
                message: null,
                cancellationToken: CancellationToken.None);

            return task.Result;
        }
    }
}
