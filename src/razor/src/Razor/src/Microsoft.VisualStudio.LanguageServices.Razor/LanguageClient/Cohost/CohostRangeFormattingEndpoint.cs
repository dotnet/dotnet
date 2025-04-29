// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Settings;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentRangeFormattingName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostRangeFormattingEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostRangeFormattingEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker,
    IClientSettingsManager clientSettingsManager,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<DocumentRangeFormattingParams, TextEdit[]?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostRangeFormattingEndpoint>();

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Formatting?.DynamicRegistration is true)
        {
            return [new Registration()
            {
                Method = Methods.TextDocumentRangeFormattingName,
                RegisterOptions = new DocumentRangeFormattingRegistrationOptions()
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(DocumentRangeFormattingParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<TextEdit[]?> HandleRequestAsync(DocumentRangeFormattingParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<TextEdit[]?> HandleRequestAsync(DocumentRangeFormattingParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        if (request.Options.OtherOptions is not null && request.Options.OtherOptions.TryGetValue("fromPaste", out var fromPasteObj) && fromPasteObj is bool fromPaste)
        {
            if (fromPaste && !_clientSettingsManager.GetClientSettings().AdvancedSettings.FormatOnPaste)
            {
                return null;
            }
        }

        _logger.LogDebug($"Getting Html formatting changes for {razorDocument.FilePath}");
        var htmlResult = await TryGetHtmlFormattingEditsAsync(request, razorDocument, cancellationToken).ConfigureAwait(false);

        if (htmlResult is not { } htmlEdits)
        {
            // We prefer to return null, so the client will try again
            _logger.LogDebug($"Didn't get any edits back from Html");
            return null;
        }

        var sourceText = await razorDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);
        var htmlChanges = htmlEdits.SelectAsArray(sourceText.GetTextChange);

        var options = RazorFormattingOptions.From(request.Options, _clientSettingsManager.GetClientSettings().AdvancedSettings.CodeBlockBraceOnNextLine);

        _logger.LogDebug($"Calling OOP with the {htmlChanges.Length} html edits, so it can fill in the rest");
        var remoteResult = await _remoteServiceInvoker.TryInvokeAsync<IRemoteFormattingService, ImmutableArray<TextChange>>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetRangeFormattingEditsAsync(solutionInfo, razorDocument.Id, htmlChanges, request.Range.ToLinePositionSpan(), options, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (remoteResult.Length > 0)
        {
            _logger.LogDebug($"Got a total of {remoteResult.Length} ranges back from OOP");

            return remoteResult.Select(sourceText.GetTextEdit).ToArray();
        }

        return null;
    }

    private async Task<TextEdit[]?> TryGetHtmlFormattingEditsAsync(DocumentRangeFormattingParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        // We don't actually request range formatting results from Html, because our formatting engine can't deal with
        // relative formatting results. Instead we request full document formatting, and filter the edits inside the
        // formatting service to only the ones we care about.
        var formattingRequest = new DocumentFormattingParams
        {
            TextDocument = request.TextDocument.WithUri(htmlDocument.Uri),
            Options = request.Options
        };

        _logger.LogDebug($"Requesting document formatting edits for {htmlDocument.Uri}");

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentFormattingParams, TextEdit[]?>(
            htmlDocument.Buffer,
            Methods.TextDocumentFormattingName,
            RazorLSPConstants.HtmlLanguageServerName,
            formattingRequest,
            cancellationToken).ConfigureAwait(false);

        if (result?.Response is null)
        {
            _logger.LogDebug($"Didn't get any ranges back from Html. Returning null so we can abandon the whole thing");
            return null;
        }

        return result.Response;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostRangeFormattingEndpoint instance)
    {
        public Task<TextEdit[]?> HandleRequestAsync(DocumentRangeFormattingParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}

