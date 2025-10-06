// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.SignTool
{
    /// <summary>
    /// Represents the type of executable format.
    /// </summary>
    public enum ExecutableType
    {
        /// <summary>
        /// No executable type detected or unknown format.
        /// </summary>
        None,
        
        /// <summary>
        /// Portable Executable format (Windows).
        /// </summary>
        PE,
        
        /// <summary>
        /// Mach-O format (macOS).
        /// </summary>
        MachO,
        
        /// <summary>
        /// Executable and Linkable Format (Linux/Unix).
        /// </summary>
        ELF
    }
}