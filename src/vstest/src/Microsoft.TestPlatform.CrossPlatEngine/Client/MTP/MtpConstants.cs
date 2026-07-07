// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Constants for the Microsoft.Testing.Platform (MTP) server-mode JSON-RPC protocol.
/// See the MTP protocol docs in microsoft/testfx (ServerMode/JsonRpc).
/// </summary>
internal static class MtpConstants
{
    // Command line used to start an MTP application in JSON-RPC server mode. vstest opens a TCP
    // listener and the application connects back to it (the application dials out to us).
    public const string ServerArgument = "--server";
    public const string ClientPortArgument = "--client-port";
    public const string NoBannerArgument = "--no-banner";

    // JSON-RPC method names.
    public const string InitializeMethod = "initialize";
    public const string DiscoverTestsMethod = "testing/discoverTests";
    public const string RunTestsMethod = "testing/runTests";
    public const string TestUpdatesTestsMethod = "testing/testUpdates/tests";
    public const string TestUpdatesAttachmentsMethod = "testing/testUpdates/attachments";
    public const string ClientLogMethod = "client/log";
    public const string ExitMethod = "exit";

    // Framing (LSP-like headers).
    public const string ContentLengthHeader = "Content-Length:";
    public const string ContentType = "application/testingplatform";

    // Request/notification parameter keys.
    public const string RunIdParameter = "runId";
    public const string TestsParameter = "tests";
    public const string ChangesProperty = "changes";
    public const string NodeProperty = "node";
    public const string AttachmentsProperty = "attachments";
    public const string AttachmentUriProperty = "uri";
    public const string AttachmentPathProperty = "path";

    // TestNode wire property keys (pure MTP shape).
    public const string Uid = "uid";
    public const string DisplayName = "display-name";
    public const string NodeType = "node-type";
    public const string ExecutionState = "execution-state";
    public const string TimeDurationMs = "time.duration-ms";
    public const string ErrorMessage = "error.message";
    public const string ErrorStackTrace = "error.stacktrace";
    public const string LocationFile = "location.file";
    public const string LocationLineStart = "location.line-start";
    public const string Traits = "traits";

    // Execution states.
    public const string StateDiscovered = "discovered";
    public const string StateInProgress = "in-progress";
    public const string StatePassed = "passed";
    public const string StateSkipped = "skipped";
    public const string StateFailed = "failed";
    public const string StateError = "error";
    public const string StateTimedOut = "timed-out";
    public const string StateCanceled = "canceled";

    // Optional VSTest-provider properties (present only when the app still runs on the VSTestBridge).
    // The converter treats these as best-effort enrichment and never requires them, so that a pure
    // MTP app with no vstest dependency at all still converts correctly.
    public const string VsTestFullyQualifiedName = "vstest.TestCase.FullyQualifiedName";
    public const string VsTestId = "vstest.TestCase.Id";
    public const string VsTestExecutorUri = "vstest.original-executor-uri";

    // Synthetic executor URI used when the app does not expose the vstest provider properties.
    public const string DefaultExecutorUri = "executor://MicrosoftTestingPlatform/v1";

    // Property used to round-trip the MTP node uid on a vstest TestCase so we can request a
    // filtered run by uid after discovery.
    public const string MtpUidPropertyId = "MTP.TestNode.Uid";
}
