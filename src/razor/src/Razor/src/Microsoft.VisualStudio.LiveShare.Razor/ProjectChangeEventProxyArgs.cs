// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.LiveShare.Razor;

internal sealed class ProjectChangeEventProxyArgs : EventArgs
{
    public ProjectChangeEventProxyArgs(ProjectSnapshotHandleProxy? older, ProjectSnapshotHandleProxy? newer, ProjectProxyChangeKind kind)
    {
        if (older is null && newer is null)
        {
            throw new ArgumentException("Both projects cannot be null.");
        }

        Older = older;
        Newer = newer;
        Kind = kind;

        ProjectFilePath = older?.FilePath ?? newer!.FilePath;
        IntermediateOutputPath = older?.IntermediateOutputPath ?? newer!.IntermediateOutputPath;
    }

    public ProjectSnapshotHandleProxy? Older { get; }

    public ProjectSnapshotHandleProxy? Newer { get; }

    public Uri ProjectFilePath { get; }

    public Uri IntermediateOutputPath { get; }

    public ProjectProxyChangeKind Kind { get; }
}
