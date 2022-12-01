// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class ProjectChangeEventArgs : EventArgs
{
    public ProjectChangeEventArgs(ProjectSnapshot older, ProjectSnapshot newer, ProjectChangeKind kind)
        : this(older, newer, null, kind, false)
    {
    }

    public ProjectChangeEventArgs(ProjectSnapshot? older, ProjectSnapshot? newer, string? documentFilePath, ProjectChangeKind kind, bool solutionIsClosing)
    {
        if (older is null && newer is null)
        {
            throw new ArgumentException("Both projects cannot be null.");
        }

        Older = older;
        Newer = newer;
        DocumentFilePath = documentFilePath;
        Kind = kind;
        SolutionIsClosing = solutionIsClosing;
        ProjectFilePath = older?.FilePath ?? newer?.FilePath;
    }

    public ProjectSnapshot? Older { get; }

    public ProjectSnapshot? Newer { get; }

    public string? ProjectFilePath { get; }

    public string? DocumentFilePath { get; }

    public ProjectChangeKind Kind { get; }

    public bool SolutionIsClosing { get; }

    public static ProjectChangeEventArgs CreateTestInstance(ProjectSnapshot older, ProjectSnapshot newer, string documentFilePath, ProjectChangeKind kind, bool solutionIsClosing = false) =>
        new(older, newer, documentFilePath, kind, solutionIsClosing);
}
