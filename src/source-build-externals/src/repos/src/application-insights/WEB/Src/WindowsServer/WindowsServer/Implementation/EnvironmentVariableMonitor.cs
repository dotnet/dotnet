namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Security;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Utility to monitor the value of environment variables which may change 
    /// during the run of an application. Checks the environment variables 
    /// intermittently.
    /// </summary>
    internal abstract class EnvironmentVariableMonitor : IDisposable
    {
        // Environment variables tracked by this monitor.
        protected readonly ConcurrentDictionary<string, string> CheckedValues;

        // help ensure no Timer problems by enforcing a minimum check interval
        protected readonly TimeSpan MinimumCheckInterval = TimeSpan.FromSeconds(5);

        // enabled flag primarily used during dispose
        protected volatile bool isEnabled = true;

        // how often we allow the code to re-check the environment
        protected TimeSpan checkInterval;

        // timer object that will periodically update the environment variables
        private readonly Timer environmentCheckTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentVariableMonitor" /> class.
        /// </summary>
        protected EnvironmentVariableMonitor(IEnumerable<string> envVars, TimeSpan checkInterval)
        {
            this.CheckedValues = new ConcurrentDictionary<string, string>();
            this.checkInterval = checkInterval > this.MinimumCheckInterval ? checkInterval : this.MinimumCheckInterval;

            foreach (string varName in envVars)
            {
                this.CheckedValues.TryAdd(varName, Environment.GetEnvironmentVariable(varName));
            }

            this.environmentCheckTimer = new Timer(this.CheckVariablesIntermittent, null, Timeout.Infinite, Timeout.Infinite);
            this.environmentCheckTimer.Change(checkInterval, TimeSpan.FromMilliseconds(-1));
        }

        /// <summary>
        /// Get the latest value assigned to an environment variable.
        /// </summary>
        /// <param name="envVarName">Name of the environment variable to acquire.</param>
        /// <param name="value">Current cached value of the environment variable.</param>
        public void GetCurrentEnvironmentVariableValue(string envVarName, ref string value)
        {
            value = this.CheckedValues.GetOrAdd(envVarName, (key) => { return Environment.GetEnvironmentVariable(key); });
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Method to update subscribers whenever a change in the tracked environment variables is detected.
        /// </summary>
        protected abstract void OnEnvironmentVariableUpdated();

        /// <summary>
        /// Check and update the variables being tracked and if any updates are detected,
        /// raise the OnEnvironmentVariableUpdated event. Restart the timer to check again
        /// in the configured interval once complete.
        /// </summary>
        /// <param name="state">Variable left unused in this implementation of TimerCallback.</param>
        protected void CheckVariablesIntermittent(object state)
        {
            try
            {
                bool shouldTriggerOnUpdate = false;

                var iter = this.CheckedValues.GetEnumerator();
                while (iter.MoveNext())
                {
                    var kvp = iter.Current;
                    string envValue = string.Empty;

                    try
                    {
                        envValue = Environment.GetEnvironmentVariable(kvp.Key);
                    }
                    catch (SecurityException e)
                    {
                        WindowsServerEventSource.Log.SecurityExceptionThrownAccessingEnvironmentVariable(kvp.Key, e.ToInvariantString());
                        this.isEnabled = false;
                        break;
                    }

                    if (envValue != null
                        && !envValue.Equals(kvp.Value, StringComparison.Ordinal)
                        && this.CheckedValues.TryUpdate(kvp.Key, envValue, kvp.Value))
                    {
                        shouldTriggerOnUpdate = true;
                    }
                }

                if (shouldTriggerOnUpdate)
                {
                    this.OnEnvironmentVariableUpdated();
                }

                if (this.isEnabled)
                {
                    this.environmentCheckTimer.Change(this.checkInterval, TimeSpan.FromMilliseconds(-1));
                }
            }
            catch (Exception e)
            {
                WindowsServerEventSource.Log.GeneralFailureOccursDuringCheckForEnvironmentVariables(e.ToInvariantString());
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.isEnabled = false;

                if (this.environmentCheckTimer != null)
                {
                    try
                    {
                        this.environmentCheckTimer.Dispose();
                    }
                    catch (Exception e)
                    {
                        WindowsServerEventSource.Log.EnvironmentVarMonitorFailedDispose(e.ToInvariantString());
                    }
                }
            }
        }
    }
}
