// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

internal class CSharpVirtualDocumentSnapshot : VirtualDocumentSnapshot
{
    public CSharpVirtualDocumentSnapshot(
        ProjectKey projectKey,
        Uri uri,
        ITextSnapshot snapshot,
        long? hostDocumentSyncVersion)
    {
        if (uri is null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (snapshot is null)
        {
            throw new ArgumentNullException(nameof(snapshot));
        }

        ProjectKey = projectKey;
        Uri = uri;
        Snapshot = snapshot;
        HostDocumentSyncVersion = hostDocumentSyncVersion;
    }

    public ProjectKey ProjectKey { get; }

    public override Uri Uri { get; }

    public override ITextSnapshot Snapshot { get; }

    public override long? HostDocumentSyncVersion { get; }
}
