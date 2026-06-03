// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Interop.PortableExecutable
{
    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_COR20_HEADER
    {
        [FieldOffset(0)]
        public uint cb;
        [FieldOffset(4)]
        public ushort MajorRuntimeVersion;
        [FieldOffset(6)]
        public ushort MinorRuntimeVersion;
        [FieldOffset(8)]
        public IMAGE_DATA_DIRECTORY MetaData;
        [FieldOffset(16)]
        public uint Flags;
        [FieldOffset(20)]
        public uint EntryPointRVA;
        [FieldOffset(20)]
        public uint EntryPointToken;
        [FieldOffset(24)]
        public IMAGE_DATA_DIRECTORY Resources;
        [FieldOffset(32)]
        public IMAGE_DATA_DIRECTORY StrongNameSignature;
        [FieldOffset(40)]
        public IMAGE_DATA_DIRECTORY CodeManagerTable;
        [FieldOffset(48)]
        public IMAGE_DATA_DIRECTORY VTableFixup;
        [FieldOffset(56)]
        public IMAGE_DATA_DIRECTORY ExportAddressTableJumps;
        [FieldOffset(64)]
        public IMAGE_DATA_DIRECTORY ManagedNativeHeader;

        /// <summary>
        /// Read the CLR header from a file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="offset">The offset in the file where the CLR header starts.</param>
        /// <returns></returns>
        public static IMAGE_COR20_HEADER Read(string path, uint offset)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                IMAGE_COR20_HEADER imageCor20Header = new IMAGE_COR20_HEADER
                {
                    cb = reader.ReadUInt32(),
                    MajorRuntimeVersion = reader.ReadUInt16(),
                    MinorRuntimeVersion = reader.ReadUInt16(),
                    MetaData = ImageDataDirectory.Read(reader),
                    Flags = reader.ReadUInt32(),
                    EntryPointToken = reader.ReadUInt32(),
                    Resources = ImageDataDirectory.Read(reader),
                    StrongNameSignature = ImageDataDirectory.Read(reader),
                    CodeManagerTable = ImageDataDirectory.Read(reader),
                    VTableFixup = ImageDataDirectory.Read(reader),
                    ExportAddressTableJumps = ImageDataDirectory.Read(reader),
                    ManagedNativeHeader = ImageDataDirectory.Read(reader)
                };

                return imageCor20Header;
            }
        }
    }
}
