// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json;

namespace OpenTelemetry.Exporter.OneCollector;

internal static class ThreadStorageHelper
{
    [ThreadStatic]
    private static Utf8JsonWriter? utf8JsonWriter;

    [ThreadStatic]
    private static CommonSchemaJsonSerializationState? commonSchemaJsonSerializationState;

    public static CommonSchemaJsonSerializationState GetCommonSchemaJsonSerializationState(string itemType, Stream stream)
    {
        var writer = utf8JsonWriter;
        if (writer == null)
        {
            writer = utf8JsonWriter = new(
                stream,
                new JsonWriterOptions { SkipValidation = true });
        }
        else
        {
            writer.Reset(stream);
        }

        var serializationState = commonSchemaJsonSerializationState;
        if (serializationState == null)
        {
            serializationState = commonSchemaJsonSerializationState ??= new(itemType, writer);
        }
        else
        {
            serializationState.Reset(itemType, writer);
        }

        return serializationState;
    }
}
