// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal class TagHelperResolutionResultJsonConverter : JsonConverter
{
    private readonly JsonSerializer _serializer;
    public static readonly TagHelperResolutionResultJsonConverter Instance = new TagHelperResolutionResultJsonConverter();

    public TagHelperResolutionResultJsonConverter()
    {
        _serializer = new JsonSerializer();
        _serializer.Converters.Add(TagHelperDescriptorJsonConverter.Instance);
        _serializer.Converters.Add(RazorDiagnosticJsonConverter.Instance);
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(TagHelperResolutionResult);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        // Verify expected object structure based on `WriteJson`
        if (reader.TokenType != JsonToken.StartObject)
        {
            return null;
        }

        var (_, _, descriptors, diagnostics) = reader.ReadProperties(static (propertyName, arg) =>
        {
            var (reader, serializer, descriptors, diagnostics) = (arg.reader, arg.serializer, arg.descriptors, arg.diagnostics);
            switch (propertyName)
            {
                case nameof(TagHelperResolutionResult.Descriptors):
                    if (reader.Read())
                    {
                        descriptors = serializer.Deserialize<TagHelperDescriptor[]>(reader);
                    }

                    break;
                case nameof(TagHelperResolutionResult.Diagnostics):
                    if (reader.Read())
                    {
                        diagnostics = serializer.Deserialize<RazorDiagnostic[]>(reader);
                    }

                    break;
            }

            return (reader, serializer, descriptors, diagnostics);
        }, (reader, serializer: _serializer, descriptors: Array.Empty<TagHelperDescriptor>(), diagnostics: Array.Empty<RazorDiagnostic>()));

        reader.ReadTokenAndAdvance(JsonToken.EndObject, out _);

        return new TagHelperResolutionResult(descriptors, diagnostics);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var result = (TagHelperResolutionResult)value;

        writer.WriteStartObject();

        writer.WritePropertyArray(nameof(TagHelperResolutionResult.Descriptors), result.Descriptors, _serializer);
        writer.WritePropertyArray(nameof(TagHelperResolutionResult.Diagnostics), result.Diagnostics, _serializer);

        writer.WriteEndObject();
    }
}
