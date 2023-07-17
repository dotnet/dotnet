// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Text;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class EditorFactoryServiceWrapper(RazorEditorFactoryService obj) : Wrapper<RazorEditorFactoryService>(obj), IRazorEditorFactoryService
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
