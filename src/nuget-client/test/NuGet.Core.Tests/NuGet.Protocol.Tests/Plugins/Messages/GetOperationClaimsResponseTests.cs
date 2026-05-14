// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetOperationClaimsResponseTests
    {
        [Fact]
        public void Constructor_ThrowsForNullClaims()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new GetOperationClaimsResponse(claims: null));

            Assert.Equal("claims", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForUndefinedClaims()
        {
            var claims = new[] { (OperationClaim)int.MaxValue };
            var exception = Assert.Throws<ArgumentException>(() => new GetOperationClaimsResponse(claims));

            Assert.Equal("claims", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesClaimsProperty()
        {
            var claims = new[] { OperationClaim.DownloadPackage };
            var response = new GetOperationClaimsResponse(claims);

            Assert.Equal(1, response.Claims.Count);
            Assert.Equal(claims[0], response.Claims[0]);
        }

        [Fact]
        public void Constructor_ClonesClaims()
        {
            var claims = new[] { OperationClaim.DownloadPackage };
            var response = new GetOperationClaimsResponse(claims);

            Assert.NotSame(claims, response.Claims);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var response = new GetOperationClaimsResponse(new[] { OperationClaim.DownloadPackage });

            var json = TestUtilities.Serialize(response);

            Assert.Equal("{\"Claims\":[\"DownloadPackage\"]}", json);
        }

        [Theory]
        [InlineData("{\"Claims\":[]}", new OperationClaim[] { })]
        [InlineData("{\"Claims\":[\"DownloadPackage\"]}", new[] { OperationClaim.DownloadPackage })]
        public void JsonDeserialization_ReturnsCorrectObject(string json, OperationClaim[] claims)
        {
            var response = JsonSerializationUtilities.Deserialize<GetOperationClaimsResponse>(json);

            Assert.Equal(claims.Length, response.Claims.Count);

            foreach (var claim in claims)
            {
                Assert.Contains(claim, response.Claims);
            }
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"Claims\":null}")]
        [InlineData("{\"Claims\":\"\"}")]
        public void JsonDeserialization_ThrowsForInvalidClaims(string json)
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => JsonSerializationUtilities.Deserialize<GetOperationClaimsResponse>(json));

            Assert.Equal("claims", exception.ParamName);
        }

        [Fact]
        public void JsonDeserialization_ThrowsForInvalidClaimsValue()
        {
            var json = "{\"Claims\":[\"abc\"]}";

            Assert.Throws<JsonSerializationException>(
                () => JsonSerializationUtilities.Deserialize<GetOperationClaimsResponse>(json));
        }
    }
}
