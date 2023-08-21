// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Shared]
[Export(typeof(ProjectConfigurationFilePathStore))]
internal class DefaultProjectConfigurationFilePathStore : ProjectConfigurationFilePathStore
{
    private readonly Dictionary<ProjectKey, string> _mappings;
    private readonly object _mappingsLock;

    public override event EventHandler<ProjectConfigurationFilePathChangedEventArgs>? Changed;

    [ImportingConstructor]
    public DefaultProjectConfigurationFilePathStore()
    {
        _mappings = new Dictionary<ProjectKey, string>();
        _mappingsLock = new object();
    }

    public override IReadOnlyDictionary<ProjectKey, string> GetMappings() => new Dictionary<ProjectKey, string>(_mappings);

    public override void Set(ProjectKey projectKey, string configurationFilePath)
    {
        if (configurationFilePath is null)
        {
            throw new ArgumentNullException(nameof(configurationFilePath));
        }

        lock (_mappingsLock)
        {
            // Resolve any relative pathing in the configuration path so we can talk in absolutes
            configurationFilePath = Path.GetFullPath(configurationFilePath);

            if (_mappings.TryGetValue(projectKey, out var existingConfigurationFilePath) &&
                FilePathComparer.Instance.Equals(configurationFilePath, existingConfigurationFilePath))
            {
                // Already have this mapping, don't invoke changed.
                return;
            }

            _mappings[projectKey] = configurationFilePath;
        }

        var args = new ProjectConfigurationFilePathChangedEventArgs(projectKey, configurationFilePath);
        Changed?.Invoke(this, args);
    }

    public override void Remove(ProjectKey projectKey)
    {
        lock (_mappingsLock)
        {
            if (!_mappings.Remove(projectKey))
            {
                // We weren't tracking the project file path, no-op.
                return;
            }
        }

        var args = new ProjectConfigurationFilePathChangedEventArgs(projectKey, configurationFilePath: null);
        Changed?.Invoke(this, args);
    }

    public override bool TryGet(ProjectKey projectKey, [NotNullWhen(returnValue: true)] out string? configurationFilePath)
    {
        lock (_mappingsLock)
        {
            return _mappings.TryGetValue(projectKey, out configurationFilePath);
        }
    }
}
