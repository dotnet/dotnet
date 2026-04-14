// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpAmp.Proto.V1;

namespace OpenTelemetry.OpAmp.Client.Messages;

/// <summary>
/// Represents an OpAMP server-to-agent custom capabilities message.
/// </summary>
public class CustomCapabilitiesMessage : OpAmpMessage
{
    internal CustomCapabilitiesMessage(CustomCapabilities customCapabilities)
    {
        this.Capabilities = [.. customCapabilities.Capabilities];
    }

    /// <summary>
    /// Gets the collection of custom capabilities.
    /// </summary>
    public ICollection<string> Capabilities { get; }
}
