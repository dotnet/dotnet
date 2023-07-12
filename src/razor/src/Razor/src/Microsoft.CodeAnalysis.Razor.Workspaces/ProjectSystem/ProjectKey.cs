// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

/// <summary>
/// A very light wrapper around a file path, used to ensure consistency across the code base for what constitutes the unique
/// identifier for a project.
/// </summary>
internal sealed class ProjectKey
{
    // ProjectKey represents the full path to the project file name. All creation logic is here in one place
    // to ensure this is consistent.
    public static ProjectKey From(HostProject hostProject) => new(hostProject.FilePath);
    public static ProjectKey From(IProjectSnapshot project) => new(project.FilePath);
    public static ProjectKey? From(Project project) => project.FilePath is null ? null : new(project.FilePath);
    public static ProjectKey From(string projectFilePath)
    {
        // Right now, we expect this to be a project file path. This will change in future
        Debug.Assert(projectFilePath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase));
        return new(projectFilePath);
    }
    public static ProjectKey FromLegacy(string projectFilePath)
    {
        // In the legacy editor, we expect this to be a project file path.
        Debug.Assert(projectFilePath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase));
        return new(projectFilePath);
    }

    public string Id { get; }

    private ProjectKey(string id)
    {
        Debug.Assert(id is not null, "Cannot create a key for null Id. Did you call ProjectKey.From(this) in a constructor, before initializing a property?");
        // The null check in the assert means the compiler thinks we're lying about id being non-nullable.. which is fair I suppose.
        Id = id.AssumeNotNull();
    }

    public override int GetHashCode()
    {
        return FilePathComparer.Instance.GetHashCode(Id);
    }

    public override bool Equals(object? other)
    {
        return FilePathComparer.Instance.Equals(Id, (other as ProjectKey)?.Id);
    }
}
