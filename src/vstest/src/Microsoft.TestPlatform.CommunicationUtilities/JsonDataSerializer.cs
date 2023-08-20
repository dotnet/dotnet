// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// JsonDataSerializes serializes and deserializes data using Json format
/// </summary>
public class JsonDataSerializer : IDataSerializer
{
    private static JsonDataSerializer? s_instance;

    private static readonly bool DisableFastJson = FeatureFlag.Instance.IsSet(FeatureFlag.DISABLE_FASTER_JSON_SERIALIZATION);

    private static readonly JsonSerializer PayloadSerializerV1; // payload serializer for version <= 1
    private static readonly JsonSerializer PayloadSerializerV2; // payload serializer for version >= 2
    private static readonly JsonSerializer FastSerializer;
    private static readonly JsonSerializerSettings FastJsonSettings; // serializer settings for faster json
    private static readonly JsonSerializerSettings JsonSettings; // serializer settings for serializer v1, which should use to deserialize message headers
    private static readonly JsonSerializer Serializer; // generic serializer

    static JsonDataSerializer()
    {

        var jsonSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            TypeNameHandling = TypeNameHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ConstructorHandling = ConstructorHandling.Default,
            MetadataPropertyHandling = MetadataPropertyHandling.Default,
            Formatting = Formatting.None,
            FloatParseHandling = FloatParseHandling.Double,
            FloatFormatHandling = FloatFormatHandling.String,
            StringEscapeHandling = StringEscapeHandling.Default,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            Culture = CultureInfo.InvariantCulture,
            CheckAdditionalContent = false,
            DateFormatString = @"yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK",
            MaxDepth = 64,
        };

        JsonSettings = jsonSettings;

        Serializer = JsonSerializer.Create();
        PayloadSerializerV1 = JsonSerializer.Create(jsonSettings);
        PayloadSerializerV2 = JsonSerializer.Create(jsonSettings);

        var contractResolver = new DefaultTestPlatformContractResolver();
        FastJsonSettings = new JsonSerializerSettings
        {
            DateFormatHandling = jsonSettings.DateFormatHandling,
            DateParseHandling = jsonSettings.DateParseHandling,
            DateTimeZoneHandling = jsonSettings.DateTimeZoneHandling,
            TypeNameHandling = jsonSettings.TypeNameHandling,
            ReferenceLoopHandling = jsonSettings.ReferenceLoopHandling,
            MissingMemberHandling = jsonSettings.MissingMemberHandling,
            // PERF: Null value handling has very small impact on serialization and deserialization. Enabling it does not warrant the risk we run
            // of changing how our consumers get their data. so we leave it at the default value
            NullValueHandling = jsonSettings.NullValueHandling,
            DefaultValueHandling = jsonSettings.DefaultValueHandling,
            ObjectCreationHandling = jsonSettings.ObjectCreationHandling,
            PreserveReferencesHandling = jsonSettings.PreserveReferencesHandling,
            ConstructorHandling = jsonSettings.ConstructorHandling,
            MetadataPropertyHandling = jsonSettings.MetadataPropertyHandling,
            Formatting = jsonSettings.Formatting,
            FloatParseHandling = jsonSettings.FloatParseHandling,
            FloatFormatHandling = jsonSettings.FloatFormatHandling,
            StringEscapeHandling = jsonSettings.StringEscapeHandling,
            TypeNameAssemblyFormatHandling = jsonSettings.TypeNameAssemblyFormatHandling,
            Culture = jsonSettings.Culture,
            CheckAdditionalContent = jsonSettings.CheckAdditionalContent,
            DateFormatString = jsonSettings.DateFormatString,
            MaxDepth = jsonSettings.MaxDepth,
            ContractResolver = contractResolver,
        };

        FastSerializer = JsonSerializer.Create(FastJsonSettings);

