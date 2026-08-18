#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.TestUtils
{
    /// <summary>
    /// Custom Json converter for <see cref="SecurityKey"/>.
    /// </summary>
    public class SecurityKeyConverterWithTypeDiscriminator : JsonConverter<SecurityKey>
    {
        enum TypeDiscriminator
        {
            CustomKey = 1
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert) =>
            typeof(SecurityKey).IsAssignableFrom(typeToConvert);

        /// <inheritdoc/>
        public override SecurityKey Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader readerClone = reader;

            if (readerClone.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var propertyName = readerClone.GetString();
            if (propertyName != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            TypeDiscriminator typeDiscriminator = (TypeDiscriminator)readerClone.GetInt32();
            SecurityKey securityKey = typeDiscriminator switch
            {
                TypeDiscriminator.CustomKey => JsonSerializer.Deserialize<CustomSecurityKey>(ref reader)!,
                _ => throw new JsonException()
            };
            return securityKey;
        }

        /// <inheritdoc/>
        public override void Write(
            Utf8JsonWriter writer, SecurityKey securityKey, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (securityKey is CustomSecurityKey customKey)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.CustomKey);
            }

            writer.WriteNumber("KeySize", securityKey.KeySize);

            writer.WriteEndObject();
        }
    }
}
