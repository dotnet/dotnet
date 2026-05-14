// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class IntervalTrackerTests
    {
        [Fact]
        public void GetIntervals_When_no_intervals_are_captured_It_returns_empty()
        {
            var tracker = new IntervalTracker("Activity");
            Assert.Equal(0, tracker.GetIntervals().Count());
        }

        [Fact]
        public void GetIntervals_When_one_interval_is_not_disposed_It_returns_empty()
        {
            var tracker = new IntervalTracker("Activity");
            _ = tracker.Start("foo");
            Assert.Equal(0, tracker.GetIntervals().Count());
        }

        [Fact]
        public void IntervalTracker_All_properly_disposed_intervals_are_captured()
        {
            var tracker = new IntervalTracker("Activity");

            using (tracker.Start("first"))
            {
            }

            using (tracker.Start("second"))
            {
            }

            using (tracker.Start("third"))
            {
            }

            var allTimings = tracker.GetIntervals().ToList();

            Assert.Equal(3, allTimings.Count);
            Assert.Contains(allTimings, e => e.Item1.Equals("first"));
            Assert.Contains(allTimings, e => e.Item1.Equals("second"));
            Assert.Contains(allTimings, e => e.Item1.Equals("third"));
        }
    }
}
