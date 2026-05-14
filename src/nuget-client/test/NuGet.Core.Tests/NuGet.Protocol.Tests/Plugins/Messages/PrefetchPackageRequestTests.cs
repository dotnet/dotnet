// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PrefetchPackageRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PrefetchPackageRequest(
                    packageSourceRepository,
                    packageId: "a",
                    packageVersion: "b"));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageId(string packageId)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PrefetchPackageRequest(
                    packageSourceRepository: "a",
                    packageId: packageId,
                    packageVersion: "b"));

            Assert.Equal("packageId", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageVersion(string packageVersion)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PrefetchPackageRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: packageVersion));

            Assert.Equal("packageVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new PrefetchPackageRequest(
                packageSourceRepository: "a",
                packageId: "b",
                packageVersion: "c");

            Assert.Equal("a", request.PackageSourceRepository);
            Assert.Equal("b", request.PackageId);
            Assert.Equal("c", request.PackageVersion);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new PrefetchPackageRequest(
                packageSourceRepository: "a",
                packageId: "b",
                packageVersion: "c");

            var json = TestUtilities.Serialize(request);

            Assert.Equal("{\"PackageId\":\"b\",\"PackageSourceRepository\":\"a\",\"PackageVersion\":\"c\"}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"PackageId\":\"a\",\"PackageSourceRepository\":\"b\",\"PackageVersion\":\"c\"}";
            var request = JsonSerializationUtilities.Deserialize<PrefetchPackageRequest>(json);

            Assert.Equal("a", request.PackageId);
            Assert.Equal("b", request.PackageSourceRepository);
            Assert.Equal("c", request.PackageVersion);
        }

        [Theory]
        [InlineData("{\"PackageId\":\"b\",\"PackageVersion\":\"c\"}", "packageSourceRepository")]
        [InlineData("{\"PackageSourceRepository\":null,\"PackageId\":\"b\",\"PackageVersion\":\"c\"}", "packageSourceRepository")]
        [InlineData("{\"PackageSourceRepository\":\"\",\"PackageId\":\"b\",\"PackageVersion\":\"c\"}", "packageSourceRepository")]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"PackageVersion\":\"c\"}", "packageId")]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"PackageId\":null,\"PackageVersion\":\"c\"}", "packageId")]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"PackageId\":\"\",\"PackageVersion\":\"c\"}", "packageId")]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"PackageId\":\"b\"}", "packageVersion")]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"PackageId\":\"b\",\"PackageVersion\":null}", "packageVersion")]
        [InlineData("{\"PackageSourceRepository\":\"a\",\"PackageId\":\"b\",\"PackageVersion\":\"\"}", "packageVersion")]
        public void JsonDeserialization_ThrowsForInvalidStringArgument(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<PrefetchPackageRequest>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }
    }
}
