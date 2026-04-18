// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using Newtonsoft.Json;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference.Serialization;

/// <summary>
/// Original Newtonsoft-based TestRunStatisticsConverter, extracted from main for comparison testing.
/// </summary>
internal class NewtonsoftTestRunStatisticsConverter : JsonConverter
{
    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        return objectType.Equals(typeof(TestRunStatistics));
    }

    /// <inheritdoc/>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return serializer.Deserialize<TestRunStatistics>(reader);
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}
