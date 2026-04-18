// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.IO;

using Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference;

/// <summary>
/// Original Newtonsoft-based JsonDataSerializer, extracted from main for comparison testing.
/// Always uses the standard (non-fast) serialization path for deterministic comparison.
/// </summary>
internal class NewtonsoftJsonDataSerializer
{
    private static NewtonsoftJsonDataSerializer? s_instance;

    private static readonly JsonSerializer PayloadSerializerV1; // payload serializer for version <= 1
    private static readonly JsonSerializer PayloadSerializerV2; // payload serializer for version >= 2
    private static readonly JsonSerializer Serializer; // generic serializer

    static NewtonsoftJsonDataSerializer()
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

        Serializer = JsonSerializer.Create();
        PayloadSerializerV1 = JsonSerializer.Create(jsonSettings);
        PayloadSerializerV2 = JsonSerializer.Create(jsonSettings);

        PayloadSerializerV1.ContractResolver = new NewtonsoftTestPlatformContractResolver1();
        PayloadSerializerV2.ContractResolver = new NewtonsoftDefaultTestPlatformContractResolver();
    }

    /// <summary>
    /// Prevents a default instance of the <see cref="NewtonsoftJsonDataSerializer"/> class from being created.
    /// </summary>
    private NewtonsoftJsonDataSerializer() { }

    /// <summary>
    /// Gets the JSON Serializer instance.
    /// </summary>
    public static NewtonsoftJsonDataSerializer Instance => s_instance ??= new NewtonsoftJsonDataSerializer();

    /// <summary>
    /// Deserialize a <see cref="NewtonsoftMessage"/> from raw JSON text.
    /// </summary>
    /// <param name="rawMessage">JSON string.</param>
    /// <returns>A <see cref="NewtonsoftMessage"/> instance.</returns>
    public NewtonsoftMessage DeserializeMessage(string rawMessage)
    {
        // Always use the standard (non-fast) path: deserialize including the JToken payload.
        return Deserialize<NewtonsoftVersionedMessage>(rawMessage)!;
    }

    /// <summary>
    /// Deserialize the <see cref="NewtonsoftMessage.Payload"/> for a message.
    /// </summary>
    /// <param name="message">A <see cref="NewtonsoftMessage"/> object.</param>
    /// <typeparam name="T">Payload type.</typeparam>
    /// <returns>The deserialized payload.</returns>
    public T? DeserializePayload<T>(NewtonsoftMessage? message)
    {
        if (message is null)
        {
            return default;
        }

        if (message.GetType() == typeof(NewtonsoftMessage))
        {
            var serializerV1 = GetPayloadSerializer(null);
            System.Diagnostics.Debug.Assert(message.Payload is not null, "Payload should not be null");
            return Deserialize<T>(serializerV1, message.Payload!);
        }

        var versionedMessage = (NewtonsoftVersionedMessage)message;
        var payloadSerializer = GetPayloadSerializer(versionedMessage.Version);

        // Standard path: message has JToken payload.
        System.Diagnostics.Debug.Assert(message.Payload is not null, "Payload should not be null");
        return Deserialize<T>(payloadSerializer, message.Payload!);
    }

    /// <summary>
    /// Deserialize raw JSON to an object using the default serializer.
    /// </summary>
    /// <param name="json">JSON string.</param>
    /// <param name="version">Version of serializer to be used.</param>
    /// <typeparam name="T">Target type to deserialize.</typeparam>
    /// <returns>An instance of <typeparamref name="T"/>.</returns>
    public T? Deserialize<T>(string json, int version = 1)
    {
        var payloadSerializer = GetPayloadSerializer(version);
        return Deserialize<T>(payloadSerializer, json);
    }

    /// <summary>
    /// Serialize an empty message (no payload).
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <returns>Serialized message.</returns>
    public string SerializeMessage(string? messageType)
    {
        return Serialize(Serializer, new NewtonsoftMessage { MessageType = messageType });
    }

    /// <summary>
    /// Serialize a message with payload (defaults to version 1).
    /// </summary>
    public string SerializePayload(string? messageType, object? payload)
    {
        return SerializePayload(messageType, payload, 1);
    }

    /// <summary>
    /// Serialize data using the version-appropriate serializer.
    /// </summary>
    public string Serialize<T>(T data, int version = 1)
    {
        var payloadSerializer = GetPayloadSerializer(version);
        return Serialize(payloadSerializer, data);
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

        // Always use the standard (non-fast) path.
        if (payload == null)
            return string.Empty;

        var serializedPayload = JToken.FromObject(payload, payloadSerializer);

        return version > 1 ?
            Serialize(Serializer, new NewtonsoftVersionedMessage { MessageType = messageType, Version = version, Payload = serializedPayload }) :
            Serialize(Serializer, new NewtonsoftMessage { MessageType = messageType, Payload = serializedPayload });
    }

    /// <summary>
    /// Serialize data.
    /// </summary>
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
    private static T? Deserialize<T>(JsonSerializer serializer, string data)
    {
        using var stringReader = new StringReader(data);
        using var jsonReader = new JsonTextReader(stringReader);
        return serializer.Deserialize<T>(jsonReader);
    }

    /// <summary>
    /// Deserialize JToken object to T object.
    /// </summary>
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
            // serializer v2, we downgrade to protocol 2 when 3 would be negotiated.
            0 or 1 or 3 => PayloadSerializerV1,
            2 or 4 or 5 or 6 or 7 => PayloadSerializerV2,

            _ => throw new NotSupportedException($"Protocol version {version} is not supported. "
                + "Ensure it is compatible with the latest serializer or add a new one."),
        };
    }
}
