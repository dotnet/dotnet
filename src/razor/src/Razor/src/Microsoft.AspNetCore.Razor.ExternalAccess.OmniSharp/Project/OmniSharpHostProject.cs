// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;

public sealed class OmniSharpHostProject
{
    public OmniSharpHostProject(string projectFilePath, string intermediateOutputPath, RazorConfiguration razorConfiguration, string rootNamespace)
    {
        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (intermediateOutputPath is null)
        {
            throw new ArgumentNullException(nameof(intermediateOutputPath));
        }

        if (razorConfiguration is null)
        {
            throw new ArgumentNullException(nameof(razorConfiguration));
        }

        InternalHostProject = new HostProject(projectFilePath, intermediateOutputPath, razorConfiguration, rootNamespace);
    }

    public OmniSharpProjectKey Key => new OmniSharpProjectKey(InternalHostProject.Key);

    internal HostProject InternalHostProject { get; }
}
