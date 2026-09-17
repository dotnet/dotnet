// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests.Resources
{
    public class PackageStagingResourceV3Tests
    {
        [Fact]
        public async Task PushPackageAsync_WhenEndpointHasQuery_ShouldPreserveQueryAndSendExpectedMultipartRequest()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            string packagePath = Path.Combine(pathContext.WorkingDirectory, "Example.1.0.0.nupkg");
            File.WriteAllText(packagePath, "package");
            HttpRequestMessage? capturedRequest = null;
            bool isMultipartRequest = false;
            string? packageContent = null;
            string? groupContent = null;
            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
            {
                ["https://unit.test/staging/package?token=abc"] = async request =>
                {
                    capturedRequest = request;
                    MultipartFormDataContent? multipart = request.Content as MultipartFormDataContent;
                    isMultipartRequest = multipart is not null;
                    HttpContent? package = multipart?.FirstOrDefault(part => part.Headers.ContentDisposition?.Name?.Trim('"') == "package");
                    HttpContent? group = multipart?.FirstOrDefault(part => part.Headers.ContentDisposition?.Name?.Trim('"') == "groupId");
                    packageContent = package is null ? null : await package.ReadAsStringAsync();
                    groupContent = group is null ? null : await group.ReadAsStringAsync();

                    return new HttpResponseMessage(HttpStatusCode.Created);
                },
            };
            using var httpSource = new TestHttpSource(new PackageSource("https://unit.test/staging/"), responses);
            var resource = new PackageStagingResourceV3(
                endpoint: new Uri("https://unit.test/staging/?token=abc"),
                httpSource: httpSource);

            // Act
            await resource.PushPackageAsync(
                packagePath: packagePath,
                apiKey: "secret",
                groupId: "release-group",
                requestTimeout: TimeSpan.FromMinutes(5),
                allowInsecureConnections: false,
                logger: NullLogger.Instance,
                cancellationToken: CancellationToken.None);

            // Assert
            capturedRequest.Should().NotBeNull();
            capturedRequest!.Method.Should().Be(HttpMethod.Put);
            capturedRequest.RequestUri!.AbsoluteUri.Should().Be("https://unit.test/staging/package?token=abc");
            capturedRequest.Headers.GetValues(ProtocolConstants.ApiKeyHeader).Should().ContainSingle("secret");
            isMultipartRequest.Should().BeTrue();
            packageContent.Should().Be("package");
            groupContent.Should().Be("release-group");
        }

        [Fact]
        public async Task PushSymbolsAsync_WhenOptionalFieldsAreNull_ShouldOmitOptionalFieldsAndUseSymbolsPart()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            string packagePath = Path.Combine(pathContext.WorkingDirectory, "Example.1.0.0.snupkg");
            File.WriteAllText(packagePath, "symbols");
            HttpRequestMessage? capturedRequest = null;
            List<string?>? partNames = null;
            string? symbolsContent = null;
            bool hasApiKeyHeader = false;
            bool isMultipartRequest = false;
            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
            {
                ["https://unit.test/staging/symbols"] = async request =>
                {
                    capturedRequest = request;
                    MultipartFormDataContent? multipart = request.Content as MultipartFormDataContent;
                    isMultipartRequest = multipart is not null;
                    partNames = multipart?
                        .Select(part => part.Headers.ContentDisposition?.Name?.Trim('"'))
                        .ToList();
                    HttpContent? symbols = multipart?.FirstOrDefault(
                        part => part.Headers.ContentDisposition?.Name?.Trim('"') == "symbols");
                    symbolsContent = symbols is null ? null : await symbols.ReadAsStringAsync();
                    hasApiKeyHeader = request.Headers.Contains(ProtocolConstants.ApiKeyHeader);

                    return new HttpResponseMessage(HttpStatusCode.OK);
                },
            };
            using var httpSource = new TestHttpSource(new PackageSource("https://unit.test/staging"), responses);
            var resource = new PackageStagingResourceV3(
                endpoint: new Uri("https://unit.test/staging"),
                httpSource: httpSource);

            // Act
            await resource.PushSymbolsAsync(
                packagePath: packagePath,
                apiKey: null,
                groupId: null,
                requestTimeout: TimeSpan.FromMinutes(5),
                allowInsecureConnections: false,
                logger: NullLogger.Instance,
                cancellationToken: CancellationToken.None);

            // Assert
            capturedRequest.Should().NotBeNull();
            capturedRequest!.Method.Should().Be(HttpMethod.Put);
            capturedRequest.RequestUri!.AbsoluteUri.Should().Be("https://unit.test/staging/symbols");
            isMultipartRequest.Should().BeTrue();
            partNames.Should().ContainSingle().Which.Should().Be("symbols");
            symbolsContent.Should().Be("symbols");
            hasApiKeyHeader.Should().BeFalse();
            partNames.Should().NotContain("groupId");
        }

        [Fact]
        public async Task PushPackageAsync_WhenEndpointIsHttpWithoutOptIn_ShouldThrow()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            string packagePath = Path.Combine(pathContext.WorkingDirectory, "Example.1.0.0.nupkg");
            File.WriteAllText(packagePath, "package");
            using var httpSource = new TestHttpSource(
                new PackageSource("https://unit.test/v3/index.json"),
                new Dictionary<string, string>());
            var resource = new PackageStagingResourceV3(
                endpoint: new Uri("http://unit.test/staging/"),
                httpSource: httpSource);

            // Act
            Exception? exception = await Record.ExceptionAsync(
                () => resource.PushPackageAsync(
                    packagePath: packagePath,
                    apiKey: "secret",
                    groupId: null,
                    requestTimeout: TimeSpan.FromMinutes(5),
                    allowInsecureConnections: false,
                    logger: NullLogger.Instance,
                    cancellationToken: CancellationToken.None));

            // Assert
            FatalProtocolException fatalProtocolException = exception.Should().BeOfType<FatalProtocolException>().Subject;
            fatalProtocolException.Message.Should().Contain("HTTP").And.Contain("http://unit.test/staging/package");
        }

        [Fact]
        public async Task PushPackageAsync_WhenEndpointIsHttpWithOptIn_ShouldSucceed()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            string packagePath = Path.Combine(pathContext.WorkingDirectory, "Example.1.0.0.nupkg");
            File.WriteAllText(packagePath, "package");
            bool requestAttempted = false;
            Uri? requestUri = null;
            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
            {
                ["http://unit.test/staging/package"] = request =>
                {
                    requestAttempted = true;
                    requestUri = request.RequestUri;
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                },
            };
            using var httpSource = new TestHttpSource(
                new PackageSource("https://unit.test/v3/index.json"),
                responses);
            var resource = new PackageStagingResourceV3(
                endpoint: new Uri("http://unit.test/staging/"),
                httpSource: httpSource);

            // Act
            await resource.PushPackageAsync(
                packagePath: packagePath,
                apiKey: "secret",
                groupId: null,
                requestTimeout: TimeSpan.FromMinutes(5),
                allowInsecureConnections: true,
                logger: NullLogger.Instance,
                cancellationToken: CancellationToken.None);

            // Assert
            requestAttempted.Should().BeTrue();
            requestUri.Should().Be(new Uri("http://unit.test/staging/package"));
        }
    }
}
