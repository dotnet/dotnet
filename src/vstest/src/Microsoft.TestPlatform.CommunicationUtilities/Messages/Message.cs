// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// Represents a communication message exchanged between vstest processes.
/// Contains the message type (routing key), protocol version, and the raw
/// JSON wire data. The payload is not deserialized until explicitly requested
/// via <see cref="JsonDataSerializer.DeserializePayload{T}"/>.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the message type (routing key).
    /// </summary>
    public string? MessageType { get; set; }

    /// <summary>
    /// Gets or sets the protocol version.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Gets or sets the raw JSON message as received from the wire.
    /// This contains the full message including MessageType, Version, and Payload.
    /// </summary>
    public string? RawMessage { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return RawMessage ?? "{}";
    }
}
