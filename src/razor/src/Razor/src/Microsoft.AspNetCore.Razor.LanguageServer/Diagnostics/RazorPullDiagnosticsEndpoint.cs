// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

internal class RazorPullDiagnosticsEndpoint
    : IRazorPullDiagnosticsEndpoint
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ClientNotifierServiceBase _languageServer;
    protected readonly RazorDocumentMappingService DocumentMappingService;

    public RazorPullDiagnosticsEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        DocumentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
    }

    public bool MutatesSolutionState => false;

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        return new RegistrationExtensionResult("_vs_supportsDiagnosticRequests", options: true);
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(VSInternalDocumentDiagnosticsParams request)
    {
        if (request.TextDocument is null)
        {
            throw new ArgumentNullException(nameof(request.TextDocument));
        }

        return request.TextDocument;
    }

    public async Task<IEnumerable<VSInternalDiagnosticReport>?> HandleRequestAsync(VSInternalDocumentDiagnosticsParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        var documentContext = context.GetRequiredDocumentContext();

        if (!_languageServerFeatureOptions.SingleServerSupport)
        {
            return default;
        }

        var delegatedParams = new DelegatedDiagnosticParams(documentContext.Identifier);

        var delegatedResponse = await _languageServer.SendRequestAsync<DelegatedDiagnosticParams, IEnumerable<VSInternalDiagnosticReport>>(
            RazorLanguageServerCustomMessageTargets.RazorPullDiagnosticEndpointName,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken);

        foreach(var report in delegatedResponse)
        {
            if (report.Diagnostics is not null)
            {
                foreach(var diagnostic in report.Diagnostics)
                {
                    if(DocumentMappingService.TryMapFromProjectedDocumentRange(codeDocument, diagnostic.Range, out var razorRange))
                    {
                        diagnostic.Range = razorRange;
                    }
                }
            }
        }

        return delegatedResponse;
    }
}
