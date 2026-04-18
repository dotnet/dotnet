#if NETCOREAPP
namespace Microsoft.ApplicationInsights.TestFramework.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("WindowsOnly")] // The LocalInProcHttpServer does not perform well on Linux.
    public class RedirectHttpHandlerTests
    {
        private const string helloString = "Hello World!";

        private const string LocalUrl1 = "http://localhost:1111";
        private const string LocalUrl2 = "http://localhost:2222";

        /// <summary>
        /// Verify behavior of HttpClient without <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// </summary>
        [TestMethod]
        public async Task DefaultUseCase()
        {
            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl2, cacheExpirationDuration: TimeSpan.FromDays(1));
            using var localServer2 = LocalInProcHttpServer.MakeTargetServer(url: LocalUrl2, response: helloString);

            var client = new MyCustomClient(url: LocalUrl1);

            // Default behavior. 1st server will redirect to 2nd.
            var testStr1 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(1, localServer2.RequestCounter);

            // Default behavior. Nothing is cached, repeat previous workflow.
            var testStr2 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr2);
            Assert.AreEqual(2, localServer1.RequestCounter);
            Assert.AreEqual(2, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// After the first request, it is expected that the client will cache the redirect.
        /// Additional requests should skip server #1 and go to server #2.
        /// </summary>
        [TestMethod]
        public async Task VerifyRedirect()
        {
            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl2, cacheExpirationDuration: TimeSpan.FromDays(1));
            using var localServer2 = LocalInProcHttpServer.MakeTargetServer(url: LocalUrl2, response: helloString);

            var client = new MyCustomClient(url: LocalUrl1, new RedirectHttpHandler());

            // Default behavior. 1st server will redirect to 2nd.
            var testStr1 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(1, localServer2.RequestCounter);

            // Redirect is cached. Request will go to 2nd server.
            var testStr2 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr2);
            Assert.AreEqual(1, localServer1.RequestCounter, "redirect should be cached");
            Assert.AreEqual(2, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// Server #1 is missing the cache header.
        /// In this case, we will use a default cache and redirect will continue.
        /// </summary>
        [TestMethod]
        public async Task VerifyBehavior_MissingCacheHeader()
        {
            using var localServer1 = new LocalInProcHttpServer(url: LocalUrl1)
            {
                ServerLogic = async (httpContext) =>
                {
                    // Returns status code and location, without cache header.
                    httpContext.Response.StatusCode = StatusCodes.Status308PermanentRedirect;
                    httpContext.Response.Headers.Add("Location", LocalUrl2);
                    await httpContext.Response.WriteAsync("redirect");
                },
            };

            using var localServer2 = LocalInProcHttpServer.MakeTargetServer(url: LocalUrl2, response: helloString);

            var client = new MyCustomClient(url: LocalUrl1, new RedirectHttpHandler());

            // 1st server should redirect to 2nd, but is missing headers.
            var testStr1 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(1, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// Server #1 is missing the redirect uri header.
        /// In this case, redirect will fail.
        /// </summary>
        [TestMethod]
        public async Task VerifyBehavior_MissingUriHeader()
        {
            using var localServer1 = new LocalInProcHttpServer(url: LocalUrl1)
            {
                ServerLogic = async (httpContext) =>
                {
                    // Returns status code and location, without cache header.
                    httpContext.Response.StatusCode = StatusCodes.Status308PermanentRedirect;
                    httpContext.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.MinValue,
                    };
                    await httpContext.Response.WriteAsync("redirect");
                },
            };

            using var localServer2 = LocalInProcHttpServer.MakeTargetServer(url: LocalUrl2, response: helloString);

            var client = new MyCustomClient(url: LocalUrl1, new RedirectHttpHandler());

            // 1st server should redirect to 2nd, but is missing headers.
            var testStr1 = await client.GetAsync();
            Assert.AreEqual("redirect", testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(0, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Create two local servers that redirect to each other.
        /// It is expected that every request will cause the cache to be updated.
        /// This test is attempting to cause deadlocks around that cache.
        /// </summary>
        [TestMethod]
        public void StressTest()
        {
            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl2, cacheExpirationDuration: TimeSpan.FromDays(1));
            using var localServer2 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl2, redirectUrl: LocalUrl1, cacheExpirationDuration: TimeSpan.FromDays(1));

            var client = new MyCustomClient(url: LocalUrl1, new RedirectHttpHandler());

            var tasks = new List<Task>();

            int numOfRequests = 200;
            for (int i = 0; i < numOfRequests; i++)
            {
                tasks.Add(client.GetAsync());
            }

            Task.WaitAll(tasks.ToArray());

            Assert.IsTrue(localServer1.RequestCounter > 0, $"{nameof(localServer1)} did not receive any requests");
            Assert.IsTrue(localServer2.RequestCounter > 0, $"{nameof(localServer2)} did not receive any requests");

            var serverHandledRequestSum = localServer1.RequestCounter + localServer2.RequestCounter;
            Debug.WriteLine($"ServerHandledRequestSum {serverHandledRequestSum}");
            var expectedNumberOfRequests = numOfRequests * RedirectHttpHandler.MaxRedirect + numOfRequests;
            Debug.WriteLine($"ExpectedNumberOfRequests {expectedNumberOfRequests}");

            Assert.AreEqual(expectedNumberOfRequests, serverHandledRequestSum, "Unexpected number of requests");
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// After the first request, it is expected that the client will cache the redirect.
        /// After this cache expries, requests will go to server #1.
        /// </summary>
        [TestMethod]
        public async Task VerifyRedirectCache()
        {
            var shortCache = TimeSpan.FromSeconds(1);

            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl2, cacheExpirationDuration: shortCache);
            using var localServer2 = LocalInProcHttpServer.MakeTargetServer(url: LocalUrl2, response: helloString);

            var client = new MyCustomClient(url: LocalUrl1, new RedirectHttpHandler());

            // Default behavior. 1st server will redirect to 2nd.
            var testStr1 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(1, localServer2.RequestCounter);

            // Redirect is cached. Request will go to 2nd server.
            var testStr2 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr2);
            Assert.AreEqual(1, localServer1.RequestCounter, "redirect should be cached");
            Assert.AreEqual(2, localServer2.RequestCounter);

            // wait for cache to expire
            await Task.Delay(shortCache * 2);

            // Default behavior. 1st server will redirect to 2nd.
            var testStr3 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr3);
            Assert.AreEqual(2, localServer1.RequestCounter);
            Assert.AreEqual(3, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// In this test, server1 will redirect to itself.
        /// Verify that <see cref="RedirectHttpHandler.MaxRedirect"/> is enforced.
        /// </summary>
        [TestMethod]
        public async Task VerifyMaxRedirects()
        {
            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl1, cacheExpirationDuration: TimeSpan.FromDays(1));

            var client = new MyCustomClient(url: LocalUrl1, new RedirectHttpHandler());

            var testStr1 = await client.GetAsync();
            Assert.AreEqual("redirect", testStr1);

            Assert.AreEqual(RedirectHttpHandler.MaxRedirect + 1, localServer1.RequestCounter, $"expecting 1 original request + {RedirectHttpHandler.MaxRedirect} additional requests");
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// In this test, server1 will redirect to itself.
        /// Here, i'm testing that if an auth header is present, it MUST be preserved for every request.
        /// </summary>
        [TestMethod]
        public async Task VerifyAuthHeaderPreserved()
        {
            var testAuthToken = "ABCD1234";

            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl1, cacheExpirationDuration: TimeSpan.FromDays(1));
            localServer1.ServerSideAsserts = (httpContext) =>
            {
                if (httpContext.Request.Headers.TryGetValue(AuthConstants.AuthorizationHeaderName, out var authValue))
                {
                    Assert.AreEqual(testAuthToken, authValue[0]);
                }
                else
                {
                    Assert.Fail("request missing auth token");
                }
            };

            var client = new MyCustomClient(url: LocalUrl1, new RedirectHttpHandler());

            var testStr1 = await client.GetAsync(testAuthToken);
            Assert.AreEqual("redirect", testStr1);

            Assert.AreEqual(RedirectHttpHandler.MaxRedirect + 1, localServer1.RequestCounter, $"expecting 1 original request + {RedirectHttpHandler.MaxRedirect} additional requests");
        }

        /// <summary>
        /// This class is a wrapper around <see cref="HttpClient"/>.
        /// I'm using this to simplify the tests above.
        /// </summary>
        private class MyCustomClient
        {
            public readonly Uri uri;
            public readonly HttpClient httpClient;

            public MyCustomClient(string url, RedirectHttpHandler httpMessageHandler = null)
            {
                this.uri = new Uri(url);
                this.httpClient = (httpMessageHandler == null)
                    ? new HttpClient()
                    : new HttpClient(httpMessageHandler);
            }

            public async Task<string> GetAsync()
            {
                var result = await this.httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, this.uri));
                return await result.Content.ReadAsStringAsync();
            }

            public async Task<string> GetAsync(string authToken)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, this.uri);
                request.Headers.TryAddWithoutValidation(AuthConstants.AuthorizationHeaderName, authToken);

                var result = await this.httpClient.SendAsync(request);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
#endif
