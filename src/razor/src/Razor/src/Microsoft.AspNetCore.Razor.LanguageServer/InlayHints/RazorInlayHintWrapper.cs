// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.InlayHints;

internal class RazorInlayHintWrapper
{
    public required TextDocumentIdentifierAndVersion TextDocument { get; set; }
    public required object? OriginalData { get; set; }
    public required Position OriginalPosition { get; set; }
}
