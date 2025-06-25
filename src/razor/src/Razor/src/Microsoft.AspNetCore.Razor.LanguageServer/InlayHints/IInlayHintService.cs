// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
