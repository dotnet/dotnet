// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// A utility to help us measure named intervals.
    /// To start a tracking call <see cref="Start"/> and to complete an interval, dispose the object returned.
    /// Overlapping internals are not supported.
    /// This utility is not thread safe.
    /// </summary>
    public class IntervalTracker
    {
        private readonly Stopwatch _intervalWatch = new Stopwatch();
        private readonly List<Tuple<string, TimeSpan>> _intervalList = new List<Tuple<string, TimeSpan>>();
        private readonly string _activityName;

        public IntervalTracker(string activityName)
        {
            _activityName = activityName;
        }

        public IDisposable Start(string intervalName)
        {
            _intervalWatch.Restart();
            return new Interval(this, intervalName);
        }

        public IEnumerable<(string, double)> GetIntervals()
        {
            foreach (var interval in _intervalList)
            {
                yield return (interval.Item1, interval.Item2.TotalSeconds);
            }
        }

        private class Interval : IDisposable
        {
            private readonly IntervalTracker _intervalTracker;
            private readonly string _intervalName;
            private readonly EtwLogActivity _etwLogActivity;

            internal Interval(IntervalTracker intervalTracker, string intervalName)
            {
                _intervalTracker = intervalTracker;
                _intervalName = intervalName;
                _etwLogActivity = new EtwLogActivity($"{_intervalTracker._activityName}/{intervalName}");
            }

            void IDisposable.Dispose()
            {
                _intervalTracker._intervalWatch.Stop();
                _intervalTracker._intervalList.Add(new Tuple<string, TimeSpan>(_intervalName, _intervalTracker._intervalWatch.Elapsed));
                ((IDisposable)_etwLogActivity).Dispose();
            }
        }
    }
}
