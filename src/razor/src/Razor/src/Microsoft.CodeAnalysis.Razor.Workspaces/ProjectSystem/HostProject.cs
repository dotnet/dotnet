// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal record class HostProject
{
    public ProjectKey Key { get; }

    /// <summary>
    /// Gets the full path to the .csproj file for this project
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Gets the full path to the folder under 'obj' where the project.razor.bin file will live
    /// </summary>
    public string IntermediateOutputPath { get; }

    public RazorConfiguration Configuration { get; init; }

    public string? RootNamespace { get; init; }

    /// <summary>
    /// An extra user-friendly string to show in the VS navigation bar to help the user, of the form "{ProjectFileName} ({Flavor})"
    /// </summary>
    public string DisplayName { get; init; }

    public HostProject(
        string filePath,
        string intermediateOutputPath,
        RazorConfiguration configuration,
        string? rootNamespace,
        string? displayName = null)
    {
        FilePath = filePath;
        IntermediateOutputPath = intermediateOutputPath;
        Configuration = configuration;
        RootNamespace = rootNamespace;
        DisplayName = displayName ?? Path.GetFileNameWithoutExtension(filePath);

        Key = new(intermediateOutputPath);
    }

    public virtual bool Equals(HostProject? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null &&
               EqualityContract == other.EqualityContract &&
               FilePathComparer.Instance.Equals(FilePath, other.FilePath) &&
               FilePathComparer.Instance.Equals(IntermediateOutputPath, other.IntermediateOutputPath) &&
               Configuration == other.Configuration &&
               RootNamespace == other.RootNamespace &&
               DisplayName == other.DisplayName;
    }

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();

        hash.Add(FilePath, FilePathComparer.Instance);
        hash.Add(IntermediateOutputPath, FilePathComparer.Instance);
        hash.Add(Configuration);
        hash.Add(RootNamespace);
        hash.Add(DisplayName);

        return hash.CombinedHash;
    }
}
