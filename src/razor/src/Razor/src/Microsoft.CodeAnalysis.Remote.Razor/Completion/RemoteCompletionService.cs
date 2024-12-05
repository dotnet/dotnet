// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Completion.Delegation;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Completion;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalCompletionList?>;
using RoslynCompletionContext = Roslyn.LanguageServer.Protocol.CompletionContext;
using RoslynCompletionList = Roslyn.LanguageServer.Protocol.CompletionList;
using RoslynCompletionSetting = Roslyn.LanguageServer.Protocol.CompletionSetting;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteCompletionService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteCompletionService
{
    internal sealed class Factory : FactoryBase<IRemoteCompletionService>
    {
        protected override IRemoteCompletionService CreateService(in ServiceArgs args)
            => new RemoteCompletionService(in args);
    }

    private readonly RazorCompletionListProvider _razorCompletionListProvider = args.ExportProvider.GetExportedValue<RazorCompletionListProvider>();
    private readonly IClientCapabilitiesService _clientCapabilitiesService = args.ExportProvider.GetExportedValue<IClientCapabilitiesService>();

    public ValueTask<CompletionPositionInfo?> GetPositionInfoAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        VSInternalCompletionContext completionContext,
        Position position,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetPositionInfoAsync(context, completionContext, position, cancellationToken),
            cancellationToken);

    private async ValueTask<CompletionPositionInfo?> GetPositionInfoAsync(
        RemoteDocumentContext remoteDocumentContext,
        VSInternalCompletionContext completionContext,
        Position position,
        CancellationToken cancellationToken)
    {
        var sourceText = await remoteDocumentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(position, out var index))
        {
            return null;
        }

        var codeDocument = await remoteDocumentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        var positionInfo = GetPositionInfo(codeDocument, index);

        if (positionInfo.LanguageKind != RazorLanguageKind.Razor
           && await DelegatedCompletionHelper.TryGetProvisionalCompletionInfoAsync(
                remoteDocumentContext,
                completionContext,
                positionInfo,
                DocumentMappingService,
                cancellationToken)
                .ConfigureAwait(false) is { } provisionalCompletionInfo)
        {
            return new CompletionPositionInfo(
                provisionalCompletionInfo.ProvisionalTextEdit,
                provisionalCompletionInfo.DocumentPositionInfo,
                ShouldIncludeDelegationSnippets: false);
        }

        var shouldIncludeSnippets = positionInfo.LanguageKind == RazorLanguageKind.Html
            && DelegatedCompletionHelper.ShouldIncludeSnippets(codeDocument, index);

        return new CompletionPositionInfo(ProvisionalTextEdit: null, positionInfo, shouldIncludeSnippets);
    }

    public ValueTask<Response> GetCompletionAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        CompletionPositionInfo positionInfo,
        VSInternalCompletionContext completionContext,
        RazorCompletionOptions razorCompletionOptions,
        HashSet<string> existingHtmlCompletions,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            documentId,
            context => GetCompletionAsync(
                context,
                positionInfo,
                completionContext,
                razorCompletionOptions,
                existingHtmlCompletions,
                cancellationToken),
            cancellationToken);

    private async ValueTask<Response> GetCompletionAsync(
        RemoteDocumentContext remoteDocumentContext,
        CompletionPositionInfo positionInfo,
        VSInternalCompletionContext completionContext,
        RazorCompletionOptions razorCompletionOptions,
        HashSet<string> existingDelegatedCompletions,
        CancellationToken cancellationToken)
    {
        VSInternalCompletionList? csharpCompletionList = null;
        var documentPositionInfo = positionInfo.DocumentPositionInfo;
        if (documentPositionInfo.LanguageKind == RazorLanguageKind.CSharp &&
            CompletionTriggerAndCommitCharacters.IsValidTrigger(CompletionTriggerAndCommitCharacters.CSharpTriggerCharacters, completionContext))
        {
            var mappedPosition = documentPositionInfo.Position;
            csharpCompletionList = await GetCSharpCompletionAsync(
                remoteDocumentContext,
                documentPositionInfo.HostDocumentIndex,
                mappedPosition,
                positionInfo.ProvisionalTextEdit,
                completionContext,
                razorCompletionOptions,
                cancellationToken)
                .ConfigureAwait(false);

            if (csharpCompletionList is not null)
            {
                Debug.Assert(existingDelegatedCompletions.Count == 0, "Delegated completion should be either C# or HTML, not both");
                existingDelegatedCompletions.UnionWith(csharpCompletionList.Items.Select((item) => item.Label));
            }
        }

        var razorCompletionList = CompletionTriggerAndCommitCharacters.IsValidTrigger(CompletionTriggerAndCommitCharacters.RazorTriggerCharacters, completionContext)
            ? await _razorCompletionListProvider.GetCompletionListAsync(
                documentPositionInfo.HostDocumentIndex,
                completionContext,
                remoteDocumentContext,
                _clientCapabilitiesService.ClientCapabilities,
                existingCompletions: existingDelegatedCompletions,
                razorCompletionOptions,
                cancellationToken)
                .ConfigureAwait(false)
            : null;

        // Merge won't return anything only if both completion lists passed in are null,
        // in which case client should just proceed with HTML completion.
        if (CompletionListMerger.Merge(razorCompletionList, csharpCompletionList) is not { } mergedCompletionList)
        {
            return Response.CallHtml;
        }

        return Response.Results(mergedCompletionList);
    }

    private async ValueTask<VSInternalCompletionList?> GetCSharpCompletionAsync(
        RemoteDocumentContext remoteDocumentContext,
        int documentIndex,
        Position mappedPosition,
        TextEdit? provisionalTextEdit,
        CompletionContext completionContext,
        RazorCompletionOptions razorCompletionOptions,
        CancellationToken cancellationToken)
    {
        var generatedDocument = await remoteDocumentContext.Snapshot
            .GetGeneratedDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (provisionalTextEdit is not null)
        {
            var generatedText = await generatedDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);
            var change = generatedText.GetTextChange(provisionalTextEdit);
            generatedText = generatedText.WithChanges([change]);
            generatedDocument = generatedDocument.WithText(generatedText);
        }

        // This is, to say the least, not ideal. In future we're going to normalize on to Roslyn LSP types, and this can go.
        if (JsonHelpers.ToRoslynLSP<RoslynCompletionContext, CompletionContext>(completionContext) is not { } roslynCompletionContext)
        {
            Debug.Fail("Unable to convert VS to Roslyn LSP completion context");
            return null;
        }

        var clientCapabilities = _clientCapabilitiesService.ClientCapabilities;
        if (JsonHelpers.ToRoslynLSP<RoslynCompletionSetting, CompletionSetting>(clientCapabilities.TextDocument?.Completion) is not { } roslynCompletionSetting)
        {
            Debug.Fail("Unable to convert VS to Roslyn LSP completion setting");
            return null;
        }

        var mappedLinePosition = mappedPosition.ToLinePosition();
        var roslynCompletionList = await ExternalAccess.Razor.Cohost.Handlers.Completion.GetCompletionListAsync(
            generatedDocument,
            mappedLinePosition,
            roslynCompletionContext,
            clientCapabilities.SupportsVisualStudioExtensions,
            roslynCompletionSetting,
            cancellationToken)
            .ConfigureAwait(false);

        if (roslynCompletionList is null)
        {
            // If we don't get a response from the delegated server, we have to make sure to return an incomplete completion
            // list. When a user is typing quickly, the delegated request from the first keystroke will fail to synchronize,
            // so if we return a "complete" list then the query won't re-query us for completion once the typing stops/slows
            // so we'd only ever return Razor completion items.
            return new VSInternalCompletionList()
            {
                Items = [],
                IsIncomplete = true
            };
        }

        var vsPlatformCompletionList = JsonHelpers.ToVsLSP<VSInternalCompletionList, RoslynCompletionList>(roslynCompletionList);

        var rewrittenResponse = await DelegatedCompletionHelper.RewriteCSharpResponseAsync(
            vsPlatformCompletionList,
            documentIndex,
            remoteDocumentContext,
            mappedPosition,
            razorCompletionOptions,
            cancellationToken)
            .ConfigureAwait(false);

        return rewrittenResponse;
    }
}
