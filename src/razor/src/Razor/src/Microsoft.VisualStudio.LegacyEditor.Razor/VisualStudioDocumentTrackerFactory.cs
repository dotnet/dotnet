// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LegacyEditor.Razor.Settings;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

[Export(typeof(IVisualStudioDocumentTrackerFactory))]
[method: ImportingConstructor]
internal sealed class VisualStudioDocumentTrackerFactory(
    ProjectSnapshotManagerDispatcher dispatcher,
    JoinableTaskContext joinableTaskContext,
    IProjectSnapshotManager projectManager,
    IWorkspaceEditorSettings workspaceEditorSettings,
    IProjectPathProvider projectPathProvider,
    ITextDocumentFactoryService textDocumentFactory,
    IImportDocumentManager importDocumentManager,
    IProjectEngineFactoryProvider projectEngineFactoryProvider) : IVisualStudioDocumentTrackerFactory
{
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;
    private readonly JoinableTaskContext _joinableTaskContext = joinableTaskContext;
    private readonly ITextDocumentFactoryService _textDocumentFactory = textDocumentFactory;
    private readonly IProjectPathProvider _projectPathProvider = projectPathProvider;
    private readonly IImportDocumentManager _importDocumentManager = importDocumentManager;
    private readonly IProjectSnapshotManager _projectManager = projectManager;
    private readonly IWorkspaceEditorSettings _workspaceEditorSettings = workspaceEditorSettings;
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider = projectEngineFactoryProvider;

    public IVisualStudioDocumentTracker? Create(ITextBuffer textBuffer)
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
        var tracker = new VisualStudioDocumentTracker(
            _dispatcher, _joinableTaskContext, filePath, projectPath, _projectManager, _workspaceEditorSettings, _projectEngineFactoryProvider, textBuffer, _importDocumentManager);

        return tracker;
    }
}
