// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeCommunicationEndpoint : ICommunicationEndPoint
{
    private bool _stopped;

    public FakeCommunicationEndpoint(FakeCommunicationChannel fakeCommunicationChannel, FakeErrorAggregator fakeErrorAggregator)
    {
        Channel = fakeCommunicationChannel;
        FakeErrorAggregator = fakeErrorAggregator;
        TestHostConnectionInfo = new TestHostConnectionInfo
        {
            Endpoint = $"127.0.0.1:{fakeCommunicationChannel.Id}",
            Role = ConnectionRole.Client,
            Transport = Transport.Sockets,
        };
    }

    public FakeErrorAggregator FakeErrorAggregator { get; }
    public FakeCommunicationChannel Channel { get; }
    public TestHostConnectionInfo TestHostConnectionInfo { get; }

    /// <summary>
    /// Notify the caller that we disconnected, this happens if process exits unexpectedly and leads to abort flow.
    /// In success case use Stop instead, to just "close" the channel, because the other side already disconnected from us
    /// and told us to tear down.
    /// </summary>
    public void Disconnect()
    {
        Disconnected?.Invoke(this, new DisconnectedEventArgs());
        _stopped = true;
    }

    #region ICommunicationEndPoint

    public event EventHandler<ConnectedEventArgs>? Connected;
    public event EventHandler<DisconnectedEventArgs>? Disconnected;

    public string Start(string endPoint)
    {
        // In normal run this endpoint can be a client or a server. When we are a client we will get an address and a port and
        // we will try to connect to it.
        // If we are a server, we will get an address and port 0, which means we should figure out a port that is free
        // and return the address and port back to the caller.
        //
        // In our fake scenario we know the "port" from the get go, we set it to an id that was given to the testhost
        // because that is currently the only way for us to check if we are connecting to the expected fake testhost
        // that has a list of canned responses, which must correlate with the requests. So e.g. if we get request for mstest1.dll
        // we should return the responses we have prepared for mstest1.dll, and not for mstest2.dll.
        //
        // We use the port number because the rest of the IP address is validated. We force sockets and IP usage in multiple places,
        // so we cannot just past the dll path (or something similar) as the endpoint name, because the other side will check if that is
        // a valid IP address and port.
        if (endPoint != TestHostConnectionInfo.Endpoint)
        {
            throw new InvalidOperationException($"Expected to connect to {endPoint} but instead got channel with {TestHostConnectionInfo.Endpoint}.");
        }
        Connected?.SafeInvoke(this, new ConnectedEventArgs(Channel), "FakeCommunicationEndpoint.Start");
        return endPoint;
    }

    public void Stop()
    {
        if (!_stopped)
        {
            // Do not allow stop to be called multiple times, because it will end up calling us back and stack overflows.
            _stopped = true;
        }
    }

    #endregion
}
