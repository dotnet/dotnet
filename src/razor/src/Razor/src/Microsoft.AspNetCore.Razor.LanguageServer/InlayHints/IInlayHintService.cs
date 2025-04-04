// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;

internal interface IInlayHintService
{
    Task<InlayHint[]?> GetInlayHintsAsync(IClientConnection clientConnection, DocumentContext documentContext, LspRange range, CancellationToken cancellationToken);

    Task<InlayHint?> ResolveInlayHintAsync(IClientConnection clientConnection, InlayHint inlayHint, CancellationToken cancellationToken);
}
