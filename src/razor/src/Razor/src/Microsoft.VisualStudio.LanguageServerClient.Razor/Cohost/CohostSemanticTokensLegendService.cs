// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Export(typeof(ISemanticTokensLegendService))]
[method: ImportingConstructor]
internal class CohostSemanticTokensLegendService(IClientCapabilitiesService clientCapabilitiesService) : RazorSemanticTokensLegendService(clientCapabilitiesService)
{
}
