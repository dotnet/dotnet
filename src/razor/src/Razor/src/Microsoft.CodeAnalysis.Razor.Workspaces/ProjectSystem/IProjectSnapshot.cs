// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IProjectSnapshot
{
    ProjectKey Key { get; }

    RazorConfiguration Configuration { get; }
    IEnumerable<string> DocumentFilePaths { get; }

    /// <summary>
    /// Gets the full path to the .csproj file for this project
    /// </summary>
    string FilePath { get; }

    /// <summary>
    /// Gets the full path to the folder under 'obj' where the project.razor.bin file will live
    /// </summary>
    string IntermediateOutputPath { get; }

    string? RootNamespace { get; }
    string DisplayName { get; }
    VersionStamp Version { get; }
    LanguageVersion CSharpLanguageVersion { get; }
    ImmutableArray<TagHelperDescriptor> TagHelpers { get; }
    ProjectWorkspaceState ProjectWorkspaceState { get; }

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
