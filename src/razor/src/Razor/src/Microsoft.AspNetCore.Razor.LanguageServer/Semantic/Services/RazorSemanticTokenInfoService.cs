// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal abstract class RazorSemanticTokensInfoService
{
    public abstract Task<SemanticTokens?> GetSemanticTokensAsync(TextDocumentIdentifier textDocumentIdentifier, Range range, VersionedDocumentContext documentContext, CancellationToken cancellationToken);
}
