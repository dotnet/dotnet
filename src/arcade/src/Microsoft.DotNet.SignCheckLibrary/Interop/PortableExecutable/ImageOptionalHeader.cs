// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Interop.PortableExecutable
{
    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_OPTIONAL_HEADER32
    {
        [FieldOffset(0)]
        public ImageOptionalHeaderMagic Magic;

        [FieldOffset(2)]
        public byte MajorLinkerVersion;

        [FieldOffset(3)]
        public byte MinorLinkerVersion;

        [FieldOffset(4)]
        public uint SizeOfCode;

        [FieldOffset(8)]
        public uint SizeOfInitializedData;

        [FieldOffset(12)]
        public uint SizeOfUninitializedData;

        [FieldOffset(16)]
        public uint AddressOfEntryPoint;

        [FieldOffset(20)]
        public uint BaseOfCode;

        [FieldOffset(24)]
        public uint BaseOfData;

        [FieldOffset(28)]
        public uint ImageBase;

        [FieldOffset(32)]
        public uint SectionAlignment;

        [FieldOffset(36)]
        public uint FileAlignment;

        [FieldOffset(40)]
        public ushort MajorOperatingSystemVersion;

        [FieldOffset(42)]
        public ushort MinorOperatingSystemVersion;

        [FieldOffset(44)]
        public ushort MajorImageVersion;

        [FieldOffset(46)]
        public ushort MinorImageVersion;

        [FieldOffset(48)]
        public ushort MajorSubsystemVersion;

        [FieldOffset(50)]
        public ushort MinorSubsystemVersion;

        [FieldOffset(52)]
        public uint Win32VersionValue;

        [FieldOffset(56)]
        public uint SizeOfImage;

        [FieldOffset(60)]
        public uint SizeOfHeaders;

        [FieldOffset(64)]
        public uint CheckSum;

        [FieldOffset(68)]
        public SubSystem Subsystem;

        [FieldOffset(70)]
        public DllCharacteristics DllCharacteristics;

        [FieldOffset(72)]
        public uint SizeOfStackReserve;

        [FieldOffset(76)]
        public uint SizeOfStackCommit;

        [FieldOffset(80)]
        public uint SizeOfHeapReserve;

        [FieldOffset(84)]
        public uint SizeOfHeapCommit;

        [FieldOffset(88)]
        public uint LoaderFlags;

        [FieldOffset(92)]
        public uint NumberOfRvaAndSizes;

        [FieldOffset(96)]
        public IMAGE_DATA_DIRECTORY ExportTable;

        [FieldOffset(104)]
        public IMAGE_DATA_DIRECTORY ImportTable;

        [FieldOffset(112)]
        public IMAGE_DATA_DIRECTORY ResourceTable;

        [FieldOffset(120)]
        public IMAGE_DATA_DIRECTORY ExceptionTable;

        [FieldOffset(128)]
        public IMAGE_DATA_DIRECTORY CertificateTable;

        [FieldOffset(136)]
        public IMAGE_DATA_DIRECTORY BaseRelocationTable;

        [FieldOffset(144)]
        public IMAGE_DATA_DIRECTORY Debug;

        [FieldOffset(152)]
        public IMAGE_DATA_DIRECTORY Architecture;

        [FieldOffset(160)]
        public IMAGE_DATA_DIRECTORY GlobalPtr;

        [FieldOffset(168)]
        public IMAGE_DATA_DIRECTORY TLSTable;

        [FieldOffset(176)]
        public IMAGE_DATA_DIRECTORY LoadConfigTable;

        [FieldOffset(184)]
        public IMAGE_DATA_DIRECTORY BoundImport;

        [FieldOffset(192)]
        public IMAGE_DATA_DIRECTORY IAT;

        [FieldOffset(200)]
        public IMAGE_DATA_DIRECTORY DelayImportDescriptor;

        [FieldOffset(208)]
        public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

        [FieldOffset(216)]
        public IMAGE_DATA_DIRECTORY Reserved;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_OPTIONAL_HEADER64
    {
        [FieldOffset(0)]
        public ImageOptionalHeaderMagic Magic;

        [FieldOffset(2)]
        public byte MajorLinkerVersion;

        [FieldOffset(3)]
        public byte MinorLinkerVersion;

        [FieldOffset(4)]
        public uint SizeOfCode;

        [FieldOffset(8)]
        public uint SizeOfInitializedData;

        [FieldOffset(12)]
        public uint SizeOfUninitializedData;

        [FieldOffset(16)]
        public uint AddressOfEntryPoint;

        [FieldOffset(20)]
        public uint BaseOfCode;

        [FieldOffset(24)]
        public ulong ImageBase;

        [FieldOffset(32)]
        public uint SectionAlignment;

        [FieldOffset(36)]
        public uint FileAlignment;

        [FieldOffset(40)]
        public ushort MajorOperatingSystemVersion;

        [FieldOffset(42)]
        public ushort MinorOperatingSystemVersion;

        [FieldOffset(44)]
        public ushort MajorImageVersion;

        [FieldOffset(46)]
        public ushort MinorImageVersion;

        [FieldOffset(48)]
        public ushort MajorSubsystemVersion;

        [FieldOffset(50)]
        public ushort MinorSubsystemVersion;

        [FieldOffset(52)]
        public uint Win32VersionValue;

        [FieldOffset(56)]
        public uint SizeOfImage;

        [FieldOffset(60)]
        public uint SizeOfHeaders;

        [FieldOffset(64)]
        public uint CheckSum;

        [FieldOffset(68)]
        public SubSystem Subsystem;

        [FieldOffset(70)]
        public DllCharacteristics DllCharacteristics;

        [FieldOffset(72)]
        public ulong SizeOfStackReserve;

        [FieldOffset(80)]
        public ulong SizeOfStackCommit;

        [FieldOffset(88)]
        public ulong SizeOfHeapReserve;

        [FieldOffset(96)]
        public ulong SizeOfHeapCommit;

        [FieldOffset(104)]
        public uint LoaderFlags;

        [FieldOffset(108)]
        public uint NumberOfRvaAndSizes;

        [FieldOffset(112)]
        public IMAGE_DATA_DIRECTORY ExportTable;

        [FieldOffset(120)]
        public IMAGE_DATA_DIRECTORY ImportTable;

        [FieldOffset(128)]
        public IMAGE_DATA_DIRECTORY ResourceTable;

        [FieldOffset(136)]
        public IMAGE_DATA_DIRECTORY ExceptionTable;

        [FieldOffset(144)]
        public IMAGE_DATA_DIRECTORY CertificateTable;

        [FieldOffset(152)]
        public IMAGE_DATA_DIRECTORY BaseRelocationTable;

        [FieldOffset(160)]
        public IMAGE_DATA_DIRECTORY Debug;

        [FieldOffset(168)]
        public IMAGE_DATA_DIRECTORY Architecture;

        [FieldOffset(176)]
        public IMAGE_DATA_DIRECTORY GlobalPtr;

        [FieldOffset(184)]
        public IMAGE_DATA_DIRECTORY TLSTable;

        [FieldOffset(192)]
        public IMAGE_DATA_DIRECTORY LoadConfigTable;

        [FieldOffset(200)]
        public IMAGE_DATA_DIRECTORY BoundImport;

        [FieldOffset(208)]
        public IMAGE_DATA_DIRECTORY IAT;

        [FieldOffset(216)]
        public IMAGE_DATA_DIRECTORY DelayImportDescriptor;

        [FieldOffset(224)]
        public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

        [FieldOffset(232)]
        public IMAGE_DATA_DIRECTORY Reserved;
    }

    public static class ImageOptionalHeader32
    {
        /// <summary>
        /// Reads the <see cref="IMAGE_OPTIONAL_HEADER32"/> from a file using the provided reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IMAGE_OPTIONAL_HEADER32 Read(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            IMAGE_OPTIONAL_HEADER32 imageOptionalHeader32 = new IMAGE_OPTIONAL_HEADER32
            {
                Magic = (ImageOptionalHeaderMagic)reader.ReadUInt16(),
                MajorLinkerVersion = reader.ReadByte(),
                MinorLinkerVersion = reader.ReadByte(),
                SizeOfCode = reader.ReadUInt32(),
                SizeOfInitializedData = reader.ReadUInt32(),
                SizeOfUninitializedData = reader.ReadUInt32(),
                AddressOfEntryPoint = reader.ReadUInt32(),
                BaseOfCode = reader.ReadUInt32(),
                BaseOfData = reader.ReadUInt32(),
                ImageBase = reader.ReadUInt32(),
                SectionAlignment = reader.ReadUInt32(),
                FileAlignment = reader.ReadUInt32(),
                MajorOperatingSystemVersion = reader.ReadUInt16(),
                MinorOperatingSystemVersion = reader.ReadUInt16(),
                MajorImageVersion = reader.ReadUInt16(),
                MinorImageVersion = reader.ReadUInt16(),
                MajorSubsystemVersion = reader.ReadUInt16(),
                MinorSubsystemVersion = reader.ReadUInt16(),
                Win32VersionValue = reader.ReadUInt32(),
                SizeOfImage = reader.ReadUInt32(),
                SizeOfHeaders = reader.ReadUInt32(),
                CheckSum = reader.ReadUInt32(),
                Subsystem = (SubSystem)reader.ReadUInt16(),
                DllCharacteristics = (DllCharacteristics)reader.ReadUInt16(),
                SizeOfStackReserve = reader.ReadUInt32(),
                SizeOfStackCommit = reader.ReadUInt32(),
                SizeOfHeapReserve = reader.ReadUInt32(),
                SizeOfHeapCommit = reader.ReadUInt32(),
                LoaderFlags = reader.ReadUInt32(),
                NumberOfRvaAndSizes = reader.ReadUInt32(),
                ExportTable = ImageDataDirectory.Read(reader),
                ImportTable = ImageDataDirectory.Read(reader),
                ResourceTable = ImageDataDirectory.Read(reader),
                ExceptionTable = ImageDataDirectory.Read(reader),
                CertificateTable = ImageDataDirectory.Read(reader),
                BaseRelocationTable = ImageDataDirectory.Read(reader),
                Debug = ImageDataDirectory.Read(reader),
                Architecture = ImageDataDirectory.Read(reader),
                GlobalPtr = ImageDataDirectory.Read(reader),
                TLSTable = ImageDataDirectory.Read(reader),
                LoadConfigTable = ImageDataDirectory.Read(reader),
                BoundImport = ImageDataDirectory.Read(reader),
                IAT = ImageDataDirectory.Read(reader),
                DelayImportDescriptor = ImageDataDirectory.Read(reader),
                CLRRuntimeHeader = ImageDataDirectory.Read(reader),
                Reserved = ImageDataDirectory.Read(reader)
            };

            return imageOptionalHeader32;
        }
    }

    public static class ImageOptionalHeader64
    {

        /// <summary>
        /// Reads the <see cref="IMAGE_OPTIONAL_HEADER64"/> from a file using the provided reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IMAGE_OPTIONAL_HEADER64 Read(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            IMAGE_OPTIONAL_HEADER64 imageOptionalHeader64 = new IMAGE_OPTIONAL_HEADER64
            {
                Magic = (ImageOptionalHeaderMagic)reader.ReadUInt16(),
                MajorLinkerVersion = reader.ReadByte(),
                MinorLinkerVersion = reader.ReadByte(),
                SizeOfCode = reader.ReadUInt32(),
                SizeOfInitializedData = reader.ReadUInt32(),
                SizeOfUninitializedData = reader.ReadUInt32(),
                AddressOfEntryPoint = reader.ReadUInt32(),
                BaseOfCode = reader.ReadUInt32(),
                ImageBase = reader.ReadUInt64(),
                SectionAlignment = reader.ReadUInt32(),
                FileAlignment = reader.ReadUInt32(),
                MajorOperatingSystemVersion = reader.ReadUInt16(),
                MinorOperatingSystemVersion = reader.ReadUInt16(),
                MajorImageVersion = reader.ReadUInt16(),
                MinorImageVersion = reader.ReadUInt16(),
                MajorSubsystemVersion = reader.ReadUInt16(),
                MinorSubsystemVersion = reader.ReadUInt16(),
                Win32VersionValue = reader.ReadUInt32(),
                SizeOfImage = reader.ReadUInt32(),
                SizeOfHeaders = reader.ReadUInt32(),
                CheckSum = reader.ReadUInt32(),
                Subsystem = (SubSystem)reader.ReadUInt16(),
                DllCharacteristics = (DllCharacteristics)reader.ReadUInt16(),
                SizeOfStackReserve = reader.ReadUInt64(),
                SizeOfStackCommit = reader.ReadUInt64(),
                SizeOfHeapReserve = reader.ReadUInt64(),
                SizeOfHeapCommit = reader.ReadUInt64(),
                LoaderFlags = reader.ReadUInt32(),
                NumberOfRvaAndSizes = reader.ReadUInt32(),
                ExportTable = ImageDataDirectory.Read(reader),
                ImportTable = ImageDataDirectory.Read(reader),
                ResourceTable = ImageDataDirectory.Read(reader),
                ExceptionTable = ImageDataDirectory.Read(reader),
                CertificateTable = ImageDataDirectory.Read(reader),
                BaseRelocationTable = ImageDataDirectory.Read(reader),
                Debug = ImageDataDirectory.Read(reader),
                Architecture = ImageDataDirectory.Read(reader),
                GlobalPtr = ImageDataDirectory.Read(reader),
                TLSTable = ImageDataDirectory.Read(reader),
                LoadConfigTable = ImageDataDirectory.Read(reader),
                BoundImport = ImageDataDirectory.Read(reader),
                IAT = ImageDataDirectory.Read(reader),
                DelayImportDescriptor = ImageDataDirectory.Read(reader),
                CLRRuntimeHeader = ImageDataDirectory.Read(reader),
                Reserved = ImageDataDirectory.Read(reader)
            };
            return imageOptionalHeader64;
        }
    }
}
