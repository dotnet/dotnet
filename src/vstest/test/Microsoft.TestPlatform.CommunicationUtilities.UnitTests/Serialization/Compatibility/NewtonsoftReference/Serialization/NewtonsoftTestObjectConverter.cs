// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.NewtonsoftReference.Serialization;

/// <summary>
/// Original Newtonsoft-based TestObjectConverter, extracted from main for comparison testing.
/// </summary>
internal class NewtonsoftTestObjectConverter : JsonConverter
{
    /// <inheritdoc/>
    public override bool CanRead => true;

    /// <inheritdoc/>
    public override bool CanWrite => false;

    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (objectType != typeof(List<KeyValuePair<TestProperty, object>>))
        {
            // Support only deserialization of KeyValuePair list
            throw new ArgumentException("the objectType was not a KeyValuePair list", nameof(objectType));
        }

        var propertyList = new List<KeyValuePair<TestProperty, object?>>();

        if (reader.TokenType != JsonToken.StartArray)
        {
            return propertyList;
        }

        var properties = JArray.Load(reader);
        if (properties == null || !properties.HasValues)
        {
            return propertyList;
        }

        // Every class that inherits from TestObject uses a properties store for <Property, Object>
        // key value pairs.
        foreach (var property in properties)
        {
            var testProperty = property?["Key"]?.ToObject<TestProperty>(serializer);

            if (testProperty == null)
            {
                continue;
            }

            // Let the null values be passed in as null data
            var token = property?["Value"];
            object? propertyData = null;
            if (token != null && token.Type != JTokenType.Null)
            {
                // If the property is already a string. No need to convert again.
                if (token.Type == JTokenType.String)
                {
                    propertyData = token.ToObject(typeof(string), serializer);
                }
                else
                {
                    // On deserialization, the value for each TestProperty is always a string. It is up
                    // to the consumer to deserialize it further as appropriate.
                    propertyData = token.ToString(Formatting.None).Trim('"');
                }
            }

            propertyList.Add(new KeyValuePair<TestProperty, object?>(testProperty, propertyData));
        }

        return propertyList;
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        // Create an array of <Property, Value> dictionary
    }
}
