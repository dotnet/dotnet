// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [UseCulture("en-US")] // We are asserting exception messages in English
    public class PackageUpdateResourceTests
    {
        private const string ApiKeyHeader = "X-NuGet-ApiKey";
        private const string NuGetClientVersionHeader = "X-NuGet-Client-Version";

        [Fact]
        public async Task PackageUpdateResource_IncludesApiKeyWhenDeletingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://www.nuget.org/api/v2";
                HttpRequestMessage actualRequest = null;
                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://www.nuget.org/api/v2/DeepEqual/1.4.0.1-rc",
                        request =>
                        {
                            actualRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    }
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                var apiKey = "SomeApiKey";

                // Act
                await resource.Delete(
                    packageId: "DeepEqual",
                    packageVersion: "1.4.0.1-rc",
                    getApiKey: _ => apiKey,
                    confirm: _ => true,
                    noServiceEndpoint: false,
                    log: NullLogger.Instance);

                // Assert
                Assert.NotNull(actualRequest);
                Assert.Equal(HttpMethod.Delete, actualRequest.Method);

                IEnumerable<string> values;
                actualRequest.Headers.TryGetValues(ProtocolConstants.ApiKeyHeader, out values);
                Assert.Equal(1, values.Count());
                Assert.Equal(apiKey, values.First());

                Assert.False(
                    actualRequest.GetOrCreateConfiguration().PromptOn403,
                    "When the API key is provided, the user should not be prompted on HTTP 403.");
            }
        }

        [Fact]
        public async Task PackageUpdateResource_AllowsNoApiKeyWhenDeletingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://www.nuget.org/api/v2";
                HttpRequestMessage actualRequest = null;
                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://www.nuget.org/api/v2/DeepEqual/1.4.0.1-rc",
                        request =>
                        {
                            actualRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    }
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                var apiKey = string.Empty;

                // Act
                await resource.Delete(
                    packageId: "DeepEqual",
                    packageVersion: "1.4.0.1-rc",
                    getApiKey: _ => apiKey,
                    confirm: _ => true,
                    noServiceEndpoint: false,
                    log: NullLogger.Instance);

                // Assert
                Assert.NotNull(actualRequest);
                Assert.Equal(HttpMethod.Delete, actualRequest.Method);

                IEnumerable<string> values;
                actualRequest.Headers.TryGetValues(ProtocolConstants.ApiKeyHeader, out values);
                Assert.Null(values);

                Assert.True(
                    actualRequest.GetOrCreateConfiguration().PromptOn403,
                    "When the API key is not provided, the user should be prompted on HTTP 403.");
            }
        }

        [Fact]
        public async Task PackageUpdateResource_AllowsApiKeyWhenPushingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://www.nuget.org/api/v2";
                HttpRequestMessage actualRequest = null;
                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://www.nuget.org/api/v2/",
                        request =>
                        {
                            actualRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    }
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                var apiKey = "SomeApiKey";

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");

                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: null,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => null,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                Assert.NotNull(actualRequest);
                Assert.Equal(HttpMethod.Put, actualRequest.Method);

                IEnumerable<string> values;
                actualRequest.Headers.TryGetValues(ProtocolConstants.ApiKeyHeader, out values);
                Assert.Equal(1, values.Count());
                Assert.Equal(apiKey, values.First());

                Assert.False(
                    actualRequest.GetOrCreateConfiguration().PromptOn403,
                    "When the API key is provided, the user should not be prompted on HTTP 403.");
            }
        }

        [Fact]
        public async Task PackageUpdateResource_AllowsNoApiKeyWhenPushingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://www.nuget.org/api/v2";
                HttpRequestMessage actualRequest = null;
                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://www.nuget.org/api/v2/",
                        request =>
                        {
                            actualRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    }
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                var apiKey = string.Empty;

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");

                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: null,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => null,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                Assert.NotNull(actualRequest);
                Assert.Equal(HttpMethod.Put, actualRequest.Method);

                IEnumerable<string> values;
                actualRequest.Headers.TryGetValues(ProtocolConstants.ApiKeyHeader, out values);
                Assert.Null(values);

                Assert.True(
                    actualRequest.GetOrCreateConfiguration().PromptOn403,
                    "When the API key is not provided, the user should be prompted on HTTP 403.");
            }
        }

        [Theory(Skip = "https://github.com/NuGet/Home/issues/10706")]
        [InlineData("https://nuget.smbsrc.net/")]
        [InlineData("http://nuget.smbsrc.net/")]
        [InlineData("https://nuget.smbsrc.net")]
        [InlineData("https://nuget.smbsrc.net/api/v2/package/")]
        public async Task PackageUpdateResource_SourceAndSymbolNuGetOrgPushingAsync(string symbolSource)
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://www.nuget.org/api/v2";
                HttpRequestMessage sourceRequest = null;
                HttpRequestMessage symbolRequest = null;
                var apiKey = "serverapikey";

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");
                var symbolPackageInfo = await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://www.nuget.org/api/v2/",
                        request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://nuget.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "http://nuget.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://www.nuget.org/api/v2/package/create-verification-key/test/1.0.0",
                        request =>
                        {
                            var content = new StringContent(string.Format(JsonData.TempApiKeyJsonData,"tempkey"), Encoding.UTF8, "application/json");
                            var response = new HttpResponseMessage(HttpStatusCode.OK){
                            Content = content
};
                            return Task.FromResult(response);
                        }
                    }

                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));

                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: symbolSource,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => apiKey,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                IEnumerable<string> apiValues;
                IEnumerable<string> symbolClientVersionValues;
                IEnumerable<string> sourceClientVersionValues;
                symbolRequest.Headers.TryGetValues(ApiKeyHeader, out apiValues);
                symbolRequest.Headers.TryGetValues(NuGetClientVersionHeader, out symbolClientVersionValues);
                sourceRequest.Headers.TryGetValues(NuGetClientVersionHeader, out sourceClientVersionValues);

                Assert.Equal("tempkey", apiValues.First());
                Assert.NotNull(symbolClientVersionValues.First());
                Assert.NotNull(sourceClientVersionValues.First());
            }
        }

        [Fact]
        public async Task PackageUpdateResource_NuGetOrgSourceOnlyPushingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://www.nuget.org/api/v2";
                var symbolSource = "https://other.smbsrc.net/";
                HttpRequestMessage sourceRequest = null;
                HttpRequestMessage symbolRequest = null;
                var apiKey = "serverapikey";

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");
                var symbolPackageInfo = await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://www.nuget.org/api/v2/",
                        request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://other.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));
                var logger = new TestLogger();
                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: symbolSource,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => apiKey,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: logger);

                // Assert
                IEnumerable<string> apiValues;
                IEnumerable<string> symbolClientVersionValues;
                IEnumerable<string> sourceClientVersionValues;
                symbolRequest.Headers.TryGetValues(ApiKeyHeader, out apiValues);
                symbolRequest.Headers.TryGetValues(NuGetClientVersionHeader, out symbolClientVersionValues);
                sourceRequest.Headers.TryGetValues(NuGetClientVersionHeader, out sourceClientVersionValues);

                Assert.Equal("serverapikey", apiValues.First());
                Assert.NotNull(symbolClientVersionValues.First());
                Assert.NotNull(sourceClientVersionValues.First());
                Assert.Equal(0, logger.WarningMessages.Count);
            }
        }

        [Fact]
        public async Task PackageUpdateResource_SymbolSourceOnlyPushingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://www.myget.org/api/v2";
                var symbolSource = "https://nuget.smbsrc.net/";
                HttpRequestMessage sourceRequest = null;
                HttpRequestMessage symbolRequest = null;
                var apiKey = "serverapikey";

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");
                var symbolPackageInfo = await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://www.myget.org/api/v2/",
                        request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://nuget.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://www.nuget.org/api/v2/package/create-verification-key/test/1.0.0",
                        request =>
                        {
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden));
                        }
                    }

                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));

                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: symbolSource,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => apiKey,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                IEnumerable<string> apiValues;
                IEnumerable<string> symbolClientVersionValues;
                symbolRequest.Headers.TryGetValues(ApiKeyHeader, out apiValues);
                symbolRequest.Headers.TryGetValues(NuGetClientVersionHeader, out symbolClientVersionValues);

                Assert.Equal("invalidapikey", apiValues.First());
                Assert.NotNull(symbolClientVersionValues.First());

            }
        }

        [Theory]
        [InlineData("https://nuget.smbsrc.net/")]
        [InlineData("http://nuget.smbsrc.net/")]
        [InlineData("https://nuget.smbsrc.net")]
        [InlineData("https://nuget.smbsrc.net/api/v2/package/")]
        public async Task PackageUpdateResource_NoSymbolSourcePushingSymbolAsync(string source)
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                HttpRequestMessage symbolRequest = null;
                var apiKey = "serverapikey";
                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");
                var symbolPackageInfo = await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://nuget.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "http://nuget.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://www.nuget.org/api/v2/package/create-verification-key/test/1.0.0",
                        request =>
                        {
                            var content = new StringContent(string.Format(JsonData.TempApiKeyJsonData,"tempkey"), Encoding.UTF8, "application/json");
                            var response = new HttpResponseMessage(HttpStatusCode.OK){
                            Content = content
};
                            return Task.FromResult(response);
                        }
                    }

                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));

                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: null,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => null,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                IEnumerable<string> apiValues;
                IEnumerable<string> symbolClientVersionValues;
                symbolRequest.Headers.TryGetValues(ApiKeyHeader, out apiValues);
                symbolRequest.Headers.TryGetValues(NuGetClientVersionHeader, out symbolClientVersionValues);

                Assert.Equal("tempkey", apiValues.First());
                Assert.NotNull(symbolClientVersionValues.First());
            }
        }

        [Fact]
        public async Task PackageUpdateResource_NoServiceEndpointOnCustomServerAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://nuget.customsrc.net/";

                HttpRequestMessage sourceRequest = null;
                var apiKey = "serverapikey";

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://nuget.customsrc.net/",
                        request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    }
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));

                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: null,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => null,
                    noServiceEndpoint: true,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                Assert.NotNull(sourceRequest);
                Assert.Equal(HttpMethod.Put, sourceRequest.Method);
                Assert.Equal(source, sourceRequest.RequestUri.AbsoluteUri);

            }
        }

        [Fact]
        public async Task PackageUpdateResource_NoServiceEndpointOnCustomServer_ShouldAddEndpointAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://nuget.customsrc.net/";

                HttpRequestMessage sourceRequest = null;
                var apiKey = "serverapikey";

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://nuget.customsrc.net/api/v2/package/",
                        request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    }
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));

                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: null,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => null,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                Assert.NotNull(sourceRequest);
                Assert.Equal(HttpMethod.Put, sourceRequest.Method);
                Assert.Equal(source + "api/v2/package/", sourceRequest.RequestUri.AbsoluteUri);

            }
        }

        [Fact]
        public async Task PackageUpdateResource_PackageNotExistOnNuGetOrgPushingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://nuget.smbsrc.net/";

                HttpRequestMessage symbolRequest = null;
                var apiKey = "serverapikey";

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");
                var symbolPackageInfo = await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://nuget.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://www.nuget.org/api/v2/package/create-verification-key/test/1.0.0",
                        request =>
                        {
                            var content = new StringContent(string.Format(JsonData.TempApiKeyJsonData, "tempkey"), Encoding.UTF8, "application/json");
                            var response = new HttpResponseMessage(HttpStatusCode.OK){
                            Content = content
};
                            return Task.FromResult(response);
                        }
                    }

                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));

                // Act
                await resource.Push(
                    packagePaths: new[] { packageInfo.FullName },
                    symbolSource: null,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => null,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                IEnumerable<string> apiValues;
                IEnumerable<string> symbolClientVersionValues;
                symbolRequest.Headers.TryGetValues(ApiKeyHeader, out apiValues);
                symbolRequest.Headers.TryGetValues(NuGetClientVersionHeader, out symbolClientVersionValues);

                Assert.Equal("tempkey", apiValues.First());
                Assert.NotNull(symbolClientVersionValues.First());
            }
        }

        [Fact]
        public async Task PackageUpdateResource_GetErrorFromCreateKeyPushingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://www.myget.org/api/v2";
                var symbolSource = "https://nuget.smbsrc.net/";
                HttpRequestMessage sourceRequest = null;
                HttpRequestMessage symbolRequest = null;
                var apiKey = "serverapikey";

                var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");
                var symbolPackageInfo =
                    await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://www.myget.org/api/v2/", request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://nuget.smbsrc.net/api/v2/package/", request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://www.nuget.org/api/v2/package/create-verification-key/test/1.0.0",
                        request =>
                        {
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                        }
                    }
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));

                // Act
                var ex = await Assert.ThrowsAsync<HttpRequestException>(
                    async () => await resource.Push(
                        packagePaths: new[] { packageInfo.FullName },
                        symbolSource: symbolSource,
                        timeoutInSecond: 5,
                        disableBuffering: false,
                        getApiKey: _ => apiKey,
                        getSymbolApiKey: _ => apiKey,
                        noServiceEndpoint: false,
                        skipDuplicate: false,
                        symbolPackageUpdateResource: null,
                        log: NullLogger.Instance));

                // Assert
                Assert.Contains("Response status code does not indicate success: 500 (Internal Server Error)", ex.Message);
            }
        }

        [Fact]
        public async Task PackageUpdateResource_RetryNuGetSymbolPushingAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "https://nuget.smbsrc.net/";
                var symbolRequest = new List<HttpRequestMessage>();
                var apiKey = "serverapikey";
                var symbolSourceRequestCount = 0;
                var createKeyRequestCount = 0;

                var symbolPackageInfo = await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "https://nuget.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest.Add(request);
                            symbolSourceRequestCount++;
                            if (symbolSourceRequestCount < 3)
                            {
                                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                            }
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "https://www.nuget.org/api/v2/package/create-verification-key/test/1.0.0",
                         request =>
                         {
                            createKeyRequestCount++;
                            var content = new StringContent(string.Format(JsonData.TempApiKeyJsonData, $"tempkey{createKeyRequestCount}"), Encoding.UTF8, "application/json");
                            var response = new HttpResponseMessage(HttpStatusCode.OK){
                            Content = content
};
                            return Task.FromResult(response);
                         }
                    }

                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));

                // Act
                await resource.Push(
                    packagePaths: new[] { symbolPackageInfo.FullName },
                    symbolSource: null,
                    timeoutInSecond: 5,
                    disableBuffering: false,
                    getApiKey: _ => apiKey,
                    getSymbolApiKey: _ => null,
                    noServiceEndpoint: false,
                    skipDuplicate: false,
                    symbolPackageUpdateResource: null,
                    log: NullLogger.Instance);

                // Assert
                var apikeys = new List<string>();

                Assert.Equal(3, symbolRequest.Count);

                IEnumerable<string> apiValues;
                for (var i = 1; i <= 3; i++)
                {
                    symbolRequest[i - 1].Headers.TryGetValues(ApiKeyHeader, out apiValues);
                    Assert.Equal($"tempkey{i}", apiValues.First());
                }
            }
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task Push_WithAnHttpSourceAndAllowInsecureConnections_NupkgOnly_Warns(bool allowInsecureConnections, bool isHttpWarningExpected)
        {
            // Arrange
            using var workingDir = TestDirectory.Create();
            var source = "http://www.nuget.org/api/v2/";
            HttpRequestMessage sourceRequest = null;
            var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");

            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        source,
                        request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                };
            var resource = await StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses).GetResourceAsync<PackageUpdateResource>();
            var logger = new TestLogger();

            // Act
            await resource.Push(
                packagePaths: new[] { packageInfo.FullName },
                symbolSource: string.Empty,
                timeoutInSecond: 5,
                disableBuffering: false,
                getApiKey: _ => "serverapikey",
                getSymbolApiKey: _ => null,
                noServiceEndpoint: false,
                skipDuplicate: false,
                symbolPackageUpdateResource: null,
                allowInsecureConnections: allowInsecureConnections,
                log: logger);

            // Assert
            Assert.NotNull(sourceRequest);
            if (isHttpWarningExpected)
            {
                Assert.Equal(1, logger.WarningMessages.Count);
                Assert.Contains("You are running the 'push' operation with an 'HTTP' source", logger.WarningMessages.Single());
            }
            else
            {
                Assert.Equal(0, logger.WarningMessages.Count);
            }

        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task Push_WhenPushingToAnHttpSymbolSourceAndAllowInsecureConnections_Warns(bool allowInsecureConnections, bool isHttpWarningExpected)
        {
            // Arrange
            using var workingDir = TestDirectory.Create();
            var source = "https://www.nuget.org/api/v2/";
            var symbolSource = "http://other.smbsrc.net/";
            HttpRequestMessage sourceRequest = null;
            HttpRequestMessage symbolRequest = null;
            var apiKey = "serverapikey";

            var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");
            var symbolPackageInfo = await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        source,
                        request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "http://other.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                };

            var resource = await StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses).GetResourceAsync<PackageUpdateResource>();
            UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));
            var logger = new TestLogger();

            // Act
            await resource.Push(
                packagePaths: new[] { packageInfo.FullName },
                symbolSource: symbolSource,
                timeoutInSecond: 5,
                disableBuffering: false,
                getApiKey: _ => apiKey,
                getSymbolApiKey: _ => apiKey,
                noServiceEndpoint: false,
                skipDuplicate: false,
                symbolPackageUpdateResource: null,
                allowInsecureConnections: allowInsecureConnections,
                log: logger);

            // Assert
            Assert.NotNull(sourceRequest);
            Assert.NotNull(symbolRequest);
            if (isHttpWarningExpected)
            {
                Assert.Equal(1, logger.WarningMessages.Count);
                Assert.Contains("You are running the 'push' operation with an 'HTTP' source", logger.WarningMessages.Single());
            }
            else
            {
                Assert.Equal(0, logger.WarningMessages.Count);
            }
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task Push_WhenPushingToAnHttpSourceAndSymbolSourceWithAllowInsecureConnections_WarnsForBoth(bool allowInsecureConnections, bool isHttpWarningExpected)
        {
            // Arrange
            using var workingDir = TestDirectory.Create();
            var source = "http://www.nuget.org/api/v2/";
            var symbolSource = "http://other.smbsrc.net/";
            HttpRequestMessage sourceRequest = null;
            HttpRequestMessage symbolRequest = null;
            var apiKey = "serverapikey";

            var packageInfo = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test", "1.0.0");
            var symbolPackageInfo = await SimpleTestPackageUtility.CreateSymbolPackageAsync(workingDir, "test", "1.0.0");

            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        source,
                        request =>
                        {
                            sourceRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                    {
                        "http://other.smbsrc.net/api/v2/package/",
                        request =>
                        {
                            symbolRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    },
                };

            var resource = await StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses).GetResourceAsync<PackageUpdateResource>();
            UserAgent.SetUserAgentString(new UserAgentStringBuilder("test client"));
            var logger = new TestLogger();

            // Act
            await resource.Push(
                packagePaths: new[] { packageInfo.FullName },
                symbolSource: symbolSource,
                timeoutInSecond: 5,
                disableBuffering: false,
                getApiKey: _ => apiKey,
                getSymbolApiKey: _ => apiKey,
                noServiceEndpoint: false,
                skipDuplicate: false,
                symbolPackageUpdateResource: null,
                allowInsecureConnections: allowInsecureConnections,
                log: logger);

            // Assert
            Assert.NotNull(sourceRequest);
            Assert.NotNull(symbolRequest);
            if (isHttpWarningExpected)
            {
                Assert.Equal(2, logger.WarningMessages.Count);
                Assert.Contains("You are running the 'push' operation with an 'HTTP' source, 'http://www.nuget.org/api/v2/'. Non-HTTPS access will be removed in a future version. Consider migrating to an 'HTTPS' source.", logger.WarningMessages.First());
                Assert.Contains("You are running the 'push' operation with an 'HTTP' source, 'http://other.smbsrc.net/api/v2/package/'. Non-HTTPS access will be removed in a future version. Consider migrating to an 'HTTPS' source.", logger.WarningMessages.Last());
            }
            else
            {
                Assert.Equal(0, logger.WarningMessages.Count);
            }
        }

        [Fact]
        public async Task Delete_WhenDeletingFromHTTPSource_Warns()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var source = "http://www.nuget.org/api/v2";
                HttpRequestMessage actualRequest = null;
                var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
                {
                    {
                        "http://www.nuget.org/api/v2/DeepEqual/1.4.0.1-rc",
                        request =>
                        {
                            actualRequest = request;
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                        }
                    }
                };

                var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
                var resource = await repo.GetResourceAsync<PackageUpdateResource>();
                var apiKey = string.Empty;
                var logger = new TestLogger();

                // Act
                await resource.Delete(
                    packageId: "DeepEqual",
                    packageVersion: "1.4.0.1-rc",
                    getApiKey: _ => apiKey,
                    confirm: _ => true,
                    noServiceEndpoint: false,
                    log: logger);

                // Assert
                Assert.NotNull(actualRequest);
                Assert.Equal(HttpMethod.Delete, actualRequest.Method);
                Assert.Equal(3, logger.WarningMessages.Count);
                Assert.Contains("You are running the 'delete' operation with an 'HTTP' source, 'http://www.nuget.org/api/v2/'. Non-HTTPS access will be removed in a future version. Consider migrating to an 'HTTPS' source.", logger.WarningMessages.Last());
            }
        }
    }
}
