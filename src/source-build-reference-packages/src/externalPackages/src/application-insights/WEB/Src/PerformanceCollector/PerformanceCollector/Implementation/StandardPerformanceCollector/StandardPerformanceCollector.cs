namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal class StandardPerformanceCollector : IPerformanceCollector, IDisposable
    {
        private readonly List<Tuple<PerformanceCounterData, ICounterValue>> performanceCounters = new List<Tuple<PerformanceCounterData, ICounterValue>>();

        private IEnumerable<string> win32Instances;
        private IEnumerable<string> clrInstances;
        private bool dependendentInstancesLoaded = false;

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
        public IEnumerable<Tuple<PerformanceCounterData, double>> Collect(Action<string, Exception> onReadingFailure = null)
        {
            return this.performanceCounters.Where(pc => !pc.Item1.IsInBadState).SelectMany(
                pc =>
                    {
                        double value;

                        try
                        {
                            value = pc.Item2.Collect();
                        }
                        catch (InvalidOperationException e)
                        {
                            onReadingFailure?.Invoke(PerformanceCounterUtility.FormatPerformanceCounter(pc.Item1.PerformanceCounter), e);
#if NETSTANDARD2_0
                            return Array.Empty<Tuple<PerformanceCounterData, double>>();
#else
                            return new Tuple<PerformanceCounterData, double>[] { };
#endif                        
                        }

                        return new[] { Tuple.Create(pc.Item1, value) };
                    });
        }

        /// <summary>
        /// Refreshes counters.
        /// </summary>
        public void RefreshCounters()
        {
            this.LoadDependentInstances();

            // We need to refresh counters in bad state and counters with placeholders in instance names
            var countersToRefresh =
                this.PerformanceCounters.Where(pc => pc.IsInBadState || pc.UsesInstanceNamePlaceholder)
                    .ToList();

            countersToRefresh.ForEach(this.RefreshCounter);

            PerformanceCollectorEventSource.Log.CountersRefreshedEvent(countersToRefresh.Count.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Registers a counter using the counter name and reportAs value to the total list of counters.
        /// </summary>
        /// <param name="perfCounterName">Name of the performance counter.</param>
        /// <param name="reportAs">Report as name for the performance counter.</param>
        /// <param name="error">Captures the error logged.</param>
        /// <param name="blockCounterWithInstancePlaceHolder">Boolean that controls the registry of the counter based on the availability of instance place holder.</param>
        public void RegisterCounter(
            string perfCounterName,
            string reportAs,            
            out string error,
            bool blockCounterWithInstancePlaceHolder = false)
        {
            bool usesInstanceNamePlaceholder;

            if (!this.dependendentInstancesLoaded)
            {
                this.LoadDependentInstances();
                this.dependendentInstancesLoaded = true;
            }

            var pc = PerformanceCounterUtility.CreateAndValidateCounter(
                perfCounterName,
                this.win32Instances,
                this.clrInstances,
                true,
                out usesInstanceNamePlaceholder,
                out error);

            // If blockCounterWithInstancePlaceHolder is true, then we register the counter only if usesInstanceNamePlaceHolder is true.
            if (pc != null && !(blockCounterWithInstancePlaceHolder && usesInstanceNamePlaceholder))
            {
                this.RegisterCounter(perfCounterName, reportAs, pc, usesInstanceNamePlaceholder, out error);
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
        /// Collects a value for a single counter.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Rebinds performance counters to Windows resources.
        /// </summary>
        private void RefreshPerformanceCounter(PerformanceCounterData pcd)
        {
            Tuple<PerformanceCounterData, ICounterValue> tupleToRemove = this.performanceCounters.FirstOrDefault(t => t.Item1 == pcd);
            if (tupleToRemove != null)
            {
                this.performanceCounters.Remove(tupleToRemove);
            }

            this.RegisterPerformanceCounter(
                pcd.OriginalString,
                pcd.ReportAs,
                pcd.PerformanceCounter.CategoryName,
                pcd.PerformanceCounter.CounterName,
                pcd.PerformanceCounter.InstanceName,
                pcd.UsesInstanceNamePlaceholder);
        }

        /// <summary>
        /// Loads instances that are used in performance counter computation.
        /// </summary>
        private void LoadDependentInstances()
        {
            this.win32Instances = PerformanceCounterUtility.GetWin32ProcessInstances();
            this.clrInstances = PerformanceCounterUtility.GetClrProcessInstances();
        }

        /// <summary>
        /// Refreshes the counter associated with a specific performance counter data.
        /// </summary>
        /// <param name="pcd">Target performance counter data to refresh.</param>
        private void RefreshCounter(PerformanceCounterData pcd)
        {
            string dummy;

            bool usesInstanceNamePlaceholder;
            var pc = PerformanceCounterUtility.CreateAndValidateCounter(
                pcd.OriginalString,
                this.win32Instances,
                this.clrInstances,
                true,
                out usesInstanceNamePlaceholder,
                out dummy);

            try
            {
                this.RefreshPerformanceCounter(pcd);

                PerformanceCollectorEventSource.Log.CounterRegisteredEvent(
                        PerformanceCounterUtility.FormatPerformanceCounter(pc));
            }
            catch (InvalidOperationException e)
            {
                PerformanceCollectorEventSource.Log.CounterRegistrationFailedEvent(
                    e.Message,
                    PerformanceCounterUtility.FormatPerformanceCounter(pc));
            }
        }

        /// <summary>
        /// Registers the counter to the existing list of counters.
        /// </summary>
        /// <param name="originalString">Counter original string.</param>
        /// <param name="reportAs">Counter report as.</param>
        /// <param name="pc">Performance counter.</param>
        /// <param name="usesInstanceNamePlaceholder">Uses Instance Name Place holder boolean.</param>
        /// <param name="error">Error message.</param>
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

                PerformanceCollectorEventSource.Log.CounterRegisteredEvent(PerformanceCounterUtility.FormatPerformanceCounter(pc));
            }
            catch (InvalidOperationException e)
            {
                PerformanceCollectorEventSource.Log.CounterRegistrationFailedEvent(e.Message, PerformanceCounterUtility.FormatPerformanceCounter(pc));
                error = e.Message;
            }
        }

        /// <summary>
        /// Register a performance counter for collection.
        /// </summary>
        /// <param name="originalString">Original string definition of the counter.</param>
        /// <param name="reportAs">Alias to report the counter as.</param>
        /// <param name="categoryName">Category name.</param>
        /// <param name="counterName">Counter name.</param>
        /// <param name="instanceName">Instance name.</param>
        /// <param name="usesInstanceNamePlaceholder">Indicates whether the counter uses a placeholder in the instance name.</param>
        private void RegisterPerformanceCounter(string originalString, string reportAs, string categoryName, string counterName, string instanceName, bool usesInstanceNamePlaceholder)
        {
            ICounterValue performanceCounter = null;

            try
            {
                performanceCounter = CounterFactory.GetCounter(originalString, categoryName, counterName, instanceName);
            }
            catch (Exception e)
            {
                // we want to have another crack at it if instance placeholder is used,
                // notably due to the fact that CLR process ID counter only starts returning values after the first garbage collection
                if (!usesInstanceNamePlaceholder)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "Failed to register performance counter. Category: {0}, counter: {1}, instance: {2}.",
                            categoryName,
                            counterName,
                            instanceName),
                        e);
                }
            }            

            bool firstReadOk = false;
            try
            {
                // perform the first read. For many counters the first read will always return 0
                // since a single sample is not enough to calculate a value
                performanceCounter.Collect();

                firstReadOk = true;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to perform the first read for performance counter. Please make sure it exists. Category: {0}, counter: {1}, instance {2}",
                        categoryName,
                        counterName,
                        instanceName),
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

                this.performanceCounters.Add(new Tuple<PerformanceCounterData, ICounterValue>(perfData, performanceCounter));
            }
        }

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.performanceCounters != null)
                {
                    foreach (var performanceCounter in this.performanceCounters)
                    {
                        if (performanceCounter.Item2 is IDisposable)
                        {
                            ((IDisposable)performanceCounter.Item2).Dispose();
                        }
                    }

                    this.performanceCounters.Clear();
                }
            }
        }
    }
}
