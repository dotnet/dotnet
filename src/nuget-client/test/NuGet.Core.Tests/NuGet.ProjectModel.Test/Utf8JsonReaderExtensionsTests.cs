// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;
using System.Text.Json;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    [UseCulture("")] // Fix tests failing on systems with non-English locales
    public class Utf8JsonReaderExtensionsTests
    {
        [Theory]
        [InlineData("null", null)]
        [InlineData("true", "True")]
        [InlineData("false", "False")]
        [InlineData("-2", "-2")]
        [InlineData("9223372036854775807", "9223372036854775807")]
        [InlineData("3.14", "3.14")]
        [InlineData("\"b\"", "b")]
        public void ReadTokenAsString_WhenValueIsConvertibleToString_ReturnsValueAsString(
            string value,
            string expectedResult)
        {
            var json = $"{{\"a\":{value}}}";
            var encodedBytes = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(encodedBytes);
            reader.Read();
            reader.Read();
            reader.Read();
            string actualResult = reader.ReadTokenAsString();
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
