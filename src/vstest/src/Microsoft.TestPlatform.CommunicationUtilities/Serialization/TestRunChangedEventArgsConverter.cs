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
/// JSON converter for <see cref="TestRunChangedEventArgs"/> that handles the constructor
/// parameter names not matching property names for STJ binding.
/// </summary>
internal class TestRunChangedEventArgsConverter : JsonConverter<TestRunChangedEventArgs>
{
    /// <inheritdoc/>
    public override TestRunChangedEventArgs? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var stats = DeserializeProperty<ITestRunStatistics>(root, "TestRunStatistics", options);
        var newTestResults = DeserializeProperty<IEnumerable<TestResult>>(root, "NewTestResults", options);
        var activeTests = DeserializeProperty<IEnumerable<TestCase>>(root, "ActiveTests", options);

        return new TestRunChangedEventArgs(stats, newTestResults, activeTests);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TestRunChangedEventArgs value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProperty(writer, "NewTestResults", value.NewTestResults, options);
        WriteProperty(writer, "TestRunStatistics", value.TestRunStatistics, options);
        WriteProperty(writer, "ActiveTests", value.ActiveTests, options);
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
