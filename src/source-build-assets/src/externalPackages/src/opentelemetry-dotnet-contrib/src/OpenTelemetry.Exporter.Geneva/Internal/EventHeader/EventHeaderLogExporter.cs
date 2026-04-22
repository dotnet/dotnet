// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET

using System.Globalization;
using Microsoft.LinuxTracepoints.Provider;
using OpenTelemetry.Exporter.Geneva.Tld;
using OpenTelemetry.Exporter.Geneva.Transports;
using OpenTelemetry.Internal;
using OpenTelemetry.Logs;

namespace OpenTelemetry.Exporter.Geneva.EventHeader;

internal class EventHeaderLogExporter : TldLogCommon, IDisposable
{
    internal const int StringLengthLimit = (1 << 14) - 1; // 16 * 1024 - 1 = 16383

    private static readonly ThreadLocal<EventHeaderDynamicBuilder> EventBuilder = new();
    private static readonly Action<LogRecordScope, EventHeaderLogExporter> ProcessScopeForIndividualColumnsAction = OnProcessScopeForIndividualColumns;

    private readonly ValueTuple<byte[], byte[]>? repeatedPartAFields;

#pragma warning disable CA2213 // Disposable fields should be disposed: logsTracepoint is a local reference set in the ctor. It's lifecycle is managed by UnixUserEventsDataTransport and is disposed in EventHeaderDynamicProvider.Dispose
    private readonly EventHeaderDynamicTracepoint logsTracepoint;
#pragma warning restore CA2213 // Disposable fields should be disposed

    public EventHeaderLogExporter(GenevaExporterOptions options)
        : base(options)
    {
        Guard.ThrowIfNull(options);

        this.logsTracepoint = UnixUserEventsDataTransport.Instance.RegisterUserEventProviderForLogs();

        if (options.PrepopulatedFields != null)
        {
            var eb = EventBuilder.Value ??= new EventHeaderDynamicBuilder();

            eb.Reset("_"); // EventName does not matter here as we only need the serialized key-value pairs

            foreach (var entry in options.PrepopulatedFields)
            {
                var key = entry.Key;
                var value = entry.Value;

                if (entry.Key == Schema.V40.PartA.Ver)
                {
                    continue;
                }

                TldExporter.V40_PART_A_TLD_MAPPING.TryGetValue(key, out var replacementKey);
                var keyToSerialize = replacementKey ?? key;
                EventHeaderExporter.Serialize(eb, keyToSerialize, value);
            }

            this.repeatedPartAFields = eb.GetRawFields();
        }
    }

    public ExportResult Export(in Batch<LogRecord> batch)
    {
        if (this.logsTracepoint.IsEnabled)
        {
            var result = ExportResult.Success;
            foreach (var logRecord in batch)
            {
                try
                {
                    var eventBuilder = this.SerializeLogRecord(logRecord);
                    eventBuilder.Write(this.logsTracepoint);
                }
                catch (Exception ex)
                {
                    ExporterEventSource.Log.FailedToSendLogData(ex);
                    result = ExportResult.Failure;
                }
            }

            return result;
        }

        return ExportResult.Failure;
    }

