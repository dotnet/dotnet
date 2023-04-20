// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IProjectSnapshot
{
    RazorConfiguration? Configuration { get; }
    IEnumerable<string> DocumentFilePaths { get; }
    string FilePath { get; }
    string? RootNamespace { get; }
    VersionStamp Version { get; }
    LanguageVersion CSharpLanguageVersion { get; }
    IReadOnlyList<TagHelperDescriptor> TagHelpers { get; }
    ProjectWorkspaceState? ProjectWorkspaceState { get; }

    RazorProjectEngine GetProjectEngine();
    IDocumentSnapshot? GetDocument(string filePath);
    bool IsImportDocument(IDocumentSnapshot document);

    /// <summary>
    /// If the provided document is an import document, gets the other documents in the project
    /// that include directives specified by the provided document. Otherwise returns an empty
    /// list.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <returns>A list of related documents.</returns>
    ImmutableArray<IDocumentSnapshot> GetRelatedDocuments(IDocumentSnapshot document);
}
