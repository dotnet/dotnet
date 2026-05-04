// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Collections.Frozen;
#endif
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter.Geneva.Transports;
using OpenTelemetry.Internal;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.Geneva.MsgPack;

internal sealed class MsgPackLogExporter : MsgPackExporter, IDisposable
{
    public const int BUFFER_SIZE = 65360; // the maximum ETW payload (inclusive)

    // This helps tests subscribe to the output of this class
    internal Action<ArraySegment<byte>>? DataTransportListener;

    private static readonly Action<LogRecordScope, MsgPackLogExporter> ProcessScopeForIndividualColumnsAction = OnProcessScopeForIndividualColumns;
    private static readonly Action<LogRecordScope, MsgPackLogExporter> ProcessScopeForEnvPropertiesAction = OnProcessScopeForEnvProperties;
    private static readonly string[] LogLevels =
    [
        "Trace", "Debug", "Information", "Warning", "Error", "Critical", "None"
    ];

    private readonly ThreadLocal<byte[]> buffer = new();
    private readonly bool shouldExportEventName;
    private readonly TableNameSerializer tableNameSerializer;

#if NET
    private readonly FrozenSet<string>? customFields;
#else
    private readonly HashSet<string>? customFields;
#endif

    private readonly ExceptionStackExportMode exportExceptionStack;
    private readonly bool userProvidedPrepopulatedFields;
    private readonly IEnumerable<string>? resourceFieldNames;
    private readonly byte[] bufferEpilogue;
    private readonly IDataTransport dataTransport;
    private readonly Func<Resource> resourceProvider;
    private readonly int stringFieldSizeLimitCharCount; // the maximum string size limit for MsgPack strings

    // This is used for Scopes
    private readonly ThreadLocal<SerializationDataForScopes> serializationData = new();

#if NET
    private FrozenDictionary<string, object>? prepopulatedFields;
#else
    private Dictionary<string, object>? prepopulatedFields;
#endif
    private bool isDisposed;

    public MsgPackLogExporter(GenevaExporterOptions options, Func<Resource> resourceProvider)
    {
        Guard.ThrowIfNull(options);
        Guard.ThrowIfNull(resourceProvider);

        this.resourceProvider = resourceProvider;

        this.tableNameSerializer = new(options, defaultTableName: "Log");
        this.exportExceptionStack = options.ExceptionStackExportMode;

        this.shouldExportEventName = (options.EventNameExportMode & EventNameExportMode.ExportAsPartAName) != 0;

        var connectionStringBuilder = new ConnectionStringBuilder(options.ConnectionString);
        switch (connectionStringBuilder.Protocol)
        {
            case TransportProtocol.Etw:
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    throw new ArgumentException("ETW cannot be used on non-Windows operating systems.");
                }

                this.dataTransport = new EtwDataTransport(connectionStringBuilder.EtwSession);
                break;
            case TransportProtocol.Unix:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    throw new ArgumentException("Unix domain socket should not be used on Windows.");
                }

