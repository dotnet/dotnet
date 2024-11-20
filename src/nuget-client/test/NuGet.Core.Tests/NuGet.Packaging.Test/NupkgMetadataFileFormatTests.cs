// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class NupkgMetadataFileFormatTests
    {
        [Fact]
        public void Read_V1FileContents_ReturnsCorrectObject()
        {
            // Arrange
            var nupkgMetadataFileContent = @"{
                ""version"": 1,
                ""contentHash"": ""NhfNp80eWq5ms7fMrjuRqpwhL1H56IVzXF9d+OIDcEfQ92m1DyE0c+ufUE1ogB09+sYLd58IO4eJ8jyn7AifbA==""
            }";

            // Act
            NupkgMetadataFile file = Read(nupkgMetadataFileContent, NullLogger.Instance, string.Empty);

            // Assert
            Assert.NotNull(file);
            Assert.Equal(1, file.Version);
            Assert.Equal("NhfNp80eWq5ms7fMrjuRqpwhL1H56IVzXF9d+OIDcEfQ92m1DyE0c+ufUE1ogB09+sYLd58IO4eJ8jyn7AifbA==", file.ContentHash);
            Assert.Null(file.Source);
        }

        [Fact]
        public void Read_V2FileContents_ReturnsCorrectObject()
        {
            // Arrange
            var nupkgMetadataFileContent = @"{
                ""version"": 2,
                ""contentHash"": ""NhfNp80eWq5ms7fMrjuRqpwhL1H56IVzXF9d+OIDcEfQ92m1DyE0c+ufUE1ogB09+sYLd58IO4eJ8jyn7AifbA=="",
                ""source"": ""https://source/v3/index.json""
            }";

            // Act
            NupkgMetadataFile file = Read(nupkgMetadataFileContent, NullLogger.Instance, string.Empty);

            // Assert
            Assert.NotNull(file);
            Assert.Equal(2, file.Version);
            Assert.Equal("NhfNp80eWq5ms7fMrjuRqpwhL1H56IVzXF9d+OIDcEfQ92m1DyE0c+ufUE1ogB09+sYLd58IO4eJ8jyn7AifbA==", file.ContentHash);
            Assert.Equal("https://source/v3/index.json", file.Source);
        }

        [Fact]
        public void Write_WithObject_RoundTrips()
        {
            var nupkgMetadataFileContent = @"{
  ""version"": 2,
  ""contentHash"": ""NhfNp80eWq5ms7fMrjuRqpwhL1H56IVzXF9d+OIDcEfQ92m1DyE0c+ufUE1ogB09+sYLd58IO4eJ8jyn7AifbA=="",
  ""source"": ""https://source/v3/index.json""
}";

            NupkgMetadataFile metadataFile = Read(nupkgMetadataFileContent, NullLogger.Instance, string.Empty);

            using (var stream = new MemoryStream())
            {
                NupkgMetadataFileFormat.Write(stream, metadataFile);
                stream.Position = 0;
                string content;
                using (var reader = new StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }

                Assert.Equal(nupkgMetadataFileContent, content);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("[]")]
        [InlineData(@"{""version"":")]
        public void Read_ContentsInvalid_ThrowsException(string contents)
        {
            // Arrange
            var logger = new TestLogger();

            // Act
            Assert.Throws<InvalidDataException>(() => Read(contents, logger, "from memory"));

            // Assert
            Assert.Equal(1, logger.Messages.Count);
        }

        [Fact]
        public void Read_ContentsWithUnexpectedValues_IgnoresUnexpectedValues()
        {
            // Arrange
            var logger = new TestLogger();
            var nupkgMetadataFileContent = @"{
                ""version"": 2,
                ""contentHash"": ""NhfNp80eWq5ms7fMrjuRqpwhL1H56IVzXF9d+OIDcEfQ92m1DyE0c+ufUE1ogB09+sYLd58IO4eJ8jyn7AifbA=="",
                ""source"": ""https://source/v3/index.json"",
                ""number"": 1,
                ""array"": [],
                ""object"": {}
            }";

            // Act
            Read(nupkgMetadataFileContent, logger, "from memory");

            // Assert
            Assert.Equal(0, logger.Messages.Count);
        }

        private NupkgMetadataFile Read(string contents, ILogger logger, string path)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(contents);
            using (var stream = new MemoryStream(utf8Bytes))
            {
                return NupkgMetadataFileFormat.Read(stream, logger, path);
            }
        }
    }
}
