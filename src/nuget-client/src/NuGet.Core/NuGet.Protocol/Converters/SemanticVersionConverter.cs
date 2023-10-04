// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// A SemanticVersion JSON converter.
    /// </summary>
    public sealed class SemanticVersionConverter : JsonConverter
    {
        /// <summary>
        /// Gets a flag indicating whether or not a type is convertible.
        /// </summary>
        /// <param name="objectType">An object type to check.</param>
        /// <returns><see langword="true" /> if <paramref name="objectType" /> is convertible; otherwise <see langword="false" />.</returns>
        public override bool CanConvert(Type objectType) => objectType == typeof(SemanticVersion);

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">A JSON reader.</param>
        /// <param name="objectType">The type of the object.</param>
        /// <param name="existingValue">The existing value of the object.</param>
        /// <param name="serializer">A serializer.</param>
        /// <returns>A <see cref="SemanticVersion" /> object.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.TokenType != JsonToken.Null ? SemanticVersion.Parse(serializer.Deserialize<string>(reader)) : null;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">A JSON writer.</param>
        /// <param name="value">A value to serialize.</param>
        /// <param name="serializer">A serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var semanticVersion = (SemanticVersion)value;

            serializer.Serialize(writer, semanticVersion.ToString());
        }
    }
}
