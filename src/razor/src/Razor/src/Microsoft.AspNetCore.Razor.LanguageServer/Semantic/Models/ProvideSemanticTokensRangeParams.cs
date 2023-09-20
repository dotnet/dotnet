// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;

internal class ProvideSemanticTokensRangeParams : SemanticTokensParams
{
    [DataMember(Name = "requiredHostDocumentVersion", IsRequired = true)]
    public long RequiredHostDocumentVersion { get; }

    [DataMember(Name = "range", IsRequired = true)]
    public Range Range { get; }

    [DataMember(Name = "correlationId", IsRequired = true)]
    public Guid CorrelationId { get; }

    public ProvideSemanticTokensRangeParams(TextDocumentIdentifier textDocument, long requiredHostDocumentVersion, Range range, Guid correlationId)
    {
        TextDocument = textDocument;
        RequiredHostDocumentVersion = requiredHostDocumentVersion;
        Range = range;
        CorrelationId = correlationId;
    }
}
