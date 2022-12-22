// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetPackageHashRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageHashRequest(
                    packageSourceRepository,
                    packageId: "a",
                    packageVersion: "b",
                    hashAlgorithm: "c"));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageId(string packageId)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageHashRequest(
                    packageSourceRepository: "a",
                    packageId: packageId,
                    packageVersion: "b",
                    hashAlgorithm: "c"));

            Assert.Equal("packageId", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageVersion(string packageVersion)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageHashRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: packageVersion,
                    hashAlgorithm: "c"));

            Assert.Equal("packageVersion", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyHashAlgorithm(string hashAlgorithm)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetPackageHashRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: "c",
                    hashAlgorithm: hashAlgorithm));

            Assert.Equal("hashAlgorithm", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new GetPackageHashRequest(
                packageSourceRepository: "a",
                packageId: "b",
                packageVersion: "c",
                hashAlgorithm: "d");

            Assert.Equal("a", request.PackageSourceRepository);
            Assert.Equal("b", request.PackageId);
            Assert.Equal("c", request.PackageVersion);
            Assert.Equal("d", request.HashAlgorithm);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new GetPackageHashRequest(
                packageSourceRepository: "a",
                packageId: "b",
                packageVersion: "c",
                hashAlgorithm: "d");

            var json = TestUtilities.Serialize(request);

            Assert.Equal("{\"HashAlgorithm\":\"d\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"a\",\"PackageVersion\":\"c\"}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"HashAlgorithm\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}";
            var request = JsonSerializationUtilities.Deserialize<GetPackageHashRequest>(json);

            Assert.Equal("a", request.HashAlgorithm);
            Assert.Equal("b", request.PackageId);
            Assert.Equal("c", request.PackageSourceRepository);
            Assert.Equal("d", request.PackageVersion);
        }

        [Theory]
        [InlineData("{}", "packageSourceRepository")]
        [InlineData("{\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "hashAlgorithm")]
        [InlineData("{\"HashAlgorithm\":null,\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "hashAlgorithm")]
        [InlineData("{\"HashAlgorithm\":\"\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "hashAlgorithm")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "packageId")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageId\":null,\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "packageId")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageId\":\"\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "packageId")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageId\":\"b\",\"PackageVersion\":\"d\"}", "packageSourceRepository")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":null,\"PackageVersion\":\"d\"}", "packageSourceRepository")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"\",\"PackageVersion\":\"d\"}", "packageSourceRepository")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\"}", "packageVersion")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":null}", "packageVersion")]
        [InlineData("{\"HashAlgorithm\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"\"}", "packageVersion")]
        public void JsonDeserialization_ThrowsForInvalidStringArgument(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetPackageHashRequest>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }
    }
}
