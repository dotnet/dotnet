// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class ProjectSnapshot(ProjectState state) : IProjectSnapshot
{
    private readonly ProjectState _state = state;

    private readonly object _gate = new();
    private readonly Dictionary<string, DocumentSnapshot> _filePathToDocumentMap = new(FilePathNormalizingComparer.Instance);

    public HostProject HostProject => _state.HostProject;

    public ProjectKey Key => _state.HostProject.Key;
    public RazorConfiguration Configuration => _state.HostProject.Configuration;
    public IEnumerable<string> DocumentFilePaths => _state.Documents.Keys;
    public string FilePath => _state.HostProject.FilePath;
    public string IntermediateOutputPath => _state.HostProject.IntermediateOutputPath;
    public string? RootNamespace => _state.HostProject.RootNamespace;
    public string DisplayName => _state.HostProject.DisplayName;
    public VersionStamp Version => _state.Version;
    public ProjectWorkspaceState ProjectWorkspaceState => _state.ProjectWorkspaceState;

    public int DocumentCount => _state.Documents.Count;

    public VersionStamp ConfigurationVersion => _state.ConfigurationVersion;
    public VersionStamp ProjectWorkspaceStateVersion => _state.ProjectWorkspaceStateVersion;
    public VersionStamp DocumentCollectionVersion => _state.DocumentCollectionVersion;

    public RazorProjectEngine GetProjectEngine()
        => _state.ProjectEngine;

    public ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(CancellationToken cancellationToken)
        => new(_state.TagHelpers);

    public bool ContainsDocument(string filePath)
    {
        lock (_gate)
        {
            // PERF: It's intentional that we call _filePathToDocumentMap.ContainsKey(...)
            // before State.Documents.ContainsKey(...), even though the latter check is
            // enough to return the correct answer. This is because _filePathToDocumentMap is
            // a Dictionary<,>, which has O(1) lookup, and State.Documents is an
            // ImmutableDictionary<,>, which has O(log n) lookup. So, checking _filePathToDocumentMap
            // first is faster if the DocumentSnapshot has already been created.

            return _filePathToDocumentMap.ContainsKey(filePath) ||
                   _state.Documents.ContainsKey(filePath);
        }
    }

    public IDocumentSnapshot? GetDocument(string filePath)
        => TryGetDocument(filePath, out var document)
            ? document
            : null;

    public bool TryGetDocument(string filePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        lock (_gate)
        {
            // Have we already seen this document? If so, return it!
            if (_filePathToDocumentMap.TryGetValue(filePath, out var snapshot))
            {
                document = snapshot;
                return true;
            }

            // Do we have DocumentSate for this document? If not, we're done!
            if (!_state.Documents.TryGetValue(filePath, out var state))
            {
                document = null;
                return false;
            }

            // If we have DocumentState, go ahead and create a new DocumentSnapshot.
            snapshot = new DocumentSnapshot(this, state);
            _filePathToDocumentMap.Add(filePath, snapshot);

            document = snapshot;
            return true;
        }
    }

    /// <summary>
    /// If the provided document is an import document, gets the other documents in the project
    /// that include directives specified by the provided document. Otherwise returns an empty
    /// list.
    /// </summary>
    public ImmutableArray<IDocumentSnapshot> GetRelatedDocuments(IDocumentSnapshot document)
    {
        var targetPath = document.TargetPath.AssumeNotNull();

        if (!_state.ImportsToRelatedDocuments.TryGetValue(targetPath, out var relatedDocuments))
        {
            return [];
        }

        lock (_gate)
        {
            using var builder = new PooledArrayBuilder<IDocumentSnapshot>(capacity: relatedDocuments.Length);

            foreach (var relatedDocumentFilePath in relatedDocuments)
            {
                if (TryGetDocument(relatedDocumentFilePath, out var relatedDocument))
                {
                    builder.Add(relatedDocument);
                }
            }

            return builder.DrainToImmutable();
        }
    }
}
