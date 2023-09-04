// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class RazorCodeDocumentExtensions
{
    public static IRazorGeneratedDocument GetGeneratedDocument(this RazorCodeDocument document, RazorLanguageKind languageKind)
        => languageKind switch
        {
            RazorLanguageKind.CSharp => document.GetCSharpDocument(),
            RazorLanguageKind.Html => document.GetHtmlDocument(),
            _ => throw new System.InvalidOperationException(),
        };
}
