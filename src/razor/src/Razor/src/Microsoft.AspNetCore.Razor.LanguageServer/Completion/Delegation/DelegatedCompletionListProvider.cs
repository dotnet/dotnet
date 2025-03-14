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
    private readonly CompletionTriggerAndCommitCharacters _triggerAndCommitCharacters;

    public DelegatedCompletionListProvider(
        IDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        CompletionListCache completionListCache,
        CompletionTriggerAndCommitCharacters completionTriggerAndCommitCharacters)
    {
        _documentMappingService = documentMappingService;
        _clientConnection = clientConnection;
        _completionListCache = completionListCache;
        _triggerAndCommitCharacters = completionTriggerAndCommitCharacters;
    }

    // virtual for tests
    public virtual ValueTask<VSInternalCompletionList?> GetCompletionListAsync(
        RazorCodeDocument codeDocument,
        int absoluteIndex,
        VSInternalCompletionContext completionContext,
        DocumentContext documentContext,
        VSInternalClientCapabilities clientCapabilities,
        RazorCompletionOptions razorCompletionOptions,
        Guid correlationId,
        CancellationToken cancellationToken)
    {
        var positionInfo = _documentMappingService.GetPositionInfo(codeDocument, absoluteIndex);
        if (positionInfo.LanguageKind == RazorLanguageKind.Razor)
        {
            // Nothing to delegate to.
            return default;
        }

        TextEdit? provisionalTextEdit = null;
        if (DelegatedCompletionHelper.TryGetProvisionalCompletionInfo(codeDocument, completionContext, positionInfo, _documentMappingService, out var provisionalCompletion))
        {
            provisionalTextEdit = provisionalCompletion.ProvisionalTextEdit;
            positionInfo = provisionalCompletion.DocumentPositionInfo;
        }

        if (DelegatedCompletionHelper.RewriteContext(completionContext, positionInfo.LanguageKind, _triggerAndCommitCharacters) is not { } rewrittenContext)
        {
            return default;
        }

        completionContext = rewrittenContext;

        // It's a bit confusing, but we have two different "add snippets" options - one is a part of
        // RazorCompletionOptions and becomes a part of RazorCompletionContext and is used by
        // RazorCompletionFactsService, and the second one below that's used for delegated completion
        // Their values are not related in any way.
        var shouldIncludeDelegationSnippets = DelegatedCompletionHelper.ShouldIncludeSnippets(codeDocument, absoluteIndex);

        return new(GetDelegatedCompletionListAsync(
            codeDocument,
            absoluteIndex,
            completionContext,
            documentContext.GetTextDocumentIdentifierAndVersion(),
            clientCapabilities,
            razorCompletionOptions,
            correlationId,
            positionInfo,
            provisionalTextEdit,
            shouldIncludeDelegationSnippets,
            cancellationToken));
    }

    private async Task<VSInternalCompletionList?> GetDelegatedCompletionListAsync(
        RazorCodeDocument codeDocument,
        int absoluteIndex,
        VSInternalCompletionContext completionContext,
        TextDocumentIdentifierAndVersion identifier,
        VSInternalClientCapabilities clientCapabilities,
        RazorCompletionOptions razorCompletionOptions,
        Guid correlationId,
        DocumentPositionInfo positionInfo,
        TextEdit? provisionalTextEdit,
        bool shouldIncludeDelegationSnippets,
        CancellationToken cancellationToken)
    {
        var delegatedParams = new DelegatedCompletionParams(
            identifier,
            positionInfo.Position,
            positionInfo.LanguageKind,
            completionContext,
            provisionalTextEdit,
            shouldIncludeDelegationSnippets,
            correlationId);

        var delegatedResponse = await _clientConnection
            .SendRequestAsync<DelegatedCompletionParams, VSInternalCompletionList?>(
                LanguageServerConstants.RazorCompletionEndpointName,
                delegatedParams,
                cancellationToken)
            .ConfigureAwait(false);

        var rewrittenResponse = positionInfo.LanguageKind == RazorLanguageKind.CSharp
            ? DelegatedCompletionHelper.RewriteCSharpResponse(delegatedResponse, absoluteIndex, codeDocument, positionInfo.Position, razorCompletionOptions)
            : DelegatedCompletionHelper.RewriteHtmlResponse(delegatedResponse, razorCompletionOptions);

        var completionCapability = clientCapabilities?.TextDocument?.Completion as VSInternalCompletionSetting;
        var resolutionContext = new DelegatedCompletionResolutionContext(delegatedParams, rewrittenResponse.Data);
        var resultId = _completionListCache.Add(rewrittenResponse, resolutionContext);
        rewrittenResponse.SetResultId(resultId, completionCapability);

        return rewrittenResponse;
    }
}
