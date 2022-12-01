// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class ProjectConfigurationFileChangeEventArgs
{
    private readonly JsonFileDeserializer _jsonFileDeserializer;
    private ProjectRazorJson? _projectRazorJson;
    private readonly object _projectSnapshotHandleLock;
    private bool _deserialized;

    public ProjectConfigurationFileChangeEventArgs(
        string configurationFilePath,
        RazorFileChangeKind kind) : this(configurationFilePath, kind, JsonFileDeserializer.Instance)
    {
    }

    // Internal for testing
    internal ProjectConfigurationFileChangeEventArgs(
        string configurationFilePath,
        RazorFileChangeKind kind,
        JsonFileDeserializer jsonFileDeserializer)
    {
        if (configurationFilePath is null)
        {
            throw new ArgumentNullException(nameof(configurationFilePath));
        }

        if (jsonFileDeserializer is null)
        {
            throw new ArgumentNullException(nameof(jsonFileDeserializer));
        }

        ConfigurationFilePath = configurationFilePath;
        Kind = kind;
        _jsonFileDeserializer = jsonFileDeserializer;
        _projectSnapshotHandleLock = new object();
    }

    public string ConfigurationFilePath { get; }

    public RazorFileChangeKind Kind { get; }

    public bool TryDeserialize([NotNullWhen(true)] out ProjectRazorJson? projectRazorJson)
    {
        if (Kind == RazorFileChangeKind.Removed)
        {
            // There's no file to represent the snapshot handle.
            projectRazorJson = null;
            return false;
        }

        lock (_projectSnapshotHandleLock)
        {
            if (!_deserialized)
            {
                // We use a deserialized flag instead of checking if _projectSnapshotHandle is null because if we're reading an old snapshot
                // handle that doesn't deserialize properly it could be expected that it would be null.
                _deserialized = true;
                var deserializedProjectRazorJson = _jsonFileDeserializer.Deserialize<ProjectRazorJson>(ConfigurationFilePath);
                if (deserializedProjectRazorJson is null)
                {
                    projectRazorJson = null;
                    return false;
                }

                var normalizedSerializedFilePath = FilePathNormalizer.Normalize(deserializedProjectRazorJson.SerializedFilePath);
                var normalizedDetectedFilePath = FilePathNormalizer.Normalize(ConfigurationFilePath);
                if (string.Equals(normalizedSerializedFilePath, normalizedDetectedFilePath, FilePathComparison.Instance))
                {
                    _projectRazorJson = deserializedProjectRazorJson;
                }
                else
                {
                    // Stale project configuration file, most likely a user copy & pasted the project configuration file and it hasn't
                    // been re-computed yet. Fail deserialization.
                    projectRazorJson = null;
                    return false;
                }
            }
        }

        projectRazorJson = _projectRazorJson;
        if (projectRazorJson is null)
        {
            // Deserialization failed
            return false;
        }

        return true;
    }
}
