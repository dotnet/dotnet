// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Completion;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide inline completions from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorInlineCompletionEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalInlineCompletionList?> ProvideInlineCompletionAsync(RazorInlineCompletionRequest inlineCompletionParams, CancellationToken cancellationToken)
    {
        if (inlineCompletionParams is null)
        {
            throw new ArgumentNullException(nameof(inlineCompletionParams));
        }

        var hostDocumentUri = inlineCompletionParams.TextDocument.DocumentUri.GetRequiredParsedUri();
        if (!_documentManager.TryGetDocument(hostDocumentUri, out var documentSnapshot))
        {
            return null;
        }

        // TODO: Support multiple C# documents per Razor document.
        if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var csharpDoc))
        {
            return null;
        }

        var csharpRequest = new VSInternalInlineCompletionRequest
        {
            Context = inlineCompletionParams.Context,
            Position = inlineCompletionParams.Position,
            TextDocument = inlineCompletionParams.TextDocument.WithUri(csharpDoc.Uri),
            Options = inlineCompletionParams.Options,
        };

        var textBuffer = csharpDoc.Snapshot.TextBuffer;
        var request = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalInlineCompletionRequest, VSInternalInlineCompletionList?>(
            textBuffer,
            VSInternalMethods.TextDocumentInlineCompletionName,
            RazorLSPConstants.RazorCSharpLanguageServerName,
            csharpRequest,
            cancellationToken).ConfigureAwait(false);

        return request?.Response;
    }

    [JsonRpcMethod(LanguageServerConstants.RazorCompletionEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<RazorVSInternalCompletionList?> ProvideCompletionsAsync(
        DelegatedCompletionParams request,
        CancellationToken cancellationToken)
    {
        var hostDocumentUri = request.Identifier.TextDocumentIdentifier.DocumentUri.GetRequiredParsedUri();

        string languageServerName;
        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (request.ProjectedKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                request.Identifier.Version,
                request.Identifier.TextDocumentIdentifier,
                cancellationToken);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else if (request.ProjectedKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                request.Identifier.Version,
                request.Identifier.TextDocumentIdentifier,
                cancellationToken);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This shouldn't really happen in a real scenario.");
            return null;
        }

        if (!synchronized || virtualDocumentSnapshot is null)
        {
            return null;
        }

        var completionParams = new RazorVSInternalCompletionParams()
        {
            Context = request.Context,
            Position = request.ProjectedPosition,
            TextDocument = request.Identifier.TextDocumentIdentifier.WithUri(virtualDocumentSnapshot.Uri),
        };

        var continueOnCapturedContext = false;
        var provisionalTextEdit = request.ProvisionalTextEdit;
        if (provisionalTextEdit is not null)
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var provisionalChange = new VisualStudioTextChange(
                provisionalTextEdit.Range.Start.Line,
                provisionalTextEdit.Range.Start.Character,
                provisionalTextEdit.Range.End.Line,
                provisionalTextEdit.Range.End.Character,
                virtualDocumentSnapshot.Snapshot,
                provisionalTextEdit.NewText);
            // We update to a negative version number so that if a request comes in for v6, it won't see our modified document. We revert the version back
            // later, don't worry.
            UpdateVirtualDocument(provisionalChange, request.ProjectedKind, -1 * request.Identifier.Version, hostDocumentUri, virtualDocumentSnapshot.Uri);

            _logger.LogDebug($"Updated for provisional completion to version -{request.Identifier.Version} of {virtualDocumentSnapshot!.Uri}.");

            // We want the delegation to continue on the captured context because we're currently on the `main` thread and we need to get back to the
            // main thread in order to update the virtual buffer with the reverted text edit.
            continueOnCapturedContext = true;
        }

        try
        {
            var textBuffer = virtualDocumentSnapshot.Snapshot.TextBuffer;
            var lspMethodName = Methods.TextDocumentCompletion.Name;
            ReinvocationResponse<RazorVSInternalCompletionList?>? response;
            using (_telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, TelemetryThresholds.CompletionSubLSPTelemetryThreshold, request.CorrelationId))
            {
                response = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorVSInternalCompletionParams, RazorVSInternalCompletionList?>(
                    textBuffer,
                    lspMethodName,
                    languageServerName,
                    completionParams,
                    cancellationToken).ConfigureAwait(continueOnCapturedContext);
            }

            var completionList = response?.Response;
            using var builder = new PooledArrayBuilder<VSInternalCompletionItem>();

            if (completionList is not null)
            {
                builder.AddRange(completionList.Items);
            }
            else
            {
                completionList = new RazorVSInternalCompletionList()
                {
                    Items = [],
                    // If we don't get a response from the delegated server, we have to make sure to return an incomplete completion
                    // list. When a user is typing quickly, the delegated request from the first keystroke will fail to synchronize,
                    // so if we return a "complete" list then the query won't re-query us for completion once the typing stops/slows
                    // so we'd only ever return Razor completion items.
                    IsIncomplete = true,
                };
            }

            if (request.ShouldIncludeSnippets)
            {
                _snippetCompletionItemProvider.AddSnippetCompletions(ref builder.AsRef(), request.ProjectedKind, request.Context.InvokeKind, request.Context.TriggerCharacter);
            }

            completionList.Items = builder.ToArray();

            return completionList;
        }
        finally
        {
            if (provisionalTextEdit is not null)
            {
                _logger.LogDebug($"Reverting the update for provisional completion back to {request.Identifier.Version} of {virtualDocumentSnapshot!.Uri}.");

                var revertedProvisionalTextEdit = BuildRevertedEdit(provisionalTextEdit);
                var revertedProvisionalChange = new VisualStudioTextChange(
                    revertedProvisionalTextEdit.Range.Start.Line,
                    revertedProvisionalTextEdit.Range.Start.Character,
                    revertedProvisionalTextEdit.Range.End.Line,
                    revertedProvisionalTextEdit.Range.End.Character,
                    virtualDocumentSnapshot.Snapshot,
                    revertedProvisionalTextEdit.NewText);
                UpdateVirtualDocument(revertedProvisionalChange, request.ProjectedKind, request.Identifier.Version, hostDocumentUri, virtualDocumentSnapshot.Uri);
            }
        }
    }

    private static TextEdit BuildRevertedEdit(TextEdit provisionalTextEdit)
    {
        TextEdit? revertedProvisionalTextEdit;

        var range = provisionalTextEdit.Range;

        if (range.Start == range.End)
        {
            // Insertion
            revertedProvisionalTextEdit = LspFactory.CreateTextEdit(
                range: LspFactory.CreateSingleLineRange(
                    range.Start,
                    length: provisionalTextEdit.NewText.Length),
                newText: string.Empty);
        }
        else
        {
            // Replace
            revertedProvisionalTextEdit = LspFactory.CreateTextEdit(range, string.Empty);
        }

        return revertedProvisionalTextEdit;
    }

    private void UpdateVirtualDocument(
        VisualStudioTextChange textChange,
        RazorLanguageKind virtualDocumentKind,
        int hostDocumentVersion,
        Uri documentSnapshotUri,
        Uri virtualDocumentUri)
    {
        if (_documentManager is not TrackingLSPDocumentManager trackingDocumentManager)
        {
            return;
        }

        if (virtualDocumentKind == RazorLanguageKind.CSharp)
        {
            trackingDocumentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
                documentSnapshotUri,
                virtualDocumentUri,
                [textChange],
                hostDocumentVersion,
                state: null);
        }
        else if (virtualDocumentKind == RazorLanguageKind.Html)
        {
            trackingDocumentManager.UpdateVirtualDocument<HtmlVirtualDocument>(
                documentSnapshotUri,
                virtualDocumentUri,
                [textChange],
                hostDocumentVersion,
                state: null);
        }
    }

    [JsonRpcMethod(LanguageServerConstants.RazorCompletionResolveEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalCompletionItem?> ProvideResolvedCompletionItemAsync(DelegatedCompletionItemResolveParams request, CancellationToken cancellationToken)
    {
        if (_snippetCompletionItemProvider.TryResolveInsertString(request.CompletionItem, out var snippetInsertText))
        {
            request.CompletionItem.InsertText = snippetInsertText;
            return request.CompletionItem;
        }

        string languageServerName;
        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (request.OriginatingKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                request.Identifier.Version,
                request.Identifier.TextDocumentIdentifier,
                cancellationToken);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else if (request.OriginatingKind == RazorLanguageKind.CSharp)
        {
            // TODO this is a partial workaround to fix prefix completion by avoiding sync (which times out during resolve endpoint) if we are currently at a higher version value
            // this does not fix postfix completion and should be superseded by eventual synchronization fix

            var futureDataSyncResult = TryReturnPossiblyFutureSnapshot<CSharpVirtualDocumentSnapshot>(request.Identifier.Version, request.Identifier.TextDocumentIdentifier);
            if (futureDataSyncResult?.Synchronized == true)
            {
                (synchronized, virtualDocumentSnapshot) = futureDataSyncResult;
            }
            else
            {
                (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                    request.Identifier.Version,
                    request.Identifier.TextDocumentIdentifier,
                    cancellationToken);
            }

            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This can't really happen in a real scenario.");
            return null;
        }

        if (!synchronized || virtualDocumentSnapshot is null)
        {
            // Document was not synchronized
            return null;
        }

        var textBuffer = virtualDocumentSnapshot.Snapshot.TextBuffer;
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalCompletionItem, VSInternalCompletionItem?>(
            textBuffer,
            Methods.TextDocumentCompletionResolve.Name,
            languageServerName,
            request.CompletionItem,
            cancellationToken).ConfigureAwait(false);

        return response?.Response;
    }

    [JsonRpcMethod(LanguageServerConstants.RazorGetFormattingOptionsEndpointName, UseSingleObjectParameterDeserialization = true)]
    public Task<FormattingOptions?> GetFormattingOptionsAsync(TextDocumentIdentifierAndVersion document, CancellationToken _)
    {
        var formattingOptions = _formattingOptionsProvider.GetOptions(document.TextDocumentIdentifier.DocumentUri.GetRequiredParsedUri());

        if (formattingOptions is null)
        {
            return SpecializedTasks.Null<FormattingOptions>();
        }

        var roslynFormattingOptions = new FormattingOptions()
        {
            TabSize = formattingOptions.TabSize,
            InsertSpaces = formattingOptions.InsertSpaces,
            // Options come from the VS protocol DLL, which uses Dict<string, object> for options, but Roslyn is more strongly typed.
            OtherOptions = formattingOptions.OtherOptions?.ToDictionary(k => k.Key, v => v.Value switch
            {
                bool b => b,
                int i => i,
                string s => s,
                _ => Assumes.NotReachable<SumType<bool, int, string>>(),
            }),
        };
        return Task.FromResult<FormattingOptions?>(roslynFormattingOptions);
    }
}
