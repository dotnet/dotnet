// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

#if NET9_0_OR_GREATER
using System.Runtime.InteropServices.Marshalling;
#endif

#if NETSTANDARD2_0
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;
#endif

namespace Microsoft.DiaSymReader
{
#if NET9_0_OR_GREATER
    [GeneratedComClass]
#endif
    internal unsafe sealed partial class ComStreamWrapper : IUnsafeComStream, System.Runtime.InteropServices.ComTypes.IStream
    {
        private readonly Stream _stream;

        internal ComStreamWrapper(Stream stream)
        {
            Debug.Assert(stream != null);
            Debug.Assert(stream.CanSeek);

            _stream = stream;
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
        public unsafe void Read(byte* pv, int cb, int* pcbRead)
        {
            var buffer = new byte[cb];
            int bytesRead = TryReadAll(_stream, buffer, 0, cb);

            for (int i = 0; i < bytesRead; ++i)
            {
                pv[i] = buffer[i];
            }

            if (pcbRead != null)
            {
                *pcbRead = bytesRead;
            }
        }

        void System.Runtime.InteropServices.ComTypes.IStream.Read(byte[] pv, int cb, nint pcbRead)
        {
            fixed (byte* p = pv)
            {
                Read(p, cb, (int*)pcbRead);
            }
        }

        public unsafe void Seek(long dlibMove, int origin, long* plibNewPosition)
        {
            long newPosition = _stream.Seek(dlibMove, (SeekOrigin)origin);
            if (plibNewPosition != null)
            {
                *plibNewPosition = newPosition;
            }
        }

        void System.Runtime.InteropServices.ComTypes.IStream.Seek(long dlibMove, int dwOrigin, nint plibNewPosition)
            => Seek(dlibMove, dwOrigin, (long*)plibNewPosition);

        public unsafe void Write(byte* pv, int cb, int* pcbWritten)
        {
            var buffer = new byte[cb];
            for (int i = 0; i < cb; ++i)
            {
                buffer[i] = pv[i];
            }

            _stream.Write(buffer, 0, cb);
            if (pcbWritten != null)
            {
                *pcbWritten = cb;
            }
        }

        void System.Runtime.InteropServices.ComTypes.IStream.Write(byte[] pv, int cb, nint pcbWritten)
        {
            fixed (byte* p = pv)
            {
                Write(p, cb, (int*)pcbWritten);
            }
        }

        public void Clone(out IntPtr ppstm)
            => throw new NotSupportedException();

        void System.Runtime.InteropServices.ComTypes.IStream.Clone(out System.Runtime.InteropServices.ComTypes.IStream ppstm)
            => throw new NotSupportedException();

        public void Commit(int grfCommitFlags)
            => _stream.Flush();

        void System.Runtime.InteropServices.ComTypes.IStream.Commit(int grfCommitFlags)
            => Commit(grfCommitFlags);

        public void CopyTo(IntPtr pstm, long cb, int* pcbRead, int* pcbWritten)
            => throw new NotSupportedException();

        void System.Runtime.InteropServices.ComTypes.IStream.CopyTo(System.Runtime.InteropServices.ComTypes.IStream pstm, long cb, nint pcbRead, nint pcbWritten)
            => throw new NotSupportedException();

        public void LockRegion(long libOffset, long cb, int lockType)
            => throw new NotSupportedException();

        void System.Runtime.InteropServices.ComTypes.IStream.LockRegion(long libOffset, long cb, int dwLockType)
            => throw new NotSupportedException();

        public void UnlockRegion(long libOffset, long cb, int lockType)
            => throw new NotSupportedException();

        void System.Runtime.InteropServices.ComTypes.IStream.UnlockRegion(long libOffset, long cb, int dwLockType)
            => throw new NotSupportedException();

        public void Revert()
            => throw new NotSupportedException();

        void System.Runtime.InteropServices.ComTypes.IStream.Revert()
            => throw new NotSupportedException();

        public void SetSize(long libNewSize)
            => _stream.SetLength(libNewSize);

        void System.Runtime.InteropServices.ComTypes.IStream.SetSize(long libNewSize)
            => SetSize(libNewSize);

        public void Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new STATSTG()
            {
                cbSize = _stream.Length
            };
        }

        void System.Runtime.InteropServices.ComTypes.IStream.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG()
            {
                cbSize = _stream.Length
            };
        }

#if NET9_0_OR_GREATER
        [CustomMarshaller(typeof(System.Runtime.InteropServices.ComTypes.IStream), MarshalMode.ManagedToUnmanagedIn, typeof(Marshaller))]
        [CustomMarshaller(typeof(System.Runtime.InteropServices.ComTypes.IStream), MarshalMode.UnmanagedToManagedIn, typeof(Marshaller))]
        public static class Marshaller
        {
            public static IntPtr ConvertToUnmanaged(System.Runtime.InteropServices.ComTypes.IStream stream)
            {
                if (stream is null)
                {
                    return IntPtr.Zero;
                }
                else if (stream is IUnsafeComStream unsafeComStream)
                {
                    return (IntPtr)ComInterfaceMarshaller<IUnsafeComStream>.ConvertToUnmanaged(unsafeComStream);
                }

                throw new NotSupportedException("IStream implementation cannot be marshalled");
            }

            public static System.Runtime.InteropServices.ComTypes.IStream ConvertToManaged(IntPtr s)
                => throw new NotSupportedException("IStream cannot be marshalled to managed");

            public static void Free(IntPtr unmanaged)
            {
                if (unmanaged != IntPtr.Zero)
                {
                    Marshal.Release(unmanaged);
                }
            }
        }
#endif
    }
}
