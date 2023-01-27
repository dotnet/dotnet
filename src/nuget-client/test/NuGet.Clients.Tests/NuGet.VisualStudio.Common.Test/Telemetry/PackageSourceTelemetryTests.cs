// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Events;
using NuGet.VisualStudio.Telemetry;
using Test.Utility;
using Xunit;

namespace NuGet.VisualStudio.Common.Test.Telemetry
{
    public class PackageSourceTelemetryTests
    {
        [Fact]
        public void Ctor_DuplicateSourceUrls_DoesNotThrow()
        {
            // Arrange
            const string feedUrl = "https://source.test/v3/index.json";
            SourceRepository[] sources = new[]
            {
                new SourceRepository(new PackageSource(source: feedUrl, name: "Source1"), Repository.Provider.GetCoreV3()),
                new SourceRepository(new PackageSource(source: feedUrl, name: "Source2"), Repository.Provider.GetCoreV3())
            };

            // Act
            _ = new PackageSourceTelemetry(sources, Guid.Empty, PackageSourceTelemetry.TelemetryAction.Restore);

            // Assert
            // no assert, just making sure ctor didn't throw.
        }

        [Fact]
        public void AddResourceData_SameSourceDifferentResource_StoredSeparately()
        {
            // Arrange
            var re1 = CreateSampleResourceEvent(method: nameof(FindPackageByIdResource.GetDependencyInfoAsync));
            var re2 = CreateSampleResourceEvent(method: nameof(FindPackageByIdResource.CopyNupkgToStreamAsync));
            var data = CreateDataDictionary(SampleSource);
            var stringTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

            // Act
            PackageSourceTelemetry.AddResourceData(re1, data, stringTable);
            PackageSourceTelemetry.AddResourceData(re2, data, stringTable);

            // Assert
            var sourceData = data[SampleSource];
            Assert.Equal(2, sourceData.Resources.Count);
        }

        [Fact]
        public void AddResourceData_SameSourceSameResource_AccumulatesCorrectly()
        {
            // Arrange
            var re1 = CreateSampleResourceEvent(duration: TimeSpan.FromMilliseconds(100));
            var re2 = CreateSampleResourceEvent(duration: TimeSpan.FromMilliseconds(200));
            var data = CreateDataDictionary(SampleSource);
            var stringTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

            // Act
            PackageSourceTelemetry.AddResourceData(re1, data, stringTable);
            PackageSourceTelemetry.AddResourceData(re2, data, stringTable);

            // Assert
            var result = Assert.Single(data).Value;
            var resourceData = Assert.Single(result.Resources).Value;
            Assert.Equal(2, resourceData.count);
            Assert.Equal(TimeSpan.FromMilliseconds(300), resourceData.duration);
        }

        [Fact]
        public void AddHttpData_NullHeaderDuration_SetsResultToNull()
        {
            // Arrange
            var pde1 = CreateSampleHttpEvent(headerDuration: TimeSpan.FromMilliseconds(150));
            var pde2 = CreateSampleHttpEvent(headerDuration: null);
            var pde3 = CreateSampleHttpEvent(headerDuration: TimeSpan.FromMilliseconds(100));
            var data = CreateDataDictionary(SampleSource);

            // Act
            PackageSourceTelemetry.AddHttpData(pde1, data);
            PackageSourceTelemetry.AddHttpData(pde2, data);
            PackageSourceTelemetry.AddHttpData(pde3, data);

            // Assert
            KeyValuePair<string, PackageSourceTelemetry.Data> pair = Assert.Single(data);
            Assert.Null(pair.Value.Http.HeaderDuration);
        }

        [Fact]
        public void AddHttpData_MultipleEvents_DurationsAccumulate()
        {
            // Arrange
            var timings = new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(150), TimeSpan.FromMilliseconds(200) };
            var data = CreateDataDictionary(SampleSource);

