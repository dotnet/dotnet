// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal interface IRazorSemanticTokensInfoService
{
    Task<SemanticTokens?> GetSemanticTokensAsync(TextDocumentIdentifier textDocumentIdentifier, Range range, VersionedDocumentContext documentContext, RazorSemanticTokensLegend razorSemanticTokensLegend, Guid correlationId, CancellationToken cancellationToken);
}
