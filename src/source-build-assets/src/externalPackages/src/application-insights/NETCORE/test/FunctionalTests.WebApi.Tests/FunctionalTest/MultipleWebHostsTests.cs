using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using AI;
using FunctionalTests.Utils;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.WebApi.Tests.FunctionalTest
{
    public class MultipleWebHostsTests : TelemetryTestsBase
    {
        private readonly string assemblyName;
        private const string requestPath = "/api/dependency";

        public MultipleWebHostsTests(ITestOutputHelper output) : base(output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        [Fact]
        public void TwoWebHostsCreatedSequentially()
        {
            using (var server1 = new InProcessServer(assemblyName, this.output))
            {
                this.ExecuteRequest(server1.BaseHost + requestPath);
                var telemetry = server1.Listener.ReceiveItems(TestListenerTimeoutInMs);
                this.DebugTelemetryItems(telemetry);

                Assert.Single(telemetry.Where(t => t is TelemetryItem<RequestData>));
                Assert.Single(telemetry.Where(IsServiceDependencyCall));
                Assert.DoesNotContain(telemetry, t => t is TelemetryItem<ExceptionData>);

                var request = telemetry.Single(t => t is TelemetryItem<RequestData>);
                Assert.Equal("200", ((TelemetryItem<RequestData>)request).data.baseData.responseCode);
            }

            using (var server2 = new InProcessServer(assemblyName, this.output))
            {
                this.ExecuteRequest(server2.BaseHost + requestPath);
                var telemetry = server2.Listener.ReceiveItems(TestListenerTimeoutInMs);
                this.DebugTelemetryItems(telemetry);

                Assert.Single(telemetry.Where(t => t is TelemetryItem<RequestData>));
                Assert.Single(telemetry.Where(IsServiceDependencyCall));
                Assert.DoesNotContain(telemetry, t => t is TelemetryItem<ExceptionData>);

                var request = telemetry.Single(t => t is TelemetryItem<RequestData>);
                Assert.Equal("200", ((TelemetryItem<RequestData>)request).data.baseData.responseCode);
            }
        }

        [Fact]
        public void TwoWebHostsCreatedInParallel()
        {
            using (var server1 = new InProcessServer(assemblyName, this.output))
            using (var server2 = new InProcessServer(assemblyName, this.output))
            {
                this.ExecuteRequest(server1.BaseHost + requestPath);
                this.ExecuteRequest(server2.BaseHost + requestPath);

                var telemetry1 = server1.Listener.ReceiveItems(TestListenerTimeoutInMs);
                var telemetry2 = server2.Listener.ReceiveItems(TestListenerTimeoutInMs);

                this.output.WriteLine("~~telemetry1~~");
                this.DebugTelemetryItems(telemetry1);
                this.output.WriteLine("~~telemetry2~~");
                this.DebugTelemetryItems(telemetry2);

                // we don't know which host reported requests
                Assert.True(2 == telemetry1.Count(t => t is TelemetryItem<RequestData>) ||
                            2 == telemetry2.Count(t => t is TelemetryItem<RequestData>));

                // we don't know which host reported dependencies
                Assert.True(2 == telemetry1.Count(IsServiceDependencyCall) ||
                            2 == telemetry2.Count(IsServiceDependencyCall));

                Assert.DoesNotContain(telemetry1, t => t is TelemetryItem<ExceptionData>);

                var request1 = telemetry1.First(t => t is TelemetryItem<RequestData>);
                var request2 = telemetry1.Last(t => t is TelemetryItem<RequestData>);
                Assert.Equal("200", ((TelemetryItem<RequestData>)request1).data.baseData.responseCode);
                Assert.Equal("200", ((TelemetryItem<RequestData>)request2).data.baseData.responseCode);
            }
        }

        [Fact]
        public void TwoWebHostsOneIsDisposed()
        {
            using (var server1 = new InProcessServer(assemblyName, this.output))
            {
                var config1 = (TelemetryConfiguration) server1.ApplicationServices.GetService(typeof(TelemetryConfiguration));
                this.ExecuteRequest(server1.BaseHost);

                using (var server2 = new InProcessServer(assemblyName, this.output))
                {
                    var config2 = (TelemetryConfiguration) server2.ApplicationServices.GetService(typeof(TelemetryConfiguration));

                    Assert.NotEqual(config1, config2);
                    Assert.NotEqual(config1.TelemetryChannel, config2.TelemetryChannel);

                    this.ExecuteRequest(server2.BaseHost);
                }

                Assert.NotNull(config1.TelemetryChannel);

                this.ExecuteRequest(server1.BaseHost + requestPath);

                var telemetry = server1.Listener.ReceiveItems(TestListenerTimeoutInMs);
                this.DebugTelemetryItems(telemetry);

                Assert.NotEmpty(telemetry.Where(t => t is TelemetryItem<RequestData>));
                var request = telemetry.Single(IsValueControllerRequest);
                Assert.Equal("200", ((TelemetryItem<RequestData>) request).data.baseData.responseCode);

                Assert.DoesNotContain(telemetry, t => t is TelemetryItem<ExceptionData>);
                Assert.Single(telemetry.Where(IsServiceDependencyCall));
            }
        }

        [Fact]
        public void ActiveConfigurationIsNotCorruptedAfterWebHostIsDisposed()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // https://github.com/dotnet/corefx/issues/25016
                // HttpListener on Linux/MacOS cannot be reused on the same port until connection
                // is closed on the OS level. So we run this test on windows only.
                return;
            }

            // Active config could be used multiple times in the same process before this test
            // let's reassign it

#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.Dispose();
#pragma warning restore CS0618 // Type or member is obsolete
            MethodInfo setActive =
                typeof(TelemetryConfiguration).GetMethod("set_Active", BindingFlags.Static | BindingFlags.NonPublic);
            setActive.Invoke(null, new object[] { TelemetryConfiguration.CreateDefault() });

#pragma warning disable CS0618 // Type or member is obsolete
            var activeConfig = TelemetryConfiguration.Active;
#pragma warning restore CS0618 // Type or member is obsolete
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.EnableActiveTelemetryConfigurationSetup = true))
            {
                this.ExecuteRequest(server.BaseHost + requestPath);

                server.DisposeHost();
                Assert.NotNull(activeConfig.TelemetryChannel);

                var telemetryClient = new TelemetryClient(activeConfig);
                telemetryClient.TrackTrace("some message after web host is disposed");

                var message = server.Listener.ReceiveItemsOfType<TelemetryItem<MessageData>>(1, TestListenerTimeoutInMs);
                Assert.Single(message);

                this.output.WriteLine(((TelemetryItem<MessageData>)message.Single()).data.baseData.message);

                Assert.Equal("some message after web host is disposed", ((TelemetryItem<MessageData>)message.Single()).data.baseData.message);
            }
        }

        private bool IsServiceDependencyCall(Envelope item)
        {
            if (!(item is TelemetryItem<RemoteDependencyData> dependency))
            {
                return false;
            }

            var url = dependency.data.baseData.data;

            // check if it's not tracked call from service to the test and a not call to get appid
            return url.Contains("microsoft.com");
        }

        private bool IsValueControllerRequest(Envelope item)
        {
            if (!(item is TelemetryItem<RequestData> dependency))
            {
                return false;
            }

            var url = dependency.data.baseData.url;

            // check if it's not tracked call from service to the test and a not call to get appid
            return url.Contains(requestPath);
        }
    }
}
