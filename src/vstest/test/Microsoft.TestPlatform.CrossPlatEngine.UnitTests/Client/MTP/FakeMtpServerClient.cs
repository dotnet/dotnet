// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Testing.Platform.ServerMode.Client;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.Client.MTP;

/// <summary>
/// An in-memory <see cref="IMtpServerClient"/> that records how the proxy managers drive it, so the
/// launch, discover, run and shutdown sequence can be asserted without starting a real MTP
/// application.
/// </summary>
internal sealed class FakeMtpServerClient : IMtpServerClient
{
    public event EventHandler<MtpTestNodeUpdateEventArgs>? TestNodesUpdated;

    public event EventHandler<MtpLogEventArgs>? LogReceived;

#pragma warning disable CS0067 // Required by IMtpServerClient; the proxy managers do not subscribe to these.
    public event EventHandler<MtpTelemetryEventArgs>? TelemetryReceived;

    public event EventHandler<MtpAttachmentsEventArgs>? AttachmentsReceived;
#pragma warning restore CS0067

    public int ProcessId { get; set; } = 4242;

    public MtpServerCapabilities? Capabilities { get; private set; }

    public Func<string, IDictionary<string, object?>?, CancellationToken, Task<IDictionary<string, object?>?>>? ServerRequestHandler { get; set; }

    /// <summary>Gets a value indicating whether <see cref="ExitAsync"/> was called.</summary>
    public bool ExitCalled { get; private set; }

    /// <summary>Gets the cancellation token <see cref="ExitAsync"/> was called with.</summary>
    public CancellationToken ExitToken { get; private set; }

    public bool Disposed { get; private set; }

    /// <summary>Gets the uids the manager asked the server to run, when a filtered run was requested.</summary>
    public IReadOnlyCollection<string>? RunFilterUids { get; private set; }

    /// <summary>Gets or sets the nodes the fake server pushes while handling discover or run.</summary>
    public IReadOnlyList<MtpTestNodeUpdate> NodesToPush { get; set; } = [];

    /// <summary>Gets or sets an exception the fake server throws from discover or run.</summary>
    public Exception? ThrowFromRequest { get; set; }

    /// <summary>Gets or sets a delay applied to <see cref="ExitAsync"/>, simulating a wedged server.</summary>
    public TimeSpan ExitDelay { get; set; } = TimeSpan.Zero;

    /// <summary>Gets or sets an exception <see cref="ExitAsync"/> throws, simulating a refused shutdown.</summary>
    public Exception? ThrowFromExit { get; set; }

    public Task<MtpServerCapabilities> InitializeAsync(CancellationToken cancellationToken = default)
    {
        Capabilities = new MtpServerCapabilities(
            serverProcessId: ProcessId,
            serverName: "FakeMtpServer",
            serverVersion: "1.0.0",
            supportsDiscovery: true,
            multiRequestSupport: false,
            vstestProviderSupport: false,
            supportsAttachments: true,
            multiConnectionProvider: false);
        return Task.FromResult(Capabilities);
    }

    public Task DiscoverTestsAsync(CancellationToken cancellationToken = default)
    {
        PushNodes();
        return ThrowFromRequest is not null ? Task.FromException(ThrowFromRequest) : Task.CompletedTask;
    }

    public Task DiscoverTestsAsync(IReadOnlyCollection<string> testNodeUids, CancellationToken cancellationToken = default)
        => DiscoverTestsAsync(cancellationToken);

    public Task DiscoverTestsWithFilterAsync(string graphFilter, CancellationToken cancellationToken = default)
        => DiscoverTestsAsync(cancellationToken);

    public Task<MtpRunResult> RunTestsAsync(CancellationToken cancellationToken = default)
        => CompleteRun();

    public Task<MtpRunResult> RunTestsAsync(IReadOnlyCollection<string> testNodeUids, CancellationToken cancellationToken = default)
    {
        RunFilterUids = testNodeUids;
        return CompleteRun();
    }

    public Task<MtpRunResult> RunTestsWithFilterAsync(string graphFilter, CancellationToken cancellationToken = default)
        => CompleteRun();

    public async Task ExitAsync(CancellationToken cancellationToken = default)
    {
        ExitCalled = true;
        ExitToken = cancellationToken;

        if (ThrowFromExit is not null)
        {
            throw ThrowFromExit;
        }

        // Honour the token so a caller that passes an already-cancelled token observes the throw a
        // real client would produce.
        cancellationToken.ThrowIfCancellationRequested();

        if (ExitDelay > TimeSpan.Zero)
        {
            await Task.Delay(ExitDelay, cancellationToken).ConfigureAwait(false);
        }
    }

    public void Dispose() => Disposed = true;

    public void RaiseLog(string level, string message)
        => LogReceived?.Invoke(this, new MtpLogEventArgs(level, message));

    private void PushNodes()
    {
        if (NodesToPush.Count > 0)
        {
            TestNodesUpdated?.Invoke(this, new MtpTestNodeUpdateEventArgs(Guid.NewGuid(), NodesToPush));
        }
    }

    private Task<MtpRunResult> CompleteRun()
    {
        PushNodes();
        return ThrowFromRequest is not null
            ? Task.FromException<MtpRunResult>(ThrowFromRequest)
            : Task.FromResult(new MtpRunResult([]));
    }
}
