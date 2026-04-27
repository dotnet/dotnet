// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.UnitTests;

/// <summary>
/// Regression tests for JsonDataSerializer payload serialization.
/// </summary>
[TestClass]
public class JsonDataSerializerPayloadRegressionTests
{
    // Regression test for #3381 — Change serializer settings to not send empty values
    [TestMethod]
    public void SerializePayload_WithMessageType_ShouldContainMessageType()
    {
        var serializer = JsonDataSerializer.Instance;

        string json = serializer.SerializePayload("TestMessage", "TestData");

        Assert.Contains("TestMessage", json);
        Assert.Contains("TestData", json);
    }

    [TestMethod]
    public void SerializePayload_WithComplexObject_ShouldSerialize()
    {
        var serializer = JsonDataSerializer.Instance;

        var testCase = new TestCase("Ns.Class.Method", new Uri("executor://test"), "test.dll");
        string json = serializer.SerializePayload("TestDiscovery.TestFound", testCase);

        Assert.Contains("Ns.Class.Method", json);
    }

    [TestMethod]
    public void DeserializeMessage_ShouldReturnCorrectMessageType()
    {
        var serializer = JsonDataSerializer.Instance;

        var rawMessage = serializer.SerializePayload("TestExecution.Started", "payload");
        var message = serializer.DeserializeMessage(rawMessage);

        Assert.AreEqual("TestExecution.Started", message.MessageType);
    }

    [TestMethod]
    public void SerializePayload_WithProtocolVersion_ShouldSucceed()
    {
        var serializer = JsonDataSerializer.Instance;

        // Version 1 serialization
        string json = serializer.SerializePayload("TestMsg", "Data", version: 1);
        Assert.IsNotNull(json);
    }
}
