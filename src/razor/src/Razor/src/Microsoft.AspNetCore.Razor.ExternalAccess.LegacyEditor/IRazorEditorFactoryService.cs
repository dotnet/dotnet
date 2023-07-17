// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorEditorFactoryService
{
    bool TryGetDocumentTracker(ITextBuffer textBuffer, [NotNullWhen(true)] out IRazorDocumentTracker? documentTracker);
    bool TryGetParser(ITextBuffer textBuffer, [NotNullWhen(true)] out IRazorParser? parser);
}
