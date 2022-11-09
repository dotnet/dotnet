// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;

namespace Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;

/// <summary>
/// Defines the contract for running various requests.
/// </summary>
public interface ITestRequestManager : IDisposable
{
    /// <summary>
    /// Initializes the extensions while probing additional paths.
    /// </summary>
    ///
    /// <param name="pathToAdditionalExtensions">Paths to additional extensions.</param>
    /// <param name="skipExtensionFilters">Skip extension filtering by name if true.</param>
    void InitializeExtensions(
        IEnumerable<string>? pathToAdditionalExtensions,
        bool skipExtensionFilters);

    /// <summary>
    /// Resets vstest.console.exe options.
    /// </summary>
    void ResetOptions();

    /// <summary>
    /// Discovers tests given a list of sources and some run settings.
    /// </summary>
    ///
    /// <param name="discoveryPayload">Discovery payload.</param>
    /// <param name="disoveryEventsRegistrar">Discovery events registrar.</param>
    /// <param name="protocolConfig">Protocol related information.</param>
    void DiscoverTests(
        DiscoveryRequestPayload discoveryPayload,
        ITestDiscoveryEventsRegistrar disoveryEventsRegistrar,
        ProtocolConfig protocolConfig);

    /// <summary>
    /// Runs tests given a list of sources and some run settings.
    /// </summary>
    ///
    /// <param name="testRunRequestPayLoad">Test run request payload.</param>
    /// <param name="customTestHostLauncher">Custom test host launcher for the run.</param>
    /// <param name="testRunEventsRegistrar">Run events registrar.</param>
    /// <param name="protocolConfig">Protocol related information.</param>
    void RunTests(
        TestRunRequestPayload testRunRequestPayLoad,
        ITestHostLauncher3? customTestHostLauncher,
        ITestRunEventsRegistrar testRunEventsRegistrar,
        ProtocolConfig protocolConfig);

    /// <summary>
    /// Processes test run attachments.
    /// </summary>
    ///
    /// <param name="testRunAttachmentsProcessingPayload">
    /// Test run attachments processing payload.
    /// </param>
    /// <param name="testRunAttachmentsProcessingEventsHandler">
    /// Test run attachments processing events handler.
    /// </param>
    /// <param name="protocolConfig">Protocol related information.</param>
    void ProcessTestRunAttachments(
        TestRunAttachmentsProcessingPayload testRunAttachmentsProcessingPayload,
        ITestRunAttachmentsProcessingEventsHandler testRunAttachmentsProcessingEventsHandler,
        ProtocolConfig protocolConfig);

    /// <summary>
    /// Starts a test session.
    /// </summary>
    ///
    /// <param name="payload">The start test session payload.</param>
    /// <param name="testHostLauncher">The custom test host launcher.</param>
    /// <param name="eventsHandler">The events handler.</param>
    /// <param name="protocolConfig">Protocol related information.</param>
    void StartTestSession(
        StartTestSessionPayload payload,
        ITestHostLauncher3? testHostLauncher,
        ITestSessionEventsHandler eventsHandler,
        ProtocolConfig protocolConfig);

    /// <summary>
    /// Stops a test session.
    /// </summary>
    ///
    /// <param name="testSessionInfo">The stop test session payload.</param>
    /// <param name="eventsHandler">The events handler.</param>
    /// <param name="protocolConfig">Protocol related information.</param>
    void StopTestSession(
        StopTestSessionPayload payload,
        ITestSessionEventsHandler eventsHandler,
        ProtocolConfig protocolConfig);

    /// <summary>
    /// Cancel the current test run request.
    /// </summary>
    void CancelTestRun();

    /// <summary>
    /// Abort the current test run.
    /// </summary>
    void AbortTestRun();

    /// <summary>
    /// Cancels the current discovery request.
    /// </summary>
    void CancelDiscovery();

    /// <summary>
    /// Cancels the current test run attachments processing request.
    /// </summary>
    void CancelTestRunAttachmentsProcessing();
}
