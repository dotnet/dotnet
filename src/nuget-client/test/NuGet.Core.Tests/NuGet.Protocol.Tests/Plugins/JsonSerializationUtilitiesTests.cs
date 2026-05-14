// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class JsonSerializationUtilitiesTests
    {
        private readonly static A _expectedObject = new A() { B = 3 };
        private readonly static string _expectedJson = "{\"B\":3}";

        [Fact]
        public void Serializer_Converters()
        {
            var converters = JsonSerializationUtilities.Serializer.Converters;

            Assert.Contains(converters, converter => converter is SemanticVersionConverter);
            Assert.Contains(converters, converter => converter is StringEnumConverter);
        }

        [Fact]
        public void Serializer_Formatting()
        {
            Assert.Equal(Formatting.None, JsonSerializationUtilities.Serializer.Formatting);
        }

        [Fact]
        public void Serializer_NullValueHandling()
        {
            Assert.Equal(NullValueHandling.Ignore, JsonSerializationUtilities.Serializer.NullValueHandling);
        }

        [Fact]
        public void Deserialize_ThrowsForNullJson()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => JsonSerializationUtilities.Deserialize<A>(json: null));

            Assert.Equal("json", exception.ParamName);
        }

        [Fact]
        public void Deserialize_ThrowsForEmptyJson()
        {
            var exception = Assert.Throws<ArgumentException>(
                    () => JsonSerializationUtilities.Deserialize<A>(json: ""));

            Assert.Equal("json", exception.ParamName);
        }

        [Fact]
        public void Deserialize_ReturnsCorrectObject()
        {
            var a = JsonSerializationUtilities.Deserialize<A>(_expectedJson);

            Assert.Equal(3, a.B);
        }

        [Fact]
        public void FromObject_ThrowsForNullValue()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => JsonSerializationUtilities.FromObject(value: null));

            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void FromObject_ReturnsCorrectJObject()
        {
            var jObject = JsonSerializationUtilities.FromObject(_expectedObject);

            Assert.Equal(_expectedJson, jObject.ToString(Formatting.None));
        }

        [Fact]
        public void Serialize_ThrowsForNullWriter()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => JsonSerializationUtilities.Serialize(writer: null, value: _expectedObject));

            Assert.Equal("writer", exception.ParamName);
        }

        [Fact]
        public void Serialize_ReturnsCorrectJson()
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                JsonSerializationUtilities.Serialize(jsonWriter, _expectedObject);

                var json = stringWriter.ToString();

                Assert.Equal(_expectedJson, json);
            }
        }

        [Fact]
        public void ToObject_ThrowsForNullValue()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => JsonSerializationUtilities.ToObject<A>(jObject: null));

            Assert.Equal("jObject", exception.ParamName);
        }

        [Fact]
        public void ToObject_ReturnsCorrectObject()
        {
            var jObject = JObject.FromObject(_expectedObject);
            var a = JsonSerializationUtilities.ToObject<A>(jObject);

            Assert.Equal(3, a.B);
        }

        private sealed class A
        {
            public int B { get; set; }
        }
    }
}
