// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorEditorFactoryService
{
    bool TryGetDocumentTracker(ITextBuffer textBuffer, [NotNullWhen(true)] out IRazorDocumentTracker? documentTracker);
    bool TryGetParser(ITextBuffer textBuffer, [NotNullWhen(true)] out IRazorParser? parser);
}
