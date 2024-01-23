// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Snippets;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using static Microsoft.VisualStudio.LanguageServer.ContainedLanguage.DefaultLSPDocumentSynchronizer;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(IRazorCustomMessageTarget))]
[Export(typeof(RazorCustomMessageTarget))]
internal partial class RazorCustomMessageTarget : IRazorCustomMessageTarget
{
    private readonly TrackingLSPDocumentManager _documentManager;
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly ITelemetryReporter _telemetryReporter;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly IProjectSnapshotManagerAccessor _projectSnapshotManagerAccessor;
    private readonly SnippetCache _snippetCache;
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
        IProjectSnapshotManagerAccessor projectSnapshotManagerAccessor,
        SnippetCache snippetCache,
        IRazorLoggerFactory loggerFactory)
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
        _projectSnapshotManagerAccessor = projectSnapshotManagerAccessor ?? throw new ArgumentNullException(nameof(projectSnapshotManagerAccessor));
        _snippetCache = snippetCache ?? throw new ArgumentNullException(nameof(snippetCache));
        _logger = loggerFactory.CreateLogger<RazorCustomMessageTarget>();
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

        if (!synchronized)
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
        // For Html documents we don't do anything fancy, just call the standard service
        // If we're not generating unique document file names, then we can treat C# documents the same way
        if (!_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath ||
            typeof(TVirtualDocumentSnapshot) == typeof(HtmlVirtualDocumentSnapshot))
        {
            return await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, cancellationToken).ConfigureAwait(false);
        }

        _logger?.LogDebug("Trying to synchronize for {caller} to version {version} of {doc} for {project}", caller, requiredHostDocumentVersion, hostDocument.Uri, hostDocument.GetProjectContext()?.Id ?? "(no project context)");

        var virtualDocument = FindVirtualDocument<TVirtualDocumentSnapshot>(hostDocument.Uri, hostDocument.GetProjectContext());

        if (virtualDocument is { ProjectKey.Id: null })
        {
            _logger?.LogDebug("Trying to sync to a doc with no project Id. Waiting for document add.");
            if (await _csharpVirtualDocumentAddListener.WaitForDocumentAddAsync(cancellationToken).ConfigureAwait(false))
            {
                _logger?.LogDebug("Wait successful!");
                virtualDocument = FindVirtualDocument<TVirtualDocumentSnapshot>(hostDocument.Uri, hostDocument.GetProjectContext());
            }
            else
            {
                _logger?.LogDebug("Timed out :(");
            }
        }

        if (virtualDocument is null)
        {
            _logger?.LogDebug("No virtual document found, falling back to old code.");
            return await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, cancellationToken).ConfigureAwait(false);
        }

        var result = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, virtualDocument.Uri, rejectOnNewerParallelRequest, cancellationToken).ConfigureAwait(false);
        _logger?.LogDebug("{result} synchronize for {caller}: Version {version} for {document}", result.Synchronized ? "Did" : "Did NOT", caller, requiredHostDocumentVersion, result.VirtualSnapshot.Uri);

        // If we failed to sync on version 1, then it could be that we got new information while waiting, so try again
        if (requiredHostDocumentVersion == 1 && !result.Synchronized)
        {
            _logger?.LogDebug("Sync failed for v1 document. Trying to get virtual document again.");
            virtualDocument = FindVirtualDocument<TVirtualDocumentSnapshot>(hostDocument.Uri, hostDocument.GetProjectContext());

            if (virtualDocument is null)
            {
                _logger?.LogDebug("No virtual document found, falling back to old code.");
                return await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, cancellationToken).ConfigureAwait(false);
            }

            _logger?.LogDebug("Got virtual document after trying again {uri}. Trying to synchronize again.", virtualDocument.Uri);

            // try again
            result = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, virtualDocument.Uri, rejectOnNewerParallelRequest, cancellationToken).ConfigureAwait(false);
            _logger?.LogDebug("{result} synchronize for {caller}: Version {version} for {document}", result.Synchronized ? "Did" : "Did NOT", caller, requiredHostDocumentVersion, result.VirtualSnapshot.Uri);
        }

        return result;
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
            return projectKey.Id is null ||
                projectContext is null ||
                projectKey.Equals(projectContext.ToProjectKey());
        }
    }
}
