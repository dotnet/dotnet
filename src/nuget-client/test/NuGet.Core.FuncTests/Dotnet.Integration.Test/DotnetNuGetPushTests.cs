// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetNuGetPushTests
    {
        private readonly DotnetIntegrationTestFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public DotnetNuGetPushTests(DotnetIntegrationTestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task PushCommand_ApiKeyFromEnvironmentVariable()
        {
            // Test that the API keys are read from environment variables
            using var pathContext = _fixture.CreateSimpleTestPathContext();
            using var server = new MockServer();

            // Arrange
            var packageId = "TestPackage";
            var packageVersion = "1.0.0";
            var package = new SimpleTestPackageContext(packageId, packageVersion);
            await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, package);
            var packagePath = Path.Combine(pathContext.PackageSource, $"{packageId}.{packageVersion}.nupkg");

            // Create symbols package
            var symbolPackagePath = packagePath.Replace(".nupkg", ".symbols.nupkg");
            File.Copy(packagePath, symbolPackagePath);

            string capturedApiKey = null;
            string capturedSymbolApiKey = null;

            server.Get.Add("/push", r => "OK");
            server.Put.Add("/push", r =>
            {
                capturedApiKey = r.Headers["X-NuGet-ApiKey"];
                return HttpStatusCode.Created;
            });

            server.Get.Add("/symbols", r => "OK");
            server.Put.Add("/symbols", r =>
            {
                capturedSymbolApiKey = r.Headers["X-NuGet-ApiKey"];
                return HttpStatusCode.Created;
            });

            pathContext.Settings.AddSource("http-feed", $"{server.Uri}push", allowInsecureConnectionsValue: "true");
            server.Start();

            var environmentVariables = new Dictionary<string, string>
            {
                { "NUGET_API_KEY", "EnvApiKey123" },
                { "NUGET_SYMBOL_API_KEY", "EnvSymbolApiKey456" }
            };

            // Act
            var args = $"nuget push {packagePath} --source {server.Uri}push --symbol-source {server.Uri}symbols";
            var result = _fixture.RunDotnetExpectSuccess(pathContext.WorkingDirectory, args, environmentVariables, _testOutputHelper);

            // Assert
            Assert.Equal("EnvApiKey123", capturedApiKey);
            Assert.Equal("EnvSymbolApiKey456", capturedSymbolApiKey);
        }

        [PlatformFact(Platform.Windows)]
        public async Task PushCommand_CommandLineApiKeyTakesPrecedenceOverEnvironmentVariable()
        {
            // Test that command line API keys take precedence over environment variables
            using var pathContext = _fixture.CreateSimpleTestPathContext();
            using var server = new MockServer();

            // Arrange
            var packageId = "TestPackage";
            var packageVersion = "1.0.0";
            var package = new SimpleTestPackageContext(packageId, packageVersion);
            await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, package);
            var packagePath = Path.Combine(pathContext.PackageSource, $"{packageId}.{packageVersion}.nupkg");

            // Create symbols package
            var symbolPackagePath = packagePath.Replace(".nupkg", ".symbols.nupkg");
            File.Copy(packagePath, symbolPackagePath);

            string capturedApiKey = null;
            string capturedSymbolApiKey = null;

            server.Get.Add("/push", r => "OK");
            server.Put.Add("/push", r =>
            {
                capturedApiKey = r.Headers["X-NuGet-ApiKey"];
                return HttpStatusCode.Created;
            });

            server.Get.Add("/symbols", r => "OK");
            server.Put.Add("/symbols", r =>
            {
                capturedSymbolApiKey = r.Headers["X-NuGet-ApiKey"];
                return HttpStatusCode.Created;
            });

            pathContext.Settings.AddSource("http-feed", $"{server.Uri}push", allowInsecureConnectionsValue: "true");
            server.Start();

            var environmentVariables = new Dictionary<string, string>
            {
                { "NUGET_API_KEY", "EnvApiKey123" },
                { "NUGET_SYMBOL_API_KEY", "EnvSymbolApiKey456" }
            };

            // Act
            var args = $"nuget push {packagePath} --source {server.Uri}push --symbol-source {server.Uri}symbols --api-key CommandLineKey --symbol-api-key CommandLineSymbolKey";
            var result = _fixture.RunDotnetExpectSuccess(pathContext.WorkingDirectory, args, environmentVariables, _testOutputHelper);

            // Assert
            Assert.Equal("CommandLineKey", capturedApiKey);
            Assert.Equal("CommandLineSymbolKey", capturedSymbolApiKey);
        }
    }
}