            // Act
            for (int i = 0; i < timings.Length; i++)
            {
                var pde = CreateSampleHttpEvent(eventDuration: timings[i]);
                PackageSourceTelemetry.AddHttpData(pde, data);
            }

            // Assert
            var pair = Assert.Single(data);
            var http = pair.Value.Http;
            Assert.Equal(timings.Length, http.Requests);
            Assert.Equal(timings.Aggregate(TimeSpan.Zero, (a, b) => a + b), http.TotalDuration);
        }

        [Fact]
        public void AddNupkgCopiedData_MultipleEvents_AccumulatesCorrectly()
        {
            // Arrange
            var sizes = new[] { 1, 22, 333, 4444, 55555, 666666 };
            var data = CreateDataDictionary(SampleSource);

            // Act
            for (int i = 0; i < sizes.Length; i++)
            {
                var nce = new ProtocolDiagnosticNupkgCopiedEvent(SampleSource, sizes[i]);
                PackageSourceTelemetry.AddNupkgCopiedData(nce, data);
            }

            // Assert
            var result = Assert.Single(data).Value;
            Assert.Equal(sizes.Length, result.NupkgCount);
            Assert.Equal(sizes.Sum(), result.NupkgSize);
        }

        [Fact]
        public async Task AddData_IsThreadSafe()
        {
            // Arrange
            var data = CreateDataDictionary(SampleSource);
            var stringTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
            var eventsToRaise = 10000;
            var sources = new HashSet<string>
            {
                SampleSource
            };

            Task SendEvents(Action action)
            {
                var tasks = new List<Task>();

                for (int i = 0; i < eventsToRaise; i++)
                {
                    tasks.Add(Task.Run(() => action()));
                }

                return Task.WhenAll(tasks);
            }

            var resourceEvent = CreateSampleResourceEvent();
            var httpEvent = CreateSampleHttpEvent();
            var nupkgCopiedEvent = new ProtocolDiagnosticNupkgCopiedEvent(SampleSource, fileSize: 123456);

            // Act
            var resourceEvents = Task.Run(() => SendEvents(() => PackageSourceTelemetry.AddResourceData(resourceEvent, data, stringTable)));
            var httpEvents = Task.Run(() => SendEvents(() => PackageSourceTelemetry.AddHttpData(httpEvent, data)));
            var nupkgCopiedEvents = Task.Run(() => SendEvents(() => PackageSourceTelemetry.AddNupkgCopiedData(nupkgCopiedEvent, data)));
            await Task.WhenAll(resourceEvents, httpEvents, nupkgCopiedEvents);

            // Assert
            KeyValuePair<string, PackageSourceTelemetry.Data> pair = Assert.Single(data);
            Assert.Equal(eventsToRaise, pair.Value.Resources.Sum(r => r.Value.count));
            Assert.Equal(eventsToRaise, pair.Value.Http.Requests);
            Assert.Equal(eventsToRaise, pair.Value.NupkgCount);
        }

        [Fact]
        public void AddHttpData_MultipleEvents_CountsBytes()
        {
            // Arrange
            var bytes = new[] { 1_000, 1_500, 10_000 };
            var data = CreateDataDictionary(SampleSource);

            // Act
            for (int i = 0; i < bytes.Length; i++)
            {
                var pde = CreateSampleHttpEvent(bytes: bytes[i]);
                PackageSourceTelemetry.AddHttpData(pde, data);
            }

            // Assert
            var pair = Assert.Single(data);
            var httpData = pair.Value.Http;
            Assert.Equal(bytes.Sum(), httpData.TotalBytes);
        }

