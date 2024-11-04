// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    internal sealed class CpioReader(Stream stream, bool leaveOpen) : IDisposable
    {
        public CpioEntry? GetNextEntry()
        {
            if (stream.Position == stream.Length)
            {
                return null;
            }

            byte[] magic = new byte[6];
            stream.ReadExactly(magic, 0, 6);
            if (!magic.AsSpan().SequenceEqual("070701"u8))
            {
                throw new InvalidDataException("Invalid header magic");
            }

            byte[] byteBuffer = new byte[8];

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong inode = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            uint mode = uint.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong ownerID = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong groupID = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ushort numberOfLinks = ushort.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong timestamp = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong size = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong devMajor = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong devMinor = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong rdevMajor = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            ulong rdevMinor = ulong.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            stream.ReadExactly(byteBuffer, 0, 8);
            int namesize = int.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            // Ignore checksum
            stream.ReadExactly(byteBuffer, 0, 8);
            _ = int.Parse(Encoding.ASCII.GetString(byteBuffer), NumberStyles.HexNumber);

            byte[] nameBuffer = new byte[namesize];
            stream.ReadExactly(nameBuffer, 0, namesize);
            string name = Encoding.ASCII.GetString(nameBuffer, 0, namesize - 1);

            stream.AlignReadTo(4);

            byte[] data = new byte[size];
            stream.ReadExactly(data, 0, (int)size);            
            MemoryStream dataStream = new(data);

            stream.AlignReadTo(4);

            if (name == "TRAILER!!!")
            {
                // We've reached the end of the archive.
                return null;
            }

            return new CpioEntry(
                inode,
                name,
                timestamp,
                ownerID,
                groupID,
                mode,
                numberOfLinks,
                devMajor,
                devMinor,
                rdevMajor,
                rdevMinor,
                dataStream);
        }

        public void Dispose()
        {
            if (!leaveOpen)
            {
                stream.Dispose();
            }
        }
    }
}
