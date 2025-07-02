namespace FunctionalTests.WebApi.Tests.FunctionalTest
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using Xunit.Abstractions;
    using System.Reflection;
    using global::FunctionalTests.Utils;

    public class RequestDependencyCorrelationTests : TelemetryTestsBase, IDisposable
    {
        private readonly string assemblyName;

        public RequestDependencyCorrelationTests(ITestOutputHelper output) : base (output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        public void TestBasicDependencyPropertiesAfterRequestingBasicPage()
        {
            const string RequestPath = "/api/values";

            using (var server = new InProcessServer(assemblyName, this.output))
            {
                DependencyTelemetry expected = new DependencyTelemetry();
                expected.ResultCode = "200";
                expected.Success = true;
                expected.Name = "GET " + RequestPath;
                expected.Data = server.BaseHost + RequestPath;

                this.ValidateBasicDependency(server, RequestPath, expected);
            }
        }

        // We may need to add more tests to cover Request + Dependency Tracking
        public void TestDependencyAndRequestWithW3CStandard()
        {
            const string RequestPath = "/api/values";

            using (var server = new InProcessServer(assemblyName, this.output))
            {
                DependencyTelemetry expected = new DependencyTelemetry
                {
                    ResultCode = "200",
                    Success = true,
                    Name = "GET " + RequestPath,
                    Data = server.BaseHost + RequestPath
                };

                var activity = new Activity("dummy")
                    .Start();

                var (request, dependency) = this.ValidateBasicDependency(server, RequestPath, expected);
                string expectedTraceId = activity.TraceId.ToHexString();
                string expectedParentSpanId = activity.SpanId.ToHexString();

                Assert.Equal(expectedTraceId, request.tags["ai.operation.id"]);
                Assert.Equal(expectedTraceId, dependency.tags["ai.operation.id"]);
                Assert.Equal(expectedParentSpanId, dependency.tags["ai.operation.parentId"]);
            }
        }

        public void Dispose()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }
    }
}
