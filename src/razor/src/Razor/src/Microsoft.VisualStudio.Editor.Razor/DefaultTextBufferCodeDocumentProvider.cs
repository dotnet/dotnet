// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

[System.Composition.Shared]
[Export(typeof(TextBufferCodeDocumentProvider))]
internal class DefaultTextBufferCodeDocumentProvider : TextBufferCodeDocumentProvider
{
    public override bool TryGetFromBuffer(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out RazorCodeDocument? codeDocument)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        if (textBuffer.Properties.TryGetProperty(typeof(VisualStudioRazorParser), out VisualStudioRazorParser parser) && parser.CodeDocument is not null)
        {
            codeDocument = parser.CodeDocument;
            return true;
        }

        codeDocument = null;
        return false;
    }
}
