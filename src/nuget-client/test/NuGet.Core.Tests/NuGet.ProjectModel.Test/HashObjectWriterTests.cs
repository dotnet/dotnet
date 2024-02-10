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
            _hashFunc = new Sha512HashFunction();
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

            const string expectedHash = "wtA8bvsWw/gGSw0FnkX5UfF0hCGmIlcaUgCd3MKmcIUeGtAmn72B1FhW+iD/rNCB3SD+znYRQgvvtJ65hLwjyg==";
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
        [InlineData("", "z6NXo38IFDLUr7F/bIoub9q2RcWGd9G0kd0sZ2LrUsjAAOxeICRcA8sN6CZR1kIMrZl/AluqG57zeTOuEBQ1Bw==")]
        [InlineData(PropertyName, "7NPeUey7AxorbVFFvKtffuKjuI3T9fqrDmyP9jMRFaMIQZdiMqy4+dvV2ci7nhuZjOXx5qdfiIns7wluMZYjlQ==")]
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
        [InlineData("", -1, "Py0m1BAc1GMLdv3VwUlClC/IwVxlRDmvpmUpLV1aQeFXw/eJsxWsbS/xi/Lu/HxvoufXcjJmlljwfe/B/aIccQ==")]
        [InlineData(PropertyName, 1, "Z7FWuyPxOQ7v6uelHNSbq7no7P2EXqJRh0k6ONDfevPFA3yycn77N+keqUWo9rq6efTrRpjaKxKuvhdas2tzfg==")]
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
        [InlineData("", true, "MuWFFw3nbGG78iKZZPcYZVMFfn8pxOZA3gBgB2KKL5Lysc/SX/xo5csUe9gvavVgis2wsA3EqJ8ZJkgU6s3SCA==")]
        [InlineData(PropertyName, false, "/prsw57A47qiwYscCHCc5UJZdEJtaxpyeagaTzwlIDbFf0CSFJeU4EIqSBvh3q9iy9SwRk3Q+RGQH7KjrhvohQ==")]
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
        [InlineData("", "", "y119K+M0qb7ZuOPtqhZB3oSq3qICw6ulw46gpooqe+mgd11zySkL+dONrIm8asZiUOWKa1Vo8lSp0c4Df92gHQ==")]
        [InlineData(PropertyName, null, "KYuQ62iFQ/6Cd6svsq38bCPZq2HUZJae+e8kyvFpxkjpBMGOa/88lvo++bIb2zHL7eO5MJN9I8r1/kwe0lSctg==")]
        [InlineData(PropertyName, "b", "kQ/OLQaqRdPBgNd/wzuUuTmSoCW13jaonYx5//arvLFtDo85lv5kfr1ATCol6HH9lDFtNS44X/HSjSI7xnxSDA==")]
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
        [InlineData("", null, "bAe84bYqI4pvIFTbU9k55dzFYneWYlLw6w2Hbbw9F71iXKv9CYdVl6WE20O73WP2Gs8N1jY8vNLnpRy2uSOkIw==")]
        [InlineData(PropertyName, "b", "6lWKPWARIKyDadU74W5+bb7W7/1mFLyZaljfm4UpudCTeiny7dbPU5hB/C63Xt6LDpqbjtLvoxS0hiWIbWGvkA==")]
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

            const string expectedHash = "BqvCuFre4Siu1xS8bzI6rXbSTCoNBI/bqGRvUTFDtUAVlDGfDg5cqeBosLcw5sboEHqOFOb/MqJBOyK1Xj5Ueg==";
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
        [InlineData(PropertyName, "b", "6lWKPWARIKyDadU74W5+bb7W7/1mFLyZaljfm4UpudCTeiny7dbPU5hB/C63Xt6LDpqbjtLvoxS0hiWIbWGvkA==")]
        public void WriteNonEmptyNameArray_WithValidValues_WritesNameArray(string name, string value, string expectedHash)
        {
            IEnumerable<string> values = value == null ? Enumerable.Empty<string>() : new[] { value };

            _writer.WriteObjectStart();
            _writer.WriteNonEmptyNameArray(name, values);

            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Theory]
        [InlineData(PropertyName, "b", "6lWKPWARIKyDadU74W5+bb7W7/1mFLyZaljfm4UpudCTeiny7dbPU5hB/C63Xt6LDpqbjtLvoxS0hiWIbWGvkA==")]
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

            const string expectedHash = "BqvCuFre4Siu1xS8bzI6rXbSTCoNBI/bqGRvUTFDtUAVlDGfDg5cqeBosLcw5sboEHqOFOb/MqJBOyK1Xj5Ueg==";
            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        [Fact]
        public void GetHash_WithNoOtherChanges_ReturnsDefaultValue()
        {
            const string expectedHash = "z4PhNX7vuL3xVChQ1m2AB9Yg5AULVxXcg/SpIdNs6c5H0NE8XYXysP+DGNKHfuwvY7kxvUdBeoGlODJ6+SfaPg==";
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

            const string expectedHash = "TGP0LarTsGYQ2bqAC8lWyRQR+JsKzsO0Y+h6w7mtTj6mBOLTy8Dr0ZypSgzwzD9xuddh2ceDT7fEXve5ohuNeQ==";
            string actualHash = _writer.GetHash();

            Assert.Equal(expectedHash, actualHash);
        }

        private void MakeReadOnly()
        {
            _writer.GetHash();
        }
    }
}
