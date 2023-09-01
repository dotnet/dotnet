// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class ProjectSnapshot : IProjectSnapshot
{
    private readonly object _lock;

    private readonly Dictionary<string, DocumentSnapshot> _documents;

    public ProjectSnapshot(ProjectState state)
    {
        State = state ?? throw new ArgumentNullException(nameof(state));

        _lock = new object();
        _documents = new Dictionary<string, DocumentSnapshot>(FilePathComparer.Instance);
    }

    public ProjectKey Key => State.HostProject.Key;

    public ProjectState State { get; }

    public RazorConfiguration? Configuration => HostProject.Configuration;

    public IEnumerable<string> DocumentFilePaths => State.Documents.Keys;

    public string FilePath => State.HostProject.FilePath;

    public string IntermediateOutputPath => State.HostProject.IntermediateOutputPath;

    public string? RootNamespace => State.HostProject.RootNamespace;

    public LanguageVersion CSharpLanguageVersion => State.CSharpLanguageVersion;

    public HostProject HostProject => State.HostProject;

    public virtual VersionStamp Version => State.Version;

    public ImmutableArray<TagHelperDescriptor> TagHelpers => State.TagHelpers;

    public ProjectWorkspaceState? ProjectWorkspaceState => State.ProjectWorkspaceState;

    public virtual IDocumentSnapshot? GetDocument(string filePath)
    {
        lock (_lock)
        {
            if (!_documents.TryGetValue(filePath, out var result) &&
                State.Documents.TryGetValue(filePath, out var state))
            {
                result = new DocumentSnapshot(this, state);
                _documents.Add(filePath, result);
            }

            return result;
        }
    }

    public bool IsImportDocument(IDocumentSnapshot document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return document.TargetPath is { } targetPath && State.ImportsToRelatedDocuments.ContainsKey(targetPath);
    }

    public ImmutableArray<IDocumentSnapshot> GetRelatedDocuments(IDocumentSnapshot document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var targetPath = document.TargetPath.AssumeNotNull();

        if (!State.ImportsToRelatedDocuments.TryGetValue(targetPath, out var relatedDocuments))
        {
            return ImmutableArray<IDocumentSnapshot>.Empty;
        }

        lock (_lock)
        {
            using var _ = ArrayBuilderPool<IDocumentSnapshot>.GetPooledObject(out var builder);

            foreach (var relatedDocumentFilePath in relatedDocuments)
            {
                if (GetDocument(relatedDocumentFilePath) is { } relatedDocument)
                {
                    builder.Add(relatedDocument);
                }
            }

            return builder.ToImmutableArray();
        }
    }

    public virtual RazorProjectEngine GetProjectEngine()
    {
        return State.ProjectEngine;
    }
}
