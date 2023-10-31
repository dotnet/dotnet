// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests;

public static class SourceMappingsSerializer
{
    internal static string Serialize(IRazorGeneratedDocument csharpDocument, RazorSourceDocument sourceDocument)
    {
        using var _ = StringBuilderPool.GetPooledObject(out var builder);
        var sourceContent = sourceDocument.Text.ToString();

        foreach (var sourceMapping in csharpDocument.SourceMappings)
        {
            builder.Append("Source Location: ");
            AppendMappingLocation(builder, sourceMapping.OriginalSpan, sourceContent);

            builder.Append("Generated Location: ");
            AppendMappingLocation(builder, sourceMapping.GeneratedSpan, csharpDocument.GeneratedCode);

            builder.AppendLine();
        }

        return builder.ToString();
    }

    private static void AppendMappingLocation(StringBuilder builder, SourceSpan location, string content)
    {
        builder
            .AppendLine(location.ToString())
            .Append('|');

        for (var i = 0; i < location.Length; i++)
        {
            builder.Append(content[location.AbsoluteIndex + i]);
        }

        builder.AppendLine("|");
    }
}
