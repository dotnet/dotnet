// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal interface IDelegatedCodeActionResolver
{
    Task<CodeAction?> ResolveCodeActionAsync(TextDocumentIdentifier razorFileIdentifier, int hostDocumentVersion, RazorLanguageKind languageKind, CodeAction codeAction, CancellationToken cancellationToken);
}
