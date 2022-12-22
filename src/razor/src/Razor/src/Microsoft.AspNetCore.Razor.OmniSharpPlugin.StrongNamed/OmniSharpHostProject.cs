// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public sealed class OmniSharpHostProject
{
    public OmniSharpHostProject(string projectFilePath, RazorConfiguration razorConfiguration, string rootNamespace)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (razorConfiguration is null)
        {
            throw new ArgumentNullException(nameof(razorConfiguration));
        }

        InternalHostProject = new HostProject(projectFilePath, razorConfiguration, rootNamespace);
    }

    public string FilePath => InternalHostProject.FilePath;

    public RazorConfiguration Configuration => InternalHostProject.Configuration;

    public string RootNamespace => InternalHostProject.RootNamespace;

    internal HostProject InternalHostProject { get; }
}