                var unixDomainSocketPath = connectionStringBuilder.ParseUnixDomainSocketPath();
                this.dataTransport = new UnixDomainSocketDataTransport(unixDomainSocketPath);
                break;
            case TransportProtocol.Tcp:
            case TransportProtocol.Udp:
            case TransportProtocol.EtwTld:
            case TransportProtocol.Unspecified:
            default:
                throw new NotSupportedException($"Protocol '{connectionStringBuilder.Protocol}' is not supported");
        }

        this.stringFieldSizeLimitCharCount = connectionStringBuilder.PrivatePreviewLogMessagePackStringSizeLimit;

        if (options.PrepopulatedFields != null && options.PrepopulatedFields.Count > 0 && options.ResourceFieldNames != null)
        {
            throw new ArgumentException("PrepopulatedFields and ResourceFieldNames are mutually exclusive options");
        }

        if (options.ResourceFieldNames != null)
        {
            foreach (var wantedResourceAttribute in options.ResourceFieldNames)
            {
                if (PART_A_MAPPING_DICTIONARY.Values.Contains(wantedResourceAttribute))
                {
                    throw new ArgumentException($"'{wantedResourceAttribute}' cannot be specified through a resource attribute. Remove it from ResourceFieldNames");
                }
            }

            this.resourceFieldNames = options.ResourceFieldNames;
        }

        this.userProvidedPrepopulatedFields = options.PrepopulatedFields != null && options.PrepopulatedFields.Count > 0;
        if (options.PrepopulatedFields != null)
        {
            var tempPrepopulatedFields = new Dictionary<string, object>(options.PrepopulatedFields.Count, StringComparer.Ordinal);
            foreach (var kv in options.PrepopulatedFields)
            {
                tempPrepopulatedFields[kv.Key] = kv.Value;
            }

#if NET
            this.prepopulatedFields = tempPrepopulatedFields.ToFrozenDictionary(StringComparer.Ordinal);
#else
            this.prepopulatedFields = tempPrepopulatedFields;
#endif
        }

        // TODO: Validate custom fields (reserved name? etc).
        if (options.CustomFields != null)
        {
            var customFields = new HashSet<string>(StringComparer.Ordinal);
            foreach (var name in options.CustomFields)
            {
                customFields.Add(name);
            }

#if NET
            this.customFields = customFields.ToFrozenSet(StringComparer.Ordinal);
#else
            this.customFields = customFields;
#endif
        }

        var buffer = new byte[BUFFER_SIZE];
        var cursor = MessagePackSerializer.Serialize(buffer, 0, new Dictionary<string, object> { { "TimeFormat", "DateTime" } });
        this.bufferEpilogue = new byte[cursor - 0];
        Buffer.BlockCopy(buffer, 0, this.bufferEpilogue, 0, cursor - 0);
    }

    internal bool IsUsingUnixDomainSocket => this.dataTransport is UnixDomainSocketDataTransport;

    public ExportResult Export(in Batch<LogRecord> batch)
    {
        var result = ExportResult.Success;

        foreach (var logRecord in batch)
        {
            try
            {
                var data = this.SerializeLogRecord(logRecord);

                this.DataTransportListener?.Invoke(data);

                this.dataTransport.Send(data.Array!, data.Count);
            }
            catch (Exception ex)
            {
                ExporterEventSource.Log.FailedToSendLogData(ex); // TODO: preallocate exception or no exception
                result = ExportResult.Failure;
            }
        }

        ExporterEventSource.Log.ExportCompleted(nameof(MsgPackLogExporter));

        return result;
    }

    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        try
        {
            (this.dataTransport as IDisposable)?.Dispose();
            this.serializationData.Dispose();
            this.buffer.Dispose();
        }
        catch (Exception ex)
        {
            ExporterEventSource.Log.ExporterException("MsgPackLogExporter Dispose failed.", ex);
        }

        this.isDisposed = true;
    }

    /// <summary>
    /// Updates the prepopulatedFields field to include resource attributes only available at runtime.
    /// This function needs to be idempotent in case it's accidentally called twice.
    /// </summary>
    internal void AddResourceAttributesToPrepopulated()
    {
        var resourceAttributes = this.resourceProvider().Attributes;

        var tempPrepopulatedFields = this.prepopulatedFields != null
            ? new Dictionary<string, object>(this.prepopulatedFields, StringComparer.Ordinal)
            : new Dictionary<string, object>(StringComparer.Ordinal);
        foreach (var resourceAttribute in resourceAttributes)
        {
            var key = resourceAttribute.Key;
            var value = resourceAttribute.Value;

            var isWantedAttribute = false;
            if (this.resourceFieldNames != null)
            {
                // this might seem inefficient, but it's only run once and I don't expect there to be many resource attributes
                foreach (var wantedAttribute in this.resourceFieldNames!)
                {
                    if (wantedAttribute == key)
                    {
                        switch (value)
                        {
                            case bool:
                            case byte:
                            case sbyte:
                            case short:
                            case ushort:
                            case int:
                            case uint:
                            case long:
                            case ulong:
                            case float:
                            case double:
                            case string:
                                break;
                            case null:
                                // This should be impossible because Resource attributes cannot have null values.
                                // But just in case, turn it into something serializable to avoid crashing.
                                value = "<NULL>";
                                break;
                            default:
                                // Try to construct a value that communicates that the type is not supported.
                                try
                                {
                                    var stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                                    value = stringValue == null ? "<Unsupported type>" : $"<Unsupported type: {stringValue}>";
                                }
                                catch
                                {
                                    value = "<Unsupported type>";
                                }

                                break;
                        }

                        isWantedAttribute = true;
                        break;
                    }
                }
            }

            if (!this.userProvidedPrepopulatedFields)
            {
                // it's only safe to add these special resource fields if we are sure the user didn't provide them as a PrepopulatedField already
                if (key == "service.name")
                {
                    key = Schema.V40.PartA.Extensions.Cloud.Role;
                    isWantedAttribute = true;
                }

                if (key == "service.instanceId")
                {
                    key = Schema.V40.PartA.Extensions.Cloud.RoleInstance;
                    isWantedAttribute = true;
                }
            }

            if (isWantedAttribute)
            {
                tempPrepopulatedFields[key] = value;
            }
        }

#if NET
        this.prepopulatedFields = tempPrepopulatedFields.ToFrozenDictionary(StringComparer.Ordinal);
#else
        this.prepopulatedFields = tempPrepopulatedFields;
#endif
    }

    internal ArraySegment<byte> SerializeLogRecord(LogRecord logRecord)
    {
        // `LogRecord.State` and `LogRecord.StateValues` were marked Obsolete in https://github.com/open-telemetry/opentelemetry-dotnet/pull/4334
#pragma warning disable 0618
        IReadOnlyList<KeyValuePair<string, object?>>? logFields;
        if (logRecord.StateValues != null)
        {
            logFields = logRecord.StateValues;
        }
        else
        {
            // Attempt to see if State could be ROL_KVP.
            logFields = logRecord.State as IReadOnlyList<KeyValuePair<string, object?>>;
        }
#pragma warning restore 0618

        var buffer = this.buffer.Value;
        if (buffer == null)
        {
            this.AddResourceAttributesToPrepopulated();
            buffer = new byte[BUFFER_SIZE]; // TODO: handle OOM
            this.buffer.Value = buffer;
        }

        /* Fluentd Forward Mode:
        [
            "Log", // (or category name)
            [
                [ <timestamp>, { "env_ver": "4.0", ... } ]
            ],
            { "TimeFormat": "DateTime" }
        ]
        */

        // Structured log.
        // 2 scenarios.
        // 1. Structured logging with template
        // eg:
        // body
        // "Hello from {food} {price}."
        // part c
        // food = onion
        // price = 100
        // TODO: 2. Structured with strongly typed logging.
        var timestamp = logRecord.Timestamp;
        var cursor = 0;
        cursor = MessagePackSerializer.WriteArrayHeader(buffer, cursor, 3);

        var categoryName = logRecord.CategoryName ?? "Log";

        cursor = this.tableNameSerializer.ResolveAndSerializeTableNameForCategoryName(buffer, cursor, categoryName, out var eventName);

        cursor = MessagePackSerializer.WriteArrayHeader(buffer, cursor, 1);
        cursor = MessagePackSerializer.WriteArrayHeader(buffer, cursor, 2);
        cursor = MessagePackSerializer.SerializeUtcDateTime(buffer, cursor, timestamp);
        cursor = MessagePackSerializer.WriteMapHeader(buffer, cursor, ushort.MaxValue); // Note: always use Map16 for perf consideration
        ushort cntFields = 0;
        var idxMapSizePatch = cursor - 2;

        if (this.prepopulatedFields != null)
        {
            foreach (var field in this.prepopulatedFields)
            {
                cursor = AddPartAField(buffer, cursor, field.Key, field.Value);
                cntFields += 1;
            }
        }

        // Part A - core envelope

        var eventId = logRecord.EventId;
        var hasEventId = eventId != default;

        if (hasEventId && this.shouldExportEventName && !string.IsNullOrWhiteSpace(eventId.Name))
        {
            // Export `eventId.Name` as the value for `env_name`
            cursor = AddPartAField(buffer, cursor, Schema.V40.PartA.Name, eventId.Name);
            cntFields += 1;
        }
        else
        {
            // Export the table name as the value for `env_name`
            cursor = AddPartAField(buffer, cursor, Schema.V40.PartA.Name, eventName);
            cntFields += 1;
        }

        cursor = AddPartAField(buffer, cursor, Schema.V40.PartA.Ver, "4.0");
        cntFields += 1;

        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_time");
        cursor = MessagePackSerializer.SerializeUtcDateTime(buffer, cursor, timestamp); // LogRecord.Timestamp should already be converted to UTC format in the SDK
        cntFields += 1;

        // Part A - dt extension
        if (logRecord.TraceId != default)
        {
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_dt_traceId");

            // Note: ToHexString returns the pre-calculated hex representation without allocation
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, logRecord.TraceId.ToHexString());
            cntFields += 1;
        }

        if (logRecord.SpanId != default)
        {
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_dt_spanId");
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, logRecord.SpanId.ToHexString());
            cntFields += 1;
        }

        // Part B

        // `LogRecord.LogLevel` was marked Obsolete in https://github.com/open-telemetry/opentelemetry-dotnet/pull/4568
