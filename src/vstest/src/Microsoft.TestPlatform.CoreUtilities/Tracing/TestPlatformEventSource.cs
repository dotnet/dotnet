// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.Tracing;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;

/// <inheritdoc/>
[EventSource(Name = "TestPlatform")]
public class TestPlatformEventSource : EventSource, ITestPlatformEventSource
{
    private static readonly TestPlatformEventSource LocalInstance = new();

    /// <summary>
    /// Gets the instance of <see cref="TestPlatformEventSource"/>.
    /// </summary>
    public static ITestPlatformEventSource Instance
    {
        get
        {
            return LocalInstance;
        }
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.VsTestConsoleStartEventId)]
    public void VsTestConsoleStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.VsTestConsoleStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.VsTestConsoleStopEventId)]
    public void VsTestConsoleStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.VsTestConsoleStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.DiscoveryRequestStartEventId)]
    public void DiscoveryRequestStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.DiscoveryRequestStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.DiscoveryRequestStopEventId)]
    public void DiscoveryRequestStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.DiscoveryRequestStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.ExecutionRequestStartEventId)]
    public void ExecutionRequestStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.ExecutionRequestStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.ExecutionRequestStopEventId)]
    public void ExecutionRequestStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.ExecutionRequestStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TestHostStartEventId)]
    public void TestHostStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TestHostStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TestHostStopEventId)]
    public void TestHostStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TestHostStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TestHostAppDomainCreationStartEventId)]
    public void TestHostAppDomainCreationStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TestHostAppDomainCreationStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TestHostAppDomainCreationStopEventId)]
    public void TestHostAppDomainCreationStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TestHostAppDomainCreationStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.AdapterSearchStartEventId)]
    public void AdapterSearchStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.AdapterSearchStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.AdapterSearchStopEventId)]
    public void AdapterSearchStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.AdapterSearchStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.AdapterExecutionStartEventId)]
    public void AdapterExecutionStart(string executorUri)
    {
        WriteEvent(TestPlatformInstrumentationEvents.AdapterExecutionStartEventId, executorUri);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.AdapterExecutionStopEventId)]
    public void AdapterExecutionStop(long numberOfTests)
    {
        WriteEvent(TestPlatformInstrumentationEvents.AdapterExecutionStopEventId, numberOfTests);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.AdapterDiscoveryStartEventId)]
    public void AdapterDiscoveryStart(string executorUri)
    {
        WriteEvent(TestPlatformInstrumentationEvents.AdapterDiscoveryStartEventId, executorUri);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.AdapterDiscoveryStopEventId)]
    public void AdapterDiscoveryStop(long numberOfTests)
    {
        WriteEvent(TestPlatformInstrumentationEvents.AdapterDiscoveryStopEventId, numberOfTests);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.DiscoveryStartEventId)]
    public void DiscoveryStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.DiscoveryStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.DiscoveryStopEventId)]
    public void DiscoveryStop(long numberOfTests)
    {
        WriteEvent(TestPlatformInstrumentationEvents.DiscoveryStopEventId, numberOfTests);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.ExecutionStartEventId)]
    public void ExecutionStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.ExecutionStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.ExecutionStopEventId)]
    public void ExecutionStop(long numberOfTests)
    {
        WriteEvent(TestPlatformInstrumentationEvents.ExecutionStopEventId, numberOfTests);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.DataCollectionStartEventId)]
    public void DataCollectionStart(string dataCollectorUri)
    {
        WriteEvent(TestPlatformInstrumentationEvents.DataCollectionStartEventId, dataCollectorUri);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.DataCollectionStopEventId)]
    public void DataCollectionStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.DataCollectionStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerInitializeStartEventId)]
    public void TranslationLayerInitializeStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerInitializeStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerInitializeStopEventId)]
    public void TranslationLayerInitializeStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerInitializeStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerDiscoveryStartEventId)]
    public void TranslationLayerDiscoveryStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerDiscoveryStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerDiscoveryStopEventId)]
    public void TranslationLayerDiscoveryStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerDiscoveryStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerExecutionStartEventId)]
    public void TranslationLayerExecutionStart(long customTestHost, long sourcesCount, long testCasesCount, string runSettings)
    {
        WriteEvent(
            TestPlatformInstrumentationEvents.TranslationLayerExecutionStartEventId,
            customTestHost,
            sourcesCount,
            testCasesCount,
            runSettings);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerExecutionStopEventId)]
    public void TranslationLayerExecutionStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerExecutionStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.MetricsDisposeStartEventId)]
    public void MetricsDisposeStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.MetricsDisposeStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.MetricsDisposeStopEventId)]
    public void MetricsDisposeStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.MetricsDisposeStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TestRunAttachmentsProcessingRequestStartEventId)]
    public void TestRunAttachmentsProcessingRequestStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TestRunAttachmentsProcessingRequestStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TestRunAttachmentsProcessingRequestStopEventId)]
    public void TestRunAttachmentsProcessingRequestStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TestRunAttachmentsProcessingRequestStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TestRunAttachmentsProcessingStartEventId)]
    public void TestRunAttachmentsProcessingStart(long numberOfAttachments)
    {
        WriteEvent(TestPlatformInstrumentationEvents.TestRunAttachmentsProcessingStartEventId, numberOfAttachments);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TestRunAttachmentsProcessingStopEventId)]
    public void TestRunAttachmentsProcessingStop(long numberOfAttachments)
    {
        WriteEvent(TestPlatformInstrumentationEvents.TestRunAttachmentsProcessingStopEventId, numberOfAttachments);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerTestRunAttachmentsProcessingStartEventId)]
    public void TranslationLayerTestRunAttachmentsProcessingStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerTestRunAttachmentsProcessingStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerTestRunAttachmentsProcessingStopEventId)]
    public void TranslationLayerTestRunAttachmentsProcessingStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerTestRunAttachmentsProcessingStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.StartTestSessionStartEventId)]
    public void StartTestSessionStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.StartTestSessionStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.StartTestSessionStopEventId)]
    public void StartTestSessionStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.StartTestSessionStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerStartTestSessionStartEventId)]
    public void TranslationLayerStartTestSessionStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerStartTestSessionStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerStartTestSessionStopEventId)]
    public void TranslationLayerStartTestSessionStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerStartTestSessionStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.StopTestSessionStartEventId)]
    public void StopTestSessionStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.StopTestSessionStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.StopTestSessionStopEventId)]
    public void StopTestSessionStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.StopTestSessionStopEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerStopTestSessionStartEventId)]
    public void TranslationLayerStopTestSessionStart()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerStopTestSessionStartEventId);
    }

    /// <inheritdoc/>
    [Event(TestPlatformInstrumentationEvents.TranslationLayerStopTestSessionStopEventId)]
    public void TranslationLayerStopTestSessionStop()
    {
        WriteEvent(TestPlatformInstrumentationEvents.TranslationLayerStopTestSessionStopEventId);
    }
}
