// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.GoToDefinition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Definition;

internal sealed class RazorComponentDefinitionService(
    IRazorComponentSearchEngine componentSearchEngine,
    IDocumentMappingService documentMappingService,
    ILoggerFactory loggerFactory)
    : AbstractRazorComponentDefinitionService(componentSearchEngine, documentMappingService, loggerFactory.GetOrCreateLogger<RazorComponentDefinitionService>())
{
}
