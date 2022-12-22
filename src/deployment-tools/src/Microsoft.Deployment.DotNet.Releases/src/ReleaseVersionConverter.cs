// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// A <see cref="JsonConverter"/> for a <see cref="ReleaseVersion"/> object.
    /// </summary>
    internal class ReleaseVersionConverter : JsonConverter<ReleaseVersion>
    {
        /// <summary>
        /// Converts the specified <see cref="ReleaseVersion"/> to a string 
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to use for writing the value.</param>
        /// <param name="value">The <see cref="ReleaseVersion"/> to write.</param>
        /// <param name="serializer">The calling serializer</param>
        public override void WriteJson(JsonWriter writer, ReleaseVersion value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());

        /// <summary>
        /// Reads a string value and converts it into a <see cref="ReleaseVersion"/> object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to use for reading the object value.</param>
        /// <param name="objectType">The type of the object.</param>
        /// <param name="existingValue">The existing value of the object.</param>
        /// <param name="hasExistingValue">The existing value of the object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>A <see cref="ReleaseVersion"/> created from the object value.</returns>
        public override ReleaseVersion ReadJson(
            JsonReader reader, Type objectType, ReleaseVersion existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var stringValue = (string)reader.Value;
            return string.IsNullOrWhiteSpace(stringValue) ? null : new ReleaseVersion(stringValue);
        }
    }
}
