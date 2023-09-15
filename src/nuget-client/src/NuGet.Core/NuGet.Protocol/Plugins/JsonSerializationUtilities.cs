// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// JSON serialization/deserialization utilities.
    /// </summary>
    public static class JsonSerializationUtilities
    {
        /// <summary>
        /// Gets the JSON serializer.
        /// </summary>
        public static JsonSerializer Serializer { get; }

        static JsonSerializationUtilities()
        {
            Serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                Converters = new JsonConverter[]
                {
                    new SemanticVersionConverter(),
                    new StringEnumConverter(),
                    new VersionRangeConverter()
                },
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        /// Deserializes an object from the provided JSON.
        /// </summary>
        /// <typeparam name="T">The deserialization type.</typeparam>
        /// <param name="json">JSON to deserialize.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="json" />
        /// is either <see langword="null" /> or an empty string.</exception>
        public static T Deserialize<T>(string json)
            where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(json));
            }

            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                return Serializer.Deserialize<T>(jsonReader);
            }
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <param name="value">An object to serialize.</param>
        /// <returns>A <see cref="JObject" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <see langword="null" />.</exception>
        public static JObject FromObject(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return JObject.FromObject(value, Serializer);
        }

        /// <summary>
        /// Serializes an object to the provided writer.
        /// </summary>
        /// <param name="writer">A JSON writer.</param>
        /// <param name="value">The value to serialize.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <see langword="null" />.</exception>
        public static void Serialize(JsonWriter writer, object value)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            Serializer.Serialize(writer, value);
        }

        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <typeparam name="T">The deserialization type.</typeparam>
        /// <param name="jObject">A JSON object.</param>
        /// <returns>An instance of <typeparamref name="T" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jObject" /> is <see langword="null" />.</exception>
        public static T ToObject<T>(JObject jObject)
        {
            if (jObject == null)
            {
                throw new ArgumentNullException(nameof(jObject));
            }

            return jObject.ToObject<T>(Serializer);
        }
    }
}
