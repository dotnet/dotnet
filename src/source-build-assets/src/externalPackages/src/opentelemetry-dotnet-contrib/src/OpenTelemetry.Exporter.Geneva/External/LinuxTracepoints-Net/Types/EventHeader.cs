#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/*--EventHeader Events--------------------------------------------------------

EventHeader is a tracing convention layered on top of Linux Tracepoints.

To reduce the number of unique Tracepoint names tracked by the kernel, we
use a small number of Tracepoints to manage a larger number of events. All
events with the same attributes (provider name, severity level, category
keyword, etc.) will share one Tracepoint.

- This means we cannot enable/disable events individually. Instead, all events
  with the same attributes will be enabled/disabled as a group.
- This means we cannot rely on the kernel's Tracepoint metadata for event
  identity or event field names/types. Instead, all events contain a common
  header that provides event identity, core event attributes, and support for
  optional event attributes. The kernel's Tracepoint metadata is used only for
  the Tracepoint's name and to determine whether the event follows the
  EventHeader conventions.

We define a naming scheme to be used for the shared Tracepoints:

  TracepointName = ProviderName + '_' + 'L' + EventLevel + 'K' + EventKeyword +
                   [Options]

We define a common event layout to be used by all EventHeader events. The
event has a header, optional header extensions, and then the event data:

  Event = eventheader + [HeaderExtensions] + Data

We define a format to be used for header extensions:

  HeaderExtension = eventheader_extension + ExtensionData

We define a header extension to be used for activity IDs.

We define a header extension to be used for event metadata (event name, field
names, field types).

For use in the event metadata extension, we define a field type system that
supports scalar, string, binary, array, and struct.

Note that we assume that the Tracepoint name corresponding to the event is
available during event decoding. The event decoder obtains the provider name
and keyword for an event by parsing the event's Tracepoint name.

--Provider Names--------------------------------------------------------------

A provider is a component that generates events. Each event from a provider is
associated with a Provider Name that uniquely identifies the provider.

The provider name should be short, yet descriptive enough to minimize the
chance of collision and to help developers track down the component generating
the events. Hierarchical namespaces may be useful for provider names, e.g.
"MyCompany_MyOrg_MyComponent".

Restrictions:

- ProviderName may not contain ' ' or ':' characters.
- strlen(ProviderName + '_' + Attributes) must be less than
  EVENTHEADER_NAME_MAX (256) characters.
- Some event APIs (e.g. tracefs) might impose additional restrictions on
  tracepoint names. For best compatibility, use only ASCII identifier characters
  [A-Za-z0-9_] in provider names.

Event attribute semantics should be consistent within a given provider. While
some event attributes have generally-accepted semantics (e.g. level value 3
is defined below as "warning"), the precise semantics of the attribute values
are defined at the scope of a provider (e.g. different providers will use
different criteria for what constitutes a warning). In addition, some
attributes (tag, keyword) are completely provider-defined. All events with a
particular provider name should use consistent semantics for all attributes
(e.g. keyword bit 0x1 should have a consistent meaning for all events from a
particular provider but will mean something different for other providers).

--Tracepoint Names------------------------------------------------------------

A Tracepoint is registered with the kernel for each unique combination of
ProviderName + Attributes. This allows a larger number of distinct events to
be controlled by a smaller number of kernel Tracepoints while still allowing
events to be enabled/disabled at a reasonable granularity.

The Tracepoint name for an EventHeader event is defined as:

  ProviderName + '_' + 'L' + eventLevel + 'K' + eventKeyword + [Options]
  or printf("%s_L%xK%lx%s", providerName, eventLevel, eventKeyword, options),
  e.g. "MyProvider_L3K2a" or "OtherProvider_L5K0Gperf".

Event level is a uint8 value 1..255 indicating event severity, formatted as
lowercase hexadecimal, e.g. printf("L%x", eventLevel). The defined level values
are: 1 = critical error, 2 = error, 3 = warning, 4 = information, 5 = verbose.

