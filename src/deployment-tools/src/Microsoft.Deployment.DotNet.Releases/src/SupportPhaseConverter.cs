// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Custom converter for <see cref="SupportPhase"/> enumeration.
    /// </summary>
    internal class SupportPhaseConverter : JsonConverter<SupportPhase>
    {
        /// <summary>
        /// Reads a string value and converts it into a <see cref="SupportPhase"/> enumeration.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to use for reading the object value.</param>
        /// <param name="objectType">The type of the object.</param>
        /// <param name="existingValue">The existing value of the object.</param>
        /// <param name="hasExistingValue">The existing value of the object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>A <see cref="SupportPhase"/> created from the object value.</returns>
        public override SupportPhase ReadJson(
            JsonReader reader, Type objectType, SupportPhase existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var tokenValue = reader.Value.ToString();
                if (!string.IsNullOrWhiteSpace(tokenValue))
                {
                    return Enum.TryParse(tokenValue, ignoreCase: true, out SupportPhase result)
                        ? result
                        : SupportPhase.Unknown;
                }
            }

            return SupportPhase.Unknown;
        }

        /// <summary>
        /// Converts the specified <see cref="SupportPhase"/> to a string 
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to use for writing the value.</param>
        /// <param name="value">The <see cref="SupportPhase"/> to write.</param>
        /// <param name="serializer">The calling serializer</param>
        public override void WriteJson(JsonWriter writer, SupportPhase value, JsonSerializer serializer) =>
            throw new NotImplementedException();
    }
}
