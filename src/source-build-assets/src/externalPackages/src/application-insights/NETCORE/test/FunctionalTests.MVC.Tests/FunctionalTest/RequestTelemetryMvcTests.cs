namespace FunctionalTests.MVC.Tests.FunctionalTest
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using AI;
    using FunctionalTests.Utils;
    using Microsoft.ApplicationInsights.DataContracts;
    using Xunit;
    using Xunit.Abstractions;

    public class RequestTelemetryMvcTests : TelemetryTestsBase
    {
        private readonly string assemblyName;

        public RequestTelemetryMvcTests(ITestOutputHelper output) : base(output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        [Fact]
        public void TestMixedTelemetryItemsReceived()
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                this.ExecuteRequest(server.BaseHost + "/Home/Contact");

                var telemetries = server.Listener.ReceiveItems(TestListenerTimeoutInMs);
                this.DebugTelemetryItems(telemetries);

                // In linux 6 items as it'll always have an Error trace from SDK itself about
                // local storage folder.
                Assert.InRange(telemetries.Length, 5, 6);

                Assert.Contains(telemetries.OfType<TelemetryItem<RemoteDependencyData>>(),
                    t => ((TelemetryItem<RemoteDependencyData>)t).data.baseData.name == "GET /Home/Contact");

                Assert.Contains(telemetries.OfType<TelemetryItem<RequestData>>(),
                    t => ((TelemetryItem<RequestData>)t).data.baseData.name == "GET Home/Contact");

                Assert.Contains(telemetries.OfType<TelemetryItem<EventData>>(),
                    t => ((TelemetryItem<EventData>)t).data.baseData.name == "GetContact");

                Assert.Contains(telemetries.OfType<TelemetryItem<MetricData>>(),
                    t => ((TelemetryItem<MetricData>)t).data.baseData.metrics[0].name == "ContactFile" && ((TelemetryItem<MetricData>)t).data.baseData.metrics[0].value == 1);

                Assert.Contains(telemetries.OfType<TelemetryItem<MessageData>>(),
                    t => ((TelemetryItem<MessageData>)t).data.baseData.message == "Fetched contact details." && ((TelemetryItem<MessageData>)t).data.baseData.severityLevel == AI.SeverityLevel.Information);
            }
        }
    }
}
