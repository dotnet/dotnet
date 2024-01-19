// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Cohost;

[Export(typeof(DocumentSnapshotFactory)), Shared]
[method: ImportingConstructor]
internal class DocumentSnapshotFactory(Lazy<ProjectSnapshotFactory> projectSnapshotFactory)
{
    private static readonly ConditionalWeakTable<TextDocument, IDocumentSnapshot> _documentSnapshots = new();

    private readonly Lazy<ProjectSnapshotFactory> _projectSnapshotFactory = projectSnapshotFactory;

    public IDocumentSnapshot GetOrCreate(TextDocument textDocument)
    {
        if (!_documentSnapshots.TryGetValue(textDocument, out var documentSnapshot))
        {
            var projectSnapshot = _projectSnapshotFactory.Value.GetOrCreate(textDocument.Project);
            documentSnapshot = new CohostDocumentSnapshot(textDocument, projectSnapshot);
            _documentSnapshots.Add(textDocument, documentSnapshot);
        }

        return documentSnapshot;
    }
}
