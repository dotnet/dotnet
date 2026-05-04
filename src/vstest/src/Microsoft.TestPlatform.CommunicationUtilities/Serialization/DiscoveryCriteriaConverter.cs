// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="DiscoveryCriteria"/> that skips the computed <c>Sources</c> property
/// (marked with <c>[IgnoreDataMember]</c>) during serialization and populates <c>AdapterSourceMap</c>
/// and other private-setter properties via reflection during deserialization.
/// </summary>
internal class DiscoveryCriteriaConverter : JsonConverter<DiscoveryCriteria>
{
    /// <inheritdoc/>
    public override DiscoveryCriteria? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var adapterSourceMap = DeserializeProperty<Dictionary<string, IEnumerable<string>>>(root, "AdapterSourceMap", options);
        var frequency = DeserializeProperty<long>(root, "FrequencyOfDiscoveredTestsEvent", options);
        var timeout = DeserializeProperty<TimeSpan>(root, "DiscoveredTestEventTimeout", options);
        var runSettings = root.TryGetProperty("RunSettings", out var rs) && rs.ValueKind != JsonValueKind.Null ? rs.GetString() : null;
        var package = root.TryGetProperty("Package", out var pkg) && pkg.ValueKind != JsonValueKind.Null ? pkg.GetString() : null;
        var testCaseFilter = root.TryGetProperty("TestCaseFilter", out var tcf) && tcf.ValueKind != JsonValueKind.Null ? tcf.GetString() : null;
        var testSessionInfo = DeserializeProperty<TestSessionInfo>(root, "TestSessionInfo", options);

        var criteria = new DiscoveryCriteria();

        // Set private-setter properties via reflection.
        var type = typeof(DiscoveryCriteria);
        type.GetProperty(nameof(DiscoveryCriteria.AdapterSourceMap))!.SetValue(criteria, adapterSourceMap);
        type.GetProperty(nameof(DiscoveryCriteria.FrequencyOfDiscoveredTestsEvent))!.SetValue(criteria, frequency);
        type.GetProperty(nameof(DiscoveryCriteria.DiscoveredTestEventTimeout))!.SetValue(criteria, timeout);
        type.GetProperty(nameof(DiscoveryCriteria.RunSettings))!.SetValue(criteria, runSettings);

        criteria.Package = package;
        criteria.TestCaseFilter = testCaseFilter;
        criteria.TestSessionInfo = testSessionInfo;

        return criteria;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, DiscoveryCriteria value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Serialize [DataMember] properties in declaration order — skip Sources which has [IgnoreDataMember].
        WriteProperty(writer, "Package", value.Package, options);
        WriteProperty(writer, "AdapterSourceMap", value.AdapterSourceMap, options);
        writer.WriteNumber("FrequencyOfDiscoveredTestsEvent", value.FrequencyOfDiscoveredTestsEvent);
        WriteProperty(writer, "DiscoveredTestEventTimeout", value.DiscoveredTestEventTimeout, options);
        WriteProperty(writer, "RunSettings", value.RunSettings, options);
        WriteProperty(writer, "TestCaseFilter", value.TestCaseFilter, options);
        WriteProperty(writer, "TestSessionInfo", value.TestSessionInfo, options);

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
