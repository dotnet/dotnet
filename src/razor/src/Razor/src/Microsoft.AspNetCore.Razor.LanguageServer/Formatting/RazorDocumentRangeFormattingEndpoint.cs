// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class RazorDocumentRangeFormattingEndpoint : IVSDocumentRangeFormattingEndpoint
{
    private readonly RazorFormattingService _razorFormattingService;
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor;

    public RazorDocumentRangeFormattingEndpoint(
        RazorFormattingService razorFormattingService,
        IOptionsMonitor<RazorLSPOptions> optionsMonitor)
    {

        if (razorFormattingService is null)
        {
            throw new ArgumentNullException(nameof(razorFormattingService));
        }

        if (optionsMonitor is null)
        {
            throw new ArgumentNullException(nameof(optionsMonitor));
        }

        _razorFormattingService = razorFormattingService;
        _optionsMonitor = optionsMonitor;
    }

    public bool MutatesSolutionState => false;

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string ServerCapability = "documentRangeFormattingProvider";

        return new RegistrationExtensionResult(ServerCapability, new SumType<bool, DocumentRangeFormattingOptions>(new DocumentRangeFormattingOptions()));
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentRangeFormattingParams request)
    {
        return request.TextDocument;
    }

    public async Task<TextEdit[]?> HandleRequestAsync(DocumentRangeFormattingParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (!_optionsMonitor.CurrentValue.EnableFormatting)
        {
            return null;
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null || cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var edits = await _razorFormattingService.FormatAsync(documentContext, request.Range, request.Options, cancellationToken);

        return edits;
    }
}
