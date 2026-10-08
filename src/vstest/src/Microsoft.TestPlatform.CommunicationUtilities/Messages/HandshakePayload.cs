// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// Payload for one message of the connection handshake, carrying the secret the sender presents to
/// prove which side of the pair it is. Message.ToString redacts this payload, so the value does not
/// reach a diagnostic log.
/// </summary>
public class HandshakePayload
{
    /// <summary>
    /// Gets or sets the secret the sender presents. The child presents its secret first, and the
    /// parent presents its own only after the child's has matched.
    /// </summary>
    public string? Secret { get; set; }
}