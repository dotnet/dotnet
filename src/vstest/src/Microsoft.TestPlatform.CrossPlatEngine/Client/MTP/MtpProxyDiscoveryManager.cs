// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Testing.Platform.ServerMode.Client;
using Microsoft.TestPlatform.Hashing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// An <see cref="IProxyDiscoveryManager"/> that discovers tests by driving a
/// Microsoft.Testing.Platform (MTP) application over the MTP JSON-RPC protocol instead of the
/// vstest testhost protocol.
/// </summary>
internal sealed class MtpProxyDiscoveryManager : IProxyDiscoveryManager, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// The test id algorithm declared for this run in runsettings, or <see langword="null"/> when the
    /// run does not declare one and the runner's own environment should decide.
    /// </summary>
    /// <remarks>
    /// The classic path reads this from the testhost's environment, which runsettings
    /// <c>RunConfiguration/EnvironmentVariables</c> populates. MTP nodes are converted into test
    /// cases here, in the runner, which does not receive those variables, so the declared value has
    /// to be read from the runsettings directly and passed to the converter.
    /// </remarks>
    private TestCaseIdAlgorithm? _testCaseIdAlgorithm;

    public void Initialize(bool skipDefaultAdapters)
    {
    }

    public void InitializeDiscovery(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler, bool skipDefaultAdapters)
        => Initialize(skipDefaultAdapters);

    public void DiscoverTests(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler)
    {
        _testCaseIdAlgorithm = MtpTestNodeConverter.ResolveTestCaseIdAlgorithm(
            InferRunSettingsHelper.GetEnvironmentVariables(discoveryCriteria.RunSettings));

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

        MtpServerClientOptions options = MtpClientOptionsFactory.CreateOptions();
        using IMtpServerClient client = MtpServerClientFactory.Launch(source, options);
        client.LogReceived += (_, e) => eventHandler.HandleLogMessage(MtpClientOptionsFactory.MapServerLogLevel(e.Level), e.Message);
        client.TestNodesUpdated += (_, e) =>
        {
            foreach (MtpTestNodeUpdate change in e.Changes)
            {
                if (MtpTestNodeConverter.IsActionNode(change))
                {
                    lock (discovered)
                    {
                        discovered.Add(MtpTestNodeConverter.ToTestCase(change, source, _testCaseIdAlgorithm));
                    }
                }
            }
        };

        try
        {
            client.InitializeAsync(_cancellationTokenSource.Token).GetAwaiter().GetResult();

            // Awaiting the discover request is sufficient: server-to-client messages arrive on a single
            // ordered stream that the client reads sequentially and dispatches synchronously, so every
            // node notification has already been delivered by the time the request completes.
            client.DiscoverTestsAsync(_cancellationTokenSource.Token).GetAwaiter().GetResult();
        }
        finally
        {
            MtpServerClientFactory.TryExit(client);
        }

        List<TestCase> chunk;
        lock (discovered)
        {
            chunk = discovered.ToList();
        }

        if (chunk.Count > 0)
        {
            eventHandler.HandleDiscoveredTests(chunk);
        }

        return chunk.Count;
    }
}
