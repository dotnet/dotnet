// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class NupkgMetadataFileTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Equals_SameContents_ReturnsTrue(int fileVersion)
        {
            Func<NupkgMetadataFile> getMetadataFile = () =>
            {
                var metadataFile = new NupkgMetadataFile()
                {
                    Version = fileVersion,
                    ContentHash = "tempContentHash"
                };
                if (fileVersion >= 2)
                {
                    metadataFile.Source = "https://source/v3/index.json";
                }

                return metadataFile;
            };

            var self = getMetadataFile();
            var other = getMetadataFile();

            Assert.NotSame(self, other);
            Assert.Equal(self, other);
        }

        [Fact]
        public void Equals_DifferentVersion_ReturnsFalse()
        {
            Func<NupkgMetadataFile> getMetadataFile = () =>
            {
                var metadataFile = new NupkgMetadataFile()
                {
                    Version = 1,
                    ContentHash = "tempContentHash"
                };

                return metadataFile;
            };

            var self = getMetadataFile();
            var other = getMetadataFile();
            other.Version = 2;

            Assert.NotSame(self, other);
            Assert.NotEqual(self, other);
        }

        [Fact]
        public void Equals_DifferentContentHash_ReturnsFalse()
        {
            Func<NupkgMetadataFile> getMetadataFile = () =>
            {
                var metadataFile = new NupkgMetadataFile()
                {
                    Version = 1,
                    ContentHash = "tempContentHash"
                };

                return metadataFile;
            };

            var self = getMetadataFile();
            var other = getMetadataFile();
            other.ContentHash = "contentHashChanged";

            Assert.NotSame(self, other);
            Assert.NotEqual(self, other);
        }

        [Fact]
        public void Equals_DifferentSource_ReturnsFalse()
        {
            Func<NupkgMetadataFile> getMetadataFile = () =>
            {
                var metadataFile = new NupkgMetadataFile()
                {
                    Version = 2,
                    ContentHash = "tempContentHash",
                    Source = "https://source/v3/index.json"
                };

                return metadataFile;
            };

            var self = getMetadataFile();
            var other = getMetadataFile();
            other.Source = "https://other/v3/index.json";

            Assert.NotSame(self, other);
            Assert.NotEqual(self, other);
        }
    }
}
