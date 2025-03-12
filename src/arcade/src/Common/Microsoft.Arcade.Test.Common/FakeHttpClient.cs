// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Arcade.Test.Common
{
    public static class FakeHttpClient
    {
        
        public static HttpClient WithResponses(params HttpResponseMessage[] responses)
            => new HttpClient( // lgtm [cs/httpclient-checkcertrevlist-disabled] HttpClient used in unit tests
                new FakeResponseOnlyHttpMessageHandler(responses));

        public static HttpClient WithResponsesGivenUris(Dictionary<string, IEnumerable<HttpResponseMessage>> FakeHttpMessageHandler)
            => new HttpClient( // lgtm [cs/httpclient-checkcertrevlist-disabled] HttpClient used in unit tests
                new FakeHttpMessageHandler(FakeHttpMessageHandler));

        public class FakeResponseOnlyHttpMessageHandler : HttpMessageHandler
        {
            private readonly IEnumerator<HttpResponseMessage> _responseEnumerator;

            public FakeResponseOnlyHttpMessageHandler(IEnumerable<HttpResponseMessage> responses)
            {
                _responseEnumerator = responses.GetEnumerator();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _responseEnumerator.Dispose();
                }
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (!_responseEnumerator.MoveNext())
                    throw new InvalidOperationException($"Unexpected end of response sequence. Number of predefined responses should be at least equal to number of requests invoked by HttpClient.");

                return Task.FromResult(_responseEnumerator.Current);
            }
        }

        public class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly Dictionary<string, IEnumerator<HttpResponseMessage>> _responseEnumerators;

            public FakeHttpMessageHandler(Dictionary<string, IEnumerable<HttpResponseMessage>> responses)
            {
                _responseEnumerators = responses.Select(kvp =>
                    new KeyValuePair<string, IEnumerator<HttpResponseMessage>>(kvp.Key, kvp.Value.GetEnumerator()))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    foreach (var e in _responseEnumerators.Values)
                    {
                        e.Dispose();
                    }
                }
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (!_responseEnumerators.TryGetValue(request.RequestUri.ToString(), out var responseEnumerator))
                {
                    if (request.RequestUri != null)
                        throw new InvalidOperationException($"Unexpected request URI: {request.RequestUri}.");
                }
                if (!responseEnumerator.MoveNext())
                    throw new InvalidOperationException($"Unexpected end of response sequence for uri '{request.RequestUri}'. " +
                        $"Number of predefined responses should be at least equal to number of requests invoked by HttpClient.");

                return Task.FromResult(responseEnumerator.Current);
            }
        }
    }
}
