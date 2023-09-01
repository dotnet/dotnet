// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.LiveShare.Razor;

internal sealed class ProjectSnapshotHandleProxy
{
    public Uri FilePath { get; }
    public Uri IntermediateOutputPath { get; }
    public RazorConfiguration Configuration { get; }
    public string? RootNamespace { get; }
    public ProjectWorkspaceState? ProjectWorkspaceState { get; }

    public ProjectSnapshotHandleProxy(
        Uri filePath,
        Uri intermediateOutputPath,
        RazorConfiguration configuration,
        string? rootNamespace,
        ProjectWorkspaceState? projectWorkspaceState)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        IntermediateOutputPath = intermediateOutputPath ?? throw new ArgumentNullException(nameof(intermediateOutputPath));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        RootNamespace = rootNamespace;
        ProjectWorkspaceState = projectWorkspaceState;
    }
}
