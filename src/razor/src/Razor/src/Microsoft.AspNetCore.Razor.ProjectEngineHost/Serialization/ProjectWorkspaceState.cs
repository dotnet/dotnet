// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

public sealed class ProjectWorkspaceState : IEquatable<ProjectWorkspaceState>
{
    public static readonly ProjectWorkspaceState Default = new(Array.Empty<TagHelperDescriptor>(), LanguageVersion.Default);

    public IReadOnlyList<TagHelperDescriptor> TagHelpers { get; }
    public LanguageVersion CSharpLanguageVersion { get; }

    public ProjectWorkspaceState(
        IReadOnlyCollection<TagHelperDescriptor> tagHelpers,
        LanguageVersion csharpLanguageVersion)
        : this((tagHelpers as IReadOnlyList<TagHelperDescriptor>) ?? tagHelpers.ToList(), csharpLanguageVersion)
    {
    }

    public ProjectWorkspaceState(
        IReadOnlyList<TagHelperDescriptor> tagHelpers,
        LanguageVersion csharpLanguageVersion)
    {
        TagHelpers = tagHelpers ?? throw new ArgumentNullException(nameof(tagHelpers));
        CSharpLanguageVersion = csharpLanguageVersion;
    }

    public override bool Equals(object? obj)
        => Equals(obj as ProjectWorkspaceState);

    public bool Equals(ProjectWorkspaceState? other)
        => other is not null &&
           TagHelpers.SequenceEqual(other.TagHelpers) &&
           CSharpLanguageVersion == other.CSharpLanguageVersion;

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();

        foreach (var tagHelper in TagHelpers)
        {
            hash.Add(tagHelper.GetHashCode());
        }

        hash.Add(CSharpLanguageVersion);

        return hash.CombinedHash;
    }
}
