// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.SpellCheck;

namespace Microsoft.AspNetCore.Razor.LanguageServer.SpellCheck;

[RazorLanguageServerEndpoint(VSInternalMethods.TextDocumentSpellCheckableRangesName)]
internal sealed class DocumentSpellCheckEndpoint(
    ISpellCheckService spellCheckService) : IRazorRequestHandler<VSInternalDocumentSpellCheckableParams, VSInternalSpellCheckableRangeReport[]?>, ICapabilitiesProvider
{
    private readonly ISpellCheckService _spellCheckService = spellCheckService;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.SpellCheckingProvider = true;
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(VSInternalDocumentSpellCheckableParams request)
        => request.TextDocument;

    public async Task<VSInternalSpellCheckableRangeReport[]?> HandleRequestAsync(VSInternalDocumentSpellCheckableParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var data = await _spellCheckService.GetSpellCheckRangeTriplesAsync(documentContext, cancellationToken).ConfigureAwait(false);

        return
            [
                new VSInternalSpellCheckableRangeReport
                {
                    Ranges = data,
                    ResultId = Guid.NewGuid().ToString()
                }
            ];
    }
}
