// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal partial class ProjectSnapshotManager
{
    public readonly struct Updater(ProjectSnapshotManager instance)
    {
        public ImmutableArray<ProjectKey> GetProjectKeysWithFilePath(string filePath)
            => instance.GetProjectKeysWithFilePath(filePath);

        public ImmutableArray<ProjectSnapshot> GetProjects()
            => instance.GetProjects();

        public bool ContainsProject(ProjectKey projectKey)
            => instance.ContainsProject(projectKey);

        public bool TryGetProject(ProjectKey projectKey, [NotNullWhen(true)] out ProjectSnapshot? project)
            => instance.TryGetProject(projectKey, out project);

        public bool IsDocumentOpen(string documentFilePath)
            => instance.IsDocumentOpen(documentFilePath);

        public ImmutableArray<string> GetOpenDocuments()
            => instance.GetOpenDocuments();

        public void AddDocument(ProjectKey projectKey, HostDocument document, TextLoader textLoader)
            => instance.AddDocument(projectKey, document, textLoader);

        public void AddDocument(ProjectKey projectKey, HostDocument document, SourceText sourceText)
            => instance.AddDocument(projectKey, document, sourceText);

        public void RemoveDocument(ProjectKey projectKey, string documentFilePath)
            => instance.RemoveDocument(projectKey, documentFilePath);

        public void UpdateDocumentText(ProjectKey projectKey, string documentFilePath, TextLoader textLoader)
            => instance.UpdateDocumentText(projectKey, documentFilePath, textLoader);

        public void UpdateDocumentText(ProjectKey projectKey, string documentFilePath, SourceText sourceText)
            => instance.UpdateDocumentText(projectKey, documentFilePath, sourceText);

        public void OpenDocument(ProjectKey projectKey, string documentFilePath, SourceText sourceText)
            => instance.OpenDocument(projectKey, documentFilePath, sourceText);

        public void CloseDocument(ProjectKey projectKey, string documentFilePath, TextLoader textLoader)
            => instance.CloseDocument(projectKey, documentFilePath, textLoader);

        public void AddProject(HostProject project)
            => instance.AddProject(project);

        public void RemoveProject(ProjectKey projectKey)
            => instance.RemoveProject(projectKey);

        public void UpdateProjectConfiguration(HostProject project)
            => instance.UpdateProjectConfiguration(project);

        public void UpdateProjectWorkspaceState(ProjectKey projectKey, ProjectWorkspaceState projectWorkspaceState)
            => instance.UpdateProjectWorkspaceState(projectKey, projectWorkspaceState);

        public void SolutionOpened()
            => instance.SolutionOpened();

        public void SolutionClosed()
            => instance.SolutionClosed();
    }
}
