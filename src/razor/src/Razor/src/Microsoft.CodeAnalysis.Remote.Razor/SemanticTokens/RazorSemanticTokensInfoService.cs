// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.DocumentMapping;
using Microsoft.Extensions.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;

[Export(typeof(IRazorSemanticTokensInfoService)), Shared]
[method: ImportingConstructor]
internal class RazorSemanticTokensInfoService(
    IRazorDocumentMappingService documentMappingService,
    ISemanticTokensLegendService semanticTokensLegendService,
    ICSharpSemanticTokensProvider csharpSemanticTokensProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IRazorLoggerFactory loggerFactory)
    : AbstractRazorSemanticTokensInfoService(
        documentMappingService,
        semanticTokensLegendService,
csharpSemanticTokensProvider,
        languageServerFeatureOptions,
        loggerFactory.CreateLogger<RemoteDocumentMappingService>())
{
}
