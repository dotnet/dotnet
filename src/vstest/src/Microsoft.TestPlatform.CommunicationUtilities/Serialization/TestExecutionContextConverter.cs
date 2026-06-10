// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="TestExecutionContext"/> that serializes only the [DataMember] properties,
/// matching the Newtonsoft DataContract serialization behavior by excluding [IgnoreDataMember] properties.
/// </summary>
internal class TestExecutionContextConverter : JsonConverter<TestExecutionContext>
{
    public override TestExecutionContext? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var data = doc.RootElement;

        var context = new TestExecutionContext();

        if (data.TryGetProperty("FrequencyOfRunStatsChangeEvent", out var freq))
            context.FrequencyOfRunStatsChangeEvent = freq.GetInt64();
        if (data.TryGetProperty("RunStatsChangeEventTimeout", out var timeout))
            context.RunStatsChangeEventTimeout = TimeSpan.Parse(timeout.GetString()!, System.Globalization.CultureInfo.InvariantCulture);
        if (data.TryGetProperty("InIsolation", out var isolation))
            context.InIsolation = isolation.GetBoolean();
        if (data.TryGetProperty("KeepAlive", out var keepAlive))
            context.KeepAlive = keepAlive.GetBoolean();
        if (data.TryGetProperty("AreTestCaseLevelEventsRequired", out var tcEvents))
            context.AreTestCaseLevelEventsRequired = tcEvents.GetBoolean();
        if (data.TryGetProperty("IsDebug", out var isDebug))
            context.IsDebug = isDebug.GetBoolean();
        if (data.TryGetProperty("TestCaseFilter", out var filter) && filter.ValueKind != JsonValueKind.Null)
            context.TestCaseFilter = filter.GetString();
        if (data.TryGetProperty("FilterOptions", out var filterOptions) && filterOptions.ValueKind != JsonValueKind.Null)
            context.FilterOptions = JsonSerializer.Deserialize<FilterOptions>(filterOptions.GetRawText(), options);

        return context;
    }

    public override void Write(Utf8JsonWriter writer, TestExecutionContext value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("FrequencyOfRunStatsChangeEvent", value.FrequencyOfRunStatsChangeEvent);
        writer.WriteString("RunStatsChangeEventTimeout", value.RunStatsChangeEventTimeout.ToString());
        writer.WriteBoolean("InIsolation", value.InIsolation);
        writer.WriteBoolean("KeepAlive", value.KeepAlive);
        writer.WriteBoolean("AreTestCaseLevelEventsRequired", value.AreTestCaseLevelEventsRequired);
        writer.WriteBoolean("IsDebug", value.IsDebug);
        writer.WriteString("TestCaseFilter", value.TestCaseFilter);
        if (value.FilterOptions is null)
        {
            writer.WriteNull("FilterOptions");
        }
        else
        {
            writer.WritePropertyName("FilterOptions");
            JsonSerializer.Serialize(writer, value.FilterOptions, options);
        }
        writer.WriteEndObject();
    }
}

#endif
