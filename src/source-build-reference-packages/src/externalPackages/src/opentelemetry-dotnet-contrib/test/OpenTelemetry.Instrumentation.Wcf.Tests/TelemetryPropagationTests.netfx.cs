// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using OpenTelemetry.Trace;
using Xunit;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

[Collection("WCF")]
public class TelemetryPropagationTests
{
    [RunAsAdminTheory]
    [InlineData("tcp")]
    [InlineData("http")]
    [InlineData("rest")]
    [InlineData("http", false, true)]
    [InlineData("tcp", false, true)]
    [InlineData("rest", false, false)]
    public async Task TelemetryContextPropagatesTest(
        string endpoint,
        bool suppressDownstreamInstrumentation = true,
        bool shouldPropagate = true)
    {
        using var context = new ServiceHostContext();

        var stoppedActivities = new List<Activity>();
        using var activityListener = new ActivityListener
        {
            ShouldListenTo = activitySource => true,
            ActivityStopped = stoppedActivities.Add,
        };
        ActivitySource.AddActivityListener(activityListener);

        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddWcfInstrumentation(options => options.SuppressDownstreamInstrumentation = suppressDownstreamInstrumentation)
            .Build();

        var serviceBase = endpoint == "tcp" ? context.ServiceBaseUriTcp : context.ServiceBaseUriHttp;
        Binding binding = endpoint switch
        {
            "tcp" => new NetTcpBinding(),
            "http" => new BasicHttpBinding(),
            "rest" => new WebHttpBinding(),
            _ => throw new ArgumentException("Invalid endpoint type", nameof(endpoint)),
        };
        var client = new ServiceClient(binding, new EndpointAddress(new Uri(serviceBase, $"/{endpoint}")));
        try
        {
            client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
            if (endpoint == "rest")
            {
                client.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
            }

            await client.ExecuteAsync(new ServiceRequest(payload: "Hello Open Telemetry!"));
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

        Assert.Equal(2, stoppedActivities.Count);
        if (shouldPropagate)
        {
            var clientSpan = stoppedActivities.Single(activity => activity.ParentId == null);
            var serverSpan = stoppedActivities.Single(activity => activity.ParentId == clientSpan.Id);
            Assert.Equal(clientSpan.TraceId, serverSpan.TraceId);
        }
        else
        {
            Assert.All(stoppedActivities, activity => Assert.Null(activity.ParentId));
            Assert.NotEqual(stoppedActivities[0].TraceId, stoppedActivities[1].TraceId);
        }
    }

    private sealed class ServiceHostContext : IDisposable
    {
        private readonly Uri serviceBaseUriTcp;
        private readonly Uri serviceBaseUriHttp;
        private readonly ServiceHost serviceHost;

        public ServiceHostContext()
        {
            var random = new Random();
            var attempts = 0;
            var retryCount = 5;
            ServiceHost? createdHost = null;
            while (retryCount > 0)
            {
                attempts++;

                try
                {
                    this.serviceBaseUriTcp = new Uri($"net.tcp://localhost:{random.Next(2000, 5000)}/");
                    this.serviceBaseUriHttp = new Uri($"http://localhost:{random.Next(2000, 5000)}/");
                    createdHost = new ServiceHost(new Service(), this.serviceBaseUriTcp, this.serviceBaseUriHttp);
                    var tcpEndpoint = createdHost.AddServiceEndpoint(typeof(IServiceContract), new NetTcpBinding(), "/tcp");
                    tcpEndpoint.Behaviors.Add(new TelemetryEndpointBehavior());
                    var httpEndpoint = createdHost.AddServiceEndpoint(typeof(IServiceContract), new BasicHttpBinding(), "/http");
                    httpEndpoint.Behaviors.Add(new TelemetryEndpointBehavior());
                    var restEndpoint = createdHost.AddServiceEndpoint(typeof(IServiceContract), new WebHttpBinding(), "/rest");
                    restEndpoint.Behaviors.Add(new TelemetryEndpointBehavior());
                    restEndpoint.Behaviors.Add(new WebHttpBehavior());
                    createdHost.Open();
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to start ServiceHost: {ex}");

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

            if (createdHost == null || this.serviceBaseUriTcp == null || this.serviceBaseUriHttp == null)
            {
                throw new InvalidOperationException($"ServiceHost could not be started after {attempts} attempts.");
            }

            this.serviceHost = createdHost;
        }

        public Uri ServiceBaseUriTcp => this.serviceBaseUriTcp;

        public Uri ServiceBaseUriHttp => this.serviceBaseUriHttp;

        public ServiceHost ServiceHost => this.serviceHost;

        public void Dispose() => this.serviceHost?.Close();
    }
}
#endif
