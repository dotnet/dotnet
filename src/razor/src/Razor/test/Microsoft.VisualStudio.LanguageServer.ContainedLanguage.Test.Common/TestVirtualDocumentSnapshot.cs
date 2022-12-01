// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

internal class TestVirtualDocumentSnapshot : VirtualDocumentSnapshot
{
    private readonly long? _hostDocumentSyncVersion;

    public TestVirtualDocumentSnapshot(Uri uri, long? hostDocumentVersion) : this(uri, hostDocumentVersion, snapshot: null, state: null)
    {
    }

    public TestVirtualDocumentSnapshot(Uri uri, long? hostDocumentVersion, ITextSnapshot snapshot, object state)
    {
        Uri = uri;
        _hostDocumentSyncVersion = hostDocumentVersion;
        Snapshot = snapshot;
        State = state;
    }

    public override Uri Uri { get; }

    public override ITextSnapshot Snapshot { get; }

    public override long? HostDocumentSyncVersion => _hostDocumentSyncVersion;

    public object State { get; }

    public TestVirtualDocumentSnapshot Fork(int hostDocumentVersion) => new(Uri, hostDocumentVersion);
}
