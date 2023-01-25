// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal class DelegatedCompletionListProvider
{
    private static readonly IReadOnlyList<string> s_razorTriggerCharacters = new[] { "@" };
    private static readonly IReadOnlyList<string> s_cSharpTriggerCharacters = new[] { " ", "(", "=", "#", ".", "<", "[", "{", "\"", "/", ":", "~" };
    private static readonly IReadOnlyList<string> s_htmlTriggerCharacters = new[] { ":", "@", "#", ".", "!", "*", ",", "(", "[", "-", "<", "&", "\\", "/", "'", "\"", "=", ":", " ", "`" };
    private static readonly ImmutableHashSet<string> s_allTriggerCharacters =
        s_cSharpTriggerCharacters
            .Concat(s_htmlTriggerCharacters)
            .Concat(s_razorTriggerCharacters)
            .ToImmutableHashSet();

    private readonly IReadOnlyList<DelegatedCompletionResponseRewriter> _responseRewriters;
    private readonly RazorDocumentMappingService _documentMappingService;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly CompletionListCache _completionListCache;

    public DelegatedCompletionListProvider(
        IEnumerable<DelegatedCompletionResponseRewriter> responseRewriters,
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        CompletionListCache completionListCache)
    {
        _responseRewriters = responseRewriters.OrderBy(rewriter => rewriter.Order).ToArray();
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
        DocumentContext documentContext,
        VSInternalClientCapabilities clientCapabilities,
        CancellationToken cancellationToken)
    {
        var projection = await _documentMappingService.GetProjectionAsync(documentContext, absoluteIndex, cancellationToken).ConfigureAwait(false);

        if (projection.LanguageKind == RazorLanguageKind.Razor)
        {
            // Nothing to delegate to.
            return null;
        }

        var provisionalCompletion = await TryGetProvisionalCompletionInfoAsync(documentContext, completionContext, projection, cancellationToken).ConfigureAwait(false);
        TextEdit? provisionalTextEdit = null;
        if (provisionalCompletion is not null)
        {
            provisionalTextEdit = provisionalCompletion.ProvisionalTextEdit;
            projection = provisionalCompletion.ProvisionalProjection;
        }

        completionContext = RewriteContext(completionContext, projection.LanguageKind);

        var delegatedParams = new DelegatedCompletionParams(
            documentContext.Identifier,
            projection.Position,
            projection.LanguageKind,
            completionContext,
            provisionalTextEdit);
        var delegatedResponse = await _languageServer.SendRequestAsync<DelegatedCompletionParams, VSInternalCompletionList?>(
            LanguageServerConstants.RazorCompletionEndpointName,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        if (delegatedResponse is null)
        {
            return null;
        }

        var rewrittenCompletionList = delegatedResponse;
        foreach (var rewriter in _responseRewriters)
        {
            rewrittenCompletionList = await rewriter.RewriteAsync(rewrittenCompletionList,
                                                                  absoluteIndex,
                                                                  documentContext,
                                                                  delegatedParams,
                                                                  cancellationToken).ConfigureAwait(false);
        }

        var completionCapability = clientCapabilities?.TextDocument?.Completion as VSInternalCompletionSetting;
        var resolutionContext = new DelegatedCompletionResolutionContext(delegatedParams, rewrittenCompletionList.Data);
        var resultId = _completionListCache.Set(rewrittenCompletionList, resolutionContext);
        rewrittenCompletionList.SetResultId(resultId, completionCapability);

        return rewrittenCompletionList;
    }

    private static VSInternalCompletionContext RewriteContext(VSInternalCompletionContext context, RazorLanguageKind languageKind)
    {
        if (context.TriggerKind != CompletionTriggerKind.TriggerCharacter)
        {
            // Non-triggered based completion, the existing context is valid.
            return context;
        }

        if (languageKind == RazorLanguageKind.CSharp && s_cSharpTriggerCharacters.Contains(context.TriggerCharacter))
        {
            // C# trigger character for C# content
            return context;
        }

        if (languageKind == RazorLanguageKind.Html && s_htmlTriggerCharacters.Contains(context.TriggerCharacter))
        {
            // HTML trigger character for HTML content
            return context;
        }

        // Trigger character not associated with the current langauge. Transform the context into an invoked context.
        var rewrittenContext = new VSInternalCompletionContext()
        {
            InvokeKind = context.InvokeKind,
            TriggerKind = CompletionTriggerKind.Invoked,
        };

        if (languageKind == RazorLanguageKind.CSharp && s_razorTriggerCharacters.Contains(context.TriggerCharacter))
        {
            // The C# language server will not return any completions for the '@' character unless we
            // send the completion request explicitly.
            rewrittenContext.InvokeKind = VSInternalCompletionInvokeKind.Explicit;
        }

        return rewrittenContext;
    }

    private async Task<ProvisionalCompletionInfo?> TryGetProvisionalCompletionInfoAsync(
        DocumentContext documentContext,
        VSInternalCompletionContext completionContext,
        Projection projection,
        CancellationToken cancellationToken)
    {
        if (projection.LanguageKind != RazorLanguageKind.Html ||
            completionContext.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
            completionContext.TriggerCharacter != ".")
        {
            // Invalid provisional completion context
            return null;
        }

        if (projection.Position.Character == 0)
        {
            // We're at the start of line. Can't have provisional completions here.
            return null;
        }

        var previousCharacterProjection = await _documentMappingService.GetProjectionAsync(documentContext, projection.AbsoluteIndex - 1, cancellationToken).ConfigureAwait(false);
        if (previousCharacterProjection.LanguageKind != RazorLanguageKind.CSharp)
        {
            return null;
        }

        // Edit the CSharp projected document to contain a '.'. This allows C# completion to provide valid
        // completion items for moments when a user has typed a '.' that's typically interpreted as Html.
        var addProvisionalDot = new TextEdit()
        {
            Range = new Range()
            {
                Start = previousCharacterProjection.Position,
                End = previousCharacterProjection.Position,
            },
            NewText = ".",
        };
        var provisionalProjection = new Projection(
            RazorLanguageKind.CSharp,
            new Position(
                previousCharacterProjection.Position.Line,
                previousCharacterProjection.Position.Character + 1),
            previousCharacterProjection.AbsoluteIndex + 1);
        return new ProvisionalCompletionInfo(addProvisionalDot, provisionalProjection);
    }

    private record class ProvisionalCompletionInfo(TextEdit ProvisionalTextEdit, Projection ProvisionalProjection);
}
