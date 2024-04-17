// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Xunit;

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
            var sourceCode = GetCodeForSpan(sourceMapping.OriginalSpan, sourceContent);
            AppendMappingLocation(builder, sourceMapping.OriginalSpan, sourceCode);

            builder.Append("Generated Location: ");
            var generatedCode = GetCodeForSpan(sourceMapping.GeneratedSpan, csharpDocument.GeneratedCode);
            AppendMappingLocation(builder, sourceMapping.GeneratedSpan, generatedCode);

            Assert.Equal(sourceCode, generatedCode);
            builder.AppendLine();
        }

        return builder.ToString();
    }

    private static void AppendMappingLocation(StringBuilder builder, SourceSpan location, string content)
    {
        builder
            .AppendLine(location.ToString())
            .Append('|')
            .Append(content)
            .AppendLine("|");
    }

    private static string GetCodeForSpan(SourceSpan location, string content)
    {
        return content[location.AbsoluteIndex..(location.AbsoluteIndex + location.Length)];
    }
}
