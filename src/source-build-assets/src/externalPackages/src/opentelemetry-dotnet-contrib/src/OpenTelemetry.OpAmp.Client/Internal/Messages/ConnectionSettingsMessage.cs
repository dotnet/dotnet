// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Messages;

namespace OpenTelemetry.OpAmp.Client.Internal.Listeners.Messages;

internal class ConnectionSettingsMessage : OpAmpMessage
{
    public ConnectionSettingsMessage(ConnectionSettingsOffers connectionSettingsOffers)
    {
        this.ConnectionSettings = connectionSettingsOffers;
    }

    public ConnectionSettingsOffers ConnectionSettings { get; set; }
}
