// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    internal sealed class ArWriter : IDisposable
    {
        private Stream _stream;
        private readonly bool _leaveOpen;

        public ArWriter(Stream output, bool leaveOpen)
        {
            _stream = output;
            _leaveOpen = leaveOpen;
#if NET
            _stream.Write("!<arch>\n"u8);
#else
            byte[] magic = Encoding.ASCII.GetBytes("!<arch>\n");
            _stream.Write(magic, 0, magic.Length);
#endif
        }

        public void AddEntry(ArEntry entry)
        {
            Write(Encoding.ASCII.GetBytes(entry.Name.PadRight(16, ' ').Substring(0, 16)));
            Write(Encoding.ASCII.GetBytes(entry.Timestamp.ToString().PadRight(12, ' ').Substring(0, 12)));
            Write(Encoding.ASCII.GetBytes(entry.OwnerID.ToString().PadRight(6, ' ').Substring(0, 6)));
            Write(Encoding.ASCII.GetBytes(entry.GroupID.ToString().PadRight(6, ' ').Substring(0, 6)));
            Write(Encoding.ASCII.GetBytes(Convert.ToString(entry.Mode, 8).PadRight(8, ' ').Substring(0, 8)));

            ulong length = (ulong)entry.DataStream.Length;

            Write(Encoding.ASCII.GetBytes(length.ToString().PadRight(10, ' ').Substring(0, 10)));
            Write(Encoding.ASCII.GetBytes("`\n"));
            entry.DataStream.CopyTo(_stream);

            if ((length % 2) == 1)
            {
                // Pad to even length with a newline
                _stream.WriteByte((byte)'\n');
            }
        }

        private void Write(byte[] data)
        {
#if NET
            _stream.Write(data);
#else
            _stream.Write(data, 0, data.Length);
#endif
        }

        public void Dispose()
        {
            if (!_leaveOpen)
            {
                _stream.Dispose();
            }
        }
    }
}
