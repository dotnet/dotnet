// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.SemanticTokens;

internal interface IRazorSemanticTokensInfoService
{
    /// <summary>
    /// Gets the int array representing the semantic tokens for the given range.
    /// </summary>
    /// <remarks>See https://microsoft.github.io/language-server-protocol/specifications/lsp/3.17/specification/#textDocument_semanticTokens for details about the int array</remarks>
    Task<int[]?> GetSemanticTokensAsync(VersionedDocumentContext documentContext, LinePositionSpan range, bool colorBackground, Guid correlationId, CancellationToken cancellationToken);
}
