// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Models;

internal sealed class RazorCodeActionResolutionParams
{
    // NOTE: Capital T here is required to match Roslyn's DocumentResolveData structure, so that the Roslyn
    //       language server can correctly route requests to us in cohosting. In future when we normalize
    //       on to Roslyn types, we should inherit from that class so we don't have to remember to do this.
    [JsonPropertyName("TextDocument")]
    public required VSTextDocumentIdentifier TextDocument { get; set; }

    [JsonPropertyName("action")]
    public required string Action { get; set; }

    [JsonPropertyName("language")]
    public required RazorLanguageKind Language { get; set; }

    [JsonPropertyName("delegatedDocumentUri")]
    public required Uri? DelegatedDocumentUri { get; set; }

    [JsonPropertyName("data")]
    public object? Data { get; set; }
}
