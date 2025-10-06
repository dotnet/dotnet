// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    internal static class StreamHelpers
    {
#if !NET
        public static void ReadExactly(this Stream stream, byte[] buffer, int offset, int count)
        {
            while (count > 0)
            {
                int read = stream.Read(buffer, offset, count);
                if (read == 0)
                {
                    throw new InvalidOperationException("Unexpected end of stream");
                }
                offset += read;
                count -= read;
            }
        }

        public static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }
#endif

        public static Span<byte> ReadExactly(this Stream stream, int n)
        {
            byte[] buffer = new byte[n];
            stream.ReadExactly(buffer, 0, n);
            return buffer;
        }

        public static int AlignUp(this int value, int alignment)
        {
            return (value + alignment - 1) & ~(alignment - 1);
        }

        public static long AlignUp(this long value, int alignment)
        {
            return (value + alignment - 1) & ~(alignment - 1);
        }

        public static void AlignReadTo(this Stream stream, int alignment)
        {
            stream.Position = stream.Position.AlignUp(alignment);
        }

        public static void AlignWriteTo(this Stream stream, int alignment)
        {
            int padding = (int)(stream.Position.AlignUp(alignment) - stream.Position);
            for (int i = 0; i < padding; i++)
            {
                stream.WriteByte(0);
            }
        }
    }
}
