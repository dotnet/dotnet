// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="TestSessionInfo"/> that handles the private setter on <see cref="TestSessionInfo.Id"/>
/// by using reflection to set the value after construction.
/// </summary>
internal class TestSessionInfoConverter : JsonConverter<TestSessionInfo>
{
    /// <inheritdoc/>
    public override TestSessionInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var id = root.TryGetProperty("Id", out var idProp) && idProp.ValueKind != JsonValueKind.Null
            ? idProp.GetGuid()
            : Guid.NewGuid();

        var info = new TestSessionInfo();
        typeof(TestSessionInfo).GetProperty(nameof(TestSessionInfo.Id))!.SetValue(info, id);

        return info;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TestSessionInfo value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Id", value.Id);
        writer.WriteEndObject();
    }
}

#endif
