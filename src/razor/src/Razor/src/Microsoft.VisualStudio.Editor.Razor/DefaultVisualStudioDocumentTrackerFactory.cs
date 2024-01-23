// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultVisualStudioDocumentTrackerFactory : VisualStudioDocumentTrackerFactory
{
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly ITextDocumentFactoryService _textDocumentFactory;
    private readonly ProjectPathProvider _projectPathProvider;
    private readonly ImportDocumentManager _importDocumentManager;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly WorkspaceEditorSettings _workspaceEditorSettings;
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;

    public DefaultVisualStudioDocumentTrackerFactory(
        ProjectSnapshotManagerDispatcher dispatcher,
        JoinableTaskContext joinableTaskContext,
        ProjectSnapshotManager projectManager,
        WorkspaceEditorSettings workspaceEditorSettings,
        ProjectPathProvider projectPathProvider,
        ITextDocumentFactoryService textDocumentFactory,
        ImportDocumentManager importDocumentManager,
        IProjectEngineFactoryProvider projectEngineFactoryProvider)
    {
        _dispatcher = dispatcher;
        _joinableTaskContext = joinableTaskContext;
        _projectManager = projectManager;
        _workspaceEditorSettings = workspaceEditorSettings;
        _projectPathProvider = projectPathProvider;
        _textDocumentFactory = textDocumentFactory;
        _importDocumentManager = importDocumentManager;
        _projectEngineFactoryProvider = projectEngineFactoryProvider;
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
            _dispatcher, _joinableTaskContext, filePath, projectPath, _projectManager, _workspaceEditorSettings, _projectEngineFactoryProvider, textBuffer, _importDocumentManager);

        return tracker;
    }
}
