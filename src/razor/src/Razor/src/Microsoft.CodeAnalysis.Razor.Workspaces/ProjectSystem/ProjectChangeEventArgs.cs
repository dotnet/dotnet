// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class ProjectChangeEventArgs : EventArgs
{
    public ProjectChangeKind Kind { get; }
    public ProjectSnapshot? Older { get; }
    public ProjectSnapshot? Newer { get; }
    public ProjectKey ProjectKey { get; }
    public string ProjectFilePath { get; }
    public string? DocumentFilePath { get; }
    public bool IsSolutionClosing { get; }

    private ProjectChangeEventArgs(
        ProjectChangeKind kind,
        ProjectSnapshot? older,
        ProjectSnapshot? newer,
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

    public static ProjectChangeEventArgs ProjectAdded(ProjectSnapshot project, bool isSolutionClosing)
        => new(ProjectChangeKind.ProjectAdded, older: null, newer: project, documentFilePath: null, isSolutionClosing);

    public static ProjectChangeEventArgs ProjectRemoved(ProjectSnapshot project, bool isSolutionClosing)
        => new(ProjectChangeKind.ProjectRemoved, older: project, newer: null, documentFilePath: null, isSolutionClosing);

    public static ProjectChangeEventArgs ProjectChanged(ProjectSnapshot older, ProjectSnapshot newer, bool isSolutionClosing)
        => new(ProjectChangeKind.ProjectChanged, older, newer, documentFilePath: null, isSolutionClosing);

    public static ProjectChangeEventArgs DocumentAdded(ProjectSnapshot older, ProjectSnapshot newer, string documentFilePath, bool isSolutionClosing)
        => new(ProjectChangeKind.DocumentAdded, older, newer, documentFilePath, isSolutionClosing);

    public static ProjectChangeEventArgs DocumentRemoved(ProjectSnapshot older, ProjectSnapshot newer, string documentFilePath, bool isSolutionClosing)
        => new(ProjectChangeKind.DocumentRemoved, older, newer, documentFilePath, isSolutionClosing);

    public static ProjectChangeEventArgs DocumentChanged(ProjectSnapshot older, ProjectSnapshot newer, string documentFilePath, bool isSolutionClosing)
        => new(ProjectChangeKind.DocumentChanged, older, newer, documentFilePath, isSolutionClosing);
}
