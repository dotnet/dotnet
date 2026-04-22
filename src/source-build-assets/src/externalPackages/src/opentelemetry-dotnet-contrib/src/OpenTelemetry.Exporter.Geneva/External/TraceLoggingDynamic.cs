/*
Dynamic ETW TraceLogging Provider API for .NET.

GENERAL:

This implementation of manifest-free ETW supports
more functionality than the implementation in
System.Diagnostics.Tracing.EventSource,
but it also has higher runtime costs. This implementation is intended for use only when
the set of events is not known at compile-time. For example,
TraceLoggingDynamic.cs might be used to implement a library providing
manifest-free ETW to a higher-level API that does not enforce compile-time
event layout.

USAGE:

  // At start of program:
  static readonly EventProvider p = new EventProvider("MyProviderName");

  // When you want to write an event:
  if (p.IsEnabled(EventLevel.Verbose, 0x1234)) // Anybody listening?
  {
      var eb = new EventBuilder(); // Or reuse an existing EventBuilder
      eb.Reset("MyEventName", EventLevel.Verbose, 0x1234);
      eb.AddInt32("MyInt32FieldName", intValue);
      eb.AddUnicodeString("MyStringFieldName", stringValue);
      p.Write(eb);
  }

The EventProvider class encapsulates an ETW REGHANDLE, which is a handle
through which events for a particular provider can be written. The
EventProvider instance should generally be created at component
initialization and one instance should be shared by all code within the
component that needs to write events for the provider. In some cases, it
might be used in a smaller scope, in which case it should be closed via
Dispose().

The EventBuilder class is used to build an event. It stores the event name,
field names, field types, and field values. When all of the fields have
been added to the builder, call eventProvider.Write(eventBuilder, ...) to
send the eventBuilder's event to ETW.

To reduce performance impact, you might want to skip building the event if
there are no ETW listeners. To do this, use eventProvider.IsEnabled(...).
It returns true if there are one or more ETW listeners that would be
interested in an event with the specified level and keyword. You only
need to build and write the event if IsEnabled returned true for the level
and keyword that you will use in the event.

The EventBuilder object is a class, and it stores two byte[] buffers.
This means each new EventBuilder object generates garbage. You can
minimize garbage by reusing the same EventBuilder object for multiple
events instead of creating a new EventBuilder for each event.

NOTES:

Collect the events using Windows SDK tools like traceview or tracelog.
Decode the events using Windows SDK tools like traceview or tracefmt.
For example, for `EventProvider("MyCompany.MyComponent")`:

  tracelog -start MyTrace -f MyTraceFile.etl -guid *MyCompany.MyComponent -level 5 -matchanykw 0xf
  <run your program>
  tracelog -stop MyTrace
  tracefmt -o MyTraceData.txt MyTraceFile.etl

ETW events are limited in size (event size = headers + metadata + data).
Windows will drop any event that is larger than 64KB and will drop any event
that is larger than the buffer size of the recording session.

Most ETW decoding tools are unable to decode an event with more than 128
fields.
*/


namespace OpenTelemetry.Exporter.Geneva.External;

using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Debug = System.Diagnostics.Debug;
using Encoding = System.Text.Encoding;
using Interlocked = System.Threading.Interlocked;
using SHA1 = System.Security.Cryptography.SHA1;
using Win32Exception = System.ComponentModel.Win32Exception;

/// <summary>
/// The EventProvider class encapsulates an ETW REGHANDLE, which is a handle
/// through which events for a particular provider can be written. The
/// EventProvider instance should generally be created at component
/// initialization and one instance should be shared by all code within the
/// component that needs to write events for the provider. In some cases, it
/// might be used in a smaller scope, in which case it should be closed via
/// Dispose().
/// </summary>

#pragma warning disable CA5350
#pragma warning disable CA5392

