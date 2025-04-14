// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Completion.Delegation;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Completion;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Razor.Snippets;
using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Roslyn.LanguageServer.Protocol.RazorVSInternalCompletionList?>;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentCompletionName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostDocumentCompletionEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostDocumentCompletionEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IClientSettingsManager clientSettingsManager,
    IClientCapabilitiesService clientCapabilitiesService,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    SnippetCompletionItemProvider snippetCompletionItemProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    LSPRequestInvoker requestInvoker,
    CompletionListCache completionListCache,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<CompletionParams, RazorVSInternalCompletionList?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly IClientCapabilitiesService _clientCapabilitiesService = clientCapabilitiesService;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly SnippetCompletionItemProvider _snippetCompletionItemProvider = snippetCompletionItemProvider;
    private readonly CompletionTriggerAndCommitCharacters _triggerAndCommitCharacters = new(languageServerFeatureOptions);
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly CompletionListCache _completionListCache = completionListCache;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostDocumentCompletionEndpoint>();

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Completion?.DynamicRegistration is true)
        {
            return [new Registration()
            {
                Method = Methods.TextDocumentCompletionName,
                RegisterOptions = new CompletionRegistrationOptions()
                {
                    ResolveProvider = false, // TODO - change to true when Resolve is implemented
                    TriggerCharacters = [.. _triggerAndCommitCharacters.AllTriggerCharacters],
                    AllCommitCharacters = [.. _triggerAndCommitCharacters.AllCommitCharacters]
                }
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(CompletionParams request)
        => request.TextDocument?.ToRazorTextDocumentIdentifier();

    protected override Task<RazorVSInternalCompletionList?> HandleRequestAsync(CompletionParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<RazorVSInternalCompletionList?> HandleRequestAsync(CompletionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        if (request.Context is null ||
            JsonHelpers.Convert<CompletionContext, VSInternalCompletionContext>(request.Context) is not { } completionContext)
        {
            _logger.LogError("Completion request context is null");
            return null;
        }

        // Save as it may be modified if we forward request to HTML language server
        var originalTextDocumentIdentifier = request.TextDocument;

        // Return immediately if this is auto-shown completion but auto-shown completion is disallowed in settings
        var clientSettings = _clientSettingsManager.GetClientSettings();
        var autoShownCompletion = completionContext.TriggerKind != CompletionTriggerKind.Invoked;
        if (autoShownCompletion && !clientSettings.ClientCompletionSettings.AutoShowCompletion)
        {
            return null;
        }

        _logger.LogDebug($"Invoking completion for {razorDocument.FilePath}");

        if (await _remoteServiceInvoker.TryInvokeAsync<IRemoteCompletionService, CompletionPositionInfo?>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken)
                    => service.GetPositionInfoAsync(
                            solutionInfo,
                            razorDocument.Id,
                            completionContext,
                            request.Position,
                            cancellationToken),
                cancellationToken).ConfigureAwait(false) is not { } completionPositionInfo)
        {
            // If we can't figure out position info for request position we can't return completions
            return null;
        }

        var documentPositionInfo = completionPositionInfo.DocumentPositionInfo;
        if (documentPositionInfo.LanguageKind != RazorLanguageKind.Razor)
        {
            if (DelegatedCompletionHelper.RewriteContext(completionContext, documentPositionInfo.LanguageKind, _triggerAndCommitCharacters) is not { } rewrittenContext)
            {
                return null;
            }

            completionContext = rewrittenContext;
        }

        // First of all, see if we in HTML and get HTML completions before calling OOP to get Razor completions.
        // Razor completion provider needs a set of existing HTML item labels.

        RazorVSInternalCompletionList? htmlCompletionList = null;
        var razorCompletionOptions = new RazorCompletionOptions(
            SnippetsSupported: true, // always true in non-legacy Razor, always false in legacy Razor
            AutoInsertAttributeQuotes: clientSettings.AdvancedSettings.AutoInsertAttributeQuotes,
            CommitElementsWithSpace: clientSettings.AdvancedSettings.CommitElementsWithSpace);
        using var _ = HashSetPool<string>.GetPooledObject(out var existingHtmlCompletions);

        if (_triggerAndCommitCharacters.IsValidHtmlTrigger(completionContext))
        {
            // We can just blindly call HTML LSP because if we are in C#, generated HTML seen by HTML LSP may return
            // results we don't want to show. So we want to call HTML LSP only if we know we are in HTML content.
            if (documentPositionInfo.LanguageKind == RazorLanguageKind.Html)
            {
                htmlCompletionList = await GetHtmlCompletionListAsync(
                    request, razorDocument, razorCompletionOptions, cancellationToken).ConfigureAwait(false);

                if (htmlCompletionList is not null)
                {
                    existingHtmlCompletions.UnionWith(htmlCompletionList.Items.Select(i => i.Label));
                }
            }
        }

        _logger.LogDebug($"Calling OOP to get completion items at {request.Position} invoked by typing '{request.Context?.TriggerCharacter}'");

        var data = await _remoteServiceInvoker.TryInvokeAsync<IRemoteCompletionService, Response>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken)
                => service.GetCompletionAsync(
                        solutionInfo,
                        razorDocument.Id,
                        completionPositionInfo,
                        completionContext,
                        razorCompletionOptions,
                        existingHtmlCompletions,
                        cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (data.StopHandling)
        {
            return null;
        }

        RazorVSInternalCompletionList? combinedCompletionList = null;
        if (data.Result is { } oopCompletionList)
        {
            combinedCompletionList = htmlCompletionList is { Items: [_, ..] }
                // If we have HTML completions, that means OOP completion list is really just Razor completion list
                ? CompletionListMerger.Merge(oopCompletionList, htmlCompletionList)
                : oopCompletionList;
        }
        else
        {
            // Didn't get anything from OOP, so just return HTML completion list or null
            combinedCompletionList = htmlCompletionList;
        }

        if (completionPositionInfo.ShouldIncludeDelegationSnippets)
        {
            combinedCompletionList = AddSnippets(
                combinedCompletionList,
                documentPositionInfo.LanguageKind,
                completionContext.InvokeKind,
                completionContext.TriggerCharacter);
        }

        if (combinedCompletionList is null)
        {
            return null;
        }

        var completionCapability = _clientCapabilitiesService.ClientCapabilities.TextDocument?.Completion as VSInternalCompletionSetting;
        var supportsCompletionListData = completionCapability?.CompletionList?.Data ?? false;

        RazorCompletionResolveData.Wrap(combinedCompletionList, originalTextDocumentIdentifier, supportsCompletionListData: supportsCompletionListData);

        return combinedCompletionList;
    }

    private async Task<RazorVSInternalCompletionList?> GetHtmlCompletionListAsync(
        CompletionParams request,
        TextDocument razorDocument,
        RazorCompletionOptions razorCompletionOptions,
        CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        var originalTextDocumentIdentifier = request.TextDocument;
        request.TextDocument = request.TextDocument.WithUri(htmlDocument.Uri);

        _logger.LogDebug($"Getting completion list for {htmlDocument.Uri} at {request.Position}");

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<CompletionParams, RazorVSInternalCompletionList?>(
            htmlDocument.Buffer,
            Methods.TextDocumentCompletionName,
            RazorLSPConstants.HtmlLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        var rewrittenResponse = DelegatedCompletionHelper.RewriteHtmlResponse(result?.Response, razorCompletionOptions);

        var completionCapability = _clientCapabilitiesService.ClientCapabilities.TextDocument?.Completion as VSInternalCompletionSetting;

        var razorDocumentIdentifier = new TextDocumentIdentifierAndVersion(originalTextDocumentIdentifier, Version: 0);
        var resolutionContext = new DelegatedCompletionResolutionContext(razorDocumentIdentifier, RazorLanguageKind.Html, rewrittenResponse.Data);
        var resultId = _completionListCache.Add(rewrittenResponse, resolutionContext);
        rewrittenResponse.SetResultId(resultId, completionCapability);

        return rewrittenResponse;
    }

    private RazorVSInternalCompletionList? AddSnippets(
        RazorVSInternalCompletionList? completionList,
        RazorLanguageKind languageKind,
        VSInternalCompletionInvokeKind invokeKind,
        string? triggerCharacter)
    {
        using var builder = new PooledArrayBuilder<VSInternalCompletionItem>();
        _snippetCompletionItemProvider.AddSnippetCompletions(
            languageKind,
            invokeKind,
            triggerCharacter,
            ref builder.AsRef());

        // If there were no snippets, just return the original list
        if (builder.Count == 0)
        {
            return completionList;
        }

        // If there was a list with items, add them to snippets
        if (completionList?.Items is { } combinedItems)
        {
            builder.AddRange(combinedItems);
        }

        // Create or update final completion list
        if (completionList is null)
        {
            completionList = new RazorVSInternalCompletionList { IsIncomplete = true, Items = builder.ToArray() };
        }
        else
        {
            completionList.Items = builder.ToArray();
        }

        return completionList;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostDocumentCompletionEndpoint instance)
    {
        public Task<RazorVSInternalCompletionList?> HandleRequestAsync(
            CompletionParams request,
            TextDocument razorDocument,
            CancellationToken cancellationToken)
                => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
