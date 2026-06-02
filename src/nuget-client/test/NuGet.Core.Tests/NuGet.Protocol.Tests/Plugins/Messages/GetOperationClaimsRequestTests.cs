// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class GetOperationClaimsRequestTests
    {
        private static readonly string _packageSourceRepository = "A";
        private static readonly A _serviceIndex = new A() { B = "C" };
        private static readonly JObject _serviceIndexJObject = JObject.FromObject(_serviceIndex);
        private static readonly string _serviceIndexJson = _serviceIndexJObject.ToString(Newtonsoft.Json.Formatting.None);

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var request = new GetOperationClaimsRequest(_packageSourceRepository, _serviceIndexJson);

            Assert.Equal(_packageSourceRepository, request.PackageSourceRepository);
            Assert.Equal("{\"B\":\"C\"}", request.ServiceIndexJson);
        }

        [Fact]
        public void JsonSerialization_ReturnsCorrectJson()
        {
            var request = new GetOperationClaimsRequest(_packageSourceRepository, _serviceIndexJson);

            var json = TestUtilities.Serialize(request);

            Assert.Equal("{\"PackageSourceRepository\":\"A\",\"ServiceIndex\":{\"B\":\"C\"}}", json);
        }

        [Theory]
        [InlineData("{\"PackageSourceRepository\":\"A\",\"ServiceIndex\":{}}", "{}")]
        [InlineData("{\"PackageSourceRepository\":\"A\",\"ServiceIndex\":{\"B\":\"C\"}}", "{\"B\":\"C\"}")]
        public void JsonDeserialization_ReturnsCorrectObject(string json, string serviceIndex)
        {
            var request = JsonSerializationUtilities.Deserialize<GetOperationClaimsRequest>(json);

            Assert.Equal("A", request.PackageSourceRepository);
            Assert.Equal(serviceIndex, request.ServiceIndexJson);
        }

        private sealed class A
        {
            public string B { get; set; }
        }
    }
}
