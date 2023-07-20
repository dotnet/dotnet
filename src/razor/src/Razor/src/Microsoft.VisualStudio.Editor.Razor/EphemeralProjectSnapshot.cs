// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class EphemeralProjectSnapshot : IProjectSnapshot
{
    private readonly HostWorkspaceServices _services;
    private readonly Lazy<RazorProjectEngine> _projectEngine;

    public EphemeralProjectSnapshot(HostWorkspaceServices services, string projectPath)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (projectPath is null)
        {
            throw new ArgumentNullException(nameof(projectPath));
        }

        _services = services;
        FilePath = projectPath;
        IntermediateOutputPath = Path.Combine(Path.GetDirectoryName(FilePath) ?? FilePath, "obj");

        _projectEngine = new Lazy<RazorProjectEngine>(CreateProjectEngine);

        Key = ProjectKey.From(this);
    }

    public ProjectKey Key { get; }

    public RazorConfiguration? Configuration => FallbackRazorConfiguration.Latest;

    public IEnumerable<string> DocumentFilePaths => Array.Empty<string>();

    public string FilePath { get; }

    public string IntermediateOutputPath { get; }

    public string? RootNamespace { get; }

    public VersionStamp Version => VersionStamp.Default;

    public LanguageVersion CSharpLanguageVersion => LanguageVersion.Default;

    public ImmutableArray<TagHelperDescriptor> TagHelpers => ImmutableArray<TagHelperDescriptor>.Empty;

    public ProjectWorkspaceState? ProjectWorkspaceState => null;

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
        var factory = _services.GetRequiredService<ProjectSnapshotProjectEngineFactory>();
        return factory.Create(this);
    }
}
