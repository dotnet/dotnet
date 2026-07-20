// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Jsonite;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// An <see cref="IProxyDiscoveryManager"/> that discovers tests by driving a
/// Microsoft.Testing.Platform (MTP) application over the MTP JSON-RPC protocol instead of the
/// vstest testhost protocol.
/// </summary>
internal sealed class MtpProxyDiscoveryManager : IProxyDiscoveryManager, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public void Initialize(bool skipDefaultAdapters)
    {
    }

    public void InitializeDiscovery(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler, bool skipDefaultAdapters)
        => Initialize(skipDefaultAdapters);

    public void DiscoverTests(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler)
    {
        var sources = discoveryCriteria.Sources?.ToList() ?? new List<string>();
        long totalTests = 0;
        bool aborted = false;

        foreach (string source in sources)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                aborted = true;
                break;
            }

            try
            {
                totalTests += DiscoverSource(source, eventHandler);
            }
            catch (OperationCanceledException)
            {
                aborted = true;
                break;
            }
            catch (Exception ex)
            {
                EqtTrace.Error("MtpProxyDiscoveryManager.DiscoverTests: discovery failed for '{0}': {1}", source, ex);
                eventHandler.HandleLogMessage(ObjectModel.Logging.TestMessageLevel.Error, $"Microsoft.Testing.Platform discovery failed for '{source}': {ex.Message}");
                aborted = true;
            }
        }

        eventHandler.HandleDiscoveryComplete(new DiscoveryCompleteEventArgs(totalTests, aborted), null);
    }

    public void Abort() => _cancellationTokenSource.Cancel();

    public void Abort(ITestDiscoveryEventsHandler2 eventHandler) => Abort();

    public void Close() => _cancellationTokenSource.Cancel();

    public void Dispose()
    {
        try
        {
            _cancellationTokenSource.Dispose();
        }
        catch
        {
            // ignore
        }
    }

    private int DiscoverSource(string source, ITestDiscoveryEventsHandler2 eventHandler)
    {
        var discovered = new List<TestCase>();
        var completed = new ManualResetEventSlim(false);

        using var connection = new MtpServerConnection();
        connection.LogReceived += (level, message) => eventHandler.HandleLogMessage(MtpClientHelpers.MapLevel(level), message);
        connection.TestNodesUpdated += parameters =>
        {
            if (MtpClientHelpers.IsCompletionSentinel(parameters))
            {
                completed.Set();
                return;
            }

            foreach (JsonObject node in MtpClientHelpers.EnumerateNodes(parameters))
            {
                if (MtpTestNodeConverter.IsActionNode(node))
                {
                    lock (discovered)
                    {
                        discovered.Add(MtpTestNodeConverter.ToTestCase(node, source));
                    }
                }
            }
        };

        connection.Start(source, environmentVariables: null, MtpClientHelpers.GetConnectionTimeout());
        connection.InvokeAsync(MtpConstants.InitializeMethod, MtpClientHelpers.InitializeParameters(), _cancellationTokenSource.Token).GetAwaiter().GetResult();

        var runId = Guid.NewGuid();
        var discoverTask = connection.InvokeAsync(
            MtpConstants.DiscoverTestsMethod,
            new Dictionary<string, object?> { [MtpConstants.RunIdParameter] = runId.ToString() },
            _cancellationTokenSource.Token);

        // The response indicates the server has finished discovery. Because messages arrive on a
        // single ordered stream that we read sequentially, every node notification sent before the
        // response has already been dispatched by the time the response completes.
        discoverTask.GetAwaiter().GetResult();
        completed.Wait(TimeSpan.FromSeconds(3));

        List<TestCase> chunk;
        lock (discovered)
        {
            chunk = discovered.ToList();
        }

        if (chunk.Count > 0)
        {
            eventHandler.HandleDiscoveredTests(chunk);
        }

        connection.SendNotification(MtpConstants.ExitMethod, null);
        return chunk.Count;
    }
}