        [Fact]
        public void AddHttpData_MultipleEvents_CountsBools()
        {
            // Arrange
            var events = new List<ProtocolDiagnosticHttpEvent>();
            var trueFalseList = new[] { true, false };
            foreach (var isSuccess in trueFalseList)
            {
                foreach (var isRetry in trueFalseList)
                {
                    foreach (var isCancelled in trueFalseList)
                    {
                        foreach (var isLastAttempt in trueFalseList)
                        {
                            events.Add(CreateSampleHttpEvent(isSuccess: isSuccess, isRetry: isRetry, isCancelled: isCancelled, isLastAttempt: isLastAttempt));
                        }
                    }
                }
            }
            var data = CreateDataDictionary(SampleSource);

            // Act
            for (int i = 0; i < events.Count; i++)
            {
                PackageSourceTelemetry.AddHttpData(events[i], data);
            }

            // Assert
            var pair = Assert.Single(data);
            var httpData = pair.Value.Http;

            // isSuccess, isRetry and isCancelled were each true for exactly half the events
            Assert.Equal(events.Count / 2, httpData.Successful);
            Assert.Equal(events.Count / 2, httpData.Retries);
            Assert.Equal(events.Count / 2, httpData.Cancelled);

            // a resource is only failed when it's unsuccessful on the last attempt, but not when we cancelled it. Therefore, we expect 2, for each value of isRetry
            Assert.Equal(2, httpData.Failed);
        }

