// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal record ProjectSnapshotHandle
{
    public ProjectId ProjectId { get; }
    public RazorConfiguration Configuration { get; }
    public string? RootNamespace { get; }

    public ProjectSnapshotHandle(ProjectId projectId, RazorConfiguration configuration, string? rootNamespace)
    {
        ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        RootNamespace = rootNamespace;
    }
}
