// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Connection states.
    /// </summary>
    /// <remarks>Member order is significant.
    /// For example, any connection state before <see cref="ConnectionState.Connecting" />
    /// implies no connection.</remarks>
    public enum ConnectionState
    {
        FailedToHandshake,
        Closing,
        Closed,
        ReadyToConnect,
        Connecting,
        Handshaking,
        Connected
    }
}
