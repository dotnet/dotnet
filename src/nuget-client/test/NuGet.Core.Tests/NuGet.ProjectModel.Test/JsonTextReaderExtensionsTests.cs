// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    [UseCulture("")] // Fix tests failing on systems with non-English locales
    public class JsonTextReaderExtensionsTests
    {
        [Fact]
        public void ReadDelimitedString_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadDelimitedString(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ReadDelimitedString_WhenValueIsNull_Throws()
        {
            const string json = "{\"a\":null}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                Assert.Throws<InvalidCastException>(() => test.Reader.ReadDelimitedString());
                Assert.Equal(JsonToken.Null, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("true", JsonToken.Boolean)]
        [InlineData("-2", JsonToken.Integer)]
        [InlineData("3.14", JsonToken.Float)]
        [InlineData("{}", JsonToken.StartObject)]
        public void ReadDelimitedString_WhenValueIsNotString_Throws(string value, JsonToken expectedTokenType)
        {
            var json = $"{{\"a\":{value}}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                Assert.Throws<InvalidCastException>(() => test.Reader.ReadDelimitedString());
                Assert.Equal(expectedTokenType, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadDelimitedString_WhenValueIsString_ReturnsValue()
        {
            const string expectedResult = "b";
            var json = $"{{\"a\":\"{expectedResult}\"}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadDelimitedString();

                Assert.Collection(actualResults, actualResult => Assert.Equal(expectedResult, actualResult));
                Assert.Equal(JsonToken.String, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("b,c,d")]
        [InlineData("b c d")]
        public void ReadDelimitedString_WhenValueIsDelimitedString_ReturnsValues(string value)
        {
            string[] expectedResults = value.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var json = $"{{\"a\":\"{value}\"}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadDelimitedString();

                Assert.Equal(expectedResults, actualResults);
                Assert.Equal(JsonToken.String, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadDelimitedString_WhenValueIsEmptyArray_Throws()
        {
            const string json = "{\"a\":[]}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                Assert.Throws<InvalidCastException>(() => test.Reader.ReadDelimitedString());
                Assert.Equal(JsonToken.StartArray, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("true")]
        [InlineData("-2")]
        [InlineData("3.14")]
        public void ReadDelimitedString_WhenValueIsConvertibleToString_Throws(string value)
        {
            var json = $"{{\"a\":[{value}]}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                Assert.Throws<InvalidCastException>(() => test.Reader.ReadDelimitedString());
                Assert.Equal(JsonToken.StartArray, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadDelimitedString_WhenValueIsArrayOfStrings_Throws()
        {
            string[] expectedResults = { "b", "c" };
            var json = $"{{\"a\":[{string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""))}]}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                Assert.Throws<InvalidCastException>(() => test.Reader.ReadDelimitedString());
                Assert.Equal(JsonToken.StartArray, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadNextToken_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadNextToken(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ReadNextToken_WhenAtEndOfStream_ReturnsFalse()
        {
            using (var test = new Test())
            {
                Assert.False(test.Reader.Read());
                Assert.False(test.Reader.ReadNextToken());
                Assert.Equal(JsonToken.None, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadNextToken_WhenNextTokenIsComment_SkipsComment()
        {
            using (var test = new Test("[/**/3]"))
            {
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.StartArray, test.Reader.TokenType);

                Assert.True(test.Reader.ReadNextToken());
                Assert.Equal(JsonToken.Integer, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadNextTokenAsString_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadNextTokenAsString(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ReadNextTokenAsString_WhenValueIsComment_SkipsComment()
        {
            using (var test = new Test("{\"a\":/**/\"b\""))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                string actualResult = test.Reader.ReadNextTokenAsString();

                Assert.Equal("b", actualResult);
                Assert.Equal(JsonToken.String, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadNextTokenAsString_WhenValueIsNone_ReturnsNull()
        {
            using (var test = new Test())
            {
                Assert.False(test.Reader.Read());
                Assert.Equal(JsonToken.None, test.Reader.TokenType);

                string actualResult = test.Reader.ReadNextTokenAsString();

                Assert.Null(actualResult);
            }
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("true", "True")]
        [InlineData("-2", "-2")]
        [InlineData("3.14", "3.14")]
        [InlineData("\"b\"", "b")]
        public void ReadNextTokenAsString_WhenValueIsConvertibleToString_ReturnsValueAsString(
            string value,
            string expectedResult)
        {
            var json = $"{{\"a\":{value}}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                string actualResult = test.Reader.ReadNextTokenAsString();

                Assert.Equal(expectedResult, actualResult);
            }
        }

        [Theory]
        [InlineData("[]")]
        [InlineData("{}")]
        public void ReadNextTokenAsString_WhenValueIsNotConvertibleToString_Throws(string value)
        {
            var json = $"{{\"a\":{value}}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                Assert.Throws<InvalidCastException>(() => test.Reader.ReadNextTokenAsString());
            }
        }

        [Fact]
        public void ReadObject_WithReaderAndOnProperty_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadObject(reader: null, onProperty: _ => { }));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ReadObject_WithReaderAndOnProperty_WhenOnPropertyIsNull_Throws()
        {
            using (var test = new Test())
            {
                ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                    () => JsonTextReaderExtensions.ReadObject(test.Reader, onProperty: null));

                Assert.Equal("onProperty", exception.ParamName);
            }
        }

        [Fact]
        public void ReadObject_WithReaderAndOnProperty_WhenNextTokenIsObject_EnumeratesProperties()
        {
            using (var test = new Test("{\"a\":3,\"b\":true}"))
            {
                var propertyNames = new List<string>();

                test.Reader.ReadObject(propertyName =>
                {
                    propertyNames.Add(propertyName);

                    switch (propertyName)
                    {
                        case "a":
                            {
                                int? actualValue = test.Reader.ReadAsInt32();

                                Assert.Equal(3, actualValue);
                            }
                            break;

                        case "b":
                            {
                                bool? actualValue = test.Reader.ReadAsBoolean();

                                Assert.True(actualValue);
                            }
                            break;
                    }
                });

                Assert.Collection(
                    propertyNames,
                    propertyName => Assert.Equal("a", propertyName),
                    propertyName => Assert.Equal("b", propertyName));
                Assert.Equal(JsonToken.EndObject, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadObject_WithReaderAndOnProperty_WhenPropertyValueIsNotRead_SkipsPropertyValue()
        {
            using (var test = new Test("{\"a\":3,\"b\":true}"))
            {
                var propertyNames = new List<string>();

                test.Reader.ReadObject(propertyName =>
                {
                    propertyNames.Add(propertyName);
                });

                Assert.Collection(
                    propertyNames,
                    propertyName => Assert.Equal("a", propertyName),
                    propertyName => Assert.Equal("b", propertyName));
                Assert.Equal(JsonToken.EndObject, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadObject_WithReaderAndOnPropertyAndStartObjectLineAndStartObjectColumn_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadObject(
                    reader: null,
                    onProperty: _ => { },
                    out int startObjectLine,
                    out int startObjectColumn));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ReadObject_WithReaderAndOnPropertyAndStartObjectLineAndStartObjectColumn_WhenOnPropertyIsNull_Throws()
        {
            using (var test = new Test())
            {
                ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                    () => JsonTextReaderExtensions.ReadObject(
                        test.Reader,
                        onProperty: null,
                        out int startObjectLine,
                        out int startObjectColum));

                Assert.Equal("onProperty", exception.ParamName);
            }
        }

        [Fact]
        public void ReadObject_WithReaderAndOnPropertyAndStartObjectLineAndStartObjectColumn_WhenNextTokenIsObject_EnumeratesProperties()
        {
            using (var test = new Test("{\"a\":3,\"b\":true}"))
            {
                var propertyNames = new List<string>();

                test.Reader.ReadObject(
                    propertyName =>
                    {
                        propertyNames.Add(propertyName);

                        switch (propertyName)
                        {
                            case "a":
                                {
                                    int? actualValue = test.Reader.ReadAsInt32();

                                    Assert.Equal(3, actualValue);
                                }
                                break;

                            case "b":
                                {
                                    bool? actualValue = test.Reader.ReadAsBoolean();

                                    Assert.True(actualValue);
                                }
                                break;
                        }
                    },
                    out int startObjectLine,
                    out int startObjectColumn);

                Assert.Collection(
                    propertyNames,
                    propertyName => Assert.Equal("a", propertyName),
                    propertyName => Assert.Equal("b", propertyName));
                Assert.Equal(1, startObjectLine);
                Assert.Equal(1, startObjectColumn);
                Assert.Equal(JsonToken.EndObject, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadObject_WithReaderAndOnPropertyAndStartObjectLineAndStartObjectColumn_WhenPropertyValueIsNotRead_SkipsPropertyValue()
        {
            using (var test = new Test("{\"a\":3,\"b\":true}"))
            {
                var propertyNames = new List<string>();

                test.Reader.ReadObject(
                    propertyName =>
                    {
                        propertyNames.Add(propertyName);
                    },
                    out int startObjectLine,
                    out int startObjectColumn);

                Assert.Collection(
                    propertyNames,
                    propertyName => Assert.Equal("a", propertyName),
                    propertyName => Assert.Equal("b", propertyName));
                Assert.Equal(1, startObjectLine);
                Assert.Equal(1, startObjectColumn);
                Assert.Equal(JsonToken.EndObject, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadProperties_WithReaderAndOnProperty_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadProperties(reader: null, onProperty: _ => { }));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ReadProperties_WithReaderAndOnProperty_WhenOnPropertyIsNull_Throws()
        {
            using (var test = new Test())
            {
                ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                    () => JsonTextReaderExtensions.ReadProperties(test.Reader, onProperty: null));

                Assert.Equal("onProperty", exception.ParamName);
            }
        }

        [Fact]
        public void ReadProperties_WithReaderAndOnProperty_WhenNextTokenIsPropertyName_EnumeratesProperties()
        {
            using (var test = new Test("{\"a\":3,\"b\":true}"))
            {
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.StartObject, test.Reader.TokenType);

                var propertyNames = new List<string>();

                test.Reader.ReadProperties(propertyName =>
                {
                    propertyNames.Add(propertyName);

                    switch (propertyName)
                    {
                        case "a":
                            {
                                int? actualValue = test.Reader.ReadAsInt32();

                                Assert.Equal(3, actualValue);
                            }
                            break;

                        case "b":
                            {
                                bool? actualValue = test.Reader.ReadAsBoolean();

                                Assert.True(actualValue);
                            }
                            break;
                    }
                });

                Assert.Collection(
                    propertyNames,
                    propertyName => Assert.Equal("a", propertyName),
                    propertyName => Assert.Equal("b", propertyName));
                Assert.Equal(JsonToken.EndObject, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadProperties_WithReaderAndOnProperty_WhenPropertyValueIsNotRead_SkipsPropertyValue()
        {
            using (var test = new Test("{\"a\":3,\"b\":true}"))
            {
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.StartObject, test.Reader.TokenType);

                var propertyNames = new List<string>();

                test.Reader.ReadProperties(propertyName =>
                {
                    propertyNames.Add(propertyName);
                });

                Assert.Collection(
                    propertyNames,
                    propertyName => Assert.Equal("a", propertyName),
                    propertyName => Assert.Equal("b", propertyName));
                Assert.Equal(JsonToken.EndObject, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadStringArrayAsList_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadStringArrayAsList(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Theory]
        [InlineData("null")]
        [InlineData("\"b\"")]
        [InlineData("{}")]
        public void ReadStringArrayAsList_WhenValueIsNotArray_ReturnsNull(string value)
        {
            using (var test = new Test($"{{\"a\":{value}}}"))
            {
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.StartObject, test.Reader.TokenType);
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                List<string> actualValues = test.Reader.ReadStringArrayAsList();

                Assert.Null(actualValues);
            }
        }

        [Fact]
        public void ReadStringArrayAsList_WhenValueIsEmptyArray_ReturnsNull()
        {
            using (var test = new Test("{\"a\":[]}"))
            {
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.StartObject, test.Reader.TokenType);
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                List<string> actualValues = test.Reader.ReadStringArrayAsList();

                Assert.Null(actualValues);
            }
        }

        [Fact]
        public void ReadStringArrayAsList_WithSupportedTypes_ReturnsStringArray()
        {
            using (var test = new Test("[\"a\",-2,3.14,true,null]"))
            {
                List<string> actualValues = test.Reader.ReadStringArrayAsList();

                Assert.Collection(
                    actualValues,
                    actualValue => Assert.Equal("a", actualValue),
                    actualValue => Assert.Equal("-2", actualValue),
                    actualValue => Assert.Equal("3.14", actualValue),
                    actualValue => Assert.Equal("True", actualValue),
                    actualValue => Assert.Null(actualValue));
                Assert.Equal(JsonToken.EndArray, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("[]")]
        [InlineData("{}")]
        public void ReadStringArrayAsList_WithUnsupportedTypes_Throws(string element)
        {
            using (var test = new Test($"[{element}]"))
            {
                Assert.Throws<InvalidCastException>(() => test.Reader.ReadStringArrayAsList());
            }
        }

        [Fact]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadStringOrArrayOfStringsAsReadOnlyList(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenValueIsNull_ReturnsNull()
        {
            const string json = "{\"a\":null}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadStringOrArrayOfStringsAsReadOnlyList();

                Assert.Null(actualResults);
                Assert.Equal(JsonToken.Null, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("true", JsonToken.Boolean)]
        [InlineData("-2", JsonToken.Integer)]
        [InlineData("3.14", JsonToken.Float)]
        [InlineData("{}", JsonToken.StartObject)]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenValueIsNotString_ReturnsNull(
            string value,
            JsonToken expectedTokenType)
        {
            var json = $"{{\"a\":{value}}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadStringOrArrayOfStringsAsReadOnlyList();

                Assert.Null(actualResults);
                Assert.Equal(expectedTokenType, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenValueIsString_ReturnsValue()
        {
            const string expectedResult = "b";
            var json = $"{{\"a\":\"{expectedResult}\"}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadStringOrArrayOfStringsAsReadOnlyList();

                Assert.Collection(actualResults, actualResult => Assert.Equal(expectedResult, actualResult));
                Assert.Equal(JsonToken.String, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("b,c,d")]
        [InlineData("b c d")]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenValueIsDelimitedString_ReturnsValue(string expectedResult)
        {
            var json = $"{{\"a\":\"{expectedResult}\"}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadStringOrArrayOfStringsAsReadOnlyList();

                Assert.Collection(actualResults, actualResult => Assert.Equal(expectedResult, actualResult));
                Assert.Equal(JsonToken.String, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenValueIsEmptyArray_ReturnsEmptyList()
        {
            const string json = "{\"a\":[]}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IReadOnlyList<string> actualResults = test.Reader.ReadStringOrArrayOfStringsAsReadOnlyList();

                Assert.Empty(actualResults);
                Assert.Equal(JsonToken.EndArray, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("true", "True")]
        [InlineData("-2", "-2")]
        [InlineData("3.14", "3.14")]
        [InlineData("\"b\"", "b")]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenValueIsConvertibleToString_ReturnsValueAsString(
            string value,
            string expectedResult)
        {
            var json = $"{{\"a\":[{value}]}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadStringOrArrayOfStringsAsReadOnlyList();

                Assert.Collection(actualResults, actualResult => Assert.Equal(expectedResult, actualResult));
                Assert.Equal(JsonToken.EndArray, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("[]", JsonToken.StartArray)]
        [InlineData("{}", JsonToken.StartObject)]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenValueIsNotConvertibleToString_ReturnsValueAsString(
            string value,
            JsonToken expectedToken)
        {
            var json = $"{{\"a\":[{value}]}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                Assert.Throws<InvalidCastException>(() => test.Reader.ReadStringOrArrayOfStringsAsReadOnlyList());
                Assert.Equal(expectedToken, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadStringOrArrayOfStringsAsReadOnlyList_WhenValueIsArrayOfStrings_ReturnsValues()
        {
            string[] expectedResults = { "b", "c" };
            var json = $"{{\"a\":[{string.Join(",", expectedResults.Select(expectedResult => $"\"{expectedResult}\""))}]}}";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.PropertyName, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadStringOrArrayOfStringsAsReadOnlyList();

                Assert.Equal(expectedResults, actualResults);
                Assert.Equal(JsonToken.EndArray, test.Reader.TokenType);
            }
        }

        [Fact]
        public void ReadStringArrayAsReadOnlyListFromArrayStart_WhenReaderIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => JsonTextReaderExtensions.ReadStringArrayAsReadOnlyListFromArrayStart(reader: null));

            Assert.Equal("reader", exception.ParamName);
        }

        [Fact]
        public void ReadStringArrayAsReadOnlyListFromArrayStart_WhenValuesAreConvertibleToString_ReturnsReadOnlyList()
        {
            const string json = "[null, true, -2, 3.14, \"a\"]";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.StartArray, test.Reader.TokenType);

                IEnumerable<string> actualResults = test.Reader.ReadStringArrayAsReadOnlyListFromArrayStart();

                Assert.Collection(
                    actualResults,
                    actualResult => Assert.Null(actualResult),
                    actualResult => Assert.Equal("True", actualResult),
                    actualResult => Assert.Equal("-2", actualResult),
                    actualResult => Assert.Equal("3.14", actualResult),
                    actualResult => Assert.Equal("a", actualResult));
                Assert.Equal(JsonToken.EndArray, test.Reader.TokenType);
            }
        }

        [Theory]
        [InlineData("[]", JsonToken.StartArray)]
        [InlineData("{}", JsonToken.StartObject)]
        public void ReadStringArrayAsReadOnlyListFromArrayStart_WhenValuesAreNotConvertibleToString_Throws(
            string value,
            JsonToken expectedToken)
        {
            var json = $"[{value}]";

            using (var test = new Test(json))
            {
                Assert.True(test.Reader.Read());
                Assert.Equal(JsonToken.StartArray, test.Reader.TokenType);

                Assert.Throws<InvalidCastException>(() => test.Reader.ReadStringArrayAsReadOnlyListFromArrayStart());
                Assert.Equal(expectedToken, test.Reader.TokenType);
            }
        }

        private sealed class Test : IDisposable
        {
            private bool _isDisposed;
            private readonly StringReader _stringReader;

            internal JsonTextReader Reader { get; }

            public Test()
                : this(string.Empty)
            {
            }

            public Test(string json)
            {
                _stringReader = new StringReader(json);
                Reader = new JsonTextReader(_stringReader);
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    ((IDisposable)Reader).Dispose();
                    _stringReader.Dispose();

                    _isDisposed = true;
                }
            }
        }
    }
}
