// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Interop.PortableExecutable
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_NT_HEADERS32
    {
        public UInt32 Signature;
        public IMAGE_FILE_HEADER FileHeader;
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader32;

        /// <summary>
        /// Reads the <see cref="IMAGE_NT_HEADERS32"/> structure from a file, starting at a specific offset
        /// </summary>
        /// <param name="path">The file to read</param>
        /// <param name="headerOffset">The offset in the file from where the structure will be read.</param>
        /// <returns></returns>
        public static IMAGE_NT_HEADERS32 Read(string path, UInt32 headerOffset)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(headerOffset, SeekOrigin.Begin);

                IMAGE_NT_HEADERS32 imageNTHeaders32 = new IMAGE_NT_HEADERS32();
                imageNTHeaders32.Signature = reader.ReadUInt32();
                imageNTHeaders32.FileHeader = ImageFileHeader.Read(reader);
                imageNTHeaders32.OptionalHeader32 = ImageOptionalHeader32.Read(reader);

                return imageNTHeaders32;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_NT_HEADERS64
    {
        public UInt32 Signature;
        public IMAGE_FILE_HEADER FileHeader;
        public IMAGE_OPTIONAL_HEADER64 OptionalHeader64;

        /// <summary>
        /// Reads the <see cref="IMAGE_NT_HEADERS64"/> structure from a file, starting at a specific offset
        /// </summary>
        /// <param name="path">The file to read</param>
        /// <param name="headerOffset">The offset in the file from where the structure will be read.</param>
        /// <returns></returns>
        public static IMAGE_NT_HEADERS64 Read(string path, UInt32 headerOffset)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(headerOffset, SeekOrigin.Begin);

                IMAGE_NT_HEADERS64 imageNTHeaders64 = new IMAGE_NT_HEADERS64();
                imageNTHeaders64.Signature = reader.ReadUInt32();
                imageNTHeaders64.FileHeader = ImageFileHeader.Read(reader);
                imageNTHeaders64.OptionalHeader64 = ImageOptionalHeader64.Read(reader);

                return imageNTHeaders64;
            }
        }
    }

    public static class ImageNTHeaders
    {
        /// <summary>
        /// Constant representing the PE signature, PE00.
        /// </summary>
        public const uint IMAGE_NT_SIGNATURE = 0x00004550; 
    }
}
