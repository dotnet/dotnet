// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal abstract class RazorProjectService
{
    public abstract void AddDocument(string filePath);

    public abstract void OpenDocument(string filePath, SourceText sourceText, int version);

    public abstract void CloseDocument(string filePath);

    public abstract void RemoveDocument(string filePath);

    public abstract void UpdateDocument(string filePath, SourceText sourceText, int version);

    public abstract ProjectKey AddProject(string filePath, string intermediateOutputPath, RazorConfiguration? configuration, string? rootNamespace);

    public abstract void UpdateProject(
        ProjectKey projectKey,
        RazorConfiguration? configuration,
        string? rootNamespace,
        ProjectWorkspaceState projectWorkspaceState,
        IReadOnlyList<DocumentSnapshotHandle> documents);
}
