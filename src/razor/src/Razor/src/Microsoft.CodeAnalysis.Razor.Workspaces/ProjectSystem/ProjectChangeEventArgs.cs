// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class ProjectChangeEventArgs : EventArgs
{
    public ProjectChangeKind Kind { get; }
    public IProjectSnapshot? Older { get; }
    public IProjectSnapshot? Newer { get; }
    public ProjectKey ProjectKey { get; }
    public string ProjectFilePath { get; }
    public string? DocumentFilePath { get; }
    public bool IsSolutionClosing { get; }

    private ProjectChangeEventArgs(
        ProjectChangeKind kind,
        IProjectSnapshot? older,
        IProjectSnapshot? newer,
        string? documentFilePath,
        bool isSolutionClosing)
    {
        if (older is null && newer is null)
        {
            ThrowHelper.ThrowInvalidOperationException("Both projects cannot be null.");
        }

        Kind = kind;
        Older = older;
        Newer = newer;
        ProjectKey = (older ?? newer).AssumeNotNull().Key;
        ProjectFilePath = (older ?? newer).AssumeNotNull().FilePath;
        DocumentFilePath = documentFilePath;
        IsSolutionClosing = isSolutionClosing;
    }

    public static ProjectChangeEventArgs ProjectAdded(IProjectSnapshot project, bool isSolutionClosing)
        => new(ProjectChangeKind.ProjectAdded, older: null, newer: project, documentFilePath: null, isSolutionClosing);

    public static ProjectChangeEventArgs ProjectRemoved(IProjectSnapshot project, bool isSolutionClosing)
        => new(ProjectChangeKind.ProjectRemoved, older: project, newer: null, documentFilePath: null, isSolutionClosing);

    public static ProjectChangeEventArgs ProjectChanged(IProjectSnapshot older, IProjectSnapshot newer, bool isSolutionClosing)
        => new(ProjectChangeKind.ProjectChanged, older, newer, documentFilePath: null, isSolutionClosing);

    public static ProjectChangeEventArgs DocumentAdded(IProjectSnapshot older, IProjectSnapshot newer, string documentFilePath, bool isSolutionClosing)
        => new(ProjectChangeKind.DocumentAdded, older, newer, documentFilePath, isSolutionClosing);

    public static ProjectChangeEventArgs DocumentRemoved(IProjectSnapshot older, IProjectSnapshot newer, string documentFilePath, bool isSolutionClosing)
        => new(ProjectChangeKind.DocumentRemoved, older, newer, documentFilePath, isSolutionClosing);

    public static ProjectChangeEventArgs DocumentChanged(IProjectSnapshot older, IProjectSnapshot newer, string documentFilePath, bool isSolutionClosing)
        => new(ProjectChangeKind.DocumentChanged, older, newer, documentFilePath, isSolutionClosing);
}
