// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Text;
using System;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.Test.Common;

internal class TestVirtualDocument : VirtualDocumentBase<TestVirtualDocumentSnapshot>
{
    public TestVirtualDocument(Uri uri, ITextBuffer textBuffer)
        : base(uri, textBuffer)
    {
    }

    protected override TestVirtualDocumentSnapshot GetUpdatedSnapshot(object? state) => new(Uri, HostDocumentVersion, TextBuffer.CurrentSnapshot, state);
}
