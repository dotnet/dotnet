// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

public partial class JsonDataSerializer
{
    private static readonly bool DisableFastJson = FeatureFlag.Instance.IsSet(FeatureFlag.VSTEST_DISABLE_FASTER_JSON_SERIALIZATION);

    private static readonly JsonSerializerOptions PayloadOptionsV1; // payload options for version <= 1
    private static readonly JsonSerializerOptions PayloadOptionsV2; // payload options for version >= 2
    private static readonly JsonSerializerOptions FastOptions; // options for faster json
    private static readonly JsonSerializerOptions DefaultOptions; // generic options

    static JsonDataSerializer()
    {
        // DefaultOptions: common converters shared by all option sets
        DefaultOptions = CreateBaseOptions();
        DefaultOptions.Converters.Add(new TestPropertyConverter());
        DefaultOptions.Converters.Add(new ObjectDictionaryConverterFactory());
        DefaultOptions.Converters.Add(new ObjectConverter());
        DefaultOptions.Converters.Add(new AttachmentSetConverter());
        DefaultOptions.Converters.Add(new UriDataAttachmentConverter());
        DefaultOptions.Converters.Add(new TestExecutionContextConverter());
        DefaultOptions.Converters.Add(new TestRunCompleteEventArgsConverter());
        DefaultOptions.Converters.Add(new TestRunChangedEventArgsConverter());
        DefaultOptions.Converters.Add(new AfterTestRunEndResultConverter());
        DefaultOptions.Converters.Add(new TestProcessAttachDebuggerPayloadConverter());
        DefaultOptions.Converters.Add(new TestSessionInfoConverter());
        DefaultOptions.Converters.Add(new DiscoveryCriteriaConverter());

        // V2 options: clone DefaultOptions and add V2-specific converters
        PayloadOptionsV2 = new JsonSerializerOptions(DefaultOptions);
        PayloadOptionsV2.Converters.Add(new TestCaseConverterV2());
        PayloadOptionsV2.Converters.Add(new TestResultConverterV2());
        PayloadOptionsV2.Converters.Add(new TestObjectConverter());
        PayloadOptionsV2.Converters.Add(new TestRunStatisticsConverter());
        PayloadOptionsV2.Converters.Add(new TestObjectBaseConverterFactory());

        // V1 options: clone DefaultOptions and add V1-specific converters
        PayloadOptionsV1 = new JsonSerializerOptions(DefaultOptions);
        PayloadOptionsV1.Converters.Add(new TestCaseConverter());
        PayloadOptionsV1.Converters.Add(new TestResultConverter());
        PayloadOptionsV1.Converters.Add(new TestObjectConverter());
        PayloadOptionsV1.Converters.Add(new TestRunStatisticsConverter());
        PayloadOptionsV1.Converters.Add(new TestObjectBaseConverterFactory());

        // Fast options: same converter set as V2
        FastOptions = new JsonSerializerOptions(PayloadOptionsV2);
    }

    private static JsonSerializerOptions CreateBaseOptions() => new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        MaxDepth = 64,
        WriteIndented = false,
        PropertyNamingPolicy = null, // PascalCase (same as Newtonsoft default)
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    private static partial (int version, string? messageType) ParseHeaderFromJson(string rawMessage)
    {
        using var doc = JsonDocument.Parse(rawMessage);
        var root = doc.RootElement;
        int version = root.TryGetProperty("Version", out var vProp)
            ? (vProp.ValueKind == JsonValueKind.Number ? vProp.GetInt32() : int.TryParse(vProp.GetString(), out var v) ? v : 0)
            : 0;
        string? messageType = root.TryGetProperty("MessageType", out var mtProp) ? mtProp.GetString() : null;
        return (version, messageType);
    }

    private static partial T? DeserializePayloadCore<T>(Message message)
    {
        var payloadOptions = GetPayloadOptions(message.Version);

        T? result;
        if (payloadOptions == PayloadOptionsV2)
        {
            // Fast path: deserialize payload directly from raw message
            var messageWithPayload = DeserializeObjectFast<PayloadedMessage<T>>(message.RawMessage!);
            result = messageWithPayload is null ? default : messageWithPayload.Payload;
        }
        else
        {
            // V1 path: need to parse the Payload field as a separate step
            // because V1 converters (TestCaseConverter, TestResultConverter) need special handling
            using var doc = JsonDocument.Parse(message.RawMessage!);
            if (doc.RootElement.TryGetProperty("Payload", out var payloadElement))
            {
                result = JsonSerializer.Deserialize<T>(payloadElement, payloadOptions);
            }
            else
            {
                result = default;
            }
        }

        // STJ's built-in dictionary converter deserializes JSON integers as double
        // when the value type is object. Fix up any IDictionary<string, object> metrics
        // so integer values remain int/long (matching Newtonsoft behavior).
        FixUpObjectDictionaries(result);

        return result;
    }

