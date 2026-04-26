// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// JsonDataSerializes serializes and deserializes data using Json format
/// </summary>
public partial class JsonDataSerializer : IDataSerializer
{
    private static JsonDataSerializer? s_instance;

    /// <summary>
    /// Gets the name of the underlying serializer (for diagnostics/tests).
    /// </summary>
#if NETCOREAPP
    internal static string SerializerName => "System.Text.Json";
#else
    internal static string SerializerName => "Jsonite";
#endif

    /// <summary>
    /// Prevents a default instance of the <see cref="JsonDataSerializer"/> class from being created.
    /// </summary>
    private JsonDataSerializer() { }

    private static bool s_serializerNameLogged;

    private static void LogSerializerNameOnce()
    {
        if (!s_serializerNameLogged && EqtTrace.IsInfoEnabled)
        {
            s_serializerNameLogged = true;
            EqtTrace.Info("JsonDataSerializer: Using {0} serializer", SerializerName);
        }
    }

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
        LogSerializerNameOnce();

        // Try fast header parse first (string parsing, no JSON library)
        if (!FastHeaderParse(rawMessage, out int version, out string? messageType))
        {
            // Fallback: parse just the header fields from JSON
            (version, messageType) = ParseHeaderFromJson(rawMessage);
        }

        return new Message
        {
            Version = version,
            MessageType = messageType,
            RawMessage = rawMessage,
        };
    }

    /// <summary>
    /// Deserialize the payload from a <see cref="Message"/>.
    /// </summary>
    /// <param name="message">A <see cref="Message"/> object.</param>
    /// <typeparam name="T">Payload type.</typeparam>
    /// <returns>The deserialized payload.</returns>
    public T? DeserializePayload<T>(Message? message)
    {
        if (message is null || message.RawMessage is null)
        {
            return default;
        }

        return DeserializePayloadCore<T>(message);
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
        return DeserializeCore<T>(json, version);
    }

    /// <summary>
    /// Serialize an empty message.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    /// <returns>Serialized message.</returns>
    public string SerializeMessage(string? messageType)
    {
        return SerializeMessageCore(messageType);
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
        return SerializePayloadCore(messageType, payload, version);
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
        return SerializeCore(data, version);
    }

    /// <inheritdoc/>
    [return: NotNullIfNotNull("obj")]
    public T? Clone<T>(T? obj)
    {
        if (obj is null)
        {
            return default;
        }

        var stringObj = Serialize(obj, 2);
        return Deserialize<T>(stringObj, 2)!;
    }

    private static partial (int version, string? messageType) ParseHeaderFromJson(string rawMessage);
    private static partial T? DeserializePayloadCore<T>(Message message);
    private static partial T? DeserializeCore<T>(string json, int version);
    private static partial string SerializeMessageCore(string? messageType);
    private static partial string SerializePayloadCore(string? messageType, object? payload, int version);
    private static partial string SerializeCore<T>(T data, int version);
}
