// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Settings;

/// <summary>
/// Specifies the type of transport protocol to be used for communication.
/// </summary>
/// <remarks>This enumeration defines the available transport protocols for communication. Use <see
/// cref="WebSocket"/> for WebSocket-based communication, or <see cref="Http"/> for
/// HTTP-based communication.</remarks>
public enum ConnectionType
{
    /// <summary>
    /// Use HTTP transport.
    /// </summary>
    Http = 0,

    /// <summary>
    /// Use WebSocket transport.
    /// </summary>
    WebSocket = 1,
}
