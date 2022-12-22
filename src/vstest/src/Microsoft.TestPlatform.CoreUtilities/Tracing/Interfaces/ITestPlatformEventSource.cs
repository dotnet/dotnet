// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;

/// <summary>
/// TestPlatform Instrumentation events
/// </summary>
public interface ITestPlatformEventSource
{
    /// <summary>
    /// The vs test console start.
    /// </summary>
    void VsTestConsoleStart();

    /// <summary>
    /// The vs test console stop.
    /// </summary>
    void VsTestConsoleStop();

    /// <summary>
    /// The discovery request start.
    /// </summary>
    void DiscoveryRequestStart();

    /// <summary>
    /// The discovery request stop.
    /// </summary>
    void DiscoveryRequestStop();

    /// <summary>
    /// The execution request start.
    /// </summary>
    void ExecutionRequestStart();

    /// <summary>
    /// The execution request stop.
    /// </summary>
    void ExecutionRequestStop();

    /// <summary>
    /// The test host start.
    /// </summary>
    void TestHostStart();

    /// <summary>
    /// The test host stop.
    /// </summary>
    void TestHostStop();

    /// <summary>
    /// The test host AppDomain Start.
    /// </summary>
    void TestHostAppDomainCreationStart();

    /// <summary>
    /// The test host AppDomain Stop.
    /// </summary>
    void TestHostAppDomainCreationStop();

    /// <summary>
    /// The adapter search start.
    /// </summary>
    void AdapterSearchStart();

    /// <summary>
    /// The adapter search stop.
    /// </summary>
    void AdapterSearchStop();

    /// <summary>
    /// The adapter execution start.
    /// </summary>
    /// <param name="executorUri">
    /// The executor uri.
    /// </param>
    void AdapterExecutionStart(string executorUri);

    /// <summary>
    /// The adapter execution stop.
    /// </summary>
    /// <param name="numberOfTests">
    /// The number of tests.
    /// </param>
    void AdapterExecutionStop(long numberOfTests);

    /// <summary>
    /// The adapter discovery start.
    /// </summary>
    /// <param name="executorUri">
    /// The executor uri.
    /// </param>
    void AdapterDiscoveryStart(string executorUri);

    /// <summary>
    /// The adapter discovery stop.
    /// </summary>
    /// <param name="numberOfTests">
    /// The number of tests.
    /// </param>
    void AdapterDiscoveryStop(long numberOfTests);

    /// <summary>
    /// The discovery start.
    /// </summary>
    void DiscoveryStart();

    /// <summary>
    /// The discovery stop.
    /// </summary>
    /// <param name="numberOfTests">
    /// The number of tests.
    /// </param>
    void DiscoveryStop(long numberOfTests);

    /// <summary>
    /// The execution start.
    /// </summary>
    void ExecutionStart();

    /// <summary>
    /// The execution stop.
    /// </summary>
    /// <param name="numberOfTests">
    /// The number of tests.
    /// </param>
    void ExecutionStop(long numberOfTests);

    /// <summary>
    /// The data collection start.
    /// </summary>
    /// <param name="dataCollectorUri">
    /// The data collector uri.
    /// </param>
    void DataCollectionStart(string dataCollectorUri);

    /// <summary>
    /// The data collection stop.
    /// </summary>
    void DataCollectionStop();

    /// <summary>
    /// Mark the start of translation layer initialization.
    /// </summary>
    void TranslationLayerInitializeStart();

    /// <summary>
    /// Mark the completion of translation layer initialization.
    /// </summary>
    void TranslationLayerInitializeStop();

    /// <summary>
    /// Mark the start of translation layer discovery request.
    /// </summary>
    void TranslationLayerDiscoveryStart();

    /// <summary>
    /// Mark the completion of translation layer discovery request.
    /// </summary>
    void TranslationLayerDiscoveryStop();

    /// <summary>
    /// Mark the start of translation layer execution request.
    /// </summary>
    /// <param name="customTestHost">Set to 1 if custom test host is used, 0 otherwise</param>
    /// <param name="sourcesCount"></param>
    /// <param name="testCasesCount">Number of test cases if request has a filter</param>
    /// <param name="runSettings"></param>
    void TranslationLayerExecutionStart(long customTestHost, long sourcesCount, long testCasesCount, string runSettings);

    /// <summary>
    /// Mark the completion of translation layer execution request.
    /// </summary>
    void TranslationLayerExecutionStop();

    /// <summary>
    /// Mark the start of Metrics Dispose.
    /// </summary>
    void MetricsDisposeStart();

    /// <summary>
    /// Mark the completion of Metrics Dispose.
    /// </summary>
    void MetricsDisposeStop();

    /// <summary>
    /// The test run attachments processing request start.
    /// </summary>
    void TestRunAttachmentsProcessingRequestStart();

    /// <summary>
    /// The test run attachments processing request stop.
    /// </summary>
    void TestRunAttachmentsProcessingRequestStop();

    /// <summary>
    /// The test run attachments processing start.
    /// </summary>
    /// <param name="numberOfAttachments">
    /// The number of attachments.
    /// </param>
    void TestRunAttachmentsProcessingStart(long numberOfAttachments);

    /// <summary>
    /// The test run attachments processing stop.
    /// </summary>
    /// <param name="numberOfAttachments">
    /// The number of attachments.
    /// </param>
    void TestRunAttachmentsProcessingStop(long numberOfAttachments);

    /// <summary>
    /// Mark the start of translation layer test run attachments processing request.
    /// </summary>
    void TranslationLayerTestRunAttachmentsProcessingStart();

    /// <summary>
    /// Mark the completion of translation layer test run attachments processing request.
    /// </summary>
    void TranslationLayerTestRunAttachmentsProcessingStop();

    /// <summary>
    /// The start of the test session start request.
    /// </summary>
    void StartTestSessionStart();

    /// <summary>
    /// The end of the test session start request.
    /// </summary>
    void StartTestSessionStop();

    /// <summary>
    /// Mark the start of a translation layer start test session request.
    /// </summary>
    void TranslationLayerStartTestSessionStart();

    /// <summary>
    /// Mark the end of a translation layer start test session request.
    /// </summary>
    void TranslationLayerStartTestSessionStop();

    /// <summary>
    /// The start of the test session stop request.
    /// </summary>
    void StopTestSessionStart();

    /// <summary>
    /// The end of the test session stop request.
    /// </summary>
    void StopTestSessionStop();

    /// <summary>
    /// Mark the start of a translation layer stop test session request.
    /// </summary>
    void TranslationLayerStopTestSessionStart();

    /// <summary>
    /// Mark the end of a translation layer stop test session request.
    /// </summary>
    void TranslationLayerStopTestSessionStop();
}
