// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Server;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.Protocol.FuncTest
{
    public class AuthenticationHandlerTests
    {
        private readonly ITestOutputHelper _output;

        public AuthenticationHandlerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/12962")]
        public async Task GetAsync_GetPackageAfterServiceIndex_SecondUrlIsPreAuthenticated()
        {
            // Arrange
            var portReserver = new PortReserver();
            await portReserver.ExecuteAsync(async (port, cancellationToken) =>
            {
                var server = new RequestCollectingServer(_output);
                server.Start(port);

                // Ensure that HttpClientHandler's PreAuthenticate is working as expected, and as needed, by a second
                // request getting a URL that is in a sub-directory of the service index
                string serviceIndexUrl = server.BaseUrl + "v3/index.json";
                string packageUrl = server.BaseUrl + "v3/flatcontainer/packageid/index.json";
                try
                {
                    var packageSource = new PackageSource(server.BaseUrl + "v3/index.json", "test");
                    packageSource.Credentials = new PackageSourceCredential(packageSource.Name, "user", "pass", true, "basic");
                    var repository = Repository.Factory.GetCoreV3(packageSource);

                    var httpSourceResource = await repository.GetResourceAsync<HttpSourceResource>(CancellationToken.None);
                    var source = httpSourceResource.HttpSource;

                    using (var sourceCacheContext = new SourceCacheContext()
                    {
                        DirectDownload = true,
                        NoCache = true,
                    })
                    {
                        Mock<ILogger> logger = new();
                        HttpSourceCacheContext httpSourceCacheContext = HttpSourceCacheContext.Create(sourceCacheContext, isFirstAttempt: true);

                        // Act
                        var request = new HttpSourceCachedRequest(serviceIndexUrl, "1", httpSourceCacheContext);
                        _ = await source.GetAsync(request, ProcessResponse, logger.Object, cancellationToken);

                        request = new HttpSourceCachedRequest(packageUrl, "2", httpSourceCacheContext);
                        _ = await source.GetAsync(request, ProcessResponse, logger.Object, cancellationToken);
                    }
                }
                finally
                {
                    server.Stop();
                }

                // Assert
                server.Requests.Count(RequestWithoutAuthorizationHeader).Should().Be(1);
                server.Requests.Any(r => r.Url!.OriginalString == serviceIndexUrl).Should().BeTrue();
                server.Requests.Any(r => r.Url!.OriginalString == packageUrl).Should().BeTrue();

                // ExecuteAsync returns Task<T>, so need to return something to give it a <T>.
                return (object?)null;
            },
            CancellationToken.None);

            static bool RequestWithoutAuthorizationHeader(HttpListenerRequest request)
            {
                string? value = request.Headers["Authorization"];
                return string.IsNullOrEmpty(value);
            }

            static Task<string> ProcessResponse(HttpSourceResult result)
            {
                return Task.FromResult(string.Empty);
            }
        }

        [Fact]
        public async Task GetAsync_GetPackageServiceIndex_EveryRequestForCredentialsInvokesCacheFirstAndCredentialProvidersIfNeeded_SucceedsAsync()
        {
            // Arrange
            var portReserver = new PortReserver();
            await portReserver.ExecuteAsync(async (port, cancellationToken) =>
            {
                var mockedCredentialService = new Mock<ICredentialService>();
                var expectedCredentials = new NetworkCredential("user", "password1");

                var server = new RequestCollectingServer(_output);
                server.Start(port);
                string serviceIndexUrl = server.BaseUrl + "v3/index.json";
                string packageUrl = server.BaseUrl + "v3/flatcontainer/packageid/index.json";

                try
                {
                    var packageSource = new PackageSource(server.BaseUrl + "v3/index.json", "test");
                    var repository = Repository.Factory.GetCoreV3(packageSource);

                    SetupCredentialServiceMock(mockedCredentialService, expectedCredentials, packageSource);
                    HttpHandlerResourceV3.CredentialService = new Lazy<ICredentialService>(() => mockedCredentialService.Object);

                    var httpSourceResource = await repository.GetResourceAsync<HttpSourceResource>(CancellationToken.None);
                    var source = httpSourceResource.HttpSource;

                    using var sourceCacheContext = new SourceCacheContext()
                    {
                        DirectDownload = true,
                        NoCache = true,
                    };
                    Mock<ILogger> logger = new();
                    HttpSourceCacheContext httpSourceCacheContext = HttpSourceCacheContext.Create(sourceCacheContext, isFirstAttempt: true);

                    // Act                    
                    var request = new HttpSourceCachedRequest(serviceIndexUrl, "1", httpSourceCacheContext);
                    _ = await source.GetAsync(request, ProcessResponse, logger.Object, cancellationToken);

                    request = new HttpSourceCachedRequest(packageUrl, "2", httpSourceCacheContext);
                    _ = await source.GetAsync(request, ProcessResponse, logger.Object, cancellationToken);
                }
                finally
                {
                    server.Stop();
                }

                // Assert

                // Each attempt to access a private feed initially receives a 401 Unauthorized response.
                // Following the 401 response, NuGet attempts to acquire the necessary credentials.
                // These credentials are then used for subsequent requests to the feed.
                // Note: If 'HttpClientHandler.PreAuthenticate' is set to true, this behavior might differ as 
                // credentials would be sent preemptively after the initial request.

                // In this test, 2 requests are sent from the test's perspective, and the previous paragraph's explanation will
                // make you believe the server should see 4, but it turns out to be 5. This discrepancy occurs because 
                // HttpSourceCredentials is only initialized with credentials if the PackageSource object has credentials set.
                // In other words, if there are creds in NuGet.Config or the appropriate environment variable. However, if the
                // package source uses a credential provider, the provider is not asked for credentials until after the first 401
                // response. After the auth handler requests HttpClientHandler to make a second request, it will again make
                // an unauthenticated request, and then finally obtain the credentials and send an authenticated request from
                // what the server sees as the 3rd request.

                // Request flow while accessing index.json
                // NuGet -> Server (No Credentials) - 1st request
                // Server -> NuGet (401 Unauthorized)
                // NuGet -> Server (Sends HttpClientHandler.Credentials) - 2nd request
                // Server -> NuGet (401 Unauthorized because HttpClientHandler.Credentials returns a null value by default)
                // NuGet -> Credential Service
                // Credential Service -> NuGet (Returns credentials)
                // NuGet -> Server (Sends credentials received from the credential service) - 3rd request
                // Server -> NuGet (200 OK)

                // Request flow while retrieving package information
                // NuGet -> Server (No Credentials) - 4th request
                // Server -> NuGet (401 Unauthorized)
                // NuGet -> Server (Sends HttpClientHandler.Credentials) - 5th request
                // Server -> NuGet (200 OK)
                server.Requests.Should().HaveCount(5);
                // This should have been 2, but is 3 because of the reason mentioned above.
                server.Requests.Count(RequestWithoutAuthorizationHeader).Should().Be(3);
                // Validate that the credentials sent with the 3rd request & 5th request are the ones received from the credential service.
                foreach (var httpListenerRequest in server.Requests)
                {
                    string? authorization = httpListenerRequest.Headers["Authorization"];
                    if (authorization != null)
                    {
                        var encodedCredentials = authorization.Substring("Basic ".Length).Trim();
                        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                        var creds = credentials.Split(':');
                        Assert.Equal(expectedCredentials.UserName, creds[0]);
                        Assert.Equal(expectedCredentials.Password, creds[1]);
                    }
                }

                mockedCredentialService.Verify(x => x.GetCredentialsAsync(It.IsAny<Uri>(), It.IsAny<IWebProxy>(), It.IsAny<CredentialRequestType>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
                // The method TryGetLastKnownGoodCredentialsFromCache is invoked only once, despite the client sending two requests:
                // the first to access index.json and the second to retrieve package information.
                // This occurs because when HttpClientHandler.Credentials is set (i.e., not null) while accessing the index.json.
                // These credentials are automatically sent with requests to the private feed upon receiving 401 challenge.
                // As a result, NuGet does not need to invoke the credential service again while retrieving the package information.
                mockedCredentialService.Verify(x => x.TryGetLastKnownGoodCredentialsFromCache(It.IsAny<Uri>(), It.IsAny<bool>(), out It.Ref<ICredentials>.IsAny), Times.Once);

                // ExecuteAsync returns Task<T>, so need to return something to give it a <T>.
                return (object?)null;
            },
            CancellationToken.None);

            static void SetupCredentialServiceMock(Mock<ICredentialService> mockedCredentialService, NetworkCredential expectedCredentials, PackageSource packageSource)
            {
                NetworkCredential? cachedCredentials = default;
                mockedCredentialService.SetupGet(x => x.HandlesDefaultCredentials).Returns(true);
                // Setup GetCredentialsAsync mock
                mockedCredentialService
                    .Setup(x => x.GetCredentialsAsync(packageSource.SourceUri, It.IsAny<IWebProxy>(), CredentialRequestType.Unauthorized, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() =>
                    {
                        cachedCredentials = expectedCredentials;
                        return cachedCredentials;
                    });
                // Setup TryGetLastKnownGoodCredentialsFromCache mock
                mockedCredentialService
                    .Setup(x => x.TryGetLastKnownGoodCredentialsFromCache(packageSource.SourceUri, It.IsAny<bool>(), out It.Ref<ICredentials>.IsAny))
                    .Returns((Uri sourceUri, bool isProxyRequest, out ICredentials? outCredentials) =>
                    {
                        outCredentials = cachedCredentials;
                        return outCredentials != null;
                    });
            }

            static bool RequestWithoutAuthorizationHeader(HttpListenerRequest request)
            {
                string? value = request.Headers["Authorization"];
                return string.IsNullOrEmpty(value);
            }

            static Task<string> ProcessResponse(HttpSourceResult result)
            {
                return Task.FromResult(string.Empty);
            }
        }

        private class RequestCollectingServer
        {
            private string? _baseUrl;
            private HttpListener? _httpListener;
            private List<HttpListenerRequest> _requests = new();
            private ITestOutputHelper _output;

            public RequestCollectingServer(ITestOutputHelper output)
            {
                _output = output;
            }

            public string BaseUrl
            {
                get
                {
                    if (_baseUrl == null)
                    {
                        throw new InvalidOperationException("Start must be called first");
                    }
                    return _baseUrl;
                }
            }

            public IReadOnlyList<HttpListenerRequest> Requests
            {
                get
                {
                    return _requests;
                }
            }

            public void Start(int port)
            {
                if (_httpListener != null)
                {
                    throw new InvalidOperationException("Already started");
                }

                string baseUrl = $"http://localhost:{port}/";
                var listener = new HttpListener();
                listener.Prefixes.Add(baseUrl);
                listener.AuthenticationSchemes = AuthenticationSchemes.Basic | AuthenticationSchemes.Anonymous;
                listener.Start();
                _httpListener = listener;
                _baseUrl = baseUrl;

                listener.BeginGetContext(HandleHttpRequest, listener);
            }

            public void HandleHttpRequest(IAsyncResult result)
            {
                HttpListener httpListener = (HttpListener)result.AsyncState!;
                if (httpListener.IsListening)
                {
                    HttpListenerContext? context = null;
                    try
                    {
                        context = httpListener.EndGetContext(result);

                        _requests.Add(context.Request);

                        string? authorization = context.Request.Headers["Authorization"];

                        if (authorization == null)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.AddHeader("WWW-Authenticate", "Basic");
                        }
                        else
                        {
                            context.Response.StatusCode = 200;
                        }

                        _output.WriteLine($"Got request for {context.Request.Url}. Auth: {authorization}");
                    }
                    catch (Exception ex)
                    {
                        if (context != null)
                        {
                            context.Response.StatusCode = 500;
                            using (var textStream = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
                            {
                                textStream.Write(ex.ToString());
                            }
                        }
                    }

                    context?.Response.Close();

                    try
                    {
                        httpListener.BeginGetContext(HandleHttpRequest, httpListener);
                    }
                    catch (ObjectDisposedException)
                    {
                        // .NET 5 throws here, whereas .NET Framework triggers the callback where we can check IsListening == false
                    }
                    catch (InvalidOperationException)
                    {
                        // Sometimes BeginGetContext throws when httpListener is stopped. Possibly race condition between this callback
                        // handler and the test stopping the httpListener at the end of the test. Appears to be .NET Framework only.
                    }
                }
            }

            public void Stop()
            {
                if (_httpListener == null)
                {
                    throw new InvalidOperationException("Not started");
                }

                var listener = _httpListener;
                _httpListener = null;
                _baseUrl = null;

                listener.Stop();
                listener.Close();
            }
        }
    }
}
