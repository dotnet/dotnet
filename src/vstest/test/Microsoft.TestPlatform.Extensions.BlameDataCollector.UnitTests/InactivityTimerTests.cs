// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector.UnitTests;

[TestClass]
public class InactivityTimerTests
{
    private int _callBackCount;
    private readonly ManualResetEventSlim _timerEvent = new();

    [TestMethod]
    public void InactivityTimerShouldResetAndCallbackWhenResetIsCalled()
    {
        var timer = new InactivityTimer(TimerCallback);
        timer.ResetTimer(TimeSpan.FromMilliseconds(1));
        _timerEvent.Wait(1000);
        Assert.AreEqual(1, _callBackCount, "Should have fired once.");
    }

    private void TimerCallback()
    {
        _callBackCount++;
        _timerEvent.Set();
    }
}
