// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

/// <summary>
/// A very light wrapper around a file path, used to ensure consistency across the code base for what constitutes the unique
/// identifier for a project.
/// </summary>
internal sealed class ProjectKey : IEquatable<ProjectKey>
{
    // ProjectKey represents the path of the intermediate output path, which is where the project.razor.json file will
    // end up. All creation logic is here in one place to ensure this is consistent.
    public static ProjectKey From(HostProject hostProject) => new(hostProject.IntermediateOutputPath);
    public static ProjectKey From(IProjectSnapshot project) => new(project.IntermediateOutputPath);
    public static ProjectKey? From(Project project)
    {
        var intermediateOutputPath = Path.GetDirectoryName(project.CompilationOutputInfo.AssemblyPath);
        return intermediateOutputPath is null ? null : new(intermediateOutputPath);
    }

    public string Id { get; }

    private ProjectKey(string id)
    {
        Debug.Assert(id is not null, "Cannot create a key for null Id. Did you call ProjectKey.From(this) in a constructor, before initializing a property?");
        Debug.Assert(!id!.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase), "We expect the intermediate output path, not the project file");
        // The null check in the assert means the compiler thinks we're lying about id being non-nullable.. which is fair I suppose.
        Id = FilePathNormalizer.NormalizeDirectory(id).AssumeNotNull();
    }

    public override int GetHashCode()
    {
        return FilePathComparer.Instance.GetHashCode(Id);
    }

    public override bool Equals(object? other)
    {
        return Equals(other as ProjectKey);
    }

    public bool Equals(ProjectKey? other)
    {
        return FilePathComparer.Instance.Equals(Id, other?.Id);
    }

    public static bool operator ==(ProjectKey? lhs, ProjectKey? rhs)
    {
        return lhs?.Equals(rhs) ?? false;
    }

    public static bool operator !=(ProjectKey? lhs, ProjectKey? rhs)
    {
        return !lhs?.Equals(rhs) ?? false;
    }

    public override string ToString()
    {
        return "{id:" + Id + "}";
    }
}
