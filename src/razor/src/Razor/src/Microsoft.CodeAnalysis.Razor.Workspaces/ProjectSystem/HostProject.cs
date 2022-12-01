// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class HostProject
{
    public HostProject(string projectFilePath, RazorConfiguration razorConfiguration, string? rootNamespace)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (razorConfiguration is null)
        {
            throw new ArgumentNullException(nameof(razorConfiguration));
        }

        FilePath = projectFilePath;
        Configuration = razorConfiguration;
        RootNamespace = rootNamespace;
    }

    public RazorConfiguration Configuration { get; }

    public string FilePath { get; }

    public string? RootNamespace { get; }
}
