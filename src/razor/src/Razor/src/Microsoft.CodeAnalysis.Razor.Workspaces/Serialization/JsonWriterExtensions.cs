// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal static class JsonWriterExtensions
{
    public static void WritePropertyArray<T>(this JsonWriter writer, string propertyName, IEnumerable<T> collection, JsonSerializer serializer)
    {
        writer.WritePropertyName(propertyName);
        writer.WriteStartArray();
        foreach (var item in collection)
        {
            serializer.Serialize(writer, item);
        }

        writer.WriteEndArray();
    }
}
