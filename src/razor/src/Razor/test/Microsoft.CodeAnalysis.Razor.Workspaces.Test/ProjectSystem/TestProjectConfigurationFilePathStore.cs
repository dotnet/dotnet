// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Test.ProjectSystem;

internal class TestProjectConfigurationFilePathStore : ProjectConfigurationFilePathStore
{
    private Dictionary<ProjectKey, string> _mappings = new();

    public override event EventHandler<ProjectConfigurationFilePathChangedEventArgs>? Changed { add { } remove { } }

    public override IReadOnlyDictionary<ProjectKey, string> GetMappings()
        => _mappings;

    public override void Remove(ProjectKey projectKey)
        => _mappings.Remove(projectKey);

    public override void Set(ProjectKey projectKey, string configurationFilePath)
        => _mappings[projectKey] = configurationFilePath;

    public override bool TryGet(ProjectKey projectKey, [NotNullWhen(true)] out string? configurationFilePath)
        => _mappings.TryGetValue(projectKey, out configurationFilePath);
}
