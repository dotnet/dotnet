// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Diagnostics;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using OpenTelemetry.Instrumentation.Wcf.Tests.Tools;
using OpenTelemetry.Trace;
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

[Collection("WCF")]
public class TelemetryBindingElementForTcpTests : IDisposable
{
    private readonly ITestOutputHelper output;
    private readonly Uri serviceBaseUri;
    private readonly ServiceHost serviceHost;

    public TelemetryBindingElementForTcpTests(ITestOutputHelper outputHelper)
    {
        this.output = outputHelper;
        this.serviceHost = this.CreateServiceHost(new NetTcpBinding(SecurityMode.None), null);
        this.serviceBaseUri = this.serviceHost.BaseAddresses[0];
    }

    public void Dispose()
    {
        this.serviceHost?.Close();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(true, true)]
    [InlineData(true, false, false)]
    [InlineData(false)]
    [InlineData(true, false, true, true)]
    [InlineData(true, false, true, true, true)]
    [InlineData(true, false, true, true, true, true)]
    [InlineData(true, false, true, true, true, true, true)]
    public async Task OutgoingRequestInstrumentationTest(
        bool instrument,
        bool filter = false,
        bool suppressDownstreamInstrumentation = true,
        bool includeVersion = false,
        bool enrich = false,
        bool enrichmentException = false,
        bool emptyOrNullAction = false)
    {
        List<Activity> stoppedActivities = [];

        var builder = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(stoppedActivities);

        if (instrument)
        {
            builder
                .AddWcfInstrumentation(options =>
                {
                    if (enrich)
                    {
                        options.Enrich = enrichmentException
                            ? (_, _, _) => throw new Exception("Error while enriching activity")
                            : (activity, eventName, _) =>
                            {
                                switch (eventName)
                                {
                                    case WcfEnrichEventNames.BeforeSendRequest:
                                        activity.SetTag("client.beforesendrequest", WcfEnrichEventNames.BeforeSendRequest);
                                        break;
                                    case WcfEnrichEventNames.AfterReceiveReply:
                                        activity.SetTag("client.afterreceivereply", WcfEnrichEventNames.AfterReceiveReply);
                                        break;
                                    default:
                                        break;
                                }
                            };
                    }

                    options.OutgoingRequestFilter = _ => !filter;
                    options.SuppressDownstreamInstrumentation = suppressDownstreamInstrumentation;
                    options.SetSoapMessageVersion = includeVersion;
                })
                .AddDownstreamInstrumentation();
        }

        var tracerProvider = builder.Build();

        var client = new ServiceClient(
            new NetTcpBinding(SecurityMode.None),
            new EndpointAddress(new Uri(this.serviceBaseUri, "/Service")));
        try
        {
            client.Endpoint.EndpointBehaviors.Add(new DownstreamInstrumentationEndpointBehavior());
            client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());

            if (emptyOrNullAction)
            {
                await client.ExecuteWithEmptyActionNameAsync(
                    new ServiceRequest(
                        payload: "Hello Open Telemetry!"));
            }
            else
            {
                await client.ExecuteAsync(
                    new ServiceRequest(
                        payload: "Hello Open Telemetry!"));
            }
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

        if (instrument)
        {
            if (!suppressDownstreamInstrumentation)
            {
                Assert.NotEmpty(stoppedActivities);
                if (filter)
                {
                    Assert.Single(stoppedActivities);
                    Assert.Equal("DownstreamInstrumentation", stoppedActivities[0].OperationName);
                }
                else
                {
                    Assert.Equal(2, stoppedActivities.Count);
                    Assert.NotNull(stoppedActivities.SingleOrDefault(activity => activity.OperationName == "DownstreamInstrumentation"));
                    Assert.NotNull(stoppedActivities.SingleOrDefault(activity => activity.OperationName == WcfInstrumentationActivitySource.OutgoingRequestActivityName));
                }
            }
            else
            {
                if (!filter)
                {
                    Assert.NotEmpty(stoppedActivities);
                    Assert.Single(stoppedActivities);

                    var activity = stoppedActivities[0];

                    if (emptyOrNullAction)
                    {
                        Assert.Equal(WcfInstrumentationActivitySource.OutgoingRequestActivityName, activity.DisplayName);
                        Assert.Equal("ExecuteWithEmptyActionName", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.RpcMethodTag).Value);
                    }
                    else
                    {
                        Assert.Equal("http://opentelemetry.io/Service/Execute", activity.DisplayName);
                        Assert.Equal("Execute", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.RpcMethodTag).Value);
                    }

                    Assert.Equal(WcfInstrumentationActivitySource.OutgoingRequestActivityName, activity.OperationName);
                    Assert.Equal(WcfInstrumentationConstants.WcfSystemValue, activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.RpcSystemTag).Value);
                    Assert.Equal("http://opentelemetry.io/Service", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.RpcServiceTag).Value);
                    Assert.Equal(this.serviceBaseUri.Host, activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.NetPeerNameTag).Value);
                    Assert.Equal(this.serviceBaseUri.Port, activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.NetPeerPortTag).Value);
                    Assert.Equal("net.tcp", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.WcfChannelSchemeTag).Value);
                    Assert.Equal("/Service", activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.WcfChannelPathTag).Value);
                    if (includeVersion)
                    {
                        var value = activity.TagObjects.FirstOrDefault(t => t.Key == WcfInstrumentationConstants.SoapMessageVersionTag).Value!.ToString();
                        Assert.Matches("""Soap.* \(http.*\) Addressing.* \(http.*\)""", value);
                    }

                    if (enrich && !enrichmentException)
                    {
                        Assert.Equal(WcfEnrichEventNames.BeforeSendRequest, activity.TagObjects.Single(t => t.Key == "client.beforesendrequest").Value);
                        Assert.Equal(WcfEnrichEventNames.AfterReceiveReply, activity.TagObjects.Single(t => t.Key == "client.afterreceivereply").Value);
                    }
                }
                else
                {
                    Assert.Empty(stoppedActivities);
                }
            }
        }
        else
        {
            Assert.Empty(stoppedActivities);
        }
    }

    [Fact]
    public async Task ActivitiesHaveCorrectParentTest()
    {
        var testSource = new ActivitySource("TestSource");

        var stoppedActivities = new List<Activity>();
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource("TestSource")
            .AddInMemoryExporter(stoppedActivities)
            .AddWcfInstrumentation()
            .Build();

        var client = new ServiceClient(
            new NetTcpBinding(SecurityMode.None),
            new EndpointAddress(new Uri(this.serviceBaseUri, "/Service")));
        try
        {
            client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());

            using var parentActivity = testSource.StartActivity("ParentActivity");
            client.ExecuteSynchronous(new ServiceRequest(payload: "Hello Open Telemetry!"));
            client.ExecuteSynchronous(new ServiceRequest(payload: "Hello Open Telemetry!"));
            var firstAsyncCall = client.ExecuteAsync(new ServiceRequest(payload: "Hello Open Telemetry!"));
            await client.ExecuteAsync(new ServiceRequest(payload: "Hello Open Telemetry!"));
            await firstAsyncCall;
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
            testSource.Dispose();
            WcfInstrumentationActivitySource.Options = null;
        }

        Assert.Equal(5, stoppedActivities.Count);
        Assert.All(stoppedActivities, activity => Assert.Equal(stoppedActivities[0].TraceId, activity.TraceId));
        var parent = stoppedActivities.Single(activity => activity.Parent == null);
        Assert.All(
            stoppedActivities.Where(activity => activity != parent),
            activity => Assert.Equal(parent.SpanId, activity.ParentSpanId));
    }

    [Fact]
    public async Task ErrorsAreHandledProperlyTest()
    {
        var testSource = new ActivitySource("TestSource");

        var stoppedActivities = new List<Activity>();
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource("TestSource")
            .AddInMemoryExporter(stoppedActivities)
            .AddWcfInstrumentation()
            .Build();

        var client = new ServiceClient(new NetTcpBinding(SecurityMode.None), new EndpointAddress(new Uri(this.serviceBaseUri, "/Service")));
        var client2 = new ServiceClient(new NetTcpBinding(SecurityMode.None), new EndpointAddress(new Uri(this.serviceBaseUri, "/Service")));
        var clientBadUrl = new ServiceClient(new NetTcpBinding(SecurityMode.None), new EndpointAddress(new Uri("net.tcp://localhost:1/Service")));
        var clientBadUrl2 = new ServiceClient(new NetTcpBinding(SecurityMode.None), new EndpointAddress(new Uri("net.tcp://localhost:1/Service")));
        try
        {
            client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
            client2.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
            clientBadUrl.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
            clientBadUrl2.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());

            using var parentActivity = testSource.StartActivity("ParentActivity");
            Assert.ThrowsAny<Exception>(client.ErrorSynchronous);

            // weird corner case: if an async call is made as the first hit (before the client is opened) the
            // async execution context gets lost somewhere in WCF internals, so we'll explicitly open it first
            client2.Open();
            await Assert.ThrowsAnyAsync<Exception>(client2.ErrorAsync);
            Assert.ThrowsAny<Exception>(() => clientBadUrl.ExecuteSynchronous(new ServiceRequest(payload: "Hello Open Telemetry!")));
            await Assert.ThrowsAnyAsync<Exception>(() => clientBadUrl2.ExecuteAsync(new ServiceRequest(payload: "Hello Open Telemetry!")));
        }
        finally
        {
            static void CloseClient(ServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }

            CloseClient(client);
            CloseClient(client2);
            CloseClient(clientBadUrl);
            CloseClient(clientBadUrl2);

            tracerProvider?.Shutdown();
            tracerProvider?.Dispose();
            testSource.Dispose();
            WcfInstrumentationActivitySource.Options = null;
        }

        // this is 3 instead of 5 because the clientBadUrl calls fail during Open(), before the activity is created
        Assert.Equal(3, stoppedActivities.Count);
        Assert.All(stoppedActivities, activity => Assert.Equal(stoppedActivities[0].TraceId, activity.TraceId));
        var parent = stoppedActivities.Single(activity => activity.Parent == null);
        var children = stoppedActivities.Where(activity => activity != parent);
        Assert.All(children, activity => Assert.Equal(parent.SpanId, activity.ParentSpanId));
    }

    [Fact]
    public async Task OrphanedTelemetryTimesOut()
    {
        var stoppedActivities = new List<Activity>();
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(stoppedActivities)
            .AddWcfInstrumentation()
            .Build();

        var binding = new NetTcpBinding(SecurityMode.None)
        {
            SendTimeout = TimeSpan.FromMilliseconds(1000),
        };
        var client = new ServiceClient(binding, new EndpointAddress(new Uri(this.serviceBaseUri, "/Service")));
        try
        {
            client.Endpoint.EndpointBehaviors.Add(new DownstreamInstrumentationEndpointBehavior());
            client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
            DownstreamInstrumentationChannel.FailNextReceive();
            await Assert.ThrowsAnyAsync<Exception>(() => client.ExecuteAsync(new ServiceRequest(payload: "Hello Open Telemetry!")));

            for (var i = 0; i < 50; i++)
            {
                if (stoppedActivities.Count > 0)
                {
                    break;
                }

                await Task.Delay(100);
            }

            Assert.Single(stoppedActivities);
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
    }

    [Fact]
    public async Task DynamicTimeoutValuesAreRespected()
    {
        var stoppedActivities = new List<Activity>();
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(stoppedActivities)
            .AddWcfInstrumentation()
            .Build();

        var binding = new NetTcpBinding(SecurityMode.None)
        {
            SendTimeout = TimeSpan.FromMilliseconds(15000),
        };
        var client = new ServiceClient(binding, new EndpointAddress(new Uri(this.serviceBaseUri, "/Service")));
        try
        {
            client.Endpoint.EndpointBehaviors.Add(new DownstreamInstrumentationEndpointBehavior());
            client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
            client.InnerChannel.OperationTimeout = TimeSpan.FromMilliseconds(1000);
            DownstreamInstrumentationChannel.FailNextReceive();
            await Assert.ThrowsAnyAsync<Exception>(() => client.ExecuteAsync(new ServiceRequest(payload: "Hello Open Telemetry!")));

            var startedWaiting = DateTime.UtcNow;
            for (var i = 0; i < 200; i++)
            {
                if (stoppedActivities.Count > 0)
                {
                    break;
                }

                await Task.Delay(100);
            }

            Assert.True(DateTime.UtcNow - startedWaiting < TimeSpan.FromSeconds(10));
            Assert.Single(stoppedActivities);
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
    }

    [Fact]
    public void StreamedTransferWithTransportSecurityWithMessageCredentialWorks()
    {
        // this config combination is unique because it uses an IRequestSessionChannel,
        // where other NetTcp configs use IDuplexChannel

        List<Activity> stoppedActivities = [];
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddInMemoryExporter(stoppedActivities)
            .AddWcfInstrumentation()
            .Build();

        var binding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential)
        {
            TransferMode = TransferMode.Streamed,
        };
        binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
        binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
        var host = this.CreateServiceHost(binding, LoadCertificate());
        var client = new ServiceClient(binding, new EndpointAddress(new Uri(host.BaseAddresses[0], "/Service")));
        try
        {
            client.ClientCredentials.ClientCertificate.Certificate = LoadCertificate();

            // never do this in production code, this insecure and for testing only
            client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
            client.Endpoint.EndpointBehaviors.Add(new TelemetryEndpointBehavior());
            client.ExecuteSynchronous(new ServiceRequest(payload: "Hello Open Telemetry!"));
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

            host.Close();

            tracerProvider?.Shutdown();
            tracerProvider?.Dispose();

            WcfInstrumentationActivitySource.Options = null;
        }

        Assert.NotEmpty(stoppedActivities);
        Assert.Equal(WcfInstrumentationActivitySource.OutgoingRequestActivityName, stoppedActivities[0].OperationName);
        Assert.Equal("http://opentelemetry.io/Service/ExecuteSynchronous", stoppedActivities[0].DisplayName);
    }

    private static X509Certificate2 LoadCertificate()
    {
        // The certificate shouldn't ever need to be regenerated, but if it does
        // for some reason these are the commands used to generate it:
        //   openssl genrsa -out otel.key 2048
        //   openssl req -x509 -sha256 -key otel.key -out otel.pem -days 1 -subj "/C=CA/ST=ST/O=OpenTelemetry/CN=localhost"
        //   openssl pkcs12 -export -inkey otel.key -in otel.pem -out certificate.pfx -password pass:

        var resourceName = "OpenTelemetry.Instrumentation.Wcf.Tests.certificate.pfx";
        using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        using var memoryStream = new MemoryStream();
        resourceStream.CopyTo(memoryStream);
        return new X509Certificate2(memoryStream.ToArray());
    }

    private ServiceHost CreateServiceHost(NetTcpBinding binding, X509Certificate2? cert)
    {
        ServiceHost? serviceHost = null;
        var random = new Random();
        var retryCount = 5;
        while (retryCount > 0)
        {
            try
            {
                var baseUri = new Uri($"net.tcp://localhost:{random.Next(2000, 5000)}/");
                serviceHost = new ServiceHost(new Service(), baseUri);
                if (cert != null)
                {
                    serviceHost.Credentials.ServiceCertificate.Certificate = cert;
                }

                var endpoint = serviceHost.AddServiceEndpoint(
                    typeof(IServiceContract),
                    binding,
                    "/Service");

                serviceHost.Open();
                break;
            }
            catch (Exception ex)
            {
                this.output.WriteLine(ex.ToString());
                if (serviceHost?.State == CommunicationState.Faulted)
                {
                    serviceHost.Abort();
                }
                else
                {
                    serviceHost?.Close();
                }

                serviceHost = null;
                retryCount--;
            }
        }

        return serviceHost ?? throw new InvalidOperationException("ServiceHost could not be started.");
    }
}
#endif
