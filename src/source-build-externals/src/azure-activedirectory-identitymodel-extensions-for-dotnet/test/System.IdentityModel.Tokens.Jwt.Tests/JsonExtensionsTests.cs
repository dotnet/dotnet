// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Json;
using Xunit;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    public class JsonExtensionsTests
    {
        [Fact]
        public void JsonWithDuplicateNames()
        {
            try
            {

                string json = @"{""tag"":""value1"", ""tag"": ""value2""}";
                var jsonObject = JsonExtensions.DeserializeFromJson<object>(json);
            }
            catch(Exception ex)
            {
                Assert.Equal(typeof(ArgumentException), ex.GetType());
                Assert.Contains("Property with the same name already exists on object.", ex.Message);
            }
        }

        [Fact]
        public void MalformedJson()
        {
            Assert.Throws<JsonReaderException>(() => JsonExtensions.DeserializeFromJson<object>(@"{""tag"":""value""}ABCD"));
        }
    }
}
