// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NuGet.ProjectModel
{
    internal static class JsonTextReaderExtensions
    {
        private static readonly char[] DelimitedStringDelimiters = new char[] { ' ', ',' };

        internal static IReadOnlyList<string> ReadDelimitedString(this JsonTextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (ReadNextToken(reader))
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                        var value = (string)reader.Value;

                        return value.Split(DelimitedStringDelimiters, StringSplitOptions.RemoveEmptyEntries);

                    default:
                        throw new InvalidCastException();
                }
            }

            return null;
        }

        internal static bool ReadNextToken(this JsonTextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            bool wasRead;

            while ((wasRead = reader.Read()) && reader.TokenType == JsonToken.Comment)
            {
            }

            return wasRead;
        }

        internal static string ReadNextTokenAsString(this JsonTextReader reader)
        {
            if (ReadNextToken(reader))
            {
                return ReadTokenAsString(reader);
            }

            return null;
        }

        internal static bool ReadObject(this JsonTextReader reader, Action<string> onProperty)
        {
            return ReadObject(reader, onProperty, out int _0, out int _1);
        }

        internal static bool ReadObject(this JsonTextReader reader, Action<string> onProperty, out int startObjectLine, out int startObjectColumn)
        {
            startObjectLine = 0;
            startObjectColumn = 0;

            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (onProperty == null)
            {
                throw new ArgumentNullException(nameof(onProperty));
            }

            if (ReadNextToken(reader) && reader.TokenType == JsonToken.StartObject)
            {
                startObjectLine = reader.LineNumber;
                startObjectColumn = reader.LinePosition;

                ReadProperties(reader, onProperty);

                return true;
            }

            return false;
        }

        internal static void ReadProperties(this JsonTextReader reader, Action<string> onProperty)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (onProperty == null)
            {
                throw new ArgumentNullException(nameof(onProperty));
            }

            while (ReadNextToken(reader) && reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = (string)reader.Value;

                int lineNumber = reader.LineNumber;
                int linePosition = reader.LinePosition;

                onProperty(propertyName);

                if (reader.LineNumber == lineNumber && reader.LinePosition == linePosition)
                {
                    reader.Skip();
                }
            }
        }

        internal static List<string> ReadStringArrayAsList(this JsonTextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            List<string> strings = null;

            if (ReadNextToken(reader) && reader.TokenType == JsonToken.StartArray)
            {
                while (ReadNextToken(reader) && reader.TokenType != JsonToken.EndArray)
                {
                    string value = ReadTokenAsString(reader);

                    strings = strings ?? new List<string>();

                    strings.Add(value);
                }
            }

            return strings;
        }

        internal static IReadOnlyList<string> ReadStringOrArrayOfStringsAsReadOnlyList(this JsonTextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (ReadNextToken(reader))
            {
                switch (reader.TokenType)
                {
                    case JsonToken.String:
                        return new[] { (string)reader.Value };

                    case JsonToken.StartArray:
                        return ReadStringArrayAsReadOnlyListFromArrayStart(reader);
                }
            }

            return null;
        }

        internal static IReadOnlyList<string> ReadStringArrayAsReadOnlyListFromArrayStart(this JsonTextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            List<string> strings = null;

            while (ReadNextToken(reader) && reader.TokenType != JsonToken.EndArray)
            {
                string value = ReadTokenAsString(reader);

                strings = strings ?? new List<string>();

                strings.Add(value);
            }

            return (IReadOnlyList<string>)strings ?? Array.Empty<string>();
        }

        private static string ReadTokenAsString(this JsonTextReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Boolean:
                case JsonToken.Float:
                case JsonToken.Integer:
                case JsonToken.String:
                    return reader.Value.ToString();

                case JsonToken.Null:
                    return null;

                default:
                    throw new InvalidCastException();
            }
        }
    }
}
