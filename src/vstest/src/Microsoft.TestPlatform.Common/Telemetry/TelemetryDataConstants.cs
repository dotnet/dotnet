// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Common.Telemetry;

/// <summary>
/// The Telemetry data constants.
/// </summary>
internal static class TelemetryDataConstants
{
    // ******************** General ***********************
    public static readonly string DiscoveredExtensions = "VS.TestPlatform.DiscoveredExtensions";

    // ******************** Execution ***********************
    public static readonly string ParallelEnabledDuringExecution = "VS.TestRun.ParallelEnabled";

    // Total number of tests ran under one test request
    public static readonly string TotalTestsRun = "VS.TestRun.TotalTests";

    // Total time taken to complete one test run request
    public static readonly string TimeTakenInSecForRun = "VS.TestRun.TimeTakenInSec";

    public static readonly string TestSettingsUsed = "VS.TestRun.IsTestSettingsUsed";

    public static readonly string DisableAppDomain = "VS.TestRun.DisableAppDomain";

    // All data related to legacy settings nodes will be prefixed with this.
    public static readonly string LegacySettingPrefix = "VS.TestRun.LegacySettings";

    public static readonly string DataCollectorsEnabled = "VS.TestRun.DataCollectorsEnabled";

    public static readonly string InvokedDataCollectors = "VS.TestRun.InvokedDataCollectors";

    public static readonly string DataCollectorsCorProfiler = "VS.TestPlatform.DataCollector.CorProfiler";

    public static readonly string DataCollectorsCoreClrProfiler = "VS.TestPlatform.DataCollector.CoreClrProfiler";

    public static readonly string RunState = "VS.TestRun.RunState";

    public static readonly string NumberOfSourcesSentForRun = "VS.TestRun.NumberOfSources";

    public static readonly string TargetDevice = "VS.TestRun.TargetDevice";

    public static readonly string TargetFramework = "VS.TestRun.TargetFramework";

    public static readonly string TargetPlatform = "VS.TestRun.TargetPlatform";

    public static readonly string MaxCPUcount = "VS.TestRun.MaxCPUcount";

    public static readonly string TestPlatformVersion = "VS.TestRun.TestPlatformVersion";

    public static readonly string TargetOS = "VS.TestRun.TargetOS";

    public static readonly string LoggerUsed = "VS.TestRun.LoggersUsed";

    public static readonly string CommandLineSwitches = "VS.TestRun.CommandLineSwitches";

    // Adapter name will get appended. eg:- VS.TestRun.TotalTestsRun.executor//cppunittestexecutor/v1
    // In case of parallel it will be sum of all tests ran by an adapter in different execution process
    public static readonly string TotalTestsRanByAdapter = "VS.TestRun.TotalTestsRun";

    // Adapter name will get appended. eg:- VS.TestRun.TimeTakenToRun.executor//cppunittestexecutor/v1
    // In case of parallel it will be sum of all time taken by an adapter to run tests in different execution process
    public static readonly string TimeTakenToRunTestsByAnAdapter = "VS.TestRun.TimeTakenToRun";

    // Reports details for MSTestV1. Reports just the count when TPv1 is used.
    // Reports legacy when the legacy runner TMI / TPv0 is used.
    // Adds an extension when an extension (tips) is used.
    // eg:- VS.TestRun.TotalTestsRunByMSTestv1 - a unit test using TPv1 and MSTestV1
    // eg:- VS.TestRun.TotalTestsRunByMSTestv1.legacy - a unit test using TPv0 and MSTestV1
    // eg:- VS.TestRun.TotalTestsRunByMSTestv1.legacy.extension.codedui - a coded ui test using TPv0 and MSTestV1
    // Counts in this metric are not subtracted from the TotalTestsRanByAdapter. This metrics just
    // provides more insight into what was actually executed.
    public static readonly string TotalTestsRunByMSTestv1 = "VS.TestRun.TotalTestsRunByMSTestv1";

