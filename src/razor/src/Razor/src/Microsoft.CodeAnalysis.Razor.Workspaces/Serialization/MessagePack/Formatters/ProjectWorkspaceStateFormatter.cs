// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

internal sealed class ProjectWorkspaceStateFormatter : ValueFormatter<ProjectWorkspaceState>
{
    public static readonly ValueFormatter<ProjectWorkspaceState> Instance = new ProjectWorkspaceStateFormatter();

    private ProjectWorkspaceStateFormatter()
    {
    }

    public override ProjectWorkspaceState Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        var tagHelpers = reader.Deserialize<TagHelperCollection>(options).AssumeNotNull();

        return ProjectWorkspaceState.Create(tagHelpers);
    }

    public override void Serialize(ref MessagePackWriter writer, ProjectWorkspaceState value, SerializerCachingOptions options)
    {
        writer.Serialize(value.TagHelpers, options);
    }
}
