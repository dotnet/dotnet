// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class ProjectConfigurationFileChangeEventArgs : EventArgs
{
    public string ConfigurationFilePath { get; }
    public RazorFileChangeKind Kind { get; }

    private readonly IRazorProjectInfoDeserializer _deserializer;
    private RazorProjectInfo? _projectInfo;
    private readonly object _gate;
    private bool _deserialized;

    public ProjectConfigurationFileChangeEventArgs(
        string configurationFilePath,
        RazorFileChangeKind kind,
        IRazorProjectInfoDeserializer? projectInfoDeserializer = null)
    {
        ConfigurationFilePath = configurationFilePath ?? throw new ArgumentNullException(nameof(configurationFilePath));
        Kind = kind;
        _deserializer = projectInfoDeserializer ?? RazorProjectInfoDeserializer.Instance;
        _gate = new object();
    }

    public bool TryDeserialize([NotNullWhen(true)] out RazorProjectInfo? projectInfo)
    {
        if (Kind == RazorFileChangeKind.Removed)
        {
            // There's no file to represent the snapshot handle.
            projectInfo = null;
            return false;
        }

        lock (_gate)
        {
            if (!_deserialized)
            {
                // We use a deserialized flag instead of checking if _projectSnapshotHandle is null because if we're reading an old snapshot
                // handle that doesn't deserialize properly it could be expected that it would be null.
                _deserialized = true;
                var deserializedProjectInfo = _deserializer.DeserializeFromFile(ConfigurationFilePath);
                if (deserializedProjectInfo is null)
                {
                    projectInfo = null;
                    return false;
                }

                var normalizedSerializedFilePath = FilePathNormalizer.Normalize(deserializedProjectInfo.SerializedFilePath);
                var normalizedDetectedFilePath = FilePathNormalizer.Normalize(ConfigurationFilePath);
                if (string.Equals(normalizedSerializedFilePath, normalizedDetectedFilePath, FilePathComparison.Instance))
                {
                    _projectInfo = deserializedProjectInfo;
                }
                else
                {
                    // Stale project configuration file, most likely a user copy & pasted the project configuration file and it hasn't
                    // been re-computed yet. Fail deserialization.
                    projectInfo = null;
                    return false;
                }
            }
        }

        projectInfo = _projectInfo;
        if (projectInfo is null)
        {
            // Deserialization failed
            return false;
        }

        return true;
    }
}
