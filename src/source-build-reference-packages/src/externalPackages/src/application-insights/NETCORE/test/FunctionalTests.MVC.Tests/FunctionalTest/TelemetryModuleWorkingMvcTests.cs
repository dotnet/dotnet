namespace FunctionalTests.MVC.Tests.FunctionalTest
{
    using System.Reflection;
    using FunctionalTests.Utils;
    using Microsoft.ApplicationInsights.DataContracts;
    using Xunit;
    using Xunit.Abstractions;

    public class TelemetryModuleWorkingMvcTests : TelemetryTestsBase
    {
        private readonly string assemblyName;

        public TelemetryModuleWorkingMvcTests(ITestOutputHelper output) : base(output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        [Fact]
        public void TestBasicDependencyPropertiesAfterRequestingBasicPage()
        {
            const string RequestPath = "/Home/About/5";

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

        [Fact]
        public void TestIfPerformanceCountersAreCollected()
        {
            ValidatePerformanceCountersAreCollected(assemblyName);
        }
    }
}
