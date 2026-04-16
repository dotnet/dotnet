#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints.Provider;

using System;
using System.Buffers;
using Debug = System.Diagnostics.Debug;
using Encoding = System.Text.Encoding;
using EventOpcode = System.Diagnostics.Tracing.EventOpcode;
using MemoryMarshal = System.Runtime.InteropServices.MemoryMarshal;

/// <summary>
/// Builder for events to be written through an <see cref="EventHeaderDynamicTracepoint"/>.
/// Create a <see cref="EventHeaderDynamicProvider"/> provider and use provider.Register to get
/// a <see cref="EventHeaderDynamicTracepoint"/> tracepoint. Create a
/// <see cref="EventHeaderDynamicBuilder"/> builder (or reuse an existing one to minimize
/// overhead), add data to it, and then call builder.Write(tracepoint) to emit the event.
/// Dispose of the builder when you're done with it to return temporary buffers to the ArrayPool.
/// </summary>
/// <remarks>
/// <para>
/// Total event size (including headers, event name string, field name strings, and field
/// values) is limited to 64KB. If the builder detects that event size will definitely exceed
/// 64KB, the TooBig property will become true and any call to the Write method will return
/// E2BIG (7) without trying to send the event to the kernel. However, the actual size limit
/// depends on the the configuration of the trace collection session. If the event is large
/// but does not definitely exceed 64KB, the Write method will send the event to the kernel,
/// but the kernel may still drop the event if the headers added by the kernel cause the event
/// to exceed 64KB. The kernel may or may not return an error in this case.
/// </para><para>
/// Builder objects are reusable. If generating several events in sequence, you can minimize
/// overhead by using the same builder for multiple events.
/// </para><para>
/// Builder objects are disposable. The builder uses an ArrayPool to minimize overhead.
/// When you are done with a builder, call Dispose() to return the allocations to the pool
/// so that they can be used by the next builder.
/// </para>
/// </remarks>
internal class EventHeaderDynamicBuilder : IDisposable
{
    private const uint SizeOfGuid = 16;
    private const EventHeaderFieldEncoding VArrayFlag = EventHeaderFieldEncoding.VArrayFlag;

    private Vector meta;
    private Vector data;
    private bool addFailed;