        PayloadSerializerV1.ContractResolver = new TestPlatformContractResolver1();
        PayloadSerializerV2.ContractResolver = contractResolver;

#if TRACE_JSON_SERIALIZATION
        // MemoryTraceWriter can help diagnose serialization issues. Enable it for
        // debug builds only.
        // Note that MemoryTraceWriter is not thread safe, please don't use it in parallel
        // test runs. See https://github.com/JamesNK/Newtonsoft.Json/issues/1279
        payloadSerializer.TraceWriter = new MemoryTraceWriter();
        payloadSerializer2.TraceWriter = new MemoryTraceWriter();
#endif
    }



    /// <summary>
    /// Prevents a default instance of the <see cref="JsonDataSerializer"/> class from being created.
    /// </summary>
    private JsonDataSerializer() { }

    /// <summary>
    /// Gets the JSON Serializer instance.
    /// </summary>
    public static JsonDataSerializer Instance => s_instance ??= new JsonDataSerializer();

    /// <summary>
    /// Deserialize a <see cref="Message"/> from raw JSON text.
    /// </summary>
    /// <param name="rawMessage">JSON string.</param>
    /// <returns>A <see cref="Message"/> instance.</returns>
    public Message DeserializeMessage(string rawMessage)
    {
        if (DisableFastJson)
        {
            // PERF: This is slow, we deserialize the message, and the payload into JToken just to get the header. We then
            // deserialize the data from the JToken, but that is twice as expensive as deserializing the whole object directly into the final object type.
            // We need this for backward compatibility though.
            return Deserialize<VersionedMessage>(rawMessage)!;
        }

        // PERF: Try grabbing the version and message type from the string directly, we are pretty certain how the message is serialized
        // when the format does not match all we do is that we check if 6th character in the message is 'V'
        if (!FastHeaderParse(rawMessage, out int version, out string? messageType))
        {
            // PERF: If the fast path fails, deserialize into header object that does not have any Payload. When the message type info
            // is at the start of the message, this is also pretty fast. Again, this won't touch the payload.
            MessageHeader header = DeserializeObjectFast<MessageHeader>(rawMessage)!;
            version = header.Version;
            messageType = header.MessageType;
        }

        var message = new VersionedMessageWithRawMessage
        {
            Version = version,
            MessageType = messageType,
            RawMessage = rawMessage,
        };

        return message;
    }

    /// <summary>
    /// Deserialize the <see cref="Message.Payload"/> for a message.
    /// </summary>
    /// <param name="message">A <see cref="Message"/> object.</param>
    /// <typeparam name="T">Payload type.</typeparam>
    /// <returns>The deserialized payload.</returns>
    public T? DeserializePayload<T>(Message? message)
    {
        if (message is null)
        {
            return default;
        }

        if (message.GetType() == typeof(Message))
        {
            // Message is specifically a Message, and not any of it's child types like VersionedMessage.
            // Get the default serializer and deserialize. This would be used for any message from very old test host.
            //
            // Unit tests also provide a Message in places where using the deserializer would actually
            // produce a VersionedMessage or VersionedMessageWithRawMessage.
            var serializerV1 = GetPayloadSerializer(null);
            TPDebug.Assert(message.Payload is not null, "Payload should not be null");
            return Deserialize<T>(serializerV1, message.Payload);
        }

        var versionedMessage = (VersionedMessage)message;
        var payloadSerializer = GetPayloadSerializer(versionedMessage.Version);

        if (DisableFastJson)
        {
            // When fast json is disabled, then the message is a VersionedMessage
            // with JToken payload.
            TPDebug.Assert(message.Payload is not null, "Payload should not be null");
            return Deserialize<T>(payloadSerializer, message.Payload);
        }

        // When fast json is enabled then the message is also a subtype of VersionedMessage, but
        // the Payload is not populated, and instead the rawMessage string it passed as is.
        var messageWithRawMessage = (VersionedMessageWithRawMessage)message;
        var rawMessage = messageWithRawMessage.RawMessage;

        if (rawMessage == null)
        {
            return default;
        }

        // The deserialized message can still have a version (0 or 1), that should use the old deserializer
        if (payloadSerializer == PayloadSerializerV2)
        {
            // PERF: Fast path is compatibile only with protocol versions that use serializer_2,
            // and this is faster than deserializing via deserializer_2.
            var messageWithPayload = DeserializeObjectFast<PayloadedMessage<T>>(rawMessage);

            return messageWithPayload == null ? default : messageWithPayload.Payload;
        }
        else
        {
            // PERF: When payloadSerializer1 was resolved we need to deserialize JToken, and then deserialize that.
            // This is still better than deserializing the JToken in DeserializeMessage because here we know that the payload
            // will actually be used.
            TPDebug.Assert(rawMessage is not null, "rawMessage should not be null");
            var rawMessagePayload = Deserialize<Message>(rawMessage)?.Payload;
            TPDebug.Assert(rawMessagePayload is not null, "rawMessagePayload should not be null");
            return Deserialize<T>(payloadSerializer, rawMessagePayload);
        }
    }

    private static T? DeserializeObjectFast<T>(string value)
    {
        using JsonTextReader reader = new(new StringReader(value));
        return (T?)FastSerializer.Deserialize(reader, typeof(T));
    }

    private static bool FastHeaderParse(string rawMessage, out int version, out string? messageType)
    {
        // PERF: This can be also done slightly better using ReadOnlySpan<char> but we don't have that available by default in .NET Framework
        // and the speed improvement does not warrant additional dependency. This is already taking just few ms for 10k messages.
        version = 0;
        messageType = null;

        try
        {
            // The incoming messages look like this, or like this:
            // {"Version":6,"MessageType":"TestExecution.GetTestRunnerProcessStartInfoForRunAll","Payload":{
            // {"MessageType":"TestExecution.GetTestRunnerProcessStartInfoForRunAll","Payload":{
            if (rawMessage.Length < 31)
            {
                // {"MessageType":"T","Payload":1} with length 31 is the smallest valid message we should be able to parse..
                return false;
            }

            // If the message is not versioned then the start quote of the message type string is at index 15 {"MessageType":"
            int messageTypeStartQuoteIndex = 15;
            int versionInt = 0;
            if (rawMessage[2] == 'V')
            {
                // This is a potential versioned message that looks like this:
                // {"Version":6,"MessageType":"TestExecution.GetTestRunnerProcessStartInfoForRunAll","Payload":{

                // Version ':' is on index 10, the number starts at the next index. Find wher the next ',' is and grab that as number.
                var versionColonIndex = 10;
                if (rawMessage[versionColonIndex] != ':')
                {
                    return false;
                }

                var firstVersionNumberIndex = 11;
                // The message is versioned, get the version and update the position of first quote that contains message type.
                if (!TryGetSubstringUntilDelimiter(rawMessage, firstVersionNumberIndex, ',', maxSearchLength: 4, out string? versionString, out int versionCommaIndex))
                {
                    return false;
                }

                // Message type delmiter is at at versionCommaIndex + the length of '"MessageType":"' which is 15 chars
                messageTypeStartQuoteIndex = versionCommaIndex + 15;

                if (!int.TryParse(versionString, out versionInt))
                {
                    return false;
                }
            }
            else if (rawMessage[2] != 'M' || rawMessage[12] != 'e')
            {
                // Message is not versioned message, and it is also not message that starts with MessageType
                return false;
            }

            if (rawMessage[messageTypeStartQuoteIndex] != '"')
            {
                return false;
            }

            int messageTypeStartIndex = messageTypeStartQuoteIndex + 1;
            // "TestExecution.LaunchAdapterProcessWithDebuggerAttachedCallback" is the longest message type we currently have with 62 chars
            if (!TryGetSubstringUntilDelimiter(rawMessage, messageTypeStartIndex, '"', maxSearchLength: 100, out string? messageTypeString, out _))
            {
                return false;
            }

            version = versionInt;
            messageType = messageTypeString;
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///  Try getting substring until a given delimiter, but don't search more characters than maxSearchLength.
    /// </summary>
    private static bool TryGetSubstringUntilDelimiter(string rawMessage, int start, char character, int maxSearchLength, out string? substring, out int delimiterIndex)
    {
        var length = rawMessage.Length;
        var searchEnd = start + maxSearchLength;
        for (int i = start; i < length && i <= searchEnd; i++)
        {
            if (rawMessage[i] == character)
            {
                delimiterIndex = i;
                substring = rawMessage.Substring(start, i - start);
                return true;
            }
        }

        delimiterIndex = -1;
        substring = null;
        return false;
    }

    /// <summary>
    /// Deserialize raw JSON to an object using the default serializer.
    /// </summary>
    /// <param name="json">JSON string.</param>
    /// <param name="version">Version of serializer to be used.</param>
    /// <typeparam name="T">Target type to deserialize.</typeparam>
    /// <returns>An instance of <typeparamref name="T"/>.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public T? Deserialize<T>(string json, int version = 1)
    {
        var payloadSerializer = GetPayloadSerializer(version);
        return Deserialize<T>(payloadSerializer, json);
    }

    /// <summary>
    /// Serialize an empty message.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <returns>Serialized message.</returns>
    public string SerializeMessage(string? messageType)
    {
        return Serialize(Serializer, new Message { MessageType = messageType });
    }

    /// <summary>
    /// Serialize a message with payload.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="payload">Payload for the message.</param>
    /// <returns>Serialized message.</returns>
    public string SerializePayload(string? messageType, object? payload)
    {
        return SerializePayload(messageType, payload, 1);
    }

    /// <summary>
    /// Serialize a message with payload.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="payload">Payload for the message.</param>
    /// <param name="version">Version for the message.</param>
    /// <returns>Serialized message.</returns>
    public string SerializePayload(string? messageType, object? payload, int version)
    {
        var payloadSerializer = GetPayloadSerializer(version);
        // Fast json is only equivalent to the serialization that is used for protocol version 2 and upwards (or more precisely for the paths that use PayloadSerializerV2)
        // so when we resolved the old serializer we should use non-fast path.
        if (DisableFastJson || payloadSerializer == PayloadSerializerV1)
        {
            if (payload == null)
                return string.Empty;

            var serializedPayload = JToken.FromObject(payload, payloadSerializer);

            return version > 1 ?
                Serialize(Serializer, new VersionedMessage { MessageType = messageType, Version = version, Payload = serializedPayload }) :
                Serialize(Serializer, new Message { MessageType = messageType, Payload = serializedPayload });
        }
        else
        {
            return Serialize(FastSerializer, new VersionedMessageForSerialization { MessageType = messageType, Version = version, Payload = payload });
        }
    }

    /// <summary>
    /// Serialize an object to JSON using default serialization settings.
    /// </summary>
    /// <typeparam name="T">Type of object to serialize.</typeparam>
    /// <param name="data">Instance of the object to serialize.</param>
    /// <param name="version">Version to be stamped.</param>
    /// <returns>JSON string.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Part of the public API")]
    public string Serialize<T>(T data, int version = 1)
    {
        var payloadSerializer = GetPayloadSerializer(version);
        return Serialize(payloadSerializer, data);
    }

    /// <inheritdoc/>
    [return: NotNullIfNotNull("obj")]
    public T? Clone<T>(T? obj)
    {
        if (obj == null)
        {
            return default;
        }

        var stringObj = Serialize(obj, 2);
        return Deserialize<T>(stringObj, 2)!;
    }

    /// <summary>
    /// Serialize data.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    /// <param name="serializer">Serializer.</param>
    /// <param name="data">Data to be serialized.</param>
    /// <returns>Serialized data.</returns>
    private static string Serialize<T>(JsonSerializer serializer, T data)
    {
        using var stringWriter = new StringWriter();
        using var jsonWriter = new JsonTextWriter(stringWriter);
        serializer.Serialize(jsonWriter, data);
        return stringWriter.ToString();
    }

    /// <summary>
    /// Deserialize data.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    /// <param name="serializer">Serializer.</param>
    /// <param name="data">Data to be deserialized.</param>
    /// <returns>Deserialized data.</returns>
    private static T? Deserialize<T>(JsonSerializer serializer, string data)
    {
        using var stringReader = new StringReader(data);
        using var jsonReader = new JsonTextReader(stringReader);
        return serializer.Deserialize<T>(jsonReader);
    }

    /// <summary>
    /// Deserialize JToken object to T object.
    /// </summary>
    /// <typeparam name="T">Type of data.</typeparam>
    /// <param name="serializer">Serializer.</param>
    /// <param name="jToken">JToken to be deserialized.</param>
    /// <returns>Deserialized data.</returns>
    private static T Deserialize<T>(JsonSerializer serializer, JToken jToken)
    {
        return jToken.ToObject<T>(serializer)!;
    }

    private static JsonSerializer GetPayloadSerializer(int? version)
    {
        version ??= 1;

        return version switch
        {
            // 0 is used during negotiation.
            // Protocol version 3 was accidentally used with serializer v1 and not
            // serializer v2, we downgrade to protocol 2 when 3 would be negotiated
            // unless this is disabled by VSTEST_DISABLE_PROTOCOL_3_VERSION_DOWNGRADE
            // env variable.
            0 or 1 or 3 => PayloadSerializerV1,
            2 or 4 or 5 or 6 or 7 => PayloadSerializerV2,

            _ => throw new NotSupportedException($"Protocol version {version} is not supported. "
                + "Ensure it is compatible with the latest serializer or add a new one."),
        };
    }

    /// <summary>
    /// Just the header from versioned messages, to avoid touching the Payload when we deserialize message.
    /// </summary>
    private class MessageHeader
    {
        public int Version { get; set; }
        public string? MessageType { get; set; }
    }

    /// <summary>
    /// Container for the rawMessage string, to avoid changing how messages are passed.
    /// This allows us to pass MessageWithRawMessage the same way that Message is passed for protocol version 1.
    /// And VersionedMessage is passed for later protocol versions, but without touching the payload string when we just
    /// need to know the header.
    /// !! This message does not populate the Payload property even though it is still present because that comes from Message.
    /// </summary>
    private class VersionedMessageWithRawMessage : VersionedMessage
    {
        public string? RawMessage { get; set; }

        public override string ToString()
        {
            return $"({MessageType}) -> {RawMessage}";
        }
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
    /// For serialization directly into string, without first converting to JToken, and then from JToken to string.
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
