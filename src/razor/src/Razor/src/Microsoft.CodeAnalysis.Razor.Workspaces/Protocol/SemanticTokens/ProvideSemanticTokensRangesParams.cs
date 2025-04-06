// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;

internal class ProvideSemanticTokensRangesParams : SemanticTokensParams
{
    [JsonPropertyName("requiredHostDocumentVersion")]
    public int RequiredHostDocumentVersion { get; }

    [JsonPropertyName("ranges")]
    public LspRange[] Ranges { get; }

    [JsonPropertyName("correlationId")]
    public Guid CorrelationId { get; }

    public ProvideSemanticTokensRangesParams(TextDocumentIdentifier textDocument, int requiredHostDocumentVersion, LspRange[] ranges, Guid correlationId)
    {
        TextDocument = textDocument;
        RequiredHostDocumentVersion = requiredHostDocumentVersion;
        Ranges = ranges;
        CorrelationId = correlationId;
    }
}
