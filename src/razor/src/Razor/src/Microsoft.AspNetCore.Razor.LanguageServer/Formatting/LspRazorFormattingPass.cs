// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal sealed class LspRazorFormattingPass(
    IDocumentMappingService documentMappingService,
    RazorLSPOptionsMonitor optionsMonitor)
    : RazorFormattingPassBase(documentMappingService)
{
    protected override bool CodeBlockBraceOnNextLine => optionsMonitor.CurrentValue.CodeBlockBraceOnNextLine;
}
