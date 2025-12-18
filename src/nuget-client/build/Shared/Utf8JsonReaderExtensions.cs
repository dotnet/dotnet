// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Text.Json;

namespace NuGet.Shared
{
    internal static class Utf8JsonReaderExtensions
    {
        internal static string ReadTokenAsString(this ref Utf8JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True:
                    return bool.TrueString;
                case JsonTokenType.False:
                    return bool.FalseString;
                case JsonTokenType.Number:
                    return reader.ReadNumberAsString();
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.None:
                case JsonTokenType.Null:
                    return null;
                default:
                    throw new InvalidCastException();
            }
        }

        private static string ReadNumberAsString(this ref Utf8JsonReader reader)
        {
            if (reader.TryGetInt64(out long value))
            {
                return value.ToString();
            }
            return reader.GetDouble().ToString();
        }
    }
}
