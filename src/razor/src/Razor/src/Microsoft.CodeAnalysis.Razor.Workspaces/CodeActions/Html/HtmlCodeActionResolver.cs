// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal class HtmlCodeActionResolver(IEditMappingService editMappingService) : IHtmlCodeActionResolver
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
