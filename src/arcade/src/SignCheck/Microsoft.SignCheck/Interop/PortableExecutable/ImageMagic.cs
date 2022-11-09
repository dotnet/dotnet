// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SignCheck.Interop.PortableExecutable
{
    // See winnt.h and  https://docs.microsoft.com/en-us/windows/desktop/api/winnt/ns-winnt-_image_optional_header
    public enum ImageOptionalHeaderMagic : ushort
    {
        /// <summary>
        /// Indicates the file is an executable image (32-bit).
        /// </summary>
        IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b,
        /// <summary>
        /// Indicates the file is an executable image (64-bit).
        /// </summary>
        IMAGE_ROM_OPTIONAL_HDR_MAGIC = 0x107,
        /// <summary>
        /// Indicates the file is a ROM image.
        /// </summary>
        IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0X20B        
    }
}
