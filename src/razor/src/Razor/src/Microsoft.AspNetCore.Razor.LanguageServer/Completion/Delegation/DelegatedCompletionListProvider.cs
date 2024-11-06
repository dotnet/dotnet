// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Completion.Delegation;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Completion;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class DelegatedCompletionListProvider
{
    private readonly IDocumentMappingService _documentMappingService;
    private readonly IClientConnection _clientConnection;
    private readonly CompletionListCache _completionListCache;

    public DelegatedCompletionListProvider(
        IDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        CompletionListCache completionListCache)
    {
        _documentMappingService = documentMappingService;
        _clientConnection = clientConnection;
        _completionListCache = completionListCache;
    }

    // virtual for tests
    public virtual FrozenSet<string> TriggerCharacters => CompletionTriggerAndCommitCharacters.AllDelegationTriggerCharacters;

    // virtual for tests
    public virtual async Task<VSInternalCompletionList?> GetCompletionListAsync(
        int absoluteIndex,
        VSInternalCompletionContext completionContext,
        DocumentContext documentContext,
        VSInternalClientCapabilities clientCapabilities,
        RazorCompletionOptions razorCompletionOptions,
        Guid correlationId,
        CancellationToken cancellationToken)
    {
        var positionInfo = await _documentMappingService
            .GetPositionInfoAsync(documentContext, absoluteIndex, cancellationToken)
            .ConfigureAwait(false);

        if (positionInfo.LanguageKind == RazorLanguageKind.Razor)
        {
            // Nothing to delegate to.
            return null;
        }

        var provisionalCompletion = await DelegatedCompletionHelper.TryGetProvisionalCompletionInfoAsync(
            documentContext,
            completionContext,
            positionInfo,
            _documentMappingService,
            cancellationToken).ConfigureAwait(false);
        TextEdit? provisionalTextEdit = null;
        if (provisionalCompletion is { } provisionalCompletionValue)
        {
            provisionalTextEdit = provisionalCompletionValue.ProvisionalTextEdit;
            positionInfo = provisionalCompletionValue.DocumentPositionInfo;
        }

        completionContext = DelegatedCompletionHelper.RewriteContext(completionContext, positionInfo.LanguageKind);

        var razorCodeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        // It's a bit confusing, but we have two different "add snippets" options - one is a part of
        // RazorCompletionOptions and becomes a part of RazorCompletionContext and is used by
        // RazorCompletionFactsService, and the second one below that's used for delegated completion
        // Their values are not related in any way.
        var shouldIncludeDelegationSnippets = DelegatedCompletionHelper.ShouldIncludeSnippets(razorCodeDocument, absoluteIndex);

        var delegatedParams = new DelegatedCompletionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            positionInfo.Position,
            positionInfo.LanguageKind,
            completionContext,
            provisionalTextEdit,
            shouldIncludeDelegationSnippets,
            correlationId);

        var delegatedResponse = await _clientConnection.SendRequestAsync<DelegatedCompletionParams, VSInternalCompletionList?>(
            LanguageServerConstants.RazorCompletionEndpointName,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        var rewrittenResponse = delegatedParams.ProjectedKind == RazorLanguageKind.CSharp
             ? await DelegatedCompletionHelper.RewriteCSharpResponseAsync(
                delegatedResponse,
                absoluteIndex,
                documentContext,
                delegatedParams.ProjectedPosition,
                razorCompletionOptions,
                cancellationToken)
                .ConfigureAwait(false)
            : DelegatedCompletionHelper.RewriteHtmlResponse(delegatedResponse, razorCompletionOptions);

        var completionCapability = clientCapabilities?.TextDocument?.Completion as VSInternalCompletionSetting;
        var resolutionContext = new DelegatedCompletionResolutionContext(delegatedParams, rewrittenResponse.Data);
        var resultId = _completionListCache.Add(rewrittenResponse, resolutionContext);
        rewrittenResponse.SetResultId(resultId, completionCapability);

        return rewrittenResponse;
    }
}