internal sealed class EventProvider
    : CriticalFinalizerObject
    , IDisposable
{
    private static readonly byte[] namespaceBytes = new byte[] {
        0x48, 0x2C, 0x2D, 0xB2, 0xC3, 0x90, 0x47, 0xC8,
        0x87, 0xF8, 0x1A, 0x15, 0xBF, 0xC1, 0x30, 0xFB,
    };

    private int level = -1;
    private readonly int eventRegisterResult;
    private long keywordAny = 0;
    private long keywordAll = 0;
    private long regHandle = 0;
    private readonly byte[] providerMeta;
    private readonly NativeMethods.EnableCallback enableCallback; // KeepAlive
    private readonly string name;
    private readonly Guid guid;

    /// <summary>
    /// Finalizes EventProvider.
    /// </summary>
    ~EventProvider()
    {
        this.DisposeImpl();
    }

    /// <summary>
    /// Initializes and registers an ETW provider with the given provider name,
    /// using GetGuidForName(name) as the provider GUID.
    /// </summary>
    public EventProvider(string name, EventProviderOptions options = default)
        : this(name, GetGuidForName(name), options)
    {
        return;
    }

    /// <summary>
    /// <para>
    /// Initializes and registers an ETW provider with the given provider name
    /// and GUID. Note that most providers should use a GUID generated by
    /// hashing the provider name (as done by GetGuidForName) rather than using
    /// a randomly-generated GUID. Note that provider name and GUID should be a
    /// strong and persistent pair, i.e. a name should uniquely identify the
    /// GUID, and the GUID should uniquely identify the name. Once a name-GUID
    /// pair has been established, do not change the name without also changing
    /// the GUID, and do not change the GUID without also changing the name.
    /// </para><para>
    /// Note: The status code returned by EventRegister will be recorded in the
    /// EventRegisterResult property. It may be appropriate to check the value of
    /// this property during development or for diagnostic purposes, e.g. via
    /// Debug.Assert(p.EventRegisterResult == 0). The value will not normally be
    /// used in release builds because it is usually not necessary to change
    /// application behavior simply because ETW failed to initialize.
    /// </para>
    /// </summary>
    public EventProvider(string name, Guid guid, EventProviderOptions options = default)
    {
        // UINT16 size + UINT8 type + GUID.
        const byte GroupTraitByteCount = 2 + 1 + 16;

        var encodingUtf8 = Encoding.UTF8;
        int nameByteCount = encodingUtf8.GetByteCount(name);
        int traitsByteCount = options.GroupGuid.HasValue
            ? GroupTraitByteCount
            : 0;

        // UINT16 size + UTF8 name + NUL termination for name + traits.
        int totalByteCount = 2 + nameByteCount + 1 + traitsByteCount;

        this.providerMeta = new byte[totalByteCount];
        this.enableCallback = this.EnableCallback;
        this.name = name;
        this.guid = guid;

        int pos = 0;

        // UINT16 size + UTF8 name + NUL termination for name
        this.providerMeta[pos++] = (byte)(totalByteCount & 0xff);
        this.providerMeta[pos++] = (byte)(totalByteCount >> 8);
        pos += encodingUtf8.GetBytes(name, 0, name.Length, this.providerMeta, pos);
        this.providerMeta[pos++] = 0;

        if (options.GroupGuid.HasValue)
        {
            // UINT16 size + UINT8 type + GUID.
            this.providerMeta[pos++] = GroupTraitByteCount & 0xff;
            this.providerMeta[pos++] = GroupTraitByteCount >> 8;
            this.providerMeta[pos++] = 1; // EtwProviderTraitTypeGroup
            options.GroupGuid.Value.ToByteArray().CopyTo(this.providerMeta, pos);
            pos += 16;
        }

        Debug.Assert(totalByteCount == pos, "Provider traits size out-of-sync.");

        this.eventRegisterResult = NativeMethods.EventRegister(
            guid,
            this.enableCallback,
            IntPtr.Zero,
            out this.regHandle);
        if (this.regHandle == 0)
        {
            GC.SuppressFinalize(this);
        }
        else
        {
            NativeMethods.EventSetInformation(
                this.regHandle,
                2, // EventProviderSetTraits
                this.providerMeta,
                this.providerMeta.Length);
        }
    }

    /// <summary>
    /// For debugging/diagnostics only.
    /// Gets the Win32 status code that was returned by EventRegister during
    /// construction. This might be checked via Debug.Assert to identify issues
    /// during development or debugging, but it is not normally used in release
    /// builds because most programs should not change behavior just because ETW
    /// logging failed to initialize.
    /// </summary>
    public int EventRegisterResult
    {
        get
        {
            return this.eventRegisterResult;
        }
    }

    /// <summary>
    /// Gets this provider's name.
    /// </summary>
    public string Name
    {
        get
        {
            return this.name;
        }
    }

    /// <summary>
    /// Gets this provider's GUID (also known as the provider ID or the ETW
    /// control GUID).
    /// </summary>
    public Guid Guid
    {
        get
        {
            return this.guid;
        }
    }

    /// <summary>
    /// Gets the current thread's ETW activity ID.
    /// </summary>
    /// <exception cref="System.ComponentModel.Win32Exception">
    /// Throws Win32Exception if EventActivityIdControl returns an error.
    /// </exception>
    public static Guid CurrentThreadActivityId
    {
        get
        {
            var value = new Guid();
            CheckWin32(
                "EventActivityIdControl",
                NativeMethods.EventActivityIdControl(EventActivityCtrl.GetId, ref value));
            return value;
        }
    }

    /// <summary>
    /// Sets the current thread's ETW activity ID.
    /// </summary>
    /// <returns>The previous value of the current thread's ETW activity ID.</returns>
    /// <exception cref="System.ComponentModel.Win32Exception">
    /// Throws Win32Exception if EventActivityIdControl returns an error.
    /// </exception>
    public static Guid SetCurrentThreadActivityId(Guid value)
    {
        CheckWin32(
            "EventActivityIdControl",
            NativeMethods.EventActivityIdControl(EventActivityCtrl.GetSetId, ref value));
        return value;
    }

    /// <summary>
    /// Uses EventActivityIdControl to generate a new ETW activity ID.
    /// Note that an activity ID is not a true GUID because it is not globally-unique.
    /// It is unique within the current boot session of Windows.
    /// </summary>
    /// <returns>A new ETW activity ID.</returns>
    /// <exception cref="System.ComponentModel.Win32Exception">
    /// Throws Win32Exception if EventActivityIdControl returns an error.
    /// </exception>
    public static Guid CreateThreadActivityId()
    {
        var value = new Guid();
        CheckWin32(
            "EventActivityIdControl",
            NativeMethods.EventActivityIdControl(EventActivityCtrl.CreateId, ref value));
        return value;
    }

    /// <summary>
    /// Generates a GUID by uppercasing and then hashing the given provider name.
    /// This hash uses the same name-hashing algorithm as is used by EventSource,
    /// LoggingChannel, WPR, tracelog, traceview, and many other ETW utilities.
    /// </summary>
    public static Guid GetGuidForName(string providerName)
    {
        using (var sha1 = SHA1.Create())
        {
            // Hash = Sha1(namespace + arg.ToUpper().ToUtf16be())
            byte[] nameBytes = Encoding.BigEndianUnicode.GetBytes(providerName.ToUpperInvariant());
            sha1.TransformBlock(namespaceBytes, 0, namespaceBytes.Length, null, 0);
            sha1.TransformFinalBlock(nameBytes, 0, nameBytes.Length);

            // Guid = Hash[0..15], with Hash[7] tweaked approximately following RFC 4122
            byte[] guidBytes = new byte[16];
            Buffer.BlockCopy(sha1.Hash!, 0, guidBytes, 0, 16);
            guidBytes[7] = (byte)((guidBytes[7] & 0x0F) | 0x50);
            return new Guid(guidBytes);
        }
    }

    /// <summary>
    /// Gets a string with the provider name and guid.
    /// </summary>
    public override string ToString()
    {
        return String.Format(CultureInfo.InvariantCulture, "Provider \"{0}\" {{{1}}}",
            this.name,
            this.guid.ToString());
    }

    /// <summary>
    /// Returns true if any consumer is listening for events from this provider.
    /// </summary>
    public bool IsEnabled()
    {
        return 0 <= this.level;
    }

    /// <summary>
    /// Returns true if any consumer is listening for events from this provider
    /// at the specified verbosity, i.e. if an event with the specified level
    /// would be considered as enabled.
    /// </summary>
    public bool IsEnabled(EventLevel level)
    {
        return (int)level <= this.level;
    }

    /// <summary>
    /// Returns true if any consumer is listening for events from this provider
    /// at the specified verbosity, i.e. if an event with the specified level
    /// and keyword would be considered as enabled.
    /// </summary>
    public bool IsEnabled(EventLevel level, long keyword)
    {
        return (int)level <= this.level && this.IsEnabledForKeyword(keyword);
    }

    /// <summary>
    /// Returns true if any consumer is listening for events from this provider
    /// at the specified verbosity, i.e. if an event with the specified level
    /// and keyword would be considered as enabled.
    /// </summary>
    public bool IsEnabled(in EventDescriptor descriptor)
    {
        return (int)descriptor.Level <= this.level && this.IsEnabledForKeyword(descriptor.Keyword);
    }

    /// <summary>
    /// Writes an event to the provider, using the current thread's ETW activity ID
    /// as the activity ID for the event.
    /// </summary>
    /// <returns>
    /// 0 (ERROR_SUCCESS) if the event is filtered-out or was successfully sent.
    /// Otherwise, a Win32 error code if ETW was unable to accept the event.
    /// The return code is normally ignored in release code. The return code is
    /// normally used for debugging/diagnosis.
    /// </returns>
    public unsafe int Write(EventBuilder eventBuilder)
    {
        return this.WriteRaw(eventBuilder, null, null);
    }

    /// <summary>
    /// Writes an event to the provider, using the specified activity ID for the
    /// event.
    /// </summary>
    /// <returns>
    /// 0 (ERROR_SUCCESS) if the event is filtered-out or was successfully sent.
    /// Otherwise, a Win32 error code if ETW was unable to accept the event.
    /// The return code is normally ignored in release code. The return code is
    /// normally used for debugging/diagnosis.
    /// </returns>
    public unsafe int Write(EventBuilder eventBuilder, Guid activityId)
    {
        return this.WriteRaw(eventBuilder, &activityId, null);
    }

    /// <summary>
    /// Writes an event to the provider, using the specified activity ID for the
    /// event and including a related activity ID in the event. This should be used
    /// only for activity-start events (events with Opcode=Start).
    /// </summary>
    /// <returns>
    /// 0 (ERROR_SUCCESS) if the event is filtered-out or was successfully sent.
    /// Otherwise, a Win32 error code if ETW was unable to accept the event.
    /// The return code is normally ignored in release code. The return code is
    /// normally used for debugging/diagnosis.
    /// </returns>
    public unsafe int Write(EventBuilder eventBuilder, Guid activityId, Guid relatedActivityId)
    {
        return this.WriteRaw(eventBuilder, &activityId, &relatedActivityId);
    }

    private unsafe int WriteRaw(
        EventBuilder eventBuilder,
        Guid* activityId,
        Guid* relatedActivityId)
    {
        EventRawInfo rawInfo = eventBuilder.GetRawData();

        if (rawInfo.MetaSize < 2 ||
            rawInfo.Meta.Length < rawInfo.MetaSize)
        {
            throw new ArgumentOutOfRangeException(nameof(rawInfo.MetaSize));
        }

        if (rawInfo.DataSize != 0 &&
            rawInfo.Data.Length < rawInfo.DataSize)
        {
            throw new ArgumentOutOfRangeException(nameof(rawInfo.DataSize));
        }

        fixed (byte*
            pProviderMeta = this.providerMeta,
            pEventMeta = rawInfo.Meta,
            pEventData = rawInfo.Data)
        {
            EventDataDescriptor3 dd3;

            dd3.ProviderMeta = unchecked((ulong)(UIntPtr)pProviderMeta);
            dd3.ProviderMetaSize = this.providerMeta.Length;
            dd3.ProviderMetaType = 2; // EVENT_DATA_DESCRIPTOR_TYPE_PROVIDER_METADATA

            dd3.EventMeta = unchecked((ulong)(UIntPtr)pEventMeta);
            dd3.EventMetaSize = rawInfo.MetaSize;
            dd3.EventMetaType = 1; // EVENT_DATA_DESCRIPTOR_TYPE_EVENT_METADATA

            dd3.EventData = unchecked((ulong)(UIntPtr)pEventData);
            dd3.EventDataSize = rawInfo.DataSize;
            dd3.EventDataType = 0; // EVENT_DATA_DESCRIPTOR_TYPE_NONE

            pEventMeta[0] = unchecked((byte)rawInfo.MetaSize);
            pEventMeta[1] = unchecked((byte)(rawInfo.MetaSize >> 8));

            var descriptor = eventBuilder.Descriptor;
            return NativeMethods.EventWriteTransfer(
                this.regHandle,
                &descriptor,
                activityId,
                relatedActivityId,
                3,
                &dd3);
        }
    }

    /// <summary>
    /// Closes this provider.
    /// </summary>
    //[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public void Dispose()
    {
        this.DisposeImpl();
        GC.SuppressFinalize(this);
    }

    //[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private void DisposeImpl()
    {
        long oldRegHandle = Interlocked.Exchange(ref this.regHandle, 0);
        if (oldRegHandle != 0)
        {
            NativeMethods.EventUnregister(oldRegHandle);
        }
    }

    private bool IsEnabledForKeyword(long keyword)
    {
        return keyword == 0 || (
            (keyword & this.keywordAny) != 0 &&
            (keyword & this.keywordAll) == this.keywordAll);
    }

    private void EnableCallback(
        in Guid sourceId,
        int command,
        byte enableLevel,
        long matchAnyKeyword,
        long matchAllKeyword,
        IntPtr filterData,
        IntPtr callbackContext)
    {
        switch (command)
        {
            case 0: // EVENT_CONTROL_CODE_DISABLE_PROVIDER
                this.level = -1;
                break;
            case 1: // EVENT_CONTROL_CODE_ENABLE_PROVIDER
                this.level = enableLevel;
                this.keywordAny = matchAnyKeyword;
                this.keywordAll = matchAllKeyword;
                break;
        }
    }

    private static void CheckWin32(string api, int result)
    {
        if (result != 0)
        {
            throw new Win32Exception(result, api);
        }
    }

    internal struct EventRawInfo
    {
        public byte[] Meta;
        public byte[] Data;
        public int MetaSize;
        public int DataSize;
    }

    private enum EventActivityCtrl : int
    {
        Invalid,
        GetId,
        SetId,
        CreateId,
        GetSetId,
        CreateSetId,
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct EventDataDescriptor3
    {
        public ulong ProviderMeta;
        public int ProviderMetaSize;
        public int ProviderMetaType;
        public ulong EventMeta;
        public int EventMetaSize;
        public int EventMetaType;
        public ulong EventData;
        public int EventDataSize;
        public int EventDataType;
    }

    private sealed class NativeMethods
    {
        public delegate void EnableCallback(
            in Guid sourceId,
            int command,
            byte level,
            long matchAnyKeyword,
            long matchAllKeyword,
            IntPtr filterData,
            IntPtr callbackContext);

        [DllImport(
            "api-ms-win-eventing-provider-l1-1-0.dll",
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            SetLastError = false)]
        public static extern int EventRegister(
            in Guid providerId,
            EnableCallback enableCallback,
            IntPtr callbackContext,
            out long regHandle);

        [DllImport(
            "api-ms-win-eventing-provider-l1-1-0.dll",
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            SetLastError = false)]
        public static extern int EventUnregister(
            long regHandle);

        [DllImport(
            "api-ms-win-eventing-provider-l1-1-0.dll",
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            SetLastError = false)]
        public static extern int EventSetInformation(
            long regHandle,
            int informationClass,
            byte[] eventInformation,
            int informationLength);

        [DllImport(
            "api-ms-win-eventing-provider-l1-1-0.dll",
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            SetLastError = false)]
        public static extern unsafe int EventWriteTransfer(
            long regHandle,
            [In] EventDescriptor* eventDescriptor,
            [In] Guid* activityId,
            [In] Guid* relatedActivityId,
            int userDataCount,
            [In] EventDataDescriptor3* userData);

        [DllImport(
            "api-ms-win-eventing-provider-l1-1-0.dll",
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            SetLastError = false)]
        public static extern int EventActivityIdControl(
            EventActivityCtrl controlCode,
            ref Guid activityId);
    }
}

/// <summary>
/// The EventBuilder class is used to build an event. It stores the event name,
/// field names, field types, and field values. Start an event and set the event
/// name by calling Reset. Add fields by calling one of the Add methods.
/// When all of the fields have been added to the builder, call
/// eventProvider.Write(eventBuilder, ...) to send the builder's data to ETW.
/// </summary>
/// <remarks>
/// The EventBuilder methods can throw exceptions in the following conditions:
///
/// - ArgumentException if AddStruct called with a fieldCount larger than 127.
/// - NullReferenceException if a parameter is null.
/// - InvalidOperationException if an Add method would require a buffer to grow
///   larger than 64KB.
/// - OutOfMemoryException if EventBuilder buffer growth fails.
///
/// EventBuilder will silently allow you to build events that cannot be written
/// or decoded:
///
/// - If more than 128 fields are used in an event (i.e. if you call more than
///   128 Add methods), almost all event decoders will fail to decode it.
/// - If the total event is too large, ETW will reject it. The exact size limit
///   depends on ETW listener configuration, but 65536 bytes is always too big.
///
/// If garbage needs to be minimized, you will want to reuse each EventBuilder
/// object for multiple events instead of creating a new EventBuilder object for
/// each event. Note that EventBuilder objects are not thread-safe, so you will be
/// responsible for caching objects such that only one thread uses a particular
/// instance of EventBuilder at any one time.
/// </remarks>
internal sealed class EventBuilder
{
    private enum InType : byte
    {
        Null, // Invalid type
        UnicodeString, // nul-terminated
        AnsiString, // nul-terminated
        Int8,
        UInt8,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Float32,
        Float64,
        Bool32,
        Binary, // counted, doesn't work for arrays.
        Guid, // size = 16
        Pointer_NotSupported,
        FileTime, // size = 8
        SystemTime, // size = 16
        Sid, // size = 8 + 4 * SubAuthorityCount
        HexInt32,
        HexInt64,
        CountedString, // counted
        CountedAnsiString, // counted
        Struct, // Use OutType for field count.
        CountedBinary, // counted, works for arrays, newer (not always supported)
        Mask = 31,
        NullArray = 64, // Invalid type
        UnicodeStringArray, // nul-terminated
        AnsiStringArray, // nul-terminated
        Int8Array,
        UInt8Array,
        Int16Array,
        UInt16Array,
        Int32Array,
        UInt32Array,
        Int64Array,
        UInt64Array,
        Float32Array,
        Float64Array,
        Bool32Array,
        BinaryArray_NotSupported,
        GuidArray, // size = 16
        PointerArray_NotSupported,
        FileTimeArray, // size = 8
        SystemTimeArray, // size = 16
        SidArray, // size = 8 + 4 * SubAuthorityCount
        HexInt32Array,
        HexInt64Array,
        CountedStringArray, // counted
        CountedAnsiStringArray, // counted
        StructArray, // Use OutType for field count.
        CountedBinaryArray, // counted, works for arrays, newer (not always supported)
    }

    private const byte InTypeCcount = 32;
    private const byte InTypeVcount = 64;
    private const byte ChainBit = 128;

    private string name;
    private Encoding utf8NameEncoding;
    private EventDescriptor descriptor;
    private int tag;

    /// <summary>
    /// Initial metadata capacity must be a power of 2 in the range 4..65536.
    /// </summary>
    private Vector metadata;

    /// <summary>
    /// Initial data capacity must be a power of 2 in the range 4..65536.
    /// </summary>
    private Vector data;

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
    public EventBuilder(int initialMetadataBufferSize = 256, int initialDataBufferSize = 256)
        : this(Encoding.UTF8, initialMetadataBufferSize, initialDataBufferSize)
    {
    }

    /// <summary>
    /// Advanced scenarios: Initializes a new instance of the EventBuilder class that
    /// uses a customized UTF-8 encoding for event and field names.
    /// </summary>
    /// <param name="utf8NameEncoding">
    /// The customized UTF-8 encoding to use for event and field names.
    /// </param>
    /// <param name="initialMetadataBufferSize">
    /// The initial capacity of the metadata buffer. This must be a power of 2 in the
    /// range 4 through 65536.
    /// </param>
    /// <param name="initialDataBufferSize">
    /// The initial capacity of the data buffer. This must be a power of 2 in the
    /// range 4 through 65536.
    /// </param>
    public EventBuilder(
        Encoding utf8NameEncoding,
        int initialMetadataBufferSize = 256,
        int initialDataBufferSize = 256)
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

        this.utf8NameEncoding = utf8NameEncoding;
        this.metadata = new Vector(initialMetadataBufferSize);
        this.data = new Vector(initialDataBufferSize);

        // The following has the same effect as Reset("").
        this.name = "";
        this.descriptor = new EventDescriptor(EventLevel.Verbose);
        this.metadata.ReserveSpaceFor(4);
    }

    /// <summary>
    /// Gets the name for the event.
    /// </summary>
    public string Name { get { return this.name; } }

    /// <summary>
    /// Gets the level for the event.
    /// </summary>
    public EventLevel Level { get { return this.descriptor.Level; } }

    /// <summary>
    /// Gets the keyword for the event.
    /// </summary>
    public long Keyword { get { return this.descriptor.Keyword; } }

    /// <summary>
    /// Gets the descriptor for the event.
    /// </summary>
    public EventDescriptor Descriptor { get { return this.descriptor; } }

    /// <summary>
    /// Gets the tag for the event.
    /// </summary>
    public int Tag { get { return this.tag; } }

    /// <summary>
    /// Advanced scenarios: Gets or sets the UTF-8 encoding that will be used for
    /// event and field names.
    /// </summary>
    public Encoding Utf8NameEncoding
    {
        get
        {
            return this.utf8NameEncoding;
        }

        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(Utf8NameEncoding));
            }

            this.utf8NameEncoding = value;
        }
    }

    /// <summary>
    /// Resets this EventBuilder and begins building a new event.
    /// </summary>
    /// <param name="name">
    /// Identifier (name) of the event. This must not be null or empty.
    /// The name string must not include any '\0' characters.
    /// Use a short but descriptive name since the name will be included in
    /// each event and will be the primary identifier for the event.
    /// For best event analysis, ensure that all events with a particular
    /// event name have the same field names and field types.
    /// </param>
    /// <param name="level">
    /// Event severity, default is Verbose.
    /// </param>
    /// <param name="keyword">
    /// Event keyword, default is 0x1. All events should have a non-zero keyword.
    /// </param>
    /// <param name="eventTag">
    /// 28-bit tag to associate with event, or 0 for none.
    /// </param>
    public void Reset(
        string name,
        EventLevel level = EventLevel.Verbose,
        long keyword = 0x1,
        int eventTag = 0)
    {
        this.name = name;
        this.descriptor = new EventDescriptor(level, keyword);
        this.tag = eventTag;
        this.ResetEvent();
    }

    /// <summary>
    /// Resets this EventBuilder and begins building a new event.
    /// </summary>
    /// <param name="name">
    /// Identifier (name) of the event. This must not be null or empty.
    /// The name string must not include any '\0' characters.
    /// Use a short but descriptive name since the name will be included in
    /// each event and will be the primary identifier for the event.
    /// For best event analysis, ensure that all events with a particular
    /// event name have the same field names and field types.
    /// </param>
    /// <param name="descriptor">
    /// Level, Keyword, Opcode, Task, Channel, Id, Version for event.
    /// </param>
    /// <param name="eventTag">
    /// 28-bit tag to associate with event, or 0 for none.
    /// </param>
    public void Reset(
        string name,
        EventDescriptor descriptor,
        int eventTag = 0)
    {
        this.name = name;
        this.descriptor = descriptor;
        this.tag = eventTag;
        this.ResetEvent();
    }

    /// <summary>
    /// Adds a UnicodeString field (nul-terminated utf-16le).
    /// NOTE: Prefer AddCountedString. Use AddUnicodeString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Xml, Json.
    /// </summary>
    public void AddUnicodeString(string name, String value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UnicodeString, outType, tag);
        this.AddScalarDataNulTerminatedString(value, 0, value.Length);
    }

    /// <summary>
    /// Adds a UnicodeString field (nul-terminated utf-16le).
    /// NOTE: Prefer AddCountedString. Use AddUnicodeString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Xml, Json.
    /// </summary>
    public void AddUnicodeString(string name, String value, int startIndex, int count, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UnicodeString, outType, tag);
        this.AddScalarDataNulTerminatedString(value, startIndex, count);
    }

    /// <summary>
    /// Adds a UnicodeString array field (nul-terminated utf-16le).
    /// NOTE: Prefer AddCountedString. Use AddUnicodeString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Xml, Json.
    /// </summary>
    public void AddUnicodeStringArray(string name, String[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UnicodeStringArray, outType, tag);
        this.AddArrayBegin(values.Length, 0);
        foreach (string value in values)
        {
            this.AddScalarDataNulTerminatedString(value, 0, value.Length);
        }
    }

    /// <summary>
    /// Adds an AnsiString field (nul-terminated MBCS).
    /// NOTE: Prefer AddCountedAnsiString. Use AddAnsiString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddAnsiString(string name, Byte[] value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.AnsiString, outType, tag);
        this.AddScalarDataNulTerminatedByteString(value, 0, value.Length);
    }

    /// <summary>
    /// Adds an AnsiString field (nul-terminated MBCS).
    /// NOTE: Prefer AddCountedAnsiString. Use AddAnsiString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddAnsiString(string name, String value, Encoding encoding, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.AnsiString, outType, tag);
        this.AddScalarDataNulTerminatedAnsiString(value, encoding, 0, value.Length);
    }

    /// <summary>
    /// Adds an AnsiString field (nul-terminated MBCS).
    /// NOTE: Prefer AddCountedAnsiString. Use AddAnsiString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddAnsiString(string name, Byte[] value, int startIndex, int count, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.AnsiString, outType, tag);
        this.AddScalarDataNulTerminatedByteString(value, startIndex, count);
    }

    /// <summary>
    /// Adds an AnsiString field (nul-terminated MBCS).
    /// NOTE: Prefer AddCountedAnsiString. Use AddAnsiString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddAnsiString(string name, String value, Encoding encoding, int startIndex, int count, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.AnsiString, outType, tag);
        this.AddScalarDataNulTerminatedAnsiString(value, encoding, startIndex, count);
    }

    /// <summary>
    /// Adds an AnsiString array field (nul-terminated MBCS).
    /// NOTE: Prefer AddCountedAnsiString. Use AddAnsiString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddAnsiStringArray(string name, Byte[][] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.AnsiStringArray, outType, tag);
        this.AddArrayBegin(values.Length, 0);
        foreach (var value in values)
        {
            this.AddScalarDataNulTerminatedByteString(value, 0, value.Length);
        }
    }

    /// <summary>
    /// Adds an AnsiString array field (nul-terminated MBCS).
    /// NOTE: Prefer AddCountedAnsiString. Use AddAnsiString only if the decoder requires nul-terminated strings.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddAnsiStringArray(string name, String[] values, Encoding encoding, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.AnsiStringArray, outType, tag);
        this.AddArrayBegin(values.Length, 0);
        foreach (var value in values)
        {
            this.AddScalarDataNulTerminatedAnsiString(value, encoding, 0, value.Length);
        }
    }

    /// <summary>
    /// Adds an Int8 field.
    /// Meaningful outTypes: Default (Signed), String (AnsiChar).
    /// </summary>
    public void AddInt8(string name, SByte value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Int8, outType, tag);
        this.AddScalarDataUInt8(unchecked((Byte)value));
    }

    /// <summary>
    /// Adds an Int8 array field.
    /// Meaningful outTypes: Default (Signed), String (AnsiChar).
    /// </summary>
    public void AddInt8Array(string name, SByte[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Int8Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(SByte), 0, values.Length);
    }

    /// <summary>
    /// Adds a UInt8 field.
    /// Meaningful outTypes: Default (Unsigned), Hex (HexInt8), String (AnsiChar), Boolean (Bool8).
    /// </summary>
    public void AddUInt8(string name, Byte value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UInt8, outType, tag);
        this.AddScalarDataUInt8(value);
    }

    /// <summary>
    /// Adds a UInt8 array field.
    /// Meaningful outTypes: Default (Unsigned), Hex (HexInt8), String (AnsiChar), Boolean (Bool8).
    /// </summary>
    public void AddUInt8Array(string name, Byte[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UInt8Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Byte), 0, values.Length);
    }

    /// <summary>
    /// Adds an Int16 field.
    /// Meaningful outTypes: Default (Signed).
    /// </summary>
    public void AddInt16(string name, Int16 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Int16, outType, tag);
        this.AddScalarDataUInt16(unchecked((UInt16)value));
    }

    /// <summary>
    /// Adds an Int16 array field.
    /// Meaningful outTypes: Default (Signed).
    /// </summary>
    public void AddInt16Array(string name, Int16[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Int16Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Int16), 0, values.Length);
    }

    /// <summary>
    /// Adds a UInt16 field.
    /// Meaningful outTypes: Default (Unsigned), Port (Big-endian UInt16), Hex (HexInt16),
    /// String (Utf16Char).
    /// </summary>
    public void AddUInt16(string name, UInt16 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UInt16, outType, tag);
        this.AddScalarDataUInt16(value);
    }

    /// <summary>
    /// Adds a UInt16 array field.
    /// Meaningful outTypes: Default (Unsigned), Port (Big-endian UInt16), Hex (HexInt16),
    /// String (Utf16Char).
    /// </summary>
    public void AddUInt16Array(string name, UInt16[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UInt16Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(UInt16), 0, values.Length);
    }

    /// <summary>
    /// Adds an Int32 field.
    /// Meaningful outTypes: Default (Signed), HResult.
    /// </summary>
    public void AddInt32(string name, Int32 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Int32, outType, tag);
        this.AddScalarDataUInt32(unchecked((UInt32)value));
    }

    /// <summary>
    /// Adds an Int32 array field.
    /// Meaningful outTypes: Default (Signed), HResult.
    /// </summary>
    public void AddInt32Array(string name, Int32[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Int32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Int32), 0, values.Length);
    }

    /// <summary>
    /// Adds a UInt32 field.
    /// Meaningful outTypes: Default (Unsigned), Pid, Tid, IPv4, Win32Error, NtStatus, Hex, CodePointer.
    /// </summary>
    public void AddUInt32(string name, UInt32 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UInt32, outType, tag);
        this.AddScalarDataUInt32(value);
    }

    /// <summary>
    /// Adds a UInt32 array field.
    /// Meaningful outTypes: Default (Unsigned), Pid, Tid, IPv4, Win32Error, NtStatus, Hex, CodePointer.
    /// </summary>
    public void AddUInt32Array(string name, UInt32[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UInt32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(UInt32), 0, values.Length);
    }

    /// <summary>
    /// Adds an Int64 field.
    /// Meaningful outTypes: Default (Signed).
    /// </summary>
    public void AddInt64(string name, Int64 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Int64, outType, tag);
        this.AddScalarDataUInt64(unchecked((UInt64)value));
    }

    /// <summary>
    /// Adds an Int64 array field.
    /// Meaningful outTypes: Default (Signed).
    /// </summary>
    public void AddInt64Array(string name, Int64[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Int64Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Int64), 0, values.Length);
    }

    /// <summary>
    /// Adds a UInt64 field.
    /// Meaningful outTypes: Default (Unsigned), Hex (HexInt64), CodePointer (HexInt64).
    /// </summary>
    public void AddUInt64(string name, UInt64 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UInt64, outType, tag);
        this.AddScalarDataUInt64(value);
    }

    /// <summary>
    /// Adds a UInt64 array field.
    /// Meaningful outTypes: Default (Unsigned), Hex (HexInt64), CodePointer (HexInt64).
    /// </summary>
    public void AddUInt64Array(string name, UInt64[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.UInt64Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(UInt64), 0, values.Length);
    }

    /// <summary>
    /// Adds an IntPtr field.
    /// Meaningful outTypes: Default (Signed).
    /// </summary>
    public void AddIntPtr(string name, IntPtr value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        if (IntPtr.Size == 8)
        {
            this.AddMetadata(name, InType.Int64, outType, tag);
            this.AddScalarDataUInt64(unchecked((UInt64)value));
        }
        else
        {
            this.AddMetadata(name, InType.Int32, outType, tag);
            this.AddScalarDataUInt32(unchecked((UInt32)value));
        }
    }

    /// <summary>
    /// Adds an IntPtr array field.
    /// Meaningful outTypes: Default (Signed).
    /// </summary>
    public void AddIntPtrArray(string name, IntPtr[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, IntPtr.Size == 8 ? InType.Int64Array : InType.Int32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, IntPtr.Size, 0, values.Length);
    }

    /// <summary>
    /// Adds a UIntPtr field.
    /// Meaningful outTypes: Default (Unsigned).
    /// </summary>
    public void AddUIntPtr(string name, UIntPtr value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        if (UIntPtr.Size == 8)
        {
            this.AddMetadata(name, InType.UInt64, outType, tag);
            this.AddScalarDataUInt64(unchecked((UInt64)value));
        }
        else
        {
            this.AddMetadata(name, InType.UInt32, outType, tag);
            this.AddScalarDataUInt32(unchecked((UInt32)value));
        }
    }

    /// <summary>
    /// Adds a UIntPtr array field.
    /// Meaningful outTypes: Default (Unsigned).
    /// </summary>
    public void AddUIntPtrArray(string name, UIntPtr[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, UIntPtr.Size == 8 ? InType.UInt64Array : InType.UInt32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, UIntPtr.Size, 0, values.Length);
    }

    /// <summary>
    /// Adds a Float32 field.
    /// Meaningful outTypes: Default.
    /// </summary>
    public void AddFloat32(string name, Single value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Float32, outType, tag);
        this.AddScalarDataFloat32(value);
    }

    /// <summary>
    /// Adds a Float32 array field.
    /// Meaningful outTypes: Default.
    /// </summary>
    public void AddFloat32Array(string name, Single[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Float32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Single), 0, values.Length);
    }

    /// <summary>
    /// Adds a Float64 field.
    /// Meaningful outTypes: Default.
    /// </summary>
    public void AddFloat64(string name, Double value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Float64, outType, tag);
        this.AddScalarDataFloat64(value);
    }

    /// <summary>
    /// Adds a Float64 array field.
    /// Meaningful outTypes: Default.
    /// </summary>
    public void AddFloat64Array(string name, Double[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Float64Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Double), 0, values.Length);
    }

    /// <summary>
    /// Adds a Bool32 field.
    /// Meaningful outTypes: Default (Boolean).
    /// </summary>
    public void AddBool32(string name, Int32 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Bool32, outType, tag);
        this.AddScalarDataUInt32(unchecked((UInt32)value));
    }

    /// <summary>
    /// Adds a Bool32 array field.
    /// Meaningful outTypes: Default (Boolean).
    /// </summary>
    public void AddBool32Array(string name, Int32[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Bool32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Int32), 0, values.Length);
    }

    /// <summary>
    /// Adds a Binary field.
    /// Meaningful outTypes: Default (Hex), IPv6, SocketAddress, Pkcs7WithTypeInfo.
    /// </summary>
    public void AddBinary(string name, Byte[] value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Binary, outType, tag);
        this.AddArrayDataBlockCopy(value, sizeof(Byte), 0, value.Length);
    }

    /// <summary>
    /// Adds a Binary field.
    /// Meaningful outTypes: Default (Hex), IPv6, SocketAddress, Pkcs7WithTypeInfo.
    /// </summary>
    public void AddBinary(string name, Byte[] value, int startIndex, int count, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Binary, outType, tag);
        this.AddArrayDataBlockCopy(value, sizeof(Byte), startIndex, count);
    }

    /// <summary>
    /// Adds a Guid field.
    /// Meaningful outTypes: Default.
    /// </summary>
    public unsafe void AddGuid(string name, Guid value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Guid, outType, tag);
        int pos = this.data.ReserveSpaceFor(16);
        Marshal.Copy((IntPtr)(&value), this.data.data, pos, 16);

    }

    /// <summary>
    /// Adds a Guid array field.
    /// Meaningful outTypes: Default.
    /// </summary>
    public unsafe void AddGuidArray(string name, Guid[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.GuidArray, outType, tag);
        fixed (Guid* valuesPtr = values)
        {
            var valuesLength = values.Length;
            var valuesSize = valuesLength * 16;
            int pos = this.AddArrayBegin(valuesLength, valuesSize);
            if (valuesPtr != null)
            {
                Marshal.Copy((IntPtr)valuesPtr, this.data.data, pos, valuesSize);
            }
        }
    }

    /// <summary>
    /// Adds a FileTime field.
    /// Meaningful outTypes: Default (FileTime), DateTimeUtc (explicit-UTC FileTime).
    /// </summary>
    public void AddFileTime(string name, Int64 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.FileTime, outType, tag);
        this.AddScalarDataUInt64(unchecked((UInt64)value));
    }

    /// <summary>
    /// Adds a FileTime array field.
    /// Meaningful outTypes: Default (FileTime), DateTimeUtc (explicit-UTC FileTime).
    /// </summary>
    public void AddFileTimeArray(string name, Int64[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.FileTimeArray, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Int64), 0, values.Length);
    }

    /// <summary>
    /// Adds a FileTime field.
    /// Meaningful outTypes: Default (FileTime), DateTimeUtc (explicit-UTC FileTime).
    /// Note: This will record the DateTime's raw time (local or UTC). It is recommended
    /// that you use AddField(dateTimeValue.ToUniversalTime()) to ensure that a UTC time
    /// is recorded in the ETW event.
    /// </summary>
    public void AddFileTime(string name, DateTime value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.FileTime, outType, tag);
        this.AddScalarDataUInt64(DateTimeToFileTime(value));
    }

    /// <summary>
    /// Adds a FileTime array field.
    /// Meaningful outTypes: Default (FileTime), DateTimeUtc (explicit-UTC FileTime).
    /// Note: This will record the DateTime's raw time (local or UTC). It is recommended
    /// that you use AddField(dateTimeValue.ToUniversalTime()) to ensure that a UTC time
    /// is recorded in the ETW event.
    /// </summary>
    public void AddFileTimeArray(string name, DateTime[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.FileTimeArray, outType, tag);

        int pos = this.AddArrayBegin(values.Length, values.Length * sizeof(UInt64));
        foreach (DateTime value in values)
        {
            pos += this.data.SetUInt64(pos, DateTimeToFileTime(value));
        }
    }

    /// <summary>
    /// Adds a SystemTime field.
    /// The input value must be Int16[8] or longer (extra will be ignored).
    /// </summary>
    public void AddSystemTime(string name, Int16[] value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.SystemTime, outType, tag);
        int pos = this.data.ReserveSpaceFor(16);
        Buffer.BlockCopy(value, 0, this.data.data, pos, 16);
    }

    /// <summary>
    /// Adds a SystemTime array field.
    /// The input value must be Int16[8] or longer (extra will be ignored).
    /// (extra words will be ignored).
    /// </summary>
    public void AddSystemTimeArray(string name, Int16[][] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.SystemTimeArray, outType, tag);

        var valuesLength = values.Length;
        int pos = this.AddArrayBegin(valuesLength, valuesLength * 16);
        foreach (var value in values)
        {
            Buffer.BlockCopy(value, 0, this.data.data, pos, 16);
            pos += 16;
        }
    }

    /// <summary>
    /// Adds a Sid field.
    /// The input value must be byte[8 + 4 * SubAuthorityCount] or longer (extra bytes will be ignored).
    /// </summary>
    public void AddSid(string name, Byte[] value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.Sid, outType, tag);
        this.AddDataBytes(value, 8 + 4 * value[1]);
    }

    /// <summary>
    /// Adds a Sid array field.
    /// The input value must be byte[8 + 4 * SubAuthorityCount] or longer (extra bytes will be ignored).
    /// </summary>
    public void AddSidArray(string name, Byte[][] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.SidArray, outType, tag);
        this.AddArrayBegin(values.Length, 0);
        foreach (var value in values)
        {
            this.AddDataBytes(value, 8 + 4 * value[1]);
        }
    }

    /// <summary>
    /// Adds a HexInt32 field.
    /// Meaningful outTypes: Default (Hex), Win32Error, NtStatus, CodePointer.
    /// </summary>
    public void AddHexInt32(string name, Int32 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.HexInt32, outType, tag);
        this.AddScalarDataUInt32(unchecked((UInt32)value));
    }

    /// <summary>
    /// Adds a HexInt32 array field.
    /// Meaningful outTypes: Default (Hex), Win32Error, NtStatus, CodePointer.
    /// </summary>
    public void AddHexInt32Array(string name, Int32[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.HexInt32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Int32), 0, values.Length);
    }

    /// <summary>
    /// Adds a HexInt32 field.
    /// Meaningful outTypes: Default (Hex), Win32Error, NtStatus, CodePointer.
    /// </summary>
    public void AddHexInt32(string name, UInt32 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.HexInt32, outType, tag);
        this.AddScalarDataUInt32(value);
    }

    /// <summary>
    /// Adds a HexInt32 array field.
    /// Meaningful outTypes: Default (Hex), Win32Error, NtStatus, CodePointer.
    /// </summary>
    public void AddHexInt32Array(string name, UInt32[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.HexInt32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(UInt32), 0, values.Length);
    }

    /// <summary>
    /// Adds a HexInt64 field.
    /// Meaningful outTypes: Default (Hex), CodePointer.
    /// </summary>
    public void AddHexInt64(string name, Int64 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.HexInt64, outType, tag);
        this.AddScalarDataUInt64(unchecked((UInt64)value));
    }

    /// <summary>
    /// Adds a HexInt64 array field.
    /// Meaningful outTypes: Default (Hex), CodePointer.
    /// </summary>
    public void AddHexInt64Array(string name, Int64[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.HexInt64Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(Int64), 0, values.Length);
    }

    /// <summary>
    /// Adds a HexInt64 field.
    /// Meaningful outTypes: Default (Hex), CodePointer.
    /// </summary>
    public void AddHexInt64(string name, UInt64 value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.HexInt64, outType, tag);
        this.AddScalarDataUInt64(value);
    }

    /// <summary>
    /// Adds a HexInt64 array field.
    /// Meaningful outTypes: Default (Hex), CodePointer.
    /// </summary>
    public void AddHexInt64Array(string name, UInt64[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.HexInt64Array, outType, tag);
        this.AddArrayDataBlockCopy(values, sizeof(UInt64), 0, values.Length);
    }

    /// <summary>
    /// Adds a HexIntPtr field.
    /// Meaningful outTypes: Default (Hex).
    /// </summary>
    public void AddHexIntPtr(string name, IntPtr value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        if (IntPtr.Size == 8)
        {
            this.AddMetadata(name, InType.HexInt64, outType, tag);
            this.AddScalarDataUInt64(unchecked((UInt64)value));
        }
        else
        {
            this.AddMetadata(name, InType.HexInt32, outType, tag);
            this.AddScalarDataUInt32(unchecked((UInt32)value));
        }
    }

    /// <summary>
    /// Adds a HexIntPtr array field.
    /// Meaningful outTypes: Default (Hex).
    /// </summary>
    public void AddHexIntPtrArray(string name, IntPtr[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, IntPtr.Size == 8 ? InType.HexInt64Array : InType.HexInt32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, IntPtr.Size, 0, values.Length);
    }

    /// <summary>
    /// Adds a HexIntPtr field.
    /// Meaningful outTypes: Default (Hex).
    /// </summary>
    public void AddHexIntPtr(string name, UIntPtr value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        if (UIntPtr.Size == 8)
        {
            this.AddMetadata(name, InType.HexInt64, outType, tag);
            this.AddScalarDataUInt64(unchecked((UInt64)value));
        }
        else
        {
            this.AddMetadata(name, InType.HexInt32, outType, tag);
            this.AddScalarDataUInt32(unchecked((UInt32)value));
        }
    }

    /// <summary>
    /// Adds a HexIntPtr array field.
    /// Meaningful outTypes: Default (Hex).
    /// </summary>
    public void AddHexIntPtrArray(string name, UIntPtr[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, UIntPtr.Size == 8 ? InType.HexInt64Array : InType.HexInt32Array, outType, tag);
        this.AddArrayDataBlockCopy(values, UIntPtr.Size, 0, values.Length);
    }

    /// <summary>
    /// Adds a CountedString field (utf-16le).
    /// Meaningful outTypes: Default (String), Xml, Json.
    /// </summary>
    public void AddCountedString(string name, String value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedString, outType, tag);
        this.AddScalarDataCountedString(value, 0, value.Length);
    }

    /// <summary>
    /// Adds a CountedString field (utf-16le).
    /// Meaningful outTypes: Default (String), Xml, Json.
    /// </summary>
    public void AddCountedString(string name, String value, int startIndex, int count, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedString, outType, tag);
        this.AddScalarDataCountedString(value, startIndex, count);
    }

    /// <summary>
    /// Adds a CountedString array field (utf-16le).
    /// Meaningful outTypes: Default (String), Xml, Json.
    /// </summary>
    public void AddCountedStringArray(string name, String[] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedStringArray, outType, tag);
        this.AddArrayBegin(values.Length, 0);
        foreach (string value in values)
        {
            this.AddScalarDataCountedString(value, 0, value.Length);
        }
    }

    /// <summary>
    /// Adds a CountedAnsiString field.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddCountedAnsiString(string name, Byte[] value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedAnsiString, outType, tag);
        this.AddArrayDataBlockCopy(value, sizeof(byte), 0, value.Length);
    }

    /// <summary>
    /// Adds a CountedAnsiString field.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddCountedAnsiString(string name, String value, Encoding encoding, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedAnsiString, outType, tag);
        this.AddScalarDataCountedAnsiString(value, encoding, 0, value.Length);
    }

    /// <summary>
    /// Adds a CountedAnsiString field.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddCountedAnsiString(string name, Byte[] value, int startIndex, int count, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedAnsiString, outType, tag);
        this.AddArrayDataBlockCopy(value, sizeof(byte), startIndex, count);
    }

    /// <summary>
    /// Adds a CountedAnsiString field.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddCountedAnsiString(string name, String value, Encoding encoding, int startIndex, int count, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedAnsiString, outType, tag);
        this.AddScalarDataCountedAnsiString(value, encoding, startIndex, count);
    }

    /// <summary>
    /// Adds a CountedAnsiString array field.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddCountedAnsiStringArray(string name, Byte[][] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedAnsiStringArray, outType, tag);
        this.AddArrayBegin(values.Length, 0);
        foreach (var value in values)
        {
            this.AddArrayDataBlockCopy(value, sizeof(byte), 0, value.Length);
        }
    }

    /// <summary>
    /// Adds a CountedAnsiString array field.
    /// Meaningful outTypes: Default (String), Utf8, Xml, Json.
    /// </summary>
    public void AddCountedAnsiStringArray(string name, String[] values, Encoding encoding, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedAnsiStringArray, outType, tag);
        this.AddArrayBegin(values.Length, 0);
        foreach (var value in values)
        {
            this.AddScalarDataCountedAnsiString(value, encoding, 0, value.Length);
        }
    }

    /// <summary>
    /// Adds a CountedBinary field. (Decoding requires Windows 2018 Fall Update or later.)
    /// Meaningful outTypes: Default (Hex), IPv6, SocketAddress, Pkcs7WithTypeInfo.
    /// </summary>
    public void AddCountedBinary(string name, Byte[] value, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedBinary, outType, tag);
        this.AddArrayDataBlockCopy(value, sizeof(Byte), 0, value.Length);
    }

    /// <summary>
    /// Adds a CountedBinary field. (Decoding requires Windows 2018 Fall Update or later.)
    /// Meaningful outTypes: Default (Hex), IPv6, SocketAddress, Pkcs7WithTypeInfo.
    /// </summary>
    public void AddCountedBinary(string name, Byte[] value, int startIndex, int count, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedBinary, outType, tag);
        this.AddArrayDataBlockCopy(value, sizeof(Byte), startIndex, count);
    }

    /// <summary>
    /// Adds a CountedBinary array field. (Decoding requires Windows 2018 Fall Update or later.)
    /// Meaningful outTypes: Default (Hex), IPv6, SocketAddress, Pkcs7WithTypeInfo.
    /// </summary>
    public void AddCountedBinaryArray(string name, Byte[][] values, EventOutType outType = EventOutType.Default, int tag = 0)
    {
        this.AddMetadata(name, InType.CountedBinaryArray, outType, tag);
        this.AddArrayBegin(values.Length, 0);
        foreach (var value in values)
        {
            this.AddArrayDataBlockCopy(value, sizeof(Byte), 0, value.Length);
        }
    }

    /// <summary>
    /// Adds a new logical field with the specified name and indicates that the next
    /// fieldCount logical fields should be considered as members of this field.
    /// Note that fieldCount must be in the range 1 to 127.
    /// </summary>
    /// <returns>
    /// The offset of the struct size within the metadata. This value is normally
    /// ignored. However, if you do not know the number of fields in a structure
    /// ahead of time, you can use this value with SetStructFieldCount.
    /// </returns>
    public int AddStruct(string name, byte fieldCount, int tag = 0)
    {
        if (fieldCount < 1 || fieldCount > 127)
        {
            throw new ArgumentOutOfRangeException(nameof(fieldCount));
        }

        return this.AddMetadata(name, InType.Struct, (EventOutType)fieldCount, tag);
    }

    /// <summary>
    /// Advanced: Resets the number of logical fields in the specified structure.
    /// </summary>
    /// <param name="metadataPosition">
    /// The position of the metadata field within the structure. This value is
    /// returned by the AddStruct method.
    /// </param>
    /// <param name="fieldCount">
    /// The actual number of fields in the structure. This value must be in the range
    /// 1 to 127.
    /// </param>
    public void SetStructFieldCount(int metadataPosition, byte fieldCount)
    {
        if (fieldCount < 1 || fieldCount > 127)
        {
            throw new ArgumentOutOfRangeException(nameof(fieldCount));
        }

        var meta = this.metadata.data;
        meta[metadataPosition] = (byte)((meta[metadataPosition] & 0x80) | (fieldCount & 0x7F));
    }

    /// <summary>
    /// Advanced: Extracts the raw data for the fields currently in the builder.
    /// This can be used to add a group of fields to a builder with AppendRawFields.
    /// </summary>
    /// <returns>Raw data that can be used with AppendRawFields</returns>
    public Tuple<byte[], byte[]> GetRawFields()
    {
        // Find start of first field.
        int metaPos = 2; // Skip space reserved for metadata size.

        // Skip past tags.
        for (; ; metaPos += 1)
        {
            Debug.Assert(metaPos < this.metadata.Size);
            if ((this.metadata.data[metaPos] & 0x80) == 0)
            {
                metaPos += 1;
                break;
            }
        }

        // Skip past event name.
        int fieldStart = 1 + Array.IndexOf<byte>(this.metadata.data, 0, metaPos, this.metadata.Size - metaPos);
        Debug.Assert(fieldStart > 0);

        var dataSize = this.data.Size;
        var dataCopy = new byte[dataSize];
        Array.Copy(this.data.data, 0, dataCopy, 0, dataSize);

        var metaSize = this.metadata.Size - fieldStart;
        var metaCopy = new byte[metaSize];
        Array.Copy(this.metadata.data, fieldStart, metaCopy, 0, metaSize);

        return new Tuple<byte[], byte[]>(dataCopy, metaCopy);
    }

    /// <summary>
    /// Advanced: Appends raw data and metadata to the builder. This can be used with
    /// the data from GetRawFields.
    /// </summary>
    /// <param name="rawFields">Data from GetRawFields</param>
    public void AppendRawFields(Tuple<byte[], byte[]> rawFields)
    {
        var rawData = rawFields.Item1;
        int dataPos = this.data.ReserveSpaceFor(rawData.Length);
        Buffer.BlockCopy(rawData, 0, this.data.data, dataPos, rawData.Length);

        var rawMeta = rawFields.Item2;
        int metaPos = this.metadata.ReserveSpaceFor(rawMeta.Length);
        Buffer.BlockCopy(rawMeta, 0, this.metadata.data, metaPos, rawMeta.Length);
    }

    /// <summary>
    /// Internal - for use by EventProvider.Write.
    /// Use EventProvider.Write to write events.
    /// </summary>
    internal EventProvider.EventRawInfo GetRawData()
    {
        return new EventProvider.EventRawInfo
        {
            Meta = this.metadata.data,
            Data = this.data.data,
            MetaSize = this.metadata.Size,
            DataSize = this.data.Size
        };
    }

    /// <returns>The position of the outType byte within the metadata array.</returns>
    private unsafe int AddMetadata(string name, InType inType, EventOutType outType, int tag)
    {
        int metadataPos;
        Debug.Assert(name.IndexOf('\0') < 0, "Field name must not have embedded NUL characters.");
        Debug.Assert(((int)outType & 127) == (int)outType, "Invalid outType.");
        Debug.Assert((tag & 0xfffffff) == tag, "Tag must be 28-bit value.");

        int pos;

        var nameLength = name.Length;
        var nameMaxByteCount = this.utf8NameEncoding.GetMaxByteCount(nameLength);
        if (tag != 0)
        {
            pos = this.metadata.ReserveSpaceFor(nameMaxByteCount + 7);
            fixed (char* pName = name)
            {
                fixed (byte* pMeta = this.metadata.data)
                {
                    pos += this.utf8NameEncoding.GetBytes(pName, nameLength, pMeta + pos, nameMaxByteCount);
                    pMeta[pos + 0] = 0; // nul
                    pMeta[pos + 1] = (byte)((byte)inType | ChainBit);
                    pMeta[pos + 2] = (byte)((byte)outType | ChainBit);
                    pMeta[pos + 3] = unchecked((byte)(0x80 | tag >> 21));
                    pMeta[pos + 4] = unchecked((byte)(0x80 | tag >> 14));
                    pMeta[pos + 5] = unchecked((byte)(0x80 | tag >> 7));
                    pMeta[pos + 6] = unchecked((byte)(0x7F & tag));
                }
            }
            this.metadata.SetSize(pos + 7);
            metadataPos = pos + 2;
        }
        else if (outType != EventOutType.Default)
        {
            pos = this.metadata.ReserveSpaceFor(nameMaxByteCount + 3);
            fixed (char* pName = name)
            {
                fixed (byte* pMeta = this.metadata.data)
                {
                    pos += this.utf8NameEncoding.GetBytes(pName, nameLength, pMeta + pos, nameMaxByteCount);
                    pMeta[pos + 0] = 0; // nul
                    pMeta[pos + 1] = (byte)((byte)inType | ChainBit);
                    pMeta[pos + 2] = (byte)outType;
                }
            }
            this.metadata.SetSize(pos + 3);
            metadataPos = pos + 2;
        }
        else
        {
            pos = this.metadata.ReserveSpaceFor(nameMaxByteCount + 2);
            fixed (char* pName = name)
            {
                fixed (byte* pMeta = this.metadata.data)
                {
                    pos += this.utf8NameEncoding.GetBytes(pName, nameLength, pMeta + pos, nameMaxByteCount);
                    pMeta[pos + 0] = 0; // nul
                    pMeta[pos + 1] = (byte)inType;
                }
            }
            this.metadata.SetSize(pos + 2);
            metadataPos = -1;
        }

        return metadataPos;
    }

    private unsafe int AddMetadataName(string name, int nulInOutTagSize)
    {
        int nameLength = name.Length;
        int byteMax = this.utf8NameEncoding.GetMaxByteCount(nameLength) + nulInOutTagSize; // nul + intype + outtype + tag

        int pos = this.metadata.ReserveSpaceFor(byteMax);
        fixed (char* pName = name)
        {
            fixed (byte* pMeta = this.metadata.data)
            {
                pos += this.utf8NameEncoding.GetBytes(pName, nameLength, pMeta + pos, byteMax);
            }
        }

        return pos;
    }

    private void AddDataBytes(byte[] value, int length)
    {
        int pos = this.data.ReserveSpaceFor(length);
        Buffer.BlockCopy(value, 0, this.data.data, pos, length);
    }

    private void AddScalarDataUInt8(Byte value)
    {
        int pos = this.data.ReserveSpaceFor(1);
        this.data.SetUInt8(pos, value);
    }

    private void AddScalarDataUInt16(UInt16 value)
    {
        int pos = this.data.ReserveSpaceFor(2);
        this.data.SetUInt16(pos, value);
    }

    private void AddScalarDataUInt32(UInt32 value)
    {
        int pos = this.data.ReserveSpaceFor(4);
        this.data.SetUInt32(pos, value);
    }

    private void AddScalarDataUInt64(UInt64 value)
    {
        int pos = this.data.ReserveSpaceFor(8);
        this.data.SetUInt64(pos, value);
    }

    private unsafe void AddScalarDataFloat32(Single value)
    {
        int pos = this.data.ReserveSpaceFor(4);
        this.data.SetUInt32(pos, *(UInt32*)&value);
    }

    private unsafe void AddScalarDataFloat64(Double value)
    {
        int pos = this.data.ReserveSpaceFor(8);
        this.data.SetUInt64(pos, *(UInt64*)&value);
    }

    private void AddScalarDataNulTerminatedByteString(byte[] value, int startIndex, int count)
    {
        int endIndex = Array.IndexOf(value, (byte)0, startIndex, count);
        int copyLength = endIndex < 0
            ? count
            : endIndex - startIndex;
        int pos = this.data.ReserveSpaceFor(copyLength + sizeof(byte));
        Buffer.BlockCopy(value, startIndex, this.data.data, pos, copyLength);
        this.data.SetUInt8(pos + copyLength, 0);
    }

    private unsafe void AddScalarDataNulTerminatedString(String value, int startIndex, int count)
    {
        int endIndex = value.IndexOf('\0', startIndex, count);
        int copyLength = endIndex < 0
            ? count
            : endIndex - startIndex;

        int valueSize = copyLength * sizeof(UInt16);
        int pos = this.data.ReserveSpaceFor(valueSize + sizeof(UInt16));
        fixed (char* pValue = value)
        {
            fixed (byte* pData = this.data.data)
            {
                // Safety: startIndex and count were validated by IndexOf.
                Buffer.MemoryCopy(pValue + startIndex, pData + pos, valueSize, valueSize);
                pos += valueSize;
                pData[pos++] = 0;
                pData[pos++] = 0;
            }
        }
    }

    private unsafe void AddScalarDataCountedString(String value, int startIndex, int count)
    {
        int valueLength = value.Length;
        if (startIndex < 0 || startIndex > valueLength)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        if (count < 0 || count > valueLength - startIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        int valueSize = count * sizeof(UInt16);
        int pos = this.data.ReserveSpaceFor(sizeof(UInt16) + valueSize);
        fixed (char* pValue = value)
        {
            fixed (byte* pData = this.data.data)
            {
                pData[pos++] = (byte)valueSize;
                pData[pos++] = (byte)(valueSize >> 8);

                // Safety: startIndex and count validated above.
                Buffer.MemoryCopy(pValue + startIndex, pData + pos, valueSize, valueSize);
            }
        }
    }

    private unsafe void AddScalarDataNulTerminatedAnsiString(
        String value,
        Encoding encoding,
        int startIndex,
        int count)
    {
        int endIndex = value.IndexOf('\0', startIndex, count);
        int copyLength = endIndex < 0
            ? count
            : endIndex - startIndex;

        int maxValueSize = encoding.GetMaxByteCount(copyLength);
        int pos = this.data.ReserveSpaceFor(maxValueSize + 1);
        fixed (char* pValue = value)
        {
            fixed (byte* pData = this.data.data)
            {
                // Safety: startIndex and count were validated by IndexOf.
                pos += encoding.GetBytes(
                    pValue + startIndex,
                    copyLength,
                    pData + pos,
                    maxValueSize);
                pData[pos++] = 0;
            }
        }

        this.data.SetSize(pos);
    }

    private unsafe void AddScalarDataCountedAnsiString(String value, Encoding encoding, int startIndex, int count)
    {
        int valueLength = value.Length;
        if (startIndex < 0 || startIndex > valueLength)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        if (count < 0 || count > valueLength - startIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        int maxValueSize = encoding.GetMaxByteCount(count);
        int pos = this.data.ReserveSpaceFor(sizeof(UInt16) + maxValueSize);
        fixed (char* pValue = value)
        {
            fixed (byte* pData = this.data.data)
            {
                // Safety: startIndex and count validated above.
                int valueSize = encoding.GetBytes(
                    pValue + startIndex,
                    count,
                    pData + pos + sizeof(UInt16),
                    maxValueSize);
                pData[pos++] = (byte)valueSize;
                pData[pos++] = (byte)(valueSize >> 8);
                pos += valueSize;
            }
        }

        this.data.SetSize(pos);
    }

    /// <summary>
    /// Usage: AddArrayDataBlockCopy(values, sizeof(ValueType)).
    /// For primitives only. Do not use if sizeof(ValueType) does not compile.
    /// </summary>
    private void AddArrayDataBlockCopy(Array values, int elementSize, int startIndex, int count)
    {
        Debug.Assert(count >= 0);
        Debug.Assert(count <= values.Length - startIndex);

        int valuesSize = count * elementSize;
        int pos = this.AddArrayBegin(count, valuesSize);
        Buffer.BlockCopy(values, startIndex * elementSize, this.data.data, pos, valuesSize);
    }

    /// <summary>
    /// Reserves space and writes the array length.
    /// Returns the position where array data should be written.
    /// </summary>
    private int AddArrayBegin(int valuesLength, int valuesSize)
    {
        // UINT16 array size
        int pos = this.data.ReserveSpaceFor(sizeof(ushort) + valuesSize);
        this.data.SetUInt16(pos, (ushort)valuesLength);
        return pos + sizeof(ushort);
    }

    private static UInt64 DateTimeToFileTime(DateTime value)
    {
        long valueTicks = value.Ticks;
        return valueTicks < 504911232000000000 ? 0u : (UInt64)valueTicks - 504911232000000000u;
    }

    private unsafe void ResetEvent()
    {
        Debug.Assert(this.name.IndexOf('\0') < 0, "Event name must not have embedded NUL characters.");
        this.data.Reset();
        this.metadata.Reset();

        int pos = 2; // skip UInt16 MetadataSize

        var eventName = this.name;
        var eventNameLength = eventName.Length;
        var eventNameMaxByteCount = this.utf8NameEncoding.GetMaxByteCount(eventNameLength);
        if ((this.tag & 0x0FE00000) == tag)
        {
            // Event tag fits in 7 bits.
            this.metadata.ReserveSpaceFor(eventNameMaxByteCount + 2 + 1 + 1);
            fixed (char* pEventName = eventName)
            {
                fixed (byte* pMeta = this.metadata.data)
                {
                    pMeta[pos++] = unchecked((byte)(this.tag >> 21));
                    pos += this.utf8NameEncoding.GetBytes(pEventName, eventNameLength, pMeta + pos, eventNameMaxByteCount);
                    pMeta[pos++] = 0;
                }
            }
        }
        else if ((tag & 0x0FFFC000) == tag)
        {
            // Event tag fits in 14 bits.
            this.metadata.ReserveSpaceFor(eventNameMaxByteCount + 2 + 2 + 1);
            fixed (char* pEventName = eventName)
            {
                fixed (byte* pMeta = this.metadata.data)
                {
                    pMeta[pos++] = unchecked((byte)(0x80 | this.tag >> 21));
                    pMeta[pos++] = unchecked((byte)(0x7f & this.tag >> 14));
                    pos += this.utf8NameEncoding.GetBytes(pEventName, eventNameLength, pMeta + pos, eventNameMaxByteCount);
                    pMeta[pos++] = 0;
                }
            }
        }
        else if ((this.tag & 0x0FFFFFFF) == tag)
        {
            // Event tag fits in 28 bits.
            this.metadata.ReserveSpaceFor(eventNameMaxByteCount + 2 + 4 + 1);
            fixed (char* pEventName = eventName)
            {
                fixed (byte* pMeta = this.metadata.data)
                {
                    pMeta[pos++] = unchecked((byte)(0x80 | this.tag >> 21));
                    pMeta[pos++] = unchecked((byte)(0x80 | this.tag >> 14));
                    pMeta[pos++] = unchecked((byte)(0x80 | this.tag >> 7));
                    pMeta[pos++] = unchecked((byte)(0x7f & this.tag >> 0));
                    pos += this.utf8NameEncoding.GetBytes(pEventName, eventNameLength, pMeta + pos, eventNameMaxByteCount);
                    pMeta[pos++] = 0;
                }
            }
        }
        else
        {
            throw new ArgumentException("Tag does not fit in 28 bits.", "tag");
        }

        this.metadata.SetSize(pos);
    }

    private struct Vector
    {
        public byte[] data;
        private int size;

        public Vector(int initialCapacity)
        {
            Debug.Assert(0 < initialCapacity, "initialCapacity <= 0");
            Debug.Assert(initialCapacity <= 65536, "initialCapacity > 65536");
            Debug.Assert((initialCapacity & (initialCapacity - 1)) == 0, "initialCapacity is not a power of 2.");
            this.data = new byte[initialCapacity];
            this.size = 0;
        }

        public int Size
        {
            get { return this.size; }
        }

        public void Reset()
        {
            this.size = 0;
        }

        public int SetUInt8(int pos, Byte value)
        {
            this.data[pos] = value;
            return 1;
        }

        public int SetUInt16(int pos, UInt16 value)
        {
            this.data[pos + 0] = unchecked((byte)(value));
            this.data[pos + 1] = unchecked((byte)(value >> 8));
            return 2;
        }

        public int SetUInt32(int pos, UInt32 value)
        {
            this.data[pos + 0] = unchecked((byte)(value));
            this.data[pos + 1] = unchecked((byte)(value >> 8));
            this.data[pos + 2] = unchecked((byte)(value >> 16));
            this.data[pos + 3] = unchecked((byte)(value >> 24));
            return 4;
        }

        public int SetUInt64(int pos, UInt64 value)
        {
            this.data[pos + 0] = unchecked((byte)(value));
            this.data[pos + 1] = unchecked((byte)(value >> 8));
            this.data[pos + 2] = unchecked((byte)(value >> 16));
            this.data[pos + 3] = unchecked((byte)(value >> 24));
            this.data[pos + 4] = unchecked((byte)(value >> 32));
            this.data[pos + 5] = unchecked((byte)(value >> 40));
            this.data[pos + 6] = unchecked((byte)(value >> 48));
            this.data[pos + 7] = unchecked((byte)(value >> 56));
            return 8;
        }

        public int ReserveSpaceFor(int requiredSize)
        {
            int oldSize = this.size;
            if (this.data.Length - oldSize < requiredSize)
            {
                this.Grow(requiredSize);
            }

            this.size += requiredSize;
            return oldSize;
        }

        public void SetSize(int newSize)
        {
            Debug.Assert(newSize <= this.size);
            this.size = newSize;
        }

        private void Grow(int byteSize)
        {
            int newCapacity = this.data.Length;
            while (true)
            {
                newCapacity *= 2;

                if (newCapacity > 65536)
                {
                    throw new InvalidOperationException("Event too large");
                }

                if (newCapacity - this.size >= byteSize)
                {
                    break;
                }
            }

            byte[] newData = new byte[newCapacity];
            Buffer.BlockCopy(this.data, 0, newData, 0, this.size);
            this.data = newData;
        }
    }
}

/// <summary>
/// Supplies details that will be recorded with an event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct EventDescriptor
{
    /// <summary>
    /// Supplies a unique identifier for this event. Set Id and Version to 0 if the
    /// event has no unique identifier. This should be non-zero only if all events
    /// with a particular provider guid + event Id + event Version have identical
    /// event name, event field names, and event field types.
    /// </summary>
    public short Id;

    /// <summary>
    /// Supplies a version for the event. Set this to 0 if Id is 0 or if this is
    /// the first version of an event. This should be incremented if any change is
    /// made to the event name, field names, or field types of an event with a
    /// non-zero Id.
    /// </summary>
    public byte Version;

    /// <summary>
    /// Supplies the channel for an event. This should almost always be set to 11
    /// for TraceLogging events.
    /// </summary>
    public byte Channel;

    /// <summary>
    /// Supplies a severity level for an event. This value is used for event
    /// filtering. Set the level to a value from 1 (Critical Error) to 5 (Verbose).
    /// Note that event level 0 is not recommended because it bypasses all level
    /// filtering.
    /// </summary>
    public EventLevel Level;

    /// <summary>
    /// Adds operational semantics to an event, such as "beginning activity" and
    /// "ending activity". Event decoders may use these values to group or flag
    /// certain events.
    /// </summary>
    public EventOpcode Opcode;

    /// <summary>
    /// User-defined value.
    /// </summary>
    public short Task;

    /// <summary>
    /// The lower 48 bits of a keyword are user-defined categories that can be
    /// defined and used to filter events. For example, a provider might declare
    /// that within the provider, all "networking" events will set bit 0x1 in
    /// the event's Keyword. Then when collecting events, if the collector only
    /// wants "networking" events then the collector could ask ETW to deliver
    /// only the events that have bit 0x1 set in the Keyword.
    ///
    /// The upper 16 bits of a keyword are reserved for definition by Microsoft.
    ///
    /// All providers should define keyword and all events should have at least
    /// one keyword bit set. Events with no keyword bits set will usually bypass
    /// keyword-based filtering, causing problems for event processing.
    /// </summary>
    public long Keyword;

    /// <summary>
    /// Initializes a new instance of the EventDescriptor struct, setting Level
    /// and Keyword as specified and setting Channel to 11.
    /// Do not use 0 for level or keyword as that will make it difficult for
    /// event listeners to properly filter the event.
    /// </summary>
    public EventDescriptor(EventLevel level, long keyword = 0x1)
    {
        this.Id = 0;
        this.Version = 0;
        this.Channel = 11;
        this.Level = level;
        this.Opcode = EventOpcode.Info;
        this.Task = 0;
        this.Keyword = keyword;
    }
}

/// <summary>
/// Provides advanced options that can be configured when registering a provider.
/// </summary>
internal struct EventProviderOptions
{
    /// <summary>
    /// If specified, the provider will be registered as belonging to the specified
    /// provider group. Note that most providers do not use groups so this can
    /// usually be left null.
    /// </summary>
    public Nullable<Guid> GroupGuid;
}

/// <summary>
/// Event field formatting hint. Used by EventBuilder's Add methods.
///
/// Every ETW field has an InType (base field type) and an OutType (formatting
/// hint). OutType may be ignored by event decoders, and not all OutTypes are valid
/// for all InTypes. For example, the Boolean OutType is a valid hint only for fields
/// with InType UInt8 and InType Bool32. Check the "inTypes" list in winmeta.xml
/// to determine what OutTypes are valid for each InType.
/// </summary>
internal enum EventOutType : byte
{
    /// <summary>
    /// Requests normal (default) formatting for the field. For example, a UInt32
    /// field with OutType=Default will be formatted as "unsigned decimal" because
    /// "Unsigned" is the default OutType for UInt32 fields.
    /// </summary>
    Default,

    /// <summary>
    /// Suggests that the field be hidden. Ignored by most decoders.
    /// </summary>
    NoPrint,

    /// <summary>
    /// Suggests that the field be formatted as a string. For example, applying
    /// OutType=String to a UInt8 field will cause the field to be treated as an ANSI
    /// code page character instead of as an unsigned decimal integer.
    ///
    /// String is meaningful when applied to fields of type Int8, UInt8, UInt16.
    ///
    /// String is the default OutType for fields of type UnicodeString, AnsiString,
    /// CountedString, CountedAnsiString, Sid.
    /// </summary>
    String,

    /// <summary>
    /// Suggests that the field be formatted as a boolean (true/false).
    ///
    /// Boolean is meaningful when applied to fields of type UInt8.
    ///
    /// Boolean is the default OutType for fields of type Bool32.
    /// </summary>
    Boolean,

    /// <summary>
    /// Suggests that the field be formatted as hexadecimal.
    ///
    /// Hex is meaningful when applied to fields of type UInt8, UInt16, UInt32,
    /// UInt64.
    ///
    /// Hex is the default OutType for fields of type HexInt32, HexInt64, Binary,
    /// CountedBinary.
    /// </summary>
    Hex,

    /// <summary>
    /// Suggests that the field be formatted as a process identifier.
    ///
    /// Pid is meaningful when applied to fields of type UInt32.
    /// </summary>
    Pid,

    /// <summary>
    /// Suggests that the field be formatted as a thread identifier.
    ///
    /// Tid is meaningful when applied to fields of type UInt32.
    /// </summary>
    Tid,

    /// <summary>
    /// Suggests that the field be formatted as a big-endian TCP/UDP port.
    ///
    /// Port is meaningful when applied to fields of type UInt16.
    /// </summary>
    Port,

    /// <summary>
    /// Suggests that the field be formatted as an IPv4 address (dotted quad).
    ///
    /// IPv4 is meaningful when applied to fields of type UInt32.
    /// </summary>
    IPv4,

    /// <summary>
    /// Suggests that the field be formatted as an IPv6 address.
    ///
    /// IPv6 is meaningful when applied to fields of type Binary, CountedBinary.
    /// </summary>
    IPv6,

    /// <summary>
    /// Suggests that the field be formatted as a sockaddr.
    ///
    /// SocketAddress is meaningful when applied to fields of type Binary,
    /// CountedBinary.
    /// </summary>
    SocketAddress,

    /// <summary>
    /// Suggests that the field be formatted as XML text.
    ///
    /// Xml is meaningful when applied to fields of type UnicodeString, AnsiString,
    /// CountedString, CountedAnsiString.
    ///
    /// Note that When Xml is applied to an AnsiString or CountedAnsiString field,
    /// it implies that the field is encoded as UTF-8.
    /// </summary>
    Xml,

    /// <summary>
    /// Suggests that the field be formatted as JSON text.
    ///
    /// Json is meaningful when applied to fields of type UnicodeString, AnsiString,
    /// CountedString, CountedAnsiString.
    ///
    /// Note that When Json is applied to an AnsiString or CountedAnsiString field,
    /// it implies that the field is encoded as UTF-8.
    /// </summary>
    Json,

    /// <summary>
    /// Suggests that the field be formatted as a WIN32 result code.
    ///
    /// Win32Error is meaningful when applied to fields of type UInt32, HexInt32.
    /// </summary>
    Win32Error,

    /// <summary>
    /// Suggests that the field be formatted as an NTSTATUS result code.
    ///
    /// NtStatus is meaningful when applied to fields of type UInt32, HexInt32.
    /// </summary>
    NtStatus,

    /// <summary>
    /// Suggests that the field be formatted as an HRESULT result code.
    ///
    /// HResult is meaningful when applied to fields of type Int32
    /// (NOT for use with UInt32 or HexInt32).
    /// </summary>
    HResult,

    /// <summary>
    /// Suggests that the field be formatted as a date/time.
    ///
    /// FileTime is the default OutType for fields of type FileTime, SystemTime.
    /// </summary>
    FileTime,

    /// <summary>
    /// Suggests that the field be formatted as a signed decimal integer.
    ///
    /// Signed is the default OutType for fields of type Int8, Int16, Int32, Int64.
    /// </summary>
    Signed,

    /// <summary>
    /// Suggests that the field be formatted as an unsigned decimal integer.
    ///
    /// Unsigned is the default OutType for fields of type UInt8, UInt16, UInt32,
    /// UInt64.
    /// </summary>
    Unsigned,

    /// <summary>
    /// Suggests that the field be formatted as a locale-invariant date/time.
    ///
    /// CultureInsensitiveDateTime is meaningful when applied to fields of type
    /// FileTime, SystemTime.
    /// </summary>
    CultureInsensitiveDateTime = 33,

    /// <summary>
    /// Suggests that the field be formatted as UTF-8 text.
    ///
    /// Utf8 is meaningful when applied to fields of type AnsiString,
    /// CountedAnsiString.
    /// </summary>
    Utf8 = 35,

    /// <summary>
    /// Suggests that the field be formatted as a PKCS-7 message followed by optional
    /// TraceLogging-style event decoding information.
    ///
    /// Pkcs7WithTypeInfo is meaningful when applied to fields of type Binary,
    /// CountedBinary.
    /// </summary>
    Pkcs7WithTypeInfo = 36,

    /// <summary>
    /// Suggests that the field be formatted as an address within the running process
    /// that could potentially be decoded as a symbol.
    ///
    /// CodePointer is meaningful when applied to fields of type UInt32, UInt64,
    /// HexInt32, HexInt64.
    /// </summary>
    CodePointer = 37,

    /// <summary>
    /// Suggests that the field be formatted as a UTC date/time.
    ///
    /// DateTimeUtc is meaningful when applied to fields of type FileTime,
    /// SystemTime.
    /// </summary>
    DateTimeUtc = 38,
}

/// <summary>
/// Used in EventDescriptor. Indicates the severity of an event. Lower numerical
/// value is more severe. Used in filtering, e.g. a consumer might only record events
/// at Warning or higher severity (i.e. Warning or lower numerical value).
/// </summary>
internal enum EventLevel : byte
{
    LogAlways,
    Critical,
    Error,
    Warning,
    Info,
    Verbose
}

/// <summary>
/// Used in EventDescriptor. Indicates special semantics of an event that might be
/// used by the event decoder when organizing events. For example, the Start opcode
/// indicates the beginning of an activity and the Stop opcode indicates the end of
/// an activity. Most events default to Info (0).
/// </summary>
internal enum EventOpcode : byte
{
    Info,
    Start,
    Stop,
    DataCollectionStart,
    DataCollectionStop,
    Extension,
    Reply,
    Resume,
    Suspend,
    Send,
    Receive = 240,
    Reserved241,
    Reserved242,
    Reserved243,
    Reserved244,
    Reserved245,
    Reserved246,
    Reserved247,
    Reserved248,
    Reserved249,
    Reserved250,
    Reserved251,
    Reserved252,
    Reserved253,
    Reserved254,
    Reserved255,
}
