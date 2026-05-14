// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.DiaSymReader.UnitTests
{
    public class DocumentExtensionsTests
    {
        private sealed class TestSymDocument : SymDocumentMock
        {
            private readonly byte[] _embeddedSource;

            public TestSymDocument(byte[] embeddedSource)
            {
                _embeddedSource = embeddedSource;
            }

            public override int GetSourceLength(out int length)
            {
                length = _embeddedSource.Length;
                return HResult.S_OK;
            }

            public override int GetSourceRange(int startLine, int startColumn, int endLine, int endColumn, int bufferLength, out int count, byte[] source)
            {
                count = _embeddedSource.Length;
                Array.Copy(_embeddedSource, source, count);
                return HResult.S_OK;
            }
        }

        private static byte[] GetCompressedSourceBlob(byte[] srcBytes)
        {
            using (var builder = new MemoryStream())
            {
                using (var writer = new BinaryWriter(builder, Encoding.UTF8, leaveOpen: true))
                {
                    writer.Write(srcBytes.Length);
                }

                using (var srcStream = new MemoryStream(srcBytes))
                using (var deflater = new DeflateStream(builder, CompressionLevel.Optimal, leaveOpen: true))
                {
                    srcStream.CopyTo(deflater);
                }

                return builder.ToArray();
            }
        }

        [Fact]
        public void GetEmbeddedSource_NoSource()
        {
            var doc = new TestSymDocument(new byte[0]);

            var src = doc.GetEmbeddedSource();
            Assert.Null(src.Array);
        }

        [Fact]
        public void GetEmbeddedSource_EmptySource()
        {
            var doc = new TestSymDocument(new byte[] { 0, 0, 0, 0 });

            var src = doc.GetEmbeddedSource();
            AssertEx.Equal(new byte[0], src);
        }

        [Fact]
        public void GetEmbeddedSource_UncompressedSource()
        {
            var doc = new TestSymDocument(new byte[] { 0, 0, 0, 0, (byte)'A', (byte)'B', (byte)'C' });

            var src = doc.GetEmbeddedSource();
            AssertEx.Equal(new byte[] { (byte)'A', (byte)'B', (byte)'C' }, src);
        }

        [Fact]
        public void GetEmbeddedSource_CompressedSource()
        {
            var srcBytes = Encoding.UTF8.GetBytes("Hello world!");
            var doc = new TestSymDocument(GetCompressedSourceBlob(srcBytes));

            var src = doc.GetEmbeddedSource();
            AssertEx.Equal(srcBytes, src);
        }

        [Fact]
        public void GetEmbeddedSource_CompressedLargeSource()
        {
            var srcBytes = new byte[1000000];
            for (int i = 0; i < srcBytes.Length; i++)
            {
                srcBytes[i] = unchecked((byte)i);
            }

            var doc = new TestSymDocument(GetCompressedSourceBlob(srcBytes));

            var src = doc.GetEmbeddedSource();
            AssertEx.Equal(srcBytes, src);
        }

        [Fact]
        public void GetEmbeddedSource_BadLength()
        {
            var doc = new TestSymDocument_BadLength();
            Assert.Throws<InvalidDataException>(() => doc.GetEmbeddedSource());
        }

        private sealed class TestSymDocument_BadLength : SymDocumentMock
        {
            public override int GetSourceLength(out int length)
            {
                length = 1;
                return HResult.S_OK;
            }
        }

        [Fact]
        public void GetEmbeddedSource_BadBytesRead()
        {
            Assert.Throws<InvalidDataException>(() => new TestSymDocument_BadBytesRead { BytesRead = -1 }.GetEmbeddedSource());
            Assert.Throws<InvalidDataException>(() => new TestSymDocument_BadBytesRead { BytesRead = 3 }.GetEmbeddedSource());
            Assert.Throws<InvalidDataException>(() => new TestSymDocument_BadBytesRead { BytesRead = 11 }.GetEmbeddedSource());
        }

        private sealed class TestSymDocument_BadBytesRead : SymDocumentMock
        {
            public int BytesRead;

            public override int GetSourceLength(out int length)
            {
                length = 10;
                return HResult.S_OK;
            }

            public override int GetSourceRange(int startLine, int startColumn, int endLine, int endColumn, int bufferLength, out int count, byte[] source)
            {
                count = BytesRead;
                return HResult.S_OK;
            }
        }

        [Fact]
        public void GetEmbeddedSource_BadCompressedData()
        {
            var data = GetCompressedSourceBlob(new byte[] { 1, 2, 3 });
            var doc = new TestSymDocument(data);

            data[0] = 4;
            Assert.Throws<InvalidDataException>(() => doc.GetEmbeddedSource());

            data[0] = 2;
            Assert.Throws<InvalidDataException>(() => doc.GetEmbeddedSource());

            data[0] = 3;
            doc.GetEmbeddedSource();
        }
    }
}
