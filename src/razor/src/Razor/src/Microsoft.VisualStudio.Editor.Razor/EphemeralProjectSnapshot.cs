// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class EphemeralProjectSnapshot : IProjectSnapshot
{
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private readonly Lazy<RazorProjectEngine> _projectEngine;

    public EphemeralProjectSnapshot(IProjectEngineFactoryProvider projectEngineFactoryProvider, string projectPath)
    {
        _projectEngineFactoryProvider = projectEngineFactoryProvider;
        FilePath = projectPath;
        IntermediateOutputPath = Path.Combine(Path.GetDirectoryName(FilePath) ?? FilePath, "obj");
        DisplayName = Path.GetFileNameWithoutExtension(projectPath);

        _projectEngine = new Lazy<RazorProjectEngine>(CreateProjectEngine);

        Key = ProjectKey.From(this);
    }

    public ProjectKey Key { get; }

    public RazorConfiguration Configuration => FallbackRazorConfiguration.Latest;

    public IEnumerable<string> DocumentFilePaths => Array.Empty<string>();

    public string FilePath { get; }

    public string IntermediateOutputPath { get; }

    public string? RootNamespace { get; }

    public string DisplayName { get; }

    public VersionStamp Version => VersionStamp.Default;

    public LanguageVersion CSharpLanguageVersion => ProjectWorkspaceState.CSharpLanguageVersion;

    public ImmutableArray<TagHelperDescriptor> TagHelpers => ProjectWorkspaceState.TagHelpers;

    public ProjectWorkspaceState ProjectWorkspaceState => ProjectWorkspaceState.Default;

    public IDocumentSnapshot? GetDocument(string filePath)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        return null;
    }

    public bool IsImportDocument(IDocumentSnapshot document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return false;
    }

    public ImmutableArray<IDocumentSnapshot> GetRelatedDocuments(IDocumentSnapshot document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return ImmutableArray<IDocumentSnapshot>.Empty;
    }

    public RazorProjectEngine GetProjectEngine()
    {
        return _projectEngine.Value;
    }

    private RazorProjectEngine CreateProjectEngine()
    {
        return _projectEngineFactoryProvider.Create(this);
    }
}
