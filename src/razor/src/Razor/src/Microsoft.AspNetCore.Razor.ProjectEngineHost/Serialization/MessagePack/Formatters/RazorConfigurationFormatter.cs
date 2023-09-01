// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class RazorConfigurationFormatter : MessagePackFormatter<RazorConfiguration>
{
    public static readonly MessagePackFormatter<RazorConfiguration> Instance = new RazorConfigurationFormatter();

    private RazorConfigurationFormatter()
    {
    }

    public override RazorConfiguration Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var configurationName = DeserializeString(ref reader, options);
        var languageVersionText = DeserializeString(ref reader, options);

        var count = reader.ReadArrayHeader();

        var extensions = count > 0
            ? ReadExtensions(ref reader, count, options)
            : Array.Empty<RazorExtension>();

        var languageVersion = RazorLanguageVersion.TryParse(languageVersionText, out var version)
            ? version
            : RazorLanguageVersion.Version_2_1;

        return RazorConfiguration.Create(languageVersion, configurationName, extensions);
    }

    private RazorExtension[] ReadExtensions(ref MessagePackReader reader, int count, MessagePackSerializerOptions options)
    {
        using var builder = new PooledArrayBuilder<RazorExtension>();

        for (var i = 0; i < count; i++)
        {
            var extensionName = DeserializeString(ref reader, options);
            builder.Add(new SerializedRazorExtension(extensionName));
        }

        return builder.ToArray();
    }

    public override void Serialize(ref MessagePackWriter writer, RazorConfiguration value, MessagePackSerializerOptions options)
    {
        writer.Write(value.ConfigurationName);

        if (value.LanguageVersion == RazorLanguageVersion.Experimental)
        {
            writer.Write(nameof(RazorLanguageVersion.Experimental));
        }
        else
        {
            writer.Write(value.LanguageVersion.ToString());
        }

        var extensions = value.Extensions;
        var count = extensions.Count;

        writer.WriteArrayHeader(count);

        for (var i = 0; i < count; i++)
        {
            writer.Write(extensions[i].ExtensionName);
        }
    }
}
