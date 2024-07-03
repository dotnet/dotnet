// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

internal class DelegatedCodeActionParams
{
    [JsonPropertyName("hostDocumentVersion")]
    public int HostDocumentVersion { get; set; }

    [JsonPropertyName("codeActionParams")]
    public required VSCodeActionParams CodeActionParams { get; set; }

    [JsonPropertyName("languageKind")]
    public RazorLanguageKind LanguageKind { get; set; }

    [JsonPropertyName("correlationId")]
    public Guid CorrelationId { get; set; }
}
