// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

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
