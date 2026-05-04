// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for converting ITestRunStatistics to TestRunStatistics.
/// Handles the private setter on Stats by deserializing into a JsonElement
/// and manually constructing the object.
/// </summary>
internal class TestRunStatisticsConverter : JsonConverter<ITestRunStatistics>
{
    /// <inheritdoc/>
    public override ITestRunStatistics? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        long executedTests = root.TryGetProperty("ExecutedTests", out var etProp) ? etProp.GetInt64() : 0;

        IDictionary<TestOutcome, long>? stats = null;
        if (root.TryGetProperty("Stats", out var statsProp) && statsProp.ValueKind == JsonValueKind.Object)
        {
            stats = new Dictionary<TestOutcome, long>();
            foreach (var kvp in statsProp.EnumerateObject())
            {
                if (Enum.TryParse<TestOutcome>(kvp.Name, out var outcome))
                {
                    stats[outcome] = kvp.Value.GetInt64();
                }
            }
        }

        return new TestRunStatistics(executedTests, stats);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ITestRunStatistics value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("ExecutedTests", value.ExecutedTests);
        writer.WritePropertyName("Stats");
        if (value is TestRunStatistics concrete && concrete.Stats is not null)
        {
            writer.WriteStartObject();
            foreach (var kvp in concrete.Stats)
            {
                writer.WriteNumber(kvp.Key.ToString(), kvp.Value);
            }
            writer.WriteEndObject();
        }
        else
        {
            writer.WriteNullValue();
        }
        writer.WriteEndObject();
    }
}

#endif
