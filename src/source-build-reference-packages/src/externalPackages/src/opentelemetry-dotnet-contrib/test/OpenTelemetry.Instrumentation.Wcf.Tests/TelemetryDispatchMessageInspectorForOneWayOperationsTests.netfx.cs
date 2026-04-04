// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Diagnostics;
using System.ServiceModel;
using OpenTelemetry.Instrumentation.Wcf.Tests.Tools;
using OpenTelemetry.Trace;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

[Collection("WCF")]
public class TelemetryDispatchMessageInspectorForOneWayOperationsTests : IDisposable
{
    private readonly ITestOutputHelper output;
    private readonly Uri serviceBaseUri;
    private readonly ServiceHost serviceHost;

    private readonly EventWaitHandle thrownExceptionsHandle = new(false, EventResetMode.ManualReset);
    private readonly List<Exception> thrownExceptions = [];

    public TelemetryDispatchMessageInspectorForOneWayOperationsTests(ITestOutputHelper outputHelper)
    {
        this.output = outputHelper;

        var random = new Random();
        var retryCount = 5;
        ServiceHost? createdHost = null;
        while (retryCount > 0)
        {
            try
            {
                this.serviceBaseUri = new Uri($"net.tcp://localhost:{random.Next(2000, 5000)}/");
                createdHost = new ServiceHost(new Service(), this.serviceBaseUri);

                var endpoint = createdHost.AddServiceEndpoint(
                    typeof(IServiceContract),
                    new NetTcpBinding(),
                    "/Service");
                endpoint.Behaviors.Add(new TelemetryEndpointBehavior());

                createdHost.Description.Behaviors.Add(
                    new ErrorHandlerServiceBehavior(this.thrownExceptionsHandle, this.thrownExceptions.Add));

                createdHost.Open();

                break;
            }
            catch (Exception ex)
            {
                this.output.WriteLine(ex.ToString());
                if (createdHost?.State == CommunicationState.Faulted)
                {
                    createdHost.Abort();
                }
                else
                {
                    createdHost?.Close();
                }

                createdHost = null;
                retryCount--;
            }
        }

        if (createdHost == null || this.serviceBaseUri == null)
        {
            throw new InvalidOperationException("ServiceHost could not be started.");
        }

        this.serviceHost = createdHost;
    }

    public void Dispose()
    {
        this.serviceHost?.Close();
        this.thrownExceptionsHandle?.Dispose();
    }

    [Fact]
    public void IncomingRequestOneWayOperationInstrumentationTest()
    {
        List<Activity> stoppedActivities = [];

        using var activityListener = new ActivityListener
        {
            ShouldListenTo = activitySource => true,
            ActivityStopped = stoppedActivities.Add,
        };

        ActivitySource.AddActivityListener(activityListener);
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddWcfInstrumentation()
            .Build();

        var client = new ServiceClient(
            new NetTcpBinding(),
            new EndpointAddress(new Uri(this.serviceBaseUri, "/Service")));

        try
        {
            client.ExecuteWithOneWay(new ServiceRequest(payload: "Hello Open Telemetry!"));
            this.thrownExceptionsHandle.WaitOne(3000);
        }
        finally
        {
            if (client.State == CommunicationState.Faulted)
            {
                client.Abort();
            }
            else
            {
                client.Close();
            }

            tracerProvider?.Shutdown();
            tracerProvider?.Dispose();

            WcfInstrumentationActivitySource.Options = null;
        }

        // Assert
        Assert.Empty(this.thrownExceptions);

        Assert.NotEmpty(stoppedActivities);
        Assert.Single(stoppedActivities);

        var activity = stoppedActivities[0];
        Assert.Equal("http://opentelemetry.io/Service/ExecuteWithOneWay", activity.DisplayName);
        Assert.Equal("ExecuteWithOneWay", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.RpcMethodTag).Value);
        Assert.DoesNotContain(activity.TagObjects, t => t.Key == WcfInstrumentationConstants.SoapReplyActionTag);

        Assert.Equal(WcfInstrumentationActivitySource.IncomingRequestActivityName, activity.OperationName);
        Assert.Equal(WcfInstrumentationConstants.WcfSystemValue, activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.RpcSystemTag).Value);
        Assert.Equal("http://opentelemetry.io/Service", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.RpcServiceTag).Value);
        Assert.Equal(this.serviceBaseUri.Host, activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.NetHostNameTag).Value);
        Assert.Equal(this.serviceBaseUri.Port, activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.NetHostPortTag).Value);
        Assert.Equal("net.tcp", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.WcfChannelSchemeTag).Value);
        Assert.Equal("/Service", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.WcfChannelPathTag).Value);
    }
}

#endif