        [Theory]
        [InlineData("")]
        [InlineData("nuget.org,nuget")]
        [InlineData(" nuget.org , nuget | privateRepository , private* ")]
        public async Task ToTelemetry_ZeroRequests_DoesNotCreateTelemetryObject(string packageSourceMapping)
        {
            // Arrange
            var data = new PackageSourceTelemetry.Data();
            data.NupkgCount = 0;
            data.Resources.Clear();
            data.Http.Requests = 0;
            var configuration = string.IsNullOrEmpty(packageSourceMapping) ? null : PackageSourceMappingUtility.GetpackageSourceMapping(packageSourceMapping);

            var sourceRepository = new SourceRepository(new PackageSource("source"), Repository.Provider.GetCoreV3());

            // Act
            var result = await PackageSourceTelemetry.ToTelemetryAsync(data, sourceRepository, "parentId", "actionName", packageSourceMappingConfiguration: configuration);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("nuget.org,nuget")]
        [InlineData(" nuget.org , nuget | privateRepository , private* ")]
        public async Task ToTelemetry_WithData_CreatesTelemetryProperties(string packageSourceMapping)
        {
            // Arrange
            var data = new PackageSourceTelemetry.Data();

            data.Resources.Add("resource 1", (count: 1, duration: TimeSpan.FromMilliseconds(1234)));
            data.Resources.Add("resource 2", (count: 1, duration: TimeSpan.FromMilliseconds(1111)));
            data.Resources.Add("resource 3", (count: 1, duration: TimeSpan.FromMilliseconds(1122)));

            data.NupkgCount = 3;
            data.NupkgSize = 123456;

            var httpData = data.Http;
            httpData.Requests = 10;
            httpData.TotalDuration = TimeSpan.FromMilliseconds(1234);
            httpData.HeaderDuration = TimeSpan.FromMilliseconds(123);
            httpData.TotalBytes = 1_000_000;
            httpData.Successful = 7;
            httpData.Retries = 4;
            httpData.Cancelled = 2;
            httpData.Failed = 1;
            httpData.StatusCodes.Add(200, 7);
            httpData.StatusCodes.Add(404, 3);
            var configuration = string.IsNullOrEmpty(packageSourceMapping) ? null : PackageSourceMappingUtility.GetpackageSourceMapping(packageSourceMapping);

            var source = new SourceRepository(new PackageSource(NuGetConstants.V3FeedUrl), Repository.Provider.GetCoreV3());

            // Act
            var result = await PackageSourceTelemetry.ToTelemetryAsync(data, source, "parentId", "actionName", packageSourceMappingConfiguration: configuration);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(PackageSourceTelemetry.EventName, result.Name);
            Assert.Equal("parentId", result[PackageSourceTelemetry.PropertyNames.ParentId]);
            Assert.Equal("actionName", result[PackageSourceTelemetry.PropertyNames.Action]);

            Assert.Equal(FeedType.HttpV3, result[PackageSourceTelemetry.PropertyNames.Source.Type]);
            var url = Assert.Single(result.GetPiiData().Where(pair => pair.Key == PackageSourceTelemetry.PropertyNames.Source.Url));
            Assert.Equal(NuGetConstants.V3FeedUrl, url.Value);
            Assert.Equal("nuget.org", result[PackageSourceTelemetry.PropertyNames.Source.MSFeed]);

            Assert.Equal(data.NupkgCount, result[PackageSourceTelemetry.PropertyNames.Nupkgs.Copied]);
            Assert.Equal(data.NupkgSize, result[PackageSourceTelemetry.PropertyNames.Nupkgs.Bytes]);

            Assert.Equal(data.Resources.Sum(r => r.Value.count), result[PackageSourceTelemetry.PropertyNames.Resources.Calls]);
            foreach (var resource in data.Resources)
            {
                var resourceTelemetryValue = Assert.Contains(PackageSourceTelemetry.PropertyNames.Resources.Details, result.ComplexData);
                var resourceTelemetry = Assert.IsType<TelemetryEvent>(resourceTelemetryValue);
                var resourceDetailsValue = Assert.Contains(resource.Key, resourceTelemetry.ComplexData);
                var resourceDetails = Assert.IsType<TelemetryEvent>(resourceDetailsValue);
                Assert.Equal(resource.Value.count, resourceDetails["count"]);
                Assert.Equal(resource.Value.duration.TotalMilliseconds, resourceDetails["duration"]);
            }

            Assert.Equal(httpData.Requests, result[PackageSourceTelemetry.PropertyNames.Http.Requests]);
            Assert.Equal(httpData.Successful, result[PackageSourceTelemetry.PropertyNames.Http.Successful]);
            Assert.Equal(httpData.Retries, result[PackageSourceTelemetry.PropertyNames.Http.Retries]);
            Assert.Equal(httpData.Cancelled, result[PackageSourceTelemetry.PropertyNames.Http.Cancelled]);
            Assert.Equal(httpData.Failed, result[PackageSourceTelemetry.PropertyNames.Http.Failed]);
            Assert.Equal(httpData.TotalBytes, result[PackageSourceTelemetry.PropertyNames.Http.Bytes]);
            Assert.Equal(httpData.TotalDuration.TotalMilliseconds, result[PackageSourceTelemetry.PropertyNames.Http.Duration.Total]);
            Assert.Equal(httpData.HeaderDuration.Value.TotalMilliseconds, result[PackageSourceTelemetry.PropertyNames.Http.Duration.Header]);

            if (string.IsNullOrEmpty(packageSourceMapping))
            {
                Assert.False((bool)result["PackageSourceMapping.IsMappingEnabled"]);
            }
            else
            {
                Assert.True((bool)result["PackageSourceMapping.IsMappingEnabled"]);
            }

            var statusCodesValue = Assert.Contains<string, object>(PackageSourceTelemetry.PropertyNames.Http.StatusCodes, result.ComplexData);
            var statusCodes = Assert.IsType<TelemetryEvent>(statusCodesValue);
            foreach (var pair in httpData.StatusCodes)
            {
                Assert.Equal(pair.Value, statusCodes[pair.Key.ToString()]);
            }
        }

