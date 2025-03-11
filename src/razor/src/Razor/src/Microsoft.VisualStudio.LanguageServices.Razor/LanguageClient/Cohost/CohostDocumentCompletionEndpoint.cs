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
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Razor.Snippets;
using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalCompletionList?>;
using RoslynCompletionContext = Roslyn.LanguageServer.Protocol.CompletionContext;
using RoslynCompletionParams = Roslyn.LanguageServer.Protocol.CompletionParams;
using RoslynLspExtensions = Roslyn.LanguageServer.Protocol.RoslynLspExtensions;
using RoslynPosition = Roslyn.LanguageServer.Protocol.Position;

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
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    SnippetCompletionItemProvider snippetCompletionItemProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    LSPRequestInvoker requestInvoker,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<RoslynCompletionParams, VSInternalCompletionList?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly SnippetCompletionItemProvider _snippetCompletionItemProvider = snippetCompletionItemProvider;
    private readonly CompletionTriggerAndCommitCharacters _triggerAndCommitCharacters = new(languageServerFeatureOptions);
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
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

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(RoslynCompletionParams request)
        => request.TextDocument is null ? null : RoslynLspExtensions.ToRazorTextDocumentIdentifier(request.TextDocument);

    protected override Task<VSInternalCompletionList?> HandleRequestAsync(RoslynCompletionParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<VSInternalCompletionList?> HandleRequestAsync(RoslynCompletionParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        if (request.Context is null || JsonHelpers.ToVsLSP<VSInternalCompletionContext, RoslynCompletionContext>(request.Context) is not VSInternalCompletionContext completionContext)
        {
            _logger.LogError("Completion request context is null");
            return null;
        }

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
                            JsonHelpers.ToVsLSP<Position, RoslynPosition>(request.Position).AssumeNotNull(),
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

        VSInternalCompletionList? htmlCompletionList = null;
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

        VSInternalCompletionList? combinedCompletionList = null;
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

        return combinedCompletionList;
    }

    private async Task<VSInternalCompletionList?> GetHtmlCompletionListAsync(
        RoslynCompletionParams request,
        TextDocument razorDocument,
        RazorCompletionOptions razorCompletionOptions,
        CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return null;
        }

        request.TextDocument = RoslynLspExtensions.WithUri(request.TextDocument, htmlDocument.Uri);

        _logger.LogDebug($"Resolving auto-insertion edit for {htmlDocument.Uri}");

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<RoslynCompletionParams, VSInternalCompletionList?>(
            htmlDocument.Buffer,
            Methods.TextDocumentCompletionName,
            RazorLSPConstants.HtmlLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        var rewrittenResponse = DelegatedCompletionHelper.RewriteHtmlResponse(result?.Response, razorCompletionOptions);

        return rewrittenResponse;
    }

    private VSInternalCompletionList? AddSnippets(
        VSInternalCompletionList? completionList,
        RazorLanguageKind languageKind,
        VSInternalCompletionInvokeKind invokeKind,
        string? triggerCharacter)
    {
        using var builder = new PooledArrayBuilder<CompletionItem>();
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
            completionList = new VSInternalCompletionList { IsIncomplete = true, Items = builder.ToArray() };
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
        public Task<VSInternalCompletionList?> HandleRequestAsync(
            RoslynCompletionParams request,
            TextDocument razorDocument,
            CancellationToken cancellationToken)
                => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
