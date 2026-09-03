// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class DownloadResourceV3Tests
    {
        [Theory]
        [InlineData("true")]  // STJ path
        [InlineData("false")] // NSJ path
        public async Task GetDownloadResourceResultAsync_FromRegistration_BothPathsAsync(string useStj)
        {
            // Arrange
            using var cacheContext = new SourceCacheContext();
            using var workingDir = TestDirectory.Create();

            var source = "http://testsource.test/v3/index.json";
            var package = await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "DeepEqual", "0.9.0");
            var packageBytes = File.ReadAllBytes(package.FullName);

            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
            {
                {
                    source,
                    _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new TestContent(JsonData.IndexWithoutFlatContainer) })
                },
                {
                    "https://api.nuget.org/v3/registration0/deepequal/index.json",
                    _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new TestContent(JsonData.DeepEqualRegistationIndex) })
                },
                {
                    "https://api.nuget.org/packages/deepequal.0.9.0.nupkg",
                    _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(packageBytes) })
                },
            };

            var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
            HttpSource httpSource = (await repo.GetResourceAsync<HttpSourceResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected HttpSourceResource.")).HttpSource;
            var regResource = await repo.GetResourceAsync<RegistrationResourceV3>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected RegistrationResourceV3.");

            var envReader = new Mock<IEnvironmentVariableReader>();
            envReader.Setup(e => e.GetEnvironmentVariable(NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar)).Returns(useStj);

            var resource = new DownloadResourceV3(source, httpSource, regResource, envReader.Object);

            // Act
            using var packagesFolder = TestDirectory.Create();
            var downloadContext = new PackageDownloadContext(cacheContext);
            using DownloadResourceResult result = await resource.GetDownloadResourceResultAsync(
                new PackageIdentity("deepequal", NuGetVersion.Parse("0.9.0")),
                downloadContext,
                packagesFolder,
                NullLogger.Instance,
                CancellationToken.None);

            // Assert
            Assert.Equal(DownloadResourceResultStatus.Available, result.Status);
            Assert.NotNull(result.PackageReader);
            Assert.Equal("DeepEqual", result.PackageReader!.GetIdentity().Id);
        }
    }
}
