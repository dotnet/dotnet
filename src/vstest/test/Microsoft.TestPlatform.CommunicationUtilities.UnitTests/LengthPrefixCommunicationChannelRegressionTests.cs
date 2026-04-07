// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.UnitTests;

/// <summary>
/// Regression tests for LengthPrefixCommunicationChannel with TrackableEvent integration.
/// </summary>
[TestClass]
public class LengthPrefixCommunicationChannelRegressionTests
{
    // Regression test for #4553 — 17.6.x consumes lot of CPU
    // The communication channel now uses TrackableEvent for message notification
    // instead of regular events, enabling subscriber-aware waiting.

    [TestMethod]
    public void MessageReceived_ShouldBeTrackableEvent()
    {
        using var stream = new MemoryStream();
        using var channel = new LengthPrefixCommunicationChannel(stream);

        // MessageReceived should be a TrackableEvent
        Assert.IsNotNull(channel.MessageReceived);
    }

    [TestMethod]
    public void MessageReceived_Subscribe_ShouldMakeWaitForSubscriberReturnTrue()
    {
        using var stream = new MemoryStream();
        using var channel = new LengthPrefixCommunicationChannel(stream);
        using var cts = new CancellationTokenSource();

        channel.MessageReceived.Subscribe((sender, args) => { });

        bool hasSubscriber = channel.MessageReceived.WaitForSubscriber(100, cts.Token);
        Assert.IsTrue(hasSubscriber, "After subscribing, WaitForSubscriber should return true.");
    }

    [TestMethod]
    public void MessageReceived_NoSubscriber_WaitShouldTimeout()
    {
        using var stream = new MemoryStream();
        using var channel = new LengthPrefixCommunicationChannel(stream);
        using var cts = new CancellationTokenSource();

        bool hasSubscriber = channel.MessageReceived.WaitForSubscriber(50, cts.Token);
        Assert.IsFalse(hasSubscriber, "Without subscribers, WaitForSubscriber should timeout and return false.");
    }
}
