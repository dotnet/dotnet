// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using Strings = NuGet.Protocol.Strings;

static internal class ODataServiceDocumentUtils
{
    public static async Task<ODataServiceDocumentResourceV2> CreateODataServiceDocumentResourceV2(
        string url,
        HttpSource client,
        DateTime utcNow,
        ILogger log,
        CancellationToken token)
    {
        // Get the service document and record the URL after any redirects.
        string lastRequestUri;
        try
        {
            lastRequestUri = await client.ProcessResponseAsync(
                new HttpSourceRequest(() => HttpRequestMessageFactory.Create(HttpMethod.Get, url, log)),
                response =>
                {
                    if (response.RequestMessage == null)
                    {
                        return Task.FromResult(url);
                    }

                    return Task.FromResult(response.RequestMessage.RequestUri.ToString());
                },
                log,
                token);
        }
        catch (Exception ex) when (!(ex is FatalProtocolException) && (!(ex is OperationCanceledException)))
        {
            string message = String.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToReadServiceIndex, url);

            throw new FatalProtocolException(message, ex);
        }

        // Trim the query string or any trailing slash.
        var builder = new UriBuilder(lastRequestUri) { Query = null };
        var baseAddress = builder.Uri.AbsoluteUri.Trim('/');

        return new ODataServiceDocumentResourceV2(baseAddress, DateTime.UtcNow);
    }
}
