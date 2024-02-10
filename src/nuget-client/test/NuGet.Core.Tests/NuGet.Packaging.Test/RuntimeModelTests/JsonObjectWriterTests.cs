// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace NuGet.RuntimeModel.Test
{
    public class JsonObjectWriterTests : IDisposable
    {
        private const string PropertyName = "a";

        private readonly StringWriter _stringWriter;
        private readonly JsonTextWriter _jsonWriter;
        private readonly JsonObjectWriter _writer;

        public JsonObjectWriterTests()
        {
            _stringWriter = new StringWriter();
            _jsonWriter = new JsonTextWriter(_stringWriter);
            _writer = new JsonObjectWriter(_jsonWriter);
        }

        public void Dispose()
        {
            _writer.Dispose();
            ((IDisposable)_jsonWriter).Dispose();
            _stringWriter.Dispose();
        }

        [Fact]
        public void Constructor_WhenWriterIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new JsonObjectWriter(writer: null));

            Assert.Equal("writer", exception.ParamName);
        }

        [Fact]
        public void Dispose_Always_IsIdempotent()
        {
            _writer.Dispose();
            _writer.Dispose();
        }

        [Fact]
        public void WriteObjectStart_WithNoParameters_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteObjectStart());
        }

        [Fact]
        public void WriteObjectStart_WithNoParameters_WhenCalled_WritesObjectStart()
        {
            Assert.Equal(string.Empty, _stringWriter.ToString());

            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());
        }

        [Fact]
        public void WriteObjectStart_WithWriter_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteObjectStart(name: null));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteObjectStart_WithWriter_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteObjectStart(PropertyName));
        }

        [Theory]
        [InlineData("")]
        [InlineData(PropertyName)]
        public void WriteObjectStart_WithValidName_WritesObjectStart(string name)
        {
            _writer.WriteObjectStart();
            _writer.WriteObjectStart(name);

            Assert.Equal($"{{\"{name}\":{{", _stringWriter.ToString());
        }

        [Fact]
        public void WriteObjectEnd_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteObjectEnd());
        }

        [Fact]
        public void WriteObjectEnd_WithoutObjectStart_Throws()
        {
            Assert.Throws<JsonWriterException>(() => _writer.WriteObjectEnd());
        }

        [Fact]
        public void WriteObjectEnd_WhenCalled_WritesObjectEnd()
        {
            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteObjectEnd();

            Assert.Equal("{}", _stringWriter.ToString());
        }

        [Fact]
        public void WriteArrayStart_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteArrayStart(PropertyName));
        }

        [Fact]
        public void WriteArrayStart_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteArrayStart(name: null));

            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(PropertyName)]
        public void WriteArrayStart_WithValidName_WritesArrayStart(string name)
        {
            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteArrayStart(name);

            Assert.Equal($"{{\"{name}\":[", _stringWriter.ToString());
        }

        [Fact]
        public void WriteArrayEnd_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteArrayEnd());
        }

        [Fact]
        public void WriteArrayEnd_WithoutArrayStart_Throws()
        {
            Assert.Throws<JsonWriterException>(() => _writer.WriteArrayEnd());
        }

        [Fact]
        public void WriteArrayEnd_WhenCalled_WritesArrayEnd()
        {
            _writer.WriteObjectStart();
            _writer.WriteArrayStart(PropertyName);

            Assert.Equal($"{{\"{PropertyName}\":[", _stringWriter.ToString());

            _writer.WriteArrayEnd();

            Assert.Equal($"{{\"{PropertyName}\":[]", _stringWriter.ToString());
        }

        [Fact]
        public void WriteNameValue_WithIntValue_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNameValue(name: null, value: 0));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNameValue_WithIntValue_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteNameValue(PropertyName, value: 0));
        }

        [Theory]
        [InlineData("", -1)]
        [InlineData(PropertyName, 1)]
        public void WriteNameValue_WithIntValue_WithValidName_WritesNameValue(string name, int value)
        {
            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteNameValue(name, value);

            Assert.Equal($"{{\"{name}\":{value}", _stringWriter.ToString());
        }

        [Fact]
        public void WriteNameValue_WithBoolValue_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNameValue(name: null, value: true));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNameValue_WithBoolValue_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteNameValue(PropertyName, value: true));
        }

        [Theory]
        [InlineData("", true)]
        [InlineData(PropertyName, false)]
        public void WriteNameValue_WithBoolValue_WithValidName_WritesNameValue(string name, bool value)
        {
            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteNameValue(name, value);

            Assert.Equal($"{{\"{name}\":{value.ToString().ToLower()}", _stringWriter.ToString());
        }

        [Fact]
        public void WriteNameValue_WithStringValue_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNameValue(name: null, value: "a"));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNameValue_WithStringValue_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteNameValue(PropertyName, value: "a"));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(PropertyName, "b")]
        public void WriteNameValue_WithStringValue_WithValidName_WritesNameValue(string name, string value)
        {
            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteNameValue(name, value);

            Assert.Equal($"{{\"{name}\":\"{value}\"", _stringWriter.ToString());
        }

        [Fact]
        public void WriteNameArray_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNameArray(name: null, values: new[] { "b", "c" }));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNameArray_WhenValuesIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNameArray(PropertyName, values: null));

            Assert.Equal("values", exception.ParamName);
        }

        [Fact]
        public void WriteNameArray_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(
                () => _writer.WriteNameArray(PropertyName, values: Enumerable.Empty<string>()));
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(PropertyName, "b")]
        public void WriteNameArray_WithValidValues_WritesNameArray(string name, string value)
        {
            IEnumerable<string> values = value == null ? Enumerable.Empty<string>() : new[] { value };

            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteNameArray(name, values);

            string stringValues = values.Any() ? $"\"{values.SingleOrDefault()}\"" : "";

            Assert.Equal($"{{\"{name}\":[{stringValues}]", _stringWriter.ToString());
        }

        [Fact]
        public void WriteNameArray_WithNullValue_WritesNameArray()
        {
            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteNameArray(PropertyName, new string[] { null });

            Assert.Equal($"{{\"{PropertyName}\":[null]", _stringWriter.ToString());
        }

        [Fact]
        public void WriteNonEmptyNameArray_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNonEmptyNameArray(name: null, values: new[] { "b", "c" }));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNonEmptyNameArray_WhenValuesIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNonEmptyNameArray(PropertyName, values: null));

            Assert.Equal("values", exception.ParamName);
        }

        [Fact]
        public void WriteNonEmptyNameArray_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(
                () => _writer.WriteNonEmptyNameArray(PropertyName, values: Enumerable.Empty<string>()));
        }

        [Theory]
        [InlineData("", null)]
        [InlineData(PropertyName, "b")]
        public void WriteNonEmptyNameArray_WithNonEmptyValidValues_WritesNameArray(string name, string value)
        {
            IEnumerable<string> values = value == null ? Enumerable.Empty<string>() : new[] { value };

            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteNonEmptyNameArray(name, values);

            var actualString = values.Any() ? $"{{\"{name}\":[\"{values.SingleOrDefault()}\"]" : "{";
            Assert.Equal(actualString, _stringWriter.ToString());
        }

        [Fact]
        public void WriteNonEmptyNameArray_WithNullValue_WritesNameArray()
        {
            _writer.WriteObjectStart();

            Assert.Equal("{", _stringWriter.ToString());

            _writer.WriteNonEmptyNameArray(PropertyName, new string[] { null });

            Assert.Equal($"{{\"{PropertyName}\":[null]", _stringWriter.ToString());
        }
    }
}