    /// <summary>
    /// Initializes a new instance of the EventBuilder.
    /// </summary>
    /// <param name="initialMetadataBufferSize">
    /// The initial capacity of the metadata buffer. This must be a power of 2 in the
    /// range 4 through 65536. Default is 256 bytes.
    /// </param>
    /// <param name="initialDataBufferSize">
    /// The initial capacity of the data buffer. This must be a power of 2 in the
    /// range 4 through 65536. Default is 256 bytes.
    /// </param>
    public EventHeaderDynamicBuilder(int initialMetadataBufferSize = 256, int initialDataBufferSize = 256)
    {
        if (initialMetadataBufferSize < 4 || initialMetadataBufferSize > 65536 ||
            (initialMetadataBufferSize & (initialMetadataBufferSize - 1)) != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialMetadataBufferSize));
        }

        if (initialDataBufferSize < 4 || initialDataBufferSize > 65536 ||
            (initialDataBufferSize & (initialDataBufferSize - 1)) != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialDataBufferSize));
        }

        this.meta = new Vector(initialMetadataBufferSize);
        this.data = new Vector(initialDataBufferSize);

        // Initial state is the same as Reset("").
        this.meta.AddByte(0); // nul-termination for empty event name. (Cannot fail.)
        Debug.Assert(this.meta.Used == 1);
    }

    /// <summary>
    /// Provider-defined event tag, or 0 if none.
    /// Reset sets this to 0. Can also be set by the SetTag method.
    /// </summary>
    public ushort Tag { get; set; }

    /// <summary>
    /// Stable id for this event, or 0 if none.
    /// Reset sets this to 0. Can also be set by the SetIdVersion.
    /// </summary>
    public ushort Id { get; set; }

    /// <summary>
    /// Increment Version whenever event layout changes.
    /// Reset sets this to 0. Can also be set by the SetIdVersion.
    /// </summary>
    public byte Version { get; set; }

    /// <summary>
    /// EventOpcode raw value. (Stores the value of the Opcode property.)
    /// Reset sets this to 0 (Info). Can also be set by SetOpcodeByte.
    /// </summary>
    public byte OpcodeByte { get; set; }

    /// <summary>
    /// Returns true if the event is too large to be written (event name, field names,
    /// and field values total more than 64KB). If this property is true, Write will
    /// return E2BIG. This status is cleared by a call to Reset.
    /// </summary>
    public bool TooBig => this.addFailed || 65520 < this.meta.Used + this.data.Used;

    /// <summary>
    /// EventOpcode: info, start activity, stop activity, etc.
    /// Reset sets this to 0 (Info). Can also be set by SetOpcode.
    /// Throws OverflowException if set to a value greater than 255.
    /// </summary>
    /// <exception cref="OverflowException">value > 255</exception>
    /// <remarks><para>
    /// Most events use Opcode = Info (0). Other Opcode values add special semantics to
    /// an event that help the event analysis tool group related events. The most
    /// frequently-used special semantics are ActivityStart and ActivityStop.
    /// </para><para>
    /// To record an activity:
    /// </para><list type="bullet"><item>
    /// Generate a new activity id. An activity id is a 128-bit value that must be
    /// unique within the trace. This can be a UUID or it can be generated by any
    /// other id-generation system that is unlikely to create the same value for any
    /// other activity id in the same trace.
    /// </item><item>
    /// Write an event with opcode = ActivityStart and with an ActivityId. The
    /// ActivityId should have the newly-generated activity. The event may optionally
    /// contain the ID of a related (parent) activity.
    /// </item><item>
    /// As appropriate, write any number of normal events (events with opcode set to
    /// something other than ActivityStart or ActivityStop, e.g. opcode = Info). To
    /// indicate that the events are part of the activity, each of these events
    /// should have the ActivityId set to the new activity id.
    /// </item><item>
    /// When the activity ends, write an event with opcode = ActivityStop and with
    /// the ActivityId set to activity id of the activity that is ending.
    /// </item></list>
    /// </remarks>
    public EventOpcode Opcode
    {
        get => (EventOpcode)this.OpcodeByte;
        set => this.OpcodeByte = checked((byte)value);
    }

    /// <summary>
    /// Clears the previous event (if any) from the builder and starts building a new event.
    /// Sets Tag, Id, Version, Opcode to 0.
    /// </summary>
    /// <param name="name">
    /// The event name for the new event. Must not contain any '\0' chars.
    /// </param>
    public EventHeaderDynamicBuilder Reset(ReadOnlySpan<char> name)
    {
        Debug.Assert(name.IndexOf('\0') < 0, "Event name must not have embedded NUL characters.");
        this.ResetImpl();

        var utf8 = Encoding.UTF8;
        var nameBuffer = this.meta.ReserveSpanFor((uint)utf8.GetMaxByteCount(name.Length) + 1);
        if (nameBuffer.IsEmpty)
        {
            this.addFailed = true;
            this.meta.AddByte(0); // nul-termination for empty event name. (Cannot fail.)
            Debug.Assert(this.meta.Used == 1);
        }
        else
        {
            int nameUsed = 0;
            try
            {
                nameUsed = utf8.GetBytes(name, nameBuffer);
            }
            finally
            {
                nameBuffer[nameUsed] = 0;
                this.meta.SetUsed(nameUsed + 1);
            }
        }

        return this;
    }

    /// <summary>
    /// Clears the previous event (if any) from the builder and starts building a new event.
    /// Sets Tag, Id, Version, Opcode to 0.
    /// </summary>
    /// <param name="nameUtf8">
    /// The UTF-8 event name for the new event. Must not contain any '\0' bytes.
    /// </param>
    public EventHeaderDynamicBuilder Reset(ReadOnlySpan<byte> nameUtf8)
    {
        Debug.Assert(nameUtf8.IndexOf((byte)0) < 0, "Event name must not have embedded NUL characters.");
        this.ResetImpl();

        var nameBuffer = this.meta.ReserveSpanFor((uint)nameUtf8.Length + 1);
        if (nameBuffer.IsEmpty)
        {
            this.addFailed = true;
            this.meta.AddByte(0); // nul-termination for empty event name. (Cannot fail.)
            Debug.Assert(this.meta.Used == 1);
        }
        else
        {
            nameBuffer[nameUtf8.Length] = 0;
            nameUtf8.CopyTo(nameBuffer);
        }

        return this;
    }

    /// <summary>
    /// Sets the provider-defined event tag. Most events have tag 0 (default).
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder SetTag(ushort tag)
    {
        this.Tag = tag;
        return this;
    }

    /// <summary>
    /// Sets the event's stable id and the event's version.
    /// Since events are frequently identified by name, many events use 0,
    /// indicating that they do not have any assigned stable id.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder SetIdVersion(ushort id, byte version)
    {
        this.Id = id;
        this.Version = version;
        return this;
    }

    /// <summary>
    /// EventOpcode: info, start activity, stop activity, etc.
    /// </summary>
    /// <returns>this</returns>
    /// <remarks><para>
    /// Most events use Opcode = Info (0). Other Opcode values add special semantics to
    /// an event that help the event analysis tool group related events. The most
    /// frequently-used special semantics are ActivityStart and ActivityStop.
    /// </para><para>
    /// To record an activity:
    /// </para><list type="bullet"><item>
    /// Generate a new activity id. An activity id is a 128-bit value that must be
    /// unique within the trace. This can be a UUID or it can be generated by any
    /// other id-generation system that is unlikely to create the same value for any
    /// other activity id in the same trace.
    /// </item><item>
    /// Write an event with opcode = ActivityStart and with an ActivityId. The
    /// ActivityId should have the newly-generated activity. The event may optionally
    /// contain the ID of a related (parent) activity.
    /// </item><item>
    /// As appropriate, write any number of normal events (events with opcode set to
    /// something other than ActivityStart or ActivityStop, e.g. opcode = Info). To
    /// indicate that the events are part of the activity, each of these events
    /// should have the ActivityId set to the new activity id.
    /// </item><item>
    /// When the activity ends, write an event with opcode = ActivityStop and with
    /// the ActivityId set to activity id of the activity that is ending.
    /// </item></list>
    /// </remarks>
    public EventHeaderDynamicBuilder SetOpcodeByte(byte opcode)
    {
        this.OpcodeByte = opcode;
        return this;
    }

    /// <summary>
    /// EventOpcode: info, start activity, stop activity, etc.
    /// Throws OverflowException if value > 255.
    /// </summary>
    /// <returns>this</returns>
    /// <exception cref="OverflowException">value > 255</exception>
    /// <remarks><para>
    /// Most events use Opcode = Info (0). Other Opcode values add special semantics to
    /// an event that help the event analysis tool group related events. The most
    /// frequently-used special semantics are ActivityStart and ActivityStop.
    /// </para><para>
    /// To record an activity:
    /// </para><list type="bullet"><item>
    /// Generate a new activity id. An activity id is a 128-bit value that must be
    /// unique within the trace. This can be a UUID or it can be generated by any
    /// other id-generation system that is unlikely to create the same value for any
    /// other activity id in the same trace.
    /// </item><item>
    /// Write an event with opcode = ActivityStart and with an ActivityId. The
    /// ActivityId should have the newly-generated activity. The event may optionally
    /// contain the ID of a related (parent) activity.
    /// </item><item>
    /// As appropriate, write any number of normal events (events with opcode set to
    /// something other than ActivityStart or ActivityStop, e.g. opcode = Info). To
    /// indicate that the events are part of the activity, each of these events
    /// should have the ActivityId set to the new activity id.
    /// </item><item>
    /// When the activity ends, write an event with opcode = ActivityStop and with
    /// the ActivityId set to activity id of the activity that is ending.
    /// </item></list>
    /// </remarks>
    public EventHeaderDynamicBuilder SetOpcode(EventOpcode opcode)
    {
        this.Opcode = opcode;
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value8"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, String8.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt8(
        ReadOnlySpan<char> name,
        Byte value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value8, format, tag);
        this.addFailed |= !this.data.AddByte(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value8"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, String8.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt8(
        ReadOnlySpan<byte> nameUtf8,
        Byte value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value8, format, tag);
        this.addFailed |= !this.data.AddByte(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value8"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, String8.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt8Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Byte> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value8 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value8"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, String8.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt8Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Byte> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value8 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value8"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, String8.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt8(
        ReadOnlySpan<char> name,
        SByte value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value8, format, tag);
        this.addFailed |= !this.data.AddByte(unchecked((Byte)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value8"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, String8.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt8(
        ReadOnlySpan<byte> nameUtf8,
        SByte value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value8, format, tag);
        this.addFailed |= !this.data.AddByte(unchecked((Byte)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value8"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, String8.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt8Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<SByte> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value8 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value8"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, String8.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt8Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<SByte> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value8 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt16(
        ReadOnlySpan<char> name,
        UInt16 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value16, format, tag);
        this.AddDataU16(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt16(
        ReadOnlySpan<byte> nameUtf8,
        UInt16 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value16, format, tag);
        this.AddDataU16(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<UInt16> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value16 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<UInt16> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value16 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt16(
        ReadOnlySpan<char> name,
        Int16 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value16, format, tag);
        this.AddDataU16(unchecked((UInt16)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt16(
        ReadOnlySpan<byte> nameUtf8,
        Int16 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value16, format, tag);
        this.AddDataU16(unchecked((UInt16)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Int16> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value16 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Int16> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value16 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.StringUtf"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddChar16(
        ReadOnlySpan<char> name,
        Char value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.StringUtf,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value16, format, tag);
        this.AddDataU16(unchecked((UInt16)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.StringUtf"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddChar16(
        ReadOnlySpan<byte> nameUtf8,
        Char value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.StringUtf,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value16, format, tag);
        this.AddDataU16(unchecked((UInt16)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> array to the event.
    /// Note that this adds an array of char (i.e. ['A', 'B', 'C']), not a String.
    /// Default format is <see cref="EventHeaderFieldFormat.StringUtf"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddChar16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Char> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.StringUtf,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value16 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value16"/> array to the event.
    /// Note that this adds an array of char (i.e. ['A', 'B', 'C']), not a String.
    /// Default format is <see cref="EventHeaderFieldFormat.StringUtf"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Boolean, HexBytes, StringUtf,
    /// Port.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddChar16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Char> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.StringUtf,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value16 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Errno, Pid, Time, Boolean,
    /// Float, HexBytes, StringUtf, IPv4.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt32(
        ReadOnlySpan<char> name,
        UInt32 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value32, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Errno, Pid, Time, Boolean,
    /// Float, HexBytes, StringUtf, IPv4.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt32(
        ReadOnlySpan<byte> nameUtf8,
        UInt32 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value32, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Errno, Pid, Time, Boolean,
    /// Float, HexBytes, StringUtf, IPv4.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt32Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<UInt32> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value32 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Errno, Pid, Time, Boolean,
    /// Float, HexBytes, StringUtf, IPv4.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt32Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<UInt32> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value32 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Errno, Pid, Time, Boolean,
    /// Float, HexBytes, StringUtf, IPv4.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt32(
        ReadOnlySpan<char> name,
        Int32 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value32, format, tag);
        this.AddDataT(unchecked((UInt32)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Errno, Pid, Time, Boolean,
    /// Float, HexBytes, StringUtf, IPv4.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt32(
        ReadOnlySpan<byte> nameUtf8,
        Int32 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value32, format, tag);
        this.AddDataT(unchecked((UInt32)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Errno, Pid, Time, Boolean,
    /// Float, HexBytes, StringUtf, IPv4.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt32Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Int32> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value32 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Errno, Pid, Time, Boolean,
    /// Float, HexBytes, StringUtf, IPv4.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt32Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Int32> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value32 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt64(
        ReadOnlySpan<char> name,
        UInt64 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value64, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt64(
        ReadOnlySpan<byte> nameUtf8,
        UInt64 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value64, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt64Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<UInt64> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value64 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUInt64Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<UInt64> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value64 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt64(
        ReadOnlySpan<char> name,
        Int64 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value64, format, tag);
        this.AddDataT(unchecked((UInt64)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt64(
        ReadOnlySpan<byte> nameUtf8,
        Int64 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value64, format, tag);
        this.AddDataT(unchecked((UInt64)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt64Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Int64> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value64 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddInt64Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Int64> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value64 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeader.IntPtrEncoding"/> field to the event (either
    /// <see cref="EventHeaderFieldEncoding.Value32"/> or <see cref="EventHeaderFieldEncoding.Value64"/>.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUIntPtr(
        ReadOnlySpan<char> name,
        nuint value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeader.IntPtrEncoding, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeader.IntPtrEncoding"/> field to the event (either
    /// <see cref="EventHeaderFieldEncoding.Value32"/> or <see cref="EventHeaderFieldEncoding.Value64"/>.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUIntPtr(
        ReadOnlySpan<byte> nameUtf8,
        nuint value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeader.IntPtrEncoding, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeader.IntPtrEncoding"/> array to the event (either
    /// <see cref="EventHeaderFieldEncoding.Value32"/> or <see cref="EventHeaderFieldEncoding.Value64"/>.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUIntPtrArray(
        ReadOnlySpan<char> name,
        ReadOnlySpan<nuint> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeader.IntPtrEncoding | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeader.IntPtrEncoding"/> array to the event (either
    /// <see cref="EventHeaderFieldEncoding.Value32"/> or <see cref="EventHeaderFieldEncoding.Value64"/>.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as UnsignedInt).
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddUIntPtrArray(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<nuint> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeader.IntPtrEncoding | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeader.IntPtrEncoding"/> field to the event (either
    /// <see cref="EventHeaderFieldEncoding.Value32"/> or <see cref="EventHeaderFieldEncoding.Value64"/>.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddIntPtr(
        ReadOnlySpan<char> name,
        nint value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeader.IntPtrEncoding, format, tag);
        this.AddDataT(unchecked((nuint)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeader.IntPtrEncoding"/> field to the event (either
    /// <see cref="EventHeaderFieldEncoding.Value32"/> or <see cref="EventHeaderFieldEncoding.Value64"/>.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddIntPtr(
        ReadOnlySpan<byte> nameUtf8,
        nint value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeader.IntPtrEncoding, format, tag);
        this.AddDataT(unchecked((nuint)value));
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeader.IntPtrEncoding"/> array to the event (either
    /// <see cref="EventHeaderFieldEncoding.Value32"/> or <see cref="EventHeaderFieldEncoding.Value64"/>.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddIntPtrArray(
        ReadOnlySpan<char> name,
        ReadOnlySpan<nint> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeader.IntPtrEncoding | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeader.IntPtrEncoding"/> array to the event (either
    /// <see cref="EventHeaderFieldEncoding.Value32"/> or <see cref="EventHeaderFieldEncoding.Value64"/>.
    /// Default format is <see cref="EventHeaderFieldFormat.SignedInt"/>.
    /// Applicable formats include: UnsignedInt, SignedInt, HexInt, Time, Float, HexBytes.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddIntPtrArray(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<nint> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.SignedInt,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeader.IntPtrEncoding | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Float"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddFloat32(
        ReadOnlySpan<char> name,
        Single value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Float,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value32, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Float"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddFloat32(
        ReadOnlySpan<byte> nameUtf8,
        Single value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Float,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value32, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Float"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddFloat32Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Single> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Float,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value32 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value32"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Float"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddFloat32Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Single> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Float,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value32 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Float"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddFloat64(
        ReadOnlySpan<char> name,
        Double value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Float,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value64, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Float"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddFloat64(
        ReadOnlySpan<byte> nameUtf8,
        Double value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Float,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value64, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Float"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddFloat64Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Double> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Float,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value64 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value64"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Float"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddFloat64Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Double> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Float,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value64 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value128"/> field to the event,
    /// fixing byte order as appropriate for GUID--UUID conversion.
    /// Default format is <see cref="EventHeaderFieldFormat.Uuid"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddGuid(
        ReadOnlySpan<char> name,
        in Guid value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Uuid,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value128, format, tag);
        this.AddDataGuid(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value128"/> field to the event,
    /// fixing byte order as appropriate for GUID--UUID conversion.
    /// Default format is <see cref="EventHeaderFieldFormat.Uuid"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddGuid(
        ReadOnlySpan<byte> nameUtf8,
        in Guid value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Uuid,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value128, format, tag);
        this.AddDataGuid(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value128"/> array to the event,
    /// fixing byte order as appropriate for GUID--UUID conversion.
    /// Default format is <see cref="EventHeaderFieldFormat.Uuid"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddGuidArray(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Guid> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Uuid,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value128 | VArrayFlag, format, tag);
        this.AddDataArrayGuid(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value128"/> array to the event,
    /// fixing byte order as appropriate for GUID--UUID conversion.
    /// Default format is <see cref="EventHeaderFieldFormat.Uuid"/>.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddGuidArray(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Guid> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Uuid,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value128 | VArrayFlag, format, tag);
        this.AddDataArrayGuid(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value128"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as HexBytes).
    /// Applicable formats include: HexBytes, Uuid, IPv6.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddValue128(
        ReadOnlySpan<char> name,
        EventHeaderValue128 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value128, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value128"/> field to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as HexBytes).
    /// Applicable formats include: HexBytes, Uuid, IPv6.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddValue128(
        ReadOnlySpan<byte> nameUtf8,
        EventHeaderValue128 value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value128, format, tag);
        this.AddDataT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value128"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as HexBytes).
    /// Applicable formats include: HexBytes, Uuid, IPv6.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddValue128Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<EventHeaderValue128> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.Value128 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.Value128"/> array to the event.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as HexBytes).
    /// Applicable formats include: HexBytes, Uuid, IPv6.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddValue128Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<EventHeaderValue128> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Value128 | VArrayFlag, format, tag);
        this.AddDataArrayT(values);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar8"/> field to the event (zero-terminated
    /// sequence of 8-bit values). You should prefer AddString8 over this method in most scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString8(
        ReadOnlySpan<char> name,
        ReadOnlySpan<byte> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar8, format, tag);
        this.AddDataZStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar8"/> field to the event (zero-terminated
    /// sequence of 8-bit values). You should prefer AddString8 over this method in most scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString8(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<byte> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar8, format, tag);
        this.AddDataZStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar8"/> array to the event (array of
    /// zero-terminated 8-bit strings). You should prefer AddString8Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString8Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<byte>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar8 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar8"/> array to the event (array of
    /// zero-terminated 8-bit strings). You should prefer AddString8Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString8Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<byte>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar8 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> field to the event (zero-terminated
    /// sequence of 16-bit values). You should prefer AddString16 over this method in most scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16(
        ReadOnlySpan<char> name,
        ReadOnlySpan<UInt16> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar16, format, tag);
        this.AddDataZStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> field to the event (zero-terminated
    /// sequence of 16-bit values). You should prefer AddString16 over this method in most scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<UInt16> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar16, format, tag);
        this.AddDataZStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> array to the event (array of
    /// zero-terminated 16-bit strings). You should prefer AddString16Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<UInt16>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> array to the event (array of
    /// zero-terminated 16-bit strings). You should prefer AddString16Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<UInt16>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> field to the event (zero-terminated
    /// sequence of 16-bit values). You should prefer AddString16 over this method in most scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Char> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar16, format, tag);
        this.AddDataZStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> field to the event (zero-terminated
    /// sequence of 16-bit values). You should prefer AddString16 over this method in most scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Char> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar16, format, tag);
        this.AddDataZStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> array to the event (array of
    /// zero-terminated 16-bit strings). You should prefer AddString16Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<Char>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> array to the event (array of
    /// zero-terminated 16-bit strings). You should prefer AddString16Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<Char>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> array to the event (array of
    /// zero-terminated 16-bit strings). You should prefer AddString16Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<String> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.AsSpan());
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar16"/> array to the event (array of
    /// zero-terminated 16-bit strings). You should prefer AddString16Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<String> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.AsSpan());
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar32"/> field to the event (zero-terminated
    /// sequence of 32-bit values). You should prefer AddString32 over this method in most scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString32(
        ReadOnlySpan<char> name,
        ReadOnlySpan<UInt32> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar32, format, tag);
        this.AddDataZStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar32"/> field to the event (zero-terminated
    /// sequence of 32-bit values). You should prefer AddString32 over this method in most scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString32(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<UInt32> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar32, format, tag);
        this.AddDataZStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar32"/> array to the event (array of
    /// zero-terminated 32-bit strings). You should prefer AddString32Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString32Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<UInt32>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.ZStringChar32 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.ZStringChar32"/> array to the event (array of
    /// zero-terminated 32-bit strings). You should prefer AddString32Array over this method in most
    /// scenarios.
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddZString32Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<UInt32>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.ZStringChar32 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataZStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char8"/> field to the event
    /// (counted sequence of 8-bit values, e.g. a UTF-8 string or a binary blob).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString8(
        ReadOnlySpan<char> name,
        ReadOnlySpan<byte> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char8, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char8"/> field to the event
    /// (counted sequence of 8-bit values, e.g. a UTF-8 string or a binary blob).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString8(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<byte> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char8, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char8"/> array to the event
    /// (e.g. array of binary blobs, array of UTF-8 strings, array of Latin1 strings, etc.).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString8Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<byte>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char8 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char8"/> array to the event
    /// (e.g. array of binary blobs, array of UTF-8 strings, array of Latin1 strings, etc.).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString8Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<byte>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char8 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> field to the event
    /// (counted sequence of 16-bit values, e.g. a UTF-16 string).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16(
        ReadOnlySpan<char> name,
        ReadOnlySpan<UInt16> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char16, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> field to the event
    /// (counted sequence of 16-bit values, e.g. a UTF-16 string).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<UInt16> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char16, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> array to the event
    /// (e.g. array of UTF-16 strings).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<UInt16>> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)value.Length);
        foreach (var v in value)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> array to the event
    /// (e.g. array of UTF-16 strings).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<UInt16>> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)value.Length);
        foreach (var v in value)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> field to the event
    /// (counted sequence of 16-bit values, e.g. a UTF-16 string).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16(
        ReadOnlySpan<char> name,
        ReadOnlySpan<Char> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char16, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> field to the event
    /// (counted sequence of 16-bit values, e.g. a UTF-16 string).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<Char> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char16, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> array to the event
    /// (array of strings).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<Char>> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)value.Length);
        foreach (var v in value)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> array to the event
    /// (array of strings).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<Char>> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)value.Length);
        foreach (var v in value)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> array to the event
    /// (array of strings).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<String> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)value.Length);
        foreach (var v in value)
        {
            this.AddDataStringT(v.AsSpan());
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char16"/> array to the event
    /// (array of strings).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString16Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<String> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char16 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)value.Length);
        foreach (var v in value)
        {
            this.AddDataStringT(v.AsSpan());
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char32"/> field to the event
    /// (counted sequence of 32-bit values, e.g. a UTF-32 string).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString32(
        ReadOnlySpan<char> name,
        ReadOnlySpan<UInt32> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char32, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char32"/> field to the event
    /// (counted sequence of 32-bit values, e.g. a UTF-32 string).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString32(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<UInt32> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char32, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char32"/> array to the event
    /// (e.g. array of UTF-32 strings).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString32Array(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<UInt32>> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.StringLength16Char32 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)value.Length);
        foreach (var v in value)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.StringLength16Char32"/> array to the event
    /// (e.g. array of UTF-32 strings).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as StringUtf).
    /// Applicable formats include: StringUtf, HexBytes, StringUtfBom, StringXml, StringJson.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddString32Array(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<UInt32>> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.StringLength16Char32 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)value.Length);
        foreach (var v in value)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.BinaryLength16Char8"/> field to the event
    /// (counted sequence of 8-bit values, e.g. a binary blob).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as HexBytes).
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddBinary(
        ReadOnlySpan<char> name,
        ReadOnlySpan<byte> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.BinaryLength16Char8, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.BinaryLength16Char8"/> field to the event
    /// (counted sequence of 8-bit values, e.g. a binary blob).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as HexBytes).
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddBinary(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<byte> value,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.BinaryLength16Char8, format, tag);
        this.AddDataStringT(value);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.BinaryLength16Char8"/> array to the event
    /// (e.g. array of binary blobs).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as HexBytes).
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddBinaryArray(
        ReadOnlySpan<char> name,
        ReadOnlySpan<ReadOnlyMemory<byte>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(name, EventHeaderFieldEncoding.BinaryLength16Char8 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EventHeaderFieldEncoding.BinaryLength16Char8"/> array to the event
    /// (e.g. array of binary blobs, array of UTF-8 strings, array of Latin1 strings, etc.).
    /// Default format is <see cref="EventHeaderFieldFormat.Default"/> (formats as HexBytes).
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddBinaryArray(
        ReadOnlySpan<byte> nameUtf8,
        ReadOnlySpan<ReadOnlyMemory<byte>> values,
        EventHeaderFieldFormat format = EventHeaderFieldFormat.Default,
        ushort tag = 0)
    {
        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.BinaryLength16Char8 | VArrayFlag, format, tag);
        this.AddDataU16((UInt16)values.Length);
        foreach (var v in values)
        {
            this.AddDataStringT(v.Span);
        }
        return this;
    }

    /// <summary>
    /// Adds a new logical field with the specified name and indicates that the next
    /// fieldCount logical fields should be considered as members of this field.
    /// Note that fieldCount must be in the range 1 to 127 (must NOT be 0).
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddStruct(
        ReadOnlySpan<char> name,
        byte fieldCount,
        ushort tag = 0)
    {
        if (fieldCount < 1 || fieldCount > 127)
        {
            throw new ArgumentOutOfRangeException(nameof(fieldCount));
        }

        this.AddMeta(name, EventHeaderFieldEncoding.Struct, (EventHeaderFieldFormat)fieldCount, tag);
        return this;
    }

    /// <summary>
    /// Adds a new logical field with the specified name and indicates that the next
    /// fieldCount logical fields should be considered as members of this field.
    /// Note that fieldCount must be in the range 1 to 127 (must NOT be 0).
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddStruct(
        ReadOnlySpan<byte> nameUtf8,
        byte fieldCount,
        ushort tag = 0)
    {
        if (fieldCount < 1 || fieldCount > 127)
        {
            throw new ArgumentOutOfRangeException(nameof(fieldCount));
        }

        this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Struct, (EventHeaderFieldFormat)fieldCount, tag);
        return this;
    }

    /// <summary>
    /// Advanced: For use when field count is not yet known.
    /// Adds a new logical field with the specified name and indicates that the next
    /// 127 logical fields should be considered as members of this field (placeholder
    /// value). Returns the position of the field count so that the placeholder value
    /// can subsequently updated by a call to SetStructFieldCount.
    /// </summary>
    /// <param name="name">The name of the field.</param>
    /// <param name="tag">User-defined field tag.</param>
    /// <param name="metadataPosition">
    /// Receives the offset of the field count within the metadata.
    /// You can use this value with SetStructFieldCount.
    /// </param>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddStructWithMetadataPosition(
        ReadOnlySpan<char> name,
        out int metadataPosition,
        ushort tag = 0)
    {
        metadataPosition = this.AddMeta(name, EventHeaderFieldEncoding.Struct, (EventHeaderFieldFormat)127, tag);
        return this;
    }

    /// <summary>
    /// Advanced: For use when field count is not yet known.
    /// Adds a new logical field with the specified name and indicates that the next
    /// 127 logical fields should be considered as members of this field (placeholder
    /// value). Returns the position of the field count so that the placeholder value
    /// can subsequently updated by a call to SetStructFieldCount.
    /// </summary>
    /// <param name="nameUtf8">The name of the field.</param>
    /// <param name="tag">User-defined field tag.</param>
    /// <param name="metadataPosition">
    /// Receives the offset of the field count within the metadata.
    /// You can use this value with SetStructFieldCount.
    /// </param>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddStructWithMetadataPosition(
        ReadOnlySpan<byte> nameUtf8,
        out int metadataPosition,
        ushort tag = 0)
    {
        metadataPosition = this.AddMeta(nameUtf8, EventHeaderFieldEncoding.Struct, (EventHeaderFieldFormat)127, tag);
        return this;
    }

    /// <summary>
    /// Advanced: Resets the number of logical fields in the specified structure.
    /// </summary>
    /// <param name="metadataPosition">
    /// The position of the metadata field within the structure. This value is
    /// returned by the AddStructWithMetadataPosition method.
    /// </param>
    /// <param name="fieldCount">
    /// The actual number of fields in the structure. This value must be in the range
    /// 1 to 127.
    /// </param>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder SetStructFieldCount(int metadataPosition, byte fieldCount)
    {
        if (fieldCount < 1 || fieldCount > 127)
        {
            throw new ArgumentOutOfRangeException(nameof(fieldCount));
        }

        var bytes = this.meta.Bytes;
        bytes[metadataPosition] = (byte)((bytes[metadataPosition] & 0x80) | (fieldCount & 0x7F));
        return this;
    }

    /// <summary>
    /// Advanced: Extracts the raw data for the fields currently in the builder.
    /// This can be used to add the same fields to multiple builders with
    /// AddRawFields, i.e. you use one builder to add a set of common fields,
    /// then you use GetRawFields() to get the raw data for the common fields and
    /// save it, then each time you need to add the common fields to an event you
    /// call AddRawFields with the saved data.
    /// <br/>
    /// If TooBig is true, this throws InvalidOperationException.
    /// </summary>
    /// <returns>
    /// Raw field bytes (rawMeta, rawData) that can be used with AddRawFields.
    /// </returns>
    /// <exception cref="InvalidOperationException">TooBig is true</exception>
    public ValueTuple<byte[], byte[]> GetRawFields()
    {
        if (this.TooBig)
        {
            throw new InvalidOperationException(
                "Cannot call GetRawFields when builder." + nameof(TooBig) + " is true.");
        }

        var metaUsed = this.meta.UsedSpan;

        // Skip event name and NUL.
        var metaFieldsPos = metaUsed.IndexOf((byte)0) + 1;
        if (metaFieldsPos <= 0)
        {
            // I think this is only reachable if the object has been disposed.
            Debug.Assert(this.meta.Used == 0);
            Debug.Assert(this.data.Used == 0);
            metaFieldsPos = metaUsed.Length;
        }

        return (metaUsed.Slice(metaFieldsPos).ToArray(), this.data.UsedSpan.ToArray());
    }

    /// <summary>
    /// Advanced: Appends raw data and metadata to the builder. This can be used with
    /// the data from GetRawFields.
    /// </summary>
    /// <param name="rawFields">Data from GetRawFields</param>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddRawFields(ValueTuple<byte[], byte[]> rawFields)
    {
        return this.AddRawFields(rawFields.Item1, rawFields.Item2);
    }

    /// <summary>
    /// Advanced: Appends raw data and metadata to the builder. This can be used with
    /// the data from GetRawFields.
    /// </summary>
    /// <returns>this</returns>
    public EventHeaderDynamicBuilder AddRawFields(ReadOnlySpan<byte> rawMeta, ReadOnlySpan<byte> rawData)
    {
        Span<byte> dest;

        dest = this.meta.ReserveSpanFor((uint)rawMeta.Length);
        if (dest.Length < rawMeta.Length)
        {
            this.addFailed = true;
        }
        else
        {
            rawMeta.CopyTo(dest);
        }

        dest = this.data.ReserveSpanFor((uint)rawData.Length);
        if (dest.Length < rawData.Length)
        {
            this.addFailed = true;
        }
        else
        {
            rawData.CopyTo(dest);
        }

        return this;
    }

    /// <summary>
    /// If builder.TooBig (event exceeds 64KB), immediately returns E2BIG.
    /// <br/>
    /// If !tracepoint.IsEnabled, immediately returns EBADF.
    /// <br/>
    /// Otherwise, writes this builder's event to the specified tracepoint and returns
    /// the errno from writev.
    /// </summary>
    /// <param name="tracepoint">
    /// The tracepoint (provider name, level, and keyword) to which the event should
    /// be written.
    /// </param>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write(EventHeaderDynamicTracepoint tracepoint)
    {
        unsafe
        {
            return tracepoint.WriteRaw(this, null, null);
        }
    }

    /// <summary>
    /// If builder.TooBig (event exceeds 64KB), immediately returns E2BIG.
    /// <br/>
    /// If !tracepoint.IsEnabled, immediately returns EBADF.
    /// <br/>
    /// Otherwise, writes this builder's event to the specified tracepoint and returns
    /// the errno from writev.
    /// </summary>
    /// <param name="tracepoint">
    /// The tracepoint (provider name, level, and keyword) to which the event should
    /// be written.
    /// </param>
    /// <param name="activityId">
    /// ID of the event's activity.
    /// </param>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write(EventHeaderDynamicTracepoint tracepoint, in Guid activityId)
    {
        unsafe
        {
            fixed (Guid* activityIdPtr = &activityId)
            {
                return tracepoint.WriteRaw(this, activityIdPtr, null);
            }
        }
    }

    /// <summary>
    /// If builder.TooBig (event exceeds 64KB), immediately returns E2BIG.
    /// <br/>
    /// If !tracepoint.IsEnabled, immediately returns EBADF.
    /// <br/>
    /// Otherwise, writes this builder's event to the specified tracepoint and returns
    /// the errno from writev.
    /// </summary>
    /// <param name="tracepoint">
    /// The tracepoint (provider name, level, and keyword) to which the event should
    /// be written.
    /// </param>
    /// <param name="activityId">
    /// ID of the event's activity.
    /// </param>
    /// <param name="relatedActivityId">
    /// ID of the activity's parent. Usually used only when Opcode = Start,
    /// i.e. when starting a new activity.
    /// </param>
    /// <returns>
    /// 0 if event was written, errno otherwise.
    /// The return value is for debugging/diagnostic purposes and is usually ignored in normal operation
    /// since most programs should continue to function even when tracing is not configured.
    /// </returns>
    public int Write(EventHeaderDynamicTracepoint tracepoint, in Guid activityId, in Guid relatedActivityId)
    {
        unsafe
        {
            fixed (Guid* activityIdPtr = &activityId, relatedActivityIdPtr = &relatedActivityId)
            {
                return tracepoint.WriteRaw(this, activityIdPtr, relatedActivityIdPtr);
            }
        }
    }

    /// <summary>
    /// Releases resources used by this builder (returns memory to the array pool).
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// If disposing, returns allocations to the array pool.
    /// </summary>
    /// <param name="disposing">true if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.meta.Dispose();
            this.data.Dispose();
        }
    }

    internal ReadOnlySpan<byte> GetRawMeta() => this.meta.UsedSpan;

    internal ReadOnlySpan<byte> GetRawData() => this.data.UsedSpan;

    private void ResetImpl()
    {
        this.meta.Reset();
        this.data.Reset();
        this.addFailed = false;
        this.Tag = 0;
        this.Id = 0;
        this.Version = 0;
        this.OpcodeByte = 0;
    }

    private void AddDataU16(UInt16 value)
    {
        var dest = this.data.ReserveSpanFor(sizeof(UInt16));
        if (dest.IsEmpty)
        {
            this.addFailed = true;
        }
        else
        {
            MemoryMarshal.Write(dest, ref value);
        }
    }

    private void AddDataGuid(in Guid value)
    {
        var dest = this.data.ReserveSpanFor(SizeOfGuid);
        if (dest.IsEmpty)
        {
            this.addFailed = true;
        }
        else
        {
            Utility.WriteGuidBigEndian(dest, value);
        }
    }

    private void AddDataT<T>(T value)
        where T : unmanaged
    {
        uint sizeofT;
        unsafe
        {
            sizeofT = (uint)sizeof(T);
        }

        var dest = this.data.ReserveSpanFor(sizeofT);
        if (dest.IsEmpty)
        {
            this.addFailed = true;
        }
        else
        {
            MemoryMarshal.Write(dest, ref value);
        }
    }

    private void AddDataStringT<T>(ReadOnlySpan<T> value)
        where T : unmanaged
    {
        if (value.Length > UInt16.MaxValue)
        {
            value = value.Slice(0, UInt16.MaxValue);
        }

        var valueBytes = MemoryMarshal.AsBytes(value);
        var dest = this.data.ReserveSpanFor(sizeof(UInt16) + (uint)valueBytes.Length);
        if (dest.IsEmpty)
        {
            this.addFailed = true;
        }
        else
        {
            UInt16 lenU16 = (UInt16)value.Length;
            MemoryMarshal.Write(dest, ref lenU16);
            valueBytes.CopyTo(dest.Slice(sizeof(UInt16)));
        }
    }

    private void AddDataZStringT<T>(ReadOnlySpan<T> value)
        where T : unmanaged, IEquatable<T>
    {
        uint sizeofT;
        unsafe
        {
            sizeofT = (uint)sizeof(T);
        }

        if (value.Length > UInt16.MaxValue)
        {
            value = value.Slice(0, UInt16.MaxValue);
        }

        var len = 0;
        while (len < value.Length)
        {
            if (value[len].Equals(default))
            {
                value = value.Slice(0, len);
                break;
            }

            len += 1;
        }

        var valueBytes = MemoryMarshal.AsBytes(value);
        var dest = this.data.ReserveSpanFor((uint)valueBytes.Length + sizeofT);
        if (dest.IsEmpty)
        {
            this.addFailed = true;
        }
        else
        {
            valueBytes.CopyTo(dest);
            var zero = default(T);
            MemoryMarshal.Write(dest.Slice(valueBytes.Length), ref zero);
        }
    }

    private void AddDataArrayT<T>(ReadOnlySpan<T> values)
        where T : unmanaged
    {
        var valuesBytes = MemoryMarshal.AsBytes(values);
        var dest = this.data.ReserveSpanFor(sizeof(UInt16) + (uint)valuesBytes.Length);
        if (dest.IsEmpty)
        {
            this.addFailed = true;
        }
        else
        {
            var countU16 = (UInt16)values.Length;
            MemoryMarshal.Write(dest, ref countU16);
            valuesBytes.CopyTo(dest.Slice(sizeof(UInt16)));
        }
    }

    private void AddDataArrayGuid(ReadOnlySpan<Guid> values)
    {
        var dest = this.data.ReserveSpanFor(sizeof(UInt16) + (uint)values.Length * SizeOfGuid);
        if (dest.IsEmpty)
        {
            this.addFailed = true;
        }
        else
        {
            var countU16 = (UInt16)values.Length;
            MemoryMarshal.Write(dest, ref countU16);
            var pos = sizeof(UInt16);
            foreach (var v in values)
            {
                Utility.WriteGuidBigEndian(dest.Slice(pos), v);
                pos += (int)SizeOfGuid;
            }
        }
    }

    /// <returns>
    /// The position of the format byte within the metadata array (for AddStructWithMetadataPosition).
    /// </returns>
    private int AddMeta(
        ReadOnlySpan<char> name,
        EventHeaderFieldEncoding encoding,
        EventHeaderFieldFormat format,
        UInt16 tag)
    {
        int metadataPos;
        Debug.Assert(name.IndexOf('\0') < 0, "Field name must not have embedded NUL characters.");
        Debug.Assert(!encoding.HasChainFlag());
        Debug.Assert(!format.HasChainFlag());

        var utf8 = Encoding.UTF8;
        var nameMaxByteCount = utf8.GetMaxByteCount(name.Length);
        if (tag != 0)
        {
            var pos = this.meta.ReserveSpaceFor((uint)nameMaxByteCount + 5);
            if (pos < 0)
            {
                this.addFailed = true;
                return 0;
            }
            var metaSpan = this.meta.UsedSpan;
            pos += utf8.GetBytes(name, metaSpan.Slice(pos));
            metaSpan[pos++] = 0;
            metaSpan[pos++] = (byte)(encoding | EventHeaderFieldEncoding.ChainFlag);
            metaSpan[pos++] = (byte)(format | EventHeaderFieldFormat.ChainFlag);
            MemoryMarshal.Write(metaSpan.Slice(pos), ref tag);
            pos += sizeof(UInt16);
            this.meta.SetUsed(pos);
            metadataPos = pos - 3; // Returned from AddStructWithMetadataPosition.
        }
        else if (format != 0)
        {
            var pos = this.meta.ReserveSpaceFor((uint)nameMaxByteCount + 3);
            if (pos < 0)
            {
                this.addFailed = true;
                return 0;
            }
            var metaSpan = this.meta.UsedSpan;
            pos += utf8.GetBytes(name, metaSpan.Slice(pos));
            metaSpan[pos++] = 0;
            metaSpan[pos++] = (byte)(encoding | EventHeaderFieldEncoding.ChainFlag);
            metaSpan[pos++] = (byte)format;
            this.meta.SetUsed(pos);
            metadataPos = pos - 1; // Returned from AddStructWithMetadataPosition.
        }
        else
        {
            var pos = this.meta.ReserveSpaceFor((uint)nameMaxByteCount + 2);
            if (pos < 0)
            {
                this.addFailed = true;
                return 0;
            }
            var metaSpan = this.meta.UsedSpan;
            pos += utf8.GetBytes(name, metaSpan.Slice(pos));
            metaSpan[pos++] = 0;
            metaSpan[pos++] = (byte)encoding;
            this.meta.SetUsed(pos);
            metadataPos = -1; // Unreachable from AddStructWithMetadataPosition.
        }

        return metadataPos; // For AddStructWithMetadataPosition: Position of the format byte, or -1 if format == 0.
    }

    /// <returns>
    /// The position of the format byte within the metadata array (for AddStructWithMetadataPosition).
    /// </returns>
    private int AddMeta(
        ReadOnlySpan<byte> nameUtf8,
        EventHeaderFieldEncoding encoding,
        EventHeaderFieldFormat format,
        UInt16 tag)
    {
        int metadataPos;
        Debug.Assert(nameUtf8.IndexOf((byte)0) < 0, "Field name must not have embedded NUL characters.");
        Debug.Assert(!encoding.HasChainFlag());
        Debug.Assert(!format.HasChainFlag());

        var nameLength = nameUtf8.Length;
        if (tag != 0)
        {
            var pos = this.meta.ReserveSpaceFor((uint)nameLength + 5);
            if (pos < 0)
            {
                this.addFailed = true;
                return 0;
            }
            var metaSpan = this.meta.UsedSpan;
            nameUtf8.CopyTo(metaSpan.Slice(pos));
            pos += nameLength;
            metaSpan[pos++] = 0;
            metaSpan[pos++] = (byte)(encoding | EventHeaderFieldEncoding.ChainFlag);
            metaSpan[pos++] = (byte)(format | EventHeaderFieldFormat.ChainFlag);
            MemoryMarshal.Write(metaSpan.Slice(pos), ref tag);
            pos += sizeof(UInt16);
            this.meta.SetUsed(pos);
            metadataPos = pos - 3; // Returned from AddStructWithMetadataPosition.
        }
        else if (format != 0)
        {
            var pos = this.meta.ReserveSpaceFor((uint)nameLength + 3);
            if (pos < 0)
            {
                this.addFailed = true;
                return 0;
            }
            var metaSpan = this.meta.UsedSpan;
            nameUtf8.CopyTo(metaSpan.Slice(pos));
            pos += nameLength;
            metaSpan[pos++] = 0;
            metaSpan[pos++] = (byte)(encoding | EventHeaderFieldEncoding.ChainFlag);
            metaSpan[pos++] = (byte)format;
            this.meta.SetUsed(pos);
            metadataPos = pos - 1; // Returned from AddStructWithMetadataPosition.
        }
        else
        {
            var pos = this.meta.ReserveSpaceFor((uint)nameLength + 2);
            if (pos < 0)
            {
                this.addFailed = true;
                return 0;
            }
            var metaSpan = this.meta.UsedSpan;
            nameUtf8.CopyTo(metaSpan.Slice(pos));
            pos += nameLength;
            metaSpan[pos++] = 0;
            metaSpan[pos++] = (byte)encoding;
            this.meta.SetUsed(pos);
            metadataPos = -1; // Unreachable from AddStructWithMetadataPosition.
        }

        return metadataPos; // For AddStructWithMetadataPosition: Position of the format byte, or -1 if format == 0.
    }

    private struct Vector : IDisposable
    {
        public Vector(int initialCapacity)
        {
            Debug.Assert(0 < initialCapacity, "initialCapacity <= 0");
            Debug.Assert(initialCapacity <= 65536, "initialCapacity > 65536");
            Debug.Assert((initialCapacity & (initialCapacity - 1)) == 0, "initialCapacity is not a power of 2.");
            this.Bytes = ArrayPool<byte>.Shared.Rent(initialCapacity);
        }

        public byte[] Bytes { readonly get; private set; }

        public int Used { readonly get; private set; }

        public readonly Span<byte> UsedSpan => new Span<byte>(this.Bytes, 0, this.Used);

        public void Dispose()
        {
            var oldBytes = this.Bytes;
            this.Bytes = Array.Empty<byte>();
            this.Used = 0;

            if (oldBytes.Length != 0)
            {
                ArrayPool<byte>.Shared.Return(oldBytes);
            }
        }

        public void Reset()
        {
            this.Used = 0;
        }

        /// <summary>
        /// Returns false if resulting buffer would exceed 64KB.
        /// </summary>
        public bool AddByte(byte value)
        {
            var oldUsed = this.Used;
            Debug.Assert(this.Bytes.Length >= oldUsed);

            if (this.Bytes.Length == oldUsed &&
                !this.Grow(1))
            {
                return false;
            }

            this.Bytes[oldUsed] = value;
            this.Used = oldUsed + 1;
            return true;
        }

        /// <summary>
        /// Returns -1 if resulting buffer would exceed 64KB.
        /// </summary>
        public int ReserveSpaceFor(uint requiredSize)
        {
            int oldUsed = this.Used;
            Debug.Assert(this.Bytes.Length >= oldUsed);

            // condition will always be true if requiredSize > int.MaxValue.
            if ((uint)(this.Bytes.Length - oldUsed) < requiredSize &&
                !this.Grow(requiredSize))
            {
                return -1;
            }

            this.Used = oldUsed + (int)requiredSize;
            return oldUsed;
        }

        /// <summary>
        /// Returns empty if resulting buffer would exceed 64KB.
        /// </summary>
        public Span<byte> ReserveSpanFor(uint requiredSize)
        {
            int oldUsed = this.Used;
            Debug.Assert(this.Bytes.Length >= oldUsed);

            // condition will always be true if requiredSize > int.MaxValue.
            if ((uint)(this.Bytes.Length - oldUsed) < requiredSize &&
                !this.Grow(requiredSize))
            {
                return default;
            }

            this.Used = oldUsed + (int)requiredSize;
            return new Span<byte>(this.Bytes, oldUsed, (int)requiredSize);
        }

        public void SetUsed(int newUsed)
        {
            Debug.Assert(newUsed <= this.Used);
            this.Used = newUsed;
        }

        /// <summary>
        /// Returns false if resulting buffer would exceed 64KB.
        /// </summary>
        private bool Grow(uint requiredSize)
        {
            if (this.Bytes.Length <= 0)
            {
                throw new ObjectDisposedException(nameof(EventHeaderDynamicBuilder));
            }

            var newCapacity = (uint)this.Used + requiredSize;
            if (newCapacity < requiredSize || newCapacity > 65536)
            {
                return false;
            }

            var sharedPool = ArrayPool<byte>.Shared;
            var oldArray = this.Bytes;
            var newArray = sharedPool.Rent((int)newCapacity);

            Buffer.BlockCopy(oldArray, 0, newArray, 0, this.Used);
            this.Bytes = newArray;
            sharedPool.Return(oldArray);
            return true;
        }
    }
}
