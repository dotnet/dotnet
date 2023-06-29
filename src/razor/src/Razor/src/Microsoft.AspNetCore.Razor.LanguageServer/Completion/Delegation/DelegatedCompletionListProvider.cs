// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class DelegatedCompletionListProvider
{
    private static readonly ImmutableHashSet<string> s_razorTriggerCharacters = new[] { "@" }.ToImmutableHashSet();
    private static readonly ImmutableHashSet<string> s_csharpTriggerCharacters = new[] { " ", "(", "=", "#", ".", "<", "[", "{", "\"", "/", ":", "~" }.ToImmutableHashSet();
    private static readonly ImmutableHashSet<string> s_htmlTriggerCharacters = new[] { ":", "@", "#", ".", "!", "*", ",", "(", "[", "-", "<", "&", "\\", "/", "'", "\"", "=", ":", " ", "`" }.ToImmutableHashSet();
    private static readonly ImmutableHashSet<string> s_allTriggerCharacters =
        s_csharpTriggerCharacters
            .Union(s_htmlTriggerCharacters)
            .Union(s_razorTriggerCharacters);

    private readonly ImmutableArray<DelegatedCompletionResponseRewriter> _responseRewriters;
    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly CompletionListCache _completionListCache;

    public DelegatedCompletionListProvider(
        IEnumerable<DelegatedCompletionResponseRewriter> responseRewriters,
        IRazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        CompletionListCache completionListCache)
    {
        _responseRewriters = responseRewriters.OrderBy(rewriter => rewriter.Order).ToImmutableArray();
        _documentMappingService = documentMappingService;
        _languageServer = languageServer;
        _completionListCache = completionListCache;
    }

    // virtual for tests
    public virtual ImmutableHashSet<string> TriggerCharacters => s_allTriggerCharacters;

    // virtual for tests
    public virtual async Task<VSInternalCompletionList?> GetCompletionListAsync(
        int absoluteIndex,
        VSInternalCompletionContext completionContext,
        VersionedDocumentContext documentContext,
        VSInternalClientCapabilities clientCapabilities,
        Guid correlationId,
        CancellationToken cancellationToken)
    {
        var positionInfo = await _documentMappingService.GetPositionInfoAsync(documentContext, absoluteIndex, cancellationToken).ConfigureAwait(false);

        if (positionInfo.LanguageKind == RazorLanguageKind.Razor)
        {
            // Nothing to delegate to.
            return null;
        }

        var provisionalCompletion = await TryGetProvisionalCompletionInfoAsync(documentContext, completionContext, positionInfo, cancellationToken).ConfigureAwait(false);
        TextEdit? provisionalTextEdit = null;
        if (provisionalCompletion is not null)
        {
            provisionalTextEdit = provisionalCompletion.ProvisionalTextEdit;
            positionInfo = provisionalCompletion.ProvisionalPositionInfo;
        }

        completionContext = RewriteContext(completionContext, positionInfo.LanguageKind);

        var delegatedParams = new DelegatedCompletionParams(
            documentContext.Identifier,
            positionInfo.Position,
            positionInfo.LanguageKind,
            completionContext,
            provisionalTextEdit,
            correlationId);

        var delegatedResponse = await _languageServer.SendRequestAsync<DelegatedCompletionParams, VSInternalCompletionList?>(
            LanguageServerConstants.RazorCompletionEndpointName,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        if (delegatedResponse is null)
        {
            return null;
        }

        var rewrittenResponse = delegatedResponse;

        foreach (var rewriter in _responseRewriters)
        {
            rewrittenResponse = await rewriter.RewriteAsync(
                rewrittenResponse,
                absoluteIndex,
                documentContext,
                delegatedParams,
                cancellationToken).ConfigureAwait(false);
        }

        var completionCapability = clientCapabilities?.TextDocument?.Completion as VSInternalCompletionSetting;
        var resolutionContext = new DelegatedCompletionResolutionContext(delegatedParams, rewrittenResponse.Data);
        var resultId = _completionListCache.Add(rewrittenResponse, resolutionContext);
        rewrittenResponse.SetResultId(resultId, completionCapability);

        return rewrittenResponse;
    }

    private static VSInternalCompletionContext RewriteContext(VSInternalCompletionContext context, RazorLanguageKind languageKind)
    {
        if (context.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
            context.TriggerCharacter is not { } triggerCharacter)
        {
            // Non-triggered based completion, the existing context is valid.
            return context;
        }

        if (languageKind == RazorLanguageKind.CSharp && s_csharpTriggerCharacters.Contains(triggerCharacter))
        {
            // C# trigger character for C# content
            return context;
        }

        if (languageKind == RazorLanguageKind.Html && s_htmlTriggerCharacters.Contains(triggerCharacter))
        {
            // HTML trigger character for HTML content
            return context;
        }

        // Trigger character not associated with the current language. Transform the context into an invoked context.
        var rewrittenContext = new VSInternalCompletionContext()
        {
            InvokeKind = context.InvokeKind,
            TriggerKind = CompletionTriggerKind.Invoked,
        };

        if (languageKind == RazorLanguageKind.CSharp && s_razorTriggerCharacters.Contains(triggerCharacter))
        {
            // The C# language server will not return any completions for the '@' character unless we
            // send the completion request explicitly.
            rewrittenContext.InvokeKind = VSInternalCompletionInvokeKind.Explicit;
        }

        return rewrittenContext;
    }

    private async Task<ProvisionalCompletionInfo?> TryGetProvisionalCompletionInfoAsync(
        VersionedDocumentContext documentContext,
        VSInternalCompletionContext completionContext,
        DocumentPositionInfo positionInfo,
        CancellationToken cancellationToken)
    {
        if (positionInfo.LanguageKind != RazorLanguageKind.Html ||
            completionContext.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
            completionContext.TriggerCharacter != ".")
        {
            // Invalid provisional completion context
            return null;
        }

        if (positionInfo.Position.Character == 0)
        {
            // We're at the start of line. Can't have provisional completions here.
            return null;
        }

        var previousCharacterPositionInfo = await _documentMappingService
            .GetPositionInfoAsync(documentContext, positionInfo.HostDocumentIndex - 1, cancellationToken)
            .ConfigureAwait(false);

        if (previousCharacterPositionInfo.LanguageKind != RazorLanguageKind.CSharp)
        {
            return null;
        }

        var previousPosition = previousCharacterPositionInfo.Position;

        // Edit the CSharp projected document to contain a '.'. This allows C# completion to provide valid
        // completion items for moments when a user has typed a '.' that's typically interpreted as Html.
        var addProvisionalDot = new TextEdit()
        {
            Range = new Range()
            {
                Start = previousPosition,
                End = previousPosition,
            },
            NewText = ".",
        };

        var provisionalPositionInfo = new DocumentPositionInfo(
            RazorLanguageKind.CSharp,
            new Position(
                previousPosition.Line,
                previousPosition.Character + 1),
            previousCharacterPositionInfo.HostDocumentIndex + 1);

        return new ProvisionalCompletionInfo(addProvisionalDot, provisionalPositionInfo);
    }

    private record class ProvisionalCompletionInfo(TextEdit ProvisionalTextEdit, DocumentPositionInfo ProvisionalPositionInfo);
}
