// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class RazorConfigurationFormatter : ValueFormatter<RazorConfiguration>
{
    public static readonly ValueFormatter<RazorConfiguration> Instance = new RazorConfigurationFormatter();

    private RazorConfigurationFormatter()
    {
    }

    public override RazorConfiguration Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        var count = reader.ReadArrayHeader();

        var configurationName = CachedStringFormatter.Instance.Deserialize(ref reader, options);
        var languageVersionText = CachedStringFormatter.Instance.Deserialize(ref reader, options);

        count -= 2;

        using var builder = new PooledArrayBuilder<RazorExtension>();

        for (var i = 0; i < count; i++)
        {
            var extensionName = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
            builder.Add(new SerializedRazorExtension(extensionName));
        }

        var extensions = builder.ToArray();

        var languageVersion = RazorLanguageVersion.TryParse(languageVersionText, out var version)
            ? version
            : RazorLanguageVersion.Version_2_1;

        return RazorConfiguration.Create(languageVersion, configurationName, extensions);
    }

    public override void Serialize(ref MessagePackWriter writer, RazorConfiguration value, SerializerCachingOptions options)
    {
        // Write two values + one value per extension.
        var extensions = value.Extensions;
        var count = extensions.Count + 2;

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

        count -= 2;

        for (var i = 0; i < count; i++)
        {
            CachedStringFormatter.Instance.Serialize(ref writer, extensions[i].ExtensionName, options);
        }
    }
}
