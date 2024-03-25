// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

/// <summary>
/// A very light wrapper around a file path, used to ensure consistency across the code base for what constitutes the unique
/// identifier for a project.
/// </summary>
[DebuggerDisplay("id: {Id}")]
internal readonly record struct ProjectKey
{
    // ProjectKey represents the path of the intermediate output path, which is where the project.razor.bin file will
    // end up. All creation logic is here in one place to ensure this is consistent.
    public static ProjectKey From(HostProject hostProject) => new(hostProject.IntermediateOutputPath);
    public static ProjectKey From(IProjectSnapshot project) => new(project.IntermediateOutputPath);
    public static ProjectKey From(Project project)
    {
        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(project.CompilationOutputInfo.AssemblyPath);
        return new(intermediateOutputPath);
    }

    internal static ProjectKey FromString(string projectKeyId) => new(projectKeyId);

    public string Id { get; }

    private ProjectKey(string id)
    {
        Debug.Assert(id is not null, "Cannot create a key for null Id. Did you call ProjectKey.From(this) in a constructor, before initializing a property?");
        Debug.Assert(!id!.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase), "We expect the intermediate output path, not the project file");

        Id = FilePathNormalizer.NormalizeDirectory(id);
    }

    public override int GetHashCode()
    {
        return Id is null ? 0 : FilePathComparer.Instance.GetHashCode(Id);
    }

    public bool Equals(ProjectKey other)
    {
        return FilePathComparer.Instance.Equals(Id, other.Id);
    }

    /// <summary>
    /// Returns <see langword="true"/> if this <see cref="ProjectKey"/> matches the given <see cref="Project"/>.
    /// </summary>
    public bool Matches(Project project)
    {
        // In order to perform this check, we are relying on the fact that Id will always end with a '/',
        // because it is guaranteed to be normalized. However, CompilationOutputInfo.AssemblyPath will
        // contain the assembly file name, which AreDirectoryPathsEquivalent will shave off before comparing.
        // So, AreDirectoryPathsEquivalent will return true when Id is "C:/my/project/path/"
        // and the assembly path is "C:\my\project\path\assembly.dll"

        Debug.Assert(Id.EndsWith('/'), $"This method can't be called if {nameof(Id)} is not a normalized directory path.");

        return FilePathNormalizer.AreDirectoryPathsEquivalent(Id, project.CompilationOutputInfo.AssemblyPath);
    }
}
