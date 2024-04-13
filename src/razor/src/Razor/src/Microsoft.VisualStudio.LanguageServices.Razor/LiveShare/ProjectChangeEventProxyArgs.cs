// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor;

namespace Microsoft.VisualStudio.Razor.LiveShare;

// This type must be public because it is exposed by a public interface that is implemented as
// an RPC proxy by live share.
public sealed class ProjectChangeEventProxyArgs : EventArgs
{
    public ProjectSnapshotHandleProxy? Older { get; }
    public ProjectSnapshotHandleProxy? Newer { get; }
    public ProjectProxyChangeKind Kind { get; }
    public Uri ProjectFilePath { get; }
    public Uri IntermediateOutputPath { get; }

    public ProjectChangeEventProxyArgs(ProjectSnapshotHandleProxy? older, ProjectSnapshotHandleProxy? newer, ProjectProxyChangeKind kind)
    {
        if (older is null && newer is null)
        {
            throw new ArgumentException("Both projects cannot be null.");
        }

        Older = older;
        Newer = newer;
        Kind = kind;

        ProjectFilePath = older?.FilePath ?? newer.AssumeNotNull().FilePath;
        IntermediateOutputPath = older?.IntermediateOutputPath ?? newer.AssumeNotNull().IntermediateOutputPath;
    }
}
