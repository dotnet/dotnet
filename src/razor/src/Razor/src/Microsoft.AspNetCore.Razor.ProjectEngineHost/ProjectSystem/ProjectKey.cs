// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

/// <summary>
/// A very light wrapper around a file path, used to ensure consistency across the code base for what constitutes the unique
/// identifier for a project.
/// </summary>
[DebuggerDisplay("id: {Id}")]
internal readonly record struct ProjectKey
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
        => FilePathComparer.Instance.Equals(Id, other.Id);

    public override int GetHashCode()
        => IsUnknown ? 0 : FilePathComparer.Instance.GetHashCode(Id);
}
