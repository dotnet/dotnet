// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Minimal consumer of the TranslationLayer API surface to exercise code paths
// that must be AoT-safe. This app is published with PublishAot=true by the
// NativeAotCompatibilityTests integration test, which reads the IL warnings the
// publish reports and runs the executable to check serialization at runtime.
//
// The app doesn't need to actually run against a vstest.console instance; it
// deserializes representative wire payloads directly.

using System;
using System.Collections.Generic;
using System.Linq;

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

// Use wire JSON rather than constructing the traits locally, so the linker cannot
// accidentally preserve types that are only instantiated by deserialization.
const string discoveredTestsJson = """
    [{
        "Id": "38b22f0b-0e57-41ff-4460-496b3c84a3f9",
        "FullyQualifiedName": "Namespace.Class.Method",
        "DisplayName": "Namespace.Class.Method",
        "ExecutorUri": "executor://xunit/VsTestRunner3/netcore/",
        "Source": "test.dll",
        "Properties": [{
            "Key": {
                "Id": "TestObject.Traits",
                "Label": "Traits",
                "ValueType": "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]",
                "Attributes": 0
            },
            "Value": [
                {"Key": "Category", "Value": "First"},
                {"Key": "Category", "Value": "Second"},
                {"Key": "Escaped", "Value": "quote: \" newline: \n unicode: \u263a"}
            ]
        }]
    }]
    """;

foreach (int version in new[] { 2, 7 })
{
    var foundMessage = JsonDataSerializer.Instance.DeserializeMessage($$"""
        {"Version":{{version}},"MessageType":"{{MessageType.TestCasesFound}}","Payload":{{discoveredTestsJson}}}
        """);
    var foundTests = JsonDataSerializer.Instance.DeserializePayload<IEnumerable<TestCase>>(foundMessage)
        ?? throw new InvalidOperationException("TestCasesFound deserialization returned null.");
    VerifyDiscoveredTraits(foundTests.Single());

    var completeMessage = JsonDataSerializer.Instance.DeserializeMessage($$"""
        {
            "Version": {{version}},
            "MessageType": "{{MessageType.DiscoveryComplete}}",
            "Payload": {
                "TotalTests": 1,
                "IsAborted": false,
                "LastDiscoveredTests": {{discoveredTestsJson}}
            }
        }
        """);
    var completePayload = JsonDataSerializer.Instance.DeserializePayload<DiscoveryCompletePayload>(completeMessage)
        ?? throw new InvalidOperationException("DiscoveryComplete deserialization returned null.");
    if (completePayload.TotalTests != 1 || completePayload.IsAborted)
    {
        throw new InvalidOperationException("DiscoveryComplete did not preserve the discovery status.");
    }

    var lastDiscoveredTests = completePayload.LastDiscoveredTests
        ?? throw new InvalidOperationException("DiscoveryComplete did not preserve the last batch of tests.");
    VerifyDiscoveredTraits(lastDiscoveredTests.Single());
}

Console.WriteLine("NativeAOT discovery traits deserialization succeeded.");

static void VerifyDiscoveredTraits(TestCase discoveredTest)
{
    var traits = discoveredTest.Traits.ToArray();
    if (discoveredTest.FullyQualifiedName != "Namespace.Class.Method"
        || traits.Length != 3
        || traits[0].Name != "Category" || traits[0].Value != "First"
        || traits[1].Name != "Category" || traits[1].Value != "Second"
        || traits[2].Name != "Escaped" || traits[2].Value != "quote: \" newline: \n unicode: \u263a")
    {
        throw new InvalidOperationException("Discovered test traits did not preserve duplicates, order, or escaped values.");
    }
}
