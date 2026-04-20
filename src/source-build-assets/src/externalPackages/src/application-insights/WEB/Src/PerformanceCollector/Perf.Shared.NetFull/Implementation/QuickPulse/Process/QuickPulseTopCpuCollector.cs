namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;

    /// <summary>
    /// Top CPU collector.
    /// </summary>
    internal sealed class QuickPulseTopCpuCollector : IQuickPulseTopCpuCollector
    {
        // process name => (last observation timestamp, last observation value)
        internal readonly Dictionary<string, TimeSpan> ProcessObservations = new Dictionary<string, TimeSpan>(StringComparer.Ordinal);

        private readonly TimeSpan accessDeniedRetryInterval = TimeSpan.FromMinutes(1);

        private readonly Clock timeProvider;

        private readonly IQuickPulseProcessProvider processProvider;

        private DateTimeOffset prevObservationTime;

        private TimeSpan? prevOverallTime;

        private DateTimeOffset lastReadAttempt = DateTimeOffset.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseTopCpuCollector"/> class. 
        /// </summary>
        /// <param name="timeProvider">Time provider.</param>
        /// <param name="processProvider">Process provider.</param>
        public QuickPulseTopCpuCollector(Clock timeProvider, IQuickPulseProcessProvider processProvider)
        {
            this.timeProvider = timeProvider;
            this.processProvider = processProvider;

            this.InitializationFailed = false;
            this.AccessDenied = false;
        }

        /// <summary>
        /// Gets a value indicating whether the initialization has failed.
        /// </summary>
        public bool InitializationFailed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the Access Denied error has taken place.
        /// </summary>
        public bool AccessDenied { get; private set; }

        /// <summary>
        /// Gets top N processes by CPU consumption.
        /// </summary>
        /// <param name="topN">Top N processes.</param>
        /// <returns>List of top processes by CPU consumption.</returns>
        public IEnumerable<Tuple<string, int>> GetTopProcessesByCpu(int topN)
        {
            try
            {
                DateTimeOffset now = this.timeProvider.UtcNow;

                if (this.InitializationFailed)
                {
                    // the initialization has failed, so we never attempt to do anything
                    return Enumerable.Empty<Tuple<string, int>>();
                }

                if (this.AccessDenied && now - this.lastReadAttempt < this.accessDeniedRetryInterval)
                {
                    // not enough time has passed since we got denied access, so don't retry just yet
                    return Enumerable.Empty<Tuple<string, int>>();
                }

                var procData = new List<Tuple<string, double>>();
                var encounteredProcs = new HashSet<string>();
                
                this.lastReadAttempt = now;

                TimeSpan? totalTime;
                foreach (var process in this.processProvider.GetProcesses(out totalTime))
                {
                    encounteredProcs.Add(process.ProcessName);
                    
                    TimeSpan lastObservation;
                    if (!this.ProcessObservations.TryGetValue(process.ProcessName, out lastObservation))
                    {
                        // this is the first time we're encountering this process
                        this.ProcessObservations.Add(process.ProcessName, process.TotalProcessorTime);

                        continue;
                    }

                    TimeSpan cpuTimeSinceLast = process.TotalProcessorTime - lastObservation;

                    this.ProcessObservations[process.ProcessName] = process.TotalProcessorTime;

                    // use perf data if available; otherwise, calculate it ourselves
                    TimeSpan timeElapsedOnAllCoresSinceLast = (totalTime - this.prevOverallTime)
                                                              ?? TimeSpan.FromTicks((now - this.prevObservationTime).Ticks * Environment.ProcessorCount);

                    double usagePercentage = timeElapsedOnAllCoresSinceLast.Ticks > 0
                                                 ? (double)cpuTimeSinceLast.Ticks / timeElapsedOnAllCoresSinceLast.Ticks
                                                 : 0;

                    procData.Add(Tuple.Create(process.ProcessName, usagePercentage));
                }

                this.CleanState(encounteredProcs);

                this.prevObservationTime = now;
                this.prevOverallTime = totalTime;

                this.AccessDenied = false;

                // TODO: implement partial sort instead of full sort below
                return procData.OrderByDescending(p => p.Item2).Select(p => Tuple.Create(p.Item1, (int)(p.Item2 * 100))).Take(topN);
            }
            catch (Exception e)
            {
                QuickPulseEventSource.Log.ProcessesReadingFailedEvent(e.ToInvariantString());

                if (e is UnauthorizedAccessException || e is SecurityException)
                {
                    this.AccessDenied = true;
                }

                return Enumerable.Empty<Tuple<string, int>>();
            }
        }

        /// <summary>
        /// Initializes the top CPU collector.
        /// </summary>
        public void Initialize()
        {
            this.InitializationFailed = false;
            this.AccessDenied = false;
            
            try
            {
                this.processProvider.Initialize();
            }
            catch (Exception e)
            {
                QuickPulseEventSource.Log.ProcessesReadingFailedEvent(e.ToInvariantString());

                this.InitializationFailed = true;

                if (e is UnauthorizedAccessException || e is SecurityException)
                {
                    this.AccessDenied = true;
                }
            }
        }

        /// <summary>
        /// Closes the top CPU collector.
        /// </summary>
        public void Close()
        {
            this.processProvider.Close();
        }

        private void CleanState(HashSet<string> encounteredProcs)
        {
            // remove processes that we haven't encountered this time around
            string[] processCpuKeysToRemove = this.ProcessObservations.Keys.Where(p => !encounteredProcs.Contains(p)).ToArray();
            foreach (var key in processCpuKeysToRemove)
            {
                this.ProcessObservations.Remove(key);
            }
        }
    }
}