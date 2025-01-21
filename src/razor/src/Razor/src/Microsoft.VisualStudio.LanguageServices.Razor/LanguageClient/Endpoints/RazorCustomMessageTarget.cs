// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Razor.Snippets;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using static Microsoft.VisualStudio.LanguageServer.ContainedLanguage.DefaultLSPDocumentSynchronizer;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

[Export(typeof(RazorCustomMessageTarget))]
internal partial class RazorCustomMessageTarget
{
    private readonly TrackingLSPDocumentManager _documentManager;
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly ITelemetryReporter _telemetryReporter;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly SnippetCompletionItemProvider _snippetCompletionItemProvider;
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer;
    private readonly FormattingOptionsProvider _formattingOptionsProvider;
    private readonly IClientSettingsManager _editorSettingsManager;
    private readonly LSPDocumentSynchronizer _documentSynchronizer;
    private readonly CSharpVirtualDocumentAddListener _csharpVirtualDocumentAddListener;
    private readonly ILogger _logger;

    [ImportingConstructor]
    public RazorCustomMessageTarget(
        LSPDocumentManager documentManager,
        JoinableTaskContext joinableTaskContext,
        LSPRequestInvoker requestInvoker,
        FormattingOptionsProvider formattingOptionsProvider,
        IClientSettingsManager editorSettingsManager,
        LSPDocumentSynchronizer documentSynchronizer,
        CSharpVirtualDocumentAddListener csharpVirtualDocumentAddListener,
        ITelemetryReporter telemetryReporter,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        ProjectSnapshotManager projectManager,
        SnippetCompletionItemProvider snippetCompletionItemProvider,
        IWorkspaceProvider workspaceProvider,
        IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
        ILoggerFactory loggerFactory)
    {
        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (documentManager is not TrackingLSPDocumentManager trackingDocumentManager)
        {
            throw new ArgumentException($"The LSP document manager should be of type {typeof(TrackingLSPDocumentManager).FullName}", nameof(documentManager));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _documentManager = trackingDocumentManager;
        _joinableTaskFactory = joinableTaskContext.Factory;

        _requestInvoker = requestInvoker ?? throw new ArgumentNullException(nameof(requestInvoker));
        _formattingOptionsProvider = formattingOptionsProvider ?? throw new ArgumentNullException(nameof(formattingOptionsProvider));
        _editorSettingsManager = editorSettingsManager ?? throw new ArgumentNullException(nameof(editorSettingsManager));
        _documentSynchronizer = documentSynchronizer ?? throw new ArgumentNullException(nameof(documentSynchronizer));
        _csharpVirtualDocumentAddListener = csharpVirtualDocumentAddListener ?? throw new ArgumentNullException(nameof(csharpVirtualDocumentAddListener));
        _telemetryReporter = telemetryReporter ?? throw new ArgumentNullException(nameof(telemetryReporter));
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        _projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
        _snippetCompletionItemProvider = snippetCompletionItemProvider ?? throw new ArgumentNullException(nameof(snippetCompletionItemProvider));
        _workspaceProvider = workspaceProvider;
        _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
        _logger = loggerFactory.GetOrCreateLogger<RazorCustomMessageTarget>();
    }

    private async Task<DelegationRequestDetails?> GetProjectedRequestDetailsAsync(IDelegatedParams request, CancellationToken cancellationToken)
    {
        string languageServerName;

        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (request.ProjectedKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                request.Identifier.Version,
                request.Identifier.TextDocumentIdentifier,
                cancellationToken,
                rejectOnNewerParallelRequest: false);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else if (request.ProjectedKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                request.Identifier.Version,
                request.Identifier.TextDocumentIdentifier,
                cancellationToken,
                rejectOnNewerParallelRequest: false);
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

        return new DelegationRequestDetails(languageServerName, virtualDocumentSnapshot.Uri, virtualDocumentSnapshot.Snapshot.TextBuffer);
    }

    private record struct DelegationRequestDetails(string LanguageServerName, Uri ProjectedUri, ITextBuffer TextBuffer);

    private async Task<SynchronizedResult<TVirtualDocumentSnapshot>> TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(
       int requiredHostDocumentVersion,
       TextDocumentIdentifier hostDocument,
       CancellationToken cancellationToken,
       bool rejectOnNewerParallelRequest = true,
       [CallerMemberName] string? caller = null)
       where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        if (_languageServerFeatureOptions.UseRazorCohostServer &&
            typeof(TVirtualDocumentSnapshot) == typeof(HtmlVirtualDocumentSnapshot))
        {
            return await TempForCohost_TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(hostDocument, cancellationToken);
        }

        _logger.LogDebug($"Trying to synchronize for {caller} to version {requiredHostDocumentVersion} of {hostDocument.Uri} for {hostDocument.GetProjectContext()?.Id ?? "(no project context)"}");

        // For Html documents we don't do anything fancy, just call the standard service
        // If we're not generating unique document file names, then we can treat C# documents the same way
        if (!_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath ||
            typeof(TVirtualDocumentSnapshot) == typeof(HtmlVirtualDocumentSnapshot))
        {
            var htmlResult = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, cancellationToken).ConfigureAwait(false);
            _logger.LogDebug($"{(htmlResult.Synchronized ? "Did" : "Did NOT")} synchronize for {caller}: Version {requiredHostDocumentVersion} for {htmlResult.VirtualSnapshot?.Uri}");
            return htmlResult;
        }

