// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Completion.Delegation;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
#if !VSCODE
// Visual Studio requires us to register for every method name, VS Code correctly realises that if you
// register for code actions, and say you have resolve support, then registering for resolve is unnecessary.
// In fact it's an error.
[Export(typeof(IDynamicRegistrationProvider))]
#endif
[Shared]
[CohostEndpoint(Methods.TextDocumentCompletionResolveName)]
[ExportRazorStatelessLspService(typeof(CohostDocumentCompletionResolveEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostDocumentCompletionResolveEndpoint(
    CompletionListCache completionListCache,
    IRemoteServiceInvoker remoteServiceInvoker,
    IClientSettingsManager clientSettingsManager,
    IHtmlRequestInvoker requestInvoker,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<VSInternalCompletionItem, VSInternalCompletionItem?>, IDynamicRegistrationProvider
{
    private readonly CompletionListCache _completionListCache = completionListCache;
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly IHtmlRequestInvoker _requestInvoker = requestInvoker;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostDocumentCompletionResolveEndpoint>();

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Completion?.DynamicRegistration is true)
        {
            return [new Registration()
            {
                Method = Methods.TextDocumentCompletionResolveName,
                RegisterOptions = new CompletionRegistrationOptions()
                {
                    ResolveProvider = true
                }
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalCompletionItem request)
    {
        if (RazorCompletionResolveData.Unwrap(request) is { } data)
        {
            return data.TextDocument.ToRazorTextDocumentIdentifier();
        }

        return null;
    }

    protected override Task<VSInternalCompletionItem?> HandleRequestAsync(VSInternalCompletionItem request, RazorCohostRequestContext context, CancellationToken cancellationToken)
        => HandleRequestAsync(request, context.TextDocument.AssumeNotNull(), cancellationToken);

    private async Task<VSInternalCompletionItem?> HandleRequestAsync(
        VSInternalCompletionItem completionItem,
        TextDocument razorDocument,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return completionItem;
        }

        var data = RazorCompletionResolveData.Unwrap(completionItem);
        completionItem.Data = data.OriginalData;

        if (_completionListCache.TryGetOriginalRequestData(completionItem, out var completionList, out var context) &&
            context is DelegatedCompletionResolutionContext delegatedContext)
        {
            Debug.Assert(delegatedContext.ProjectedKind == RazorLanguageKind.Html);

#if VSCODE
            Debug.Assert(_requestInvoker is not null);
            Debug.Assert(_logger is not null);
            Debug.Assert(nameof(DelegatedCompletionHelper).Length > 0);

            // We don't support Html completion resolve in VS Code
            return completionItem;
#else
            completionItem.Data = DelegatedCompletionHelper.GetOriginalCompletionItemData(completionItem, completionList, delegatedContext.OriginalCompletionListData);
            return await ResolveHtmlCompletionItemAsync(completionItem, razorDocument, cancellationToken).ConfigureAwait(false);
#endif
        }

        var clientSettings = _clientSettingsManager.GetClientSettings();
        var formattingOptions = new RazorFormattingOptions()
        {
            InsertSpaces = !clientSettings.ClientSpaceSettings.IndentWithTabs,
            TabSize = clientSettings.ClientSpaceSettings.IndentSize,
            CodeBlockBraceOnNextLine = clientSettings.AdvancedSettings.CodeBlockBraceOnNextLine
        };

        // Couldn't find an associated completion list, so its either Razor or C#. Either way, over to OOP
        var result = await _remoteServiceInvoker.TryInvokeAsync<IRemoteCompletionService, VSInternalCompletionItem>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken)
                => service.ResolveCompletionItemAsync(
                    solutionInfo,
                    razorDocument.Id,
                    completionItem,
                    formattingOptions,
                    cancellationToken),
            cancellationToken).ConfigureAwait(false);

        return result;
    }

#if !VSCODE
    private async Task<VSInternalCompletionItem> ResolveHtmlCompletionItemAsync(
        VSInternalCompletionItem request,
        TextDocument razorDocument,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Resolving Html completion item {request.Label} for {razorDocument.FilePath}");

        var result = await _requestInvoker.MakeHtmlLspRequestAsync<VSInternalCompletionItem, VSInternalCompletionItem>(
            razorDocument,
            Methods.TextDocumentCompletionResolveName,
            request,
            cancellationToken).ConfigureAwait(false);

        return result ?? request;
    }
#endif

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostDocumentCompletionResolveEndpoint instance)
    {
        public RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalCompletionItem request)
            => instance.GetRazorTextDocumentIdentifier(request);

        public Task<VSInternalCompletionItem?> HandleRequestAsync(
            VSInternalCompletionItem request,
            TextDocument razorDocument,
            CancellationToken cancellationToken)
                => instance.HandleRequestAsync(request, razorDocument, cancellationToken);
    }
}
