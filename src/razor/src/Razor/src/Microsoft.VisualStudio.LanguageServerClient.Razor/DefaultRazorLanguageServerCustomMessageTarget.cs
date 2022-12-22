// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;
using Microsoft.AspNetCore.Razor.LanguageServer.Folding;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.LanguageServerClient.Razor.WrapWithTag;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json.Linq;
using ImplementationResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalReferenceItem[]>;
using SemanticTokensRangeParams = Microsoft.VisualStudio.LanguageServer.Protocol.SemanticTokensRangeParams;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(RazorLanguageServerCustomMessageTarget))]
internal class DefaultRazorLanguageServerCustomMessageTarget : RazorLanguageServerCustomMessageTarget
{
    private readonly TrackingLSPDocumentManager _documentManager;
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly FormattingOptionsProvider _formattingOptionsProvider;
    private readonly EditorSettingsManager _editorSettingsManager;
    private readonly LSPDocumentSynchronizer _documentSynchronizer;

    [ImportingConstructor]
    public DefaultRazorLanguageServerCustomMessageTarget(
        LSPDocumentManager documentManager,
        JoinableTaskContext joinableTaskContext,
        LSPRequestInvoker requestInvoker,
        FormattingOptionsProvider formattingOptionsProvider,
        EditorSettingsManager editorSettingsManager,
        LSPDocumentSynchronizer documentSynchronizer)
    {
        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (formattingOptionsProvider is null)
        {
            throw new ArgumentNullException(nameof(formattingOptionsProvider));
        }

        if (editorSettingsManager is null)
        {
            throw new ArgumentNullException(nameof(editorSettingsManager));
        }

        if (documentSynchronizer is null)
        {
            throw new ArgumentNullException(nameof(documentSynchronizer));
        }

        _documentManager = (TrackingLSPDocumentManager)documentManager;

        if (_documentManager is null)
        {
            throw new ArgumentException("The LSP document manager should be of type " + typeof(TrackingLSPDocumentManager).FullName, nameof(_documentManager));
        }

        _joinableTaskFactory = joinableTaskContext.Factory;
        _requestInvoker = requestInvoker;
        _formattingOptionsProvider = formattingOptionsProvider;
        _editorSettingsManager = editorSettingsManager;
        _documentSynchronizer = documentSynchronizer;
    }

