// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

/// <summary>
/// Interface for TestRuntimeProvider which manages test host processes for test engine.
/// </summary>
public interface ITestRuntimeProvider
{
    /// <summary>
    /// Raised when host is launched successfully
    /// Consumed by TestPlatform to initialize connection b/w test host and test platform
    /// </summary>
    event EventHandler<HostProviderEventArgs>? HostLaunched;

    /// <summary>
    /// Raised when host is cleaned up and removes all it's dependencies
    /// </summary>
    event EventHandler<HostProviderEventArgs>? HostExited;

    /// <summary>
    /// Gets a value indicating whether the test host is specific to a test source. If yes, each test source
    /// is launched in a separate host process.
    /// </summary>
    bool Shared { get; }

    /// <summary>
    /// Sets a Message Logger
    /// </summary>
    /// <param name="logger">provide logger to runtimes</param>
    /// <param name="runsettingsXml">provide runsettings to runtimes for initialization</param>
    void Initialize(IMessageLogger? logger, string runsettingsXml);

    /// <summary>
    /// Gets a value indicating whether the test host is specific to a test source. If yes, each test source
    /// is launched in a separate host process.
    /// </summary>
    /// <param name="runsettingsXml">
    /// The run Configuration.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    bool CanExecuteCurrentRunConfiguration(string? runsettingsXml);

    /// <summary>
    /// Sets a custom launcher
    /// </summary>
    /// <param name="customLauncher">Custom launcher to set</param>
    void SetCustomLauncher(ITestHostLauncher customLauncher);

    /// <summary>
    /// Gets the end point address and behavior of TestRuntime
    /// E.g. for phone device EndPoint:127.0.0.1:8080, ConnectionRole Host, TransportProtocol: Sockets
    /// </summary>
    /// <returns> Socket where the service is hosted by TestRuntime</returns>
    TestHostConnectionInfo GetTestHostConnectionInfo();

    /// <summary>
    /// Launches the test host for discovery/execution.
    /// </summary>
    /// <param name="testHostStartInfo">Start parameters for the test host.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns whether the test host launched successfully or not.</returns>
    Task<bool> LaunchTestHostAsync(TestProcessStartInfo testHostStartInfo, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the start parameters for the test host.
    /// </summary>
    /// <param name="sources">Test source paths.</param>
    /// <param name="environmentVariables">Set of environment variables for the test host process.</param>
    /// <param name="connectionInfo">Set of connection parameters for the test host process to communicate with test runner.</param>
    /// <returns>ProcessStartInfo of the test host.</returns>
    TestProcessStartInfo GetTestHostProcessStartInfo(IEnumerable<string> sources, IDictionary<string, string?>? environmentVariables, TestRunnerConnectionInfo connectionInfo);

    /// <summary>
    /// Gets paths of any additional extensions.
    /// A test host manager may choose to provide a custom extensions acquisition and discovery
    /// mechanism. E.g. for .net core, extensions are discovered from the <c>testproject.deps.json</c> file.
    /// </summary>
    /// <param name="sources">List of test sources.</param>
    /// <param name="extensions"></param>
    /// <returns>List of paths to extension assemblies.</returns>
    IEnumerable<string> GetTestPlatformExtensions(IEnumerable<string> sources, IEnumerable<string> extensions);

    /// <summary>
    /// Gets path of test sources, based on RuntimeProvider, and where the test is actually deployed(Remote Scenario).
    /// A test host manager may choose to accept input source as XML file, and provide appropriate source(dll/exe) which the adapters can actually consume
    /// E.g. for UWP, input source could be "appx recipe" file, which gives information about actual source exe.
    /// </summary>
    /// <param name="sources">List of test sources.</param>
    /// <returns>Updated List of test sources based on remote/local scenario.</returns>
    IEnumerable<string> GetTestSources(IEnumerable<string> sources);

    /// <summary>
    /// Cleanup the test host process and it's dependencies.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task CleanTestHostAsync(CancellationToken cancellationToken);
}

public class HostProviderEventArgs : EventArgs
{
    public HostProviderEventArgs(string message)
    {
        Data = message;
        ErrroCode = 0;
    }

    public HostProviderEventArgs(string message, int errorCode, int processId)
    {
        Data = message;
        ErrroCode = errorCode;
        ProcessId = processId;
    }

    public string Data { get; set; }

    public int ErrroCode { get; set; }

    public int ProcessId { get; set; }
}
