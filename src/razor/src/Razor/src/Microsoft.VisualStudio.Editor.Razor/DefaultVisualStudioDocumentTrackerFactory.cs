// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultVisualStudioDocumentTrackerFactory : VisualStudioDocumentTrackerFactory
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly ITextDocumentFactoryService _textDocumentFactory;
    private readonly ProjectPathProvider _projectPathProvider;
    private readonly Workspace _workspace;
    private readonly ImportDocumentManager _importDocumentManager;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly WorkspaceEditorSettings _workspaceEditorSettings;

    public DefaultVisualStudioDocumentTrackerFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext,
        ProjectSnapshotManager projectManager,
        WorkspaceEditorSettings workspaceEditorSettings,
        ProjectPathProvider projectPathProvider,
        ITextDocumentFactoryService textDocumentFactory,
        ImportDocumentManager importDocumentManager,
        Workspace workspace)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        if (workspaceEditorSettings is null)
        {
            throw new ArgumentNullException(nameof(workspaceEditorSettings));
        }

        if (projectPathProvider is null)
        {
            throw new ArgumentNullException(nameof(projectPathProvider));
        }

        if (textDocumentFactory is null)
        {
            throw new ArgumentNullException(nameof(textDocumentFactory));
        }

        if (importDocumentManager is null)
        {
            throw new ArgumentNullException(nameof(importDocumentManager));
        }

        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
        _projectManager = projectManager;
        _workspaceEditorSettings = workspaceEditorSettings;
        _projectPathProvider = projectPathProvider;
        _textDocumentFactory = textDocumentFactory;
        _importDocumentManager = importDocumentManager;
        _workspace = workspace;
    }

    public override VisualStudioDocumentTracker? Create(ITextBuffer textBuffer)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (!_textDocumentFactory.TryGetTextDocument(textBuffer, out var textDocument))
        {
            Debug.Fail("Text document should be available from the text buffer.");
            return null;
        }

        if (!_projectPathProvider.TryGetProjectPath(textBuffer, out var projectPath))
        {
            return null;
        }

        var filePath = textDocument.FilePath;
        var tracker = new DefaultVisualStudioDocumentTracker(
            _projectSnapshotManagerDispatcher, _joinableTaskContext, filePath, projectPath, _projectManager, _workspaceEditorSettings, _workspace, textBuffer, _importDocumentManager);

        return tracker;
    }
}
