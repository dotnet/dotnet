namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights.Common;

    internal class WebAppPerformanceCollector : IPerformanceCollector
    {
        private readonly List<Tuple<PerformanceCounterData, ICounterValue>> performanceCounters = new List<Tuple<PerformanceCounterData, ICounterValue>>();

        /// <summary>
        /// Gets a collection of registered performance counters.
        /// </summary>
        public IEnumerable<PerformanceCounterData> PerformanceCounters
        {
            get { return this.performanceCounters.Select(t => t.Item1).ToList(); }
        }

        /// <summary>
        /// Performs collection for all registered counters.
        /// </summary>
        /// <param name="onReadingFailure">Invoked when an individual counter fails to be read.</param>
        public IEnumerable<Tuple<PerformanceCounterData, double>> Collect(
            Action<string, Exception> onReadingFailure = null)
        {
            return this.performanceCounters.Where(pc => !pc.Item1.IsInBadState).SelectMany(
                counter =>
                    {
                        double value;

                        try
                        {
                            value = CollectCounter(counter.Item1.OriginalString, counter.Item2);
                        }
                        catch (InvalidOperationException e)
                        {
                            onReadingFailure?.Invoke(counter.Item1.OriginalString, e);

                            return ArrayExtensions.Empty<Tuple<PerformanceCounterData, double>>();
                        }

                        return new[] { Tuple.Create(counter.Item1, value) };
                    });
        }

        /// <summary>
        /// Refreshes counters.
        /// </summary>
        public void RefreshCounters()
        {
            var countersToRefresh =
                this.PerformanceCounters.Where(pc => pc.IsInBadState)
                    .ToList();

            countersToRefresh.ForEach(this.RefreshPerformanceCounter);

            PerformanceCollectorEventSource.Log.CountersRefreshedEvent(countersToRefresh.Count.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Registers a counter using the counter name and reportAs value to the total list of counters.
        /// </summary>
        /// <param name="perfCounter">Name of the performance counter.</param>
        /// <param name="reportAs">Report as name for the performance counter.</param>
        /// <param name="error">Captures the error logged.</param>
        /// <param name="blockCounterWithInstancePlaceHolder">Boolean that controls the registry of the counter based on the availability of instance place holder.</param>
        public void RegisterCounter(
            string perfCounter,
            string reportAs,
            out string error,
            bool blockCounterWithInstancePlaceHolder)
        {            
            try
            {
                bool useInstancePlaceHolder = false;                
                var pc = PerformanceCounterUtility.CreateAndValidateCounter(perfCounter, null, null, false, out useInstancePlaceHolder, out error);                

                if (pc != null)
                {
                    this.RegisterPerformanceCounter(perfCounter, GetCounterReportAsName(perfCounter, reportAs), pc.CategoryName, pc.CounterName, pc.InstanceName, useInstancePlaceHolder);
                }
                else
                {
                    // Even if validation failed, we might still be able to collect perf counter in WebApp.
                    this.RegisterPerformanceCounter(perfCounter, GetCounterReportAsName(perfCounter, reportAs), string.Empty, perfCounter, string.Empty, useInstancePlaceHolder);
                }                
            }
            catch (Exception e)
            {
                PerformanceCollectorEventSource.Log.WebAppCounterRegistrationFailedEvent(
                    e.Message,
                    perfCounter);
                error = e.Message;
            }
        }

        /// <summary>
        /// Removes a counter.
        /// </summary>
        /// <param name="perfCounter">Name of the performance counter to remove.</param>
        /// <param name="reportAs">ReportAs value of the performance counter to remove.</param>
        public void RemoveCounter(string perfCounter, string reportAs)
        {
            Tuple<PerformanceCounterData, ICounterValue> keyToRemove =
                this.performanceCounters.FirstOrDefault(
                    pair =>
                    string.Equals(pair.Item1.ReportAs, reportAs, StringComparison.Ordinal)
                    && string.Equals(pair.Item1.OriginalString, perfCounter, StringComparison.OrdinalIgnoreCase));

            if (keyToRemove != null)
            {
                this.performanceCounters.Remove(keyToRemove);
            }
        }

        /// <summary>
        /// Rebinds performance counters to Windows resources.
        /// </summary>
        public void RefreshPerformanceCounter(PerformanceCounterData pcd)
        {
            Tuple<PerformanceCounterData, ICounterValue> tupleToRemove = this.performanceCounters.FirstOrDefault(t => t.Item1 == pcd);
            if (tupleToRemove != null)
            {
                this.performanceCounters.Remove(tupleToRemove);
            }

            try
            {
                this.RegisterPerformanceCounter(
                    pcd.OriginalString,
                    pcd.ReportAs,
                    pcd.PerformanceCounter.CategoryName,
                    pcd.PerformanceCounter.CounterName,
                    pcd.PerformanceCounter.InstanceName,
                    pcd.UsesInstanceNamePlaceholder);
            }
            catch (InvalidOperationException e)
            {
                PerformanceCollectorEventSource.Log.CounterRegistrationFailedEvent(
                    e.Message,
                    PerformanceCounterUtility.FormatPerformanceCounter(pcd.PerformanceCounter));
            }
        }

        /// <summary>
        /// Collects a value for a single counter.
        /// </summary>
        private static double CollectCounter(string coutnerOriginalString, ICounterValue counter)
        {
            try
            {
                return counter.Collect();
            }
            catch (Exception e)
            {
                 throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to perform a read for web app performance counter {0}",
                        coutnerOriginalString),
                    e);
            }
        }

        /// <summary>
        /// Gets metric alias to be the value given by the user.
        /// </summary>
        /// <param name="counterName">Name of the counter to retrieve.</param>
        /// <param name="reportAs">Alias to report the counter.</param>
        /// <returns>Alias that will be used for the counter.</returns>
        private static string GetCounterReportAsName(string counterName, string reportAs)
        {
            return reportAs ?? counterName;
        }

        /// <summary>
        /// Register a performance counter for collection.
        /// </summary>
        private void RegisterPerformanceCounter(string originalString, string reportAs, string categoryName, string counterName, string instanceName, bool usesInstanceNamePlaceholder)
        {
            ICounterValue counter = null;

            try
            {
                counter = CounterFactory.GetCounter(originalString, reportAs);
            }
            catch
            {
                PerformanceCollectorEventSource.Log.CounterNotWebAppSupported(originalString);
                return;
            }

            bool firstReadOk = false;

            try
            {
                // perform the first read. For many counters the first read will always return 0
                // since a single sample is not enough to calculate a value
                var value = counter.Collect();
                firstReadOk = true;
            } 
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to perform the first read for web app performance counter. Please make sure it exists. Counter: {0}",
                        counterName),
                    e);
            }
            finally
            {
                PerformanceCounterData perfData = new PerformanceCounterData(
                        originalString,
                        reportAs,
                        usesInstanceNamePlaceholder,
                        !firstReadOk,
                        categoryName,
                        counterName,
                        instanceName);

                this.performanceCounters.Add(new Tuple<PerformanceCounterData, ICounterValue>(perfData, counter));
            }
        }
    }
}
