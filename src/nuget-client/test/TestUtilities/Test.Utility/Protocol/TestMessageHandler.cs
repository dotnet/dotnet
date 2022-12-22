// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Utility
{
    public class TestMessageHandler : HttpClientHandler
    {
        private Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> _responses;
        private int _currentRequestCount;
        private int _maxConcurrencyRequest;

        public int MaxConcurrencyRequest => _maxConcurrencyRequest;

        public int WaitTimeInMs { get; set; }

        public TestMessageHandler(Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> responses)
        {
            _responses = responses;
        }

        public TestMessageHandler(Dictionary<string, string> responses, string errorContent)
        {
            _responses = GetResponse(responses, errorContent);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                Interlocked.Increment(ref _currentRequestCount);

                if (_currentRequestCount > MaxConcurrencyRequest)
                {
                    Interlocked.Exchange(ref _maxConcurrencyRequest, _currentRequestCount);
                }

                if (WaitTimeInMs > 0)
                {
                    Thread.Sleep(WaitTimeInMs);
                }

                return SendAsyncPublic(request);
            }
            finally
            {
                Interlocked.Decrement(ref _currentRequestCount);
            }
        }

        private Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> GetResponse(Dictionary<string, string> responses, string errorContent)
        {
            return responses.ToDictionary<KeyValuePair<string, string>, string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>(
                pair => pair.Key,
                pair => _ => Task.FromResult(GetResponseMessage(pair.Value, errorContent)),
                responses.Comparer);
        }

        private HttpResponseMessage GetResponseMessage(string source, string errorContent)
        {
            var msg = new HttpResponseMessage(HttpStatusCode.OK);

            if (source == null)
            {
                msg = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                msg.Content = new TestContent(errorContent);
            }
            else if (source == string.Empty)
            {
                msg = new HttpResponseMessage(HttpStatusCode.NotFound);
                msg.Content = new TestContent(errorContent);
            }
            else if (source == "204")
            {
                msg = new HttpResponseMessage(HttpStatusCode.NoContent);
                msg.Content = new TestContent(string.Empty);
            }
            else if (source.StartsWith("301 "))
            {
                var url = source.Substring(4);
                msg = new HttpResponseMessage(HttpStatusCode.MovedPermanently)
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, url),
                    Content = new TestContent(string.Empty)
                };
            }
            else
            {
                msg.Content = new TestContent(source);
            }

            return msg;
        }

        public virtual async Task<HttpResponseMessage> SendAsyncPublic(HttpRequestMessage request)
        {
            Func<HttpRequestMessage, Task<HttpResponseMessage>> getResponse;
            if (_responses.TryGetValue(request.RequestUri.AbsoluteUri, out getResponse))
            {
                return await getResponse(request);
            }
            else
            {
                throw new Exception("Unhandled test request: " + request.RequestUri.AbsoluteUri);
            }
        }
    }
}
