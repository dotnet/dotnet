// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class ServiceIndexResourceV3Tests
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            var serviceIndex = CreateServiceIndex();
            var expectedJson = serviceIndex.ToString();
            var expectedRequestTime = DateTime.UtcNow;
            var resource = new ServiceIndexResourceV3(serviceIndex, expectedRequestTime);

            Assert.Equal(expectedJson, resource.Json);
            Assert.Equal(expectedRequestTime, resource.RequestTime);
            Assert.Equal(1, resource.Entries.Count);
            Assert.Equal("a", resource.Entries[0].Type);
            Assert.Equal("http://unit.test/b", resource.Entries[0].Uri.ToString());
        }

        private static JObject CreateServiceIndex()
        {
            return new JObject
            {
                { "version", "1.2.3" },
                { "resources", new JArray
                    {
                        new JObject
                        {
                            { "@type", "a" },
                            { "@id", "http://unit.test/b" }
                        }
                    }
                }
            };
        }
    }
}
