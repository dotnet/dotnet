// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class ProjectChangeEventArgs : EventArgs
{
    public ProjectChangeEventArgs(IProjectSnapshot older, IProjectSnapshot newer, ProjectChangeKind kind)
        : this(older, newer, null, kind, false)
    {
    }

    public ProjectChangeEventArgs(IProjectSnapshot? older, IProjectSnapshot? newer, string? documentFilePath, ProjectChangeKind kind, bool solutionIsClosing)
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
        ProjectFilePath = (older ?? newer)!.FilePath;
        ProjectKey = (older ?? newer)!.Key;
    }

    public IProjectSnapshot? Older { get; }

    public IProjectSnapshot? Newer { get; }

    public ProjectKey ProjectKey { get; }

    public string ProjectFilePath { get; }

    public string? DocumentFilePath { get; }

    public ProjectChangeKind Kind { get; }

    public bool SolutionIsClosing { get; }

    public static ProjectChangeEventArgs CreateTestInstance(IProjectSnapshot older, IProjectSnapshot newer, string documentFilePath, ProjectChangeKind kind, bool solutionIsClosing = false) =>
        new(older, newer, documentFilePath, kind, solutionIsClosing);
}
