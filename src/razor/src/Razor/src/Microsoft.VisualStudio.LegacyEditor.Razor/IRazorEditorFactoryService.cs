// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal interface IRazorEditorFactoryService
{
    bool TryGetDocumentTracker(ITextBuffer textBuffer, [NotNullWhen(true)] out IVisualStudioDocumentTracker? documentTracker);
    bool TryGetParser(ITextBuffer textBuffer, [NotNullWhen(true)] out IVisualStudioRazorParser? parser);
    bool TryGetSmartIndenter(ITextBuffer textBuffer, [NotNullWhen(true)] out BraceSmartIndenter? braceSmartIndenter);
}
