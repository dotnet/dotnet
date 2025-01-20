// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal sealed class ProjectWorkspaceState : IEquatable<ProjectWorkspaceState>
{
    public static readonly ProjectWorkspaceState Default = new(ImmutableArray<TagHelperDescriptor>.Empty);

    public ImmutableArray<TagHelperDescriptor> TagHelpers { get; }

    private ProjectWorkspaceState(
        ImmutableArray<TagHelperDescriptor> tagHelpers)
    {
        TagHelpers = tagHelpers;
    }

    public static ProjectWorkspaceState Create(
        ImmutableArray<TagHelperDescriptor> tagHelpers)
        => tagHelpers.IsEmpty
            ? Default
            : new(tagHelpers);

    public override bool Equals(object? obj)
        => obj is ProjectWorkspaceState other && Equals(other);

    public bool Equals(ProjectWorkspaceState? other)
        => other is not null &&
           TagHelpers.SequenceEqual(other.TagHelpers);

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();

        hash.Add(TagHelpers);

        return hash.CombinedHash;
    }
}
