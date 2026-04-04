// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using Google.Protobuf;
using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Internal.Transport;

namespace OpenTelemetry.OpAmp.Client.Tests.Mocks;

internal class MockTransport : IOpAmpTransport, IDisposable
{
    private readonly ConcurrentQueue<AgentToServer> messages = [];
    private readonly AutoResetEvent messageEvent = new(false);
    private readonly int expectedCount;

    public MockTransport(int expectedCount)
    {
        this.expectedCount = expectedCount;
    }

    public IReadOnlyCollection<AgentToServer> Messages => this.messages.ToList().AsReadOnly();

    public Task SendAsync<T>(T message, CancellationToken token)
        where T : IMessage<T>
    {
        if (message is AgentToServer agentToServer)
        {
            this.messages.Enqueue(agentToServer);

            if (this.messages.Count == this.expectedCount)
            {
                this.messageEvent.Set();
            }
        }
        else
        {
            throw new InvalidOperationException("Unsupported message type. Only AgentToServer messages are supported.");
        }

        return Task.CompletedTask;
    }

    public void WaitForMessages(TimeSpan timeout)
    {
        this.messageEvent.WaitOne(timeout);
    }

    public void Dispose()
    {
        this.messageEvent.Dispose();
    }
}
