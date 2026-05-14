#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace Microsoft.LinuxTracepoints
{
    /// <summary>
    /// Values for EventHeader.Flags.
    /// </summary>
    [System.Flags]
    internal enum EventHeaderFlags : byte
    {
        /// <summary>
        /// Pointer32, BigEndian, no extensions.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Pointer is 64 bits, not 32 bits.
        /// </summary>
        Pointer64 = 0x01,

        /// <summary>
        /// Event uses little-endian, not big-endian.
        /// </summary>
        LittleEndian = 0x02,

        /// <summary>
        /// There is at least one EventHeaderExtension block.
        /// </summary>
        Extension = 0x04,
    }
}
