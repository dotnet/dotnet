namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;

    using CounterData = System.Tuple<Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.PerformanceCounterData, System.Collections.Generic.List<double>>;

    /// <summary>
    /// Mock to test clients of PerformanceCollector.
    /// </summary>
    internal class PerformanceCollectorMock : IPerformanceCollector
    {
        public object Sync = new object();

        private readonly List<CounterData> counters = new List<CounterData>();

        public List<CounterData> Counters
        {
            get
            {
                lock (this.Sync)
                {
                    return this.counters;
                }
            }
        }

        public IEnumerable<PerformanceCounterData> PerformanceCounters
        {
            get
            {
                lock (this.Sync)
                {
                    return this.counters.Select(c => c.Item1);
                }
            }
        }

        public IEnumerable<Tuple<PerformanceCounterData, double>> Collect(Action<string, Exception> onReadingFailure)
        {
            lock (this.Sync)
            {
                foreach (var counter in this.counters)
                {
                    var value = (double)counter.Item1.OriginalString.GetHashCode();

                    var result =
                        Tuple.Create(
                            new PerformanceCounterData(
                                counter.Item1.OriginalString,
                                counter.Item1.ReportAs,
                                counter.Item1.UsesInstanceNamePlaceholder,
                                counter.Item1.IsInBadState,
                                counter.Item1.PerformanceCounter.CategoryName,
                                counter.Item1.PerformanceCounter.CounterName,
                                counter.Item1.PerformanceCounter.InstanceName),
                            value);

                    counter.Item2.Add(value);

                    yield return result;
                }
            }
        }

        public void RefreshPerformanceCounter(PerformanceCounterData pcd)
        {
        }

        public void RefreshCounters()
        {
        }

        public void RegisterCounter(
            string perfCounterName,
            string reportAs,
            out string error,
            bool blockCounterWithInstancePlaceHolder)
        {
            bool usesInstanceNamePlaceholder;
            var pc = this.CreateCounter(
                perfCounterName,
                false,
                out usesInstanceNamePlaceholder,
                out error);

            if (pc != null)
            {
                this.RegisterCounter(perfCounterName, reportAs, pc, usesInstanceNamePlaceholder, out error);
            }
        }

        public void RemoveCounter(string perfCounter, string reportAs)
        {
            this.counters.RemoveAll(
                counter =>
                string.Equals(counter.Item1.ReportAs, reportAs, StringComparison.Ordinal)
                && string.Equals(counter.Item1.OriginalString, perfCounter, StringComparison.OrdinalIgnoreCase));
        }

        public PerformanceCounterStructure CreateCounter(
            string perfCounterName,
            bool supportInstanceNames,
            out bool usesInstanceNamePlaceholder,
            out string error)
        {
            error = null;

            try
            {
                return PerformanceCounterUtility.ParsePerformanceCounter(
                    perfCounterName,
                    new string[] { },
                    new string[] { },
                    supportInstanceNames,
                    out usesInstanceNamePlaceholder);
            }
            catch (Exception e)
            {
                usesInstanceNamePlaceholder = false;
                PerformanceCollectorEventSource.Log.CounterParsingFailedEvent(e.Message, perfCounterName);
                error = e.Message;

                return null;
            }
        }

        private void RegisterCounter(
            string originalString,
            string reportAs,
            PerformanceCounterStructure pc,
            bool usesInstanceNamePlaceholder,
            out string error)
        {
            error = null;

            try
            {
                this.RegisterPerformanceCounter(
                    originalString,
                    reportAs,
                    pc.CategoryName,
                    pc.CounterName,
                    pc.InstanceName,
                    usesInstanceNamePlaceholder);

                PerformanceCollectorEventSource.Log.CounterRegisteredEvent(
                    PerformanceCounterUtility.FormatPerformanceCounter(pc));
            }
            catch (InvalidOperationException e)
            {
                PerformanceCollectorEventSource.Log.CounterRegistrationFailedEvent(
                    e.Message,
                    PerformanceCounterUtility.FormatPerformanceCounter(pc));
                error = e.Message;
            }
        }

        private void RegisterPerformanceCounter(
            string originalString,
            string reportAs,
            string categoryName,
            string counterName,
            string instanceName,
            bool usesInstanceNamePlaceholder)
        {
            lock (this.Sync)
            {
                this.counters.Add(
                    Tuple.Create(
                        new PerformanceCounterData(
                            originalString,
                            reportAs,
                            usesInstanceNamePlaceholder,
                            false,
                            categoryName,
                            counterName,
                            instanceName),
                        new List<double>()));
            }
        }
    }
}
