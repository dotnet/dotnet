// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.GoToDefinition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.GoToDefinition;

[Export(typeof(IRazorComponentDefinitionService)), Shared]
[method: ImportingConstructor]
internal sealed class RazorComponentDefinitionService(
    IRazorComponentSearchEngine componentSearchEngine,
    IDocumentMappingService documentMappingService,
    ILoggerFactory loggerFactory)
    : AbstractRazorComponentDefinitionService(componentSearchEngine, documentMappingService, loggerFactory.GetOrCreateLogger<RazorComponentDefinitionService>())
{
}
