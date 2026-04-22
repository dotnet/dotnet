// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.UnitTests;

/// <summary>
/// Regression tests for JSON serialization with discovery status information.
/// </summary>
[TestClass]
public class JsonDataSerializerDiscoveryStatusRegressionTests
{
    // Regression test for #3381 — Change serializer settings to not send empty values
    // DiscoveryCompletePayload should include source status lists when serialized.

    [TestMethod]
    public void SerializeDeserialize_DiscoveryCompletePayload_ShouldPreserveSourceStatus()
    {
        var serializer = JsonDataSerializer.Instance;

        var payload = new DiscoveryCompleteEventArgs(42, false)
        {
            FullyDiscoveredSources = new System.Collections.Generic.List<string> { "a.dll", "b.dll" },
            PartiallyDiscoveredSources = new System.Collections.Generic.List<string> { "c.dll" },
            NotDiscoveredSources = new System.Collections.Generic.List<string> { "d.dll" },
        };

        // Serialize
        string json = serializer.SerializePayload("TestDiscovery.Completed", payload);

        // Should contain the source status data
        Assert.Contains("a.dll", json);
        Assert.Contains("c.dll", json);
        Assert.Contains("d.dll", json);
    }

    [TestMethod]
    public void SerializePayload_NullSourceLists_ShouldSerializeSuccessfully()
    {
        var serializer = JsonDataSerializer.Instance;

        var payload = new DiscoveryCompleteEventArgs(10, false)
        {
            FullyDiscoveredSources = null,
            PartiallyDiscoveredSources = null,
            NotDiscoveredSources = null,
        };

        // Should not throw
        string json = serializer.SerializePayload("TestDiscovery.Completed", payload);
        Assert.IsNotNull(json);
    }
}
