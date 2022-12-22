// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class KeyPairFileReaderTests
    {
        private static readonly Encoding _encoding = new UTF8Encoding();

        [Fact]
        public void Constructor_IfStreamIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new KeyPairFileReader(stream: null, encoding: _encoding));

            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Constructor_IfEncodingIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new KeyPairFileReader(Stream.Null, encoding: null));

            Assert.Equal("encoding", exception.ParamName);
        }

        [Fact]
        public void ReadSection_IfContentEmpty_ReturnsEmptyResult()
        {
            using (var stream = new MemoryStream())
            using (var reader = new KeyPairFileReader(stream, _encoding))
            {
                var section = reader.ReadSection();

                Assert.Empty(section);
            }
        }

        [Theory]
        [InlineData(":")]
        [InlineData("a")]
        [InlineData("a:")]
        [InlineData(":b")]
        [InlineData("&:b")]
        public void ReadSection_IfPropertyIsInvalid_Throws(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            using (var stream = new MemoryStream(bytes))
            using (var reader = new KeyPairFileReader(stream, _encoding))
            {
                var exception = Assert.Throws<SignatureException>(() => reader.ReadSection());

                Assert.Equal("The package signature content is invalid.", exception.Message);
            }
        }

        [Theory]
        [InlineData("a:b")]
        [InlineData("a:b\r\n")]
        public void ReadSection_IfSectionIsNotNewLineTerminated_Throws(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);

            using (var stream = new MemoryStream(bytes))
            using (var reader = new KeyPairFileReader(stream, _encoding))
            {
                var exception = Assert.Throws<SignatureException>(() => reader.ReadSection());

                Assert.Equal("The package signature content is invalid.", exception.Message);
            }
        }

        [Fact]
        public void ReadSection_ReadsMultipleSections()
        {
            var bytes = Encoding.UTF8.GetBytes("a:b\r\n\r\nc:d\r\n\r\n");

            using (var stream = new MemoryStream(bytes))
            using (var reader = new KeyPairFileReader(stream, _encoding))
            {
                var headerSection = reader.ReadSection();

                Assert.Equal(1, headerSection.Count);
                Assert.Equal("b", headerSection["a"]);

                var section = reader.ReadSection();

                Assert.Equal(1, section.Count);
                Assert.Equal("d", section["c"]);

                section = reader.ReadSection();

                Assert.Empty(section);
            }
        }

        [Fact]
        public void Dispose_DisposesStreamReader()
        {
            using (var stream = new MemoryStream())
            {
                Assert.True(stream.CanRead);

                using (var reader = new KeyPairFileReader(stream, _encoding))
                {
                }

                Assert.False(stream.CanRead);
            }
        }
    }
}
