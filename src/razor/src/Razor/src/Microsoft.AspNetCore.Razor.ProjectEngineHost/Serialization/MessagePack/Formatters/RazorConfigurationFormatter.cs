// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class RazorConfigurationFormatter : ValueFormatter<RazorConfiguration>
{
    private const int SerializerPropertyCount = 7;

    public static readonly ValueFormatter<RazorConfiguration> Instance = new RazorConfigurationFormatter();

    private RazorConfigurationFormatter()
    {
    }

    public override RazorConfiguration Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        // The count is the number of values + the number of extensions
        var count = reader.ReadArrayHeader();

        var configurationName = CachedStringFormatter.Instance.Deserialize(ref reader, options) ?? string.Empty;
        var languageVersionText = CachedStringFormatter.Instance.Deserialize(ref reader, options) ?? string.Empty;
        var csharpLanguageVersion = (LanguageVersion)reader.ReadInt32();
        var suppressAddComponentParameter = reader.ReadBoolean();
        var useConsolidatedMvcViews = reader.ReadBoolean();
        var useRoslynTokenizer = reader.ReadBoolean();
        var preprocessorSymbols = reader.Deserialize<ImmutableArray<string>>(options);

        count -= SerializerPropertyCount;

        using var builder = new PooledArrayBuilder<RazorExtension>();

        for (var i = 0; i < count; i++)
        {
            var extensionName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
            builder.Add(new RazorExtension(extensionName));
        }

        var extensions = builder.DrainToImmutable();

        var languageVersion = RazorLanguageVersion.TryParse(languageVersionText, out var version)
            ? version
            : RazorLanguageVersion.Version_2_1;

        return new(
            languageVersion,
            configurationName,
            extensions,
            csharpLanguageVersion,
            UseConsolidatedMvcViews: useConsolidatedMvcViews,
            SuppressAddComponentParameter: suppressAddComponentParameter,
            UseRoslynTokenizer: useRoslynTokenizer,
            PreprocessorSymbols: preprocessorSymbols);
    }

    public override void Serialize(ref MessagePackWriter writer, RazorConfiguration value, SerializerCachingOptions options)
    {
        // Write SerializerPropertyCount values + 1 value per extension.
        var extensions = value.Extensions;
        var count = extensions.Length + SerializerPropertyCount;

        writer.WriteArrayHeader(count);

        CachedStringFormatter.Instance.Serialize(ref writer, value.ConfigurationName, options);

        if (value.LanguageVersion == RazorLanguageVersion.Experimental)
        {
            CachedStringFormatter.Instance.Serialize(ref writer, nameof(RazorLanguageVersion.Experimental), options);
        }
        else
        {
            CachedStringFormatter.Instance.Serialize(ref writer, value.LanguageVersion.ToString(), options);
        }

        writer.Write((int)value.CSharpLanguageVersion);
        writer.Write(value.SuppressAddComponentParameter);
        writer.Write(value.UseConsolidatedMvcViews);
        writer.Write(value.UseRoslynTokenizer);
        writer.Serialize(value.PreprocessorSymbols, options);

        count -= SerializerPropertyCount;

        for (var i = 0; i < count; i++)
        {
            CachedStringFormatter.Instance.Serialize(ref writer, extensions[i].ExtensionName, options);
        }
    }
}
