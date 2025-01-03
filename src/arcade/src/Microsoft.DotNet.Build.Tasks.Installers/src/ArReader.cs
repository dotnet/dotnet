// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    public sealed class ArReader(Stream stream, bool leaveOpen) : IDisposable
    {
        private bool readMagic;
        public ArEntry? GetNextEntry()
        {
            if (!readMagic)
            {
                byte[] magic = new byte[8];
                ReadExactly(magic, 0, 8);
                readMagic = true;
                if (!magic.AsSpan().SequenceEqual("!<arch>\n"u8))
                {
                    throw new InvalidDataException("Invalid archive magic");
                }
            }

            if (stream.Position == stream.Length)
            {
                return null;
            }

            byte[] fileName = new byte[16];
            ReadExactly(fileName, 0, 16);
            string name = Encoding.ASCII.GetString(fileName).TrimEnd(' ');
            byte[] timestampBytes = new byte[12];
            ReadExactly(timestampBytes, 0, 12);

            ulong timestamp = ulong.Parse(Encoding.ASCII.GetString(timestampBytes).TrimEnd(' '));

            byte[] ownerIDBytes = new byte[6];
            ReadExactly(ownerIDBytes, 0, 6);

            ulong ownerID = ulong.Parse(Encoding.ASCII.GetString(ownerIDBytes).TrimEnd(' '));

            byte[] groupIDBytes = new byte[6];
            ReadExactly(groupIDBytes, 0, 6);

            ulong groupID = ulong.Parse(Encoding.ASCII.GetString(groupIDBytes).TrimEnd(' '));

            byte[] modeBytes = new byte[8];
            ReadExactly(modeBytes, 0, 8);

            uint mode = Convert.ToUInt32(Encoding.ASCII.GetString(modeBytes).TrimEnd(' '), 8);

            byte[] sizeBytes = new byte[10];
            ReadExactly(sizeBytes, 0, 10);

            ulong size = ulong.Parse(Encoding.ASCII.GetString(sizeBytes).TrimEnd(' '));

            byte[] footer = new byte[2];
            ReadExactly(footer, 0, 2);
            if (!footer.AsSpan().SequenceEqual("`\n"u8))
            {
                throw new InvalidDataException("Invalid archive magic");
            }

            byte[] data = new byte[size];
            ReadExactly(data, 0, checked((int)size));

            MemoryStream dataStream = new MemoryStream(data);

            if (size % 2 == 1)
            {
                // Skip the padding newline
                // for an odd-length entry
                _ = stream.ReadByte();
            }

            return new ArEntry(
                name,
                timestamp,
                ownerID,
                groupID,
                mode,
                dataStream);
        }

        public void Dispose()
        {
            if (!leaveOpen)
            {
                stream.Dispose();
            }
        }

#if !NET
        private void ReadExactly(byte[] buffer, int offset, int count)
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
#else
        private void ReadExactly(byte[] buffer, int offset, int count)
        {
            stream.ReadExactly(buffer.AsSpan(offset, count));
        }
#endif
    }
}