    /// <summary>
    /// Walks known properties with IDictionary&lt;string, object&gt; and converts
    /// double values that represent whole numbers back to int/long.
    /// STJ deserializes JSON integers as double for object-typed dictionary values.
    /// </summary>
    private static void FixUpObjectDictionaries(object? obj)
    {
        switch (obj)
        {
            case IDictionary<string, object> d:
                FixUpDictionary(d);
                break;
            case TestRunCompleteEventArgs args:
                FixUpDictionary(args.Metrics);
                break;
            case TestRunAttachmentsProcessingCompleteEventArgs args:
                FixUpDictionary(args.Metrics);
                break;
            case AfterTestRunEndResult r:
                FixUpDictionary(r.Metrics);
                break;
            case ObjectModel.DiscoveryCompletePayload p:
                FixUpDictionary(p.Metrics);
                break;
            case ObjectModel.TestRunAttachmentsProcessingCompletePayload p:
                FixUpObjectDictionaries(p.AttachmentsProcessingCompleteEventArgs);
                break;
        }
    }

    private static void FixUpDictionary(IDictionary<string, object>? dict)
    {
        if (dict is null)
        {
            return;
        }

        foreach (var key in dict.Keys.ToArray())
        {
            if (dict[key] is double d && d == Math.Truncate(d) && d is >= int.MinValue and <= int.MaxValue)
            {
                dict[key] = (int)d;
            }
        }
    }

    private static partial T? DeserializeCore<T>(string json, int version)
    {
        var options = GetPayloadOptions(version);
        return Deserialize<T>(options, json);
    }

    private static partial string SerializeMessageCore(string? messageType)
    {
        return Serialize(DefaultOptions, new MessageEnvelope { MessageType = messageType });
    }

    private static partial string SerializePayloadCore(string? messageType, object? payload, int version)
    {
        var payloadOptions = GetPayloadOptions(version);
        // Fast json is only equivalent to the serialization that is used for protocol version 2 and upwards (or more precisely for the paths that use PayloadOptionsV2)
        // so when we resolved the old options we should use non-fast path.
        if (DisableFastJson || payloadOptions == PayloadOptionsV1)
        {
            if (payload is null)
                return string.Empty;

            var serializedPayload = JsonSerializer.SerializeToElement(payload, payloadOptions);

            return version > 1 ?
                Serialize(DefaultOptions, new VersionedMessageEnvelope { MessageType = messageType, Version = version, Payload = serializedPayload }) :
                Serialize(DefaultOptions, new MessageEnvelope { MessageType = messageType, Payload = serializedPayload });
        }
        else
        {
            return Serialize(FastOptions, new VersionedMessageForSerialization { MessageType = messageType, Version = version, Payload = payload });
        }
    }

    private static partial string SerializeCore<T>(T data, int version)
    {
        var options = GetPayloadOptions(version);
        return Serialize(options, data);
    }

    private static T? DeserializeObjectFast<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value, FastOptions);
    }

    /// <summary>
    /// Serialize data.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    /// <param name="options">Serializer options.</param>
    /// <param name="data">Data to be serialized.</param>
    /// <returns>Serialized data.</returns>
    private static string Serialize<T>(JsonSerializerOptions options, T data)
    {
        return JsonSerializer.Serialize(data, options);
    }

    /// <summary>
    /// Deserialize data.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    /// <param name="options">Serializer options.</param>
    /// <param name="data">Data to be deserialized.</param>
    /// <returns>Deserialized data.</returns>
    private static T? Deserialize<T>(JsonSerializerOptions options, string data)
    {
        return JsonSerializer.Deserialize<T>(data, options);
    }

    private static JsonSerializerOptions GetPayloadOptions(int? version)
    {
        version ??= 1;

        return version switch
        {
            // 0 is used during negotiation.
            // Protocol version 3 was accidentally used with serializer v1 and not
            // serializer v2, we downgrade to protocol 2 when 3 would be negotiated
            // unless this is disabled by VSTEST_DISABLE_PROTOCOL_3_VERSION_DOWNGRADE
            // env variable.
            0 or 1 or 3 => PayloadOptionsV1,
            2 or 4 or 5 or 6 or 7 => PayloadOptionsV2,

            _ => throw new NotSupportedException($"Protocol version {version} is not supported. "
                + "Ensure it is compatible with the latest serializer or add a new one."),
        };
    }

    /// <summary>
    /// This grabs payload from the message, we already know version and message type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private class PayloadedMessage<T>
    {
        public T? Payload { get; set; }
    }

    /// <summary>
    /// Serialization-only DTO for building the JSON wire format (without Version).
    /// NOT a Message — this is never returned to callers.
    /// </summary>
    private class MessageEnvelope
    {
        public string? MessageType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Payload { get; set; }
    }

    /// <summary>
    /// Serialization-only DTO for building the JSON wire format (with Version).
    /// NOT a Message — this is never returned to callers.
    /// </summary>
    private class VersionedMessageEnvelope
    {
        public int Version { get; set; }
        public string? MessageType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Payload { get; set; }
    }

    /// <summary>
    /// For serialization directly into string, without first converting to JsonElement, and then from JsonElement to string.
    /// </summary>
    private class VersionedMessageForSerialization
    {
        /// <summary>
        /// Gets or sets the version of the message
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        public string? MessageType { get; set; }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        public object? Payload { get; set; }
    }
}
#endif
