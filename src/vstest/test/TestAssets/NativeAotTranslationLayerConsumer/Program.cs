// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Minimal consumer of the TranslationLayer API surface to exercise code paths
// that must be AoT-safe. This app is published with PublishAot=true by the
// NativeAotCompatibilityTests integration test, which reads the IL warnings the
// publish reports and asserts that none of them come from the serialization code.
//
// The app doesn't need to actually run against a vstest.console instance; it
// just needs to reference enough API surface for the linker to analyze the
// full call graph.

using System;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

// Reference VsTestConsoleWrapper to pull in the TranslationLayer and ensure
// the linker transitively analyzes JsonDataSerializer, the source-gen context,
// and all the custom converters.
Console.WriteLine("NativeAOT TranslationLayer consumer — linker analysis target.");

// Touch the VsTestConsoleWrapper type so the linker doesn't remove it.
// We can't actually connect (no vstest.console running), but the linker
// needs to see the type is used to analyze its full dependency graph.
Console.WriteLine($"VsTestConsoleWrapper type: {typeof(VsTestConsoleWrapper).FullName}");

var parameters = new ConsoleParameters();
Console.WriteLine($"ConsoleParameters created: LogFilePath={parameters.LogFilePath}");

// Touch key ObjectModel types that flow through the wire protocol
// to ensure the linker preserves them and their serialization metadata.
var testCase = new TestCase("Namespace.Class.Method", new Uri("executor://test"), "test.dll");
Console.WriteLine($"TestCase: {testCase.FullyQualifiedName}");

var testResult = new TestResult(testCase) { Outcome = TestOutcome.Passed };
Console.WriteLine($"TestResult: {testResult.Outcome}");

// Touch payload types to ensure they're preserved.
var discoveryPayload = new DiscoveryRequestPayload { Sources = ["test.dll"], RunSettings = "<RunSettings/>" };
Console.WriteLine($"DiscoveryRequestPayload sources: {string.Join(",", discoveryPayload.Sources!)}");

var testRunPayload = new TestRunRequestPayload { Sources = ["test.dll"], RunSettings = "<RunSettings/>" };
Console.WriteLine($"TestRunRequestPayload sources: {string.Join(",", testRunPayload.Sources!)}");

var testSessionInfo = new TestSessionInfo();
var discoveryCriteria = new DiscoveryCriteria(
    ["test.dll"],
    frequencyOfDiscoveredTestsEvent: 10,
    discoveredTestEventTimeout: TimeSpan.FromSeconds(30),
    runSettings: "<RunSettings/>",
    testSessionInfo);

// Round trip through the public serializer entry point, the same one vstest.console and testhost
// use on the wire. It reaches TestPlatformJsonContext and the internal converters underneath, so
// the linker still analyzes them, and this consumer needs no access to internals.
var serialized = JsonDataSerializer.Instance.SerializePayload(MessageType.StartDiscovery, discoveryCriteria);
var message = JsonDataSerializer.Instance.DeserializeMessage(serialized);
var deserializedCriteria = JsonDataSerializer.Instance.DeserializePayload<DiscoveryCriteria>(message)
    ?? throw new InvalidOperationException("DiscoveryCriteria deserialization returned null.");

if (deserializedCriteria.TestSessionInfo?.Id != testSessionInfo.Id
    || deserializedCriteria.FrequencyOfDiscoveredTestsEvent != discoveryCriteria.FrequencyOfDiscoveredTestsEvent
    || deserializedCriteria.DiscoveredTestEventTimeout != discoveryCriteria.DiscoveredTestEventTimeout
    || deserializedCriteria.RunSettings != discoveryCriteria.RunSettings)
{
    throw new InvalidOperationException("DiscoveryCriteria NativeAOT round trip did not preserve its values.");
}

Console.WriteLine("NativeAOT serialization round trip succeeded.");
