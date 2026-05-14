// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetPackageVersionsRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageVersionsRequest(
                    packageSourceRepository,
                    packageId: "a"));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageId(string packageId)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageVersionsRequest(
                    packageSourceRepository: "a",
                    packageId: packageId));

            Assert.Equal("packageId", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new GetPackageVersionsRequest(
                packageSourceRepository: "a",
                packageId: "b");

            Assert.Equal("a", request.PackageSourceRepository);
            Assert.Equal("b", request.PackageId);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new GetPackageVersionsRequest(
                packageSourceRepository: "a",
                packageId: "b");

            var json = TestUtilities.Serialize(request);

            Assert.Equal("{\"PackageId\":\"b\",\"PackageSourceRepository\":\"a\"}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"PackageId\":\"a\",\"PackageSourceRepository\":\"b\"}";
            var request = JsonSerializationUtilities.Deserialize<GetPackageVersionsRequest>(json);

            Assert.Equal("b", request.PackageSourceRepository);
            Assert.Equal("a", request.PackageId);
        }

        [Theory]
        [InlineData("{}", "packageSourceRepository")]
        [InlineData("{\"PackageSourceRepository\":\"b\"}", "packageId")]
        [InlineData("{\"PackageId\":null,\"PackageSourceRepository\":\"b\"}", "packageId")]
        [InlineData("{\"PackageId\":\"\",\"PackageSourceRepository\":\"b\"}", "packageId")]
        [InlineData("{\"PackageId\":\"a\"}", "packageSourceRepository")]
        [InlineData("{\"PackageId\":\"a\",\"PackageSourceRepository\":null}", "packageSourceRepository")]
        [InlineData("{\"PackageId\":\"a\",\"PackageSourceRepository\":\"\"}", "packageSourceRepository")]
        public void JsonDeserialization_ThrowsForInvalidPackageSourceRepository(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetPackageVersionsRequest>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }
    }
}