Event keyword is a uint64 bitmask indicating event category membership,
formatted as lowercase hexadecimal, e.g. printf("K%lx", eventKeyword). Each
bit in the keyword corresponds to a provider-defined category, e.g. a provider
might define 0x2 = networking and 0x4 = I/O so that keyword value of 0x2|0x4 =
0x6 would indicate that an event is in both the networking and I/O categories.

Options (optional attributes) can be specified after the keyword attribute.
Each option consists of an uppercase ASCII letter (option type) followed by 0
or more ASCII digits or lowercase ASCII letters (option value). To support
consistent event names, the options must be sorted in alphabetical order, e.g.
"Aoption" should come before "Boption".

The currently defined options are:

- 'G' = provider Group name. Defines a group of providers. This can be used by
  event analysis tools to find all providers that generate a certain kind of
  information.

Restrictions:

- ProviderName may not contain ' ' or ':' characters.
- Tracepoint name must be less than EVENTHEADER_NAME_MAX (256)
  characters in length.
- Some event APIs (e.g. tracefs) might impose additional restrictions on
  tracepoint names. For best compatibility, use only ASCII identifier characters
  [A-Za-z0-9_] in provider names.

--Header-----------------------------------------------------------------------

Because multiple events may share a single Tracepoint, each event must contain
information needed to distinguish it from other events. To enable this, each
event starts with an EventHeader structure which contains information about
the event:

- flags: Bits indicating pointer size (32 or 64 bits), byte order
  (big-endian or little), and whether any header extensions are present.
- opcode: Indicates special event semantics e.g. "normal event",
  "activity start event", "activity end event".
- tag: Provider-defined 16-bit value. Can be used for anything.
- id: 16-bit stable event identifier, or 0 if no identifier is assigned.
- version: 8-bit event version, incremented for e.g. field type changes.
- level: 8-bit event severity level, 1 = critical .. 5 = verbose.
  (level value in event header must match the level in the Tracepoint name.)

If the extension flag is not set, the header is immediately followed by the
event payload.

If the extension flag is set, the header is immediately followed by one or more
header extensions. Each header extension has a 16-bit size, a 15-bit type code,
and a 1-bit flag indicating whether another header extension follows the
current extension. The final header extension is immediately followed by the
event payload.

The following header extensions are defined:

- Activity ID: Contains a 128-bit ID that can be used to correlate events. May
  also contain the 128-bit ID of the parent activity (typically used only for
  the first event of an activity).
- Metadata: Contains the event's metadata: event name, event attributes, field
  names, field attributes, and field types. Both simple (e.g. Int32, HexInt16,
  Float64, Char32, Uuid) and complex (e.g. NulTerminatedString8,
  CountedString16, Binary, Struct, Array) types are supported.
*/
namespace Microsoft.LinuxTracepoints
{
    using System;
    using System.Runtime.InteropServices;
    using Tracing = System.Diagnostics.Tracing;

