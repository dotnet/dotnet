// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol.Debugging;

internal class RazorBreakpointSpanParams
{
    [JsonPropertyName("uri")]
    public required Uri Uri { get; init; }

    [JsonPropertyName("position")]
    public required Position Position { get; init; }
}
