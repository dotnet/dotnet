// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Events;
using NuGet.Protocol.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class ServiceIndexResourceV3Tests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Constructor_InitializesProperties(bool useStj)
        {
            var serviceIndex = CreateServiceIndex();
            var expectedRequestTime = DateTime.UtcNow;
            var resource = CreateResource(serviceIndex, useStj, expectedRequestTime);

            Assert.Equal(expectedRequestTime, resource.RequestTime);
            Assert.Equal(1, resource.Entries.Count);
            Assert.Equal("a", resource.Entries[0].Type);
            Assert.Equal("http://unit.test/b", resource.Entries[0].Uri.ToString());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetServiceEntries_InvokesDiagnosticEventForSourceResources(bool useStj)
        {
            // Arrange
            int eventInvokeCount = 0;
            List<ProtocolDiagnosticServiceIndexEntryEvent> capturedEvents = new List<ProtocolDiagnosticServiceIndexEntryEvent>();

            ProtocolDiagnostics.ServiceIndexEntryEvent += (pdEvent) =>
            {
                eventInvokeCount++;
                capturedEvents.Add(pdEvent);
            };

            var source = $"https://test/index.json";
            var content = CreateServiceIndexWithFourResourceTypesTwoHTTP();

            var expectedRequestTime = DateTime.UtcNow;
            var resource = CreateResource(content, useStj, expectedRequestTime, new Configuration.PackageSource(source));

            // Act
            var result = resource.GetServiceEntries(ServiceTypes.SearchQueryService);

            // Assert
            int httpResourceCapture = 0;

            foreach (var serviceIndexEvent in capturedEvents)
            {
                Assert.Equal(serviceIndexEvent.Source, source);
                httpResourceCapture += serviceIndexEvent.HttpsSourceHasHttpResource ? 1 : 0;
            }

            Assert.Equal(2, httpResourceCapture);
            Assert.Equal(2, eventInvokeCount);
        }

        private static ServiceIndexResourceV3 CreateResource(
            JObject jObject,
            bool useStj,
            DateTime requestTime = default,
            Configuration.PackageSource? packageSource = null)
        {
            if (useStj)
            {
                var model = JsonSerializer.Deserialize(jObject.ToString(), JsonContext.Default.ServiceIndexModel)!;
                return new ServiceIndexResourceV3(model, requestTime, packageSource);
            }

            return new ServiceIndexResourceV3(jObject, requestTime, packageSource);
        }

        private static JObject CreateServiceIndexWithFourResourceTypesTwoHTTP()
        {
            var obj = new JObject
            {
                { "version", "3.1.0-beta" },
                { "resources", new JArray
                    {
                        new JObject
                        {
                            { "@type", "SearchQueryService/Versioned" },
                            { "@id", "http://tempuri.org/A/5.0.0/2" },
                            { "clientVersion", "5.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "SearchQueryService/Versioned" },
                            { "@id", "http://tempuri.org/A/5.0.0/1" },
                            { "clientVersion", "5.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "SearchQueryService/Versioned" },
                            { "@id", "https://test" },
                            { "clientVersion", "4.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "SearchQueryService/Versioned" },
                            { "@id", "https://test" },
                            { "clientVersion", "5.0.0" },
                        },
                    }
                }
            };

            return obj;
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
