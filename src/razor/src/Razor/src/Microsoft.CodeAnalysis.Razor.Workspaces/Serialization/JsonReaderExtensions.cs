// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal static class JsonReaderExtensions
{
    public static bool ReadTokenAndAdvance(this JsonReader reader, JsonToken expectedTokenType, out object value)
    {
        value = reader.Value;
        return reader.TokenType == expectedTokenType && reader.Read();
    }

    public static void ReadProperties<TArg>(this JsonReader reader, Action<string, TArg> onProperty, TArg arg)
    {
        do
        {
            switch (reader.TokenType)
            {
                case JsonToken.PropertyName:
                    var propertyName = reader.Value.ToString();
                    onProperty(propertyName, arg);
                    break;
                case JsonToken.EndObject:
                    return;
            }
        } while (reader.Read());
    }

    public static TArg ReadProperties<TArg>(this JsonReader reader, Func<string, TArg, TArg> onProperty, TArg arg)
    {
        do
        {
            switch (reader.TokenType)
            {
                case JsonToken.PropertyName:
                    var propertyName = reader.Value.ToString();
                    arg = onProperty(propertyName, arg);
                    break;
                case JsonToken.EndObject:
                    return arg;
            }
        } while (reader.Read());

        return arg;
    }

    public static bool TryReadNextProperty<TReturn>(this JsonReader reader, string propertyName, out TReturn value)
    {
        do
        {
            switch (reader.TokenType)
            {
                case JsonToken.PropertyName:
                    // Ensures we're at the expected property & the reader
                    // can read the property value.
                    if (reader.Value.ToString() == propertyName &&
                        reader.Read())
                    {
                        value = (TReturn)reader.Value;
                        return true;
                    }
                    else
                    {
                        value = default;
                        return false;
                    }
            }
        } while (reader.Read());

        throw new JsonSerializationException($"Could not find string property '{propertyName}'.");
    }
}
