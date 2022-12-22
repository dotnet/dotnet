// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultVisualStudioDocumentTracker : VisualStudioDocumentTracker
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly string _filePath;
    private readonly string _projectPath;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly WorkspaceEditorSettings _workspaceEditorSettings;
    private readonly ITextBuffer _textBuffer;
    private readonly ImportDocumentManager _importDocumentManager;
    private readonly List<ITextView> _textViews;
    private readonly Workspace _workspace;
    private bool _isSupportedProject;
    private ProjectSnapshot? _projectSnapshot;
    private int _subscribeCount;

    public override event EventHandler<ContextChangeEventArgs>? ContextChanged;

    public DefaultVisualStudioDocumentTracker(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext,
        string filePath,
        string projectPath,
        ProjectSnapshotManager projectManager,
        WorkspaceEditorSettings workspaceEditorSettings,
        Workspace workspace,
        ITextBuffer textBuffer,
        ImportDocumentManager importDocumentManager)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException(SR.ArgumentCannotBeNullOrEmpty, nameof(filePath));
        }

        if (projectPath is null)
        {
            throw new ArgumentNullException(nameof(projectPath));
        }

        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        if (workspaceEditorSettings is null)
        {
            throw new ArgumentNullException(nameof(workspaceEditorSettings));
        }

        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (importDocumentManager is null)
        {
            throw new ArgumentNullException(nameof(importDocumentManager));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
        _filePath = filePath;
        _projectPath = projectPath;
        _projectManager = projectManager;
        _workspaceEditorSettings = workspaceEditorSettings;
        _textBuffer = textBuffer;
        _importDocumentManager = importDocumentManager;
        _workspace = workspace; // For now we assume that the workspace is the always default VS workspace.

        _textViews = new List<ITextView>();
    }

    public override RazorConfiguration? Configuration => _projectSnapshot?.Configuration;

    public override EditorSettings EditorSettings => _workspaceEditorSettings.Current;

    public override IReadOnlyList<TagHelperDescriptor>? TagHelpers => ProjectSnapshot?.TagHelpers;

    public override bool IsSupportedProject => _isSupportedProject;

    internal override ProjectSnapshot? ProjectSnapshot => _projectSnapshot;

    public override ITextBuffer TextBuffer => _textBuffer;

    public override IReadOnlyList<ITextView> TextViews => _textViews;

    public override Workspace Workspace => _workspace;

    public override string FilePath => _filePath;

    public override string ProjectPath => _projectPath;

    internal void AddTextView(ITextView textView)
    {
        if (textView is null)
        {
            throw new ArgumentNullException(nameof(textView));
        }

        _joinableTaskContext.AssertUIThread();

        if (!_textViews.Contains(textView))
        {
            _textViews.Add(textView);
        }
    }

    internal void RemoveTextView(ITextView textView)
    {
        if (textView is null)
        {
            throw new ArgumentNullException(nameof(textView));
        }

        _joinableTaskContext.AssertUIThread();

        if (_textViews.Contains(textView))
        {
            _textViews.Remove(textView);
        }
    }

    public override ITextView? GetFocusedTextView()
    {
        _joinableTaskContext.AssertUIThread();

        for (var i = 0; i < TextViews.Count; i++)
        {
            if (TextViews[i].HasAggregateFocus)
            {
                return TextViews[i];
            }
        }

        return null;
    }

    public void Subscribe()
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_subscribeCount++ > 0)
        {
            return;
        }

        _projectSnapshot = _projectManager.GetOrCreateProject(_projectPath);
        _isSupportedProject = true;

        _projectManager.Changed += ProjectManager_Changed;
        _workspaceEditorSettings.Changed += EditorSettingsManager_Changed;
        _importDocumentManager.Changed += Import_Changed;

        _importDocumentManager.OnSubscribed(this);

        _ = OnContextChangedAsync(ContextChangeKind.ProjectChanged);
    }

    public void Unsubscribe()
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_subscribeCount == 0 || _subscribeCount-- > 1)
        {
            return;
        }

        _importDocumentManager.OnUnsubscribed(this);

        _projectManager.Changed -= ProjectManager_Changed;
        _workspaceEditorSettings.Changed -= EditorSettingsManager_Changed;
        _importDocumentManager.Changed -= Import_Changed;

        // Detached from project.
        _isSupportedProject = false;
        _projectSnapshot = null;

        _ = OnContextChangedAsync(kind: ContextChangeKind.ProjectChanged);
    }

    private async Task OnContextChangedAsync(ContextChangeKind kind)
    {
        await _joinableTaskContext.Factory.SwitchToMainThreadAsync();
        ContextChanged?.Invoke(this, new ContextChangeEventArgs(kind));
    }

    // Internal for testing
    internal void ProjectManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        // Don't do any work if the solution is closing
        if (e.SolutionIsClosing)
        {
            return;
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_projectPath is not null &&
            string.Equals(_projectPath, e.ProjectFilePath, StringComparison.OrdinalIgnoreCase))
        {
            // This will be the new snapshot unless the project was removed.
            _projectSnapshot = _projectManager.GetLoadedProject(e.ProjectFilePath);

            switch (e.Kind)
            {
                case ProjectChangeKind.DocumentAdded:
                case ProjectChangeKind.DocumentRemoved:
                case ProjectChangeKind.DocumentChanged:

                    // Nothing to do.
                    break;

                case ProjectChangeKind.ProjectAdded:
                case ProjectChangeKind.ProjectChanged:

                    // Just an update
                    _ = OnContextChangedAsync(ContextChangeKind.ProjectChanged);

                    if (e.Older is null ||
                        !Enumerable.SequenceEqual(e.Older.TagHelpers, e.Newer!.TagHelpers))
                    {
                        _ = OnContextChangedAsync(ContextChangeKind.TagHelpersChanged);
                    }

                    break;

                case ProjectChangeKind.ProjectRemoved:

                    // Fall back to ephemeral project
                    _projectSnapshot = _projectManager.GetOrCreateProject(ProjectPath);
                    _ = OnContextChangedAsync(ContextChangeKind.ProjectChanged);
                    break;

                default:
                    throw new InvalidOperationException($"Unknown ProjectChangeKind {e.Kind}");
            }
        }
    }

    // Internal for testing
    internal void EditorSettingsManager_Changed(object sender, EditorSettingsChangedEventArgs args)
        => _ = OnContextChangedAsync(ContextChangeKind.EditorSettingsChanged);

    // Internal for testing
    internal void Import_Changed(object sender, ImportChangedEventArgs args)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        foreach (var path in args.AssociatedDocuments)
        {
            if (string.Equals(_filePath, path, StringComparison.OrdinalIgnoreCase))
            {
                _ = OnContextChangedAsync(ContextChangeKind.ImportsChanged);
                break;
            }
        }
    }
}
