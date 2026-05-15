// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Buffers;
using System.Text;
using System.Text.Json;

namespace NuGet.Protocol.Converters
{
    internal static class Utf8JsonReaderExtensions
    {
        /// <summary>
        /// Reads the current JSON scalar token (String, Number, True, or False) as a string,
        /// coercing non-string types to their string representations — matching Newtonsoft.Json's
        /// <c>serializer.Deserialize&lt;string&gt;</c> and <c>JToken.Value&lt;string&gt;()</c> behavior.
        /// Throws <see cref="JsonException"/> for non-scalar tokens (including Null — handle that in the caller).
        /// </summary>
        internal static string CoerceScalarTokenToString(this ref Utf8JsonReader reader)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString()!,
                JsonTokenType.True => bool.TrueString,
                JsonTokenType.False => bool.FalseString,
                JsonTokenType.Number => reader.HasValueSequence
                    ? Encoding.UTF8.GetString(reader.ValueSequence.ToArray())
                    : Encoding.UTF8.GetString(reader.ValueSpan.ToArray()),
                _ => throw new JsonException(string.Format(System.Globalization.CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType))
            };
        }
    }
}
