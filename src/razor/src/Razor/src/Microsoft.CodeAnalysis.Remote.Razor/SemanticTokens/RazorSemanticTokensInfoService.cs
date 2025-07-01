// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;

[Export(typeof(IRazorSemanticTokensInfoService)), Shared]
[method: ImportingConstructor]
internal class RazorSemanticTokensInfoService(
    IDocumentMappingService documentMappingService,
    ISemanticTokensLegendService semanticTokensLegendService,
    ICSharpSemanticTokensProvider csharpSemanticTokensProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    ILoggerFactory loggerFactory)
    : AbstractRazorSemanticTokensInfoService(
        documentMappingService,
        semanticTokensLegendService,
        csharpSemanticTokensProvider,
        languageServerFeatureOptions,
        loggerFactory.GetOrCreateLogger<RazorSemanticTokensInfoService>())
{
}
