// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_SECTION_HEADER
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] Name;

        [FieldOffset(8)]
        public UInt32 VirtualSize;

        [FieldOffset(12)]
        public UInt32 VirtualAddress;

        [FieldOffset(16)]
        public UInt32 SizeOfRawData;

        [FieldOffset(20)]
        public UInt32 PointerToRawData;

        [FieldOffset(24)]
        public UInt32 PointerToRelocations;

        [FieldOffset(28)]
        public UInt32 PointerToLinenumbers;

        [FieldOffset(32)]
        public UInt16 NumberOfRelocations;

        [FieldOffset(34)]
        public UInt16 NumberOfLinenumbers;

        [FieldOffset(36)]
        public UInt32 Characteristics;

        /// <summary>
        /// Converts the <see cref="IMAGE_SECTION_HEADER.Name"/> to a string.
        /// </summary>
        public string SectionName
        {
            get
            {
                return new string(Name);
            }
        }

        /// <summary>
        /// Retrieve a list of <see cref="IMAGE_SECTION_HEADER"/> from a file.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="numberOfSections">The number of sections to retrieve</param>
        /// <returns></returns>
        public static List<IMAGE_SECTION_HEADER> Read(BinaryReader reader, ushort numberOfSections, uint firstSectionHeaderOffset)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            var sectionHeaders = new List<IMAGE_SECTION_HEADER>();
            reader.BaseStream.Seek(firstSectionHeaderOffset, SeekOrigin.Begin);

            for (int i = 0; i < numberOfSections; i++)
            {
                IMAGE_SECTION_HEADER sectionHeader = new IMAGE_SECTION_HEADER
                {
                    Name = reader.ReadChars(8),
                    VirtualSize = reader.ReadUInt32(),
                    VirtualAddress = reader.ReadUInt32(),
                    SizeOfRawData = reader.ReadUInt32(),
                    PointerToRawData = reader.ReadUInt32(),
                    PointerToRelocations = reader.ReadUInt32(),
                    PointerToLinenumbers = reader.ReadUInt32(),
                    NumberOfLinenumbers = reader.ReadUInt16(),
                    NumberOfRelocations = reader.ReadUInt16(),
                    Characteristics = reader.ReadUInt32()
                };

                sectionHeaders.Add(sectionHeader);
            }

            return sectionHeaders;
        }
    }
}
