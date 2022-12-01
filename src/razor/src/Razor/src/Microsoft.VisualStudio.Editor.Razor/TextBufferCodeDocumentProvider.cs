// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class TextBufferCodeDocumentProvider
{
    public abstract bool TryGetFromBuffer(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out RazorCodeDocument? codeDocument);
}
