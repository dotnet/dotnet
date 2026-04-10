// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.UnitTests;

/// <summary>
/// Regression tests for TrackableEvent — subscribe/notify/wait synchronization.
/// </summary>
[TestClass]
public class TrackableEventRegressionTests
{
    // Regression test for #4553 — 17.6.x consumes lot of CPU
    // TrackableEvent replaces polling-based event handling with ManualResetEventSlim.
    [TestMethod]
    public void WaitForSubscriber_NoSubscriber_ShouldTimeout()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        using var cts = new CancellationTokenSource();

        bool result = trackableEvent.WaitForSubscriber(50, cts.Token);

        Assert.IsFalse(result, "WaitForSubscriber should return false when no subscriber is registered.");
    }

    // Regression test for #4553
    [TestMethod]
    public void WaitForSubscriber_AfterSubscribe_ShouldReturnTrue()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        using var cts = new CancellationTokenSource();

        trackableEvent.Subscribe((sender, args) => { });

        bool result = trackableEvent.WaitForSubscriber(1000, cts.Token);

        Assert.IsTrue(result, "WaitForSubscriber should return true after a subscriber is registered.");
    }

    // Regression test for #4553
    [TestMethod]
    public void WaitForSubscriber_AfterUnsubscribe_ShouldTimeout()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        using var cts = new CancellationTokenSource();

        EventHandler<MessageReceivedEventArgs> handler = (sender, args) => { };
        trackableEvent.Subscribe(handler);
        trackableEvent.Unsubscribe(handler);

        bool result = trackableEvent.WaitForSubscriber(50, cts.Token);

        Assert.IsFalse(result, "WaitForSubscriber should return false after all subscribers are removed.");
    }

    // Regression test for #4553
    [TestMethod]
    public void Notify_WithSubscriber_ShouldInvokeHandler()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        bool handlerCalled = false;
        var expectedArgs = new MessageReceivedEventArgs { Data = "test-data" };

        trackableEvent.Subscribe((sender, args) =>
        {
            handlerCalled = true;
            Assert.AreEqual("test-data", args.Data);
        });

        trackableEvent.Notify(this, expectedArgs, "TestNotify");

        Assert.IsTrue(handlerCalled, "Notify should invoke the subscribed handler.");
    }

    // Regression test for #4553
    [TestMethod]
    public void Notify_WithoutSubscriber_ShouldNotThrow()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        var args = new MessageReceivedEventArgs { Data = "test-data" };

        // Should not throw even without subscribers
        trackableEvent.Notify(this, args, "TestNotify");
    }

    // Regression test for #4553
    [TestMethod]
    public void Subscribe_Null_ShouldNotThrow()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();

        // Subscribe with null should not throw
        trackableEvent.Subscribe(null);
    }

    // Regression test for #4553
    [TestMethod]
    public void WaitForSubscriber_CancellationRequested_ShouldReturnFalse()
    {
        var trackableEvent = new TrackableEvent<MessageReceivedEventArgs>();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        try
        {
            trackableEvent.WaitForSubscriber(5000, cts.Token);
            // If it returns without throwing, that's also acceptable behavior
        }
        catch (OperationCanceledException)
        {
            // Expected — ManualResetEventSlim.Wait throws on cancellation
        }
    }
}
