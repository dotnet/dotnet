// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(ISemanticTokensLegendService))]
[method: ImportingConstructor]
internal class CohostSemanticTokensLegendService(IClientCapabilitiesService clientCapabilitiesService)
    : AbstractRazorSemanticTokensLegendService(clientCapabilitiesService)
{
}
