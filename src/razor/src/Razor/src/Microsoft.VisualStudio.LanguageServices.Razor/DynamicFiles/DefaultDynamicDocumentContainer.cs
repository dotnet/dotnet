// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

// This types purpose is to serve as a non-Razor specific document delivery mechanism for Roslyn.
// Given a DocumentSnapshot this class allows the retrieval of a TextLoader for the generated C#
// and services to help map spans and excerpts to and from the top-level Razor document to behind
// the scenes C#.
internal sealed class DefaultDynamicDocumentContainer(IDocumentSnapshot documentSnapshot, ILoggerFactory loggerFactory) : IDynamicDocumentContainer
{
    private readonly IDocumentSnapshot _documentSnapshot = documentSnapshot ?? throw new ArgumentNullException(nameof(documentSnapshot));
    private RazorDocumentExcerptService? _excerptService;
    private RazorSpanMappingService? _spanMappingService;
    private RazorMappingService? _mappingService;

    public string FilePath => _documentSnapshot.FilePath;

    public bool SupportsDiagnostics => false;

    public void SetSupportsDiagnostics(bool enabled)
    {
        // This dynamic document container never supports diagnostics, so we don't allow enabling them.
    }

    public TextLoader GetTextLoader(string filePath)
        => new GeneratedDocumentTextLoader(_documentSnapshot, filePath);

    public IRazorDocumentExcerptServiceImplementation GetExcerptService()
        => _excerptService ?? InterlockedOperations.Initialize(ref _excerptService,
            new RazorDocumentExcerptService(_documentSnapshot, GetSpanMappingService()));

    public IRazorSpanMappingService GetSpanMappingService()
        => _spanMappingService ?? InterlockedOperations.Initialize(ref _spanMappingService,
            new RazorSpanMappingService(_documentSnapshot));

    public IRazorDocumentPropertiesService GetDocumentPropertiesService()
    {
        // DocumentPropertiesServices are used to tell Roslyn to provide C# diagnostics for LSP provided documents to be shown
        // in the editor given a specific Language Server Client. Given this type is a container for DocumentSnapshots, we don't
        // have a Language Server to associate errors with or an open document to display those errors on. We return `null` to
        // opt out of those features.
        return null!;
    }

    public IRazorMappingService? GetMappingService()
        => _mappingService ?? InterlockedOperations.Initialize(ref _mappingService,
            new RazorMappingService(_documentSnapshot, NoOpTelemetryReporter.Instance, loggerFactory));
}
