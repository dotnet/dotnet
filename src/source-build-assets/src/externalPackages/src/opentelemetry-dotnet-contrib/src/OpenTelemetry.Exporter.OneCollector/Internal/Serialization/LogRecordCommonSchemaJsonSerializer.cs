// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Text.Json;
using OpenTelemetry.Internal;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.OneCollector;

internal sealed class LogRecordCommonSchemaJsonSerializer : CommonSchemaJsonSerializer<LogRecord>
{
    private static readonly JsonEncodedText EventIdProperty = JsonEncodedText.Encode("eventId");
    private static readonly JsonEncodedText SeverityTextProperty = JsonEncodedText.Encode("severityText");
    private static readonly JsonEncodedText SeverityNumberProperty = JsonEncodedText.Encode("severityNumber");
    private static readonly JsonEncodedText BodyProperty = JsonEncodedText.Encode("body");
    private static readonly JsonEncodedText FormattedMessageProperty = JsonEncodedText.Encode("formattedMessage");
    private static readonly JsonEncodedText NamespaceProperty = JsonEncodedText.Encode("namespace");
    private static readonly JsonEncodedText DistributedTraceExtensionProperty = JsonEncodedText.Encode("dt");
    private static readonly JsonEncodedText DistributedTraceExtensionTraceIdProperty = JsonEncodedText.Encode("traceId");
    private static readonly JsonEncodedText DistributedTraceExtensionSpanIdProperty = JsonEncodedText.Encode("spanId");
    private static readonly JsonEncodedText ExceptionExtensionProperty = JsonEncodedText.Encode("ex");
    private static readonly JsonEncodedText ExceptionExtensionTypeProperty = JsonEncodedText.Encode("type");
    private static readonly JsonEncodedText ExceptionExtensionMessageProperty = JsonEncodedText.Encode("msg");
    private static readonly JsonEncodedText ExceptionExtensionStackTraceProperty = JsonEncodedText.Encode("stack");

    private static readonly JsonEncodedText[] LogLevelToSeverityTextMappings =
    [
        JsonEncodedText.Encode("Trace"),
        JsonEncodedText.Encode("Debug"),
        JsonEncodedText.Encode("Information"),
        JsonEncodedText.Encode("Warning"),
        JsonEncodedText.Encode("Error"),
        JsonEncodedText.Encode("Critical"),
        JsonEncodedText.Encode("Trace") // Note: This is the "None" bucket.
    ];

    private static readonly int[] LogLevelToSeverityNumberMappings =
    [
        1, 5, 9, 13, 17, 21, 1
    ];

    private static readonly Action<LogRecordScope, CommonSchemaJsonSerializationState> SerializeScopeItemToJson = (scope, serializationState) =>
    {
        var writer = serializationState.Writer;

        foreach (var scopeAttribute in scope)
        {
            if (scopeAttribute.Key == "{OriginalFormat}")
            {
                continue;
            }

            if (AttributeKeyStartWithExtensionPrefix(scopeAttribute.Key))
            {
                serializationState.AddExtensionAttribute(scopeAttribute!);
                continue;
            }

            CommonSchemaJsonSerializationHelper.SerializeKeyValueToJson(scopeAttribute.Key, scopeAttribute.Value, writer);
        }
    };

    private readonly EventNameManager eventNameManager;
    private readonly OneCollectorExporterSerializationExceptionStackTraceHandlingType exceptionStackTraceHandling;

    public LogRecordCommonSchemaJsonSerializer(
        EventNameManager eventNameManager,
        string tenantToken,
        OneCollectorExporterSerializationExceptionStackTraceHandlingType exceptionStackTraceHandling = OneCollectorExporterSerializationExceptionStackTraceHandlingType.Ignore,
        int maxPayloadSizeInBytes = int.MaxValue,
        int maxNumberOfItemsPerPayload = int.MaxValue)
        : base(tenantToken, maxPayloadSizeInBytes, maxNumberOfItemsPerPayload)
    {
        Debug.Assert(eventNameManager != null, "eventNameManager was null");

        this.eventNameManager = eventNameManager!;
        this.exceptionStackTraceHandling = exceptionStackTraceHandling;
    }

    public override string Description => "LogRecord Common Schema JSON";

