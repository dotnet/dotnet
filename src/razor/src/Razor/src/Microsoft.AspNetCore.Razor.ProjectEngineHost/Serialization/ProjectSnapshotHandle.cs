// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Serialization;

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
