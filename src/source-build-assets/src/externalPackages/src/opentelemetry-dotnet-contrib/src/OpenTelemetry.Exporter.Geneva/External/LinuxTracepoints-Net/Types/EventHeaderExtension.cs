#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// <para>
    /// Additional information for an EventHeader event.
    /// </para><para>
    /// If EventHeader.Flags has the Extension bit set then the EventHeader is
    /// followed by one or more EventHeaderExtension blocks. Otherwise the EventHeader
    /// is followed by the event payload data.
    /// </para><para>
    /// If EventHeaderExtension.Kind has the Chain flag set then the
    /// EventHeaderExtension block is followed immediately (no alignment/padding) by
    /// another extension block. Otherwise it is followed immediately (no
    /// alignment/padding) by the event payload data.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EventHeaderExtension
    {
        /// <summary>
        /// The size of this structure in bytes (4).
        /// </summary>
        public const int SizeOfStruct = 4;

        /// <summary>
        /// The size of the extension data in bytes.
        /// The data immediately follows this structure with no padding/alignment.
        /// </summary>
        public ushort Size;

        /// <summary>
        /// The kind of extension. This determines the format of the extension data.
        /// In addition, the Chain flag indicates whether another extension follows.
        /// </summary>
        public EventHeaderExtensionKind Kind;

        // Followed by Size bytes of data. No padding/alignment.
    }
}
