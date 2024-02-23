// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Moq;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    [UseCulture("")] // Fix tests failing on systems with non-English locales
    public class Utf8JsonStreamReaderTests
    {
        private static readonly string JsonWithOverflowObject = "{\"object1\":{\"a\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"b\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"c\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"d\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"e\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"f\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"g\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"h\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"i\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"j\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"k\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"l\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"m\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"n\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"o\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"p\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"q\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"r\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"s\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"t\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"u\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\"},\"object2\":{\"a\":\"abcdefghijklmnopqrstuvwxyz\",\"b\":\"abcdefghijklmnopqrstuvwxyz\",\"c\":\"abcdefghijklmnopqrstuvwxyz\",\"d\":\"abcdefghijklmnopqrstuvwxyz\",\"e\":\"abcdefghijklmnopqrstuvwxyz\",\"f\":\"abcdefghijklmnopqrstuvwxyz\",\"g\":\"abcdefghijklmnopqrstuvwxyz\",\"h\":\"abcdefghijklmnopqrstuvwxyz\",\"i\":\"abcdefghijklmnopqrstuvwxyz\",\"j\":\"abcdefghijklmnopqrstuvwxyz\",\"k\":\"abcdefghijklmnopqrstuvwxyz\",\"l\":\"abcdefghijklmnopqrstuvwxyz\",\"m\":\"abcdefghijklmnopqrstuvwxyz\",\"n\":\"abcdefghijklmnopqrstuvwxyz\",\"o\":\"abcdefghijklmnopqrstuvwxyz\",\"p\":\"abcdefghijklmnopqrstuvwxyz\",\"q\":\"abcdefghijklmnopqrstuvwxyz\",\"r\":\"abcdefghijklmnopqrstuvwxyz\",\"s\":\"abcdefghijklmnopqrstuvwxyz\",\"t\":\"abcdefghijklmnopqrstuvwxyz\",\"u\":\"abcdefghijklmnopqrstuvwxyz\"}}";
        private static readonly string JsonWithoutOverflow = "{\"object1\":{\"a\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"b\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"c\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"d\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"e\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"f\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"g\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"h\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"i\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"j\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"k\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"l\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"m\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"n\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"o\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"p\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"q\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"r\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"s\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"t\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"u\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\"}}";
        private static readonly string JsonWithOverflow = "{\"object1\":{\"a\":\"abcdefghijklmnopqrstuvwxyz\",\"b\":\"abcdefghijklmnopqrstuvwxyz\",\"c\":\"abcdefghijklmnopqrstuvwxyz\",\"d\":\"abcdefghijklmnopqrstuvwxyz\",\"e\":\"abcdefghijklmnopqrstuvwxyz\",\"f\":\"abcdefghijklmnopqrstuvwxyz\",\"g\":\"abcdefghijklmnopqrstuvwxyz\",\"h\":\"abcdefghijklmnopqrstuvwxyz\",\"i\":\"abcdefghijklmnopqrstuvwxyz\",\"j\":\"abcdefghijklmnopqrstuvwxyz\",\"k\":\"abcdefghijklmnopqrstuvwxyz\",\"l\":\"abcdefghijklmnopqrstuvwxyz\",\"m\":\"abcdefghijklmnopqrstuvwxyz\",\"n\":\"abcdefghijklmnopqrstuvwxyz\",\"o\":\"abcdefghijklmnopqrstuvwxyz\",\"p\":\"abcdefghijklmnopqrstuvwxyz\",\"q\":\"abcdefghijklmnopqrstuvwxyz\",\"r\":\"abcdefghijklmnopqrstuvwxyz\",\"s\":\"abcdefghijklmnopqrstuvwxyz\",\"t\":\"abcdefghijklmnopqrstuvwxyz\",\"u\":\"abcdefghijklmnopqrstuvwxyz\"}, \"object2\": {\"a\":\"abcdefghijklmnopqrstuvwxyz\",\"b\":\"abcdefghijklmnopqrstuvwxyz\",\"c\":\"abcdefghijklmnopqrstuvwxyz\",\"d\":\"abcdefghijklmnopqrstuvwxyz\",\"e\":\"abcdefghijklmnopqrstuvwxyz\",\"f\":\"abcdefghijklmnopqrstuvwxyz\",\"g\":\"abcdefghijklmnopqrstuvwxyz\",\"h\":\"abcdefghijklmnopqrstuvwxyz\",\"i\":\"abcdefghijklmnopqrstuvwxyz\",\"j\":\"abcdefghijklmnopqrstuvwxyz\",\"k\":\"abcdefghijklmnopqrstuvwxyz\",\"l\":\"abcdefghijklmnopqrstuvwxyz\",\"m\":\"abcdefghijklmnopqrstuvwxyz\",\"n\":\"abcdefghijklmnopqrstuvwxyz\",\"o\":\"abcdefghijklmnopqrstuvwxyz\",\"p\":\"abcdefghijklmnopqrstuvwxyz\",\"q\":\"abcdefghijklmnopqrstuvwxyz\",\"r\":\"abcdefghijklmnopqrstuvwxyz\",\"s\":\"abcdefghijklmnopqrstuvwxyz\",\"t\":\"abcdefghijklmnopqrstuvwxyz\",\"u\":\"abcdefghijklmnopqrstuvwxyz\"}, \"object3\":{\"a\":\"abcdefghijklmnopqrstuvwxyz\",\"b\":\"abcdefghijklmnopqrstuvwxyz\",\"c\":\"abcdefghijklmnopqrstuvwxyz\",\"d\":\"abcdefghijklmnopqrstuvwxyz\",\"e\":\"abcdefghijklmnopqrstuvwxyz\",\"f\":\"abcdefghijklmnopqrstuvwxyz\",\"g\":\"abcdefghijklmnopqrstuvwxyz\",\"h\":\"abcdefghijklmnopqrstuvwxyz\",\"i\":\"abcdefghijklmnopqrstuvwxyz\",\"j\":\"abcdefghijklmnopqrstuvwxyz\",\"k\":\"abcdefghijklmnopqrstuvwxyz\",\"l\":\"abcdefghijklmnopqrstuvwxyz\",\"m\":\"abcdefghijklmnopqrstuvwxyz\",\"n\":\"abcdefghijklmnopqrstuvwxyz\",\"o\":\"abcdefghijklmnopqrstuvwxyz\",\"p\":\"abcdefghijklmnopqrstuvwxyz\",\"q\":\"abcdefghijklmnopqrstuvwxyz\",\"r\":\"abcdefghijklmnopqrstuvwxyz\",\"s\":\"abcdefghijklmnopqrstuvwxyz\",\"t\":\"abcdefghijklmnopqrstuvwxyz\",\"u\":\"abcdefghijklmnopqrstuvwxyz\"}}";
        private static readonly string SmallJson = "{\"object1\":{\"a\":\"abcdefghijklmnopqrstuvwxyz\"}}";

        [Fact]
        public void Utf8JsonStreamReaderCtr_WhenStreamIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                using var reader = new Utf8JsonStreamReader(null);
            });
        }

        [Fact]
        public void Utf8JsonStreamReaderCtr_WhenBufferToSmall_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var json = "{}";

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                using (var reader = new Utf8JsonStreamReader(stream, 10))
                {
                }
            });
        }


        [Fact]
        public void Utf8JsonStreamReaderCtr_WhenEmptyJson_Throws()
        {
            Assert.ThrowsAny<JsonException>(() =>
            {
                var json = "";

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                }
            });
        }

        [Fact]
        public void Utf8JsonStreamReaderCtr_WhenEmptySream_Throws()
        {
            Assert.ThrowsAny<JsonException>(() =>
            {
                using (var stream = new MemoryStream(Array.Empty<byte>()))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                }
            });
        }

        [Fact]
        public void Utf8JsonStreamReaderCtr_WhenStreamStartsWithUtf8Bom_SkipThem()
        {
            var json = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble()) + "{}";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                Assert.Equal(5, stream.Position);
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
            }
        }

        [Fact]
        public void Utf8JsonStreamReaderCtr_WhenStreamStartsWithoutUtf8Bom_ReadFromStart()
        {
            var json = "{}";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                Assert.Equal(2, stream.Position);
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
            }
        }

        [Fact]
        public void Utf8JsonStreamReaderCtr_WhenReadingWithOverflow_FinalBlockFalse()
        {
            var json = Encoding.UTF8.GetBytes(JsonWithOverflowObject);
            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream, 1024))
            {
                Assert.False(reader.IsFinalBlock);
            }
        }

        [Fact]
        public void Read_WhenReadingMalformedJsonString_Throws()
        {
            var json = Encoding.UTF8.GetBytes("{\"a\":\"string}");

            Assert.ThrowsAny<JsonException>(() =>
            {
                using (var stream = new MemoryStream(json))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                    Assert.True(reader.IsFinalBlock);
                    Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                    reader.Read();
                    Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                    reader.Read();
                }
            });
        }

        [Fact]
        public void Read_WhenReadingMalformedJson_Throws()
        {
            var json = Encoding.UTF8.GetBytes("{\"a\":\"string\"}ohno");
            Assert.ThrowsAny<JsonException>(() =>
            {
                using (var stream = new MemoryStream(json))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                    Assert.True(reader.IsFinalBlock);
                    Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                    reader.Read();
                    Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                    reader.Read();
                    reader.Read();
                    reader.Read();
                    reader.Read();
                }
            });
        }

        [Fact]
        public void Read_WhenReadingSmallJson_Read()
        {
            var json = Encoding.UTF8.GetBytes(SmallJson);

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                Assert.True(reader.IsFinalBlock);
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.String, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.EndObject, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.EndObject, reader.TokenType);
            }
        }

        [Fact]
        public void Read_WhenReadingSmallJsonPastEnd_Read()
        {
            var json = Encoding.UTF8.GetBytes(SmallJson);
            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                Assert.True(reader.IsFinalBlock);
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.String, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.EndObject, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.EndObject, reader.TokenType);
                Assert.False(reader.Read());
            }
        }

        [Fact]
        public void Read_WhenReadingWithoutOverflow_Read()
        {
            var json = Encoding.UTF8.GetBytes(JsonWithoutOverflow);

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                Assert.True(reader.IsFinalBlock);
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
            }
        }

        [Fact]
        public void Read_WhenReadingWithOverflow_ReadNextBuffer()
        {
            var json = Encoding.UTF8.GetBytes(JsonWithOverflowObject);
            var mock = SetupMockArrayBuffer();

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream, 1024, mock.Object))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "r")
                    {
                        break;
                    }
                }
                reader.Read();
                mock.Verify(m => m.Rent(1024), Times.Exactly(1));
                Assert.Equal(JsonTokenType.String, reader.TokenType);
                Assert.Equal("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", reader.GetString());
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                Assert.Equal("s", reader.GetString());
            }
        }

        [Fact]
        public void Read_WhenReadingWithLargeToken_ResizeBuffer()
        {
            var json = Encoding.UTF8.GetBytes("{\"largeToken\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"smallToken\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\"}");
            var mock = SetupMockArrayBuffer();

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream, 1024, mock.Object))
            {
                reader.Read();
                reader.Read();

                mock.Verify(m => m.Rent(1024), Times.Exactly(1));
                mock.Verify(m => m.Rent(2048), Times.Exactly(1));
                mock.Verify(m => m.Return(It.IsAny<byte[]>(), true), Times.Exactly(1));
                Assert.Equal(JsonTokenType.String, reader.TokenType);
                Assert.Equal("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
                    reader.GetString());
            }
        }

        [Fact]
        public void Read_WithLargeToken_WhenReadPastFinal_ReadReturnsFalse()
        {
            var json = Encoding.UTF8.GetBytes("{\"largeToken\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\",\"smallToken\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\"}");
            var mock = SetupMockArrayBuffer();

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream, 1024, mock.Object))
            {
                reader.Read();
                reader.Read();
                reader.Read();
                reader.Read();
                reader.Read();
                mock.Verify(m => m.Rent(1024), Times.Exactly(1));
                mock.Verify(m => m.Rent(2048), Times.Exactly(1));
                mock.Verify(m => m.Return(It.IsAny<byte[]>(), true), Times.Exactly(1));
                Assert.False(reader.Read());
            }
        }

        [Fact]
        public void Read_WhenReadingWithOverflowToBufferSize_LoadNextBuffer()
        {
            var json = Encoding.UTF8.GetBytes("{\"largeToken\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrst\",\"smallToken\":\"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz\"}");

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream, 1024))
            {
                reader.Read();
                reader.Read();
                reader.Read();

                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                Assert.Equal("smallToken", reader.GetString());
                Assert.True(reader.Read());
                Assert.Equal(JsonTokenType.String, reader.TokenType);
                Assert.Equal("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", reader.GetString());
            }
        }

        [Fact]
        public void Dispose_NoErrors()
        {
            var json = Encoding.UTF8.GetBytes(SmallJson);
            var mock = SetupMockArrayBuffer();

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream, 1024, arrayPool: mock.Object))
            {
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
            }
            mock.Verify(m => m.Return(It.IsAny<byte[]>(), true), Times.Exactly(1));
        }

        [Fact]
        public void Dispose_Read_ObjectDisposedException()
        {
            var json = Encoding.UTF8.GetBytes(SmallJson);
            Assert.Throws<ObjectDisposedException>(() =>
            {
                using (var stream = new MemoryStream(json))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                    Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                    reader.Dispose();
                    reader.Read();
                }
            });
        }

        [Fact]
        public void Dispose_Skip_ObjectDisposedException()
        {
            var json = Encoding.UTF8.GetBytes(SmallJson);
            Assert.Throws<ObjectDisposedException>(() =>
            {
                using (var stream = new MemoryStream(json))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                    Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                    reader.Dispose();
                    reader.Skip();
                }
            });
        }

        [Theory]
        [InlineData("{\"object1\": { \"a\":\"asdad\" }")]
        [InlineData("{\"object1\": { \"a\":\"asdad }}")]
        [InlineData("{\"object1\":  \"a\":\"asdad\" }}")]
        public void Skip_WhenReadingWithMalformedJson(string malformedJson)
        {
            var json = Encoding.UTF8.GetBytes(malformedJson);

            Assert.ThrowsAny<JsonException>(() =>
            {
                using (var stream = new MemoryStream(json))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                    Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                    reader.Skip();
                    Assert.Equal(JsonTokenType.EndObject, reader.TokenType);
                }
            });
        }

        [Fact]
        public void Skip_WhenReadingWithoutOverflow_SkipObject()
        {
            var json = Encoding.UTF8.GetBytes(JsonWithoutOverflow);

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                Assert.Equal(JsonTokenType.StartObject, reader.TokenType);
                reader.Skip();
                Assert.Equal(JsonTokenType.EndObject, reader.TokenType);
            }
        }

        [Fact]
        public void Skip_WhenReadingWithOverflow_Skip()
        {
            var json = Encoding.UTF8.GetBytes(JsonWithOverflow);
            var mock = SetupMockArrayBuffer();

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream, 1024, mock.Object))
            {
                reader.Read();
                reader.Skip();
                reader.Read();
                reader.Skip();
                Assert.Equal(JsonTokenType.EndObject, reader.TokenType);
                reader.Read();
                Assert.Equal("object3", reader.GetString());
                mock.Verify(m => m.Rent(1024), Times.Exactly(1));
            }
        }

        [Fact]
        public void Skip_WhenReadingWithOverflowObject_ResizeBuffer()
        {
            var json = Encoding.UTF8.GetBytes(JsonWithOverflowObject);
            var mock = SetupMockArrayBuffer();

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream, 1024, mock.Object))
            {
                reader.Read();
                reader.Skip();
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                Assert.Equal("object2", reader.GetString());
                mock.Verify(m => m.Rent(1024), Times.Exactly(1));
                mock.Verify(m => m.Rent(2048), Times.Exactly(1));
                mock.Verify(m => m.Return(It.IsAny<byte[]>(), true), Times.Exactly(1));
            }
        }

        [Fact]
        public void ReadNextTokenAsString_WhenCalled_AdvanceToken()
        {
            var json = Encoding.UTF8.GetBytes("{\"token\":\"value\"}");

            using (var stream = new MemoryStream(json))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                var result = reader.ReadNextTokenAsString();
                Assert.Equal(JsonTokenType.String, reader.TokenType);
                Assert.Equal("value", result);
            }
        }

        [Fact]
        public void ReadNextTokenAsString_WithMalformedJson_GetException()
        {
            var json = Encoding.UTF8.GetBytes("{\"token\":\"value}");
            Assert.ThrowsAny<JsonException>(() =>
            {
                using (var stream = new MemoryStream(json))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                    reader.Read();
                    Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                    reader.ReadNextTokenAsString();
                }
            });
        }

        [Theory]
        [InlineData("true", JsonTokenType.True)]
        [InlineData("false", JsonTokenType.False)]
        [InlineData("-2", JsonTokenType.Number)]
        [InlineData("3.14", JsonTokenType.Number)]
        [InlineData("{}", JsonTokenType.StartObject)]
        [InlineData("[]", JsonTokenType.StartArray)]
        [InlineData("[true]", JsonTokenType.StartArray)]
        [InlineData("[-2]", JsonTokenType.StartArray)]
        [InlineData("[3.14]", JsonTokenType.StartArray)]
        [InlineData("[\"a\", \"b\"]", JsonTokenType.StartArray)]
        public void ReadDelimitedString_WhenValueIsNotString_Throws(string value, JsonTokenType expectedTokenType)
        {
            var json = $"{{\"a\":{value}}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            var tokenType = JsonTokenType.None;
            var exceptionThrown = Assert.Throws<InvalidCastException>(() =>
            {
                using var stream = new MemoryStream(encodedBytes);
                using var reader = new Utf8JsonStreamReader(stream);
                reader.Read();
                try
                {
                    reader.ReadDelimitedString();
                }
                finally
                {
                    tokenType = reader.TokenType;
                }
            });
            Assert.Null(exceptionThrown.InnerException);
            Assert.Equal(expectedTokenType, tokenType);
        }

        [Fact]
        public void ReadDelimitedString_WhenValueIsString_ReturnsValue()
        {
            const string expectedResult = "b";
            var json = $"{{\"a\":\"{expectedResult}\"}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                IEnumerable<string> actualResults = reader.ReadDelimitedString();
                Assert.Collection(actualResults, actualResult => Assert.Equal(expectedResult, actualResult));
                Assert.Equal(JsonTokenType.String, reader.TokenType);
            }
        }

        [Theory]
        [InlineData("b,c,d")]
        [InlineData("b c d")]
        public void ReadDelimitedString_WhenValueIsDelimitedString_ReturnsValues(string value)
        {
            string[] expectedResults = value.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var json = $"{{\"a\":\"{value}\"}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                IEnumerable<string> actualResults = reader.ReadDelimitedString();
                Assert.Equal(expectedResults, actualResults);
                Assert.Equal(JsonTokenType.String, reader.TokenType);
            }
        }

        [Theory]
        [InlineData("null")]
        [InlineData("\"b\"")]
        [InlineData("{}")]
        public void ReadStringArrayAsIList_WhenValueIsNotArray_ReturnsNull(string value)
        {
            var json = $"{{\"a\":{value}}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                reader.Read();
                Assert.NotEqual(JsonTokenType.PropertyName, reader.TokenType);
                IList<string> actualValues = reader.ReadStringArrayAsIList();
                Assert.Null(actualValues);
            }
        }

        [Fact]
        public void ReadStringArrayAsIList_WhenValueIsEmptyArray_ReturnsNull()
        {
            var encodedBytes = Encoding.UTF8.GetBytes("{\"a\":[]}");
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                reader.Read();
                Assert.NotEqual(JsonTokenType.PropertyName, reader.TokenType);
                IList<string> actualValues = reader.ReadStringArrayAsIList();
                Assert.Null(actualValues);
            }
        }

        [Fact]
        public void ReadStringArrayAsIList_WithSupportedTypes_ReturnsStringArray()
        {
            var encodedBytes = Encoding.UTF8.GetBytes("[\"a\",-2,3.14,true,null]");
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                IList<string> actualValues = reader.ReadStringArrayAsIList();

                Assert.Collection(
                    actualValues,
                    actualValue => Assert.Equal("a", actualValue),
                    actualValue => Assert.Equal("-2", actualValue),
                    actualValue => Assert.Equal("3.14", actualValue),
                    actualValue => Assert.Equal("True", actualValue),
                    actualValue => Assert.Null(actualValue));
                Assert.Equal(JsonTokenType.EndArray, reader.TokenType);
            }
        }

        [Theory]
        [InlineData("[]")]
        [InlineData("{}")]
        public void ReadStringArrayAsIList_WithUnsupportedTypes_Throws(string element)
        {
            var encodedBytes = Encoding.UTF8.GetBytes($"[{element}]");
            Assert.Throws<InvalidCastException>(() =>
            {
                using (var stream = new MemoryStream(encodedBytes))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                    reader.ReadStringArrayAsIList();
                }
            });
        }


        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void ReadNextTokenAsBoolOrFalse_WithValidValues_ReturnsBoolean(string value, bool expectedResult)
        {
            var json = $"{{\"a\":{value}}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                bool actualResult = reader.ReadNextTokenAsBoolOrFalse();
                Assert.Equal(expectedResult, actualResult);
            }
        }

        [Theory]
        [InlineData("\"words\"")]
        [InlineData("-3")]
        [InlineData("3.3")]
        [InlineData("[]")]
        [InlineData("{}")]
        public void ReadNextTokenAsBoolOrFalse_WithInvalidValues_ReturnsFalse(string value)
        {
            var json = $"{{\"a\":{value}}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                bool actualResult = reader.ReadNextTokenAsBoolOrFalse();
                Assert.False(actualResult);
            }
        }

        [Fact]
        public void ReadNextStringOrArrayOfStringsAsReadOnlyList_WhenValueIsNull_ReturnsNull()
        {
            const string json = "{\"a\":null}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                IEnumerable<string> actualResults = reader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                Assert.Null(actualResults);
                Assert.Equal(JsonTokenType.Null, reader.TokenType);
            }
        }

        [Theory]
        [InlineData("true", JsonTokenType.True)]
        [InlineData("false", JsonTokenType.False)]
        [InlineData("-2", JsonTokenType.Number)]
        [InlineData("3.14", JsonTokenType.Number)]
        [InlineData("{}", JsonTokenType.StartObject)]
        public void ReadNextStringOrArrayOfStringsAsReadOnlyList_WhenValueIsNotString_ReturnsNull(
            string value,
            JsonTokenType expectedTokenType)
        {
            var json = $"{{\"a\":{value}}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);

                IEnumerable<string> actualResults = reader.ReadNextStringOrArrayOfStringsAsReadOnlyList();

                Assert.Null(actualResults);
                Assert.Equal(expectedTokenType, reader.TokenType);
            }
        }

        [Fact]
        public void ReadNextStringOrArrayOfStringsAsReadOnlyList_WhenValueIsString_ReturnsValue()
        {
            const string expectedResult = "b";
            var json = $"{{\"a\":\"{expectedResult}\"}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);

                IEnumerable<string> actualResults = reader.ReadNextStringOrArrayOfStringsAsReadOnlyList();

                Assert.Collection(actualResults, actualResult => Assert.Equal(expectedResult, actualResult));
                Assert.Equal(JsonTokenType.String, reader.TokenType);
            }
        }

        [Theory]
        [InlineData("b,c,d")]
        [InlineData("b c d")]
        public void ReadNextStringOrArrayOfStringsAsReadOnlyList_WhenValueIsDelimitedString_ReturnsValue(string expectedResult)
        {
            var json = $"{{\"a\":\"{expectedResult}\"}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);

                IEnumerable<string> actualResults = reader.ReadNextStringOrArrayOfStringsAsReadOnlyList();

                Assert.Collection(actualResults, actualResult => Assert.Equal(expectedResult, actualResult));
                Assert.Equal(JsonTokenType.String, reader.TokenType);
            }
        }

        [Fact]
        public void ReadNextStringOrArrayOfStringsAsReadOnlyList_WhenValueIsEmptyArray_ReturnsEmptyList()
        {
            const string json = "{\"a\":[]}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);

                IReadOnlyList<string> actualResults = reader.ReadNextStringOrArrayOfStringsAsReadOnlyList();

                Assert.Empty(actualResults);
                Assert.Equal(JsonTokenType.EndArray, reader.TokenType);
            }
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("true", "True")]
        [InlineData("-2", "-2")]
        [InlineData("3.14", "3.14")]
        [InlineData("\"b\"", "b")]
        public void ReadNextStringOrArrayOfStringsAsReadOnlyList_WhenValueIsConvertibleToString_ReturnsValueAsString(
            string value,
            string expectedResult)
        {
            var json = $"{{\"a\":[{value}]}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();

                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);

                IEnumerable<string> actualResults = reader.ReadNextStringOrArrayOfStringsAsReadOnlyList();

                Assert.Collection(actualResults, actualResult => Assert.Equal(expectedResult, actualResult));
                Assert.Equal(JsonTokenType.EndArray, reader.TokenType);
            }
        }

        [Theory]
        [InlineData("[]", JsonTokenType.StartArray)]
        [InlineData("{}", JsonTokenType.StartObject)]
        public void ReadNextStringOrArrayOfStringsAsReadOnlyList_WhenValueIsNotConvertibleToString_ReturnsValueAsString(
            string value,
            JsonTokenType expectedToken)
        {
            var json = $"{{\"a\":[{value}]}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            var tokenType = JsonTokenType.None;
            var exceptionThrown = Assert.Throws<InvalidCastException>(() =>
            {
                using var stream = new MemoryStream(encodedBytes);
                using var reader = new Utf8JsonStreamReader(stream);
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);
                try
                {
                    reader.ReadNextStringOrArrayOfStringsAsReadOnlyList();
                }
                finally
                {
                    tokenType = reader.TokenType;
                }
            });
            Assert.Equal(expectedToken, tokenType);
        }

        [Fact]
        public void ReadNextStringOrArrayOfStringsAsReadOnlyList_WhenValueIsArrayOfStrings_ReturnsValues()
        {
            string[] expectedResults = { "b", "c" };
            var json = $"{{\"a\":[{string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""))}]}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                reader.Read();
                Assert.Equal(JsonTokenType.PropertyName, reader.TokenType);

                IEnumerable<string> actualResults = reader.ReadNextStringOrArrayOfStringsAsReadOnlyList();

                Assert.Equal(expectedResults, actualResults);
                Assert.Equal(JsonTokenType.EndArray, reader.TokenType);
            }
        }

        [Fact]
        public void ReadObjectAsList_WithValidJson_ReturnsValue()
        {
            var json = "{ \"a\": { \"property1\": \"1\" },\"b\": { \"property1\": \"1\" }, \"c\": { \"property1\": \"1\" }}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                var results = reader.ReadObjectAsList(Utf8JsonReaderExtensions.LockFileItemConverter);
                Assert.Equal(3, results.Count);
            }
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("null")]
        public void ReadObjectAsList_WithEmptyValues_ReturnsEmptyArray(string json)
        {
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                var results = reader.ReadObjectAsList(Utf8JsonReaderExtensions.LockFileItemConverter);
                Assert.Equal(0, results.Count);
                Assert.IsType(Array.Empty<LockFileItem>().GetType(), results);
            }
        }

        [Fact]
        public void ReadObjectAsList_WithInvalidValue_ThrowsException()
        {
            var json = "\"a\": { \"property1\": \"1\" }";

            var encodedBytes = Encoding.UTF8.GetBytes(json);
            Assert.Throws<JsonException>(() =>
            {
                using (var stream = new MemoryStream(encodedBytes))
                using (var reader = new Utf8JsonStreamReader(stream))
                {
                    reader.ReadObjectAsList(Utf8JsonReaderExtensions.LockFileItemConverter);
                }
            });
        }

        [Fact]
        public void ReadStringArrayAsReadOnlyListFromArrayStart_WhenValuesAreConvertibleToString_ReturnsReadOnlyList()
        {
            const string json = "[null, true, -2, 3.14, \"a\"]";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(encodedBytes))
            using (var reader = new Utf8JsonStreamReader(stream))
            {
                Assert.Equal(JsonTokenType.StartArray, reader.TokenType);

                IEnumerable<string> actualResults = reader.ReadStringArrayAsReadOnlyListFromArrayStart();

                Assert.Collection(
                    actualResults,
                    actualResult => Assert.Null(null),
                    actualResult => Assert.Equal("True", actualResult),
                    actualResult => Assert.Equal("-2", actualResult),
                    actualResult => Assert.Equal("3.14", actualResult),
                    actualResult => Assert.Equal("a", actualResult));
                Assert.Equal(JsonTokenType.EndArray, reader.TokenType);
            }
        }

        [Theory]
        [InlineData("[]", JsonTokenType.StartArray)]
        [InlineData("{}", JsonTokenType.StartObject)]
        public void ReadStringArrayAsReadOnlyListFromArrayStart_WhenValuesAreNotConvertibleToString_Throws(
            string value,
            JsonTokenType expectedToken)
        {
            var json = $"[{value}]";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            var tokenType = JsonTokenType.None;
            var exceptionThrown = Assert.Throws<InvalidCastException>(() =>
            {
                using var stream = new MemoryStream(encodedBytes);
                using var reader = new Utf8JsonStreamReader(stream);
                Assert.Equal(JsonTokenType.StartArray, reader.TokenType);
                try
                {
                    reader.ReadStringArrayAsReadOnlyListFromArrayStart();
                }
                finally
                {
                    tokenType = reader.TokenType;
                }
            });
            Assert.Equal(expectedToken, tokenType);
        }

        [Theory]
        [InlineData("value")]
        [InlineData("")]
        public void ValueTextEquals_WithValidData_ReturnTrue(string value)
        {
            var json = $"{{ \"{value}\":\"property\"}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            var utf8Bytes = Encoding.UTF8.GetBytes(value);
            using var stream = new MemoryStream(encodedBytes);
            using var reader = new Utf8JsonStreamReader(stream);
            reader.Read();
            Assert.True(reader.ValueTextEquals(utf8Bytes));
        }

        private Mock<ArrayPool<byte>> SetupMockArrayBuffer()
        {
            Mock<ArrayPool<byte>> mock = new Mock<ArrayPool<byte>>();
            mock.Setup(m => m.Rent(1024)).Returns(new byte[1024]);
            mock.Setup(m => m.Rent(2048)).Returns(new byte[2048]);
            mock.Setup(m => m.Return(It.IsAny<byte[]>(), It.IsAny<bool>()));

            return mock;
        }
    }
}
