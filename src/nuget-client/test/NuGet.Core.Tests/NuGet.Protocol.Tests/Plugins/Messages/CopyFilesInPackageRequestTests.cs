// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class CopyFilesInPackageRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageRequest(
                    packageSourceRepository,
                    packageId: "a",
                    packageVersion: "b",
                    filesInPackage: new[] { "c" },
                    destinationFolderPath: "d"));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageId(string packageId)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageRequest(
                    packageSourceRepository: "a",
                    packageId: packageId,
                    packageVersion: "b",
                    filesInPackage: new[] { "c" },
                    destinationFolderPath: "d"));

            Assert.Equal("packageId", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageVersion(string packageVersion)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: packageVersion,
                    filesInPackage: new[] { "c" },
                    destinationFolderPath: "d"));

            Assert.Equal("packageVersion", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullFilesInPackage()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: "c",
                    filesInPackage: null,
                    destinationFolderPath: "d"));

            Assert.Equal("filesInPackage", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForEmptyFilesInPackage()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: "c",
                    filesInPackage: Enumerable.Empty<string>(),
                    destinationFolderPath: "d"));

            Assert.Equal("filesInPackage", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyDestinationFolderPath(string destinationFolderPath)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new CopyFilesInPackageRequest(
                    packageSourceRepository: "a",
                    packageId: "b",
                    packageVersion: "c",
                    filesInPackage: new[] { "d" },
                    destinationFolderPath: destinationFolderPath));

            Assert.Equal("destinationFolderPath", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new CopyFilesInPackageRequest(
                packageSourceRepository: "a",
                packageId: "b",
                packageVersion: "c",
                filesInPackage: new[] { "d" },
                destinationFolderPath: "e");

            Assert.Equal("a", request.PackageSourceRepository);
            Assert.Equal("b", request.PackageId);
            Assert.Equal("c", request.PackageVersion);
            Assert.Equal(new[] { "d" }, request.FilesInPackage);
            Assert.Equal("e", request.DestinationFolderPath);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new CopyFilesInPackageRequest(
                packageSourceRepository: "a",
                packageId: "b",
                packageVersion: "c",
                filesInPackage: new[] { "d" },
                destinationFolderPath: "e");

            var actualJson = TestUtilities.Serialize(request);

            Assert.Equal("{\"DestinationFolderPath\":\"e\",\"FilesInPackage\":[\"d\"],\"PackageId\":\"b\",\"PackageSourceRepository\":\"a\",\"PackageVersion\":\"c\"}", actualJson);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}";
            var request = JsonSerializationUtilities.Deserialize<CopyFilesInPackageRequest>(json);

            Assert.Equal("d", request.PackageSourceRepository);
            Assert.Equal("c", request.PackageId);
            Assert.Equal("e", request.PackageVersion);
            Assert.Equal(new[] { "b" }, request.FilesInPackage);
            Assert.Equal("a", request.DestinationFolderPath);
        }

        [Theory]
        [InlineData("{}", "packageSourceRepository")]
        [InlineData("{\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "destinationFolderPath")]
        [InlineData("{\"DestinationFolderPath\":null,\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "destinationFolderPath")]
        [InlineData("{\"DestinationFolderPath\":\"\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "destinationFolderPath")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "filesInPackage")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":null,\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "filesInPackage")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[],\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "filesInPackage")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "packageId")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":null,\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "packageId")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"e\"}", "packageId")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageVersion\":\"e\"}", "packageSourceRepository")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":null,\"PackageVersion\":\"e\"}", "packageSourceRepository")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":\"\",\"PackageVersion\":\"e\"}", "packageSourceRepository")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\"}", "packageVersion")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":null}", "packageVersion")]
        [InlineData("{\"DestinationFolderPath\":\"a\",\"FilesInPackage\":[\"b\"],\"PackageId\":\"c\",\"PackageSourceRepository\":\"d\",\"PackageVersion\":\"\"}", "packageVersion")]
        public void JsonDeserialization_ThrowsForInvalidStringArgument(string json, string parameterName)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<CopyFilesInPackageRequest>(json));

            Assert.Equal(parameterName, exception.ParamName);
        }
    }
}
