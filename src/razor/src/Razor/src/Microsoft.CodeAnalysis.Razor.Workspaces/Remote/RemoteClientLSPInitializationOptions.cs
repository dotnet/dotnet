// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal struct RemoteClientLSPInitializationOptions
{
    [JsonPropertyName("tokenTypes")]
    public required string[] TokenTypes { get; set; }

    [JsonPropertyName("tokenModifiers")]
    public required string[] TokenModifiers { get; set; }

    [JsonPropertyName("clientCapabilities")]
    public required VSInternalClientCapabilities ClientCapabilities { get; set; }
}
