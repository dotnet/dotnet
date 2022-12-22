// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for the <see cref="TestObject"/> and derived entities.
/// </summary>
public class TestObjectConverter : JsonConverter
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

/// TODO: This is not used now, but I was experimenting with this quite a bit for performance, leaving it here in case I was wrong
/// and the serializer settings actually have signigicant impact on the speed.
/// <summary>
/// JSON converter for the <see cref="TestObject"/> and derived entities.
/// </summary>
internal class TestObjectConverter7 : JsonConverter
{
    // Empty is not present everywhere
#pragma warning disable CA1825 // Avoid zero-length array allocations
    private static readonly object[] EmptyObjectArray = new object[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations

    public TestObjectConverter7()
    {
        TestPropertyCtor = typeof(TestProperty).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
    }

    /// <inheritdoc/>
    public override bool CanRead => true;

    /// <inheritdoc/>
    public override bool CanWrite => false;

    public ConstructorInfo? TestPropertyCtor { get; }

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

        if (reader.TokenType != JsonToken.StartArray)
        {
            return new List<KeyValuePair<TestProperty, object>>();
        }

        var deserializedProperties = serializer.Deserialize<List<KeyValuePair<TestPropertyTemplate, JToken>>>(reader)!;
        // Initialize the list capacity to be the number of properties we might add.
        var propertyList = new List<KeyValuePair<TestProperty, object?>>(deserializedProperties.Count);

        // Every class that inherits from TestObject uses a properties store for <Property, Object>
        // key value pairs.
        foreach (var property in deserializedProperties)
        {
            // This call will fail with NRE on .NET Standard 1.3
            var testProperty = (TestProperty)TestPropertyCtor!.Invoke(EmptyObjectArray);
            testProperty.Id = property.Key.Id!;
            testProperty.Label = property.Key.Label!;
            testProperty.Category = property.Key.Category!;
            testProperty.Description = property.Key.Description!;
            testProperty.Attributes = (TestPropertyAttributes)property.Key.Attributes;
            testProperty.ValueType = property.Key.ValueType!;


            object? propertyData = null;
            JToken token = property.Value;
            if (token.Type != JTokenType.Null)
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

    private class TestPropertyTemplate
    {
        public string? Id { get; set; }
        public string? Label { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public int Attributes { get; set; }
        public string? ValueType { get; set; }
    }
}
