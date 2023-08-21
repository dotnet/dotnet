// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class VersionedDocumentContext : DocumentContext
{
    public virtual int Version { get; }

    public VersionedDocumentContext(Uri uri, IDocumentSnapshot snapshot, VSProjectContext? projectContext, int version)
        : base(uri, snapshot, projectContext)
    {
        Version = version;
    }

    // Sadly we target net472 which doesn't support covariant return types, so this can't override.
    public new TextDocumentIdentifierAndVersion Identifier => new TextDocumentIdentifierAndVersion(
        base.Identifier,
        Version);
}
