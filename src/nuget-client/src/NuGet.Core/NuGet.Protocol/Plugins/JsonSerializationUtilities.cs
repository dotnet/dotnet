// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// JSON serialization/deserialization utilities.
    /// </summary>
#if NET5_0_OR_GREATER
    [UnconditionalSuppressMessage("AOT", "IL2026", Justification = "Legacy Newtonsoft.Json infrastructure; methods on this class are already annotated with [RUC]/[RDC].")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Legacy Newtonsoft.Json infrastructure; methods on this class are already annotated with [RUC]/[RDC].")]
#endif
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
        /// <returns>An instance of <typeparamref name="T" />, or <see langword="null" /> if the JSON represents a null value.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="json" />
        /// is either <see langword="null" /> or an empty string.</exception>
#if NET5_0_OR_GREATER
        [RequiresUnreferencedCode("Uses Newtonsoft.Json reflection-based deserialization.")]
        [RequiresDynamicCode("Uses Newtonsoft.Json reflection-based deserialization.")]
#endif
        public static T? Deserialize<T>(string json)
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
#if NET5_0_OR_GREATER
        [RequiresUnreferencedCode("Uses Newtonsoft.Json reflection-based serialization.")]
        [RequiresDynamicCode("Uses Newtonsoft.Json reflection-based serialization.")]
#endif
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
#if NET5_0_OR_GREATER
        [RequiresUnreferencedCode("Uses Newtonsoft.Json reflection-based serialization.")]
        [RequiresDynamicCode("Uses Newtonsoft.Json reflection-based serialization.")]
#endif
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
        /// <returns>An instance of <typeparamref name="T" />, or <see langword="null" /> if the JSON represents a null value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jObject" /> is <see langword="null" />.</exception>
#if NET5_0_OR_GREATER
        [RequiresUnreferencedCode("Uses Newtonsoft.Json reflection-based deserialization.")]
        [RequiresDynamicCode("Uses Newtonsoft.Json reflection-based deserialization.")]
#endif
        public static T? ToObject<T>(JObject jObject)
        {
            if (jObject == null)
            {
                throw new ArgumentNullException(nameof(jObject));
            }

            return jObject.ToObject<T>(Serializer);
        }
    }
}
