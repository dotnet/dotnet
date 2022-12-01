// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class RazorEditorFactoryService
{
    public abstract bool TryGetDocumentTracker(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out VisualStudioDocumentTracker? documentTracker);

    public abstract bool TryGetParser(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out VisualStudioRazorParser? parser);

    internal abstract bool TryGetSmartIndenter(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out BraceSmartIndenter? braceSmartIndenter);
}
