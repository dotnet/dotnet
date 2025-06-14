// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

/// <summary>
/// A very light wrapper around a file path, used to ensure consistency across the code base for what constitutes the unique
/// identifier for a project.
/// </summary>
[DebuggerDisplay("id: {Id}")]
internal readonly record struct ProjectKey : IComparable<ProjectKey>
{
    public static ProjectKey Unknown { get; } = default;

    [MemberNotNullWhen(false, nameof(Id))]
    public bool IsUnknown => Id is null;

    public string Id { get; }

    public ProjectKey(string id)
    {
        Debug.Assert(!id.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase), "We expect the intermediate output path, not the project file");

        Id = FilePathNormalizer.NormalizeDirectory(id);
    }

    public bool Equals(ProjectKey other)
        => PathUtilities.OSSpecificPathComparer.Equals(Id, other.Id);

    public override int GetHashCode()
        => IsUnknown ? 0 : PathUtilities.OSSpecificPathComparer.GetHashCode(Id);

    public override string ToString()
        => IsUnknown ? "<Unknown Project>" : Id;

    public int CompareTo(ProjectKey other)
    {
        // Sort "unknown" project keys after other project keys.
        if (IsUnknown)
        {
            return other.IsUnknown ? 0 : 1;
        }
        else if (other.IsUnknown)
        {
            return -1;
        }

        return PathUtilities.OSSpecificPathComparer.Compare(Id, other.Id);
    }
}
