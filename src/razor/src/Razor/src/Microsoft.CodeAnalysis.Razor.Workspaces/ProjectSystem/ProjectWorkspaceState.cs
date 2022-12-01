// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public sealed class ProjectWorkspaceState : IEquatable<ProjectWorkspaceState>
{
    internal static readonly ProjectWorkspaceState Default = new(Array.Empty<TagHelperDescriptor>(), LanguageVersion.Default);

    public ProjectWorkspaceState(
        IReadOnlyCollection<TagHelperDescriptor> tagHelpers,
        LanguageVersion csharpLanguageVersion)
        : this((tagHelpers as IReadOnlyList<TagHelperDescriptor>) ?? tagHelpers.ToList(), csharpLanguageVersion)
    {
    }

    [JsonConstructor]
    public ProjectWorkspaceState(
        IReadOnlyList<TagHelperDescriptor> tagHelpers,
        LanguageVersion csharpLanguageVersion)
    {
        if (tagHelpers is null)
        {
            throw new ArgumentNullException(nameof(tagHelpers));
        }

        TagHelpers = tagHelpers;
        CSharpLanguageVersion = csharpLanguageVersion;
    }

    public IReadOnlyList<TagHelperDescriptor> TagHelpers { get; }

    public LanguageVersion CSharpLanguageVersion { get; }

    public override bool Equals(object? obj)
        => Equals(obj as ProjectWorkspaceState);

    public bool Equals(ProjectWorkspaceState? other)
        => other is not null &&
           TagHelpers.SequenceEqual(other.TagHelpers) &&
           CSharpLanguageVersion == other.CSharpLanguageVersion;

    public override int GetHashCode()
    {
        var hash = new HashCodeCombiner();

        foreach (var tagHelper in TagHelpers)
        {
            hash.Add(tagHelper.GetHashCode());
        }

        hash.Add(CSharpLanguageVersion);

        return hash.CombinedHash;
    }
}