    /// <summary>
    /// <para>
    /// Core metadata for an EventHeader event.
    /// </para><para>
    /// Each EventHeader event starts with an instance of the EventHeader structure.
    /// It contains core information recorded for every event to help with event
    /// identification, filtering, and decoding.
    /// </para><para>
    /// If EventHeader.Flags has the Extension bit set then the EventHeader is followed
    /// by one or more EventHeaderExtension blocks. Otherwise the EventHeader is
    /// followed by the event payload data.
    /// </para><para>
    /// If an EventHeaderExtension.Kind has the Chain flag set then the
    /// EventHeaderExtension block is followed immediately (no alignment/padding) by
    /// another extension block. Otherwise it is followed immediately (no
    /// alignment/padding) by the event payload data.
    /// </para><para>
    /// If there is a Metadata extension then it contains the event name, field names,
    /// and field types needed to decode the payload data. Otherwise, the payload
    /// decoding system is defined externally, i.e. you will use the provider name to
    /// find the appropriate decoding manifest, then use the event's id+version to
    /// find the decoding information within the manifest, then use that decoding
    /// information to decode the event payload data.
    /// </para><para>
    /// For a particular event definition (i.e. for a particular event name, or for a
    /// particular event id+version), the information in the EventHeader (and in the
    /// Metadata extension, if present) should be constant. For example, instead of
    /// having a single event with a runtime-variable level, you should have a
    /// distinct event definition (with distinct event name and/or distinct event id)
    /// for each level.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EventHeader
    {
        /// <summary>
        /// The size of this structure in bytes (8).
        /// </summary>
        public const int SizeOfStruct = 8;

        /// <summary>
        /// Pointer-size and Endian flags as appropriate for the currently-running process,
        /// no extension blocks present.
        /// </summary>
        public static readonly EventHeaderFlags DefaultFlags =
            (IntPtr.Size == 8 ? EventHeaderFlags.Pointer64 : EventHeaderFlags.None) |
            (BitConverter.IsLittleEndian ? EventHeaderFlags.LittleEndian : EventHeaderFlags.None);

        /// <summary>
        /// The encoding corresponding to IntPtr. If IntPtr.Size == 8, this is Value64.
        /// Otherwise, this is Value32.
        /// </summary>
        public static readonly EventHeaderFieldEncoding IntPtrEncoding =
            IntPtr.Size == 8 ? EventHeaderFieldEncoding.Value64 : EventHeaderFieldEncoding.Value32;

        /// <summary>
        /// Pointer64, LittleEndian, Extension.
        /// </summary>
        public EventHeaderFlags Flags;

        /// <summary>
        /// Increment Version whenever event layout changes.
        /// </summary>
        public byte Version;

        /// <summary>
        /// Stable id for this event, or 0 if none.
        /// </summary>
        public ushort Id;

        /// <summary>
        /// Provider-defined event tag, or 0 if none.
        /// </summary>
        public ushort Tag;

        /// <summary>
        /// EventOpcode raw value. (Stores the value of the Opcode property.)
        /// </summary>
        public byte OpcodeByte;

        /// <summary>
        /// EventLevel raw value. (Stores the value of the Level property.)
        /// </summary>
        public byte LevelByte;

        /// <summary>
        /// EventOpcode: info, start activity, stop activity, etc.
        /// Throws OverflowException if set value > 255.
        /// </summary>
        /// <remarks><para>
        /// Most events set Opcode = Info (0). Other Opcode values add special semantics to
        /// an event that help the event analysis tool with grouping related events. The
        /// most frequently-used special semantics are ActivityStart and ActivityStop.
        /// </para><para>
        /// To record an activity:
        /// </para><list type="bullet"><item>
        /// Generate a new activity id. An activity id is a 128-bit value that must be
        /// unique within the trace. This can be a UUID or it can be generated by any
        /// other id-generation system that is unlikely to create the same value for any
        /// other activity id in the same trace.
        /// </item><item>
        /// Write an event with opcode = ActivityStart and with an ActivityId header
        /// extension. The ActivityId extension should have the newly-generated activity
        /// id, followed by the id of a parent activity (if any). If there is a parent
        /// activity, the extension length will be 32; otherwise it will be 16.
        /// </item><item>
        /// As appropriate, write any number of normal events (events with opcode set to
        /// something other than ActivityStart or ActivityStop, e.g. opcode = Info). To
        /// indicate that the events are part of the activity, each of these events
        /// should have an ActivityId header extension with the new activity id
        /// (extension length will be 16).
        /// </item><item>
        /// When the activity ends, write an event with opcode = ActivityStop and with
        /// an ActivityId header extension containing the activity id of the activity
        /// that is ending (extension length will be 16).
        /// </item></list>
        /// </remarks>
        /// <exception cref="OverflowException">Set value > 255</exception>
        public Tracing.EventOpcode Opcode
        {
            readonly get => (Tracing.EventOpcode)OpcodeByte;
            set => OpcodeByte = checked((byte)value);
        }

        /// <summary>
        /// EventLevel: critical, error, warning, info, verbose.
        /// Throws OverflowException if set value > 255.
        /// </summary>
        /// <exception cref="OverflowException">Set value > 255</exception>
        public Tracing.EventLevel Level
        {
            readonly get => (Tracing.EventLevel)LevelByte;
            set => LevelByte = checked((byte)value);
        }

        // Followed by: EventHeaderExtension block(s), then event payload.
    }
}
