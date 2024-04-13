// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal static class TextBufferExtensions
{
    /// <summary>
    /// Indicates if a <paramref name="textBuffer"/> has the legacy Razor content type. This is represented by the projection based ASP.NET Core Razor editor.
    /// </summary>
    /// <param name="textBuffer">The text buffer to inspect</param>
    /// <returns><c>true</c> if the text buffers content type represents an ASP.NET Core projection based Razor editor content type.</returns>
    public static bool IsLegacyCoreRazorBuffer(this ITextBuffer textBuffer)
    {
        var contentType = textBuffer.ContentType;

        return contentType.IsOfType(RazorLanguage.CoreContentType) ||
               contentType.IsOfType(RazorConstants.LegacyCoreContentType);
    }

    public static bool TryGetCodeDocument(this ITextBuffer textBuffer, [NotNullWhen(true)] out RazorCodeDocument? codeDocument)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (textBuffer.Properties.TryGetProperty(typeof(IVisualStudioRazorParser), out IVisualStudioRazorParser parser) &&
            parser.CodeDocument is not null)
        {
            codeDocument = parser.CodeDocument;
            return true;
        }

        codeDocument = null;
        return false;
    }
}
