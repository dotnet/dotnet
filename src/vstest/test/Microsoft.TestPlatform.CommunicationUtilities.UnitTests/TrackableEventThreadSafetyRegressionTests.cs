// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.UnitTests;

/// <summary>
/// Regression tests for TrackableEvent thread safety and synchronization.
/// </summary>
[TestClass]
public class TrackableEventThreadSafetyRegressionTests
{
    // Regression test for #4553 — 17.6.x consumes lot of CPU
    // TrackableEvent replaced polling-based event notification with ManualResetEventSlim.
    // This test verifies cross-thread subscribe-then-wait behavior.
    [TestMethod]
    public void WaitForSubscriber_SubscribeFromDifferentThread_ShouldSignal()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Subscribe from another thread
        var subscribeThread = new Thread(() =>
        {
            Thread.Sleep(50);
            trackableEvent.Subscribe((sender, args) => { });
        });
        subscribeThread.Start();

        bool result = trackableEvent.WaitForSubscriber(3000, cts.Token);

        Assert.IsTrue(result, "WaitForSubscriber should return true when subscription happens from another thread.");
        subscribeThread.Join();
    }

    // Regression test for #4553
    [TestMethod]
    public void MultipleSubscribers_AllShouldBeNotified()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        int callCount = 0;

        trackableEvent.Subscribe((sender, args) => Interlocked.Increment(ref callCount));
        trackableEvent.Subscribe((sender, args) => Interlocked.Increment(ref callCount));

        var args = new MessageReceivedEventArgs { Data = "test" };
        trackableEvent.Notify(this, args, "MultiNotify");

        Assert.AreEqual(2, callCount, "Both subscribers should be notified.");
    }

    // Regression test for #4553
    [TestMethod]
    public void UnsubscribeOne_OtherShouldStillWork()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        int callCount = 0;
        using var cts = new CancellationTokenSource();

        EventHandler<MessageReceivedEventArgs> handler1 = (sender, args) => Interlocked.Increment(ref callCount);
        EventHandler<MessageReceivedEventArgs> handler2 = (sender, args) => Interlocked.Increment(ref callCount);

        trackableEvent.Subscribe(handler1);
        trackableEvent.Subscribe(handler2);
        trackableEvent.Unsubscribe(handler1);

        // WaitForSubscriber should still return true because handler2 is still subscribed
        bool hasSubscriber = trackableEvent.WaitForSubscriber(100, cts.Token);
        Assert.IsTrue(hasSubscriber);

        trackableEvent.Notify(this, new MessageReceivedEventArgs { Data = "test" }, "Partial");
        Assert.AreEqual(1, callCount, "Only handler2 should be called.");
    }
}
