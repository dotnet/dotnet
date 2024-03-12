using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using IntegrationTests.WebApp;

namespace IntegrationTests.Tests
{
    public partial class RequestCollectionTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task RequestSuccessWithTraceParent()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Home";
            var url = client.BaseAddress + path;

            // Act
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "traceparent", "00-4e3083444c10254ba40513c7316332eb-e2a5f830c0ee2c46-00"}
                };
            var request = CreateRequestMessage(requestHeaders);
            request.RequestUri = new Uri(url);
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            this.output.WriteLine(await response.Content.ReadAsStringAsync());

            await WaitForTelemetryToArrive();

            var items = _factory.sentItems;
            PrintItems(items);
            // 1 Trace from Ilogger, 1 Request
            Assert.Equal(2, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            var traces = GetTelemetryOfType<TraceTelemetry>(items);
            Assert.Single(traces);
            var trace = traces[0];
            Assert.NotNull(req);
            Assert.NotNull(trace);

            Assert.Equal("4e3083444c10254ba40513c7316332eb", req.Context.Operation.Id);
            Assert.Equal("e2a5f830c0ee2c46", req.Context.Operation.ParentId);
            Assert.Equal("4e3083444c10254ba40513c7316332eb", trace.Context.Operation.Id);
            Assert.Equal(req.Id, trace.Context.Operation.ParentId);

            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "200",
                 expectedName: "GET " + path + "/Get",
                 expectedUrl: url,
                 expectedSuccess: true);
        }

        [Fact]
        public async Task RequestFailedWithTraceParent()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Home/Error";
            var url = client.BaseAddress + path;

            // Act
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "traceparent", "00-4e3083444c10254ba40513c7316332eb-e2a5f830c0ee2c46-00"}
                };
            var request = CreateRequestMessage(requestHeaders);
            request.RequestUri = new Uri(url);

            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            await WaitForTelemetryToArrive();
            var items = _factory.sentItems;
            PrintItems(items);
            Assert.Equal(2, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            var exceptions = GetTelemetryOfType<ExceptionTelemetry>(items);
            Assert.Single(exceptions);
            var exception = exceptions[0];
            Assert.NotNull(req);
            Assert.NotNull(exception);

            Assert.Equal("4e3083444c10254ba40513c7316332eb", req.Context.Operation.Id);
            Assert.Equal("4e3083444c10254ba40513c7316332eb", exception.Context.Operation.Id);
            Assert.Equal("e2a5f830c0ee2c46", req.Context.Operation.ParentId);
            Assert.Equal(req.Id, exception.Context.Operation.ParentId);

            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "500",
                 expectedName: "GET " + path,
                 expectedUrl: url,
                 expectedSuccess: false);
        }

        [Fact]
        public async Task RequestSuccessWithW3CCompatibleRequestId()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Home";
            var url = client.BaseAddress + path;

            // Act
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "Request-Id", "|40d1a5a08a68c0998e4a3b7c91915ca6.b9e41c35_1."}
                };
            var request = CreateRequestMessage(requestHeaders);
            request.RequestUri = new Uri(url);

            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            await WaitForTelemetryToArrive();
            var items = _factory.sentItems;
            PrintItems(items);
            // 1 Trace from Ilogger, 1 Request
            Assert.Equal(2, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            var traces = GetTelemetryOfType<TraceTelemetry>(items);
            Assert.Single(traces);
            var trace = traces[0];
            Assert.NotNull(req);
            Assert.NotNull(trace);

            Assert.Equal("40d1a5a08a68c0998e4a3b7c91915ca6", req.Context.Operation.Id);
            Assert.Equal("40d1a5a08a68c0998e4a3b7c91915ca6", trace.Context.Operation.Id);

            Assert.Equal("|40d1a5a08a68c0998e4a3b7c91915ca6.b9e41c35_1.", req.Context.Operation.ParentId);
            Assert.Equal(req.Id, trace.Context.Operation.ParentId);

            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "200",
                 expectedName: "GET " + path + "/Get",
                 expectedUrl: url,
                 expectedSuccess: true);
        }

        [Fact]
        public async Task RequestFailedWithW3CCompatibleRequestId()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Home/Error";
            var url = client.BaseAddress + path;

            // Act
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "Request-Id", "|40d1a5a08a68c0998e4a3b7c91915ca6.b9e41c35_1."}
                };
            var request = CreateRequestMessage(requestHeaders);
            request.RequestUri = new Uri(url);

            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            await WaitForTelemetryToArrive();
            var items = _factory.sentItems;
            PrintItems(items);
            Assert.Equal(2, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            var exceptions = GetTelemetryOfType<ExceptionTelemetry>(items);
            Assert.Single(exceptions);
            var exception = exceptions[0];
            Assert.NotNull(req);
            Assert.NotNull(exception);

            Assert.Equal("40d1a5a08a68c0998e4a3b7c91915ca6", req.Context.Operation.Id);
            Assert.Equal("40d1a5a08a68c0998e4a3b7c91915ca6", exception.Context.Operation.Id);

            Assert.Equal(req.Id, exception.Context.Operation.ParentId);
            Assert.Equal("|40d1a5a08a68c0998e4a3b7c91915ca6.b9e41c35_1.", req.Context.Operation.ParentId);
            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "500",
                 expectedName: "GET " + path,
                 expectedUrl: url,
                 expectedSuccess: false);
        }

        [Fact]
        public async Task RequestSuccessWithNonW3CCompatibleRequestId()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Home";
            var url = client.BaseAddress + path;

            // Act
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "Request-Id", "|noncompatible.b9e41c35_1."}
                };
            var request = CreateRequestMessage(requestHeaders);
            request.RequestUri = new Uri(url);

            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            await WaitForTelemetryToArrive();
            var items = _factory.sentItems;
            PrintItems(items);
            // 1 Trace from Ilogger, 1 Request
            Assert.Equal(2, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            var traces = GetTelemetryOfType<TraceTelemetry>(items);
            Assert.Single(traces);
            var trace = traces[0];
            Assert.NotNull(req);
            Assert.NotNull(trace);

            Assert.NotEqual("noncompatible", req.Context.Operation.Id);
            Assert.NotEqual("noncompatible", trace.Context.Operation.Id);

            Assert.Equal("|noncompatible.b9e41c35_1.", req.Context.Operation.ParentId);
            Assert.Equal(req.Id, trace.Context.Operation.ParentId);
            Assert.Equal("noncompatible", req.Properties["ai_legacyRootId"]);

            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "200",
                 expectedName: "GET " + path + "/Get",
                 expectedUrl: url,
                 expectedSuccess: true);
        }
    }
}
