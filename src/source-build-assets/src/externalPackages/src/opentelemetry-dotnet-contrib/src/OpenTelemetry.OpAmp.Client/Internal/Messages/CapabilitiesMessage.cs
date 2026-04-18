// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Messages;

namespace OpenTelemetry.OpAmp.Client.Internal.Listeners.Messages;

internal class CapabilitiesMessage : OpAmpMessage
{
    public CapabilitiesMessage(ServerCapabilities capabilities)
    {
        this.Capabilities = capabilities;
    }

    public ServerCapabilities Capabilities { get; set; }
}
