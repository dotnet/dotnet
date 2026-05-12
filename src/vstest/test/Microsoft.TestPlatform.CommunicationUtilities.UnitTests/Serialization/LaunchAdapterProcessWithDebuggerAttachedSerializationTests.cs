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
/// Wire-format tests for <see cref="MessageType.LaunchAdapterProcessWithDebuggerAttached"/>
/// ("TestExecution.LaunchAdapterProcessWithDebuggerAttached").
///
/// This message requests the IDE to launch a test adapter process with a debugger attached.
/// Payload is identical for V1/V7.
/// The only difference is the outer envelope: V1 omits the Version field, V7 includes it.
/// </summary>
[TestClass]
[TestCategory("Serialization")]
public class LaunchAdapterProcessWithDebuggerAttachedSerializationTests
{
    // ── Example payload ──────────────────────────────────────────────────
    private static readonly TestProcessStartInfo Payload = new()
    {
        FileName = @"C:\Program Files\dotnet\dotnet.exe",
        Arguments = "exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 54321",
        WorkingDirectory = @"C:\src\Contoso.Math.Tests\bin\Debug\net8.0",
        EnvironmentVariables = new Dictionary<string, string?>
        {
            ["DOTNET_ROOT"] = @"C:\Program Files\dotnet",
            ["VSTEST_CONNECTION_TIMEOUT"] = "90"
        },
        CustomProperties = new Dictionary<string, string>
        {
            ["IsBeingDebugged"] = "true"
        }
    };

    // ── V1 wire format ───────────────────────────────────────────────────
    // Protocol versions 0, 1, 3 — no Version field in the envelope.
    private static readonly string V1Json = """
        {
          "MessageType": "TestExecution.LaunchAdapterProcessWithDebuggerAttached",
          "Payload": {
            "FileName": "C:\\Program Files\\dotnet\\dotnet.exe",
            "Arguments": "exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 54321",
            "WorkingDirectory": "C:\\src\\Contoso.Math.Tests\\bin\\Debug\\net8.0",
            "EnvironmentVariables": {
              "DOTNET_ROOT": "C:\\Program Files\\dotnet",
              "VSTEST_CONNECTION_TIMEOUT": "90"
            },
            "CustomProperties": {
              "IsBeingDebugged": "true"
            }
          }
        }
        """;

    // ── V7 wire format ───────────────────────────────────────────────────
    // Protocol versions 2, 4, 5, 6, 7 — includes Version in the envelope.
    private static readonly string V7Json = """
        {
          "Version": 7,
          "MessageType": "TestExecution.LaunchAdapterProcessWithDebuggerAttached",
          "Payload": {
            "FileName": "C:\\Program Files\\dotnet\\dotnet.exe",
            "Arguments": "exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 54321",
            "WorkingDirectory": "C:\\src\\Contoso.Math.Tests\\bin\\Debug\\net8.0",
            "EnvironmentVariables": {
              "DOTNET_ROOT": "C:\\Program Files\\dotnet",
              "VSTEST_CONNECTION_TIMEOUT": "90"
            },
            "CustomProperties": {
              "IsBeingDebugged": "true"
            }
          }
        }
        """;

    // ── Serialize ────────────────────────────────────────────────────────

    [TestMethod]
    public void SerializePayloadV1()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.LaunchAdapterProcessWithDebuggerAttached, Payload, version: 1);

        AssertJsonEqual(V1Json, json);
    }

    [TestMethod]
    public void SerializePayloadV7()
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.LaunchAdapterProcessWithDebuggerAttached, Payload, version: 7);

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
        Assert.AreEqual("exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 54321", result.Arguments);
        Assert.AreEqual(@"C:\src\Contoso.Math.Tests\bin\Debug\net8.0", result.WorkingDirectory);
        Assert.IsNotNull(result.EnvironmentVariables);
        Assert.AreEqual(@"C:\Program Files\dotnet", result.EnvironmentVariables["DOTNET_ROOT"]);
        Assert.AreEqual("90", result.EnvironmentVariables["VSTEST_CONNECTION_TIMEOUT"]);
        Assert.IsNotNull(result.CustomProperties);
        Assert.AreEqual("true", result.CustomProperties["IsBeingDebugged"]);
    }

    [TestMethod]
    public void DeserializePayloadV7()
    {
        var message = JsonDataSerializer.Instance.DeserializeMessage(Minify(V7Json));
        var result = JsonDataSerializer.Instance.DeserializePayload<TestProcessStartInfo>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(@"C:\Program Files\dotnet\dotnet.exe", result.FileName);
        Assert.AreEqual("exec --runtimeconfig testhost.runtimeconfig.json testhost.dll --port 54321", result.Arguments);
        Assert.AreEqual(@"C:\src\Contoso.Math.Tests\bin\Debug\net8.0", result.WorkingDirectory);
        Assert.IsNotNull(result.EnvironmentVariables);
        Assert.AreEqual(@"C:\Program Files\dotnet", result.EnvironmentVariables["DOTNET_ROOT"]);
        Assert.AreEqual("90", result.EnvironmentVariables["VSTEST_CONNECTION_TIMEOUT"]);
        Assert.IsNotNull(result.CustomProperties);
        Assert.AreEqual("true", result.CustomProperties["IsBeingDebugged"]);
    }

    // ── Round-trip ───────────────────────────────────────────────────────

    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void RoundTrip(int version)
    {
        var json = JsonDataSerializer.Instance.SerializePayload(
            MessageType.LaunchAdapterProcessWithDebuggerAttached, Payload, version);
        var message = JsonDataSerializer.Instance.DeserializeMessage(json);
        var result = JsonDataSerializer.Instance.DeserializePayload<TestProcessStartInfo>(message);

        Assert.IsNotNull(result);
        Assert.AreEqual(Payload.FileName, result.FileName);
        Assert.AreEqual(Payload.Arguments, result.Arguments);
        Assert.AreEqual(Payload.WorkingDirectory, result.WorkingDirectory);
        Assert.IsNotNull(result.EnvironmentVariables);
        Assert.IsTrue(Payload.EnvironmentVariables!.All(
            kvp => result.EnvironmentVariables[kvp.Key] == kvp.Value));
        Assert.IsNotNull(result.CustomProperties);
        Assert.IsTrue(Payload.CustomProperties!.All(
            kvp => result.CustomProperties[kvp.Key] == kvp.Value));
    }

    // ── Helpers ──────────────────────────────────────────────────────────

}
