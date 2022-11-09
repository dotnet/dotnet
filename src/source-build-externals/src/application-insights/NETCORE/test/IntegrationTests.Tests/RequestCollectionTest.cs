using IntegrationTests.WebApp;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    public partial class RequestCollectionTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        protected readonly ITestOutputHelper output;

        public RequestCollectionTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            this.output = output;
            _factory = factory;
            _factory.sentItems.Clear();
        }

        [Fact]
        public async Task RequestSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Home/Empty";
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
            Assert.Equal(1, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            Assert.NotNull(req);
            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "200",
                 expectedName: "GET " + path,
                 expectedUrl: url,
                 expectedSuccess: true);
        }

        [Fact]
        public async Task RequestSuccessActionWithParameter()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Home/5";
            var expectedName = "GET Home/Get [id]";
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
            Assert.NotNull(trace);

            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "200",
                 expectedName: expectedName,
                 expectedUrl: url,
                 expectedSuccess: true);
        }

        [Fact]
        public async Task RequestFailed()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Home/Error";
            var url = client.BaseAddress + path;

            // Act
            var request = CreateRequestMessage();
            request.RequestUri = new Uri(url);
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            await WaitForTelemetryToArrive();

            var items = _factory.sentItems;
            PrintItems(items);
            Assert.Equal(2, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            var exceptions = GetTelemetryOfType<ExceptionTelemetry>(items);
            Assert.Single(reqs);
            Assert.Single(exceptions);

            var req = reqs[0];
            var exc = exceptions[0];
            Assert.NotNull(req);
            Assert.NotNull(exc);

            Assert.Equal(exc.Context.Operation.Id, req.Context.Operation.Id);
            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "500",
                 expectedName: "GET " + path,
                 expectedUrl: url,
                 expectedSuccess: false);
        }

        [Fact]
        public async Task RequestNonExistentPage()
        {
            // Arrange
            var client = _factory.CreateClient();
            var path = "Nonexistent";
            var url = client.BaseAddress + path;

            // Act
            var request = CreateRequestMessage();
            request.RequestUri = new Uri(url);
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            await WaitForTelemetryToArrive();

            var items = _factory.sentItems;
            PrintItems(items);
            Assert.Equal(1, items.Count);

            var reqs = GetTelemetryOfType<RequestTelemetry>(items);
            Assert.Single(reqs);
            var req = reqs[0];
            Assert.NotNull(req);

            ValidateRequest(
                 requestTelemetry: req,
                 expectedResponseCode: "404",
                 expectedName: "GET /" + path,
                 expectedUrl: url,
                 expectedSuccess: false);
        }

        private async Task WaitForTelemetryToArrive()
        {
            // The response to the test server request is completed
            // before the actual telemetry is sent from HostingDiagnosticListener.
            // This could be a TestServer issue/feature. (In a real application, the response is not
            // sent to the user until TrackRequest() is called.)
            // The simplest workaround is to do a wait here.
            // This could be improved when entire functional tests are migrated to use this pattern.
            await Task.Delay(1000);
        }

        private void ValidateRequest(RequestTelemetry requestTelemetry,
            string expectedResponseCode,
            string expectedName,
            string expectedUrl,
            bool expectedSuccess)
        {
            Assert.Equal(expectedResponseCode, requestTelemetry.ResponseCode);
            Assert.Equal(expectedName, requestTelemetry.Name);
            Assert.Equal(expectedSuccess, requestTelemetry.Success);
            Assert.Equal(expectedUrl, requestTelemetry.Url.ToString());
            Assert.True(requestTelemetry.Duration.TotalMilliseconds > 0);
            // requestTelemetry.Timestamp
        }

        private HttpRequestMessage CreateRequestMessage(Dictionary<string, string> requestHeaders = null)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Get;
            if (requestHeaders != null)
            {
                foreach (var h in requestHeaders)
                {
                    httpRequestMessage.Headers.Add(h.Key, h.Value);
                }
            }

            return httpRequestMessage;
        }

        private List<T> GetTelemetryOfType<T>(ConcurrentBag<ITelemetry> items)
        {
            List<T> foundItems = new List<T>();
            foreach (var item in items)
            {
                if (item is T)
                {
                    foundItems.Add((T)item);
                }
            }

            return foundItems;
        }

        private void PrintItems(ConcurrentBag<ITelemetry> items)
        {
            int i = 1;
            foreach (var item in items)
            {
                this.output.WriteLine("Item " + (i++) + ".");

                if (item is RequestTelemetry req)
                {
                    this.output.WriteLine("RequestTelemetry");
                    this.output.WriteLine(req.Name);
                    this.output.WriteLine(req.Duration.ToString());
                }
                else if (item is DependencyTelemetry dep)
                {
                    this.output.WriteLine("DependencyTelemetry");
                    this.output.WriteLine(dep.Name);
                }
                else if (item is TraceTelemetry trace)
                {
                    this.output.WriteLine("TraceTelemetry");
                    this.output.WriteLine(trace.Message);
                }
                else if (item is ExceptionTelemetry exc)
                {
                    this.output.WriteLine("ExceptionTelemetry");
                    this.output.WriteLine(exc.Message);
                }
                else if (item is MetricTelemetry met)
                {
                    this.output.WriteLine("MetricTelemetry");
                    this.output.WriteLine(met.Name + "" + met.Sum);
                }

                PrintProperties(item as ISupportProperties);
                this.output.WriteLine("----------------------------");
            }
        }

        private void PrintProperties(ISupportProperties itemProps)
        {
            foreach (var prop in itemProps.Properties)
            {
                this.output.WriteLine(prop.Key + ":" + prop.Value);
            }
        }

    }
}
