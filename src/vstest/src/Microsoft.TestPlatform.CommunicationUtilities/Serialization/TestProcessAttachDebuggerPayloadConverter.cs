// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="TestProcessAttachDebuggerPayload"/> that handles the constructor
/// parameter name 'pid' not matching the property name 'ProcessID'.
/// </summary>
internal class TestProcessAttachDebuggerPayloadConverter : JsonConverter<TestProcessAttachDebuggerPayload>
{
    /// <inheritdoc/>
    public override TestProcessAttachDebuggerPayload? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var processId = root.TryGetProperty("ProcessID", out var pidProp) ? pidProp.GetInt32() : 0;
        var targetFramework = root.TryGetProperty("TargetFramework", out var tf) && tf.ValueKind != JsonValueKind.Null
            ? tf.GetString()
            : null;

        return new TestProcessAttachDebuggerPayload(processId) { TargetFramework = targetFramework };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TestProcessAttachDebuggerPayload value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("ProcessID", value.ProcessID);
        writer.WriteString("TargetFramework", value.TargetFramework);
        writer.WriteEndObject();
    }
}

#endif