    // Testing constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal DefaultRazorLanguageServerCustomMessageTarget(TrackingLSPDocumentManager documentManager,
        LSPDocumentSynchronizer documentSynchronizer)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _documentManager = documentManager;
        _documentSynchronizer = documentSynchronizer;
    }

    public override async Task UpdateCSharpBufferAsync(UpdateBufferRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        UpdateCSharpBuffer(request);
    }

    // Internal for testing
    internal void UpdateCSharpBuffer(UpdateBufferRequest request)
    {
        if (request is null || request.HostDocumentFilePath is null || request.HostDocumentVersion is null)
        {
            return;
        }

        var hostDocumentUri = new Uri(request.HostDocumentFilePath);
        _documentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
            hostDocumentUri,
            request.Changes.Select(change => change.ToVisualStudioTextChange()).ToArray(),
            request.HostDocumentVersion.Value,
            state: null);
    }

    public override async Task UpdateHtmlBufferAsync(UpdateBufferRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        UpdateHtmlBuffer(request);
    }

    // Internal for testing
    internal void UpdateHtmlBuffer(UpdateBufferRequest request)
    {
        if (request is null || request.HostDocumentFilePath is null || request.HostDocumentVersion is null)
        {
            return;
        }

        var hostDocumentUri = new Uri(request.HostDocumentFilePath);
        _documentManager.UpdateVirtualDocument<HtmlVirtualDocument>(
            hostDocumentUri,
            request.Changes.Select(change => change.ToVisualStudioTextChange()).ToArray(),
            request.HostDocumentVersion.Value,
            state: null);
    }

    public override async Task<RazorDocumentRangeFormattingResponse> RazorDocumentFormattingAsync(VersionedDocumentFormattingParams request, CancellationToken cancellationToken)
    {
        var response = new RazorDocumentRangeFormattingResponse() { Edits = Array.Empty<TextEdit>() };

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var (synchronized, htmlDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
            request.HostDocumentVersion,
            request.TextDocument.Uri,
            cancellationToken);

        var languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        var projectedUri = htmlDocument.Uri;

        if (!synchronized)
        {
            Debug.Fail("RangeFormatting not synchronized.");
            return response;
        }

        var formattingParams = new DocumentFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = projectedUri },
            Options = request.Options
        };

        var textBuffer = htmlDocument.Snapshot.TextBuffer;
        var edits = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentFormattingParams, TextEdit[]>(
            textBuffer,
            Methods.TextDocumentFormattingName,
            languageServerName,
            formattingParams,
            cancellationToken).ConfigureAwait(false);

        response.Edits = edits?.Response ?? Array.Empty<TextEdit>();

        return response;
    }

    public override async Task<RazorDocumentRangeFormattingResponse> HtmlOnTypeFormattingAsync(RazorDocumentOnTypeFormattingParams request, CancellationToken cancellationToken)
    {
        var response = new RazorDocumentRangeFormattingResponse() { Edits = Array.Empty<TextEdit>() };

        var hostDocumentUri = request.TextDocument.Uri;

        var languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        var (synchronized, htmlDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
            request.HostDocumentVersion, hostDocumentUri, cancellationToken);

        if (!synchronized)
        {
            return response;
        }

        var formattingParams = new DocumentOnTypeFormattingParams()
        {
            Character = request.Character,
            Position = request.Position,
            TextDocument = new TextDocumentIdentifier() { Uri = htmlDocument.Uri },
            Options = request.Options
        };

        var textBuffer = htmlDocument.Snapshot.TextBuffer;
        var edits = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentOnTypeFormattingParams, TextEdit[]>(
            textBuffer,
            Methods.TextDocumentOnTypeFormattingName,
            languageServerName,
            formattingParams,
            cancellationToken).ConfigureAwait(false);

        response.Edits = edits?.Response ?? Array.Empty<TextEdit>();

        return response;
    }

    public override async Task<RazorDocumentRangeFormattingResponse> RazorRangeFormattingAsync(RazorDocumentRangeFormattingParams request, CancellationToken cancellationToken)
    {
        var response = new RazorDocumentRangeFormattingResponse() { Edits = Array.Empty<TextEdit>() };

        if (request.Kind == RazorLanguageKind.Razor)
        {
            return response;
        }

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var hostDocumentUri = new Uri(request.HostDocumentFilePath);
        var (synchronized, csharpDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
            request.HostDocumentVersion,
            hostDocumentUri,
            cancellationToken);

        string languageServerName;
        Uri projectedUri;

        if (!synchronized)
        {
            // Document could not be synchronized
            return response;
        }

        if (request.Kind == RazorLanguageKind.CSharp)
        {
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
            projectedUri = csharpDocument.Uri;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This can't really happen in a real scenario.");
            return response;
        }

        var formattingParams = new DocumentRangeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = projectedUri },
            Range = request.ProjectedRange,
            Options = request.Options
        };

        var textBuffer = csharpDocument.Snapshot.TextBuffer;
        var edits = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentRangeFormattingParams, TextEdit[]>(
            textBuffer,
            Methods.TextDocumentRangeFormattingName,
            languageServerName,
            formattingParams,
            cancellationToken).ConfigureAwait(false);

        response.Edits = edits?.Response ?? Array.Empty<TextEdit>();

        return response;
    }

    public override async Task<IReadOnlyList<VSInternalCodeAction>?> ProvideCodeActionsAsync(DelegatedCodeActionParams codeActionParams, CancellationToken cancellationToken)
    {
        if (codeActionParams is null)
        {
            throw new ArgumentNullException(nameof(codeActionParams));
        }

        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (codeActionParams.LanguageKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                codeActionParams.HostDocumentVersion,
                codeActionParams.CodeActionParams.TextDocument.Uri,
                cancellationToken);
        }
        else if (codeActionParams.LanguageKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                codeActionParams.HostDocumentVersion,
                codeActionParams.CodeActionParams.TextDocument.Uri,
                cancellationToken);
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This shouldn't really happen in a real scenario.");
            return null;
        }

        if (!synchronized || virtualDocumentSnapshot is null)
        {
            // Document could not synchronize
            return null;
        }

        codeActionParams.CodeActionParams.TextDocument.Uri = virtualDocumentSnapshot.Uri;

        var textBuffer = virtualDocumentSnapshot.Snapshot.TextBuffer;
        var requests = _requestInvoker.ReinvokeRequestOnMultipleServersAsync<CodeActionParams, IReadOnlyList<VSInternalCodeAction>>(
            textBuffer,
            Methods.TextDocumentCodeActionName,
            SupportsCodeActionResolve,
            codeActionParams.CodeActionParams,
            cancellationToken).ConfigureAwait(false);

        var codeActions = new List<VSInternalCodeAction>();
        await foreach (var response in requests)
        {
            if (response.Response != null)
            {
                codeActions.AddRange(response.Response);
            }
        }

        return codeActions;
    }

    public override async Task<VSInternalCodeAction?> ResolveCodeActionsAsync(RazorResolveCodeActionParams resolveCodeActionParams, CancellationToken cancellationToken)
    {
        if (resolveCodeActionParams is null)
        {
            throw new ArgumentNullException(nameof(resolveCodeActionParams));
        }

        if (!_documentManager.TryGetDocument(resolveCodeActionParams.Uri, out var documentSnapshot))
        {
            // Couldn't resolve the document associated with the code action bail out.
            return null;
        }

        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (resolveCodeActionParams.LanguageKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                resolveCodeActionParams.HostDocumentVersion,
                resolveCodeActionParams.Uri,
                cancellationToken);
        }
        else if (resolveCodeActionParams.LanguageKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                resolveCodeActionParams.HostDocumentVersion,
                resolveCodeActionParams.Uri,
                cancellationToken);
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This shouldn't really happen in a real scenario.");
            return null;
        }

        if (!synchronized || virtualDocumentSnapshot is null)
        {
            // Document could not synchronize
            return null;
        }

        var textBuffer = virtualDocumentSnapshot.Snapshot.TextBuffer;
        var codeAction = resolveCodeActionParams.CodeAction;
        var requests = _requestInvoker.ReinvokeRequestOnMultipleServersAsync<CodeAction, VSInternalCodeAction?>(
            textBuffer,
            Methods.CodeActionResolveName,
            SupportsCodeActionResolve,
            codeAction,
            cancellationToken).ConfigureAwait(false);

        await foreach (var response in requests)
        {
            if (response.Response is not null)
            {
                // Only take the first response from a resolution
                return response.Response;
            }
        }

        return null;
    }

    public override async Task<ProvideSemanticTokensResponse?> ProvideSemanticTokensRangeAsync(
        ProvideSemanticTokensRangeParams semanticTokensParams,
        CancellationToken cancellationToken)
    {
        if (semanticTokensParams is null)
        {
            throw new ArgumentNullException(nameof(semanticTokensParams));
        }

        if (semanticTokensParams.Range is null)
        {
            throw new ArgumentNullException(nameof(semanticTokensParams.Range));
        }

        var (synchronized, csharpDoc) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
            (int)semanticTokensParams.RequiredHostDocumentVersion, semanticTokensParams.TextDocument.Uri, cancellationToken);

        if (csharpDoc is null)
        {
            return null;
        }

        if (!synchronized)
        {
            // If we're unable to synchronize we won't produce useful results, but we have to indicate
            // it's due to out of sync by providing the old version
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: -1);
        }

        semanticTokensParams.TextDocument.Uri = csharpDoc.Uri;

        var newParams = new SemanticTokensRangeParams
        {
            TextDocument = semanticTokensParams.TextDocument,
            PartialResultToken = semanticTokensParams.PartialResultToken,
            Range = semanticTokensParams.Range,
        };

        var textBuffer = csharpDoc.Snapshot.TextBuffer;
        var csharpResults = await _requestInvoker.ReinvokeRequestOnServerAsync<SemanticTokensRangeParams, VSSemanticTokensResponse>(
            textBuffer,
            Methods.TextDocumentSemanticTokensRangeName,
            RazorLSPConstants.RazorCSharpLanguageServerName,
            newParams,
            cancellationToken).ConfigureAwait(false);

        var result = csharpResults?.Response;
        if (result is null)
        {
            // Weren't able to re-invoke C# semantic tokens but we have to indicate it's due to out of sync by providing the old version
            return new ProvideSemanticTokensResponse(tokens: null, hostDocumentSyncVersion: csharpDoc.HostDocumentSyncVersion);
        }

        var response = new ProvideSemanticTokensResponse(result.Data, semanticTokensParams.RequiredHostDocumentVersion);

        return response;
    }

    public override async Task<IReadOnlyList<ColorInformation>?> ProvideHtmlDocumentColorAsync(DelegatedDocumentColorParams documentColorParams, CancellationToken cancellationToken)
    {
        if (documentColorParams is null)
        {
            throw new ArgumentNullException(nameof(documentColorParams));
        }

        var (synchronized, htmlDoc) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
            documentColorParams.HostDocumentVersion, documentColorParams.TextDocument.Uri, cancellationToken);

        documentColorParams.TextDocument.Uri = htmlDoc.Uri;
        var htmlTextBuffer = htmlDoc.Snapshot.TextBuffer;
        var requests = _requestInvoker.ReinvokeRequestOnMultipleServersAsync<DocumentColorParams, ColorInformation[]>(
            htmlTextBuffer,
            Methods.DocumentColorRequest.Name,
            SupportsDocumentColor,
            documentColorParams,
            cancellationToken).ConfigureAwait(false);

        var colorInformation = new List<ColorInformation>();
        await foreach (var response in requests)
        {
            if (response.Response is not null)
            {
                colorInformation.AddRange(response.Response);
            }
        }

        return colorInformation;
    }

    private static bool SupportsCodeActionResolve(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        var (providesCodeActions, resolvesCodeActions) = serverCapabilities?.CodeActionProvider?.Match(
            boolValue => (boolValue, false),
            options => (true, options.ResolveProvider)) ?? (false, false);

        return providesCodeActions && resolvesCodeActions;
    }

    private static bool SupportsDocumentColor(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        var supportsDocumentColor = serverCapabilities?.DocumentColorProvider?.Match(
            boolValue => boolValue,
            options => options != null) ?? false;

        return supportsDocumentColor;
    }

    // NOTE: This method is a polyfill for VS. We only intend to do it this way until VS formally
    // supports sending workspace configuration requests.
    public override Task<object[]> WorkspaceConfigurationAsync(
        ConfigurationParams configParams,
        CancellationToken cancellationToken)
    {
        if (configParams is null)
        {
            throw new ArgumentNullException(nameof(configParams));
        }

        var result = new List<object>();
        foreach (var item in configParams.Items)
        {
            // Right now in VS we only care about editor settings, but we should update this logic later if
            // we want to support Razor and HTML settings as well.
            var setting = item.Section == "vs.editor.razor"
                ? _editorSettingsManager.Current
                : new object();
            result.Add(setting);
        }

        return Task.FromResult(result.ToArray());
    }

    public override async Task<VSInternalWrapWithTagResponse> RazorWrapWithTagAsync(VSInternalWrapWithTagParams wrapWithParams, CancellationToken cancellationToken)
    {
        // Same as in LanguageServerConstants, and in Web Tools
        const string HtmlWrapWithTagEndpoint = "textDocument/_vsweb_wrapWithTag";

        var response = new VSInternalWrapWithTagResponse(wrapWithParams.Range, Array.Empty<TextEdit>());

        var (synchronized, htmlDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
            wrapWithParams.TextDocument.Version,
            wrapWithParams.TextDocument.Uri,
            cancellationToken);

        if (!synchronized)
        {
            Debug.Fail("Document was not synchronized");
            return response;
        }

        var languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        var projectedUri = htmlDocument.Uri;

        // We call the Html language server to do the actual work here, now that we have the vitrual document that they know about
        var request = new VSInternalWrapWithTagParams(
            wrapWithParams.Range,
            wrapWithParams.TagName,
            wrapWithParams.Options,
            new VersionedTextDocumentIdentifier() { Uri = projectedUri, });

        var textBuffer = htmlDocument.Snapshot.TextBuffer;
        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalWrapWithTagParams, VSInternalWrapWithTagResponse>(
            textBuffer,
            HtmlWrapWithTagEndpoint,
            languageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        if (result?.Response is not null)
        {
            response = result.Response;
        }

        return response;
    }

    public override async Task<VSInternalInlineCompletionList?> ProvideInlineCompletionAsync(RazorInlineCompletionRequest inlineCompletionParams, CancellationToken cancellationToken)
    {
        if (inlineCompletionParams is null)
        {
            throw new ArgumentNullException(nameof(inlineCompletionParams));
        }

        var hostDocumentUri = inlineCompletionParams.TextDocument.Uri;
        if (!_documentManager.TryGetDocument(hostDocumentUri, out var documentSnapshot))
        {
            return null;
        }

        if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var csharpDoc))
        {
            return null;
        }

        var csharpRequest = new VSInternalInlineCompletionRequest
        {
            Context = inlineCompletionParams.Context,
            Position = inlineCompletionParams.Position,
            TextDocument = new TextDocumentIdentifier { Uri = csharpDoc.Uri, },
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

    public override async Task<RazorFoldingRangeResponse?> ProvideFoldingRangesAsync(RazorFoldingRangeRequestParam foldingRangeParams, CancellationToken cancellationToken)
    {
        if (foldingRangeParams is null)
        {
            throw new ArgumentNullException(nameof(foldingRangeParams));
        }

        var csharpRanges = new List<FoldingRange>();
        var csharpTask = Task.Run(async () =>
        {
            var (synchronized, csharpSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                foldingRangeParams.HostDocumentVersion, foldingRangeParams.TextDocument.Uri, cancellationToken);

            if (synchronized)
            {
                var csharpRequestParams = new FoldingRangeParams()
                {
                    TextDocument = new()
                    {
                        Uri = csharpSnapshot.Uri
                    }
                };

                var request = await _requestInvoker.ReinvokeRequestOnServerAsync<FoldingRangeParams, IEnumerable<FoldingRange>?>(
                    csharpSnapshot.Snapshot.TextBuffer,
                    Methods.TextDocumentFoldingRange.Name,
                    RazorLSPConstants.RazorCSharpLanguageServerName,
                    SupportsFoldingRange,
                    csharpRequestParams,
                    cancellationToken).ConfigureAwait(false);

                var result = request?.Response;
                if (result is null)
                {
                    csharpRanges = null;
                }
                else
                {
                    csharpRanges.AddRange(result);
                }
            }
        }, cancellationToken);

        var htmlRanges = new List<FoldingRange>();
        var htmlTask = Task.CompletedTask;
        htmlTask = Task.Run(async () =>
        {
            var (synchronized, htmlDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                foldingRangeParams.HostDocumentVersion, foldingRangeParams.TextDocument.Uri, cancellationToken);

            if (synchronized)
            {
                var htmlRequestParams = new FoldingRangeParams()
                {
                    TextDocument = new()
                    {
                        Uri = htmlDocument.Uri
                    }
                };

                var request = await _requestInvoker.ReinvokeRequestOnServerAsync<FoldingRangeParams, IEnumerable<FoldingRange>?>(
                    htmlDocument.Snapshot.TextBuffer,
                    Methods.TextDocumentFoldingRange.Name,
                    RazorLSPConstants.HtmlLanguageServerName,
                    SupportsFoldingRange,
                    htmlRequestParams,
                    cancellationToken).ConfigureAwait(false);

                var result = request?.Response;
                if (result is null)
                {
                    htmlRanges = null;
                }
                else
                {
                    htmlRanges.AddRange(result);
                }
            }
        }, cancellationToken);

        var allTasks = Task.WhenAll(htmlTask, csharpTask);

        try
        {
            await allTasks.ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Return null if any of the tasks getting folding ranges
            // results in an error
            return null;
        }

        return new(htmlRanges.ToImmutableArray(), csharpRanges.ToImmutableArray());
    }

    private static bool SupportsFoldingRange(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        var supportsFoldingRange = serverCapabilities?.FoldingRangeProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;

        return supportsFoldingRange;
    }

    public override Task<WorkspaceEdit?> ProvideTextPresentationAsync(RazorTextPresentationParams presentationParams, CancellationToken cancellationToken)
    {
        return ProvidePresentationAsync(presentationParams, presentationParams.TextDocument.Uri, presentationParams.HostDocumentVersion, presentationParams.Kind, VSInternalMethods.TextDocumentTextPresentationName, cancellationToken);
    }

    public override Task<WorkspaceEdit?> ProvideUriPresentationAsync(RazorUriPresentationParams presentationParams, CancellationToken cancellationToken)
    {
        return ProvidePresentationAsync(presentationParams, presentationParams.TextDocument.Uri, presentationParams.HostDocumentVersion, presentationParams.Kind, VSInternalMethods.TextDocumentUriPresentationName, cancellationToken);
    }

    public async Task<WorkspaceEdit?> ProvidePresentationAsync<TParams>(TParams presentationParams, Uri hostDocumentUri, int hostDocumentVersion, RazorLanguageKind kind, string methodName, CancellationToken cancellationToken)
        where TParams : notnull, IPresentationParams
    {
        string languageServerName;
        VirtualDocumentSnapshot document;
        if (kind == RazorLanguageKind.CSharp)
        {
            var syncResult = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                hostDocumentVersion,
                hostDocumentUri,
                cancellationToken);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
            presentationParams.TextDocument = new TextDocumentIdentifier
            {
                Uri = syncResult.VirtualSnapshot.Uri,
            };
            document = syncResult.VirtualSnapshot;
        }
        else if (kind == RazorLanguageKind.Html)
        {
            var syncResult = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                hostDocumentVersion,
                hostDocumentUri,
                cancellationToken);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
            presentationParams.TextDocument = new TextDocumentIdentifier
            {
                Uri = syncResult.VirtualSnapshot.Uri,
            };
            document = syncResult.VirtualSnapshot;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This can't really happen in a real scenario.");
            return null;
        }

        var textBuffer = document.Snapshot.TextBuffer;
        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<TParams, WorkspaceEdit?>(
            textBuffer,
            methodName,
            languageServerName,
            presentationParams,
            cancellationToken).ConfigureAwait(false);

        return result?.Response;
    }

    // JToken returning because there's no value in converting the type into its final type because this method serves entirely as a delegation point (immedaitely re-serializes).
    public override async Task<JToken?> ProvideCompletionsAsync(
        DelegatedCompletionParams request,
        CancellationToken cancellationToken)
    {
        var hostDocumentUri = request.HostDocument.Uri;

        string languageServerName;
        Uri projectedUri;
        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (request.ProjectedKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                request.HostDocument.Version,
                request.HostDocument.Uri,
                cancellationToken);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
            projectedUri = virtualDocumentSnapshot.Uri;
        }
        else if (request.ProjectedKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                request.HostDocument.Version,
                hostDocumentUri,
                cancellationToken);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
            projectedUri = virtualDocumentSnapshot.Uri;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This shouldn't really happen in a real scenario.");
            return null;
        }

        if (!synchronized)
        {
            return null;
        }

        var completionParams = new CompletionParams()
        {
            Context = request.Context,
            Position = request.ProjectedPosition,
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = projectedUri,
            },
        };

        var continueOnCapturedContext = false;
        var provisionalTextEdit = request.ProvisionalTextEdit;
        if (provisionalTextEdit is not null)
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var provisionalChange = new VisualStudioTextChange(provisionalTextEdit, virtualDocumentSnapshot.Snapshot);
            UpdateVirtualDocument(provisionalChange, request.ProjectedKind, request.HostDocument.Version, hostDocumentUri);

            // We want the delegation to continue on the captured context because we're currently on the `main` thread and we need to get back to the
            // main thread in order to update the virtual buffer with the reverted text edit.
            continueOnCapturedContext = true;
        }

        try
        {
            var textBuffer = virtualDocumentSnapshot.Snapshot.TextBuffer;
            var response = await _requestInvoker.ReinvokeRequestOnServerAsync<CompletionParams, JToken?>(
                textBuffer,
                Methods.TextDocumentCompletion.Name,
                languageServerName,
                completionParams,
                cancellationToken).ConfigureAwait(continueOnCapturedContext);
            return response?.Response;
        }
        finally
        {
            if (provisionalTextEdit is not null)
            {
                var revertedProvisionalTextEdit = BuildRevertedEdit(provisionalTextEdit);
                var revertedProvisionalChange = new VisualStudioTextChange(revertedProvisionalTextEdit, virtualDocumentSnapshot.Snapshot);
                UpdateVirtualDocument(revertedProvisionalChange, request.ProjectedKind, request.HostDocument.Version, hostDocumentUri);
            }
        }
    }

    private static TextEdit BuildRevertedEdit(TextEdit provisionalTextEdit)
    {
        TextEdit? revertedProvisionalTextEdit;
        if (provisionalTextEdit.Range.Start == provisionalTextEdit.Range.End)
        {
            // Insertion
            revertedProvisionalTextEdit = new TextEdit()
            {
                Range = new Range()
                {
                    Start = provisionalTextEdit.Range.Start,

                    // We're making an assumption that provisional text edits do not span more than 1 line.
                    End = new Position(provisionalTextEdit.Range.End.Line, provisionalTextEdit.Range.End.Character + provisionalTextEdit.NewText.Length),
                },
                NewText = string.Empty
            };
        }
        else
        {
            // Replace
            revertedProvisionalTextEdit = new TextEdit()
            {
                Range = provisionalTextEdit.Range,
                NewText = string.Empty
            };
        }

        return revertedProvisionalTextEdit;
    }

    private void UpdateVirtualDocument(
        VisualStudioTextChange textChange,
        RazorLanguageKind virtualDocumentKind,
        int hostDocumentVersion,
        Uri documentSnapshotUri)
    {
        if (_documentManager is not TrackingLSPDocumentManager trackingDocumentManager)
        {
            return;
        }

        if (virtualDocumentKind == RazorLanguageKind.CSharp)
        {
            trackingDocumentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
                documentSnapshotUri,
                new[] { textChange },
                hostDocumentVersion,
                state: null);
        }
        else if (virtualDocumentKind == RazorLanguageKind.Html)
        {
            trackingDocumentManager.UpdateVirtualDocument<HtmlVirtualDocument>(
                documentSnapshotUri,
                new[] { textChange },
                hostDocumentVersion,
                state: null);
        }
    }

    public override async Task<JToken?> ProvideResolvedCompletionItemAsync(DelegatedCompletionItemResolveParams request, CancellationToken cancellationToken)
    {
        string languageServerName;
        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (request.OriginatingKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                request.HostDocument.Version,
                request.HostDocument.Uri,
                cancellationToken);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else if (request.OriginatingKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                request.HostDocument.Version,
                request.HostDocument.Uri,
                cancellationToken);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This can't really happen in a real scenario.");
            return null;
        }

        if (!synchronized)
        {
            // Document was not synchronized
            return null;
        }

        var completionResolveParams = request.CompletionItem;

        var textBuffer = virtualDocumentSnapshot.Snapshot.TextBuffer;
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalCompletionItem, JToken?>(
            textBuffer,
            Methods.TextDocumentCompletionResolve.Name,
            languageServerName,
            completionResolveParams,
            cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }

    public override Task<FormattingOptions?> GetFormattingOptionsAsync(TextDocumentIdentifier document, CancellationToken cancellationToken)
    {
        var formattingOptions = _formattingOptionsProvider.GetOptions(document.Uri);
        return Task.FromResult(formattingOptions);
    }

    public override async Task<WorkspaceEdit?> RenameAsync(DelegatedRenameParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return null;
        }

        var renameParams = new RenameParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = delegationDetails.Value.ProjectedUri,
            },
            Position = request.ProjectedPosition,
            NewName = request.NewName,
        };

        var textBuffer = delegationDetails.Value.TextBuffer;
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<RenameParams, WorkspaceEdit?>(
            textBuffer,
            Methods.TextDocumentRenameName,
            delegationDetails.Value.LanguageServerName,
            renameParams,
            cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }

    public override async Task<VSInternalDocumentOnAutoInsertResponseItem?> OnAutoInsertAsync(DelegatedOnAutoInsertParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var onAutoInsertParams = new VSInternalDocumentOnAutoInsertParams
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = delegationDetails.Value.ProjectedUri,
            },
            Position = request.ProjectedPosition,
            Character = request.Character,
            Options = request.Options
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem?>(
           delegationDetails.Value.TextBuffer,
           VSInternalMethods.OnAutoInsertName,
           delegationDetails.Value.LanguageServerName,
           onAutoInsertParams,
           cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }

    public override async Task<Range?> ValidateBreakpointRangeAsync(DelegatedValidateBreakpointRangeParams request, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var validateBreakpointRangeParams = new VSInternalValidateBreakableRangeParams
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = delegationDetails.Value.ProjectedUri,
            },
            Range = request.ProjectedRange
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalValidateBreakableRangeParams, Range?>(
            delegationDetails.Value.TextBuffer,
            VSInternalMethods.TextDocumentValidateBreakableRangeName,
            delegationDetails.Value.LanguageServerName,
            validateBreakpointRangeParams,
            cancellationToken).ConfigureAwait(false);
        return response?.Response;
    }

    public override Task<VSInternalHover?> HoverAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionRequestAsync<VSInternalHover>(request, Methods.TextDocumentHoverName, cancellationToken);

    public override Task<Location[]?> DefinitionAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionRequestAsync<Location[]>(request, Methods.TextDocumentDefinitionName, cancellationToken);

    public override Task<DocumentHighlight[]?> DocumentHighlightAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionRequestAsync<DocumentHighlight[]>(request, Methods.TextDocumentDocumentHighlightName, cancellationToken);

    public override Task<SignatureHelp?> SignatureHelpAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionRequestAsync<SignatureHelp>(request, Methods.TextDocumentSignatureHelpName, cancellationToken);

    public override Task<ImplementationResult> ImplementationAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionRequestAsync<ImplementationResult>(request, Methods.TextDocumentImplementationName, cancellationToken);

    public override Task<VSInternalReferenceItem[]?> ReferencesAsync(DelegatedPositionParams request, CancellationToken cancellationToken)
        => DelegateTextDocumentPositionRequestAsync<VSInternalReferenceItem[]>(request, Methods.TextDocumentReferencesName, cancellationToken);

    public override async Task<RazorPullDiagnosticResponse?> DiagnosticsAsync(DelegatedDiagnosticParams request, CancellationToken cancellationToken)
    {
        var csharpTask = Task.Run(() => GetVirtualDocumentPullDiagnosticsAsync<CSharpVirtualDocumentSnapshot>(request.HostDocument, RazorLSPConstants.RazorCSharpLanguageServerName, cancellationToken), cancellationToken);
        var htmlTask = Task.Run(() => GetVirtualDocumentPullDiagnosticsAsync<HtmlVirtualDocumentSnapshot>(request.HostDocument, RazorLSPConstants.HtmlLanguageServerName, cancellationToken), cancellationToken);

        var allTasks = Task.WhenAll(htmlTask, csharpTask);

        try
        {
            await allTasks.ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Return null if any of the tasks getting diagnostics results in an error
            return null;
        }

        var csharpDiagnostics = await csharpTask.ConfigureAwait(false) ?? Array.Empty<VSInternalDiagnosticReport>();
        var htmlDiagnostics = await htmlTask.ConfigureAwait(false) ?? Array.Empty<VSInternalDiagnosticReport>();

        return new RazorPullDiagnosticResponse(csharpDiagnostics, htmlDiagnostics);
    }

    private async Task<VSInternalDiagnosticReport[]?> GetVirtualDocumentPullDiagnosticsAsync<TVirtualDocumentSnapshot>(VersionedTextDocumentIdentifier hostDocument, string delegatedLanguageServerName, CancellationToken cancellationToken)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        var (synchronized, virtualDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(
            hostDocument.Version,
            hostDocument.Uri,
            cancellationToken).ConfigureAwait(false);
        if (!synchronized)
        {
            return null;
        }

        var request = new VSInternalDocumentDiagnosticsParams
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = virtualDocument.Uri,
            },
        };
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalDocumentDiagnosticsParams, VSInternalDiagnosticReport[]?>(
            virtualDocument.Snapshot.TextBuffer,
            VSInternalMethods.DocumentPullDiagnosticName,
            delegatedLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return null;
        }

        return response.Response;
    }
    private async Task<TResult?> DelegateTextDocumentPositionRequestAsync<TResult>(DelegatedPositionParams request, string methodName, CancellationToken cancellationToken)
    {
        var delegationDetails = await GetProjectedRequestDetailsAsync(request, cancellationToken).ConfigureAwait(false);
        if (delegationDetails is null)
        {
            return default;
        }

        var positionParams = new TextDocumentPositionParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = delegationDetails.Value.ProjectedUri,
            },
            Position = request.ProjectedPosition,
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<TextDocumentPositionParams, TResult?>(
            delegationDetails.Value.TextBuffer,
            methodName,
            delegationDetails.Value.LanguageServerName,
            positionParams,
            cancellationToken).ConfigureAwait(false);

        if (response is null)
        {
            return default;
        }

        return response.Response;
    }

    private async Task<DelegationRequestDetails?> GetProjectedRequestDetailsAsync(IDelegatedParams request, CancellationToken cancellationToken)
    {
        string languageServerName;

        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (request.ProjectedKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                request.HostDocument.Version,
                request.HostDocument.Uri,
                rejectOnNewerParallelRequest: false,
                cancellationToken);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else if (request.ProjectedKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                request.HostDocument.Version,
                request.HostDocument.Uri,
                rejectOnNewerParallelRequest: false,
                cancellationToken);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This shouldn't really happen in a real scenario.");
            return null;
        }

        if (!synchronized)
        {
            return null;
        }

        return new DelegationRequestDetails(languageServerName, virtualDocumentSnapshot.Uri, virtualDocumentSnapshot.Snapshot.TextBuffer);
    }

    private record struct DelegationRequestDetails(string LanguageServerName, Uri ProjectedUri, ITextBuffer TextBuffer);
}
