// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal sealed class HtmlCodeActionResolver(IEditMappingService editMappingService) : IHtmlCodeActionResolver
{
    private readonly IEditMappingService _editMappingService = editMappingService;

    public string Action => LanguageServerConstants.CodeActions.Default;

    public async Task<CodeAction> ResolveAsync(
        DocumentContext documentContext,
        CodeAction codeAction,
        CancellationToken cancellationToken)
    {
        await HtmlCodeActionProvider.RemapAndFixHtmlCodeActionEditAsync(_editMappingService, documentContext.Snapshot, codeAction, cancellationToken).ConfigureAwait(false);

        return codeAction;
    }
}
