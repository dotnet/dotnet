// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;

/// <summary>
/// TestPlatform Event Ids and tasks constants
/// </summary>
internal class TestPlatformInstrumentationEvents
{
    /// <summary>
    /// The discovery start event id.
    /// </summary>
    public const int DiscoveryStartEventId = 0x1;

    /// <summary>
    /// The discovery stop event id.
    /// </summary>
    public const int DiscoveryStopEventId = 0x2;

    /// <summary>
    /// The execution start event id.
    /// </summary>
    public const int ExecutionStartEventId = 0x4;

    /// <summary>
    /// The execution stop event id.
    /// </summary>
    public const int ExecutionStopEventId = 0x5;

    /// <summary>
    /// The adapter execution start event id.
    /// </summary>
    public const int AdapterExecutionStartEventId = 0x7;

    /// <summary>
    /// The adapter execution stop event id.
    /// </summary>
    public const int AdapterExecutionStopEventId = 0x8;

    /// <summary>
    /// The console runner start event id.
    /// </summary>
    public const int VsTestConsoleStartEventId = 0x10;

    /// <summary>
    /// The console runner stop event id.
    /// </summary>
    public const int VsTestConsoleStopEventId = 0x11;

    /// <summary>
    /// The test host start event id.
    /// </summary>
    public const int TestHostStartEventId = 0x13;

    /// <summary>
    /// The test host stop event id.
    /// </summary>
    public const int TestHostStopEventId = 0x14;

    /// <summary>
    /// The adapter search start event id.
    /// </summary>
    public const int AdapterSearchStartEventId = 0x16;

    /// <summary>
    /// The adapter search stop event id.
    /// </summary>
    public const int AdapterSearchStopEventId = 0x17;

    /// <summary>
    /// The discovery request start event id.
    /// </summary>
    public const int DiscoveryRequestStartEventId = 0x19;

    /// <summary>
    /// The discovery request stop event id.
    /// </summary>
    public const int DiscoveryRequestStopEventId = 0x20;

    /// <summary>
    /// The execution request start event id.
    /// </summary>
    public const int ExecutionRequestStartEventId = 0x21;

    /// <summary>
    /// The execution request stop event id.
    /// </summary>
    public const int ExecutionRequestStopEventId = 0x22;

    /// <summary>
    /// The data collection start event id.
    /// </summary>
    public const int DataCollectionStartEventId = 0x25;

    /// <summary>
    /// The data collection stop event id.
    /// </summary>
    public const int DataCollectionStopEventId = 0x26;

    /// <summary>
    /// The adapter discovery start event id.
    /// </summary>
    public const int AdapterDiscoveryStartEventId = 0x27;

    /// <summary>
    /// The adapter discovery stop event id.
    /// </summary>
    public const int AdapterDiscoveryStopEventId = 0x28;

    /// <summary>
    /// The test host appdomain start event id.
    /// </summary>
    public const int TestHostAppDomainCreationStartEventId = 0x30;

    /// <summary>
    /// The test host appdomain stop event id.
    /// </summary>
    public const int TestHostAppDomainCreationStopEventId = 0x31;

    /// <summary>
    /// Events fired on initialization of translation layer.
    /// </summary>
    public const int TranslationLayerInitializeStartEventId = 0x32;

    /// <summary>
    /// Events fired on initialization complete of translation layer.
    /// </summary>
    public const int TranslationLayerInitializeStopEventId = 0x33;

    /// <summary>
    /// Events fired on discovery start of translation layer.
    /// </summary>
    public const int TranslationLayerDiscoveryStartEventId = 0x34;

    /// <summary>
    /// Events fired on discovery complete in translation layer.
    /// </summary>
    public const int TranslationLayerDiscoveryStopEventId = 0x35;

    /// <summary>
    /// Event fired on execution start in translation layer.
    /// </summary>
    public const int TranslationLayerExecutionStartEventId = 0x36;

    /// <summary>
    /// Event fired on execution complete in translation layer.
    /// </summary>
    public const int TranslationLayerExecutionStopEventId = 0x37;

    /// <summary>
    /// Event fired on Metrics Dispose start.
    /// </summary>
    public const int MetricsDisposeStartEventId = 0x38;

    /// <summary>
    /// Event fired on Metrics Dispose completes.
    /// </summary>
    public const int MetricsDisposeStopEventId = 0x39;

    /// <summary>
    /// The session attachments processing start event id.
    /// </summary>
    public const int TestRunAttachmentsProcessingStartEventId = 0x40;

    /// <summary>
    /// The session attachments processing stop event id.
    /// </summary>
    public const int TestRunAttachmentsProcessingStopEventId = 0x41;

    /// <summary>
    /// The session attachments processing request start event id.
    /// </summary>
    public const int TestRunAttachmentsProcessingRequestStartEventId = 0x42;

    /// <summary>
    /// The session attachments processing request stop event id.
    /// </summary>
    public const int TestRunAttachmentsProcessingRequestStopEventId = 0x43;

    /// <summary>
    /// Events fired on session attachments processing start of translation layer.
    /// </summary>
    public const int TranslationLayerTestRunAttachmentsProcessingStartEventId = 0x44;

    /// <summary>
    /// Events fired on session attachments processing complete in translation layer.
    /// </summary>
    public const int TranslationLayerTestRunAttachmentsProcessingStopEventId = 0x45;

    /// <summary>
    /// The start test session start event id.
    /// </summary>
    public const int StartTestSessionStartEventId = 0x46;

    /// <summary>
    /// The start test session stop event id.
    /// </summary>
    public const int StartTestSessionStopEventId = 0x47;

    /// <summary>
    /// The translation layer start test session start event id.
    /// </summary>
    public const int TranslationLayerStartTestSessionStartEventId = 0x48;

    /// <summary>
    /// The translation layer start test session stop event id.
    /// </summary>
    public const int TranslationLayerStartTestSessionStopEventId = 0x49;

    /// <summary>
    /// The stop test session start event id.
    /// </summary>
    public const int StopTestSessionStartEventId = 0x4A;

    /// <summary>
    /// The stop test session stop event id.
    /// </summary>
    public const int StopTestSessionStopEventId = 0x4B;

    /// <summary>
    /// The translation layer stop test session start event id.
    /// </summary>
    public const int TranslationLayerStopTestSessionStartEventId = 0x4C;

    /// <summary>
    /// The translation layer stop test session stop event id.
    /// </summary>
    public const int TranslationLayerStopTestSessionStopEventId = 0x4D;
}
