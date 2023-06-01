// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal sealed class ProjectSnapshotHandle
{
    public string FilePath { get; }
    public RazorConfiguration? Configuration { get; }
    public string? RootNamespace { get; }

    public ProjectSnapshotHandle(string filePath, RazorConfiguration? configuration, string? rootNamespace)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        Configuration = configuration;
        RootNamespace = rootNamespace;
    }
}
