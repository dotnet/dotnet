// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Globalization;
using System.Text;
using OpenTelemetry.Exporter.Geneva.External;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Exporter.Geneva.Tld;

internal sealed class TldTraceExporter : IDisposable
{
    // TODO: Is using a single ThreadLocal a better idea?
    private static readonly ThreadLocal<EventBuilder> EventBuilder = new();
    private static readonly ThreadLocal<List<KeyValuePair<string, object?>>> KeyValuePairs = new();
    private static readonly ThreadLocal<KeyValuePair<string, object>[]> PartCFields = new(); // This is used to temporarily store the PartC fields from tags

    private static readonly string INVALID_SPAN_ID = default(ActivitySpanId).ToHexString();

    private static readonly Dictionary<string, string> CS40_PART_B_MAPPING = new()
    {
        ["db.system"] = "dbSystem",
        ["db.name"] = "dbName",
        ["db.statement"] = "dbStatement",

        ["http.method"] = "httpMethod",
        ["http.request.method"] = "httpMethod",
        ["http.url"] = "httpUrl",
        ["url.full"] = "httpUrl",
        ["http.status_code"] = "httpStatusCode",
        ["http.response.status_code"] = "httpStatusCode",

        ["messaging.system"] = "messagingSystem",
        ["messaging.destination"] = "messagingDestination",
        ["messaging.url"] = "messagingUrl",
    };

    private readonly string partAName = "Span";
    private readonly byte partAFieldsCount = 3; // At least three fields: time, ext_dt_traceId, ext_dt_spanId
    private readonly HashSet<string>? customFields;
    private readonly Tuple<byte[], byte[]>? repeatedPartAFields;
    private readonly bool shouldIncludeTraceState;
    private readonly EventProvider eventProvider;

    private bool isDisposed;

    public TldTraceExporter(GenevaExporterOptions options)
    {
        Guard.ThrowIfNull(options);

        var connectionStringBuilder = new ConnectionStringBuilder(options.ConnectionString);
        this.eventProvider = new EventProvider(connectionStringBuilder.EtwSession);

        if (options.TableNameMappings != null
            && options.TableNameMappings.TryGetValue("Span", out var customTableName))
        {
            this.partAName = customTableName;
        }

        // TODO: Validate custom fields (reserved name? etc).
        if (options.CustomFields != null)
        {
            var customFields = new HashSet<string>(StringComparer.Ordinal)
            {
                // Seed customFields with Span PartB
                "azureResourceProvider",
            };

            foreach (var mapping in CS40_PART_B_MAPPING)
            {
                customFields.Add(mapping.Key);
            }

            foreach (var name in options.CustomFields)
            {
                customFields.Add(name);
            }

            this.customFields = customFields;
        }

        if (options.PrepopulatedFields != null)
        {
            var prePopulatedFieldsCount = (byte)(options.PrepopulatedFields.Count - 1); // PrepopulatedFields option has the key ".ver" added to it which is not needed for TLD
            this.partAFieldsCount += prePopulatedFieldsCount;

            var eb = EventBuilder.Value ??= new EventBuilder(UncheckedASCIIEncoding.SharedInstance);

            eb.Reset(this.partAName);

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

        this.shouldIncludeTraceState = options.IncludeTraceStateForSpan;
    }

    public ExportResult Export(in Batch<Activity> batch)
    {
        if (this.eventProvider.IsEnabled())
        {
            var result = ExportResult.Success;

            foreach (var activity in batch)
            {
                try
                {
                    var eventBuilder = this.SerializeActivity(activity);

                    this.eventProvider.Write(eventBuilder);
                }
                catch (Exception ex)
                {
                    ExporterEventSource.Log.FailedToSendTraceData(ex); // TODO: preallocate exception or no exception
                    result = ExportResult.Failure;
                }
            }

            return result;
        }

        return ExportResult.Failure;
    }

    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        try
        {
            // DO NOT Dispose eventBuilder, keyValuePairs, and partCFields as they are static
            this.eventProvider.Dispose();
        }
        catch (Exception ex)
        {
            ExporterEventSource.Log.ExporterException("TldTraceExporter Dispose failed.", ex);
        }

        this.isDisposed = true;
    }

