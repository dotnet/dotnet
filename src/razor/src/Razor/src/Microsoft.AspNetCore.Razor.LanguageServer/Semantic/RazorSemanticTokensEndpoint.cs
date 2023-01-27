// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class RazorSemanticTokensEndpoint : ISemanticTokensRangeEndpoint
{
    public bool MutatesSolutionState { get; } = false;

    public RazorSemanticTokensEndpoint()
    {
    }

    public async Task<SemanticTokens?> HandleRequestAsync(SemanticTokensRangeParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var documentContext = requestContext.GetRequiredDocumentContext();
        var semanticTokensInfoService = requestContext.GetRequiredService<RazorSemanticTokensInfoService>();

        var semanticTokens = await semanticTokensInfoService.GetSemanticTokensAsync(request.TextDocument, request.Range, documentContext, cancellationToken);
        var amount = semanticTokens is null ? "no" : (semanticTokens.Data.Length / 5).ToString(Thread.CurrentThread.CurrentCulture);

        requestContext.Logger.LogInformation("Returned {amount} semantic tokens for range {request.Range} in {request.TextDocument.Uri}.", amount, request.Range, request.TextDocument.Uri);

        if (semanticTokens is not null)
        {
            Debug.Assert(semanticTokens.Data.Length % 5 == 0, $"Number of semantic token-ints should be divisible by 5. Actual number: {semanticTokens.Data.Length}");
            Debug.Assert(semanticTokens.Data.Length == 0 || semanticTokens.Data[0] >= 0, $"Line offset should not be negative.");
        }

        return semanticTokens;
    }

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string ServerCapability = "semanticTokensProvider";

        return new RegistrationExtensionResult(ServerCapability,
            new SemanticTokensOptions
            {
                Full = false,
                Legend = RazorSemanticTokensLegend.Instance,
                Range = true,
            });
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(SemanticTokensRangeParams request)
    {
        return request.TextDocument;
    }
}
