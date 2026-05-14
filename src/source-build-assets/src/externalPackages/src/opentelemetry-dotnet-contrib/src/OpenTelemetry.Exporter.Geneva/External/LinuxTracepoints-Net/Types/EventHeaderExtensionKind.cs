#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints
{
    /// <summary>
    /// Values for EventHeaderExtension.Kind.
    /// </summary>
    internal enum EventHeaderExtensionKind : ushort
    {
        /// <summary>
        /// Mask for the base extension kind (low 15 bits).
        /// </summary>
        ValueMask = 0x7fff,

        /// <summary>
        /// If not set, this is the last extension block (event payload data follows).
        /// If set, this is not the last extension block (another extension block follows).
        /// </summary>
        ChainFlag = 0x8000,

        /// <summary>
        /// Invalid extension kind.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// <para>
        /// Extension contains an event definition (i.e. event metadata).
        /// </para><para>
        /// Event definition format:
        /// </para><list type="bullet"><item>
        /// char EventName[]; // Nul-terminated utf-8 string: "eventName{;attribName=attribValue}"
        /// </item><item>
        /// 0 or more field definition blocks.
        /// </item></list><para>
        /// Field definition block:
        /// </para><list type="bullet"><item>
        /// char FieldName[]; // Nul-terminated utf-8 string: "fieldName{;attribName=attribValue}"
        /// </item><item>
        /// uint8_t Encoding; // Encoding is 0..31, with 3 flag bits.
        /// </item><item>
        /// uint8_t Format; // Present if (Encoding &amp; 128). Format is 0..127, with 1 flag bit.
        /// </item><item>
        /// uint16_t Tag; // Present if (Format &amp; 128). Contains provider-defined value.
        /// </item><item>
        /// uint16_t ArrayLength; // Present if (Encoding &amp; 32). Contains element count of constant-length array.
        /// </item></list><para>
        /// Notes:
        /// </para><list type="bullet"><item>
        /// eventName and fieldName may not contain any ';' characters.
        /// </item><item>
        /// eventName and fieldName may be followed by attribute strings.
        /// </item><item>
        /// attribute string is: ';' + attribName + '=' + attribValue.
        /// </item><item>
        /// attribName may not contain any ';' or '=' characters.
        /// </item><item>
        /// Semicolons in attribValue must be escaped by doubling, e.g.
        /// "my;value" is escaped as "my;;value".
        /// </item></list>
        /// </summary>
        Metadata,

        /// <summary>
        /// <para>
        /// Extension contains activity id information.
        /// </para><para>
        /// Any event that is part of an activity has an ActivityId extension.
        /// </para><list type="bullet"><item>
        /// Activity is started by an event with opcode = ActivityStart. The
        /// ActivityId extension for the start event must contain a newly-generated
        /// activity id and may optionally contain the parent activity id.
        /// </item><item>
        /// Activity may contain any number of normal events (opcode something other
        /// than ActivityStart or ActivityStop). The ActivityId extension for each
        /// normal event must contain the id of the associated activity (otherwise
        /// it is not considered to be part of the activity).
        /// </item><item>
        /// Activity is ended by an event with opcode = ActivityStop. The ActivityId
        /// extension for the stop event must contain the id of the activity that is
        /// ending.
        /// </item></list><para>
        /// An activity id is a 128-bit value that is unique within this trace
        /// session. It may be a UUID. Since UUID generation can be expensive, this
        /// may also be a 128-bit LUID (locally-unique id), generated using any method
        /// that is unlikely to conflict with other activity ids in the same trace.
        /// </para><para>
        /// If extension.Size == 16 then value is a 128-bit activity id.
        /// </para><para>
        /// If extension.Size == 32 then value is a 128-bit activity id followed by a
        /// 128-bit related (parent) activity id.
        /// </para>
        /// </summary>
        ActivityId,
    }

    /// <summary>
    /// Extension methods for <see cref="EventHeaderExtensionKind"/>.
    /// </summary>
    internal static class EventHeaderExtensionKindExtensions
    {
        /// <summary>
        /// Returns the format without any flags (format &amp; ValueMask).
        /// </summary>
        public static EventHeaderExtensionKind BaseKind(this EventHeaderExtensionKind kind) =>
            kind & EventHeaderExtensionKind.ValueMask;

        /// <summary>
        /// Returns true if ChainFlag is present (more extensions are present in event).
        /// </summary>
        public static bool HasChainFlag(this EventHeaderExtensionKind kind) =>
            0 != (kind & EventHeaderExtensionKind.ChainFlag);
    }
}
