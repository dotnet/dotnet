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
    using System.Reflection;
    using global::FunctionalTests.Utils;

    public class RequestCollectionTests : TelemetryTestsBase
    {
        private readonly string assemblyName;

        public RequestCollectionTests(ITestOutputHelper output) : base (output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        [Fact]
        public void TestIfPerformanceCountersAreCollected()
        {
            this.output.WriteLine("Validating perfcounters");
            ValidatePerformanceCountersAreCollected(assemblyName);
        }

        [Fact]
        public void TestBasicRequestPropertiesAfterRequestingControllerThatThrows()
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                const string RequestPath = "/api/exception";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Exception/Get";
                expectedRequestTelemetry.ResponseCode = "500";
                expectedRequestTelemetry.Success = false;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);

                // the is no response header because of https://github.com/Microsoft/ApplicationInsights-aspnetcore/issues/717
                this.ValidateBasicRequest(server, RequestPath, expectedRequestTelemetry, false);
            }
        }

        [Fact]
        public void TestBasicExceptionPropertiesAfterRequestingControllerThatThrows()
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                var expectedExceptionTelemetry = new ExceptionTelemetry();
                expectedExceptionTelemetry.Exception = new InvalidOperationException();

                this.ValidateBasicException(server, "/api/exception", expectedExceptionTelemetry);
            }
        }

        [Fact]
        public void TestBasicRequestPropertiesAfterRequestingValuesController()
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                const string RequestPath = "/api/values";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);

                Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "Request-Context", "appId=value"},
                };

                this.ValidateRequestWithHeaders(server, RequestPath, requestHeaders, expectedRequestTelemetry, expectRequestContextInResponse: true);
            }
        }

        [Fact]
        public void TestBasicRequestPropertiesAfterRequestingNotExistingController()
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                const string RequestPath = "/api/notexistingcontroller";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET /api/notexistingcontroller";
                expectedRequestTelemetry.ResponseCode = "404";
                expectedRequestTelemetry.Success = false;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);

                Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "Request-Context", "appId=value"},
                };

                this.ValidateRequestWithHeaders(server, RequestPath, requestHeaders, expectedRequestTelemetry, expectRequestContextInResponse: true);
            }
        }

        [Fact]
        public void TestBasicRequestPropertiesAfterRequestingWebApiShimRoute()
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                const string RequestPath = "/api/values/1";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get [id]";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);

                Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "Request-Context", "appId=value"},
                };

                this.ValidateRequestWithHeaders(server, RequestPath, requestHeaders, expectedRequestTelemetry, expectRequestContextInResponse: true);
            }
        }

        [Fact]
        public void TestNoHeadersInjectedInResponseWhenConfiguredAndNoIncomingRequestContext()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.RequestCollectionOptions.InjectResponseHeaders = false))
            {
                const string RequestPath = "/api/values/1";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get [id]";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);

                this.ValidateRequestWithHeaders(server, RequestPath, null, expectedRequestTelemetry, false);
            }
        }

        [Fact]
        public void TestNoHeadersInjectedInResponseWhenConfiguredAndWithIncomingRequestContext()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.RequestCollectionOptions.InjectResponseHeaders = false))
            {
                const string RequestPath = "/api/values/1";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get [id]";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);

                Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "Request-Context", "appId=value"},
                };

                this.ValidateRequestWithHeaders(server, RequestPath, requestHeaders, expectedRequestTelemetry, false);
            }
        }
    }
}

