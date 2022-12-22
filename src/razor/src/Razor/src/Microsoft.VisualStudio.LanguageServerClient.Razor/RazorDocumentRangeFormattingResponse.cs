// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class RazorDocumentRangeFormattingResponse
{
    public required TextEdit[] Edits { get; set; }
}