#pragma warning disable 0618
        var logLevel = logRecord.LogLevel;
#pragma warning restore 0618

        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "severityText");
        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, LogLevels[(int)logLevel]);
        cntFields += 1;

        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "severityNumber");
        cursor = MessagePackSerializer.SerializeUInt8(buffer, cursor, GetSeverityNumber(logLevel));
        cntFields += 1;

        var hasEnvProperties = false;
        var bodyPopulated = false;
        var namePopulated = false;
        for (var i = 0; i < logFields?.Count; i++)
        {
            var entry = logFields[i];

            // Iteration #1 - Get those fields which become dedicated columns
            // i.e all Part B fields and opt-in Part C fields.
            if (entry.Key == "{OriginalFormat}")
            {
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "body");
                cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, logRecord.FormattedMessage ?? Convert.ToString(entry.Value, CultureInfo.InvariantCulture), this.stringFieldSizeLimitCharCount);
                cntFields += 1;
                bodyPopulated = true;
                continue;
            }
            else if (this.customFields == null || this.customFields.Contains(entry.Key))
            {
                // TODO: the above null check can be optimized and avoided inside foreach.
                if (entry.Value != null)
                {
                    // null is not supported.
                    if (string.Equals(entry.Key, "name", StringComparison.Ordinal))
                    {
                        if (entry.Value is not string)
                        {
                            // name must be string according to Part B in Common Schema. Skip serializing this field otherwise
                            continue;
                        }

                        namePopulated = true;
                    }

                    cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, entry.Key, this.stringFieldSizeLimitCharCount);
                    cursor = this.SerializeValueWithLimitIfString(buffer, cursor, entry.Value);
                    cntFields += 1;
                }
            }
            else
            {
                hasEnvProperties = true;
                continue;
            }
        }

        if (!namePopulated)
        {
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "name");
            cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, categoryName, this.stringFieldSizeLimitCharCount);
            cntFields += 1;
        }

        if (!bodyPopulated && logRecord.FormattedMessage != null)
        {
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "body");
            cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, logRecord.FormattedMessage, this.stringFieldSizeLimitCharCount);
            cntFields += 1;
        }

        // Prepare state for scopes
        var dataForScopes = this.serializationData.Value ??= new(buffer);

        dataForScopes.Cursor = cursor;
        dataForScopes.FieldsCount = cntFields;
        dataForScopes.HasEnvProperties = hasEnvProperties;

        logRecord.ForEachScope(ProcessScopeForIndividualColumnsAction, this);

        // Update the variables that could have been modified in ProcessScopeForIndividualColumns
        hasEnvProperties = dataForScopes.HasEnvProperties;
        cursor = dataForScopes.Cursor;
        cntFields = dataForScopes.FieldsCount;

        if (hasEnvProperties)
        {
            // Iteration #2 - Get all "other" fields and collapse them into single field
            // named "env_properties".
            ushort envPropertiesCount = 0;
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_properties");
            cursor = MessagePackSerializer.WriteMapHeader(buffer, cursor, ushort.MaxValue);
            var idxMapSizeEnvPropertiesPatch = cursor - 2;
            for (var i = 0; i < logFields!.Count; i++)
            {
                var entry = logFields[i];
                if (entry.Key == "{OriginalFormat}" || this.customFields!.Contains(entry.Key))
                {
                    continue;
                }
                else
                {
                    cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, entry.Key, this.stringFieldSizeLimitCharCount);
                    cursor = MessagePackSerializer.Serialize(buffer, cursor, entry.Value);
                    envPropertiesCount += 1;
                }
            }

            // Prepare state for scopes
            dataForScopes.Cursor = cursor;
            dataForScopes.EnvPropertiesCount = envPropertiesCount;

            logRecord.ForEachScope(ProcessScopeForEnvPropertiesAction, this);

            // Update the variables that could have been modified in ProcessScopeForEnvProperties
            cursor = dataForScopes.Cursor;
            envPropertiesCount = dataForScopes.EnvPropertiesCount;

            cntFields += 1;
            MessagePackSerializer.WriteUInt16(buffer, idxMapSizeEnvPropertiesPatch, envPropertiesCount);
        }

        if (hasEventId)
        {
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "eventId");
            cursor = MessagePackSerializer.SerializeInt32(buffer, cursor, eventId.Id);
            cntFields += 1;
        }

        // Part A - ex extension
        if (logRecord.Exception != null)
        {
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_ex_type");
            cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, logRecord.Exception.GetType().FullName, this.stringFieldSizeLimitCharCount);
            cntFields += 1;

            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_ex_msg");
            cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, logRecord.Exception.Message, this.stringFieldSizeLimitCharCount);
            cntFields += 1;

            // The current approach relies on the existing trim
            // capabilities which trims string in excess of STRING_SIZE_LIMIT_CHAR_COUNT
            // TODO: Revisit this:
            // 1. Trim it off based on how much more bytes are available
            // before running out of limit instead of STRING_SIZE_LIMIT_CHAR_COUNT.
            // 2. Trim smarter, by trimming the middle of stack, an
            // keep top and bottom.
            if (this.exportExceptionStack == ExceptionStackExportMode.ExportAsString)
            {
                var exceptionStack = logRecord.Exception.ToInvariantString();
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_ex_stack");
                cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, exceptionStack, this.stringFieldSizeLimitCharCount);
                cntFields += 1;
            }
            else if (this.exportExceptionStack == ExceptionStackExportMode.ExportAsStackTraceString)
            {
                var exceptionStack = logRecord.Exception.StackTrace;
                if (exceptionStack != null)
                {
                    cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_ex_stack");
                    cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, exceptionStack, this.stringFieldSizeLimitCharCount);
                    cntFields += 1;
                }
            }
        }

        MessagePackSerializer.WriteUInt16(buffer, idxMapSizePatch, cntFields);
        Buffer.BlockCopy(this.bufferEpilogue, 0, buffer, cursor, this.bufferEpilogue.Length);
        cursor += this.bufferEpilogue.Length;
        return new(buffer, 0, cursor);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte GetSeverityNumber(LogLevel logLevel)
    {
        // Maps the Ilogger LogLevel to OpenTelemetry logging level.
        // https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/logs/data-model.md#mapping-of-severitynumber
        // TODO: for improving perf simply do ((int)loglevel * 4) + 1
        // or ((int)logLevel << 2) + 1
        return logLevel switch
        {
            LogLevel.Trace => 1,
            LogLevel.Debug => 5,
            LogLevel.Information => 9,
            LogLevel.Warning => 13,
            LogLevel.Error => 17,
            LogLevel.Critical => 21,

            // we reach default only for LogLevel.None
            // but that is filtered out anyway.
            // should we throw here then?
            LogLevel.None => 1,
            _ => 1,
        };
    }

    private static void OnProcessScopeForIndividualColumns(LogRecordScope scope, MsgPackLogExporter state)
    {
        Debug.Assert(state.serializationData.Value != null, "state.serializationData.Value was null");

        var stateData = state.serializationData.Value!;
        var customFields = state.customFields;

        foreach (var scopeItem in scope)
        {
            if (string.IsNullOrEmpty(scopeItem.Key) || scopeItem.Key == "{OriginalFormat}")
            {
                continue;
            }

            if (customFields == null || customFields.Contains(scopeItem.Key))
            {
                if (scopeItem.Value != null)
                {
                    // null is not supported.
                    stateData.Cursor = MessagePackSerializer.SerializeUnicodeString(stateData.Buffer, stateData.Cursor, scopeItem.Key);
                    stateData.Cursor = MessagePackSerializer.Serialize(stateData.Buffer, stateData.Cursor, scopeItem.Value);
                    stateData.FieldsCount += 1;
                }
            }
            else
            {
                stateData.HasEnvProperties = true;
            }
        }
    }

    private static void OnProcessScopeForEnvProperties(LogRecordScope scope, MsgPackLogExporter state)
    {
        Debug.Assert(state.serializationData.Value != null, "state.serializationData.Value was null");

        var stateData = state.serializationData.Value!;
        var customFields = state.customFields;

        foreach (var scopeItem in scope)
        {
            if (string.IsNullOrEmpty(scopeItem.Key) || scopeItem.Key == "{OriginalFormat}")
            {
                continue;
            }

            if (!customFields!.Contains(scopeItem.Key))
            {
                stateData.Cursor = MessagePackSerializer.SerializeUnicodeString(stateData.Buffer, stateData.Cursor, scopeItem.Key);
                stateData.Cursor = MessagePackSerializer.Serialize(stateData.Buffer, stateData.Cursor, scopeItem.Value);
                stateData.EnvPropertiesCount += 1;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int SerializeValueWithLimitIfString(byte[] buffer, int cursor, object? value)
    {
        if (value is string stringValue)
        {
            return MessagePackSerializer.SerializeUnicodeString(buffer, cursor, stringValue, this.stringFieldSizeLimitCharCount);
        }

        return MessagePackSerializer.Serialize(buffer, cursor, value);
    }

    private sealed class SerializationDataForScopes
    {
        public readonly byte[] Buffer;
        public int Cursor;
        public ushort FieldsCount;
        public bool HasEnvProperties;
        public ushort EnvPropertiesCount;

        public SerializationDataForScopes(byte[] buffer)
        {
            this.Buffer = buffer;
        }
    }
}
