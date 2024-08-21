// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

[Export(typeof(DocumentSnapshotFactory)), Shared]
[method: ImportingConstructor]
internal class DocumentSnapshotFactory(Lazy<ProjectSnapshotFactory> projectSnapshotFactory, IFilePathService filePathService)
{
    private static readonly ConditionalWeakTable<TextDocument, RemoteDocumentSnapshot> s_documentSnapshots = new();

    private readonly Lazy<ProjectSnapshotFactory> _projectSnapshotFactory = projectSnapshotFactory;
    private readonly IFilePathService _filePathService = filePathService;

    public RemoteDocumentSnapshot GetOrCreate(TextDocument textDocument)
    {
        lock (s_documentSnapshots)
        {
            if (!s_documentSnapshots.TryGetValue(textDocument, out var documentSnapshot))
            {
                var projectSnapshot = _projectSnapshotFactory.Value.GetOrCreate(textDocument.Project);
                documentSnapshot = new RemoteDocumentSnapshot(textDocument, projectSnapshot, _filePathService);
                s_documentSnapshots.Add(textDocument, documentSnapshot);
            }

            return documentSnapshot;
        }
    }
}
