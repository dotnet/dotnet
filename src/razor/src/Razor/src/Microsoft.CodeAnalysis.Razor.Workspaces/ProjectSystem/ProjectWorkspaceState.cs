// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

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
