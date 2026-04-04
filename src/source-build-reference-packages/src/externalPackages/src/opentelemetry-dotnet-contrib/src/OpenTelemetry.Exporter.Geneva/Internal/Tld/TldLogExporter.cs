// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Globalization;
using System.Text;
using OpenTelemetry.Exporter.Geneva.External;
using OpenTelemetry.Internal;
using OpenTelemetry.Logs;

namespace OpenTelemetry.Exporter.Geneva.Tld;

internal sealed class TldLogExporter : TldLogCommon, IDisposable
{
    // TODO: Is using a single ThreadLocal a better idea?
    private static readonly ThreadLocal<EventBuilder> EventBuilder = new();
    private static readonly Action<LogRecordScope, TldLogExporter> ProcessScopeForIndividualColumnsAction = OnProcessScopeForIndividualColumns;

    private readonly Tuple<byte[], byte[]>? repeatedPartAFields;
    private readonly EventProvider eventProvider;

    private bool isDisposed;

    public TldLogExporter(GenevaExporterOptions options)
        : base(options)
    {
        Guard.ThrowIfNull(options);

        var connectionStringBuilder = new ConnectionStringBuilder(options.ConnectionString);
        this.eventProvider = new EventProvider(connectionStringBuilder.EtwSession);

        if (options.PrepopulatedFields != null)
        {
            var eb = EventBuilder.Value ??= new EventBuilder(UncheckedASCIIEncoding.SharedInstance);

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
                TldExporter.Serialize(eb, keyToSerialize, value);

                this.repeatedPartAFields = eb.GetRawFields();
            }
        }
    }

    public ExportResult Export(in Batch<LogRecord> batch)
    {
        if (this.eventProvider.IsEnabled())
        {
            var result = ExportResult.Success;

            foreach (var logRecord in batch)
            {
                try
                {
                    var eventBuilder = this.SerializeLogRecord(logRecord);

                    this.eventProvider.Write(eventBuilder);
                }
                catch (Exception ex)
                {
                    ExporterEventSource.Log.FailedToSendLogData(ex); // TODO: preallocate exception or no exception
                    result = ExportResult.Failure;
                }
            }

            return result;
        }

        return ExportResult.Failure;
    }

    internal EventBuilder SerializeLogRecord(LogRecord logRecord)
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

        var eb = EventBuilder.Value ??= new EventBuilder(UncheckedASCIIEncoding.SharedInstance);

        var timestamp = logRecord.Timestamp;

        eb.Reset(eventName!);
        eb.AddUInt16("__csver__", 1024, EventOutType.Hex);

        var partAFieldsCountPatch = eb.AddStruct("PartA", this.partAFieldsCount);
        eb.AddFileTime("time", timestamp);
        if (this.repeatedPartAFields != null)
        {
            eb.AppendRawFields(this.repeatedPartAFields);
        }

        var partAFieldsCount = this.partAFieldsCount;

        // Part A - dt extension
        if (logRecord.TraceId != default)
        {
            eb.AddCountedString("ext_dt_traceId", logRecord.TraceId.ToHexString());
            partAFieldsCount++;
        }

        if (logRecord.SpanId != default)
        {
            eb.AddCountedString("ext_dt_spanId", logRecord.SpanId.ToHexString());
            partAFieldsCount++;
        }

        // Part A - ex extension
        if (logRecord.Exception != null)
        {
            var fullName = logRecord.Exception.GetType().FullName;
            if (!string.IsNullOrEmpty(fullName))
            {
                eb.AddCountedAnsiString("ext_ex_type", fullName, Encoding.UTF8);
            }

            eb.AddCountedAnsiString("ext_ex_msg", logRecord.Exception.Message, Encoding.UTF8);

            partAFieldsCount += 2;

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
                eb.AddCountedAnsiString("ext_ex_stack", exceptionStack, Encoding.UTF8, 0, Math.Min(exceptionStack.Length, TldExporter.StringLengthLimit));
                partAFieldsCount++;
            }
            else if (this.exceptionStackExportMode == ExceptionStackExportMode.ExportAsStackTraceString)
            {
                var stackTrace = logRecord.Exception.StackTrace;
                if (stackTrace != null)
                {
                    eb.AddCountedAnsiString("ext_ex_stack", stackTrace, Encoding.UTF8, 0, Math.Min(stackTrace.Length, TldExporter.StringLengthLimit));
                    partAFieldsCount++;
                }
            }
        }

        eb.SetStructFieldCount(partAFieldsCountPatch, partAFieldsCount);

        // Part B

        byte partBFieldsCount = 4;
        var partBFieldsCountPatch = eb.AddStruct("PartB", partBFieldsCount); // We at least have three fields in Part B: _typeName, severityText, severityNumber, name
        eb.AddCountedString("_typeName", "Log");

        // `LogRecord.LogLevel` was marked Obsolete in https://github.com/open-telemetry/opentelemetry-dotnet/pull/4568
