// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal interface IHtmlCodeActionResolver : ICodeActionResolver
{
    Task<CodeAction> ResolveAsync(DocumentContext documentContext, CodeAction codeAction, CancellationToken cancellationToken);
}
