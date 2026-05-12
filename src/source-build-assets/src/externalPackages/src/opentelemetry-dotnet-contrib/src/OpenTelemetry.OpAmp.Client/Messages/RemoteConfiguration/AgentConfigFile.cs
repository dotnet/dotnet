// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;

namespace OpenTelemetry.OpAmp.Client.Messages;

/// <summary>
/// Represents an agent configuration file.
/// </summary>
public class AgentConfigFile
{
    private readonly ByteString body;

    internal AgentConfigFile(string name, global::OpAmp.Proto.V1.AgentConfigFile agentConfigFile)
    {
        this.body = agentConfigFile.Body ?? ByteString.Empty;
        this.ContentType = agentConfigFile.ContentType;
        this.Name = name;
    }

    /// <summary>
    /// Gets the byte content of the configuration file.
    /// </summary>
    public ReadOnlySpan<byte> Body => this.body.Span;

    /// <summary>
    /// Gets the MIME Content-Type that describes the data contained in the body of the remote configuration file.
    /// </summary>
    public string? ContentType { get; }

    /// <summary>
    /// Gets the name of this configuration file.
    /// </summary>
    public string Name { get; }
}
