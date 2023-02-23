// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Editor;

/// <summary>
/// Settings that are set and handled on the client, but needed by the LSP Server to function correctly. When these are
/// updated a workspace/didchangeconfiguration should be sent from client to the server. Then the server requests
/// workspace/configuration to get the latest settings. For VS, the razor protocol also handles this and serializes the
/// settings back to the server.
/// </summary>
/// <param name="ClientSpaceSettings"></param>
/// <param name="AdvancedSettings"></param>
public record ClientSettings(ClientSpaceSettings ClientSpaceSettings, ClientAdvancedSettings AdvancedSettings)
{
    public static readonly ClientSettings Default = new(ClientSpaceSettings.Default, ClientAdvancedSettings.Default);
}

public sealed record ClientSpaceSettings(bool IndentWithTabs, int IndentSize)
{
    public static readonly ClientSpaceSettings Default = new(IndentWithTabs: false, IndentSize: 4);

    public int IndentSize { get; } = IndentSize >= 0 ? IndentSize : throw new ArgumentOutOfRangeException(nameof(IndentSize));
}

public sealed record ClientAdvancedSettings(bool FormatOnType)
{
    public static readonly ClientAdvancedSettings Default = new(FormatOnType: true);
}
