// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Protocol.Plugins;
using NuGet.Protocol.Plugins.Tests;
using Xunit;

namespace NuGet.Protocol.Tests.Plugins
{
    public class GetAuthenticationCredentialsRequestTests
    {

        [Fact]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository()
        {
            Uri uri = null;
            var exception = Assert.Throws<ArgumentNullException>(
                () => new GetAuthenticationCredentialsRequest(
                    uri: uri,
                    isRetry: false,
                    isNonInteractive: false,
                    canShowDialog: false
                    ));
            Assert.Equal("uri", exception.ParamName);
        }

        [Theory]
        [InlineData(@"http://api.nuget.org/v3/index.json", false, false, false)]
        [InlineData(@"http://api.nuget.org/v3/index.json", true, false, false)]
        [InlineData(@"http://api.nuget.org/v3/index.json", false, false, true)]
        [InlineData(@"http://api.nuget.org/v3/index.json", true, false, true)]
        [InlineData(@"http://api.nuget.org/v3/index.json", false, true, false)]
        [InlineData(@"http://api.nuget.org/v3/index.json", true, true, false)]
        [InlineData(@"http://api.nuget.org/v3/index.json", false, true, true)]
        [InlineData(@"http://api.nuget.org/v3/index.json", true, true, true)]
        public void AJsonSerialization_ReturnsCorrectJson(
            string uri,
            bool isRetry,
            bool canShowDialog,
            bool isNonInteractive)
        {
            var expectedJson = "{\"Uri\":\"" + uri + "\",\"IsRetry\":" + isRetry.ToString().ToLowerInvariant() + ",\"IsNonInteractive\":" + isNonInteractive.ToString().ToLowerInvariant() + ",\"CanShowDialog\":" + canShowDialog.ToString().ToLowerInvariant() + "}";

            var request = new GetAuthenticationCredentialsRequest(new Uri(uri), isRetry, isNonInteractive, canShowDialog);


            var actualJson = TestUtilities.Serialize(request);

            Assert.Equal(expectedJson, actualJson);
        }

        [Theory]
        [InlineData("{\"Uri\":\"http://api.nuget.org/v3/index.json\",\"IsRetry\":true,\"IsNonInteractive\":true,\"CanShowDialog\":true}", "http://api.nuget.org/v3/index.json", true, true, true)]
        [InlineData("{\"Uri\":\"http://api.nuget.org/v3/index.json\",\"IsRetry\":true,\"IsNonInteractive\":false,\"CanShowDialog\":true}", "http://api.nuget.org/v3/index.json", true, true, false)]
        [InlineData("{\"Uri\":\"http://api.nuget.org/v3/index.json\",\"IsRetry\":false,\"IsNonInteractive\":false,\"CanShowDialog\":true}", "http://api.nuget.org/v3/index.json", false, true, false)]
        [InlineData("{\"Uri\":\"http://api.nuget.org/v3/index.json\",\"IsRetry\":false,\"IsNonInteractive\":true,\"CanShowDialog\":true}", "http://api.nuget.org/v3/index.json", false, true, true)]
        [InlineData("{\"Uri\":\"http://api.nuget.org/v3/index.json\",\"IsRetry\":true,\"IsNonInteractive\":true,\"CanShowDialog\":false}", "http://api.nuget.org/v3/index.json", true, false, true)]
        [InlineData("{\"Uri\":\"http://api.nuget.org/v3/index.json\",\"IsRetry\":true,\"IsNonInteractive\":false,\"CanShowDialog\":false}", "http://api.nuget.org/v3/index.json", true, false, false)]
        [InlineData("{\"Uri\":\"http://api.nuget.org/v3/index.json\",\"IsRetry\":false,\"IsNonInteractive\":false,\"CanShowDialog\":false}", "http://api.nuget.org/v3/index.json", false, false, false)]
        [InlineData("{\"Uri\":\"http://api.nuget.org/v3/index.json\",\"IsRetry\":false,\"IsNonInteractive\":true,\"CanShowDialog\":false}", "http://api.nuget.org/v3/index.json", false, false, true)]
        public void AJsonDeserialization_ReturnsCorrectObject(
            string json,
            string packageSourceRepository,
            bool isRetry,
            bool canShowDialog,
            bool isNonInteractive)
        {
            var request = JsonSerializationUtilities.Deserialize<GetAuthenticationCredentialsRequest>(json);

            Assert.Equal(packageSourceRepository, request.Uri.ToString());
            Assert.Equal(isRetry, request.IsRetry);
            Assert.Equal(isNonInteractive, request.IsNonInteractive);
            Assert.Equal(canShowDialog, request.CanShowDialog);
        }
    }
}
