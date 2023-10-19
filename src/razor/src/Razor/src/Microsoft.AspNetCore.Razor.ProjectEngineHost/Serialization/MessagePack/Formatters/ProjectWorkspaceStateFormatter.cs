// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class ProjectWorkspaceStateFormatter : ValueFormatter<ProjectWorkspaceState>
{
    public static readonly ValueFormatter<ProjectWorkspaceState> Instance = new ProjectWorkspaceStateFormatter();

    private ProjectWorkspaceStateFormatter()
    {
    }

    public override ProjectWorkspaceState Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(3);

        var checksums = reader.Deserialize<ImmutableArray<Checksum>>(options);

        reader.ReadArrayHeaderAndVerify(checksums.Length);

        using var builder = new PooledArrayBuilder<TagHelperDescriptor>(capacity: checksums.Length);
        var cache = TagHelperCache.Default;

        foreach (var checksum in checksums)
        {
            if (!cache.TryGet(checksum, out var tagHelper))
            {
                tagHelper = TagHelperFormatter.Instance.Deserialize(ref reader, options);
                cache.TryAdd(checksum, tagHelper);
            }
            else
            {
                TagHelperFormatter.Instance.Skim(ref reader, options);
            }

            builder.Add(tagHelper);
        }

        var tagHelpers = builder.DrainToImmutable();
        var csharpLanguageVersion = (LanguageVersion)reader.ReadInt32();

        return new ProjectWorkspaceState(tagHelpers, csharpLanguageVersion);
    }

    public override void Serialize(ref MessagePackWriter writer, ProjectWorkspaceState value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(3);

        var checksums = value.TagHelpers.SelectAsArray(x => x.Checksum);

        writer.Serialize(checksums, options);
        writer.Serialize(value.TagHelpers, options);
        writer.Write((int)value.CSharpLanguageVersion);
    }
}
