// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class HostProject
{
    public HostProject(string projectFilePath, RazorConfiguration razorConfiguration, string? rootNamespace)
    {
        FilePath = projectFilePath ?? throw new ArgumentNullException(nameof(projectFilePath));
        Configuration = razorConfiguration ?? throw new ArgumentNullException(nameof(razorConfiguration));
        RootNamespace = rootNamespace;

        Key = ProjectKey.From(this);
    }

    public RazorConfiguration Configuration { get; }

    public ProjectKey Key { get; }

    /// <summary>
    /// Gets the full path to the .csproj file for this project
    /// </summary>
    public string FilePath { get; }

    public string? RootNamespace { get; }
}
