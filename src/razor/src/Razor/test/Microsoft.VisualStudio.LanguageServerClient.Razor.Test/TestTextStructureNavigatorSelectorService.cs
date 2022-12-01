// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Test;

internal class TestTextStructureNavigatorSelectorService : ITextStructureNavigatorSelectorService
{
    private static readonly ITextStructureNavigator s_textStructureNavigator = new TestTextStructureNavigator();

    public ITextStructureNavigator CreateTextStructureNavigator(ITextBuffer textBuffer, IContentType contentType) => s_textStructureNavigator;

    public ITextStructureNavigator GetTextStructureNavigator(ITextBuffer textBuffer) => s_textStructureNavigator;

    private class TestTextStructureNavigator : ITextStructureNavigator
    {
        public IContentType ContentType => throw new NotImplementedException();

        public TextExtent GetExtentOfWord(SnapshotPoint currentPosition)
            => new(new SnapshotSpan(new StringTextSnapshot("@{ }"), new Span(0, 0)), isSignificant: false);

        public SnapshotSpan GetSpanOfEnclosing(SnapshotSpan activeSpan) => throw new NotImplementedException();

        public SnapshotSpan GetSpanOfFirstChild(SnapshotSpan activeSpan) => throw new NotImplementedException();

        public SnapshotSpan GetSpanOfNextSibling(SnapshotSpan activeSpan) => throw new NotImplementedException();

        public SnapshotSpan GetSpanOfPreviousSibling(SnapshotSpan activeSpan) => throw new NotImplementedException();
    }
}
