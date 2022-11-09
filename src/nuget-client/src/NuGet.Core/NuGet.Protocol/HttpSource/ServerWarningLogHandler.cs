// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol
{
    public class ServerWarningLogHandler
        : DelegatingHandler
    {
        public ServerWarningLogHandler(HttpClientHandler clientHandler)
            : base(clientHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var configuration = request.GetOrCreateConfiguration();

            var response = await base.SendAsync(request, cancellationToken);

            response.LogServerWarning(configuration.Logger);

            return response;
        }
    }
}
