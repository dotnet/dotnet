// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.Build.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Build.UnitTests;

/// <summary>
/// Regression tests for VSTestTask2 duration formatting.
/// </summary>
[TestClass]
public class VSTestTask2DurationFormattingRegressionTests
{
    // Regression test for #4894 — Time is reported incorrectly for xunit

    [TestMethod]
    public void GetFormattedDurationString_ExactlyOneMinute_ShouldFormat()
    {
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromMinutes(1));
        Assert.AreEqual("1m", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_MinutesAndSeconds_ShouldNotIncludeMilliseconds()
    {
        var duration = TimeSpan.FromMinutes(1) + TimeSpan.FromSeconds(30) + TimeSpan.FromMilliseconds(500);
        string? result = VSTestTask2.GetFormattedDurationString(duration);

        // When minutes > 0, milliseconds should not be included
        Assert.AreEqual("1m 30s", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_HoursMinutesSeconds_ShouldNotIncludeSeconds()
    {
        var duration = TimeSpan.FromHours(2) + TimeSpan.FromMinutes(30) + TimeSpan.FromSeconds(45);
        string? result = VSTestTask2.GetFormattedDurationString(duration);

        // When hours > 0, seconds should not be included
        Assert.AreEqual("2h 30m", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_OnlyMilliseconds_ShouldFormat()
    {
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromMilliseconds(42));
        Assert.AreEqual("42ms", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_ExactlyOneSecond_ShouldFormat()
    {
        string? result = VSTestTask2.GetFormattedDurationString(TimeSpan.FromSeconds(1));
        Assert.AreEqual("1s", result);
    }

    [TestMethod]
    public void GetFormattedDurationString_FractionalMilliseconds_ShouldRoundDown()
    {
        // TimeSpan of 0.5ms
        var duration = TimeSpan.FromTicks(5000); // 0.5ms
        string? result = VSTestTask2.GetFormattedDurationString(duration);
        // Milliseconds property rounds down, so 0ms = "< 1ms"
        Assert.AreEqual("< 1ms", result);
    }
}
