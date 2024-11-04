// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RpmLead
    {
        public string Name { get; set; }
        public byte Major { get; set; }
        public byte Minor { get; set; }
        public short Type { get; set; }
        public short Architecture { get; set; }
        public short OperatingSystem { get; set; }
        public short SignatureType { get; set; }

        public static RpmLead Read(Stream stream)
        {
            if (!stream.ReadExactly(4).SequenceEqual((ReadOnlySpan<byte>)[0xed, 0xab, 0xee, 0xdb]))
            {
                throw new InvalidDataException("Invalid RPM magic");
            }
            
            RpmLead lead = default;
            lead.Major = (byte)stream.ReadByte();
            lead.Minor = (byte)stream.ReadByte();
            lead.Type = BinaryPrimitives.ReadInt16BigEndian(stream.ReadExactly(2));
            lead.Architecture = BinaryPrimitives.ReadInt16BigEndian(stream.ReadExactly(2));
            byte[] name = new byte[66];
            stream.ReadExactly(name, 0, 66);
            lead.Name = Encoding.UTF8.GetString(name, 0, Array.IndexOf<byte>(name, 0));
            lead.OperatingSystem = BinaryPrimitives.ReadInt16BigEndian(stream.ReadExactly(2));
            lead.SignatureType = BinaryPrimitives.ReadInt16BigEndian(stream.ReadExactly(2));
            stream.ReadExactly(16); // Skip reserved
            return lead;
        }

        public readonly void WriteTo(Stream stream)
        {
            stream.Write([0xed, 0xab, 0xee, 0xdb]);
            stream.WriteByte(Major);
            stream.WriteByte(Minor);
            byte[] beBytes = new byte[2];
            BinaryPrimitives.WriteInt16BigEndian(beBytes, Type);
            stream.Write(beBytes);
            BinaryPrimitives.WriteInt16BigEndian(beBytes, Architecture);
            stream.Write(beBytes);
            byte[] name = new byte[66];
            Encoding.UTF8.GetBytes(Name, 0, Name.Length, name, 0);
            name[65] = 0;
            stream.Write(name);

            BinaryPrimitives.WriteInt16BigEndian(beBytes, OperatingSystem);
            stream.Write(beBytes);
            BinaryPrimitives.WriteInt16BigEndian(beBytes, SignatureType);
            stream.Write(beBytes);
            
            stream.Write(new byte[16]);
        }

        public override string ToString()
        {
            return $"{Name} {Major}.{Minor} {Type} Arch({Architecture}) OS({OperatingSystem}) Sig({SignatureType})";
        }
    }
}
