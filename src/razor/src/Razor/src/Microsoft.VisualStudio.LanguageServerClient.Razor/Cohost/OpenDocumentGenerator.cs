// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Shared]
[Export(typeof(OpenDocumentGenerator))]
[Export(typeof(IProjectSnapshotChangeTrigger))]
[method: ImportingConstructor]
internal sealed class OpenDocumentGenerator(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    LSPDocumentManager documentManager,
    CSharpVirtualDocumentAddListener csharpVirtualDocumentAddListener,
    ISnapshotResolver snapshotResolver,
    JoinableTaskContext joinableTaskContext,
    IRazorLoggerFactory loggerFactory) : IProjectSnapshotChangeTrigger
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
    private readonly TrackingLSPDocumentManager _documentManager = documentManager as TrackingLSPDocumentManager ?? throw new ArgumentNullException(nameof(documentManager));
    private readonly CSharpVirtualDocumentAddListener _csharpVirtualDocumentAddListener = csharpVirtualDocumentAddListener ?? throw new ArgumentNullException(nameof(csharpVirtualDocumentAddListener));
    private readonly ISnapshotResolver _snapshotResolver = snapshotResolver ?? throw new ArgumentNullException(nameof(snapshotResolver));
    private readonly JoinableTaskFactory _joinableTaskFactory = joinableTaskContext.Factory;
    private readonly ILogger _logger = loggerFactory.CreateLogger<OpenDocumentGenerator>();

    private ProjectSnapshotManager? _projectManager;

    public void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        if (!_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return;
        }

        _projectManager = projectManager;
        _projectManager.Changed += ProjectManager_Changed;
    }

    public async Task DocumentOpenedOrChangedAsync(string textDocumentPath, int version, CancellationToken cancellationToken)
    {
        // We are purposefully trigger code generation here directly, rather than using the project manager events that the above call
        // would have triggered, because Cohosting is intended to eventually remove the project manager and its events. We also want
        // to eventually remove this code too, and just rely on the source generator, but by keeping the concepts separate we are not
        // tying the code to any particular order of feature removal.
        if (!_snapshotResolver.TryResolveAllProjects(textDocumentPath, out var projectSnapshots))
        {
            projectSnapshots = [_snapshotResolver.GetMiscellaneousProject()];
        }

        foreach (var project in projectSnapshots)
        {
            var document = project.GetDocument(textDocumentPath);
            if (document is not null)
            {
                await UpdateGeneratedDocumentsAsync(document, version, cancellationToken);
            }
        }
    }

    private async Task UpdateGeneratedDocumentsAsync(IDocumentSnapshot document, int version, CancellationToken cancellationToken)
    {
        // These flags exist to workaround things in VS Code, so bringing cohosting to VS Code without also fixing these flags, is very silly.
        Debug.Assert(_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath);
        Debug.Assert(!_languageServerFeatureOptions.UpdateBuffersForClosedDocuments);

        // Actually do the generation
        var generatedOutput = await document.GetGeneratedOutputAsync().ConfigureAwait(false);

        // Now we have to update the LSP buffer etc.
        // Fortunate this code will be removed in time
        var hostDocumentUri = new Uri(document.FilePath);

        _logger.LogDebug("[Cohost] Updating generated document buffers for {version} of {uri} in {projectKey}", version, hostDocumentUri, document.Project.Key);

        if (_documentManager.TryGetDocument(hostDocumentUri, out var documentSnapshot))
        {
            // Html
            var htmlVirtualDocumentSnapshot = TryGetHtmlSnapshot(documentSnapshot);

            // Buffer work has to be on the UI thread, and getting the C# buffers might result in a change to which buffers exist
            await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // CSharp
            var csharpVirtualDocumentSnapshot = await TryGetCSharpSnapshotAsync(documentSnapshot, document.Project.Key, version, cancellationToken).ConfigureAwait(true);

            Debug.Assert(htmlVirtualDocumentSnapshot is not null && csharpVirtualDocumentSnapshot is not null ||
                htmlVirtualDocumentSnapshot is null && csharpVirtualDocumentSnapshot is null, "Found a Html XOR a C# document. Expected both or neither.");

            if (htmlVirtualDocumentSnapshot is not null)
            {
                _logger.LogDebug("Updating to version {version}: {virtualDocument}", version, htmlVirtualDocumentSnapshot.Uri);
                _documentManager.UpdateVirtualDocument<HtmlVirtualDocument>(
                    hostDocumentUri,
                    [new VisualStudioTextChange(0, htmlVirtualDocumentSnapshot.Snapshot.Length, generatedOutput.GetHtmlSourceText().ToString())],
                    version,
                    state: null);
            }

            if (csharpVirtualDocumentSnapshot is not null)
            {
                _logger.LogDebug("Updating to version {version}: {virtualDocument}", version, csharpVirtualDocumentSnapshot.Uri);
                _documentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
                    hostDocumentUri,
                    csharpVirtualDocumentSnapshot.Uri,
                    [new VisualStudioTextChange(0, csharpVirtualDocumentSnapshot.Snapshot.Length, generatedOutput.GetCSharpSourceText().ToString())],
                    version,
                    state: null);
                return;
            }
        }
    }

    private async Task<CSharpVirtualDocumentSnapshot?> TryGetCSharpSnapshotAsync(LSPDocumentSnapshot documentSnapshot, ProjectKey projectKey, int version, CancellationToken cancellationToken)
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        if (documentSnapshot.TryGetAllVirtualDocuments<CSharpVirtualDocumentSnapshot>(out var virtualDocuments))
        {
            if (virtualDocuments is [{ ProjectKey.Id: null }])
            {
                // If there is only a single virtual document, and its got a null id, then that means it's in our "misc files" project.
                // That means its probably new, as Visual Studio opens a buffer for a document before we get the notifications about it
                // being added to any projects. Lets try refreshing before we worry.
                _logger.LogDebug("[Cohost] Refreshing virtual documents, and waiting for them, (for {hostDocumentUri})", documentSnapshot.Uri);

                var task = _csharpVirtualDocumentAddListener.WaitForDocumentAddAsync(cancellationToken);
                _documentManager.RefreshVirtualDocuments();
                _ = await task.ConfigureAwait(true);

                // Since we're dealing with snapshots, we have to get the new ones after refreshing
                if (!_documentManager.TryGetDocument(documentSnapshot.Uri, out var newDocumentSnapshot) ||
                    !newDocumentSnapshot.TryGetAllVirtualDocuments<CSharpVirtualDocumentSnapshot>(out virtualDocuments))
                {
                    // This should never happen.
                    // The server clearly wants to tell us about a document in a project, but we don't know which project it's in.
                    // Sadly there isn't anything we can do here to, we're just in a state where the server and client are out of
                    // sync with their understanding of the document contents, and since changes come in as a list of changes,
                    // the user experience is broken. All we can do is hope the user closes and re-opens the document.
                    _logger.LogError("[Cohost] Server wants to update {hostDocumentUri} in {projectKeyId} but we only know about that document in misc files. Server and client are now out of sync.", documentSnapshot.Uri, projectKey);
                    Debug.Fail($"Server wants to update {documentSnapshot.Uri} in {projectKey} but we don't know about the document being in any projects");
                    return null;
                }
            }

            foreach (var virtualDocument in virtualDocuments)
            {
                if (virtualDocument.ProjectKey.Equals(projectKey))
                {
                    _logger.LogDebug("[Cohost] Found C# virtual doc for {version}: {uri}", version, virtualDocument.Uri);

                    return virtualDocument;
                }
            }

            _logger.LogError("[Cohost] Couldn't find any virtual docs for {version} of {uri} in {projectKey}", version, documentSnapshot.Uri, projectKey);
            Debug.Fail($"Couldn't find any virtual docs for {version} of {documentSnapshot.Uri} in {projectKey}");
        }

        return null;
    }

    private static HtmlVirtualDocumentSnapshot? TryGetHtmlSnapshot(LSPDocumentSnapshot documentSnapshot)
    {
        _ = documentSnapshot.TryGetVirtualDocument<HtmlVirtualDocumentSnapshot>(out var htmlVirtualDocumentSnapshot);
        return htmlVirtualDocumentSnapshot;
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        // We only respond to ProjectChanged events, as opens and changes are handled by LSP endpoints, which call into this class too
        if (e.Kind == ProjectChangeKind.ProjectChanged)
        {
            var newProject = e.Newer.AssumeNotNull();

            foreach (var documentFilePath in newProject.DocumentFilePaths)
            {
                if (_projectManager!.IsDocumentOpen(documentFilePath) &&
                    newProject.GetDocument(documentFilePath) is { } document &&
                    _documentManager.TryGetDocument(new Uri(document.FilePath), out var documentSnapshot))
                {
                    // This is not ideal, but we need to re-use the existing snapshot version because our system uses the version
                    // of the text buffer, but a project change doesn't change the text buffer.
                    // See https://github.com/dotnet/razor/issues/9197 for more info and some issues this causes
                    // This should all be moot eventually in Cohosting eventually anyway (ie, this whole file should be deleted)
                    _ = UpdateGeneratedDocumentsAsync(document, documentSnapshot.Version, CancellationToken.None);
                }
            }
        }
    }
}
