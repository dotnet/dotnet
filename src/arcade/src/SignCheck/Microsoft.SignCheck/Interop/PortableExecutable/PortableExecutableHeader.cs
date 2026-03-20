// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.SignCheck.Interop.PortableExecutable
{
    public class PortableExecutableHeader
    {
        private IMAGE_COR20_HEADER _imageCor20Header;
        private IMAGE_DOS_HEADER _imageDosHeader;
        private IMAGE_NT_HEADERS32 _imageNTHeaders32;
        private IMAGE_NT_HEADERS64 _imageNTHeaders64;
        private List<IMAGE_SECTION_HEADER> _imageSectionHeaders;

        /// <summary>
        /// The <see cref="IMAGE_DATA_DIRECTORY"/> entry pointing to the CLR header.
        /// </summary>
        public IMAGE_DATA_DIRECTORY CLRRuntimeHeader
        {
            get
            {
                if (OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR32_MAGIC)
                {
                    return ImageNTHeaders32.OptionalHeader32.CLRRuntimeHeader;
                }
                else if (OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR64_MAGIC)
                {
                    return ImageNTHeaders64.OptionalHeader64.CLRRuntimeHeader;
                }
                else
                {
                    return ImageDataDirectory.Empty;
                }
            }
        }

        public IMAGE_FILE_HEADER FileHeader
        {
            get
            {
                if (IsPE32)
                {
                    return ImageNTHeaders32.FileHeader;
                }
                else if (IsPE64)
                {
                    return ImageNTHeaders64.FileHeader;
                }

                return new IMAGE_FILE_HEADER();
            }
        }

        /// <summary>
        /// The offset of the first <see cref="IMAGE_SECTION_HEADER"/>.
        /// </summary>
        public uint FirstImageSectionHeaderOffset
        {
            get;
            private set;
        }

        /// <summary>
        /// Exposes access to the CLR runtime header.
        /// </summary>
        public IMAGE_COR20_HEADER ImageCor20Header
        {
            get
            {
                return _imageCor20Header;
            }
        }

        /// <summary>
        /// Exposes access to the <see cref="IMAGE_DOS_HEADER"/> structure of the PE binary.
        /// </summary>
        public IMAGE_DOS_HEADER ImageDosHeader
        {
            get
            {
                return _imageDosHeader;
            }

            private set
            {
                _imageDosHeader = value;
            }
        }

        /// <summary>
        /// Exposes access to the <see cref="IMAGE_NT_HEADERS32"/> structure of the PE binary.
        /// </summary>
        public IMAGE_NT_HEADERS32 ImageNTHeaders32
        {
            get
            {
                return _imageNTHeaders32;
            }

            private set
            {
                _imageNTHeaders32 = value;
            }
        }

        /// <summary>
        /// Exposes access to the <see cref="IMAGE_NT_HEADERS64"/> structure in the PE binary.
        /// </summary>
        public IMAGE_NT_HEADERS64 ImageNTHeaders64
        {
            get
            {
                return _imageNTHeaders64;
            }

            private set
            {
                _imageNTHeaders64 = value;
            }
        }

        /// <summary>
        /// The offset of the IMAGE_NT_HEADERS structure. The structure can either be an <see cref="IMAGE_NT_HEADERS32"/> or
        /// <see cref="IMAGE_NT_HEADERS64"/>.
        /// </summary>
        public uint ImageNTHeadersOffset
        {
            get
            {
                return ImageDosHeader.e_lfanew;
            }
        }

        /// <summary>
        /// Determine if the IMAGE_NT_HEADERS Signature field contains the "PE00" signature.
        /// </summary>
        public bool IsValidNTHeader
        {
            get
            {
                return ImageNTHeadersSignature == ImageNTHeaders.IMAGE_NT_SIGNATURE;
            }
        }

        /// <summary>
        /// The signature of the IMAGE_NT_HEADERS structure.
        /// </summary>
        public uint ImageNTHeadersSignature
        {
            get
            {
                if (OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR32_MAGIC)
                {
                    return ImageNTHeaders32.Signature;
                }
                else if (OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR64_MAGIC)
                {
                    return ImageNTHeaders64.Signature;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// An enumerable collection of <see cref="IMAGE_SECTION_HEADER"/>
        /// </summary>
        public IEnumerable<IMAGE_SECTION_HEADER> ImageSectionHeaders
        {
            get
            {
                if (_imageSectionHeaders == null)
                {
                    _imageSectionHeaders = new List<IMAGE_SECTION_HEADER>();
                }
                return _imageSectionHeaders;
            }
        }

        public bool IsILImage
        {
            get
            {
                return (ImageCor20Header.ManagedNativeHeader.Size == 0) && (ImageCor20Header.ManagedNativeHeader.VirtualAddress == 0);
            }
        }

        public bool IsManagedCode
        {
            get
            {
                return CLRRuntimeHeader.Size > 0;
            }
        }

        public bool IsPE32
        {
            get
            {
                return OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR32_MAGIC;
            }
        }

        public bool IsPE64
        {
            get
            {
                return OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR64_MAGIC;
            }
        }

        /// <summary>
        /// The number of <see cref="IMAGE_SECTION_HEADER"/> entries.
        /// </summary>
        public ushort NumberOfImageSectionHeaders
        {
            get;
            private set;
        }

        public ImageOptionalHeaderMagic OptionalHeaderMagic
        {
            get;
            private set;
        }

        /// <summary>
        /// The path of the executable image.
        /// </summary>
        public string Path
        {
            get;
            private set;
        }

        public uint SectionAlignment
        {
            get
            {
                if (OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR32_MAGIC)
                {
                    return ImageNTHeaders32.OptionalHeader32.SectionAlignment;
                }
                else if (OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR64_MAGIC)
                {
                    return ImageNTHeaders64.OptionalHeader64.SectionAlignment;
                }

                return 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortableExecutableHeader"/> class using the specified path of a portable executable image."/>
        /// </summary>
        /// <param name="path">The path of the executable image.</param>
        public PortableExecutableHeader(string path)
        {
            Path = path;
            ImageDosHeader = IMAGE_DOS_HEADER.Read(path);
            ReadImageNTHeaders();

            if (IsValidNTHeader)
            {
                ReadImageSectionHeaders();

                if (CLRRuntimeHeader.VirtualAddress != 0)
                {
                    IMAGE_SECTION_HEADER section = GetSectionFromRva(CLRRuntimeHeader.VirtualAddress);

                    uint clrOffset = CLRRuntimeHeader.VirtualAddress - section.VirtualAddress + section.PointerToRawData;
                    _imageCor20Header = IMAGE_COR20_HEADER.Read(Path, clrOffset);
                }
            }
        }

        /// <summary>
        /// Locate the <see cref="IMAGE_SECTION_HEADER"/> that corresponds to the given RVA
        /// </summary>
        /// <param name="rva">The address to map to a header.</param>
        /// <returns></returns>
        public IMAGE_SECTION_HEADER GetSectionFromRva(uint rva)
        {
            IMAGE_SECTION_HEADER section = new IMAGE_SECTION_HEADER();

            var sections = ImageSectionHeaders.ToArray();

            int i = 0;
            while (i < FileHeader.NumberOfSections)
            {
                if (rva < sections[i].VirtualAddress + AlignTo(sections[i].VirtualSize, SectionAlignment))
                {
                    if (rva < sections[i].VirtualAddress)
                    {
                        return section;
                    }
                    else
                    {
                        return sections[i];
                    }
                }
                i++;
            }
            return section;
        }

        private void ReadImageNTHeaders()
        {
            // Calculate the offset where the IMAGE_OPTIONAL_HEADER starts
            UInt32 imageOptionalHeaderOffset = ImageNTHeadersOffset + 4 + (UInt32)Marshal.SizeOf<IMAGE_FILE_HEADER>();

            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.BaseStream.Seek(imageOptionalHeaderOffset, SeekOrigin.Begin);

                // Retrieve the Magic field and then read the appropriate (32-bit or 64-bit) IMAGE_NT_OPTIONAL_HEADER
                OptionalHeaderMagic = (ImageOptionalHeaderMagic)reader.ReadUInt16();

                if (OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR32_MAGIC)
                {
                    ImageNTHeaders32 = IMAGE_NT_HEADERS32.Read(Path, ImageDosHeader.e_lfanew);
                    FirstImageSectionHeaderOffset = ImageNTHeadersOffset + (UInt32)Marshal.SizeOf<IMAGE_NT_HEADERS32>();
                    NumberOfImageSectionHeaders = ImageNTHeaders32.FileHeader.NumberOfSections;
                }
                else if (OptionalHeaderMagic == ImageOptionalHeaderMagic.IMAGE_NT_OPTIONAL_HDR64_MAGIC)
                {
                    ImageNTHeaders64 = IMAGE_NT_HEADERS64.Read(Path, ImageDosHeader.e_lfanew);
                    FirstImageSectionHeaderOffset = ImageNTHeadersOffset + (UInt32)Marshal.SizeOf<IMAGE_NT_HEADERS64>();
                    NumberOfImageSectionHeaders = ImageNTHeaders64.FileHeader.NumberOfSections;
                }
            }
        }

        private void ReadImageSectionHeaders()
        {
            using (FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                _imageSectionHeaders = IMAGE_SECTION_HEADER.Read(reader, NumberOfImageSectionHeaders, FirstImageSectionHeaderOffset);
            }
        }

        private static uint AlignTo(uint address, uint boundary)
        {
            return address + boundary - (address % boundary);
        }
    }
}

