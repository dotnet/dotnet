// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor.Documents;

[Export(typeof(RazorCodeDocumentProvidingSnapshotChangeTrigger))]
[Export(typeof(IRazorStartupService))]
internal class RazorCodeDocumentProvidingSnapshotChangeTrigger : IRazorStartupService
{
    private readonly HashSet<string> _openDocuments = new(FilePathComparer.Instance);
    private readonly Dictionary<string, ProjectKey> _documentProjectMap = new(FilePathComparer.Instance);
    private readonly IProjectSnapshotManager _projectManager;

    public event EventHandler<string>? DocumentReady;

    [ImportingConstructor]
    public RazorCodeDocumentProvidingSnapshotChangeTrigger(IProjectSnapshotManager projectManager)
    {
        _projectManager = projectManager;
        _projectManager.Changed += ProjectManager_Changed;
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        if (e.Kind == ProjectChangeKind.ProjectAdded)
        {
            var projectDocuments = e.Newer!.DocumentFilePaths.ToArray();
            foreach (var doc in _openDocuments)
            {
                if (projectDocuments.Contains(doc))
                {
                    _openDocuments.Remove(doc);
                    DocumentReady?.Invoke(this, doc);
                }
            }
        }
        else if (e.Kind == ProjectChangeKind.DocumentAdded)
        {
            var documentFilePath = e.DocumentFilePath!;
            _documentProjectMap[documentFilePath] = e.ProjectKey;
            if (_openDocuments.Contains(documentFilePath))
            {
                _openDocuments.Remove(documentFilePath);
                DocumentReady?.Invoke(this, documentFilePath);
            }
        }
        else if (e.Kind == ProjectChangeKind.DocumentRemoved)
        {
            _documentProjectMap.Remove(e.DocumentFilePath!);
        }
    }

    public async Task<RazorCodeDocument?> GetRazorCodeDocumentAsync(string filePath, CancellationToken cancellationToken)
    {
        if (!_documentProjectMap.TryGetValue(filePath, out var projectKey))
        {
            _openDocuments.Add(filePath);
            return null;
        }

        if (!_projectManager.TryGetProject(projectKey, out var project))
        {
            return null;
        }

        if (!project.TryGetDocument(filePath, out var document))
        {
            return null;
        }

        return await document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
    }
}
