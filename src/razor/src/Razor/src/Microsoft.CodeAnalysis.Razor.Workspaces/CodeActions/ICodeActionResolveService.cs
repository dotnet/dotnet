// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal interface ICodeActionResolveService
{
    Task<CodeAction> ResolveCodeActionAsync(DocumentContext documentContext, CodeAction request, CodeAction? resolvedDelegatedCodeAction, RazorFormattingOptions options, CancellationToken cancellationToken);
}
