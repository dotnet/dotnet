// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

public static class TestProjectKey
{
    /// <summary>
    /// For testing only, creates a project key directly from a string. Exists so we don't expose a real string-only
    /// factory for ProjectKeys.
    /// </summary>
    internal static ProjectKey Create(string projectFilePath)
    {
        // Parameters here are largely useless, we just want a key
        var hostProject = new HostProject(projectFilePath, projectFilePath, FallbackRazorConfiguration.Latest, rootNamespace: null);
        return hostProject.Key;
    }
}
