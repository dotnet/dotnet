// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal class RemoteDocumentContext : VersionedDocumentContext
{
    public TextDocument TextDocument => ((RemoteDocumentSnapshot)Snapshot).TextDocument;

    public RemoteDocumentContext(Uri uri, RemoteDocumentSnapshot snapshot)
        // HACK: Need to revisit version and projectContext here I guess
        : base(uri, snapshot, projectContext: null, version: 1)
    {
    }
}
