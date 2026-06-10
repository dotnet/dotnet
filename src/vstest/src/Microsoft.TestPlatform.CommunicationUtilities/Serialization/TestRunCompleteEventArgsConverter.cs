// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="TestRunCompleteEventArgs"/> that handles the private parameterless
/// constructor and private property setters by using the public parameterized constructor.
/// </summary>
internal class TestRunCompleteEventArgsConverter : JsonConverter<TestRunCompleteEventArgs>
{
    /// <inheritdoc/>
    public override TestRunCompleteEventArgs? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var stats = DeserializeProperty<ITestRunStatistics>(root, "TestRunStatistics", options);
        var isCanceled = root.TryGetProperty("IsCanceled", out var ic) && ic.GetBoolean();
        var isAborted = root.TryGetProperty("IsAborted", out var ia) && ia.GetBoolean();
        var error = DeserializeProperty<Exception>(root, "Error", options);
        var attachmentSets = DeserializeProperty<Collection<AttachmentSet>>(root, "AttachmentSets", options) ?? new Collection<AttachmentSet>();
        var invokedDataCollectors = DeserializeProperty<Collection<InvokedDataCollector>>(root, "InvokedDataCollectors", options) ?? new Collection<InvokedDataCollector>();
        var elapsedTime = DeserializeProperty<TimeSpan>(root, "ElapsedTimeInRunningTests", options);

        var result = new TestRunCompleteEventArgs(stats, isCanceled, isAborted, error, attachmentSets, invokedDataCollectors, elapsedTime);
        result.Metrics = DeserializeProperty<IDictionary<string, object>>(root, "Metrics", options);
        result.DiscoveredExtensions = DeserializeProperty<Dictionary<string, HashSet<string>>>(root, "DiscoveredExtensions", options);

        return result;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TestRunCompleteEventArgs value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProperty(writer, "TestRunStatistics", value.TestRunStatistics, options);
        writer.WriteBoolean("IsCanceled", value.IsCanceled);
        writer.WriteBoolean("IsAborted", value.IsAborted);
        WriteProperty(writer, "Error", value.Error, options);
        WriteProperty(writer, "AttachmentSets", value.AttachmentSets, options);
        WriteProperty(writer, "InvokedDataCollectors", value.InvokedDataCollectors, options);
        WriteProperty(writer, "ElapsedTimeInRunningTests", value.ElapsedTimeInRunningTests, options);
        WriteProperty(writer, "Metrics", value.Metrics, options);
        WriteProperty(writer, "DiscoveredExtensions", value.DiscoveredExtensions, options);
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
