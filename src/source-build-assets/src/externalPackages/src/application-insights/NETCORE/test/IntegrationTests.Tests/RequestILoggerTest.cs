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
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using IntegrationTests.WebApp;

namespace IntegrationTests.Tests
{
    public partial class RequestCollectionTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task RequestILoggerUserConfigOverRidesDefaultLevel()
        {
            // Arrange
            var loggerCategory = "IntegrationTests.WebApp.Controllers.HomeController";

            var client = _factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices((services) =>
            services.AddLogging(logBuilder => logBuilder.AddFilter<ApplicationInsightsLoggerProvider>(loggerCategory, LogLevel.Information))
            )).CreateClient();

            var path = "Home";
            var url = client.BaseAddress + path;

            // Act
            var request = CreateRequestMessage();
            request.RequestUri = new Uri(url);
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            await WaitForTelemetryToArrive();

            var items = _factory.sentItems;
            PrintItems(items);
            Assert.Equal(3, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            Assert.NotNull(req);

            var traces = GetTelemetryOfType<TraceTelemetry>(items);
            Assert.Equal(2, traces.Count);
            var trace1 = traces[0];
            var trace2 = traces[1];
            Assert.Equal(trace1.Context.Operation.ParentId, req.Id);

            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "200",
                 expectedName: "GET " + path + "/Get",
                 expectedUrl: url,
                 expectedSuccess: true);

            client.Dispose();
        }

        [Fact]
        public async Task IloggerWarningOrAboveCapturedByDefault()
        {
            // Arrange
            var client = _factory.CreateClient();

            var path = "Home";
            var url = client.BaseAddress + path;

            // Act
            var request = CreateRequestMessage();
            request.RequestUri = new Uri(url);
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            await WaitForTelemetryToArrive();

            var items = _factory.sentItems;
            PrintItems(items);
            Assert.Equal(2, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            Assert.NotNull(req);

            var traces = GetTelemetryOfType<TraceTelemetry>(items);
            Assert.Single(traces);
            var trace = traces[0];
            Assert.Equal(trace.Context.Operation.ParentId, req.Id);

            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "200",
                 expectedName: "GET " + path + "/Get",
                 expectedUrl: url,
                 expectedSuccess: true);

        }
    }
}
