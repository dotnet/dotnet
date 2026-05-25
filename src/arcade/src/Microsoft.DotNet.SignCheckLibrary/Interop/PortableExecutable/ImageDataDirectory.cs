// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Interop.PortableExecutable
{
    // See https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-image_data_directory
    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_DATA_DIRECTORY
    {
        /// <summary>
        /// The relative virtual address (RVA) of the table.
        /// </summary>
        [FieldOffset(0)]
        public UInt32 VirtualAddress;
        /// <summary>
        /// The size of the table, in bytes.
        /// </summary>
        [FieldOffset(4)]
        public UInt32 Size;
    }

    public static class ImageDataDirectory
    {
        /// <summary>
        /// Reads the <see cref="IMAGE_DATA_DIRECTORY"/> from a file using the current position of an existing reader.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to use</param> for reading the directory entry. The entry is read using the current position of the reader.
        /// <returns>The <see cref="IMAGE_DATA_DIRECTORY"/> that was read.</returns>
        public static IMAGE_DATA_DIRECTORY Read(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return new IMAGE_DATA_DIRECTORY
            {
                VirtualAddress = reader.ReadUInt32(),
                Size = reader.ReadUInt32()
            };
        }

        public static readonly IMAGE_DATA_DIRECTORY Empty = new IMAGE_DATA_DIRECTORY { Size = 0, VirtualAddress = 0 };
    }
}
