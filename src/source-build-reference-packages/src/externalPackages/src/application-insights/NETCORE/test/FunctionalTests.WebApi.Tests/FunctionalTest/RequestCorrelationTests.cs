namespace FunctionalTests.WebApi.Tests.FunctionalTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.ApplicationInsights.DataContracts;
    using Xunit;
    using Xunit.Abstractions;
    using System.Linq;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using System.Text.RegularExpressions;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using System.Reflection;
    using global::FunctionalTests.Utils;

    public class RequestCorrelationTests : TelemetryTestsBase
    {
        private readonly string assemblyName;

        public RequestCorrelationTests(ITestOutputHelper output) : base(output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        [Fact]
        public void TestRequestWithNoCorrelationHeaders()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.EnableDependencyTrackingTelemetryModule = false))
            {
                const string RequestPath = "/api/values/1";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get [id]";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);

                Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    // No Request-ID, No TraceParent
                    { "Request-Context", "appId=value"},
                };

                var item = this.ValidateBasicRequest(server, RequestPath, expectedRequestTelemetry);

                Assert.Equal(32, item.tags["ai.operation.id"].Length);
                Assert.True(Regex.Match(item.tags["ai.operation.id"], @"[a-z][0-9]").Success);

                Assert.False(item.tags.ContainsKey("ai.operation.parentId"));
            }
        }

        [Fact]
        public void TestRequestWithRequestIdHeader()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.EnableDependencyTrackingTelemetryModule = false))
            {
                const string RequestPath = "/api/values";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new Uri(server.BaseHost + RequestPath);

                var headers = new Dictionary<string, string>
                {
                    // Request-ID Correlation Header
                    { "Request-Id", "|8ee8641cbdd8dd280d239fa2121c7e4e.df07da90a5b27d93."},
                    { "Request-Context", "appId=value"},
                    { "Correlation-Context"  , "k1=v1,k2=v2" }
                };

                var actualRequest = this.ValidateRequestWithHeaders(server, RequestPath, headers, expectedRequestTelemetry);

                Assert.Equal("8ee8641cbdd8dd280d239fa2121c7e4e", actualRequest.tags["ai.operation.id"]);
                Assert.Equal("|8ee8641cbdd8dd280d239fa2121c7e4e.df07da90a5b27d93.", actualRequest.tags["ai.operation.parentId"]);
                Assert.Equal("v1", actualRequest.data.baseData.properties["k1"]);
                Assert.Equal("v2", actualRequest.data.baseData.properties["k2"]);
            }
        }

        [Fact]
        public void TestRequestWithNonW3CCompatibleRequestIdHeader()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.EnableDependencyTrackingTelemetryModule = false))
            {
                const string RequestPath = "/api/values";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new Uri(server.BaseHost + RequestPath);

                var headers = new Dictionary<string, string>
                {
                    // Request-ID Correlation Header
                    { "Request-Id", "|noncompatible.df07da90a5b27d93."},
                    { "Request-Context", "appId=value"},
                    { "Correlation-Context"  , "k1=v1,k2=v2" }
                };

                var actualRequest = this.ValidateRequestWithHeaders(server, RequestPath, headers, expectedRequestTelemetry);

                Assert.NotEqual("noncompatible", actualRequest.tags["ai.operation.id"]);
                Assert.Equal("|noncompatible.df07da90a5b27d93.", actualRequest.tags["ai.operation.parentId"]);
                Assert.Equal("noncompatible", actualRequest.data.baseData.properties["ai_legacyRootId"]);
                Assert.Equal("v1", actualRequest.data.baseData.properties["k1"]);
                Assert.Equal("v2", actualRequest.data.baseData.properties["k2"]);
            }
        }

        [Fact]
        public void TestRequestWithNonW3CCompatibleNonHierrachicalRequestIdHeader()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.EnableDependencyTrackingTelemetryModule = false))
            {
                const string RequestPath = "/api/values";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new Uri(server.BaseHost + RequestPath);

                var headers = new Dictionary<string, string>
                {
                    // Request-ID Correlation Header
                    { "Request-Id", "somerandomidnotinanyformat"},
                    { "Request-Context", "appId=value"},
                    { "Correlation-Context"  , "k1=v1,k2=v2" }
                };

                var actualRequest = this.ValidateRequestWithHeaders(server, RequestPath, headers, expectedRequestTelemetry);

                Assert.NotEqual("noncompatible", actualRequest.tags["ai.operation.id"]);
                Assert.Equal("somerandomidnotinanyformat", actualRequest.tags["ai.operation.parentId"]);
                Assert.Equal("somerandomidnotinanyformat", actualRequest.data.baseData.properties["ai_legacyRootId"]);
                Assert.Equal("v1", actualRequest.data.baseData.properties["k1"]);
                Assert.Equal("v2", actualRequest.data.baseData.properties["k2"]);
            }
        }

        [Fact]
        public void TestRequestWithTraceParentHeader()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.EnableDependencyTrackingTelemetryModule = false))
            {
                const string RequestPath = "/api/values";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new Uri(server.BaseHost + RequestPath);

                var headers = new Dictionary<string, string>
                {
                    // TraceParent Correlation Header
                    ["traceparent"] = "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
                    ["tracestate"] = "some=state",
                    ["Correlation-Context"] = "k1=v1,k2=v2"
                };

                var actualRequest = this.ValidateRequestWithHeaders(server, RequestPath, headers, expectedRequestTelemetry);

                Assert.Equal("4bf92f3577b34da6a3ce929d0e0e4736", actualRequest.tags["ai.operation.id"]);
                Assert.Equal("00f067aa0ba902b7", actualRequest.tags["ai.operation.parentId"]);

                // Correlation-Context will be read if either Request-Id or TraceParent available.
                Assert.True(actualRequest.data.baseData.properties.ContainsKey("k1"));
                Assert.True(actualRequest.data.baseData.properties.ContainsKey("k2"));

                // TraceState is simply set to Activity, and not added to Telemetry.
                Assert.False(actualRequest.data.baseData.properties.ContainsKey("some"));
            }
        }

        [Fact]
        public void TestRequestWithRequestIdAndTraceParentHeader()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.EnableDependencyTrackingTelemetryModule = false))
            {
                const string RequestPath = "/api/values";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new Uri(server.BaseHost + RequestPath);

                var headers = new Dictionary<string, string>
                {
                    // Both request id and traceparent
                    ["Request-Id"] = "|8ee8641cbdd8dd280d239fa2121c7e4e.df07da90a5b27d93.",
                    ["traceparent"] = "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
                    ["tracestate"] = "some=state",
                    ["Correlation-Context"] = "k1=v1,k2=v2"
                };

                var actualRequest = this.ValidateRequestWithHeaders(server, RequestPath, headers, expectedRequestTelemetry);

                Assert.Equal("4bf92f3577b34da6a3ce929d0e0e4736", actualRequest.tags["ai.operation.id"]);
                Assert.NotEqual("8ee8641cbdd8dd280d239fa2121c7e4e", actualRequest.tags["ai.operation.id"]);
                Assert.Equal("00f067aa0ba902b7", actualRequest.tags["ai.operation.parentId"]);

                // Correlation-Context will be read if either Request-Id or traceparent is present.
                Assert.True(actualRequest.data.baseData.properties.ContainsKey("k1"));
                Assert.True(actualRequest.data.baseData.properties.ContainsKey("k2"));

                // TraceState is simply set to Activity, and not added to Telemetry.
                Assert.False(actualRequest.data.baseData.properties.ContainsKey("some"));
            }
        }

        [Fact]
        public void TestRequestWithRequestIdAndTraceParentHeaderWithW3CDisabled()
        {
            try
            {
                // disable w3c
                Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
                Activity.ForceDefaultIdFormat = true;

                using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.EnableDependencyTrackingTelemetryModule = false))
                {
                    const string RequestPath = "/api/values";

                    var expectedRequestTelemetry = new RequestTelemetry();
                    expectedRequestTelemetry.Name = "GET Values/Get";
                    expectedRequestTelemetry.ResponseCode = "200";
                    expectedRequestTelemetry.Success = true;
                    expectedRequestTelemetry.Url = new Uri(server.BaseHost + RequestPath);

                    var headers = new Dictionary<string, string>
                    {
                        // Both request id and traceparent
                        ["Request-Id"] = "|8ee8641cbdd8dd280d239fa2121c7e4e.df07da90a5b27d93.",
                        ["traceparent"] = "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
                        ["tracestate"] = "some=state",
                        ["Correlation-Context"] = "k1=v1,k2=v2"
                    };

                    var actualRequest = this.ValidateRequestWithHeaders(server, RequestPath, headers, expectedRequestTelemetry);

                    Assert.Equal("8ee8641cbdd8dd280d239fa2121c7e4e", actualRequest.tags["ai.operation.id"]);
                    Assert.NotEqual("4bf92f3577b34da6a3ce929d0e0e4736", actualRequest.tags["ai.operation.id"]);
                    Assert.Contains("df07da90a5b27d93", actualRequest.tags["ai.operation.parentId"]);

                    // Correlation-Context should be read and populated.
                    Assert.True(actualRequest.data.baseData.properties.ContainsKey("k1"));
                    Assert.True(actualRequest.data.baseData.properties.ContainsKey("k2"));
                }
            }
            finally
            {
                Activity.DefaultIdFormat = ActivityIdFormat.W3C;
                Activity.ForceDefaultIdFormat = true;
            }
        }
    }
}
