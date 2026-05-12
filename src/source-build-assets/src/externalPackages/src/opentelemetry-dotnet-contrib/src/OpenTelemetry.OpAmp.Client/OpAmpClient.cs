// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;
using OpenTelemetry.OpAmp.Client.Internal;
using OpenTelemetry.OpAmp.Client.Internal.Services;
using OpenTelemetry.OpAmp.Client.Internal.Services.Heartbeat;
using OpenTelemetry.OpAmp.Client.Internal.Transport;
using OpenTelemetry.OpAmp.Client.Internal.Transport.Http;
using OpenTelemetry.OpAmp.Client.Internal.Transport.WebSocket;
using OpenTelemetry.OpAmp.Client.Listeners;
using OpenTelemetry.OpAmp.Client.Messages;
using OpenTelemetry.OpAmp.Client.Settings;

namespace OpenTelemetry.OpAmp.Client;

/// <summary>
/// OpenTelemetry OpAMP Client.
/// </summary>
public sealed class OpAmpClient : IDisposable
{
    private readonly OpAmpClientSettings settings = new();
    private readonly FrameProcessor processor = new();
    private readonly Dictionary<string, IBackgroundService> services = [];
    private readonly FrameDispatcher dispatcher;
    private readonly IOpAmpTransport transport;
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpAmpClient"/> class.
    /// </summary>
    /// <param name="configure">Configure OpAMP client settings callback.</param>
    public OpAmpClient(Action<OpAmpClientSettings>? configure = null)
    {
        configure?.Invoke(this.settings);

        this.transport = ConstructTransport(this.settings, this.processor);
        this.dispatcher = new FrameDispatcher(this.transport, this.settings);

        this.ConfigureServices();
    }

    /// <summary>
    /// Starts the <see cref="OpAmpClient"/> instance, establishing the connection to the server and starting all configured services.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    public async Task StartAsync(CancellationToken token = default)
    {
        this.ThrowIfDisposed();

        if (this.transport is WsTransport wsTransport)
        {
            await wsTransport.StartAsync(token)
                .ConfigureAwait(false);
        }

        await this.dispatcher.DispatchIdentificationAsync(token)
            .ConfigureAwait(false);

        foreach (var service in this.services.Values)
        {
            service.Start();
        }
    }

