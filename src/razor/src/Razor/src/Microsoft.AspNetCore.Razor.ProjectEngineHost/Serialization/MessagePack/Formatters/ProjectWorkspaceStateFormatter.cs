// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class ProjectWorkspaceStateFormatter : MessagePackFormatter<ProjectWorkspaceState>
{
    public static readonly MessagePackFormatter<ProjectWorkspaceState> Instance = new ProjectWorkspaceStateFormatter();

    private ProjectWorkspaceStateFormatter()
    {
    }

    public override ProjectWorkspaceState Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        using var _ = TagHelperSerializationCache.Pool.GetPooledObject(out var cache);

        var tagHelpers = TagHelperFormatter.Instance.DeserializeImmutableArray(ref reader, options, cache);
        var csharpLanguageVersion = (LanguageVersion)reader.ReadInt32();

        return new ProjectWorkspaceState(tagHelpers, csharpLanguageVersion);
    }

    public override void Serialize(ref MessagePackWriter writer, ProjectWorkspaceState value, MessagePackSerializerOptions options)
    {
        using var _ = TagHelperSerializationCache.Pool.GetPooledObject(out var cache);

        TagHelperFormatter.Instance.SerializeArray(ref writer, value.TagHelpers, options, cache);
        writer.Write((int)value.CSharpLanguageVersion);
    }
}
