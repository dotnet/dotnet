// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class VersionedDocumentContext(Uri uri, IDocumentSnapshot snapshot, VSProjectContext? projectContext, int version)
    : DocumentContext(uri, snapshot, projectContext)
{
    public int Version { get; } = version;

    public TextDocumentIdentifierAndVersion GetTextDocumentIdentifierAndVersion()
        => new(GetTextDocumentIdentifier(), Version);
}
