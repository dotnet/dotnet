// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

namespace Microsoft.VisualStudio.LiveShare.Razor;

public sealed class ProjectSnapshotHandleProxy
{
    public Uri FilePath { get; }
    public RazorConfiguration Configuration { get; }
    public string? RootNamespace { get; }
    public ProjectWorkspaceState? ProjectWorkspaceState { get; }

    public ProjectSnapshotHandleProxy(
        Uri filePath,
        RazorConfiguration configuration,
        string? rootNamespace,
        ProjectWorkspaceState? projectWorkspaceState)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        RootNamespace = rootNamespace;
        ProjectWorkspaceState = projectWorkspaceState;
    }
}
