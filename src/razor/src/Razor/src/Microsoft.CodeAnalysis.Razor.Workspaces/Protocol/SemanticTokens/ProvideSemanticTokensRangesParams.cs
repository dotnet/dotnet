// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.SemanticTokens;

internal class ProvideSemanticTokensRangesParams : SemanticTokensParams
{
    [JsonPropertyName("requiredHostDocumentVersion")]
    public long RequiredHostDocumentVersion { get; }

    [JsonPropertyName("ranges")]
    public Range[] Ranges { get; }

    [JsonPropertyName("correlationId")]
    public Guid CorrelationId { get; }

    public ProvideSemanticTokensRangesParams(TextDocumentIdentifier textDocument, long requiredHostDocumentVersion, Range[] ranges, Guid correlationId)
    {
        TextDocument = textDocument;
        RequiredHostDocumentVersion = requiredHostDocumentVersion;
        Ranges = ranges;
        CorrelationId = correlationId;
    }
}
