// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.Build.Execution;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public sealed class ProjectConfigurationProviderContext
{
    public ProjectConfigurationProviderContext(
        IReadOnlyList<string> projectCapabilities,
        ProjectInstance projectInstance)
    {
        if (projectCapabilities is null)
        {
            throw new ArgumentNullException(nameof(projectCapabilities));
        }

        if (projectInstance is null)
        {
            throw new ArgumentNullException(nameof(projectInstance));
        }

        ProjectCapabilities = projectCapabilities;
        ProjectInstance = projectInstance;
    }

    public IReadOnlyList<string> ProjectCapabilities { get; }

    public ProjectInstance ProjectInstance { get; }
}
