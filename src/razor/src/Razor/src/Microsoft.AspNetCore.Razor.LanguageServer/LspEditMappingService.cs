// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LspEditMappingService(
    IDocumentMappingService documentMappingService,
    IFilePathService filePathService,
    IDocumentContextFactory documentContextFactory) : AbstractEditMappingService(documentMappingService, filePathService)
{
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;

    protected override bool TryGetDocumentContext(IDocumentSnapshot contextDocumentSnapshot, Uri razorDocumentUri, VSProjectContext? projectContext, [NotNullWhen(true)] out DocumentContext? documentContext)
    {
        if (!_documentContextFactory.TryCreate(razorDocumentUri, projectContext, out documentContext))
        {
            return false;
        }

        return true;
    }
}
