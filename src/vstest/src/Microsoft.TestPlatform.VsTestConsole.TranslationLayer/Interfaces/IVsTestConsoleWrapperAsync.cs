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
/// Asynchronous equivalent of <see cref="IVsTestConsoleWrapper"/>.
/// </summary>
///
/// <remarks>
/// The type is obsolete as a reference: consumers should name the derived <see cref="IVsTestConsoleWrapper"/>
/// instead, which exposes every member declared here. Most of those members are individually deprecated on top
/// of that, either because they do not actually run asynchronously or because they merely duplicate the
/// synchronous member of the same name, and each carries its own attribute pointing at the synchronous API.
/// <para>
/// The per-member attributes below are kept deliberately rather than replaced by this one. The C# compiler
/// does not report an interface-level obsoletion when a member is invoked through the derived
/// <see cref="IVsTestConsoleWrapper"/>, which is how virtually every consumer reaches these methods, so
/// removing them would silence the warning for almost everybody. The two attributes cover different call
/// shapes: this one flags code that names <c>IVsTestConsoleWrapperAsync</c> directly, the member ones flag
/// the calls themselves.
/// </para>
/// <para>
/// The two <c>ProcessTestRunAttachmentsAsync</c> overloads are the exception: they genuinely run
/// asynchronously and have no synchronous replacement, so they carry no member-level attribute. They are still
/// reachable, without any warning, through <see cref="IVsTestConsoleWrapper"/> — which is what code that only
/// wants them should reference, since naming this interface warns regardless of which member is used.
/// </para>
/// <para>
/// This is not a binary breaking change, and <c>error: false</c> keeps it a warning. Code that names this type
/// directly does get a warning it did not get before.
/// </para>
/// </remarks>
[Obsolete("Reference IVsTestConsoleWrapper instead, which exposes the same members.", error: false)]
public interface IVsTestConsoleWrapperAsync
{
    /// <summary>
    /// Asynchronous equivalent of <see cref="IVsTestConsoleWrapper.StartSession"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task StartSessionAsync();

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.InitializeExtensions(
    ///     IEnumerable{string})"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task InitializeExtensionsAsync(IEnumerable<string> pathToAdditionalExtensions);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.DiscoverTests(
    ///     IEnumerable{string},
    ///     string,
    ///     ITestDiscoveryEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? discoverySettings,
        ITestDiscoveryEventsHandler discoveryEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.DiscoverTests(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     ITestDiscoveryEventsHandler2)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.DiscoverTests(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestDiscoveryEventsHandler2)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task DiscoverTestsAsync(
        IEnumerable<string> sources,
        string? discoverySettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestDiscoveryEventsHandler2 discoveryEventsHandler);

    /// <summary>
    /// See <see cref="IVsTestConsoleWrapper.CancelDiscovery"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    void CancelDiscovery();

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTests(
    ///     IEnumerable{string},
    ///     string,
    ///     ITestRunEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTests(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     ITestRunEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTests(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTests(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITelemetryEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTests(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     ITestRunEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    ///     IVsTestConsoleWrapper.RunTests(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     TestPlatformOptions,
    ///     ITestRunEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    ///     IVsTestConsoleWrapper.RunTests(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    ///     IVsTestConsoleWrapper.RunTests(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITelemetryEventsHandler)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTestsWithCustomTestHost(
    ///     IEnumerable{string},
    ///     string,
    ///     ITestRunEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTestsWithCustomTestHost(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     ITestRunEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTestsWithCustomTestHost(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTestsWithCustomTestHost(
    ///     IEnumerable{string},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITelemetryEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsWithCustomTestHostAsync(
        IEnumerable<string> sources,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTestsWithCustomTestHost(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     ITestRunEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTestsWithCustomTestHost(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     TestPlatformOptions,
    ///     ITestRunEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTestsWithCustomTestHost(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// Asynchronous equivalent of <see cref="
    /// IVsTestConsoleWrapper.RunTestsWithCustomTestHost(
    ///     IEnumerable{TestCase},
    ///     string,
    ///     TestPlatformOptions,
    ///     TestSessionInfo,
    ///     ITestRunEventsHandler,
    ///     ITelemetryEventsHandler,
    ///     ITestHostLauncher)"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    Task RunTestsWithCustomTestHostAsync(
        IEnumerable<TestCase> testCases,
        string? runSettings,
        TestPlatformOptions? options,
        TestSessionInfo? testSessionInfo,
        ITestRunEventsHandler testRunEventsHandler,
        ITelemetryEventsHandler telemetryEventsHandler,
        ITestHostLauncher customTestHostLauncher);

    /// <summary>
    /// See <see cref="IVsTestConsoleWrapper.CancelTestRun"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    void CancelTestRun();

    /// <summary>
    /// See <see cref="IVsTestConsoleWrapper.AbortTestRun"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    void AbortTestRun();

    /// <summary>
    /// Gets back all attachments to test platform for additional processing (for example merging).
    /// </summary>
    ///
    /// <param name="attachments">Collection of attachments.</param>
    /// <param name="processingSettings">XML processing settings.</param>
    /// <param name="isLastBatch">
    /// Indicates that all test executions are done and all data is provided.
    /// </param>
    /// <param name="collectMetrics">Enables metrics collection (used for telemetry).</param>
    /// <param name="eventsHandler">Event handler to receive session complete event.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ProcessTestRunAttachmentsAsync(
        IEnumerable<AttachmentSet> attachments,
        string? processingSettings,
        bool isLastBatch,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler eventsHandler,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets back all attachments to test platform for additional processing (for example merging).
    /// </summary>
    ///
    /// <param name="attachments">Collection of attachments.</param>
    /// <param name="invokedDataCollectors">Collection of invoked data collectors.</param>
    /// <param name="processingSettings">XML processing settings.</param>
    /// <param name="isLastBatch">
    /// Indicates that all test executions are done and all data is provided.
    /// </param>
    /// <param name="collectMetrics">Enables metrics collection (used for telemetry).</param>
    /// <param name="eventsHandler">Event handler to receive session complete event.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ProcessTestRunAttachmentsAsync(
        IEnumerable<AttachmentSet> attachments,
        IEnumerable<InvokedDataCollector>? invokedDataCollectors,
        string? processingSettings,
        bool isLastBatch,
        bool collectMetrics,
        ITestRunAttachmentsProcessingEventsHandler eventsHandler,
        CancellationToken cancellationToken);

    /// <summary>
    /// See <see cref="IVsTestConsoleWrapper.EndSession"/>.
    /// </summary>
    [Obsolete("The async APIs don't work, use the sync API instead.")]
    void EndSession();
}