#pragma warning disable 0618
        var logLevel = logRecord.LogLevel;
#pragma warning restore 0618

        eb.AddCountedString("severityText", LogLevels[(int)logLevel]);
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
                eb.AddCountedAnsiString(
                    "body",
                    logRecord.FormattedMessage ?? Convert.ToString(entry.Value, CultureInfo.InvariantCulture) ?? string.Empty,
                    Encoding.UTF8);
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
                            eb.AddCountedAnsiString("name", nameValue, Encoding.UTF8);
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
            eb.AddCountedAnsiString("name", categoryName, Encoding.UTF8);
        }

        if (!bodyPopulated && logRecord.FormattedMessage != null)
        {
            eb.AddCountedAnsiString("body", logRecord.FormattedMessage, Encoding.UTF8);
            partBFieldsCount++;
        }

        eb.SetStructFieldCount(partBFieldsCountPatch, partBFieldsCount);

        // Part C

        // Prepare state for scopes
        var dataForScopes = this.serializationData.Value ??= new();

        dataForScopes.HasEnvProperties = hasEnvProperties;
        dataForScopes.PartCFieldsCountFromState = partCFieldsCountFromState;

        logRecord.ForEachScope(ProcessScopeForIndividualColumnsAction, this);

        // Update the variables that could have been modified in ProcessScopeForIndividualColumns
        hasEnvProperties = dataForScopes.HasEnvProperties;
        partCFieldsCountFromState = dataForScopes.PartCFieldsCountFromState;

        var partCFieldsCount = partCFieldsCountFromState + hasEnvProperties; // We at least have these many fields in Part C

        if (partCFieldsCount > 0)
        {
            var partCFieldsCountPatch = eb.AddStruct("PartC", (byte)partCFieldsCount);

            for (var i = 0; i < partCFieldsCountFromState; i++)
            {
                TldExporter.Serialize(eb, kvpArrayForPartCFields[i].Key, kvpArrayForPartCFields[i].Value);
            }

            if (hasEnvProperties == 1)
            {
                // Get all "other" fields and collapse them into single field
                // named "env_properties".
                var serializedEnvPropertiesStringAsBytes = JsonSerializer.SerializeKeyValuePairsListAsBytes(envPropertiesList, out var count);
                eb.AddCountedAnsiString("env_properties", serializedEnvPropertiesStringAsBytes, 0, count);
            }

            eb.SetStructFieldCount(partCFieldsCountPatch, (byte)partCFieldsCount);
        }

        return eb;
    }

    protected override void Dispose(bool disposing)
    {
        if (!this.isDisposed)
        {
            if (disposing)
            {
                // Dispose managed resources specific to TldLogExporter
                try
                {
                    // DO NOT Dispose eventBuilder, keyValuePairs, and partCFields as they are static
                    this.eventProvider.Dispose();
                }
                catch (Exception ex)
                {
                    ExporterEventSource.Log.ExporterException("TldLogExporter Dispose failed.", ex);
                }
            }

            // Dispose unmanaged resources specific to TldLogExporter

            this.isDisposed = true;
        }

        base.Dispose(disposing);
    }

    private static void OnProcessScopeForIndividualColumns(LogRecordScope scope, TldLogExporter state)
        => OnProcessScopeForIndividualColumns(
            scope,
            state.serializationData.Value!,
            state.customFields,
            PartCFields.Value!,
            EnvProperties.Value);
}
