// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Razor;

// This types purpose is to serve as a non-Razor specific document delivery mechanism for Roslyn.
// Given a DocumentSnapshot this class allows the retrieval of a TextLoader for the generated C#
// and services to help map spans and excerpts to and from the top-level Razor document to behind
// the scenes C#.
internal sealed class DefaultDynamicDocumentContainer : DynamicDocumentContainer
{
    private readonly IDocumentSnapshot _documentSnapshot;
    private RazorDocumentExcerptService? _excerptService;
    private RazorSpanMappingService? _mappingService;

    public DefaultDynamicDocumentContainer(IDocumentSnapshot documentSnapshot)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        _documentSnapshot = documentSnapshot;
    }

    public override string FilePath => _documentSnapshot.FilePath.AssumeNotNull();

    public override bool SupportsDiagnostics => false;

    public override TextLoader GetTextLoader(string filePath) => new GeneratedDocumentTextLoader(_documentSnapshot, filePath);

    public override IRazorDocumentExcerptServiceImplementation GetExcerptService()
    {
        if (_excerptService is null)
        {
            var mappingService = GetMappingService();
            _excerptService = new RazorDocumentExcerptService(_documentSnapshot, mappingService);
        }

        return _excerptService;
    }

    public override IRazorSpanMappingService GetMappingService()
    {
        _mappingService ??= new RazorSpanMappingService(_documentSnapshot);

        return _mappingService;
    }

    public override IRazorDocumentPropertiesService? GetDocumentPropertiesService()
    {
        // DocumentPropertiesServices are used to tell Roslyn to provide C# diagnostics for LSP provided documents to be shown
        // in the editor given a specific Language Server Client. Given this type is a container for DocumentSnapshots, we don't
        // have a Language Server to associate errors with or an open document to display those errors on. We return `null` to
        // opt out of those features.
        return null;
    }
}
