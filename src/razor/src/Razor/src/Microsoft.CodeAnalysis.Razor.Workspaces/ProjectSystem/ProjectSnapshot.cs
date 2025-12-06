// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class ProjectSnapshot(ProjectState state) : IProjectSnapshot, ILegacyProjectSnapshot
{
    private readonly ProjectState _state = state;

    private readonly object _gate = new();
    private Dictionary<string, DocumentSnapshot>? _filePathToDocumentMap;

    public HostProject HostProject => _state.HostProject;

    public ProjectKey Key => _state.HostProject.Key;
    public RazorConfiguration Configuration => _state.HostProject.Configuration;
    public IEnumerable<string> DocumentFilePaths => _state.Documents.Keys;
    public string FilePath => _state.HostProject.FilePath;
    public string IntermediateOutputPath => _state.HostProject.IntermediateOutputPath;
    public string? RootNamespace => _state.HostProject.RootNamespace;
    public string DisplayName => _state.HostProject.DisplayName;
    public LanguageVersion CSharpLanguageVersion => _state.CSharpLanguageVersion;
    public ProjectWorkspaceState ProjectWorkspaceState => _state.ProjectWorkspaceState;

    public int DocumentCount => _state.Documents.Count;

    public RazorProjectEngine ProjectEngine => _state.ProjectEngine;

    public ValueTask<TagHelperCollection> GetTagHelpersAsync(CancellationToken cancellationToken)
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

            if (_filePathToDocumentMap is not null && _filePathToDocumentMap.ContainsKey(filePath))
            {
                return true;
            }

            return _state.Documents.ContainsKey(filePath);
        }
    }

    public bool TryGetDocument(string filePath, [NotNullWhen(true)] out DocumentSnapshot? document)
    {
        lock (_gate)
        {
            // Have we already seen this document? If so, return it!
            if (_filePathToDocumentMap is not null &&
                _filePathToDocumentMap.TryGetValue(filePath, out var snapshot))
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

            _filePathToDocumentMap ??= new(capacity: _state.Documents.Count, FilePathNormalizingComparer.Instance);
            _filePathToDocumentMap.Add(filePath, snapshot);

            document = snapshot;
            return true;
        }
    }

    bool IProjectSnapshot.TryGetDocument(string filePath, [NotNullWhen(true)] out IDocumentSnapshot? document)
    {
        if (TryGetDocument(filePath, out var result))
        {
            document = result;
            return true;
        }

        document = null;
        return false;
    }

    /// <summary>
    /// If the provided document file path references an import document, gets the other
    /// documents in the project that include directives specified by the provided document.
    /// Otherwise returns an empty array.
    /// </summary>
    public ImmutableArray<string> GetRelatedDocumentFilePaths(string documentFilePath)
    {
        if (!_state.Documents.TryGetValue(documentFilePath, out var documentState))
        {
            return [];
        }

        var targetPath = documentState.HostDocument.TargetPath;

        if (!_state.ImportsToRelatedDocuments.TryGetValue(targetPath, out var relatedDocuments))
        {
            return [];
        }

        lock (_gate)
        {
            using var builder = new PooledArrayBuilder<string>(capacity: relatedDocuments.Count);

            foreach (var relatedDocumentFilePath in relatedDocuments)
            {
                if (ContainsDocument(relatedDocumentFilePath))
                {
                    builder.Add(relatedDocumentFilePath);
                }
            }

            return builder.ToImmutableAndClear();
        }
    }

    #region ILegacyProjectSnapshot support

    RazorConfiguration ILegacyProjectSnapshot.Configuration => Configuration;
    string ILegacyProjectSnapshot.FilePath => FilePath;
    string? ILegacyProjectSnapshot.RootNamespace => RootNamespace;
    LanguageVersion ILegacyProjectSnapshot.CSharpLanguageVersion => CSharpLanguageVersion;
    TagHelperCollection ILegacyProjectSnapshot.TagHelpers => ProjectWorkspaceState.TagHelpers;

    RazorProjectEngine ILegacyProjectSnapshot.GetProjectEngine()
        => _state.ProjectEngine;

    ILegacyDocumentSnapshot? ILegacyProjectSnapshot.GetDocument(string filePath)
        => TryGetDocument(filePath, out var document)
            ? document
            : null;

    #endregion
}
