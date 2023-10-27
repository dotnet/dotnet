// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class HostProject
{
    public HostProject(string projectFilePath, string intermediateOutputPath, RazorConfiguration razorConfiguration, string? rootNamespace, string? displayName = null)
    {
        FilePath = projectFilePath ?? throw new ArgumentNullException(nameof(projectFilePath));
        IntermediateOutputPath = intermediateOutputPath ?? throw new ArgumentNullException(nameof(intermediateOutputPath));
        Configuration = razorConfiguration ?? throw new ArgumentNullException(nameof(razorConfiguration));
        RootNamespace = rootNamespace;
        DisplayName = displayName ?? Path.GetFileNameWithoutExtension(projectFilePath);

        Key = ProjectKey.From(this);
    }

    public RazorConfiguration Configuration { get; }

    public ProjectKey Key { get; }

    /// <summary>
    /// Gets the full path to the .csproj file for this project
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Gets the full path to the folder under 'obj' where the project.razor.bin file will live
    /// </summary>
    public string IntermediateOutputPath { get; }

    public string? RootNamespace { get; }

    /// <summary>
    /// An extra user-friendly string to show in the VS navigation bar to help the user, of the form "{ProjectFileName} ({Flavor})"
    /// </summary>
    public string DisplayName { get; }
}
