// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Collections.Frozen;
#endif
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using OpenTelemetry.Exporter.Geneva.Transports;
using OpenTelemetry.Internal;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.Geneva.MsgPack;

internal sealed class MsgPackTraceExporter : MsgPackExporter, IDisposable
{
    internal static readonly Dictionary<string, string> CS40_PART_B_MAPPING_DICTIONARY = new()
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

    internal static readonly Dictionary<string, int> CS40_PART_B_HTTPURL_MAPPING_DICTIONARY = new()
    {
        // Mapping from HTTP semconv to httpUrl
        // Combination of url.scheme, server.address, server.port, url.path and url.query attributes for HTTP server spans
        ["url.scheme"] = 0,
        ["server.address"] = 1,
        ["server.port"] = 2,
        ["url.path"] = 3,
        ["url.query"] = 4,
    };

#if NET
    internal static readonly FrozenDictionary<string, string> CS40_PART_B_MAPPING = CS40_PART_B_MAPPING_DICTIONARY.ToFrozenDictionary();
    internal static readonly FrozenDictionary<string, int> CS40_PART_B_HTTPURL_MAPPING = CS40_PART_B_HTTPURL_MAPPING_DICTIONARY.ToFrozenDictionary();
#else
    internal static readonly Dictionary<string, string> CS40_PART_B_MAPPING = CS40_PART_B_MAPPING_DICTIONARY;
    internal static readonly Dictionary<string, int> CS40_PART_B_HTTPURL_MAPPING = CS40_PART_B_HTTPURL_MAPPING_DICTIONARY;
#endif

    internal readonly ThreadLocal<byte[]> Buffer = new();
    internal readonly ThreadLocal<object?[]> HttpUrlParts = new();

#if NET
    internal readonly FrozenSet<string>? CustomFields;
    internal readonly FrozenSet<string>? DedicatedFields;
#else
    internal readonly HashSet<string>? CustomFields;
    internal readonly HashSet<string>? DedicatedFields;
#endif

    private const int BUFFER_SIZE = 65360; // the maximum ETW payload (inclusive)

    private static readonly string INVALID_SPAN_ID = default(ActivitySpanId).ToHexString();

    private readonly IDataTransport dataTransport;

    // this is a reference to the eponymous options field.
    // It would make more sense as a HashSet/FrozenSet, but it's only used once,
    // so constructing a whole new data structure for it is overkill.
    private readonly IEnumerable<string>? resourceFieldNames;
    private readonly bool shouldIncludeTraceState;
    private readonly bool userProvidedPrepopulatedFields;
    private readonly string partAName;
    private readonly Func<Resource> resourceProvider;

    private byte[]? bufferPrologue;
    private byte[]? bufferEpilogue;
    private int timestampPatchIndex;
    private int mapSizePatchIndex;

    // this stores the prepopulated fields until CreateFraming can consume them into the prologue.
    // after CreateFraming is called, this dictionary is set to null, so don't use it after that.
    private Dictionary<string, object> prepopulatedFields;

    private bool isDisposed;

    public MsgPackTraceExporter(GenevaExporterOptions options, Func<Resource> resourceProvider)
    {
        Guard.ThrowIfNull(options);
        Guard.ThrowIfNull(resourceProvider);

        this.resourceProvider = resourceProvider;

        var partAName = "Span";
        if (options.TableNameMappings != null
            && options.TableNameMappings.TryGetValue("Span", out var customTableName))
        {
            partAName = customTableName;
        }

        this.partAName = partAName;

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
        }

        this.userProvidedPrepopulatedFields = options.PrepopulatedFields != null && options.PrepopulatedFields.Count > 0;

        this.prepopulatedFields = new Dictionary<string, object>(0, StringComparer.Ordinal);
        if (options.PrepopulatedFields != null)
        {
            foreach (var entry in options.PrepopulatedFields)
            {
                this.prepopulatedFields.Add(entry.Key, entry.Value);
            }
        }

