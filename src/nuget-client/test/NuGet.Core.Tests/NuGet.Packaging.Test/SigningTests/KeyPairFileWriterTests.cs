// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class KeyPairFileWriterTests
    {
        private static readonly Encoding _encoding = new UTF8Encoding();

        [Fact]
        public void Constructor_IfStreamIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new KeyPairFileWriter(stream: null, encoding: _encoding, leaveOpen: true));

            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Constructor_IfEncodingIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new KeyPairFileWriter(Stream.Null, encoding: null, leaveOpen: true));

            Assert.Equal("encoding", exception.ParamName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Constructor_WithLeaveOpen_TogglesStreamDisposal(bool leaveOpen)
        {
            using (var stream = new MemoryStream())
            {
                Assert.True(stream.CanWrite);

                using (var writer = new KeyPairFileWriter(stream, _encoding, leaveOpen))
                {
                }

                Assert.Equal(leaveOpen, stream.CanWrite);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WritePair_WhenKeyNullOrEmpty_Throws(string key)
        {
            using (var stream = new MemoryStream())
            using (var writer = new KeyPairFileWriter(stream, _encoding, leaveOpen: true))
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => writer.WritePair(key, "b"));

                Assert.Equal("key", exception.ParamName);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WritePair_WhenValueNull_Throws(string value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new KeyPairFileWriter(stream, _encoding, leaveOpen: true))
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => writer.WritePair("a", value));

                Assert.Equal("value", exception.ParamName);
            }
        }

        [Fact]
        public void WritePair_WithValidInput_WritesContent()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new KeyPairFileWriter(stream, _encoding, leaveOpen: true))
                {
                    writer.WritePair("a", "b");
                }

                Assert.Equal(_encoding.GetBytes("a:b\n"), stream.ToArray());
            }
        }

        [Fact]
        public void WriteSectionBreak_WritesEol()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new KeyPairFileWriter(stream, _encoding, leaveOpen: true))
                {
                    writer.WriteSectionBreak();
                }

                Assert.Equal(_encoding.GetBytes("\n"), stream.ToArray());
            }
        }
    }
}
