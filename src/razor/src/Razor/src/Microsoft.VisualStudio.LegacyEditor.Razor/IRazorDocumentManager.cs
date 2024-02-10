// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
