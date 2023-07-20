// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class ProjectConfigurationFilePathChangedEventArgs : EventArgs
{
    public ProjectConfigurationFilePathChangedEventArgs(ProjectKey projectKey, string? configurationFilePath)
    {
        if (projectKey is null)
        {
            throw new ArgumentNullException(nameof(projectKey));
        }

        ProjectKey = projectKey;
        ConfigurationFilePath = configurationFilePath;
    }

    public ProjectKey ProjectKey { get; }

    public string? ConfigurationFilePath { get; }
}
