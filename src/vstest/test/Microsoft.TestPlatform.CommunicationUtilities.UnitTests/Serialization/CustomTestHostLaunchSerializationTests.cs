// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization.SerializationTestHelpers;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests.Serialization;

/// <summary>
/// Wire-format tests for <see cref="MessageType.CustomTestHostLaunch"/> ("TestExecution.CustomTestHostLaunch").
///
/// This message is sent to request the IDE to launch a custom test host process.
/// The payload is <see cref="TestProcessStartInfo"/> which contains process start information.
///
/// Payload is identical for V1 and V7 because no TestCase/TestResult objects are involved.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class CustomTestHostLaunchSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TestProcessStartInfo Payload = new()
    {
        FileName = @"C:\Program Files\dotnet\dotnet.exe",
        Arguments = "exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 12345",
        WorkingDirectory = @"C:\src\Contoso.Math.Tests\bin\Debug\net8.0",
        EnvironmentVariables = new Dictionary<string, string?>
        {
            ["DOTNET_ROOT"] = @"C:\Program Files\dotnet",
            ["VSTEST_CONNECTION_TIMEOUT"] = "90"
        },
        CustomProperties = new Dictionary<string, string>
        {
            ["IsBeingDebugged"] = "false"
        }
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.CustomTestHostLaunch",
          "Payload": {
            "FileName": "C:\\Program Files\\dotnet\\dotnet.exe",
            "Arguments": "exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 12345",
            "WorkingDirectory": "C:\\src\\Contoso.Math.Tests\\bin\\Debug\\net8.0",
            "EnvironmentVariables": {
              "DOTNET_ROOT": "C:\\Program Files\\dotnet",
              "VSTEST_CONNECTION_TIMEOUT": "90"
            },
            "CustomProperties": {
              "IsBeingDebugged": "false"
            }
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.CustomTestHostLaunch",
          "Payload": {
            "FileName": "C:\\Program Files\\dotnet\\dotnet.exe",
            "Arguments": "exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 12345",
            "WorkingDirectory": "C:\\src\\Contoso.Math.Tests\\bin\\Debug\\net8.0",
            "EnvironmentVariables": {
              "DOTNET_ROOT": "C:\\Program Files\\dotnet",
              "VSTEST_CONNECTION_TIMEOUT": "90"
            },
            "CustomProperties": {
              "IsBeingDebugged": "false"
            }
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.CustomTestHostLaunch, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.CustomTestHostLaunch, Payload, version: 7);

        AssertJsonEqual(V7Json, json);
    }

    // ── Deserialize ──────────────────────────────────────────────────────

    [TestMethod]
    public void DeserializePayloadV1()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V1Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestProcessStartInfo>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(@"C:\Program Files\dotnet\dotnet.exe", result.FileName);
        Assert.AreEqual("exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 12345", result.Arguments);
        Assert.AreEqual(@"C:\src\Contoso.Math.Tests\bin\Debug\net8.0", result.WorkingDirectory);
        Assert.IsNotNull(result.EnvironmentVariables);
        Assert.AreEqual(@"C:\Program Files\dotnet", result.EnvironmentVariables["DOTNET_ROOT"]);
        Assert.AreEqual("90", result.EnvironmentVariables["VSTEST_CONNECTION_TIMEOUT"]);
        Assert.IsNotNull(result.CustomProperties);
        Assert.AreEqual("false", result.CustomProperties["IsBeingDebugged"]);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestProcessStartInfo>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(@"C:\Program Files\dotnet\dotnet.exe", result.FileName);
        Assert.AreEqual("exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 12345", result.Arguments);
        Assert.AreEqual(@"C:\src\Contoso.Math.Tests\bin\Debug\net8.0", result.WorkingDirectory);
        Assert.IsNotNull(result.EnvironmentVariables);
        Assert.AreEqual(@"C:\Program Files\dotnet", result.EnvironmentVariables["DOTNET_ROOT"]);
        Assert.AreEqual("90", result.EnvironmentVariables["VSTEST_CONNECTION_TIMEOUT"]);
        Assert.IsNotNull(result.CustomProperties);
        Assert.AreEqual("false", result.CustomProperties["IsBeingDebugged"]);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.CustomTestHostLaunch, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestProcessStartInfo>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.FileName, result.FileName);
        Assert.AreEqual(Payload.Arguments, result.Arguments);
        Assert.AreEqual(Payload.WorkingDirectory, result.WorkingDirectory);
        Assert.IsNotNull(result.EnvironmentVariables);
        Assert.AreEqual(Payload.EnvironmentVariables!["DOTNET_ROOT"], result.EnvironmentVariables["DOTNET_ROOT"]);
        Assert.AreEqual(Payload.EnvironmentVariables["VSTEST_CONNECTION_TIMEOUT"], result.EnvironmentVariables["VSTEST_CONNECTION_TIMEOUT"]);
        Assert.IsNotNull(result.CustomProperties);
        Assert.AreEqual(Payload.CustomProperties!["IsBeingDebugged"], result.CustomProperties["IsBeingDebugged"]);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
