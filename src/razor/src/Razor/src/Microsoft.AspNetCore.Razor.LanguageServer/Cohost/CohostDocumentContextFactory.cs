// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Cohost;

// NOTE: This is not a "normal" MEF export (ie, exporting an interface) purely because of a strange desire to keep API in
//       RazorCohostRequestContextExtensions looking like the previous code in the non-cohost world.
[ExportRazorStatelessLspService(typeof(CohostDocumentContextFactory))]
[method: ImportingConstructor]
internal class CohostDocumentContextFactory(DocumentSnapshotFactory documentSnapshotFactory, IDocumentVersionCache documentVersionCache) : AbstractRazorLspService
{
    private readonly DocumentSnapshotFactory _documentSnapshotFactory = documentSnapshotFactory;
    private readonly IDocumentVersionCache _documentVersionCache = documentVersionCache;

    public VersionedDocumentContext Create(Uri documentUri, TextDocument textDocument)
    {
        var documentSnapshot = _documentSnapshotFactory.GetOrCreate(textDocument);

        // HACK: For cohosting, we just grab the "current" version, because we know it will have been updated
        //       since the change handling is synchronous. In future we can just remove the whole concept of
        //       document versions because TextDocument is inherently versioned.
        var version = _documentVersionCache.GetLatestDocumentVersion(documentSnapshot.FilePath.AssumeNotNull());

        return new VersionedDocumentContext(documentUri, documentSnapshot, null, version);
    }
}
