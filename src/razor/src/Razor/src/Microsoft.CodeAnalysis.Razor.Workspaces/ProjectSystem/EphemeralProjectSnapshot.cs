// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class EphemeralProjectSnapshot : IProjectSnapshot
{
    private readonly HostWorkspaceServices _services;
    private readonly Lazy<RazorProjectEngine> _projectEngine;

    public EphemeralProjectSnapshot(HostWorkspaceServices services, ProjectKey projectKey)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (projectKey is null)
        {
            throw new ArgumentNullException(nameof(projectKey));
        }

        _services = services;
        Debug.Assert(projectKey.Id.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase), "EphemeralProjectSnapshot should only be used in the legacy editor, where projects are tracked by the full path to the .csproj file");
        FilePath = projectKey.Id;

        _projectEngine = new Lazy<RazorProjectEngine>(CreateProjectEngine);

        Key = projectKey;
    }

    public ProjectKey Key { get; }

    public RazorConfiguration? Configuration => FallbackRazorConfiguration.Latest;

    public IEnumerable<string> DocumentFilePaths => Array.Empty<string>();

    public string FilePath { get; }

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
