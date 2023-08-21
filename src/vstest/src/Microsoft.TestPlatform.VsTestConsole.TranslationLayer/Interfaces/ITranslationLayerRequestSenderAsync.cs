// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

/// <summary>
/// Asynchronous equivalent of <see cref="ITranslationLayerRequestSender"/>.
/// </summary>
internal interface ITranslationLayerRequestSenderAsync : IDisposable
{
    /// <summary>
    /// Asynchronous equivalent of <see cref="
    ///     ITranslationLayerRequestSender.InitializeCommunication"/>
    /// and <see cref="
    ///     ITranslationLayerRequestSender.WaitForRequestHandlerConnection(
    ///     int)"/>.
    /// </summary>
    Task<int> InitializeCommunicationAsync(int clientConnectionTimeout);

    /// <summary>
    /// Asynchronous equivalent of ITranslationLayerRequestSender.DiscoverTests/>.
    /// </summary>
    Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// ITranslationLayerRequestSender.StartTestRun(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITelemetryEventsHandler)"/>.
    /// </summary>
    Task StartTestRunAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// ITranslationLayerRequestSender.StartTestRun(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITelemetryEventsHandler)"/>.
    /// </summary>
    Task StartTestRunAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// ITranslationLayerRequestSender.StartTestRunWithCustomHost(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITelemetryEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    Task StartTestRunWithCustomHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// ITranslationLayerRequestSender.StartTestRunWithCustomHost(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITelemetryEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    Task StartTestRunWithCustomHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler runEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// ITranslationLayerRequestSender.StartTestSession(
    ///     IList{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     ITestSessionEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    Task<TestSessionInfo?> StartTestSessionAsync(
        IList<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler,
        ITestHostLauncher? testHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// ITranslationLayerRequestSender.StopTestSession(
    ///     TestSessionInfo,
    ///     TestPlatformOptions,
    ///     ITestSessionEventsHandler)"/>.
    /// </summary>
    Task<bool> StopTestSessionAsync(
        TestSessionInfo? testSessionInfo,
        TestPlatformOptions? options,
        ITestSessionEventsHandler eventsHandler);

    /// <summary>
    /// Provides back all attachments to test platform for additional processing (for example
    /// merging).
    /// </summary>
    ///
    /// <param name="attachments">Collection of attachments.</param>
    /// <param name="invokedDataCollectors">Collection of invoked data collectors.</param>
    /// <param name="runSettings">RunSettings configuration</param>
    /// <param name="collectMetrics">Enables metrics collection.</param>
    /// <param name="testRunAttachmentsProcessingCompleteEventsHandler">Events handler.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ProcessTestRunAttachmentsAsync(
        IEnumerable<AttachmentSet> attachments,
        IEnumerable<InvokedDataCollector>? invokedDataCollectors,
        string? runSettings,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler testRunAttachmentsProcessingCompleteEventsHandler,
        CancellationToken cancellationToken);
}
