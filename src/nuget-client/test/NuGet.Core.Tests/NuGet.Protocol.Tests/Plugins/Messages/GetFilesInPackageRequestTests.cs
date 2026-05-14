// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetFilesInPackageRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetFilesInPackageRequest(
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
                () => new GetFilesInPackageRequest(
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
                () => new GetFilesInPackageRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: packageVersion));

            Assert.Equal("packageVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new GetFilesInPackageRequest(
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
            var request = new GetFilesInPackageRequest(
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
            var request = JsonSerializationUtilities.Deserialize<GetFilesInPackageRequest>(json);

            Assert.Equal("b", request.PackageSourceRepository);
            Assert.Equal("a", request.PackageId);
            Assert.Equal("c", request.PackageVersion);
        }

        [Theory]
        [InlineData("{}", "packageSourceRepository")]
        [InlineData("{\"PackageSourceRepository\":\"b\",\"PackageVersion\":\"c\"}", "packageId")]
        [InlineData("{\"PackageId\":null,\"PackageSourceRepository\":\"b\",\"PackageVersion\":\"c\"}", "packageId")]
        [InlineData("{\"PackageId\":\"\",\"PackageSourceRepository\":\"b\",\"PackageVersion\":\"c\"}", "packageId")]
        [InlineData("{\"PackageId\":\"a\",\"PackageVersion\":\"c\"}", "packageSourceRepository")]
        [InlineData("{\"PackageId\":\"a\",\"PackageSourceRepository\":null,\"PackageVersion\":\"c\"}", "packageSourceRepository")]
        [InlineData("{\"PackageId\":\"a\",\"PackageSourceRepository\":\"\",\"PackageVersion\":\"c\"}", "packageSourceRepository")]
        [InlineData("{\"PackageId\":\"a\",\"PackageSourceRepository\":\"b\"}", "packageVersion")]
        [InlineData("{\"PackageId\":\"a\",\"PackageSourceRepository\":\"b\",\"PackageVersion\":null}", "packageVersion")]
        [InlineData("{\"PackageId\":\"a\",\"PackageSourceRepository\":\"b\",\"PackageVersion\":\"\"}", "packageVersion")]
        public void JsonDeserialization_ThrowsForInvalidStringArgument(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetFilesInPackageRequest>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }
    }
}
