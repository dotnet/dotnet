// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
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
[CohostEndpoint(Methods.TextDocumentOnTypeFormattingName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostOnTypeFormattingEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostOnTypeFormattingEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker,
    IClientSettingsManager clientSettingsManager,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<DocumentOnTypeFormattingParams, TextEdit[]?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostOnTypeFormattingEndpoint>();

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public Registration? GetRegistration(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Formatting?.DynamicRegistration is true)
        {
            return new Registration()
            {
                Method = Methods.TextDocumentOnTypeFormattingName,
                RegisterOptions = new DocumentOnTypeFormattingRegistrationOptions()
                {
                    DocumentSelector = filter,
                }.EnableOnTypeFormattingTriggerCharacters()
            };
        }

        return null;
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(DocumentOnTypeFormattingParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<TextEdit[]?> HandleRequestAsync(DocumentOnTypeFormattingParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<TextEdit[]?> HandleRequestAsync(DocumentOnTypeFormattingParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var clientSettings = _clientSettingsManager.GetClientSettings();
        if (!clientSettings.AdvancedSettings.FormatOnType)
        {
            _logger.LogInformation($"Formatting on type disabled.");
            return null;
        }

        if (!RazorFormattingService.AllTriggerCharacterSet.Contains(request.Character))
        {
            _logger.LogWarning($"Unexpected trigger character '{request.Character}'.");
            return null;
        }

        // We have to go to OOP to find out if we want Html formatting for this request. This is a little unfortunate
        // but just asking Html for formatting, just in case, would be bad for a couple of reasons. Firstly, the Html
        // trigger characters are a superset of the C# triggers, so we can't use that as a sign. Secondly, whilst we
        // might be making one Html request, it could be then calling CSS or TypeScript servers, so our single request
        // to OOP could potentially save a few requests downstream. Lastly, our request to OOP is MessagePack which is
        // generally faster than Json anyway.
        var triggerKind = await _remoteServiceInvoker.TryInvokeAsync<IRemoteFormattingService, IRemoteFormattingService.TriggerKind>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetOnTypeFormattingTriggerKindAsync(solutionInfo, razorDocument.Id, request.Position.ToLinePosition(), request.Character, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (triggerKind == IRemoteFormattingService.TriggerKind.Invalid)
        {
            return null;
        }

        var sourceText = await razorDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);

        ImmutableArray<TextChange> htmlChanges = [];
        if (triggerKind == IRemoteFormattingService.TriggerKind.ValidHtml)
        {
            _logger.LogDebug($"Getting Html formatting changes for {razorDocument.FilePath}");
            var htmlResult = await GetHtmlFormattingEditsAsync(request, razorDocument, cancellationToken).ConfigureAwait(false);

            if (htmlResult is not { } htmlEdits)
            {
                // We prefer to return null, so the client will try again
                _logger.LogDebug($"Didn't get any edits back from Html");
                return null;
            }

            htmlChanges = htmlEdits.SelectAsArray(sourceText.GetTextChange);
        }

        var options = RazorFormattingOptions.From(request.Options, clientSettings.AdvancedSettings.CodeBlockBraceOnNextLine);

        _logger.LogDebug($"Calling OOP with the {htmlChanges.Length} html edits, so it can fill in the rest");
        var remoteResult = await _remoteServiceInvoker.TryInvokeAsync<IRemoteFormattingService, ImmutableArray<TextChange>>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetOnTypeFormattingEditsAsync(solutionInfo, razorDocument.Id, htmlChanges, request.Position.ToLinePosition(), request.Character, options, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (remoteResult.Length > 0)
        {
            _logger.LogDebug($"Got a total of {remoteResult.Length} ranges back from OOP");

            return remoteResult.Select(sourceText.GetTextEdit).ToArray();
        }

        return null;
    }

    private async Task<TextEdit[]?> GetHtmlFormattingEditsAsync(DocumentOnTypeFormattingParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument = request.TextDocument.WithUri(htmlDocument.Uri);

        _logger.LogDebug($"Requesting document formatting edits for {htmlDocument.Uri}");

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentOnTypeFormattingParams, TextEdit[]?>(
            htmlDocument.Buffer,
            Methods.TextDocumentOnTypeFormattingName,
            RazorLSPConstants.HtmlLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        if (result?.Response is null)
        {
            _logger.LogDebug($"Didn't get any ranges back from Html. Returning null so we can abandon the whole thing");
            return null;
        }

        return result.Response;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostOnTypeFormattingEndpoint instance)
    {
        public Task<TextEdit[]?> HandleRequestAsync(DocumentOnTypeFormattingParams request, TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}