    // Total number of adapter discovered on the machine.
    public static readonly string NumberOfAdapterDiscoveredDuringExecution = "VS.TestRun.AdaptersDiscoveredCount";

    public static readonly string NumberOfAdapterUsedToRunTests = "VS.TestRun.AdaptersUsedCount";

    // It will be the sum of the times taken by all adapter to run tests.
    // In case of parallel it can be more than total time taken to complete run request.
    public static readonly string TimeTakenByAllAdaptersInSec = "VS.TestRun.TimeTakenByAllAdapters";

    // *********************Discovery****************************
    public static readonly string TotalTestsDiscovered = "VS.TestDiscovery.TotalTests";

    public static readonly string ParallelEnabledDuringDiscovery = "VS.TestDiscovery.ParallelEnabled";

    // All the times are in sec
    public static readonly string TimeTakenInSecForDiscovery = "VS.TestDiscovery.TotalTimeTakenInSec";

    public static readonly string TimeTakenToLoadAdaptersInSec = "VS.TestDiscovery.TimeTakenToLoadAdaptersInSec";

    // It will be the sum of the times taken by all adapter to discover tests.
    public static readonly string TimeTakenInSecByAllAdapters = "VS.TestDiscovery.TimeTakenInSecByAllAdapters";

    // Adapter name will get appended. eg:- VS.TestDiscovery.TimeTakenAdapter.executor//cppunittestexecutor/v1
    public static readonly string TimeTakenToDiscoverTestsByAnAdapter = "VS.TestDiscovery.TimeTakenAdapter";

    // Adapter name will get appended. eg:- VS.TestDiscovery.TotalTestsDiscovered.executor//cppunittestexecutor/v1
    public static readonly string TotalTestsByAdapter = "VS.TestDiscovery.TotalTestsDiscovered";

    public static readonly string DiscoveryState = "VS.TestDiscovery.DiscoveryState";

    public static readonly string NumberOfSourcesSentForDiscovery = "VS.TestDiscovery.NumberOfSources";

    public static readonly string NumberOfAdapterDiscoveredDuringDiscovery = "VS.TestDiscovery.AdaptersDiscoveredCount";

    public static readonly string NumberOfAdapterUsedToDiscoverTests = "VS.TestDiscovery.AdaptersUsedCount";

    // *********************Attachments Processing****************************
    public static readonly string NumberOfAttachmentsSentForProcessing = "VS.AttachmentsProcessing.InitialAttachmentsCount";

    public static readonly string NumberOfAttachmentsAfterProcessing = "VS.AttachmentsProcessing.FinalAttachmentsCount";

    public static readonly string TimeTakenInSecForAttachmentsProcessing = "VS.AttachmentsProcessing.TotalTimeTakenInSec";

    public static readonly string AttachmentsProcessingState = "VS.AttachmentsProcessing.State";

    // *********************Test Sessions****************************
    public static readonly string ParallelEnabledDuringStartTestSession = "VS.TestSession.ParallelEnabled";

    public static readonly string TestSessionId = "VS.TestSession.Id";

    public static readonly string TestSessionSpawnedTesthostCount = "VS.TestSession.SpawnedTesthostCount";

    public static readonly string TestSessionTesthostSpawnTimeInSec = "VS.TestSession.TesthostSpawnTimeInSec";

    public static readonly string TestSessionState = "VS.TestSession.State";

    public static readonly string TestSessionTotalSessionTimeInSec = "VS.TestSession.TotalSessionTimeInSec";

    // **************Events Name **********************************
    public static readonly string TestDiscoveryCompleteEvent = "vs/testplatform/testdiscoverysession";

    public static readonly string TestExecutionCompleteEvent = "vs/testplatform/testrunsession";

    public static readonly string TestAttachmentsProcessingCompleteEvent = "vs/testplatform/testattachmentsprocessingsession";

    public static readonly string StartTestSessionCompleteEvent = "vs/testplatform/starttestsession";

    public static readonly string StopTestSessionCompleteEvent = "vs/testplatform/stoptestsession";
}