        [Fact]
        public void GetTotals_WithData_ReturnsCorrectSums()
        {
            // Arrange
            var data = new PackageSourceTelemetry.Data();
            data.NupkgSize = 97;
            data.Resources.Add("resource 1", (count: 2, duration: TimeSpan.FromMilliseconds(11)));
            data.Resources.Add("resource 2", (count: 5, duration: TimeSpan.FromMilliseconds(13)));

            var allData = new ConcurrentDictionary<string, PackageSourceTelemetry.Data>();
            for (int i = 1; i <= 7; i++)
            {
                allData[i.ToString()] = data;
            }

            int expectedRequests = 0;
            long expectedBytes = allData.Sum(d => d.Value.NupkgSize);
            TimeSpan expectedDuration = TimeSpan.Zero;

            foreach (var resourceData in allData.SelectMany(d => d.Value.Resources))
            {
                expectedRequests += resourceData.Value.count;
                expectedDuration += resourceData.Value.duration;
            }

            // Act
            var totals = PackageSourceTelemetry.GetTotals(allData);

            // Assert
            Assert.Equal(expectedRequests, totals.Requests);
            Assert.Equal(expectedBytes, totals.Bytes);
            Assert.Equal(expectedDuration, totals.Duration);
        }

        [Theory]
        [InlineData("https://api.nuget.org/v3/index.json", "nuget.org")]
        [InlineData("https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json", "Azure DevOps")]
        [InlineData("https://nuget.pkg.github.com/contoso/index.json", "GitHub")]
        [InlineData("https://api.contoso.org/v2/index.json", null)]
        [InlineData(".\\my\\PublicRepository\\", null)]
        public void GetMSFeed_CorrectlyIdentifies_SourceType(string source, string expectedSourceType)
        {
            // Arrange
            PackageSource packageSource = new(source);

            // Act
            string actualSourceType = PackageSourceTelemetry.GetMsFeed(packageSource);

            // Assert
            Assert.Equal(expectedSourceType, actualSourceType);
        }

        private static IReadOnlyDictionary<string, PackageSourceTelemetry.Data> CreateDataDictionary(params string[] sources)
        {
            var data = new Dictionary<string, PackageSourceTelemetry.Data>();
            foreach (var source in sources)
            {
                data[source] = new PackageSourceTelemetry.Data();
            }

            return data;
        }

        private const string SampleSource = "https://source.test/v3/index.json";
        private static readonly Uri SampleNupkgUri = new Uri("https://source.test/v3/flatcontainer/package/package.1.0.0.nupkg");

        private static ProtocolDiagnosticResourceEvent CreateSampleResourceEvent(
            string source = SampleSource,
            string resourceType = nameof(FindPackageByIdResource),
            string type = nameof(HttpFileSystemBasedFindPackageByIdResource),
            string method = nameof(FindPackageByIdResource.GetDependencyInfoAsync),
            TimeSpan? duration = null)
        {
            duration = duration ?? TimeSpan.FromMilliseconds(100);

            var resourceEvent = new ProtocolDiagnosticResourceEvent(source,
                resourceType,
                type,
                method,
                duration.Value);
            return resourceEvent;
        }

        private static ProtocolDiagnosticHttpEvent CreateSampleHttpEvent(
            DateTime? timestamp = null,
            string source = SampleSource,
            Uri url = null,
            TimeSpan? headerDuration = null,
            TimeSpan? eventDuration = null,
            long bytes = 1_000_000,
            int? httpStatusCode = 200,
            bool isSuccess = true,
            bool isRetry = false,
            bool isCancelled = false,
            bool isLastAttempt = false)
        {
            if (timestamp == null)
            {
                timestamp = DateTime.UtcNow;
            }

            if (url == null)
            {
                url = SampleNupkgUri;
            }

            if (eventDuration == null)
            {
                eventDuration = TimeSpan.FromMilliseconds(100);
            }

            var pde = new ProtocolDiagnosticHttpEvent(
                timestamp: timestamp.Value,
                source: source,
                url: url,
                headerDuration: headerDuration,
                eventDuration: eventDuration.Value,
                bytes: bytes,
                httpStatusCode: httpStatusCode,
                isSuccess: isSuccess,
                isRetry: isRetry,
                isCancelled: isCancelled,
                isLastAttempt: isLastAttempt);

            return pde;
        }
    }
}
