// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;

internal sealed class AddUsingsCodeActionParams
{
    [JsonPropertyName("uri")]
    public required Uri Uri { get; set; }

    [JsonPropertyName("namespace")]
    public required string Namespace { get; set; }

    [JsonPropertyName("additionalEdit")]
    public TextDocumentEdit? AdditionalEdit { get; set; }
}
