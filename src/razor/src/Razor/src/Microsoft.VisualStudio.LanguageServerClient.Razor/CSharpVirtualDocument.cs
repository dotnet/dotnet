// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class CSharpVirtualDocument : VirtualDocumentBase<CSharpVirtualDocumentSnapshot>
{
    private readonly ProjectKey _projectKey;

    public CSharpVirtualDocument(ProjectKey projectKey, Uri uri, ITextBuffer textBuffer)
        : base(uri, textBuffer)
    {
        _projectKey = projectKey;
    }

    protected override CSharpVirtualDocumentSnapshot GetUpdatedSnapshot(object? state) => new(_projectKey, Uri, TextBuffer.CurrentSnapshot, HostDocumentVersion);

    public override VirtualDocumentSnapshot Update(IReadOnlyList<ITextChange> changes, int hostDocumentVersion, object? state)
    {
        var result = base.Update(changes, hostDocumentVersion, state);

#if DEBUG
        var text = TextBuffer.CurrentSnapshot.GetText();

        var generatedFileStartIndex = text.IndexOf("#pragma warning disable 1591");
        var secondGeneratedFileStartIndex = text.IndexOf("#pragma warning disable 1591", generatedFileStartIndex + 20);

        Debug.Assert(secondGeneratedFileStartIndex == -1, "Generated C# file appears to have duplicated file contents. This could indicate a sync problem between language server and client.");
#endif

        return result;
    }
}
