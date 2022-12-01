// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class CSharpVirtualDocument : VirtualDocumentBase<CSharpVirtualDocumentSnapshot>
{
    public CSharpVirtualDocument(Uri uri, ITextBuffer textBuffer) : base(uri, textBuffer)
    {
    }

    protected override CSharpVirtualDocumentSnapshot GetUpdatedSnapshot(object? state) => new(Uri, TextBuffer.CurrentSnapshot, HostDocumentVersion);
}
