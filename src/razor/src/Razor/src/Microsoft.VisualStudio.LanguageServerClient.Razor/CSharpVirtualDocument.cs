// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
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
}
