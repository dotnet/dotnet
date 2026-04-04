#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#pragma warning disable CA1720 // Identifier contains type name

namespace Microsoft.LinuxTracepoints
{
    /// <summary>
    /// <para>
    /// Values for the Format byte of a field definition.
    /// </para><para>
    /// The low 7 bits of the Format byte contain the field's format.
    /// In the case of the Struct encoding, the low 7 bits of the Format byte contain
    /// the number of logical fields in the struct (which must not be 0).
    /// </para><para>
    /// The top bit of the field format byte is the ChainFlag. If set, it indicates
    /// that a field tag (uint16) is present after the format byte. If not set, the
    /// field tag is not present and is assumed to be 0.
    /// </para>
    /// </summary>
    internal enum EventHeaderFieldFormat : byte
    {
        /// <summary>
        /// Mask for the base format type (low 7 bits).
        /// </summary>
        ValueMask = 0x7F,

        /// <summary>
        /// If present in the field, this flag indicates that a uint16
        /// field tag follows the EventHeaderFieldFormat byte.
        /// </summary>
        ChainFlag = 0x80,

        /// <summary>
        /// Use the default format of the encoding.
        /// </summary>
        Default = 0,

        /// <summary>
        /// unsigned integer, event byte order.
        /// Use with Value8..Value64 encodings.
        /// </summary>
        UnsignedInt,

        /// <summary>
        /// signed integer, event byte order.
        /// Use with Value8..Value64 encodings.
        /// </summary>
        SignedInt,

        /// <summary>
        /// hex integer, event byte order.
        /// Use with Value8..Value64 encodings.
        /// </summary>
        HexInt,

        /// <summary>
        /// errno, event byte order.
        /// Use with Value32 encoding.
        /// </summary>
        Errno,

        /// <summary>
        /// process id, event byte order.
        /// Use with Value32 encoding.
        /// </summary>
        Pid,

        /// <summary>
        /// signed integer, event byte order, seconds since 1970.
        /// Use with Value32 or Value64 encodings.
        /// </summary>
        Time,

        /// <summary>
        /// 0 = false, 1 = true, event byte order.
        /// Use with Value8..Value32 encodings.
        /// </summary>
        Boolean,

        /// <summary>
        /// floating point, event byte order.
        /// Use with Value32..Value64 encodings.
        /// </summary>
        Float,

        /// <summary>
        /// binary, decoded as hex dump of bytes.
        /// Use with any encoding.
        /// </summary>
        HexBytes,

        /// <summary>
        /// 8-bit char string, unspecified character set (usually treated as ISO-8859-1 or CP-1252).
        /// Use with Value8 and Char8 encodings.
        /// </summary>
        String8,

        /// <summary>
        /// UTF string, event byte order, code unit size based on encoding.
        /// Use with Value16..Value32 and Char8..Char32 encodings.
        /// </summary>
        StringUtf,

        /// <summary>
        /// UTF string, BOM used if present, otherwise behaves like StringUtf.
        /// Use with Char8..Char32 encodings.
        /// </summary>
        StringUtfBom,

        /// <summary>
        /// XML string, otherwise behaves like StringUtfBom.
        /// Use with Char8..Char32 encodings.
        /// </summary>
        StringXml,

        /// <summary>
        /// JSON string, otherwise behaves like StringUtfBom.
        /// Use with Char8..Char32 encodings.
        /// </summary>
        StringJson,

        /// <summary>
        /// UUID, network byte order (RFC 4122 format).
        /// Use with Value128 encoding.
        /// </summary>
        Uuid,

        /// <summary>
        /// IP port, network byte order (in_port_t layout).
        /// Use with Value16 encoding.
        /// </summary>
        Port,

        /// <summary>
        /// IP address, network byte order (in_addr/in6_addr layout).
        /// Use with Value32 or Value128 encoding.
        /// </summary>
        IPAddress,

        /// <summary>
        /// Deprecated: Alternate format for IPAddress.
        /// Do not generate events with this format.
        /// Decode this format as IPAddress.
        /// </summary>
        IPAddressObsolete,
    }

    /// <summary>
    /// Extension methods for <see cref="EventHeaderFieldFormat"/>.
    /// </summary>
    internal static class EventHeaderFieldFormatExtensions
    {
        /// <summary>
        /// Returns the format without any flags (format &amp; ValueMask).
        /// </summary>
        public static EventHeaderFieldFormat WithoutFlags(this EventHeaderFieldFormat format) =>
            format & EventHeaderFieldFormat.ValueMask;

        /// <summary>
        /// Returns true if ChainFlag is present (tag present in event).
        /// </summary>
        public static bool HasChainFlag(this EventHeaderFieldFormat format) =>
            0 != (format & EventHeaderFieldFormat.ChainFlag);
    }
}