        var virtualDocument = FindVirtualDocument<TVirtualDocumentSnapshot>(hostDocument.Uri, hostDocument.GetProjectContext());

        if (virtualDocument is { ProjectKey.IsUnknown: true })
        {
            _logger.LogDebug($"Trying to sync to a doc with no project Id. Waiting for document add.");
            if (await _csharpVirtualDocumentAddListener.WaitForDocumentAddAsync(cancellationToken).ConfigureAwait(false))
            {
                _logger.LogDebug($"Wait successful!");
                virtualDocument = FindVirtualDocument<TVirtualDocumentSnapshot>(hostDocument.Uri, hostDocument.GetProjectContext());
            }
            else
            {
                _logger.LogDebug($"Timed out :(");
            }
        }

        if (virtualDocument is null)
        {
            _logger.LogDebug($"No virtual document found, falling back to old code.");
            return await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, cancellationToken).ConfigureAwait(false);
        }

        var result = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, virtualDocument.Uri, rejectOnNewerParallelRequest, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug($"{(result.Synchronized ? "Did" : "Did NOT")} synchronize for {caller}: Version {requiredHostDocumentVersion} for {result.VirtualSnapshot?.Uri}");

        // If we failed to sync on version 1, then it could be that we got new information while waiting, so try again
        if (requiredHostDocumentVersion == 1 && !result.Synchronized)
        {
            _logger.LogDebug($"Sync failed for v1 document. Trying to get virtual document again.");
            virtualDocument = FindVirtualDocument<TVirtualDocumentSnapshot>(hostDocument.Uri, hostDocument.GetProjectContext());

            if (virtualDocument is null)
            {
                _logger.LogDebug($"No virtual document found, falling back to old code.");
                return await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, cancellationToken).ConfigureAwait(false);
            }

            _logger.LogDebug($"Got virtual document after trying again {virtualDocument.Uri}. Trying to synchronize again.");

            // try again
            result = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, virtualDocument.Uri, rejectOnNewerParallelRequest, cancellationToken).ConfigureAwait(false);
            _logger.LogDebug($"{(result.Synchronized ? "Did" : "Did NOT")} synchronize for {caller}: Version {requiredHostDocumentVersion} for {result.VirtualSnapshot?.Uri}");
        }

        return result;
    }

    private async Task<SynchronizedResult<TVirtualDocumentSnapshot>> TempForCohost_TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(TextDocumentIdentifier hostDocument, CancellationToken cancellationToken)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        Debug.Assert(typeof(TVirtualDocumentSnapshot) == typeof(HtmlVirtualDocumentSnapshot));
        // Cohosting is responsible for Html, so we have to go through its service instead
        var workspace = _workspaceProvider.GetWorkspace();
        var documentIds = workspace.CurrentSolution.GetDocumentIdsWithFilePath(RazorUri.GetDocumentFilePathFromUri(hostDocument.Uri));

        if (documentIds.Length != 1)
        {
            _logger.LogError($"Couldn't get document id from the workspace for {hostDocument.Uri}");
            return new SynchronizedResult<TVirtualDocumentSnapshot>(false, null);
        }

        var document = workspace.CurrentSolution.GetAdditionalDocument(documentIds[0]);
        if (document is null)
        {
            _logger.LogError($"Couldn't get document from the workspace for {documentIds[0]} which should be {hostDocument.Uri}");
            return new SynchronizedResult<TVirtualDocumentSnapshot>(false, null);
        }

        if (!await _htmlDocumentSynchronizer.TrySynchronizeAsync(document, cancellationToken).ConfigureAwait(false))
        {
            return new SynchronizedResult<TVirtualDocumentSnapshot>(false, null);
        }

        if (!_documentManager.TryGetDocument(hostDocument.Uri, out var snapshot))
        {
            _logger.LogError($"Couldn't find document in LSPDocumentManager for {hostDocument.Uri}");
            return new SynchronizedResult<TVirtualDocumentSnapshot>(false, null);
        }

        if (!snapshot.TryGetVirtualDocument<TVirtualDocumentSnapshot>(out var virtualDocument))
        {
            _logger.LogError($"Couldn't find virtual document snapshot for {hostDocument.Uri}");
            return new SynchronizedResult<TVirtualDocumentSnapshot>(false, null);
        }

        return new SynchronizedResult<TVirtualDocumentSnapshot>(true, virtualDocument);
    }

    private SynchronizedResult<TVirtualDocumentSnapshot>? TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(
        int requiredHostDocumentVersion, TextDocumentIdentifier hostDocument)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        if (_documentSynchronizer is not DefaultLSPDocumentSynchronizer documentSynchronizer)
        {
            Debug.Fail("Got an LSP document synchronizer I don't know how to handle.");
            throw new InvalidOperationException("Got an LSP document synchronizer I don't know how to handle.");
        }

        // If we're not generating unique document file names, then we don't need to ensure we find the right virtual document
        // as there can only be one anyway
        if (_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath &&
            hostDocument.GetProjectContext() is { } projectContext &&
            FindVirtualDocument<TVirtualDocumentSnapshot>(hostDocument.Uri, projectContext) is { } virtualDocument)
        {
            return documentSynchronizer.TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, virtualDocument.Uri);
        }

        return documentSynchronizer.TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri);
    }

    private CSharpVirtualDocumentSnapshot? FindVirtualDocument<TVirtualDocumentSnapshot>(
        Uri hostDocumentUri, VSProjectContext? projectContext)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        if (!_documentManager.TryGetDocument(hostDocumentUri, out var documentSnapshot) ||
            !documentSnapshot.TryGetAllVirtualDocumentsAsArray<TVirtualDocumentSnapshot>(out var virtualDocuments))
        {
            return null;
        }

        foreach (var virtualDocument in virtualDocuments)
        {
            // NOTE: This is _NOT_ the right snapshot, or at least cannot be assumed to be, we just need the Uri
            // to pass to the synchronizer, so it can get the right snapshot
            if (virtualDocument is not CSharpVirtualDocumentSnapshot csharpVirtualDocument)
            {
                Debug.Fail("FindVirtualDocumentUri should only be called for C# documents, as those are the only ones that have multiple virtual documents");
                return null;
            }

            if (IsMatch(csharpVirtualDocument.ProjectKey, projectContext))
            {
                return csharpVirtualDocument;
            }
        }

        return null;

        static bool IsMatch(ProjectKey projectKey, VSProjectContext? projectContext)
        {
            // If we don't have a project key on our virtual document, then it means we don't know about project info
            // yet, so there would only be one virtual document, so return true.
            // If the request doesn't have project context, then we can't reason about which project we're asked about
            // so return true.
            // In both cases we'll just return the first virtual document we find.
            return projectKey.IsUnknown ||
                projectContext is null ||
                projectKey.Equals(projectContext.ToProjectKey());
        }
    }
}
