// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xunit;
using NuGet.Protocol.Events;

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

        [Fact]
        public void GetServiceEntries_InvokesDiagnosticEventForSourceResources()
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
            var resource = new ServiceIndexResourceV3(content, expectedRequestTime, new Configuration.PackageSource(source));

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
