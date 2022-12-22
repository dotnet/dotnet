// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            NupkgMetadataFile file;
            using (var reader = new StringReader(nupkgMetadataFileContent))
            {
                file = NupkgMetadataFileFormat.Read(reader, NullLogger.Instance, string.Empty);
            }

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
            NupkgMetadataFile file;
            using (var reader = new StringReader(nupkgMetadataFileContent))
            {
                file = NupkgMetadataFileFormat.Read(reader, NullLogger.Instance, string.Empty);
            }

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

            NupkgMetadataFile metadataFile = null;

            using (var reader = new StringReader(nupkgMetadataFileContent))
            {
                metadataFile = NupkgMetadataFileFormat.Read(reader, NullLogger.Instance, string.Empty);
            }

            using (var writer = new StringWriter())
            {
                NupkgMetadataFileFormat.Write(writer, metadataFile);
                var output = JObject.Parse(writer.ToString());
                var expected = JObject.Parse(nupkgMetadataFileContent);

                Assert.Equal(expected.ToString(), output.ToString());
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("[]")]
        public void Read_ContentsNotAnObject_ThrowsException(string contents)
        {
            // Arrange
            var logger = new TestLogger();

            // Act
            using (var stringReader = new StringReader(contents))
            {
                Assert.Throws<InvalidDataException>(() => NupkgMetadataFileFormat.Read(stringReader, logger, "from memory"));
            }

            // Assert
            Assert.Equal(1, logger.Messages.Count);
        }

        [Fact]
        public void Read_ContentsInvalidJson_ThrowsJsonReaderException()
        {
            // Arrange
            var logger = new TestLogger();

            // Act
            using (var stringReader = new StringReader(@"{""version"":"))
            {
                Assert.Throws<JsonReaderException>(() => NupkgMetadataFileFormat.Read(stringReader, logger, "from memory"));
            }

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
            using (var stringReader = new StringReader(nupkgMetadataFileContent))
            {
                NupkgMetadataFileFormat.Read(stringReader, logger, "from memory");
            }

            // Assert
            Assert.Equal(0, logger.Messages.Count);
        }
    }
}
