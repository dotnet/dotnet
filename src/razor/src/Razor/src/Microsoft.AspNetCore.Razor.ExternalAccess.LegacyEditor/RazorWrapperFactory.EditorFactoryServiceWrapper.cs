// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class EditorFactoryServiceWrapper(VisualStudio.LegacyEditor.Razor.IRazorEditorFactoryService obj) : Wrapper<VisualStudio.LegacyEditor.Razor.IRazorEditorFactoryService>(obj), IRazorEditorFactoryService
    {
        public bool TryGetDocumentTracker(ITextBuffer textBuffer, [NotNullWhen(true)] out IRazorDocumentTracker? documentTracker)
        {
            if (Object.TryGetDocumentTracker(textBuffer, out var obj))
            {
                documentTracker = Wrap(obj);
                return true;
            }

            documentTracker = null;
            return false;
        }

        public bool TryGetParser(ITextBuffer textBuffer, [NotNullWhen(true)] out IRazorParser? parser)
        {
            if (Object.TryGetParser(textBuffer, out var obj))
            {
                parser = Wrap(obj);
                return true;
            }

            parser = null;
            return false;
        }
    }
}
