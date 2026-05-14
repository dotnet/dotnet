// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class JsonExtensionsTest
    {
        [Fact]
        public void FromJTokenWithBadUrl()
        {
            // Arrange
            var token = JToken.Parse(JsonData.BadProjectUrlJsonData);

            // Act
            var metaData = token.FromJToken<PackageSearchMetadata>();

            // Assert
            Assert.Null(metaData.ProjectUrl);
        }

        [Fact]
        public async Task FromJTokenWithDeprecationMetadata()
        {
            // Arrange
            var token = JToken.Parse(JsonData.PackageRegistrationCatalogEntryWithDeprecationMetadata);

            // Act
            var metaData = token.FromJToken<PackageSearchMetadata>();

            // Assert
            Assert.Equal(metaData.DeprecationMetadata, await metaData.GetDeprecationMetadataAsync());
            Assert.Equal(new[] { "CriticalBugs", "Legacy" }, metaData.DeprecationMetadata.Reasons);
            Assert.Equal("this is a message", metaData.DeprecationMetadata.Message);
            Assert.Null(metaData.DeprecationMetadata.AlternatePackage);
        }
    }
}
