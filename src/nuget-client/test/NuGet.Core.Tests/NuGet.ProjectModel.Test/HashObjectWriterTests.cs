// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Packaging;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class HashObjectWriterTests : IDisposable
    {
        private const string PropertyName = "a";

        private readonly IHashFunction _hashFunc;
        private readonly HashObjectWriter _writer;

        public HashObjectWriterTests()
        {
            _hashFunc = new FnvHash64Function();
            _writer = new HashObjectWriter(_hashFunc);
        }

        public void Dispose()
        {
            _writer.Dispose();
            _hashFunc.Dispose();
        }

        [Fact]
        public void Constructor_WhenHashFuncIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new HashObjectWriter(hashFunc: null));

            Assert.Equal("hashFunc", exception.ParamName);
        }

        [Fact]
        public void Dispose_Always_IsIdempotent()
        {
            _writer.Dispose();
            _writer.Dispose();
        }

        [Fact]
        public void WriteObjectStart_WithNoParameter_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteObjectStart());
        }

        [Fact]
        public void WriteObjectStart_WithNoParameter_WhenReadOnly_Throws()
        {
            MakeReadOnly();

            Assert.Throws<InvalidOperationException>(() => _writer.WriteObjectStart());
        }

        [Fact]
        public void WriteObjectStart_WithNoParameter_WhenCalled_WritesObjectStart()
        {
            _writer.WriteObjectStart();

            const string expectedHash = "uhgChkz2Y68=";
            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void WriteObjectStart_WithName_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteObjectStart(name: null));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteObjectStart_WithName_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteObjectStart(PropertyName));
        }

        [Fact]
        public void WriteObjectStart_WithName_WhenReadOnly_Throws()
        {
            MakeReadOnly();

            Assert.Throws<InvalidOperationException>(() => _writer.WriteObjectStart(PropertyName));
        }

        [Theory]
        [InlineData("", "1fyYPzTw+J4=")]
        [InlineData(PropertyName, "aEkDaamOy8A=")]
        public void WriteObjectStart_WithName_WithValidName_WritesObjectStart(string name, string expectedHash)
        {
            _writer.WriteObjectStart();
            _writer.WriteObjectStart(name);

            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void WriteObjectEnd_WhenDisposed_Throws()
        {
            _writer.WriteObjectStart();
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteObjectEnd());
        }

        [Fact]
        public void WriteObjectEnd_WhenReadOnly_Throws()
        {
            _writer.WriteObjectStart();

            MakeReadOnly();

            Assert.Throws<InvalidOperationException>(() => _writer.WriteObjectEnd());
        }

        [Fact]
        public void WriteObjectEnd_WithoutObjectStart_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _writer.WriteObjectEnd());
        }

        [Fact]
        public void WriteNameValue_WithIntValue_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNameValue(name: null, value: 3));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNameValue_WithIntValue_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteNameValue(PropertyName, value: 3));
        }

        [Fact]
        public void WriteNameValue_WithIntValue_WhenReadOnly_Throws()
        {
            MakeReadOnly();

            Assert.Throws<InvalidOperationException>(() => _writer.WriteNameValue(PropertyName, value: 3));
        }

        [Theory]
        [InlineData("", -1, "5vr8D8iI2Lg=")]
        [InlineData(PropertyName, 1, "dtICaalIy8A=")]
        public void WriteNameValue_WithIntValue_WithValidValue_WritesNameValue(string name, int value, string expectedHash)
        {
            _writer.WriteObjectStart();
            _writer.WriteNameValue(name, value);

            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
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

        [Fact]
        public void WriteNameValue_WithBoolValue_WhenReadOnly_Throws()
        {
            MakeReadOnly();

            Assert.Throws<InvalidOperationException>(() => _writer.WriteNameValue(PropertyName, value: true));
        }

        [Theory]
        [InlineData("", true, "rE+NQworCkw=")]
        [InlineData(PropertyName, false, "0rT5fJwG03I=")]
        public void WriteNameValue_WithBoolValue_WithValidValue_WritesNameValue(string name, bool value, string expectedHash)
        {
            _writer.WriteObjectStart();
            _writer.WriteNameValue(name, value);

            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void WriteNameValue_WithStringValue_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNameValue(name: null, value: "b"));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNameValue_WithStringValue_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteNameValue(PropertyName, value: "b"));
        }

        [Fact]
        public void WriteNameValue_WithStringValue_WhenReadOnly_Throws()
        {
            MakeReadOnly();

            Assert.Throws<InvalidOperationException>(() => _writer.WriteNameValue(PropertyName, value: "b"));
        }

        [Theory]
        [InlineData("", "", "OAojEMgFBbk=")]
        [InlineData(PropertyName, null, "ViPwxht58Ok=")]
        [InlineData(PropertyName, "b", "kwmmpEQYi7g=")]
        public void WriteNameValue_WithStringValue_WithValidValue_WritesNameValue(string name, string value, string expectedHash)
        {
            _writer.WriteObjectStart();
            _writer.WriteNameValue(name, value);

            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void WriteNameArray_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNameArray(name: null, values: new[] { "b" }));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNameArray_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteNameArray(PropertyName, new[] { "b" }));
        }

        [Fact]
        public void WriteNameArray_WhenReadOnly_Throws()
        {
            MakeReadOnly();

            Assert.Throws<InvalidOperationException>(() => _writer.WriteNameArray(PropertyName, new[] { "b" }));
        }

        [Theory]
        [InlineData("", null, "uFlSEcjwabo=")]
        [InlineData(PropertyName, "b", "Td9cKXXB7Gk=")]
        public void WriteNameArray_WithValidValues_WritesNameArray(string name, string value, string expectedHash)
        {
            IEnumerable<string> values = value == null ? Enumerable.Empty<string>() : new[] { value };

            _writer.WriteObjectStart();
            _writer.WriteNameArray(name, values);

            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void WriteNameArray_WithNullValue_WritesNameArray()
        {
            _writer.WriteObjectStart();
            _writer.WriteNameArray(PropertyName, new string[] { null });

            const string expectedHash = "6JqdqL+bz8M=";
            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void WriteNonEmptyNameArray_WhenNameIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => _writer.WriteNonEmptyNameArray(name: null, values: new[] { "b" }));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void WriteNonEmptyNameArray_WhenDisposed_Throws()
        {
            _writer.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _writer.WriteNonEmptyNameArray(PropertyName, new[] { "b" }));
        }

        [Fact]
        public void WriteNonEmptyNameArray_WhenReadOnly_Throws()
        {
            MakeReadOnly();

            Assert.Throws<InvalidOperationException>(() => _writer.WriteNonEmptyNameArray(PropertyName, new[] { "b" }));
        }

        [Theory]
        [InlineData(PropertyName, "b", "Td9cKXXB7Gk=")]
        public void WriteNonEmptyNameArray_WithValidValues_WritesNameArray(string name, string value, string expectedHash)
        {
            IEnumerable<string> values = value == null ? Enumerable.Empty<string>() : new[] { value };

            _writer.WriteObjectStart();
            _writer.WriteNonEmptyNameArray(name, values);

            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Theory]
        [InlineData(PropertyName, "b", "Td9cKXXB7Gk=")]
        public void WriteNonEmptyNameArray_WithEmptyValues_DoesNotWriteNameArray(string name, string value, string expectedHash)
        {
            IEnumerable<string> values = value == null ? Enumerable.Empty<string>() : new[] { value };

            _writer.WriteObjectStart();
            _writer.WriteNonEmptyNameArray("FirstEmptyWrite", Enumerable.Empty<string>());
            _writer.WriteNonEmptyNameArray(name, values);
            _writer.WriteNonEmptyNameArray("SecondEmptyWrite", Enumerable.Empty<string>());

            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void WriteNonEmptyNameArray_WithNullValue_WritesNameArray()
        {
            _writer.WriteObjectStart();
            _writer.WriteNonEmptyNameArray(PropertyName, new string[] { null });

            const string expectedHash = "6JqdqL+bz8M=";
            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void GetHash_WithNoOtherChanges_ReturnsDefaultValue()
        {
            const string expectedHash = "AAAAAAAAAAA=";
            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void GetHash_WhenCalledOnCompleteObject_ReturnsHash()
        {
            _writer.WriteObjectStart();
            _writer.WriteObjectStart(PropertyName);
            _writer.WriteNameValue("b", 0);
            _writer.WriteNameValue("c", "d");
            _writer.WriteNameArray("e", new[] { "f", "g" });
            _writer.WriteObjectEnd();
            _writer.WriteObjectEnd();

            const string expectedHash = "heVVAmQ95DE=";
            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        private void MakeReadOnly()
        {
            _writer.GetHash();
        }
    }
}
