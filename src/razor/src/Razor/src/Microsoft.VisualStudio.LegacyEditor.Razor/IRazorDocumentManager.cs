// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal interface IRazorDocumentManager
{
    Task OnTextViewOpenedAsync(ITextView textView, IEnumerable<ITextBuffer> subjectBuffers);
    Task OnTextViewClosedAsync(ITextView textView, IEnumerable<ITextBuffer> subjectBuffers);
}
