// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.DiaSymReader.UnitTests
{
    public class ComMemoryStreamTests
    {
        private string ChunksToString(IEnumerable<ArraySegment<byte>> chunks)
            => string.Join("|", chunks.Select(chunk => BitConverter.ToString(chunk.Array, chunk.Offset, chunk.Count)));

        [Fact]
        public void GetChunks_Empty()
        {
            var stream = new ComMemoryStream();
            var chunks = stream.GetChunks().ToArray();
            Assert.Empty(chunks);
        }

        [Fact]
        public unsafe void GetChunks_MultiChunkWrite()
        {
            var stream = new ComMemoryStream(chunkSize: 1);

            fixed (byte* buffer = new byte[] { 0, 1, 2, 3 })
            {
                int written;
                ((IUnsafeComStream)stream).Write(buffer, 3, (int*)&written);
                Assert.Equal(3, written);
            }

            AssertEx.Equal("00|01|02", ChunksToString(stream.GetChunks()));
        }

        [Fact]
        public unsafe void GetChunks_MultiChunkSeek()
        {
            var stream = new ComMemoryStream(chunkSize: 1);

            long position;
            ((IUnsafeComStream)stream).Seek(5, ComMemoryStream.STREAM_SEEK_SET, &position);
            Assert.Equal(5, position);

            Assert.Equal("00-00-00-00-00", ChunksToString(stream.GetChunks()));
        }

        [Fact]
        public unsafe void GetChunks_SeekWriteRead()
        {
            var stream = new ComMemoryStream(chunkSize: 5);

            long position;
            ((IUnsafeComStream)stream).Seek(5, ComMemoryStream.STREAM_SEEK_SET, &position);
            Assert.Equal(5, position);
            Assert.Equal("00-00-00-00-00", ChunksToString(stream.GetChunks()));

            ((IUnsafeComStream)stream).Seek(5, ComMemoryStream.STREAM_SEEK_CUR, &position);
            Assert.Equal(10, position);
            Assert.Equal("00-00-00-00-00-00-00-00-00-00", ChunksToString(stream.GetChunks()));

            ((IUnsafeComStream)stream).Seek(-2, ComMemoryStream.STREAM_SEEK_END, &position);
            Assert.Equal(8, position);
            Assert.Equal("00-00-00-00-00-00-00-00-00-00", ChunksToString(stream.GetChunks()));

            fixed (byte* bufferPtr = new byte[] { 2, 3 })
            {
                int written;
                ((IUnsafeComStream)stream).Write(bufferPtr, 2, &written);
                Assert.Equal(2, written);
            }

            Assert.Equal("00-00-00-00-00|00-00-00-02-03", ChunksToString(stream.GetChunks()));

            ((IUnsafeComStream)stream).Seek(1, ComMemoryStream.STREAM_SEEK_CUR, &position);
            Assert.Equal(11, position);
            Assert.Equal("00-00-00-00-00|00-00-00-02-03|00", ChunksToString(stream.GetChunks()));

            ((IUnsafeComStream)stream).Seek(-3, ComMemoryStream.STREAM_SEEK_CUR, &position);
            Assert.Equal(8, position);

            var buffer = new byte[] { 1, 1, 1, 1, 1 };
            fixed (byte* bufferPtr = buffer)
            {
                int read;
                ((IUnsafeComStream)stream).Read(bufferPtr, 3, &read);

                AssertEx.Equal(new byte[] { 2, 3, 0, 1, 1 }, buffer);
            }
        }
    }
}
