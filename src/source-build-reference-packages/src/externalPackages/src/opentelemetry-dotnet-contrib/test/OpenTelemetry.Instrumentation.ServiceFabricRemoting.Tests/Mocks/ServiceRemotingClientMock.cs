// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Fabric;
using System.Text;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using OpenTelemetry.Context.Propagation;
using ServiceFabric.Mocks.RemotingV2;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

internal class ServiceRemotingClientMock : IServiceRemotingClient
{
    public ServiceRemotingClientMock()
    {
    }

    public ResolvedServicePartition ResolvedServicePartition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string ListenerName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ResolvedServiceEndpoint Endpoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    /// <summary>
    /// The RequestResponseAsync method reads the headers from the request and injects them into the response, using OpneTelemetry's TextMapPropagator.
    /// </summary>
    public Task<IServiceRemotingResponseMessage> RequestResponseAsync(IServiceRemotingRequestMessage requestMessage)
    {
        IServiceRemotingRequestMessageHeader requestMessageHeader = requestMessage.GetHeader();
        PropagationContext propagationContext = Propagators.DefaultTextMapPropagator.Extract(default, requestMessageHeader, ServiceFabricRemotingUtils.ExtractTraceContextFromRequestMessageHeader);

        MockServiceRemotingResponseMessage responseMessage = new MockServiceRemotingResponseMessage()
        {
            Header = new ServiceRemotingResponseMessageHeaderMock(),
        };

        Propagators.DefaultTextMapPropagator.Inject(new PropagationContext(propagationContext.ActivityContext, propagationContext.Baggage), responseMessage.Header, this.InjectTraceContextIntoServiceRemotingRequestMessageHeader);

        return Task.FromResult<IServiceRemotingResponseMessage>(responseMessage);
    }

    public void SendOneWay(IServiceRemotingRequestMessage requestMessage) => throw new NotImplementedException();

    private void InjectTraceContextIntoServiceRemotingRequestMessageHeader(IServiceRemotingResponseMessageHeader responseMessageHeaders, string key, string value)
    {
        if (!responseMessageHeaders.TryGetHeaderValue(key, out byte[] _))
        {
            byte[] valueAsBytes = Encoding.UTF8.GetBytes(value);

            responseMessageHeaders.AddHeader(key, valueAsBytes);
        }
    }
}