    internal EventHeaderDynamicBuilder SerializeLogRecord(LogRecord logRecord)
    {
        IReadOnlyList<KeyValuePair<string, object?>>? listKvp;

        // `LogRecord.State` and `LogRecord.StateValues` were marked Obsolete in https://github.com/open-telemetry/opentelemetry-dotnet/pull/4334
#pragma warning disable 0618
        if (logRecord.StateValues != null)
        {
            listKvp = logRecord.StateValues;
        }
        else
        {
            // Attempt to see if State could be ROL_KVP.
            listKvp = logRecord.State as IReadOnlyList<KeyValuePair<string, object?>>;
        }
#pragma warning restore 0618

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

        var categoryName = logRecord.CategoryName ?? this.defaultEventName;

        // If user configured explicit TableName, use it.
        if (this.tableMappings?.TryGetValue(categoryName, out var eventName) != true)
        {
            if (!this.shouldPassThruTableMappings)
            {
                eventName = this.defaultEventName;
            }
            else
            {
                // TODO: Avoid allocation
                eventName = GetSanitizedCategoryName(categoryName);
            }
        }

        var eb = EventBuilder.Value ??= new EventHeaderDynamicBuilder(); // TODO: make sure it can be reused with a reset

        var timestamp = logRecord.Timestamp;

        eb.Reset(eventName!);
        eb.AddUInt16("__csver__", 1024, Microsoft.LinuxTracepoints.EventHeaderFieldFormat.HexInt);

        eb.AddStructWithMetadataPosition("PartA", out var partAFieldsCountMetadataPosition);
        EventHeaderExporter.Serialize(eb, "time", timestamp); // TODO: this is different from TldLogExporter due to lack of AddFileTime method.
        if (this.repeatedPartAFields != null)
        {
            eb.AddRawFields(this.repeatedPartAFields.Value);
        }

        var partAFieldsCount = this.partAFieldsCount;

        // Part A - dt extension
        if (logRecord.TraceId != default)
        {
            eb.AddString16("ext_dt_traceId", logRecord.TraceId.ToHexString());
            partAFieldsCount++;
        }

        if (logRecord.SpanId != default)
        {
            eb.AddString16("ext_dt_spanId", logRecord.SpanId.ToHexString());
            partAFieldsCount++;
        }

        // Part A - ex extension
        if (logRecord.Exception != null)
        {
            var fullName = logRecord.Exception.GetType().FullName;
            if (!string.IsNullOrEmpty(fullName))
            {
                eb.AddString16("ext_ex_type", fullName);
                partAFieldsCount++;
            }

            eb.AddString16("ext_ex_msg", logRecord.Exception.Message);
            partAFieldsCount++;

            if (this.exceptionStackExportMode == ExceptionStackExportMode.ExportAsString)
            {
                // The current approach relies on the existing trim
                // capabilities which trims string in excess of STRING_SIZE_LIMIT_CHAR_COUNT
                // TODO: Revisit this:
                // 1. Trim it off based on how much more bytes are available
                // before running out of limit instead of STRING_SIZE_LIMIT_CHAR_COUNT.
                // 2. Trim smarter, by trimming the middle of stack, an
                // keep top and bottom.
                var exceptionStack = logRecord.Exception.ToInvariantString();
                if (exceptionStack.Length > StringLengthLimit)
                {
                    exceptionStack = exceptionStack[..StringLengthLimit];
                }

                eb.AddString16("ext_ex_stack", exceptionStack);
                partAFieldsCount++;
            }
            else if (this.exceptionStackExportMode == ExceptionStackExportMode.ExportAsStackTraceString)
            {
                var stackTrace = logRecord.Exception.StackTrace;
                if (stackTrace != null)
                {
                    if (stackTrace.Length > StringLengthLimit)
                    {
                        stackTrace = stackTrace[..StringLengthLimit];
                    }

                    eb.AddString16("ext_ex_stack", stackTrace);
                    partAFieldsCount++;
                }
            }
        }

        eb.SetStructFieldCount(partAFieldsCountMetadataPosition, partAFieldsCount);

        // Part B

        byte partBFieldsCount = 4; // We at least have three fields in Part B: _typeName, severityText, severityNumber, name
        eb.AddStructWithMetadataPosition("PartB", out var partBFieldsCountMetadataPosition);
        eb.AddString16("_typeName", "Log");

        // `LogRecord.LogLevel` was marked Obsolete in https://github.com/open-telemetry/opentelemetry-dotnet/pull/4568
#pragma warning disable 0618
        var logLevel = logRecord.LogLevel;
#pragma warning restore 0618

        eb.AddString16("severityText", LogLevels[(int)logLevel]);
        eb.AddUInt8("severityNumber", GetSeverityNumber(logLevel));

        var eventId = logRecord.EventId;
        if (eventId != default)
        {
            eb.AddInt32("eventId", eventId.Id);
            partBFieldsCount++;
        }

        byte hasEnvProperties = 0;
        var bodyPopulated = false;
        var namePopulated = false;

        byte partCFieldsCountFromState = 0;
        var kvpArrayForPartCFields = PartCFields.Value ??= new KeyValuePair<string, object>[120];

        List<KeyValuePair<string, object?>>? envPropertiesList = null;

        for (var i = 0; i < listKvp?.Count; i++)
        {
            var entry = listKvp[i];

            // Iteration #1 - Get those fields which become dedicated columns
            // i.e all Part B fields and opt-in Part C fields.
            if (entry.Key == "{OriginalFormat}")
            {
                eb.AddString16(
                    "body",
                    logRecord.FormattedMessage ?? Convert.ToString(entry.Value, CultureInfo.InvariantCulture) ?? string.Empty);
                partBFieldsCount++;
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
                        if (entry.Value is string nameValue)
                        {
                            // name must be string according to Part B in Common Schema. Skip serializing this field otherwise
                            eb.AddString16("name", nameValue);
                            namePopulated = true;
                        }
                    }
                    else
                    {
                        kvpArrayForPartCFields[partCFieldsCountFromState] = new(entry.Key, entry.Value);
                        partCFieldsCountFromState++;
                    }
                }
            }
            else
            {
                if (hasEnvProperties == 0)
                {
                    hasEnvProperties = 1;
                    envPropertiesList = EnvProperties.Value ??= [];

                    envPropertiesList.Clear();
                }

                // TODO: This could lead to unbounded memory usage.
                envPropertiesList!.Add(new(entry.Key, entry.Value));
            }
        }

        if (!namePopulated)
        {
            eb.AddString16("name", categoryName);
        }

        if (!bodyPopulated && logRecord.FormattedMessage != null)
        {
            eb.AddString16("body", logRecord.FormattedMessage);
            partBFieldsCount++;
        }

        eb.SetStructFieldCount(partBFieldsCountMetadataPosition, partBFieldsCount);

        // Part C

        // Prepare state for scopes
        var dataForScopes = this.serializationData.Value ??= new();

        dataForScopes.HasEnvProperties = hasEnvProperties;
        dataForScopes.PartCFieldsCountFromState = partCFieldsCountFromState;

        logRecord.ForEachScope(ProcessScopeForIndividualColumnsAction, this);

        // Update the variables that could have been modified in ProcessScopeForIndividualColumns
        hasEnvProperties = dataForScopes.HasEnvProperties;
        partCFieldsCountFromState = dataForScopes.PartCFieldsCountFromState;

        var partCFieldsCount = (byte)(partCFieldsCountFromState + hasEnvProperties); // We at least have these many fields in Part C

        if (partCFieldsCount > 0)
        {
            eb.AddStructWithMetadataPosition("PartC", out var partCFieldsCountMetadataPosition);

            for (var i = 0; i < partCFieldsCountFromState; i++)
            {
                EventHeaderExporter.Serialize(eb, kvpArrayForPartCFields[i].Key, kvpArrayForPartCFields[i].Value);
            }

            if (hasEnvProperties == 1)
            {
                // Get all "other" fields and collapse them into single field
                // named "env_properties".
                var serializedEnvPropertiesStringAsBytes = JsonSerializer.SerializeKeyValuePairsListAsBytes(envPropertiesList, out var count);
                eb.AddString8("env_properties", serializedEnvPropertiesStringAsBytes);
            }

            eb.SetStructFieldCount(partCFieldsCountMetadataPosition, partCFieldsCount);
        }

        return eb;
    }

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private static void OnProcessScopeForIndividualColumns(LogRecordScope scope, EventHeaderLogExporter state)
        => OnProcessScopeForIndividualColumns(
            scope,
            state.serializationData.Value!,
            state.customFields,
            PartCFields.Value!,
            EnvProperties.Value);
}

#endif
