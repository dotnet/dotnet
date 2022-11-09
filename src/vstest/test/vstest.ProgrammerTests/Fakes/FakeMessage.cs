// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace vstest.ProgrammerTests.Fakes;

/// <summary>
/// Marker for Fake message so we can put put all FakeMessages into one collection, without making it too wide.
/// </summary>
internal abstract class FakeMessage
{
    /// <summary>
    /// The message serialized using the default JsonDataSerializer.
    /// </summary>
    // TODO: Is there a better way to ensure that is is not null, we will always set it in the inherited types, but it would be nice to have warning if we did not.
    // And adding constructor makes it difficult to use the serializer, especially if we wanted to the serializer dynamic and not a static instance.
    public string SerializedMessage { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public static FakeMessage NoResponse { get; } = new FakeMessage<int>("NoResponse", 0);
}

/// <summary>
/// A class like Message / VersionedMessage that is easier to create and review during debugging.
/// </summary>
internal sealed class FakeMessage<T> : FakeMessage
{
    public FakeMessage(string messageType, T payload, int version = 0)
    {
        MessageType = messageType;
        Payload = payload;
        Version = version;
        SerializedMessage = JsonDataSerializer.Instance.SerializePayload(MessageType, payload, version);
    }

    /// <summary>
    /// Message identifier, usually coming from the MessageType class.
    /// </summary>
    public string MessageType { get; }

    /// <summary>
    /// The payload that this message is holding.
    /// </summary>
    public T Payload { get; }

    /// <summary>
    /// Version of the message to allow the internal serializer to choose the correct serialization strategy.
    /// </summary>
    public int Version { get; }

    public override string ToString()
    {
        return $"{MessageType} {{{Payload}}}";
    }
}
