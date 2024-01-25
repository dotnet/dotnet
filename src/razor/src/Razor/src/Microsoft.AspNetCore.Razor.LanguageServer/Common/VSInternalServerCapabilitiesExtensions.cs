// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

internal static class VSInternalServerCapabilitiesExtensions
{
    public static void EnableDocumentColorProvider(this VSInternalServerCapabilities serverCapabilities)
    {
        serverCapabilities.DocumentColorProvider = new DocumentColorOptions();
    }

    public static void EnableSemanticTokens(this VSInternalServerCapabilities serverCapabilities, SemanticTokensLegend legend)
    {
        serverCapabilities.SemanticTokensOptions = new SemanticTokensOptions
        {
            Full = false,
            Legend = legend,
            Range = true,
        };
    }
}