    internal EventBuilder SerializeActivity(Activity activity)
    {
        var eb = EventBuilder.Value ??= new EventBuilder(UncheckedASCIIEncoding.SharedInstance);

        eb.Reset(this.partAName);
        eb.AddUInt16("__csver__", 1024, EventOutType.Hex);

        var dtBegin = activity.StartTimeUtc;
        var tsBegin = dtBegin.Ticks;
        var tsEnd = tsBegin + activity.Duration.Ticks;
        var dtEnd = new DateTime(tsEnd);

        eb.AddStruct("PartA", this.partAFieldsCount);
        eb.AddFileTime("time", dtEnd);
        eb.AddCountedString("ext_dt_traceId", activity.Context.TraceId.ToHexString());
        eb.AddCountedString("ext_dt_spanId", activity.Context.SpanId.ToHexString());
        if (this.repeatedPartAFields != null)
        {
            eb.AppendRawFields(this.repeatedPartAFields);
        }

        byte partBFieldsCount = 5;
        var partBFieldsCountPatch = eb.AddStruct("PartB", partBFieldsCount); // We at least have five fields in PartB: _typeName, name, kind, startTime, success
        eb.AddCountedString("_typeName", "Span");
        eb.AddCountedAnsiString("name", activity.DisplayName, Encoding.UTF8);
        eb.AddUInt8("kind", (byte)activity.Kind);
        eb.AddFileTime("startTime", dtBegin);

        var strParentId = activity.ParentSpanId.ToHexString();

        // Note: this should be blazing fast since Object.ReferenceEquals(strParentId, INVALID_SPAN_ID) == true
        if (!string.Equals(strParentId, INVALID_SPAN_ID, StringComparison.Ordinal))
        {
            eb.AddCountedString("parentId", strParentId);
            partBFieldsCount++;
        }

        if (this.shouldIncludeTraceState)
        {
            var traceStateString = activity.TraceStateString;
            if (!string.IsNullOrEmpty(traceStateString))
            {
                eb.AddCountedAnsiString("traceState", traceStateString!, Encoding.UTF8);
                partBFieldsCount++;
            }
        }

        var linkEnumerator = activity.EnumerateLinks();
        if (linkEnumerator.MoveNext())
        {
            var keyValuePairsForLinks = KeyValuePairs.Value ??= [];

            keyValuePairsForLinks.Clear();

            do
            {
                ref readonly var link = ref linkEnumerator.Current;

                // TODO: This could lead to unbounded memory usage.
                keyValuePairsForLinks.Add(new("toTraceId", link.Context.TraceId.ToHexString()));
                keyValuePairsForLinks.Add(new("toSpanId", link.Context.SpanId.ToHexString()));
            }
            while (linkEnumerator.MoveNext());

            var serializedLinksStringAsBytes = JsonSerializer.SerializeKeyValuePairsListAsBytes(keyValuePairsForLinks, out var count);
            eb.AddCountedAnsiString("links", serializedLinksStringAsBytes, 0, count);

            partBFieldsCount++;
        }

        byte hasEnvProperties = 0;
        byte isStatusSuccess = 1;
        var statusDescription = string.Empty;

        var partCFieldsCountFromTags = 0;
        var kvpArrayForPartCFields = PartCFields.Value ??= new KeyValuePair<string, object>[120];

        List<KeyValuePair<string, object?>>? envPropertiesList = null;

        foreach (ref readonly var entry in activity.EnumerateTagObjects())
        {
            if (entry.Value == null)
            {
                continue;
            }

            // TODO: check name collision
            if (CS40_PART_B_MAPPING.TryGetValue(entry.Key, out var replacementKey))
            {
                TldExporter.Serialize(eb, replacementKey, entry.Value);
                partBFieldsCount++;
                continue;
            }

            if (entry.Key.StartsWith("otel.status_", StringComparison.Ordinal))
            {
                var keyPart = entry.Key.AsSpan().Slice(12);
                if (keyPart is "code")
                {
                    if (string.Equals(Convert.ToString(entry.Value, CultureInfo.InvariantCulture), "ERROR", StringComparison.Ordinal))
                    {
                        isStatusSuccess = 0;
                    }

                    continue;
                }

                if (keyPart is "description")
                {
                    statusDescription = Convert.ToString(entry.Value, CultureInfo.InvariantCulture) ?? string.Empty;
                    continue;
                }
            }

            if (this.customFields == null || this.customFields.Contains(entry.Key))
            {
                // TODO: the above null check can be optimized and avoided inside foreach.
                kvpArrayForPartCFields[partCFieldsCountFromTags] = new(entry.Key, entry.Value);
                partCFieldsCountFromTags++;
                continue;
            }

            if (hasEnvProperties == 0)
            {
                hasEnvProperties = 1;

                envPropertiesList = KeyValuePairs.Value ??= [];

                envPropertiesList.Clear();
            }

            // TODO: This could lead to unbounded memory usage.
            envPropertiesList!.Add(new(entry.Key, entry.Value));
        }

        if (activity.Status != ActivityStatusCode.Unset)
        {
            if (activity.Status == ActivityStatusCode.Error)
            {
                isStatusSuccess = 0;
            }

            if (!string.IsNullOrEmpty(activity.StatusDescription))
            {
                eb.AddCountedAnsiString("statusMessage", statusDescription!, Encoding.UTF8);
                partBFieldsCount++;
            }
        }
        else if (!string.IsNullOrEmpty(activity.StatusDescription))
        {
            eb.AddCountedAnsiString("statusMessage", statusDescription!, Encoding.UTF8);
            partBFieldsCount++;
        }

        // Do not increment partBFieldsCount here as the field "success" has already been accounted for
        eb.AddUInt8("success", isStatusSuccess, EventOutType.Boolean);

        eb.SetStructFieldCount(partBFieldsCountPatch, partBFieldsCount);

        var partCFieldsCount = partCFieldsCountFromTags + hasEnvProperties;

        if (partCFieldsCount > 0)
        {
            eb.AddStruct("PartC", (byte)partCFieldsCount);

            for (var i = 0; i < partCFieldsCountFromTags; i++)
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
        }

        return eb;
    }
}
