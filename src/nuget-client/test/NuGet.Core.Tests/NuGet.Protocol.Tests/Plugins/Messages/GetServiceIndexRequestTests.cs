// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetServiceIndexRequestTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsForNullOrEmptyPackageSourceRepository(string packageSourceRepository)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new GetServiceIndexRequest(packageSourceRepository));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesPackageSourceRepositoryProperty()
        {
            var request = new GetServiceIndexRequest(packageSourceRepository: "a");

            Assert.Equal("a", request.PackageSourceRepository);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new GetServiceIndexRequest(packageSourceRepository: "a");

            var json = TestUtilities.Serialize(request);

            Assert.Equal("{\"PackageSourceRepository\":\"a\"}", json);
        }

        [Fact]
        public void JsonDeserialization_ReturnsCorrectObject()
        {
            var json = "{\"PackageSourceRepository\":\"a\"}";
            var request = JsonSerializationUtilities.Deserialize<GetServiceIndexRequest>(json);

            Assert.Equal("a", request.PackageSourceRepository);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"PackageSourceRepository\":null}")]
        [InlineData("{\"PackageSourceRepository\":\"\"}")]
        public void JsonDeserialization_ThrowsForInvalidPackageSourceRepository(string json)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<GetServiceIndexRequest>(json));

            Assert.Equal("packageSourceRepository", exception.ParamName);
        }
    }
}
