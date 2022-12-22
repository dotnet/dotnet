// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Interop.PortableExecutable
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DOS_HEADER
    {
        /// <summary>
        /// Magic number.
        /// </summary>
        public UInt16 e_magic;
        /// <summary>
        /// Number of bytes on the last page of the file.
        /// </summary>
        public UInt16 e_cblp;
        /// <summary>
        ///  Number of pages in the file.
        /// </summary>
        public UInt16 e_cp;
        /// <summary>
        /// Relocations.
        /// </summary>
        public UInt16 e_crlc;
        /// <summary>
        /// Size of header in paragraphs.
        /// </summary>
        public UInt16 e_cparhdr;
        /// <summary>
        /// Minimum extra paragraphs needed.
        /// </summary>
        public UInt16 e_minalloc;
        /// <summary>
        /// Maximum extra paragraphs needed.
        /// </summary>
        public UInt16 e_maxalloc;
        /// <summary>
        /// Initial (relative) SS value.
        /// </summary>
        public UInt16 e_ss;
        /// <summary>
        /// Initial SP value.
        /// </summary>
        public UInt16 e_sp;
        /// <summary>
        /// Checksum.
        /// </summary>
        public UInt16 e_csum;
        /// <summary>
        /// Initial IP value.
        /// </summary>
        public UInt16 e_ip;
        /// <summary>
        /// Initial (relative) CS value.
        /// </summary>
        public UInt16 e_cs;
        /// <summary>
        /// File address of relocation table.
        /// </summary>
        public UInt16 elfarlc;
        /// <summary>
        /// Overlay number.
        /// </summary>
        public UInt16 e_ovno;
        /// <summary>
        /// Reserved words.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public UInt16[] e_res;
        /// <summary>
        /// OEM identifier (for e_oeminfo).
        /// </summary>
        public UInt16 e_oemid;
        /// <summary>
        /// OEM information - e_oemid specific.
        /// </summary>
        public UInt16 e_oeminfo;
        /// <summary>
        /// Reserved words.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public UInt16[] e_res2;
        /// <summary>
        /// File address of new EXE header (<see cref="IMAGE_NT_HEADERS"/>).
        /// </summary>
        public UInt32 e_lfanew;

        /// <summary>
        /// Reads the <see cref="IMAGE_DOS_HEADER"/> of an executable file.
        /// </summary>
        /// <param name="path">The path of the executable.</param>
        /// <returns>The <see cref="IMAGE_DOS_HEADER"/> of the executable file.</returns>
        public static IMAGE_DOS_HEADER Read(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                var _imageDOSHeader = new IMAGE_DOS_HEADER
                {
                    e_magic = reader.ReadUInt16(),
                    e_cblp = reader.ReadUInt16(),
                    e_cp = reader.ReadUInt16(),
                    e_crlc = reader.ReadUInt16(),
                    e_cparhdr = reader.ReadUInt16(),
                    e_minalloc = reader.ReadUInt16(),
                    e_maxalloc = reader.ReadUInt16(),
                    e_ss = reader.ReadUInt16(),
                    e_sp = reader.ReadUInt16(),
                    e_csum = reader.ReadUInt16(),
                    e_ip = reader.ReadUInt16(),
                    e_cs = reader.ReadUInt16(),
                    elfarlc = reader.ReadUInt16(),
                    e_ovno = reader.ReadUInt16(),
                    e_res = new UInt16[] { reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16() },
                    e_oemid = reader.ReadUInt16(),
                    e_oeminfo = reader.ReadUInt16(),
                    e_res2 = new UInt16[] { reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16() },
                    e_lfanew = reader.ReadUInt32(),
                };

                return _imageDOSHeader;
            }
        }
    }
}
