// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Defines the functionality of the test platform.
/// </summary>
public interface ITestPlatform : IDisposable
{
    /// <summary>
    /// Updates the extensions to be used by the test service.
    /// </summary>
    ///
    /// <param name="pathToAdditionalExtensions">
    /// Specifies the path to unit test extensions. If no additional extension is available,
    /// then specify null or empty list.
    /// </param>
    /// <param name="skipExtensionFilters">
    /// Flag indicating if we should skip the default adapters initialization.
    /// </param>
    void UpdateExtensions(
        IEnumerable<string>? pathToAdditionalExtensions,
        bool skipExtensionFilters);

    /// <summary>
    /// Clears the extensions.
    /// </summary>
    void ClearExtensions();

    /// <summary>
    /// Creates a discovery request.
    /// </summary>
    ///
    /// <param name="requestData">Providing common services and data for discovery.</param>
    /// <param name="discoveryCriteria">Specifies the discovery parameters.</param>
    /// <param name="options">Test platform options.</param>
    ///
    /// <returns>A DiscoveryRequest object.</returns>
    IDiscoveryRequest CreateDiscoveryRequest(
        IRequestData requestData,
        DiscoveryCriteria discoveryCriteria,
        TestPlatformOptions? options,
        Dictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger);

    /// <summary>
    /// Creates a test run request.
    /// </summary>
    ///
    /// <param name="requestData">Providing common services and data for execution.</param>
    /// <param name="testRunCriteria">Specifies the test run criteria.</param>
    /// <param name="options">Test platform options.</param>
    ///
    /// <returns>A RunRequest object.</returns>
    ITestRunRequest CreateTestRunRequest(
        IRequestData requestData,
        TestRunCriteria testRunCriteria,
        TestPlatformOptions? options,
        Dictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger);

    /// <summary>
    /// Starts a test session.
    /// </summary>
    ///
    /// <param name="requestData">
    /// Providing common services and data for test session start.
    /// </param>
    /// <param name="criteria">Specifies the start test session criteria.</param>
    /// <param name="eventsHandler">Events handler for handling session events.</param>
    ///
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    bool StartTestSession(
        IRequestData requestData,
        StartTestSessionCriteria criteria,
        ITestSessionEventsHandler eventsHandler,
        Dictionary<string, SourceDetail> sourceToSourceDetailMap,
        IWarningLogger warningLogger);
}
