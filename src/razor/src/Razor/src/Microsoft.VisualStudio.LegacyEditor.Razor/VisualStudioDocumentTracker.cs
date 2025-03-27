// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;
using Microsoft.CodeAnalysis.Razor.Settings;

using Microsoft.VisualStudio.LegacyEditor.Razor.Settings;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal sealed class VisualStudioDocumentTracker : IVisualStudioDocumentTracker
{
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly string _filePath;
    private readonly string _projectPath;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly IWorkspaceEditorSettings _workspaceEditorSettings;
    private readonly ITextBuffer _textBuffer;
    private readonly IImportDocumentManager _importDocumentManager;
    private readonly List<ITextView> _textViews;
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private bool _isSupportedProject;
    private ILegacyProjectSnapshot? _projectSnapshot;

    private int _subscribeCount;

    public event EventHandler<ContextChangeEventArgs>? ContextChanged;

    public VisualStudioDocumentTracker(
        JoinableTaskContext joinableTaskContext,
        string filePath,
        string projectPath,
        ProjectSnapshotManager projectManager,
        IWorkspaceEditorSettings workspaceEditorSettings,
        IProjectEngineFactoryProvider projectEngineFactoryProvider,
        ITextBuffer textBuffer,
        IImportDocumentManager importDocumentManager)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException(SR.ArgumentCannotBeNullOrEmpty, nameof(filePath));
        }

        _joinableTaskContext = joinableTaskContext;
        _filePath = filePath;
        _projectPath = projectPath;
        _projectManager = projectManager;
        _workspaceEditorSettings = workspaceEditorSettings;
        _textBuffer = textBuffer;
        _importDocumentManager = importDocumentManager;
        _projectEngineFactoryProvider = projectEngineFactoryProvider;

        _textViews = new List<ITextView>();
    }

    public RazorConfiguration? Configuration => _projectSnapshot?.Configuration;

    public ClientSpaceSettings EditorSettings => _workspaceEditorSettings.Current.ClientSpaceSettings;

    public ImmutableArray<TagHelperDescriptor> TagHelpers
        => _projectSnapshot is { TagHelpers: var tagHelpers }
            ? tagHelpers
            : [];

    public bool IsSupportedProject => _isSupportedProject;

    public ILegacyProjectSnapshot? ProjectSnapshot => _projectSnapshot;

    public ITextBuffer TextBuffer => _textBuffer;

    public IReadOnlyList<ITextView> TextViews => _textViews;

    public string FilePath => _filePath;

    public string ProjectPath => _projectPath;

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

    public ITextView? GetFocusedTextView()
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
        if (Interlocked.Increment(ref _subscribeCount) != 1)
        {
            return;
        }

        _projectSnapshot = GetOrCreateProject(_projectPath);
        _isSupportedProject = true;

        _projectManager.Changed += ProjectManager_Changed;
        _workspaceEditorSettings.Changed += EditorSettingsManager_Changed;
        _importDocumentManager.Changed += Import_Changed;

        _importDocumentManager.OnSubscribed(this);

        OnContextChangedAsync(ContextChangeKind.ProjectChanged).Forget();
    }

    private ILegacyProjectSnapshot GetOrCreateProject(string projectFilePath)
    {
        var projectKeys = _projectManager.GetProjectKeysWithFilePath(projectFilePath);

        if (projectKeys is [var projectKey, ..] && _projectManager.TryGetProject(projectKey, out var project))
        {
            return (ILegacyProjectSnapshot)project;
        }

        return new EphemeralProjectSnapshot(_projectEngineFactoryProvider, projectFilePath);
    }

    public void Unsubscribe()
    {
        if (Interlocked.Decrement(ref _subscribeCount) != 0)
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

        OnContextChangedAsync(ContextChangeKind.ProjectChanged).Forget();
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
        if (e.IsSolutionClosing)
        {
            return;
        }

        if (_projectPath is not null &&
            string.Equals(_projectPath, e.ProjectFilePath, StringComparison.OrdinalIgnoreCase))
        {
            switch (e.Kind)
            {
                case ProjectChangeKind.DocumentAdded:
                case ProjectChangeKind.DocumentRemoved:
                case ProjectChangeKind.DocumentChanged:
                    _projectSnapshot = (e.Newer as ILegacyProjectSnapshot).AssumeNotNull();
                    break;

                case ProjectChangeKind.ProjectAdded:
                case ProjectChangeKind.ProjectChanged:
                    var newer = (e.Newer as ILegacyProjectSnapshot).AssumeNotNull();
                    _projectSnapshot = newer;

                    // Just an update
                    OnContextChangedAsync(ContextChangeKind.ProjectChanged).Forget();

                    if (e.Older is not ILegacyProjectSnapshot older ||
                        !older.TagHelpers.SequenceEqual(newer.TagHelpers))
                    {
                        OnContextChangedAsync(ContextChangeKind.TagHelpersChanged).Forget();
                    }

                    break;

                case ProjectChangeKind.ProjectRemoved:
                    // Fall back to ephemeral project
                    _projectSnapshot = GetOrCreateProject(ProjectPath);
                    OnContextChangedAsync(ContextChangeKind.ProjectChanged).Forget();

                    break;

                default:
                    throw new InvalidOperationException($"Unknown ProjectChangeKind {e.Kind}");
            }
        }
    }

    // Internal for testing
    internal void EditorSettingsManager_Changed(object sender, ClientSettingsChangedEventArgs args)
        => OnContextChangedAsync(ContextChangeKind.EditorSettingsChanged).Forget();

    // Internal for testing
    internal void Import_Changed(object sender, ImportChangedEventArgs args)
    {
        foreach (var path in args.AssociatedDocuments)
        {
            if (string.Equals(_filePath, path, StringComparison.OrdinalIgnoreCase))
            {
                OnContextChangedAsync(ContextChangeKind.ImportsChanged).Forget();
                break;
            }
        }
    }
}
