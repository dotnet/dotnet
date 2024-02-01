// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class ServiceIndexResourceV3ProviderTests
    {
        [Theory]
        [InlineData(@"d:\packages\mypackages.json")]
        [InlineData(@"\\network\mypackages.json")]
        [InlineData(@"/mypackages/mypackages.json")]
        [InlineData(@"~/mypackages/mypackages.json")]
        public async Task TryCreate_ReturnsFalse_IfSourceIsADirectory(string location)
        {
            // Arrange
            var packageSource = new PackageSource(location);
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(packageSource, new[] { provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Theory]
        [InlineData("https://www.nuget.org/api/v2")]
        [InlineData("https://www.nuget.org/api/v2.json/")]
        public async Task TryCreate_ReturnsFalse_IfSourceDoesNotHaveAJsonSuffix(string location)
        {
            // Arrange
            var packageSource = new PackageSource(location);
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(packageSource, new[] { provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task TryCreate_Throws_IfSourceLocationReturnsFailureCode()
        {
            // Arrange
            var source = $"https://does-not-exist.server-{new Guid().ToString()}/does-not-exist.json";
            // This will return a 404 - NotFound.
            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, string.Empty } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var exception = await Assert.ThrowsAsync<FatalProtocolException>(async () =>
            {
                var result = await provider.TryCreate(sourceRepository, default(CancellationToken));
            });

            Assert.IsType<HttpRequestException>(exception.InnerException);
        }

        [Theory]
        [InlineData("not-valid-json")]
        [InlineData(@"<?xml version=""1.0"" encoding=""utf-8""?><service xml:base=""http://www.nuget.org/api/v2/""
xmlns=""http://www.w3.org/2007/app"" xmlns:atom=""http://www.w3.org/2005/Atom""><workspace><atom:title>Default</atom:title>
<collection href=""Packages""><atom:title>Packages</atom:title></collection></workspace></service>")]
        public async Task TryCreate_Throws_IfSourceLocationDoesNotReturnValidJson(string content)
        {
            // Arrange
            var source = $"https://fake.server-{new Guid().ToString()}/users.json";
            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act and assert
            var exception = await Assert.ThrowsAsync<FatalProtocolException>(async () =>
            {
                var result = await provider.TryCreate(sourceRepository, default(CancellationToken));
            });

            Assert.IsType<InvalidDataException>(exception.InnerException);
            Assert.IsType<JsonReaderException>(exception.InnerException.InnerException);
        }

        [Theory]
        [InlineData("{ version: \"not-semver\" } ")]
        [InlineData("{ version: \"3.0.0.0\" } ")] // not strict semver
        public async Task TryCreate_Throws_IfInvalidVersionInJson(string content)
        {
            // Arrange
            var source = $"https://fake.server-{new Guid().ToString()}/users.json";
            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var exception = await Assert.ThrowsAsync<FatalProtocolException>(async () =>
            {
                var result = await provider.TryCreate(sourceRepository, default(CancellationToken));
            });

            // Assert
            Assert.StartsWith("The source version is not supported", exception.InnerException.Message);
        }

        [Theory]
        [InlineData("{ json: \"that does not contain version.\" }")]
        [InlineData("{ version: 3 } ")] // version is not a string
        [InlineData("{ version: { value: 3 } } ")] // version is not a string
        public async Task TryCreate_Throws_IfNoVersionInJson(string content)
        {
            // Arrange
            var source = $"https://fake.server-{new Guid().ToString()}/users.json";
            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var exception = await Assert.ThrowsAsync<FatalProtocolException>(async () =>
            {
                var result = await provider.TryCreate(sourceRepository, default(CancellationToken));
            });

            // Assert
            Assert.Equal("The source does not have the 'version' property.", exception.InnerException.Message);
        }

        [Fact]
        public async Task TryCreate_ReturnsTrue_IfSourceLocationReturnsValidJson()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = @"{ version: '3.1.0-beta' }";
            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);
        }

        [Fact]
        public async Task Query_For_Particular_Resource()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = CreateTestIndex();

            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);

            var resource = result.Item2 as ServiceIndexResourceV3;

            Assert.NotNull(resource);

            var endpoints = resource.GetServiceEntryUris(new NuGetVersion(4, 0, 0), "Citrus");

            Assert.True(endpoints.Count == 1);

            var endpointSet = new HashSet<string>(endpoints.Select(u => u.AbsoluteUri));
            Assert.Contains("http://tempuri.org/orange", endpointSet);
        }

        [Fact]
        public async Task Query_For_Particular_Multi_Value_Resource()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = CreateTestIndex();

            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);

            var resource = result.Item2 as ServiceIndexResourceV3;

            Assert.NotNull(resource);

            var endpoints = resource.GetServiceEntryUris(new NuGetVersion(4, 0, 0), "Fruit");

            Assert.True(endpoints.Count == 3);

            var endpointSet = new HashSet<string>(endpoints.Select(u => u.AbsoluteUri));
            Assert.Contains("http://tempuri.org/banana", endpointSet);
            Assert.Contains("http://tempuri.org/apple", endpointSet);
            Assert.Contains("http://tempuri.org/orange", endpointSet);
        }

        [Fact]
        public async Task Query_For_Resource_With_Precedence()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = CreateTestIndex();

            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);

            var resource = result.Item2 as ServiceIndexResourceV3;

            Assert.NotNull(resource);

            var endpoints = resource.GetServiceEntryUris(new NuGetVersion(4, 0, 0), "Chocolate", "Vegetable");

            Assert.True(endpoints.Count == 1);

            var endpointSet = new HashSet<string>(endpoints.Select(u => u.AbsoluteUri));
            Assert.Contains("http://tempuri.org/chocolate", endpointSet);
        }

        [Fact]
        public async Task Query_For_Resource_With_VersionPrecedence_ExactMatch()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = CreateVersionedTestIndex();

            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);

            var resource = result.Item2 as ServiceIndexResourceV3;

            Assert.NotNull(resource);

            var endpoints = resource.GetServiceEntryUris(new NuGetVersion(4, 0, 0), "A", "B");

            Assert.True(endpoints.Count == 1);

            var endpointSet = new HashSet<string>(endpoints.Select(u => u.AbsoluteUri));
            Assert.Contains("http://tempuri.org/A/4.0.0", endpointSet);
        }

        [Fact]
        public async Task Query_For_Resource_With_NoCompatibleVersion()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = CreateVersionedTestIndex();

            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);

            var resource = result.Item2 as ServiceIndexResourceV3;

            Assert.NotNull(resource);

            var endpoints = resource.GetServiceEntryUris(NuGetVersion.Parse("0.0.0-beta"), "A");

            Assert.True(endpoints.Count == 0);
        }

        [Fact]
        public async Task Query_For_Resource_With_VersionPrecedence_LowerVersion()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = CreateVersionedTestIndex();

            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);

            var resource = result.Item2 as ServiceIndexResourceV3;

            Assert.NotNull(resource);

            var endpoints = resource.GetServiceEntryUris(new NuGetVersion(4, 1, 0), "A", "B");

            Assert.True(endpoints.Count == 1);

            var endpointSet = new HashSet<string>(endpoints.Select(u => u.AbsoluteUri));
            Assert.Contains("http://tempuri.org/A/4.0.0", endpointSet);
        }

        [Fact]
        public async Task Query_For_Resource_With_VersionPrecedence_NoFallbackBetweenTypes()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = CreateVersionedTestIndex2();

            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);

            var resource = result.Item2 as ServiceIndexResourceV3;

            Assert.NotNull(resource);

            var endpoints = resource.GetServiceEntryUris(new NuGetVersion(4, 1, 0), "A", "B");

            Assert.True(endpoints.Count == 1);

            var endpointSet = new HashSet<string>(endpoints.Select(u => u.AbsoluteUri));
            Assert.Contains("http://tempuri.org/B", endpointSet);
        }

        [Fact]
        public async Task Query_For_Resource_ReturnAllOfSameTypeVersion()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = CreateVersionedTestIndex3();

            var httpProvider = StaticHttpSource.CreateHttpSource(new Dictionary<string, string> { { source, content } });
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            var result = await provider.TryCreate(sourceRepository, default(CancellationToken));

            // Assert
            Assert.True(result.Item1);

            var resource = result.Item2 as ServiceIndexResourceV3;

            Assert.NotNull(resource);

            var endpoints = resource.GetServiceEntryUris(new NuGetVersion(5, 0, 0), "A", "B");

            Assert.True(endpoints.Count == 2);

            var endpointSet = new HashSet<string>(endpoints.Select(u => u.AbsoluteUri));
            Assert.Contains("http://tempuri.org/A/5.0.0/1", endpointSet);
            Assert.Contains("http://tempuri.org/A/5.0.0/2", endpointSet);
        }

        [Fact]
        public async Task TryCreate_CancellationThrows()
        {
            // Arrange
            var source = $"https://some-site-{new Guid().ToString()}.org/test.json";
            var content = @"{ version: '3.1.0-beta' }";
            //Create an HTTP provider that will cancel the token.
            var httpProvider = StaticHttpSource.CreateHttpSource(
                new Dictionary<string, string> { { source, content } },
                errorContent: string.Empty,
                httpSource: null,
                throwOperationCancelledException: true);
            var provider = new ServiceIndexResourceV3Provider();
            var sourceRepository = new SourceRepository(new PackageSource(source),
                new INuGetResourceProvider[] { httpProvider, provider });

            // Act
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                Task task() => provider.TryCreate(sourceRepository, tokenSource.Token);

                // Assert
                await Assert.ThrowsAsync<OperationCanceledException>(task);
            }
        }

        private static string CreateTestIndex()
        {
            var obj = new JObject
            {
                { "version", "3.1.0-beta" },
                { "resources", new JArray
                    {
                        new JObject
                        {
                            { "@type", "Fruit" },
                            { "@id", "http://tempuri.org/banana" },
                        },
                        new JObject
                        {
                            { "@type", "Fruit" },
                            { "@id", "http://tempuri.org/apple" },
                        },
                        new JObject
                        {
                            { "@type", new JArray { "Fruit", "Citrus" } },
                            { "@id", "http://tempuri.org/orange" },
                        },
                        new JObject
                        {
                            { "@type", new JArray { "Vegetable" } },
                            { "@id", "http://tempuri.org/cabbage" },
                        },
                        new JObject
                        {
                            { "@type", new JArray { "Chocolate" } },
                            { "@id", "http://tempuri.org/chocolate" },
                        },
                    }
                }
            };

            return obj.ToString();
        }

        private static string CreateVersionedTestIndex()
        {
            var obj = new JObject
            {
                { "version", "3.1.0-beta" },
                { "resources", new JArray
                    {
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A" }
                        },
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/4.0.0" },
                            { "clientVersion", "4.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/5.0.0" },
                            { "clientVersion", "5.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/6.0.0-beta" },
                            { "clientVersion", "6.0.0-beta" },
                        },
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/6.0.0-rc" },
                            { "clientVersion", new JArray { "6.0.0-rc.1", "6.0.0-rc.2" } },
                        },
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/7.0.0" },
                            { "clientVersion", "7.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "B" },
                            { "@id", "http://tempuri.org/B" },
                            { "clientVersion", "4.0.0" },
                        }
                    }
                }
            };

            return obj.ToString();
        }

        private static string CreateVersionedTestIndex2()
        {
            var obj = new JObject
            {
                { "version", "3.1.0-beta" },
                { "resources", new JArray
                    {
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/5.0.0" },
                            { "clientVersion", "5.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "B" },
                            { "@id", "http://tempuri.org/B" },
                            { "clientVersion", "4.0.0" },
                        }
                    }
                }
            };

            return obj.ToString();
        }

        private static string CreateVersionedTestIndex3()
        {
            var obj = new JObject
            {
                { "version", "3.1.0-beta" },
                { "resources", new JArray
                    {
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/5.0.0/2" },
                            { "clientVersion", "5.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/5.0.0/1" },
                            { "clientVersion", "5.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "A" },
                            { "@id", "http://tempuri.org/A/4.0.0" },
                            { "clientVersion", "4.0.0" },
                        },
                        new JObject
                        {
                            { "@type", "B" },
                            { "@id", "http://tempuri.org/B/5.0.0/1" },
                            { "clientVersion", "5.0.0" },
                        },
                    }
                }
            };

            return obj.ToString();
        }
    }
}