    /// <summary>
    /// Stops the <see cref="OpAmpClient"/> instance gracefully, terminating the connection to the server and stopping all running services.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous stop operation.</returns>
    /// <remarks>
    /// This method is the preferred shutdown path when the caller wants the client to notify the
    /// server that it is disconnecting. In particular, for WebSocket transport this attempts a
    /// graceful close handshake after sending the agent disconnect message.
    /// </remarks>
    public async Task StopAsync(CancellationToken token = default)
    {
        this.ThrowIfDisposed();

        await this.dispatcher.DispatchAgentDisconnectAsync(token)
            .ConfigureAwait(false);

        foreach (var service in this.services.Values)
        {
            service.Stop();
        }

        if (this.transport is WsTransport wsTransport)
        {
            await wsTransport.StopAsync(token)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Subscribe the specified listener to receive OpAMP messages of <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">The <see cref="OpAmpMessage"/> to subscribe to.</typeparam>
    /// <param name="listener">A listener capable of handling messages of type <typeparamref name="T"/>.</param>
    public void Subscribe<T>(IOpAmpListener<T> listener)
        where T : OpAmpMessage
    {
        this.ThrowIfDisposed();
        Guard.ThrowIfNull(listener, nameof(listener));
        this.processor.Subscribe(listener);
    }

    /// <summary>
    /// Unsubscribe the specified listener from receiving OpAMP messages of <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">The <see cref="OpAmpMessage"/> to unsubscribe from.</typeparam>
    /// <param name="listener">A listener capable of handling messages of type <typeparamref name="T"/>.</param>
    public void Unsubscribe<T>(IOpAmpListener<T> listener)
        where T : OpAmpMessage
    {
        this.ThrowIfDisposed();
        Guard.ThrowIfNull(listener, nameof(listener));
        this.processor.Unsubscribe(listener);
    }

    /// <summary>
    /// Report current effective configuration of the agent.
    /// </summary>
    /// <param name="files">Configuration files to report.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    /// <remarks>
    /// <para>
    /// File contents are transmitted verbatim to the OpAMP server with no redaction.
    /// Ensure the transport is secured with TLS and the OpAMP server is fully trusted
    /// before reporting files that may contain sensitive data such as passwords or tokens.
    /// </para>
    /// </remarks>
    public Task SendEffectiveConfigAsync(IEnumerable<EffectiveConfigFile> files, CancellationToken cancellationToken = default)
    {
        this.ThrowIfDisposed();

        if (!this.settings.EffectiveConfigurationReporting.EnableReporting)
        {
            throw new InvalidOperationException("Effective configuration reporting is not enabled in settings.");
        }

        return this.dispatcher.DispatchEffectiveConfigAsync(files, cancellationToken);
    }

    /// <summary>
    /// Reports custom capabilities supported by the agent.
    /// </summary>
    /// <param name="capabilities">Capabilities list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    public Task SendCustomCapabilitiesAsync(IEnumerable<string> capabilities, CancellationToken cancellationToken = default)
    {
        this.ThrowIfDisposed();

        return this.dispatcher.DispatchCustomCapabilitiesAsync(capabilities, cancellationToken);
    }

    /// <summary>
    /// Sends a custom message related to a supported custom capability.
    /// </summary>
    /// <param name="capability">Capability that matches a reported custom capability.</param>
    /// <param name="type">Type of message within the capability.</param>
    /// <param name="data">Contents of the message.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    public Task SendCustomMessageAsync(string capability, string type, ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        this.ThrowIfDisposed();

        return this.dispatcher.DispatchCustomMessageAsync(capability, type, data, cancellationToken);
    }

    /// <summary>
    /// Disposes the <see cref="OpAmpClient"/> instance and releases all associated resources.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Disposal performs synchronous, best-effort cleanup of background services and transport
    /// resources.
    /// </para>
    /// <para>
    /// This is not a graceful shutdown path and does not send the agent disconnect message. Call
    /// <see cref="StopAsync(CancellationToken)"/> before disposal when the client
    /// should unregister cleanly from the server.
    /// </para>
    /// </remarks>
    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        this.disposed = true;

        foreach (var service in this.services.Values)
        {
            service.Stop();
        }

        foreach (var service in this.services.Values)
        {
            if (service is IDisposable disposableService)
            {
                disposableService.Dispose();
            }
        }

        if (this.transport is IDisposable disposableTransport)
        {
            disposableTransport.Dispose();
        }

        this.dispatcher.Dispose();
    }

    // Used for testing purposes only.
    internal Task SendHeartbeatAsync(HealthReport healthReport, CancellationToken cancellationToken = default)
    {
        this.ThrowIfDisposed();

        return this.dispatcher.DispatchHeartbeatAsync(healthReport, cancellationToken);
    }

    private static IOpAmpTransport ConstructTransport(OpAmpClientSettings settings, FrameProcessor processor)
    {
        return settings.ConnectionType switch
        {
            ConnectionType.WebSocket => new WsTransport(settings, processor),
            ConnectionType.Http => new PlainHttpTransport(settings, processor),
            _ => throw new NotSupportedException("Unsupported transport type"),
        };
    }

    private void ThrowIfDisposed()
    {
#if NET8_0_OR_GREATER
        ObjectDisposedException.ThrowIf(this.disposed, this);
#else
        if (this.disposed)
        {
            throw new ObjectDisposedException(nameof(OpAmpClient));
        }
#endif
    }

    private void ConfigureServices()
    {
        this.ConfigureService<HeartbeatService>(
            settings => settings.Heartbeat.IsEnabled,
            () => new(this.dispatcher, this.processor));
    }

    private void ConfigureService<T>(Predicate<OpAmpClientSettings> isEnabledCallback, Func<T> construct)
        where T : IBackgroundService
    {
        if (isEnabledCallback(this.settings))
        {
            var service = construct();
            service.Configure(this.settings);

            this.services[service.ServiceName] = service;
        }
    }
}
