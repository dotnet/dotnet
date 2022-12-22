// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// Construct used for communication
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the message type.
    /// </summary>
    public string? MessageType { get; set; }

    /// <summary>
    /// Gets or sets the payload.
    /// </summary>
    // TODO: Our public contract says that we should be able to communicate over JSON, but we should not be stopping ourselves from
    // negotiating a different protocol. Or using a different serialization library than NewtonsoftJson. Check why this is published as JToken
    // and not as a string.
    public JToken? Payload { get; set; }

    /// <summary>
    /// To string implementation.
    /// </summary>
    /// <returns> The <see cref="string"/>. </returns>
    public override string ToString()
    {
        // TODO: Review where this is used, we should avoid extensive serialization and deserialization,
        // and this might be happening in multiple places that are not the edge of our process.
        return $"({MessageType}) -> {(Payload == null ? "null" : Payload.ToString(Formatting.Indented))}";
    }
}
