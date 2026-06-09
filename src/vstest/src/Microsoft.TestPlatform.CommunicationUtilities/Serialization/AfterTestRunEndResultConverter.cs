// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="AfterTestRunEndResult"/> that handles the private parameterless
/// constructor and private property setters by using the public parameterized constructor.
/// </summary>
internal class AfterTestRunEndResultConverter : JsonConverter<AfterTestRunEndResult>
{
    /// <inheritdoc/>
    public override AfterTestRunEndResult? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var attachmentSets = DeserializeProperty<Collection<AttachmentSet>>(root, "AttachmentSets", options) ?? new Collection<AttachmentSet>();
        var invokedDataCollectors = DeserializeProperty<Collection<InvokedDataCollector>>(root, "InvokedDataCollectors", options);
        var metrics = DeserializeProperty<IDictionary<string, object>>(root, "Metrics", options) ?? new Dictionary<string, object>();

        return new AfterTestRunEndResult(attachmentSets, invokedDataCollectors, metrics);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, AfterTestRunEndResult value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProperty(writer, "AttachmentSets", value.AttachmentSets, options);
        WriteProperty(writer, "InvokedDataCollectors", value.InvokedDataCollectors, options);
        WriteProperty(writer, "Metrics", value.Metrics, options);
        writer.WriteEndObject();
    }

    private static T? DeserializeProperty<T>(JsonElement element, string name, JsonSerializerOptions options)
    {
        if (element.TryGetProperty(name, out var prop) && prop.ValueKind != JsonValueKind.Null)
        {
            return JsonSerializer.Deserialize<T>(prop.GetRawText(), options);
        }

        return default;
    }

    private static void WriteProperty<T>(Utf8JsonWriter writer, string name, T value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(name);
        JsonSerializer.Serialize(writer, value, options);
    }
}

#endif
