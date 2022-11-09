// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.DiaSymReader
{
    internal sealed class ComStreamWrapper : IStream
    {
        private readonly Stream _stream;

        internal ComStreamWrapper(Stream stream)
        {
            Debug.Assert(stream != null);
            Debug.Assert(stream.CanSeek);

            _stream = stream;
        }

        public void Commit(int grfCommitFlags)
        {
            _stream.Flush();
        }

        /// <summary>
        /// Attempts to read all of the requested bytes from the stream into the buffer
        /// </summary>
        /// <returns>
        /// The number of bytes read. Less than <paramref name="count" /> will
        /// only be returned if the end of stream is reached before all bytes can be read.
        /// </returns>
        /// <remarks>
        /// Unlike <see cref="Stream.Read(byte[], int, int)"/> it is not guaranteed that
        /// the stream position or the output buffer will be unchanged if an exception is
        /// returned.
        /// </remarks>
        private static int TryReadAll(Stream stream, byte[] buffer, int offset, int count)
        {
            // The implementations for many streams, e.g. FileStream, allows 0 bytes to be
            // read and returns 0, but the documentation for Stream.Read states that 0 is
            // only returned when the end of the stream has been reached. Rather than deal
            // with this contradiction, let's just never pass a count of 0 bytes
            Debug.Assert(count > 0);

            int totalBytesRead;
            int bytesRead = 0;
            for (totalBytesRead = 0; totalBytesRead < count; totalBytesRead += bytesRead)
            {
                // Note: Don't attempt to save state in-between calls to .Read as it would
                // require a possibly massive intermediate buffer array
                bytesRead = stream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    break;
                }
            }

            return totalBytesRead;
        }

        /// <summary>
        /// The actual number of bytes read can be fewer than the number of bytes requested 
        /// if an error occurs or if the end of the stream is reached during the read operation.
        /// </summary>
        public unsafe void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            int bytesRead = TryReadAll(_stream, pv, 0, cb);

            if (pcbRead != IntPtr.Zero)
            {
                *(int*)pcbRead = bytesRead;
            }
        }

        public unsafe void Seek(long dlibMove, int origin, IntPtr plibNewPosition)
        {
            long newPosition = _stream.Seek(dlibMove, (SeekOrigin)origin);
            if (plibNewPosition != IntPtr.Zero)
            {
                *(long*)plibNewPosition = newPosition;
            }
        }

        public void SetSize(long libNewSize)
        {
            _stream.SetLength(libNewSize);
        }

        public void Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new STATSTG()
            {
                cbSize = _stream.Length
            };
        }

        public unsafe void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            _stream.Write(pv, 0, cb);
            if (pcbWritten != IntPtr.Zero)
            {
                *(int*)pcbWritten = cb;
            }
        }

        public void Clone(out IStream ppstm)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            throw new NotSupportedException();
        }

        public void LockRegion(long libOffset, long cb, int lockType)
        {
            throw new NotSupportedException();
        }

        public void Revert()
        {
            throw new NotSupportedException();
        }

        public void UnlockRegion(long libOffset, long cb, int lockType)
        {
            throw new NotSupportedException();
        }
    }
}
