// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

/// <summary>
/// IDataSerializer interface for serializing data
/// </summary>
public interface IDataSerializer
{
    /// <summary>
    /// Deserializes the raw message into Message
    /// </summary>
    /// <param name="rawMessage">Raw message off the IPC channel</param>
    /// <returns>Message object</returns>
    Message DeserializeMessage(string rawMessage);

    /// <summary>
    /// Deserializes the Message into actual TestPlatform objects
    /// </summary>
    /// <typeparam name="T"> The type of object to deserialize to. </typeparam>
    /// <param name="message"> Message object </param>
    /// <returns> TestPlatform object </returns>
    T? DeserializePayload<T>(Message? message);

    /// <summary>
    /// Serializes and creates a raw message given a message type
    /// </summary>
    /// <param name="messageType">Message Type</param>
    /// <returns>Raw Serialized message</returns>
    string SerializeMessage(string? messageType);

    /// <summary>
    /// Serializes and creates a raw message given a message type and the object payload
    /// </summary>
    /// <param name="messageType">Message Type</param>
    /// <param name="payload">Payload of the message</param>
    /// <returns>Raw Serialized message</returns>
    string SerializePayload(string? messageType, object? payload);

    /// <summary>
    /// Serializes and creates a raw message given a message type and the object payload
    /// </summary>
    /// <param name="messageType">Message Type</param>
    /// <param name="payload">Payload of the message</param>
    /// <param name="version">version to be sent</param>
    /// <returns>Raw Serialized message</returns>
    string SerializePayload(string? messageType, object? payload, int version);

    /// <summary>
    /// Creates cloned object for given object.
    /// </summary>
    /// <typeparam name="T"> The type of object to be cloned. </typeparam>
    /// <param name="obj">Object to be cloned.</param>
    /// <returns>Newly cloned object.</returns>
    T? Clone<T>(T? obj);
}
