// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Defines the functionality of a test engine.
/// </summary>
public interface ITestEngine
{
    /// <summary>
    /// Fetches the DiscoveryManager for this engine. This manager would provide all
    /// functionality required for discovery.
    /// </summary>
    ///
    /// <param name="requestData">
    /// The request data for providing discovery services and data.
    /// </param>
    /// <param name="testHostManager">Test host manager for the current test discovery.</param>
    /// <param name="discoveryCriteria">The discovery criteria.</param>
    ///
    /// <returns>An IProxyDiscoveryManager object that can do discovery.</returns>
    IProxyDiscoveryManager GetDiscoveryManager(
        IRequestData requestData,
        DiscoveryCriteria discoveryCriteria,
        IDictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger);

    /// <summary>
    /// Fetches the ExecutionManager for this engine. This manager would provide all
    /// functionality required for execution.
    /// </summary>
    ///
    /// <param name="requestData">
    /// The request data for providing common execution services and data.
    /// </param>
    /// <param name="testHostManager">Test host manager for the current test run.</param>
    /// <param name="testRunCriteria">Test run criteria of the current test run.</param>
    ///
    /// <returns>An IProxyExecutionManager object that can do execution.</returns>
    IProxyExecutionManager GetExecutionManager(
        IRequestData requestData,
        TestRunCriteria testRunCriteria,
        IDictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger);

    /// <summary>
    /// Fetches the TestSessionManager for this engine. This manager would provide all
    /// functionality required for test session management.
    /// </summary>
    ///
    /// <param name="requestData">
    /// The request data for providing test session services and data.
    /// </param>
    /// <param name="testSessionCriteria">
    /// Test session criteria of the current test session.
    /// </param>
    ///
    /// <returns>An IProxyTestSessionManager object that can manage test sessions.</returns>
    IProxyTestSessionManager? GetTestSessionManager(
        IRequestData requestData,
        StartTestSessionCriteria testSessionCriteria,
        IDictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger);

    /// <summary>
    /// Fetches the extension manager for this engine. This manager would provide extensibility
    /// features that this engine supports.
    /// </summary>
    ///
    /// <returns>An ITestExtensionManager object that helps with extensibility.</returns>
    ITestExtensionManager GetExtensionManager();

    /// <summary>
    /// Fetches the logger manager for this engine. This manager will provide logger
    /// extensibility features that this engine supports.
    /// </summary>
    ///
    /// <param name="requestData">
    /// The request data for providing common execution services and data.
    /// </param>
    ///
    /// <returns>An ITestLoggerManager object that helps with logger extensibility.</returns>
    ITestLoggerManager GetLoggerManager(IRequestData requestData);
}
