// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class SolutionRestoreWorkerTests
    {
        [Fact]
        public void CalculateTimeoutTime_WithTimeoutLargerThanTimeElapsed_ReturnsPositiveValue()
        {
            var startTime = new DateTime(year: 2021, month: 7, day: 21, hour: 10, minute: 5, second: 20);
            var currentTime = new DateTime(year: 2021, month: 7, day: 21, hour: 10, minute: 7, second: 00);
            TimeSpan timeoutSpan = new(hours: 0, minutes: 5, seconds: 0);

            var timeout = SolutionRestoreWorker.CalculateTimeoutTime(startTime: startTime, currentTime: currentTime, timeoutTime: timeoutSpan);
            timeout.TotalMilliseconds.Should().Be(200000);
        }

        [Fact]
        public void CalculateTimeoutTime_WithTimeElapsedLargerThanTimeout_Returns0()
        {
            var startTime = new DateTime(year: 2021, month: 7, day: 21, hour: 10, minute: 5, second: 20);
            var currentTime = new DateTime(year: 2021, month: 7, day: 21, hour: 11, minute: 0, second: 00);
            TimeSpan timeoutSpan = new(hours: 0, minutes: 5, seconds: 0);

            var timeout = SolutionRestoreWorker.CalculateTimeoutTime(startTime: startTime, currentTime: currentTime, timeoutTime: timeoutSpan);
            timeout.TotalMilliseconds.Should().Be(0);
        }
    }
}
