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
    internal sealed class CpioWriter(Stream stream, bool leaveOpen) : IDisposable
    {
        public void WriteNextEntry(CpioEntry entry)
        {
            stream.Write("070701"u8.ToArray());

            using StreamWriter writer = new(stream, Encoding.ASCII, bufferSize: -1, leaveOpen: true);

            writer.Write(entry.Inode.ToString("x8"));
            writer.Write(entry.Mode.ToString("x8"));
            writer.Write(entry.OwnerID.ToString("x8"));
            writer.Write(entry.GroupID.ToString("x8"));
            writer.Write(entry.NumberOfLinks.ToString("x8"));
            writer.Write(entry.Timestamp.ToString("x8"));
            writer.Write(entry.DataStream.Length.ToString("x8"));
            writer.Write(entry.DevMajor.ToString("x8"));
            writer.Write(entry.DevMinor.ToString("x8"));
            writer.Write(entry.RDevMajor.ToString("x8"));
            writer.Write(entry.RDevMinor.ToString("x8"));
            writer.Write((entry.Name.Length + 1).ToString("x8")); // This field should include the null terminator.
            writer.Flush();
            stream.Write("00000000"u8.ToArray()); // Check field
            writer.Write(entry.Name);
            writer.Flush();
            stream.WriteByte(0);
            stream.AlignWriteTo(4);
            entry.DataStream.CopyTo(stream);
            stream.AlignWriteTo(4);
        }

        public void Dispose()
        {
            CpioEntry trailerEntry = new(0, "TRAILER!!!", 0, 0, 0, 0, 0, 0, 0, 0, 0, new MemoryStream());
            WriteNextEntry(trailerEntry);
            if (!leaveOpen)
            {
                stream.Dispose();
            }
        }
    }
}
