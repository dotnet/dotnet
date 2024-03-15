// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal interface IRazorProjectService
{
    void AddDocument(string filePath);
    void OpenDocument(string filePath, SourceText sourceText, int version);
    void UpdateDocument(string filePath, SourceText sourceText, int version);
    void CloseDocument(string filePath);
    void RemoveDocument(string filePath);

    ProjectKey AddProject(
        string filePath,
        string intermediateOutputPath,
        RazorConfiguration? configuration,
        string? rootNamespace,
        string? displayName = null);

    void UpdateProject(
        ProjectKey projectKey,
        RazorConfiguration? configuration,
        string? rootNamespace,
        string displayName,
        ProjectWorkspaceState projectWorkspaceState,
        ImmutableArray<DocumentSnapshotHandle> documents);
}
