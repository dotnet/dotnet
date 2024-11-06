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
using Microsoft.CodeAnalysis.Razor.AutoInsert;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol.AutoInsert;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;
using Microsoft.VisualStudio.Razor.Settings;
using RazorLSPConstants = Microsoft.VisualStudio.Razor.LanguageClient.RazorLSPConstants;
using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.CodeAnalysis.Razor.Protocol.AutoInsert.RemoteAutoInsertTextEdit?>;

namespace Microsoft.VisualStudio.LanguageServices.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.OnAutoInsertName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostOnAutoInsertEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostOnAutoInsertEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IClientSettingsManager clientSettingsManager,
#pragma warning disable RS0030 // Do not use banned APIs
    [ImportMany] IEnumerable<IOnAutoInsertTriggerCharacterProvider> onAutoInsertTriggerCharacterProviders,
#pragma warning restore RS0030 // Do not use banned APIs
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostOnAutoInsertEndpoint>();

    private readonly ImmutableArray<string> _triggerCharacters = CalculateTriggerChars(onAutoInsertTriggerCharacterProviders);

    private static ImmutableArray<string> CalculateTriggerChars(IEnumerable<IOnAutoInsertTriggerCharacterProvider> onAutoInsertTriggerCharacterProviders)
    {
        var providerTriggerCharacters = onAutoInsertTriggerCharacterProviders.Select((provider) => provider.TriggerCharacter).Distinct();

        ImmutableArray<string> triggerCharacters = [
            .. providerTriggerCharacters,
            .. AutoInsertService.HtmlAllowedAutoInsertTriggerCharacters,
            .. AutoInsertService.CSharpAllowedAutoInsertTriggerCharacters ];

        return triggerCharacters;
    }

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.SupportsVisualStudioExtensions)
        {
            return [new Registration
            {
                Method = VSInternalMethods.OnAutoInsertName,
                RegisterOptions = new VSInternalDocumentOnAutoInsertRegistrationOptions()
                    .EnableOnAutoInsert(_triggerCharacters)
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalDocumentOnAutoInsertParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<VSInternalDocumentOnAutoInsertResponseItem?> HandleRequestAsync(VSInternalDocumentOnAutoInsertParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<VSInternalDocumentOnAutoInsertResponseItem?> HandleRequestAsync(VSInternalDocumentOnAutoInsertParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Resolving auto-insertion for {razorDocument.FilePath}");

        var clientSettings = _clientSettingsManager.GetClientSettings();
        var razorFormattingOptions = RazorFormattingOptions.From(request.Options, codeBlockBraceOnNextLine: false);
        var autoInsertOptions = RemoteAutoInsertOptions.From(clientSettings, razorFormattingOptions);

        _logger.LogDebug($"Calling OOP to resolve insertion at {request.Position} invoked by typing '{request.Character}'");
        var data = await _remoteServiceInvoker.TryInvokeAsync<IRemoteAutoInsertService, Response>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken)
                => service.GetAutoInsertTextEditAsync(
                        solutionInfo,
                        razorDocument.Id,
                        request.Position.ToLinePosition(),
                        request.Character,
                        autoInsertOptions,
                        cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (data.Result is { } remoteInsertTextEdit)
        {
            _logger.LogDebug($"Got insert text edit from OOP {remoteInsertTextEdit}");
            return RemoteAutoInsertTextEdit.ToLspInsertTextEdit(remoteInsertTextEdit);
        }

        if (data.StopHandling)
        {
            return null;
        }

        // Got no data but no signal to stop handling

        return await TryResolveHtmlInsertionAsync(
            razorDocument,
            request,
            clientSettings.AdvancedSettings.AutoInsertAttributeQuotes,
            cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<VSInternalDocumentOnAutoInsertResponseItem?> TryResolveHtmlInsertionAsync(
        TextDocument razorDocument,
        VSInternalDocumentOnAutoInsertParams request,
        bool autoInsertAttributeQuotes,
        CancellationToken cancellationToken)
    {
        if (!autoInsertAttributeQuotes && request.Character == "=")
        {
            // Use Razor setting for auto insert attribute quotes. HTML Server doesn't have a way to pass that
            // information along so instead we just don't delegate the request.
            _logger.LogTrace($"Not delegating to HTML completion because AutoInsertAttributeQuotes is disabled");
            return null;
        }

        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument = request.TextDocument.WithUri(htmlDocument.Uri);

        _logger.LogDebug($"Resolving auto-insertion edit for {htmlDocument.Uri}");

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem?>(
            htmlDocument.Buffer,
            VSInternalMethods.OnAutoInsertName,
            RazorLSPConstants.HtmlLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        if (result?.Response is null)
        {
            _logger.LogDebug($"Didn't get insert edit back from Html.");
            return null;
        }

        return result.Response;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostOnAutoInsertEndpoint instance)
    {
        public Task<VSInternalDocumentOnAutoInsertResponseItem?> HandleRequestAsync(
            VSInternalDocumentOnAutoInsertParams request,
            TextDocument razorDocument,
            CancellationToken cancellationToken)
                => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
