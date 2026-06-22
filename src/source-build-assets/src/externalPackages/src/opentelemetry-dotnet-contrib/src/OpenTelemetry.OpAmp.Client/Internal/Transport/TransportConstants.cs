// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Internal.Transport;

internal static class TransportConstants
{
    /// <summary>
    /// Maximum allowed size of a single OpAMP message received from the server (128 KB).
    /// Applies to both HTTP and WebSocket transports. Responses exceeding this limit
    /// are rejected to prevent uncontrolled memory allocation.
    /// </summary>
    /// <remarks>
    /// For WebSocket transport, the limit is enforced after each <c>ReceiveAsync</c> increment,
    /// so the client may briefly buffer up to this many bytes plus at most one full receive
    /// buffer worth of additional payload before the connection is closed. That tradeoff keeps
    /// streaming reads bounded without requiring a length prefix on every frame.
    /// </remarks>
    public const int MaxMessageSize = 128 * 1024;
}
