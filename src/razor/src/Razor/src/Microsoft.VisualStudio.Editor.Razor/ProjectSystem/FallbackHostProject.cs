// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class FallbackHostProject : HostProject
{
    public FallbackHostProject(string projectFilePath, string intermediateOutputPath, RazorConfiguration razorConfiguration, string? rootNamespace, string displayName)
        : base(projectFilePath, intermediateOutputPath, razorConfiguration, rootNamespace, displayName)
    {
    }
}
