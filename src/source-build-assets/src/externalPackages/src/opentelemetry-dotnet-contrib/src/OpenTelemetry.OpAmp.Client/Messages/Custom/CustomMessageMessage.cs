// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;
using OpAmp.Proto.V1;

namespace OpenTelemetry.OpAmp.Client.Messages;

/// <summary>
/// Represents an OpAMP server-to-agent custom message message.
/// </summary>
public class CustomMessageMessage : OpAmpMessage
{
    private readonly ByteString data;

    internal CustomMessageMessage(CustomMessage customMessage)
    {
        this.data = customMessage.Data;

        this.Capability = customMessage.Capability;
        this.Type = customMessage.Type;
    }

    /// <summary>
    /// Gets the capability associated with custom message.
    /// </summary>
    public string Capability { get; }

    /// <summary>
    /// Gets the type of custom message.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the data of custom message.
    /// </summary>
    public ReadOnlySpan<byte> Data => this.data.Span;
}
