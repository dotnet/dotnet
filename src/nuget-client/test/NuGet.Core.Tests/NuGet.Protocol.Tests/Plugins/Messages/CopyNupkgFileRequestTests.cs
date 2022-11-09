// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class CopyNupkgFileRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyNupkgFileRequest(
                    packageSourceRepository,
                    packageId: "a",
                    packageVersion: "b",
                    destinationFilePath: "c"));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageId(string packageId)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyNupkgFileRequest(
                    packageSourceRepository: "a",
                    packageId: packageId,
                    packageVersion: "b",
                    destinationFilePath: "c"));

            Assert.Equal("packageId", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageVersion(string packageVersion)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyNupkgFileRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: packageVersion,
                    destinationFilePath: "c"));

            Assert.Equal("packageVersion", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyDestinationFilePath(string destinationFilePath)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyNupkgFileRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: "c",
                    destinationFilePath: destinationFilePath));

            Assert.Equal("destinationFilePath", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new CopyNupkgFileRequest(
                packageSourceRepository: "a",
                packageId: "b",
                packageVersion: "c",
                destinationFilePath: "d");

            Assert.Equal("a", request.PackageSourceRepository);
            Assert.Equal("b", request.PackageId);
            Assert.Equal("c", request.PackageVersion);
            Assert.Equal("d", request.DestinationFilePath);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new CopyNupkgFileRequest(
                packageSourceRepository: "a",
                packageId: "b",
                packageVersion: "c",
                destinationFilePath: "d");

            var actualJson = TestUtilities.Serialize(request);

            Assert.Equal("{\"DestinationFilePath\":\"d\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"a\",\"PackageVersion\":\"c\"}", actualJson);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"DestinationFilePath\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}";
            var request = JsonSerializationUtilities.Deserialize<CopyNupkgFileRequest>(json);

            Assert.Equal("c", request.PackageSourceRepository);
            Assert.Equal("b", request.PackageId);
            Assert.Equal("d", request.PackageVersion);
            Assert.Equal("a", request.DestinationFilePath);
        }

        [Theory]
        [InlineData("{}", "packageSourceRepository")]
        [InlineData("{\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "destinationFilePath")]
        [InlineData("{\"DestinationFilePath\":null,\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "destinationFilePath")]
        [InlineData("{\"DestinationFilePath\":\"\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "destinationFilePath")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "packageId")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageId\":null,\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "packageId")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageId\":\"\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"d\"}", "packageId")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageId\":\"b\",\"PackageVersion\":\"d\"}", "packageSourceRepository")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":null,\"PackageVersion\":\"d\"}", "packageSourceRepository")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"\",\"PackageVersion\":\"d\"}", "packageSourceRepository")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\"}", "packageVersion")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":null}", "packageVersion")]
        [InlineData("{\"DestinationFilePath\":\"a\",\"PackageId\":\"b\",\"PackageSourceRepository\":\"c\",\"PackageVersion\":\"\"}", "packageVersion")]
        public void JsonDeserialization_ThrowsForInvalidStringArgument(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<CopyNupkgFileRequest>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }
    }
}