        // TODO: Validate custom fields (reserved name? etc).
        if (options.CustomFields != null)
        {
            var customFields = new HashSet<string>(StringComparer.Ordinal);
            var dedicatedFields = new HashSet<string>(StringComparer.Ordinal);

            // Seed customFields with Span PartB
            customFields.Add("azureResourceProvider");
            dedicatedFields.Add("azureResourceProvider");

            foreach (var name in CS40_PART_B_MAPPING.Values)
            {
                customFields.Add(name);
                dedicatedFields.Add(name);
            }

            foreach (var name in options.CustomFields)
            {
                customFields.Add(name);
                dedicatedFields.Add(name);
            }

#if NET
            this.CustomFields = customFields.ToFrozenSet(StringComparer.Ordinal);
#else
            this.CustomFields = customFields;
#endif

            foreach (var name in CS40_PART_B_MAPPING.Keys)
            {
                dedicatedFields.Add(name);
            }

            dedicatedFields.Add("otel.status_code");
            dedicatedFields.Add("otel.status_description");

#if NET
            this.DedicatedFields = dedicatedFields.ToFrozenSet(StringComparer.Ordinal);
#else
            this.DedicatedFields = dedicatedFields;
#endif
        }

        this.resourceFieldNames = options.ResourceFieldNames;
        this.shouldIncludeTraceState = options.IncludeTraceStateForSpan;
    }

    internal bool IsUsingUnixDomainSocket => this.dataTransport is UnixDomainSocketDataTransport;

    public ExportResult Export(in Batch<Activity> batch)
    {
        // Note: The MessagePackSerializer takes way less time / memory than creating the activity itself.
        //       This makes the short-circuit check less useful.
        //       On the other side, running the serializer could help to catch error in the early phase of development lifecycle.
        //
        // if (!m_dataTransport.IsEnabled())
        // {
        //     return ExportResult.Success;
        // }

        var result = ExportResult.Success;

        foreach (var activity in batch)
        {
            try
            {
                var data = this.SerializeActivity(activity);

                this.dataTransport.Send(data.Array!, data.Count);
            }
            catch (Exception ex)
            {
                ExporterEventSource.Log.FailedToSendTraceData(ex); // TODO: preallocate exception or no exception
                result = ExportResult.Failure;
            }
        }

        ExporterEventSource.Log.ExportCompleted(nameof(MsgPackTraceExporter));

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
            this.Buffer.Dispose();
            this.HttpUrlParts.Dispose();
        }
        catch (Exception ex)
        {
            ExporterEventSource.Log.ExporterException("MsgPackTraceExporter Dispose failed.", ex);
        }

        this.isDisposed = true;
    }

    internal static bool CacheIfPartOfHttpUrl(KeyValuePair<string, object?> entry, object?[] httpUrlParts)
    {
        if (CS40_PART_B_HTTPURL_MAPPING.TryGetValue(entry.Key, out var index))
        {
            if (index < httpUrlParts.Length)
            {
                httpUrlParts[index] = entry.Value;
                return true;
            }
        }

        return false;
    }

    internal static string? GetHttpUrl(object?[] httpUrlParts)
    {
        // OpenTelemetry Semantic Convention: https://github.com/open-telemetry/semantic-conventions/blob/v1.28.0/docs/http/http-spans.md#http-server-semantic-conventions
        var scheme = httpUrlParts[0]?.ToString() ?? string.Empty;  // 0 => CS40_PART_B_HTTPURL_MAPPING["url.scheme"]
        var address = httpUrlParts[1]?.ToString() ?? string.Empty;  // 1 => CS40_PART_B_HTTPURL_MAPPING["server.address"]
        var port = httpUrlParts[2]?.ToString();  // 2 => CS40_PART_B_HTTPURL_MAPPING["server.port"]
        port = port != null ? $":{port}" : string.Empty;
        var path = httpUrlParts[3]?.ToString() ?? string.Empty;  // 3 => CS40_PART_B_HTTPURL_MAPPING["url.path"]
        var query = httpUrlParts[4]?.ToString();  // 4 => CS40_PART_B_HTTPURL_MAPPING["url.query"]
        query = query != null ? $"?{query}" : string.Empty;

        var length = scheme.Length + Uri.SchemeDelimiter.Length + address.Length + port.Length + path.Length + query.Length;

        // No URL elements found, i.e. no scheme, no address, no port, no path, no query
        if (length == Uri.SchemeDelimiter.Length)
        {
            return null;
        }

        var urlStringBuilder = new StringBuilder(length)
            .Append(scheme)
            .Append(Uri.SchemeDelimiter)
            .Append(address)
            .Append(port)
            .Append(path)
            .Append(query);

        return urlStringBuilder.ToString();
    }

    /// <summary>
    /// Populates this.bufferPrologue and this.bufferEpilogue.
    /// </summary>
    internal void CreateFraming()
    {
        var buffer = new byte[BUFFER_SIZE];

        var cursor = 0;

        /* Fluentd Forward Mode:
        [
            "Span",
            [
                [ <timestamp>, { "env_ver": "4.0", ... } ]
            ],
            { "TimeFormat": "DateTime" }
        ]
        */
        cursor = MessagePackSerializer.WriteArrayHeader(buffer, cursor, 3);
        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, this.partAName);
        cursor = MessagePackSerializer.WriteArrayHeader(buffer, cursor, 1);
        cursor = MessagePackSerializer.WriteArrayHeader(buffer, cursor, 2);

        // timestamp
        cursor = MessagePackSerializer.WriteTimestamp96Header(buffer, cursor);
        this.timestampPatchIndex = cursor;
        cursor += 12; // reserve 12 bytes for the timestamp

        cursor = MessagePackSerializer.WriteMapHeader(buffer, cursor, ushort.MaxValue); // Note: always use Map16 for perf consideration
        this.mapSizePatchIndex = cursor - 2;

        // TODO: Do we support PartB as well?
        // Part A - core envelope
        cursor = AddPartAField(buffer, cursor, Schema.V40.PartA.Name, this.partAName);

        cursor = AddPartAField(buffer, cursor, Schema.V40.PartA.Ver, "4.0");

        var resourceAttributes = this.resourceProvider().Attributes;

        if (this.resourceFieldNames != null)
        {
            // if ResourceFieldNames is set, we use resource attributes rather than PrepopulatedFields
            this.prepopulatedFields = new Dictionary<string, object>(0, StringComparer.Ordinal);
        }

        // this is guaranteed to not be null because it's set in the constructor
        Guard.ThrowIfNull(this.prepopulatedFields);

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
                this.prepopulatedFields[key] = value;
            }
        }

        foreach (var entry in this.prepopulatedFields)
        {
            cursor = AddPartAField(buffer, cursor, entry.Key, entry.Value);
        }

        this.bufferPrologue = new byte[cursor - 0];
        System.Buffer.BlockCopy(buffer, 0, this.bufferPrologue, 0, cursor - 0);

        // Now generate the epilogue
        cursor = MessagePackSerializer.Serialize(buffer, 0, new Dictionary<string, object> { { "TimeFormat", "DateTime" } });

        this.bufferEpilogue = new byte[cursor - 0];
        System.Buffer.BlockCopy(buffer, 0, this.bufferEpilogue, 0, cursor - 0);
    }

    internal ArraySegment<byte> SerializeActivity(Activity activity)
    {
        var buffer = this.Buffer.Value;
        if (buffer == null)
        {
            this.CreateFraming();
            buffer = new byte[BUFFER_SIZE]; // TODO: handle OOM
            System.Buffer.BlockCopy(this.bufferPrologue!, 0, buffer, 0, this.bufferPrologue!.Length);
            this.Buffer.Value = buffer;
        }

        // These are guaranteed to not be null because CreateFraming must be called.
        Guard.ThrowIfNull(this.bufferPrologue);
        Guard.ThrowIfNull(this.bufferEpilogue);

        var cursor = this.bufferPrologue.Length;

        var cntFields = (ushort)(this.prepopulatedFields.Count + 2); // the two extra are for Name and Ver, set in CreateFraming
        var dtBegin = activity.StartTimeUtc;
        var tsBegin = dtBegin.Ticks;
        var tsEnd = tsBegin + activity.Duration.Ticks;
        var dtEnd = new DateTime(tsEnd, DateTimeKind.Utc);

        MessagePackSerializer.WriteTimestamp96(buffer, this.timestampPatchIndex, tsEnd);

        #region Part A - core envelope
        cursor = AddPartAField(buffer, cursor, Schema.V40.PartA.Time, dtEnd);
        cntFields += 1;
        #endregion

        #region Part A - dt extension

        // Note: ToHexString returns the pre-calculated hex representation without allocation
        cursor = AddPartAField(buffer, cursor, Schema.V40.PartA.Extensions.Dt.TraceId, activity.Context.TraceId.ToHexString());
        cntFields += 1;

        cursor = AddPartAField(buffer, cursor, Schema.V40.PartA.Extensions.Dt.SpanId, activity.Context.SpanId.ToHexString());
        cntFields += 1;
        #endregion

        #region Part B Span - required fields
        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "name");
        cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, activity.DisplayName);
        cntFields += 1;

        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "kind");
        cursor = MessagePackSerializer.SerializeInt32(buffer, cursor, (int)activity.Kind);
        cntFields += 1;

        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "startTime");
        cursor = MessagePackSerializer.SerializeUtcDateTime(buffer, cursor, dtBegin);
        cntFields += 1;

        cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "success");
        cursor = MessagePackSerializer.SerializeBool(buffer, cursor, true);
        var idxSuccessPatch = cursor - 1;
        cntFields += 1;
        #endregion

        #region Part B Span optional fields and Part C fields
        var strParentId = activity.ParentSpanId.ToHexString();

        // Note: this should be blazing fast since Object.ReferenceEquals(strParentId, INVALID_SPAN_ID) == true
        if (!string.Equals(strParentId, INVALID_SPAN_ID, StringComparison.Ordinal))
        {
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "parentId");
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, strParentId);
            cntFields += 1;
        }

        if (this.shouldIncludeTraceState)
        {
            var traceStateString = activity.TraceStateString;
            if (!string.IsNullOrEmpty(traceStateString))
            {
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "traceState");
                cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, traceStateString);
                cntFields += 1;
            }
        }

        var linkEnumerator = activity.EnumerateLinks();
        if (linkEnumerator.MoveNext())
        {
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "links");
            cursor = MessagePackSerializer.WriteArrayHeader(buffer, cursor, ushort.MaxValue); // Note: always use Array16 for perf consideration
            var idxLinkPatch = cursor - 2;

            ushort cntLink = 0;
            do
            {
                ref readonly var link = ref linkEnumerator.Current;

                cursor = MessagePackSerializer.WriteMapHeader(buffer, cursor, 2);
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "toTraceId");
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, link.Context.TraceId.ToHexString());
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "toSpanId");
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, link.Context.SpanId.ToHexString());
                cntLink += 1;
            }
            while (linkEnumerator.MoveNext());

            MessagePackSerializer.WriteUInt16(buffer, idxLinkPatch, cntLink);
            cntFields += 1;
        }

        // TODO: The current approach is to iterate twice over TagObjects so that all
        // env_properties can be added the very end. This avoids speculating the size
        // and preallocating a separate buffer for it.
        // Alternates include static allocation and iterate once.
        // The TODO: here is to measure perf and change to alternate, if required.

        // Iteration #1 - Get those fields which become dedicated column
        // i.e all PartB fields and opt-in part c fields.
        var hasEnvProperties = false;
        var isStatusSuccess = true;
        string? statusDescription = null;

        var isServerActivity = activity.Kind == ActivityKind.Server;
        var httpUrlParts = this.HttpUrlParts.Value ?? new object?[CS40_PART_B_HTTPURL_MAPPING.Count];
        if (isServerActivity)
        {
            if (this.HttpUrlParts.Value == null)
            {
                this.HttpUrlParts.Value = httpUrlParts;
            }
            else
            {
                Array.Clear(httpUrlParts, 0, httpUrlParts.Length);
            }
        }

        foreach (ref readonly var entry in activity.EnumerateTagObjects())
        {
            if (isServerActivity && CacheIfPartOfHttpUrl(entry, httpUrlParts))
            {
                continue; // Skip this entry, since it is part of httpUrl.
            }

            // TODO: check name collision
            if (CS40_PART_B_MAPPING.TryGetValue(entry.Key, out var replacementKey))
            {
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, replacementKey);
            }
            else if (IfTagMatchesStatusOrStatusDescription(entry, ref isStatusSuccess, ref statusDescription))
            {
                continue;
            }
            else if (this.CustomFields == null || this.CustomFields.Contains(entry.Key))
            {
                // TODO: the above null check can be optimized and avoided inside foreach.
                cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, entry.Key);
            }
            else
            {
                hasEnvProperties = true;
                continue;
            }

            cursor = MessagePackSerializer.Serialize(buffer, cursor, entry.Value);
            cntFields += 1;
        }

        if (isServerActivity)
        {
            var httpUrl = GetHttpUrl(httpUrlParts);
            if (httpUrl != null)
            {
                // If the activity is a server activity and has http.url, we need to add it as a dedicated field.
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "httpUrl");
                cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, httpUrl);
                cntFields += 1;
            }
        }

        if (hasEnvProperties)
        {
            // Anything that's not a dedicated field gets put into a part C field called properties
            ushort envPropertiesCount = 0;
            cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "env_properties");
            cursor = MessagePackSerializer.WriteMapHeader(buffer, cursor, ushort.MaxValue);
            var idxMapSizeEnvPropertiesPatch = cursor - 2;

            foreach (ref readonly var entry in activity.EnumerateTagObjects())
            {
                if (this.DedicatedFields!.Contains(entry.Key))
                {
                    continue;
                }
                else
                {
                    cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, entry.Key);
                    cursor = MessagePackSerializer.Serialize(buffer, cursor, entry.Value);
                    envPropertiesCount += 1;
                }
            }

            cntFields += 1;
            MessagePackSerializer.WriteUInt16(buffer, idxMapSizeEnvPropertiesPatch, envPropertiesCount);
        }

        if (activity.Status != ActivityStatusCode.Unset)
        {
            if (activity.Status == ActivityStatusCode.Error)
            {
                MessagePackSerializer.SerializeBool(buffer, idxSuccessPatch, false);
            }

            if (!string.IsNullOrEmpty(activity.StatusDescription))
            {
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "statusMessage");
                cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, activity.StatusDescription);
                cntFields += 1;
            }
        }
        else if (!isStatusSuccess)
        {
            MessagePackSerializer.SerializeBool(buffer, idxSuccessPatch, false);

            if (!string.IsNullOrEmpty(statusDescription))
            {
                cursor = MessagePackSerializer.SerializeAsciiString(buffer, cursor, "statusMessage");
                cursor = MessagePackSerializer.SerializeUnicodeString(buffer, cursor, statusDescription);
                cntFields += 1;
            }
        }
        #endregion

        MessagePackSerializer.WriteUInt16(buffer, this.mapSizePatchIndex, cntFields);

        System.Buffer.BlockCopy(this.bufferEpilogue, 0, buffer, cursor, this.bufferEpilogue.Length);
        cursor += this.bufferEpilogue.Length;

        return new(buffer, 0, cursor);
    }

    private static bool IfTagMatchesStatusOrStatusDescription(
        KeyValuePair<string, object?> entry,
        ref bool isStatusSuccess,
        ref string? statusDescription)
    {
        if (entry.Key.StartsWith("otel.status_", StringComparison.Ordinal))
        {
            var keyPart = entry.Key.AsSpan().Slice(12);
            if (keyPart is "code")
            {
                if (string.Equals(Convert.ToString(entry.Value, CultureInfo.InvariantCulture), "ERROR", StringComparison.Ordinal))
                {
                    isStatusSuccess = false;
                }

                return true;
            }

            if (keyPart is "description")
            {
                statusDescription = Convert.ToString(entry.Value, CultureInfo.InvariantCulture) ?? string.Empty;
                return true;
            }
        }

        return false;
    }
}
