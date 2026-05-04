// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpAmp.Proto.V1;
using OpenTelemetry.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Services.Heartbeat;
using OpenTelemetry.OpAmp.Client.Internal.Transport;
using OpenTelemetry.OpAmp.Client.Settings;

namespace OpenTelemetry.OpAmp.Client.Internal;

internal sealed class FrameDispatcher : IDisposable
{
    private readonly IOpAmpTransport transport;
    private readonly FrameBuilder frameBuilder;
    private readonly SemaphoreSlim syncRoot = new(1, 1);

    public FrameDispatcher(IOpAmpTransport transport, OpAmpClientSettings settings)
    {
        Guard.ThrowIfNull(transport, nameof(transport));
        Guard.ThrowIfNull(settings, nameof(settings));

        this.transport = transport;
        this.frameBuilder = new FrameBuilder(settings);
    }

    // TODO: May need to redesign to request only partials
    // so any other message waiting to be sent can be included to optimize transport usage and locking time.
    public async Task DispatchIdentificationAsync(CancellationToken token)
    {
        await this.DispatchFrameAsync(
            BuildIdentificationMessage,
            OpAmpClientEventSource.Log.SendingIdentificationMessage,
            OpAmpClientEventSource.Log.SendIdentificationMessageException,
            token).ConfigureAwait(false);

        static AgentToServer BuildIdentificationMessage(FrameBuilder fb)
        {
            return fb.StartBaseMessage()
                .AddAgentDescription()
                .AddCapabilities()
                .Build();
        }
    }

    public async Task DispatchHeartbeatAsync(HealthReport report, CancellationToken token)
    {
        await this.DispatchFrameAsync(
            BuildHeartbeatMessage,
            OpAmpClientEventSource.Log.SendingHeartbeatMessage,
            OpAmpClientEventSource.Log.SendHeartbeatMessageException,
            token).ConfigureAwait(false);

        AgentToServer BuildHeartbeatMessage(FrameBuilder fb)
        {
            return fb.StartBaseMessage().AddHealth(report).Build();
        }
    }

    public async Task DispatchAgentDisconnectAsync(CancellationToken token)
    {
        await this.DispatchFrameAsync(
            BuildDisconnectMessage,
            OpAmpClientEventSource.Log.SendingAgentDisconnectMessage,
            OpAmpClientEventSource.Log.SendHeartbeatMessageException,
            token).ConfigureAwait(false);

        static AgentToServer BuildDisconnectMessage(FrameBuilder fb)
        {
            return fb.StartBaseMessage().AddAgentDisconnect().Build();
        }
    }

    public void Dispose()
    {
        this.syncRoot.Dispose();
    }

    private async Task DispatchFrameAsync(
        Func<FrameBuilder, AgentToServer> messageBuilder,
        Action informationLogger,
        Action<Exception> exceptionLogger,
        CancellationToken token)
    {
        await this.syncRoot.WaitAsync(token)
            .ConfigureAwait(false);

        try
        {
            var message = messageBuilder(this.frameBuilder);

            informationLogger();

            await this.transport.SendAsync(message, token)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            exceptionLogger(ex);

            this.frameBuilder.Reset(); // Reset the builder in case of failure
        }
        finally
        {
            this.syncRoot.Release();
        }
    }
}
