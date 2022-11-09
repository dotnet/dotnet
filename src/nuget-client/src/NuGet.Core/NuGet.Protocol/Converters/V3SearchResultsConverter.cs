// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NuGet.Protocol.Model;

namespace NuGet.Protocol.Converters
{
    internal class V3SearchResultsConverter : JsonConverter
    {
        private uint _take;

        public V3SearchResultsConverter(uint take)
        {
            _take = take;
        }

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(V3SearchResults);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType != typeof(V3SearchResults))
            {
                throw new InvalidOperationException();
            }

            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonException("Expected StartObject, found " + reader.TokenType);
            }

            var searchResults = new V3SearchResults();

            var finished = false;

            while (!finished)
            {
                reader.Read();

                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        switch ((string)reader.Value)
                        {
                            case "totalHits":
                                if (long.TryParse(reader.ReadAsString(), out var totalHits))
                                {
                                    searchResults.TotalHits = totalHits;
                                }
                                else
                                {
                                    throw new JsonException("totalHits should be a long integer");
                                }

                                break;

                            case "data":
                                reader.Read();

                                if (reader.TokenType != JsonToken.StartArray)
                                {
                                    throw new JsonException("data should be an array");
                                }

                                reader.Read();

                                while (reader.TokenType != JsonToken.EndArray)
                                {
                                    var searchResult = serializer.Deserialize<PackageSearchMetadata>(reader);

                                    searchResults.Data.Add(searchResult);

                                    if (searchResults.Data.Count >= _take)
                                    {
                                        finished = true;
                                        break;
                                    }

                                    reader.Read();
                                }

                                break;

                            default:
                                reader.Skip();
                                break;
                        }
                        break;

                    case JsonToken.EndObject:
                        finished = true;
                        break;

                    default:
                        throw new JsonException();
                }
            }

            return searchResults;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
