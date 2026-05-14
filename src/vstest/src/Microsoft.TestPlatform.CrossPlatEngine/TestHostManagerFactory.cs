// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;

/// <summary>
/// The factory that provides discovery and execution managers to the test host.
/// </summary>
public class TestHostManagerFactory : ITestHostManagerFactory
{
    private IDiscoveryManager? _discoveryManager;
    private IExecutionManager? _executionManager;
    private readonly bool _telemetryOptedIn;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestHostManagerFactory"/> class.
    /// </summary>
    ///
    /// <param name="telemetryOptedIn">
    /// A value indicating if the telemetry is opted in or not.
    /// </param>
    public TestHostManagerFactory(bool telemetryOptedIn)
    {
        _telemetryOptedIn = telemetryOptedIn;
    }

    /// <summary>
    /// The discovery manager instance for any discovery related operations inside of the test host.
    /// </summary>
    /// <returns>The discovery manager.</returns>
    public IDiscoveryManager GetDiscoveryManager()
        => _discoveryManager ??= new DiscoveryManager(GetRequestData(_telemetryOptedIn));

    /// <summary>
    /// The execution manager instance for any discovery related operations inside of the test host.
    /// </summary>
    /// <returns>The execution manager.</returns>
    public IExecutionManager GetExecutionManager()
        => _executionManager ??= new ExecutionManager(GetRequestData(_telemetryOptedIn));

    private static RequestData GetRequestData(bool telemetryOptedIn)
    {
        return new RequestData
        {
            MetricsCollection =
                telemetryOptedIn
                    ? new MetricsCollection()
                    : new NoOpMetricsCollection(),
            IsTelemetryOptedIn = telemetryOptedIn
        };
    }
}
