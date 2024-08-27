// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal class RemoteDocumentContext : DocumentContext
{
    public TextDocument TextDocument => Snapshot.TextDocument;

    public new RemoteDocumentSnapshot Snapshot => (RemoteDocumentSnapshot)base.Snapshot;

    public RemoteDocumentContext(Uri uri, RemoteDocumentSnapshot snapshot)
        // HACK: Need to revisit projectContext here I guess
        : base(uri, snapshot, projectContext: null)
    {
    }
}