    protected override void SerializeItemToJson(Resource resource, LogRecord item, CommonSchemaJsonSerializationState serializationState)
    {
        Debug.Assert(serializationState != null, "serializationState was null");

        var writer = serializationState!.Writer;

        Debug.Assert(writer != null, "writer was null");

        int attributeStartIndex = 0;
        EventNameManager.ResolvedEventFullName resolvedEventFullName;
        if (item.Attributes != null
            && item.Attributes.Count > 0
            && item.Attributes[0].Key == "{EventFullName}"
            && item.Attributes[0].Value is string eventFullName
            && !string.IsNullOrEmpty(eventFullName))
        {
            attributeStartIndex++;
            resolvedEventFullName = this.eventNameManager.ResolveEventFullName(eventFullName);
        }
        else
        {
            resolvedEventFullName = this.eventNameManager.ResolveEventFullName(
                item.CategoryName,
                item.EventId.Name);
        }

        writer!.WriteStartObject();

        writer.WriteString(CommonSchemaJsonSerializationHelper.VersionProperty, CommonSchemaJsonSerializationHelper.Version4Value);

        writer.WritePropertyName(CommonSchemaJsonSerializationHelper.NameProperty);
#if DEBUG
        writer.WriteRawValue(resolvedEventFullName.EventFullName, skipInputValidation: false);
#else
        writer.WriteRawValue(resolvedEventFullName.EventFullName, skipInputValidation: true);
#endif

        writer.WriteString(CommonSchemaJsonSerializationHelper.TimeProperty, item.Timestamp);

        writer.WriteString(CommonSchemaJsonSerializationHelper.IKeyProperty, this.TenantTokenWithTenancySystemSymbol);

        writer.WriteStartObject(CommonSchemaJsonSerializationHelper.DataProperty);

        if (item.EventId.Id != 0)
        {
            writer.WriteNumber(EventIdProperty, item.EventId.Id);
        }

        if (resolvedEventFullName.OriginalEventNamespace != null)
        {
            writer.WritePropertyName(NamespaceProperty);
#if DEBUG
            writer.WriteRawValue(resolvedEventFullName.OriginalEventNamespace, skipInputValidation: false);
#else
            writer.WriteRawValue(resolvedEventFullName.OriginalEventNamespace, skipInputValidation: true);
#endif
        }

        if (resolvedEventFullName.OriginalEventName != null)
        {
            writer.WritePropertyName(CommonSchemaJsonSerializationHelper.NameProperty);
#if DEBUG
            writer.WriteRawValue(resolvedEventFullName.OriginalEventName, skipInputValidation: false);
#else
            writer.WriteRawValue(resolvedEventFullName.OriginalEventName, skipInputValidation: true);
#endif
        }

#if EXPOSE_EXPERIMENTAL_FEATURES
#pragma warning disable CS0618 // Type or member is obsolete
        // TODO: Update to use LogRecord.Severity
        var logLevel = (int)item.LogLevel;
#pragma warning restore CS0618 // Type or member is obsolete
#else
        var logLevel = (int)item.LogLevel;
#endif
        writer.WriteString(SeverityTextProperty, LogLevelToSeverityTextMappings[logLevel]);
        writer.WriteNumber(SeverityNumberProperty, LogLevelToSeverityNumberMappings[logLevel]);

        string? originalFormat = null;

        if (item.Attributes != null)
        {
            for (int i = attributeStartIndex; i < item.Attributes.Count; i++)
            {
                var attribute = item.Attributes[i];

                if (string.IsNullOrEmpty(attribute.Key))
                {
                    continue;
                }

                if (attribute.Key == "{OriginalFormat}")
                {
                    originalFormat = attribute.Value as string;
                    continue;
                }

                if (AttributeKeyStartWithExtensionPrefix(attribute.Key))
                {
                    serializationState.AddExtensionAttribute(attribute);
                    continue;
                }

                CommonSchemaJsonSerializationHelper.SerializeKeyValueToJson(attribute.Key, attribute.Value, writer);
            }
        }

        var hasBody = false;

        if (!string.IsNullOrEmpty(originalFormat))
        {
            writer.WriteString(BodyProperty, originalFormat);
            hasBody = true;
        }
        else if (!string.IsNullOrEmpty(item.Body))
        {
            writer.WriteString(BodyProperty, item.Body);
            hasBody = true;
        }
        else if (!string.IsNullOrEmpty(item.FormattedMessage))
        {
            writer.WriteString(BodyProperty, item.FormattedMessage);
            writer.WriteString(FormattedMessageProperty, item.FormattedMessage);
        }

        if (hasBody
            && !string.IsNullOrEmpty(item.FormattedMessage)
            && item.Body != item.FormattedMessage)
        {
            writer.WriteString(FormattedMessageProperty, item.FormattedMessage);
        }

        SerializeResourceToJsonInsideCurrentObject(resource, serializationState);

        item.ForEachScope(SerializeScopeItemToJson, serializationState);

        writer.WriteEndObject();

        this.SerializeExtensionPropertiesToJson(item, writer, serializationState);

        writer.WriteEndObject();
    }

    private void SerializeExtensionPropertiesToJson(LogRecord item, Utf8JsonWriter writer, CommonSchemaJsonSerializationState serializationState)
    {
        var hasTraceContext = item.TraceId != default;
        var hasException = item.Exception != null;
        var hasExtensions = serializationState.ExtensionAttributeCount > 0;

        if (!hasTraceContext && !hasException && !hasExtensions)
        {
            return;
        }

        writer.WriteStartObject(CommonSchemaJsonSerializationHelper.ExtensionsProperty);

        if (hasTraceContext)
        {
            writer.WriteStartObject(DistributedTraceExtensionProperty);
            writer.WriteString(DistributedTraceExtensionTraceIdProperty, item.TraceId.ToHexString());
            writer.WriteString(DistributedTraceExtensionSpanIdProperty, item.SpanId.ToHexString());
            /*
             * Note: OneCollector does not currently support traceFlags. See:
             * https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1313
             *
             * writer.WriteNumber(DistributedTraceExtensionTraceFlagsProperty, (int)item.TraceFlags);
             */
            writer.WriteEndObject();
        }

        if (hasException)
        {
            writer.WriteStartObject(ExceptionExtensionProperty);
            writer.WriteString(ExceptionExtensionTypeProperty, item.Exception!.GetType().FullName);
            writer.WriteString(ExceptionExtensionMessageProperty, item.Exception.Message);

            if (this.exceptionStackTraceHandling == OneCollectorExporterSerializationExceptionStackTraceHandlingType.IncludeAsString)
            {
                writer.WriteString(ExceptionExtensionStackTraceProperty, item.Exception.ToInvariantString());
            }

            writer.WriteEndObject();
        }

        if (hasExtensions)
        {
            serializationState.SerializeExtensionPropertiesToJson(writeExtensionObjectEnvelope: false);
        }

        writer.WriteEndObject();
    }
}
