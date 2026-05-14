// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeTestPlatformEventSource : ITestPlatformEventSource
{
    public FakeTestPlatformEventSource(FakeErrorAggregator fakeErrorAggregator)
    {
        FakeErrorAggregator = fakeErrorAggregator;
    }

    public FakeErrorAggregator FakeErrorAggregator { get; }

    public void AdapterDiscoveryStart(string executorUri)
    {
        // do nothing
    }

    public void AdapterDiscoveryStop(long numberOfTests)
    {
        // do nothing
    }

    public void AdapterExecutionStart(string executorUri)
    {
        // do nothing
    }

    public void AdapterExecutionStop(long numberOfTests)
    {
        // do nothing
    }

    public void AdapterSearchStart()
    {
        // do nothing
    }

    public void AdapterSearchStop()
    {
        // do nothing
    }

    public void DataCollectionStart(string dataCollectorUri)
    {
        // do nothing
    }

    public void DataCollectionStop()
    {
        // do nothing
    }

    public void DiscoveryRequestStart()
    {
        // do nothing
    }

    public void DiscoveryRequestStop()
    {
        // do nothing
    }

    public void DiscoveryStart()
    {
        // do nothing
    }

    public void DiscoveryStop(long numberOfTests)
    {
        // do nothing
    }

    public void ExecutionRequestStart()
    {
        // do nothing
    }

    public void ExecutionRequestStop()
    {
        // do nothing
    }

    public void ExecutionStart()
    {
        // do nothing
    }

    public void ExecutionStop(long numberOfTests)
    {
        // do nothing
    }

    public void MetricsDisposeStart()
    {
        // do nothing
    }

    public void MetricsDisposeStop()
    {
        // do nothing
    }

    public void StartTestSessionStart()
    {
        // do nothing
    }

    public void StartTestSessionStop()
    {
        // do nothing
    }

    public void StopTestSessionStart()
    {
        // do nothing
    }

    public void StopTestSessionStop()
    {
        // do nothing
    }

    public void TestHostAppDomainCreationStart()
    {
        // do nothing
    }

    public void TestHostAppDomainCreationStop()
    {
        // do nothing
    }

    public void TestHostStart()
    {
        // do nothing
    }

    public void TestHostStop()
    {
        // do nothing
    }

    public void TestRunAttachmentsProcessingRequestStart()
    {
        // do nothing
    }

    public void TestRunAttachmentsProcessingRequestStop()
    {
        // do nothing
    }

    public void TestRunAttachmentsProcessingStart(long numberOfAttachments)
    {
        // do nothing
    }

    public void TestRunAttachmentsProcessingStop(long numberOfAttachments)
    {
        // do nothing
    }

    public void TranslationLayerDiscoveryStart()
    {
        // do nothing
    }

    public void TranslationLayerDiscoveryStop()
    {
        // do nothing
    }

    public void TranslationLayerExecutionStart(long customTestHost, long sourcesCount, long testCasesCount, string runSettings)
    {
        // do nothing
    }

    public void TranslationLayerExecutionStop()
    {
        // do nothing
    }

    public void TranslationLayerInitializeStart()
    {
        // do nothing
    }

    public void TranslationLayerInitializeStop()
    {
        // do nothing
    }

    public void TranslationLayerStartTestSessionStart()
    {
        // do nothing
    }

    public void TranslationLayerStartTestSessionStop()
    {
        // do nothing
    }

    public void TranslationLayerStopTestSessionStart()
    {
        // do nothing
    }

    public void TranslationLayerStopTestSessionStop()
    {
        // do nothing
    }

    public void TranslationLayerTestRunAttachmentsProcessingStart()
    {
        // do nothing
    }

    public void TranslationLayerTestRunAttachmentsProcessingStop()
    {
        // do nothing
    }

    public void VsTestConsoleStart()
    {
        // do nothing
    }

    public void VsTestConsoleStop()
    {
        // do nothing
    }
}
