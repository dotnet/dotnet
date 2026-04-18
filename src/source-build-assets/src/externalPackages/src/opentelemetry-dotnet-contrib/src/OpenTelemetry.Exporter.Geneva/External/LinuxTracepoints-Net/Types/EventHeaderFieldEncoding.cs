#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints
{
    /// <summary>
    /// <para>
    /// Values for the Encoding byte of a field definition.
    /// </para><para>
    /// The low 5 bits of the Encoding byte contain the field's encoding. The encoding
    /// indicates the following information about the field:
    /// <list type="bullet"><item>
    /// How the decoder should determine the size of the field. For example,
    /// <c>Value32</c> indicates a 4-byte field, <c>Value128</c> indicates a 16-byte
    /// field, <c>ZStringChar8</c> indicates that the field ends at the first char8 unit
    /// with value 0, and <c>BinaryLength16Char8</c> indicates that the first 16 bits of
    /// the field are the uint16 <c>Length</c> and that the subsequent <c>Length</c>
    /// char8 units are the field value.
    /// <item></item>
    /// How the field should be formatted if the field's format is
    /// <see cref="EventHeaderFieldFormat.Default"/>, unrecognized, or unsupported. For
    /// example, a <c>Value32</c> encoding with <c>Default</c> or unrecognized format
    /// should be treated as if it had <c>UnsignedInt</c> format. A
    /// <c>StringLength16Char8</c> encoding with <c>Default</c> or unrecognized format
    /// should be treated as if it had <c>StringUtf</c> format. A
    /// <c>BinaryLength16Char8</c> encoding with <c>Default</c> or unrecognized format
    /// should be treated as if it had <c>HexBytes</c> format.
    /// </item>
    /// </list>
    /// The <c>StringLength16Char8</c> and <c>BinaryLength16Char8</c> are special. These
    /// encodings can be used with both variable-length (e.g. <c>HexBytes</c> and
    /// <c>String</c>) formats as well as with fixed-length (e.g. <c>UnsignedInt</c>,
    /// <c>Float</c>, <c>IPAddress</c>) formats. When used with fixed-length formats,
    /// the semantics depend on the field's variable Length (as determined from the first
    /// two bytes of the field):
    /// <list type="bullet"><item>
    /// If the Length is 0, the field is formatted as <c>null</c>. For example, a field
    /// with encoding = <c>BinaryLength16Char8</c>, format = <c>SignedInt</c>, and
    /// Length = 0 would be formatted as a null value.
    /// </item><item>
    /// If the Length is appropriate for the format, the field is formatted as if it had
    /// the Value8, Value16, Value32, Value64, or Value128 encoding corresponding to its
    /// size. For example, a field with encoding = <c>BinaryLength16Char8</c>,
    /// format = <c>SignedInt</c>, and Length = 4 would be formatted as an Int32 field.
    /// </item><item>
    /// If the Length is not appropriate for the format, the field is formatted as if it
    /// had the default format for the encoding. For example, a field with
    /// encoding = <c>BinaryLength16Char8</c>, format = <c>SignedInt</c>, and Length = 16
    /// would be formatted as a <c>HexBytes</c> field since 16 is not a supported size
    /// for the <c>SignedInt</c> format and the default format for
    /// <c>BinaryLength16Char8</c> is <c>HexBytes</c>.
    /// </item></list>
    /// </para><para>
    /// The top 3 bits of the field encoding byte are flags:
    /// </para><list type="bullet"><item>
    /// CArrayFlag indicates that this field is a constant-length array, with the
    /// element count specified as a 16-bit value in the event metadata (must not be
    /// 0).
    /// </item><item>
    /// VArrayFlag indicates that this field is a variable-length array, with the
    /// element count specified as a 16-bit value in the event payload (immediately
    /// before the array elements, may be 0).
    /// </item><item>
    /// ChainFlag indicates that a format byte is present after the encoding byte.
    /// If Chain is not set, the format byte is omitted and is assumed to be 0.
    /// </item></list><para>
    /// Setting both CArray and VArray is invalid (reserved).
    /// </para>
    /// </summary>
    internal enum EventHeaderFieldEncoding : byte
    {
        /// <summary>
        /// Mask for the base encoding type (low 5 bits).
        /// </summary>
        ValueMask = 0x1F,

        /// <summary>
        /// Mask for the encoding flags: CArrayFlag, VArrayFlag, ChainFlag.
        /// </summary>
        FlagMask = 0xE0,

        /// <summary>
        /// Mask for the array flags: CArrayFlag, VArrayFlag.
        /// </summary>
        ArrayFlagMask = 0x60,

        /// <summary>
        /// Constant-length array: 16-bit element count in metadata (must not be 0).
        /// </summary>
        CArrayFlag = 0x20,

        /// <summary>
        /// Variable-length array: 16-bit element count in payload (may be 0).
        /// </summary>
        VArrayFlag = 0x40,

        /// <summary>
        /// If present in the field, this flag indicates that an EventHeaderFieldFormat
        /// byte follows the EventHeaderFieldEncoding byte.
        /// </summary>
        ChainFlag = 0x80,

        /// <summary>
        /// Invalid encoding value.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// 0-byte value, logically groups subsequent N fields, N = format &amp; 0x7F, N must not be 0.
        /// </summary>
        Struct,

        /// <summary>
        /// 1-byte value, default format UnsignedInt.
        /// </summary>
        Value8,

        /// <summary>
        /// 2-byte value, default format UnsignedInt.
        /// </summary>
        Value16,

        /// <summary>
        /// 4-byte value, default format UnsignedInt.
        /// </summary>
        Value32,

        /// <summary>
        /// 8-byte value, default format UnsignedInt.
        /// </summary>
        Value64,

        /// <summary>
        /// 16-byte value, default format HexBytes.
        /// </summary>
        Value128,

        /// <summary>
        /// zero-terminated uint8[], default format StringUtf.
        /// </summary>
        ZStringChar8,

        /// <summary>
        /// zero-terminated uint16[], default format StringUtf.
        /// </summary>
        ZStringChar16,

        /// <summary>
        /// zero-terminated uint32[], default format StringUtf.
        /// </summary>
        ZStringChar32,

        /// <summary>
        /// uint16 Length followed by uint8 Data[Length], default format StringUtf.
        /// This should be treated exactly the same as BinaryLength16Char8 except that it
        /// has a different default format.
        /// </summary>
        StringLength16Char8,

        /// <summary>
        /// uint16 Length followed by uint16 Data[Length], default format StringUtf.
        /// </summary>
        StringLength16Char16,

        /// <summary>
        /// uint16 Length followed by uint32 Data[Length], default format StringUtf.
        /// </summary>
        StringLength16Char32,

        /// <summary>
        /// uint16 Length followed by uint8 Data[Length], default format HexBytes.
        /// This should be treated exactly the same as StringLength16Char8 except that it
        /// has a different default format.
        /// </summary>
        BinaryLength16Char8,

        /// <summary>
        /// Invalid encoding value. Value will change in future versions of this header.
        /// </summary>
        Max,
    }

    /// <summary>
    /// Extension methods for <see cref="EventHeaderFieldEncoding"/>.
    /// </summary>
    internal static class EventHeaderFieldEncodingExtensions
    {
        /// <summary>
        /// Returns the encoding without any flags (encoding &amp; ValueMask).
        /// </summary>
        public static EventHeaderFieldEncoding WithoutFlags(this EventHeaderFieldEncoding encoding) =>
            encoding & EventHeaderFieldEncoding.ValueMask;

        /// <summary>
        /// Returns the encoding without the chain flag (encoding &amp; ~ChainFlag).
        /// </summary>
        public static EventHeaderFieldEncoding WithoutChainFlag(this EventHeaderFieldEncoding encoding) =>
            encoding & ~EventHeaderFieldEncoding.ChainFlag;

        /// <summary>
        /// Returns the array flags of the encoding (VArrayFlag or CArrayFlag, if set).
        /// </summary>
        public static EventHeaderFieldEncoding ArrayFlags(this EventHeaderFieldEncoding encoding) =>
            encoding & EventHeaderFieldEncoding.ArrayFlagMask;

        /// <summary>
        /// Returns true if any ArrayFlag is present (constant-length or variable-length array).
        /// </summary>
        public static bool IsArray(this EventHeaderFieldEncoding encoding) =>
            0 != (encoding & EventHeaderFieldEncoding.ArrayFlagMask);

        /// <summary>
        /// Returns true if CArrayFlag is present (constant-length array).
        /// </summary>
        public static bool IsCArray(this EventHeaderFieldEncoding encoding) =>
            0 != (encoding & EventHeaderFieldEncoding.CArrayFlag);

        /// <summary>
        /// Returns true if VArrayFlag is present (variable-length array).
        /// </summary>
        public static bool IsVArray(this EventHeaderFieldEncoding encoding) =>
            0 != (encoding & EventHeaderFieldEncoding.VArrayFlag);

        /// <summary>
        /// Returns true if ChainFlag is present (format byte is present in event).
        /// </summary>
        public static bool HasChainFlag(this EventHeaderFieldEncoding encoding) =>
            0 != (encoding & EventHeaderFieldEncoding.ChainFlag);

        /// <summary>
        /// Gets the default format for the encoding, or EventHeaderFieldFormat.Default if the encoding is invalid.
        /// <list type="bullet"><item>
        /// Value8, Value16, Value32, Value64: UnsignedInt.
        /// </item><item>
        /// Value128: HexBytes.
        /// </item><item>
        /// String: StringUtf.
        /// </item><item>
        /// Other: Default.
        /// </item></list>
        /// </summary>
        public static EventHeaderFieldFormat DefaultFormat(this EventHeaderFieldEncoding encoding)
        {
            switch (encoding & EventHeaderFieldEncoding.ValueMask)
            {
                case EventHeaderFieldEncoding.Value8:
                case EventHeaderFieldEncoding.Value16:
                case EventHeaderFieldEncoding.Value32:
                case EventHeaderFieldEncoding.Value64:
                    return EventHeaderFieldFormat.UnsignedInt;
                case EventHeaderFieldEncoding.Value128:
                    return EventHeaderFieldFormat.HexBytes;
                case EventHeaderFieldEncoding.ZStringChar8:
                case EventHeaderFieldEncoding.ZStringChar16:
                case EventHeaderFieldEncoding.ZStringChar32:
                case EventHeaderFieldEncoding.StringLength16Char8:
                case EventHeaderFieldEncoding.StringLength16Char16:
                case EventHeaderFieldEncoding.StringLength16Char32:
                    return EventHeaderFieldFormat.StringUtf;
                case EventHeaderFieldEncoding.BinaryLength16Char8:
                    return EventHeaderFieldFormat.HexBytes;
                default:
                    return EventHeaderFieldFormat.Default;
            }
        }
    }
}
