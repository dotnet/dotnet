namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    internal delegate void MonitoredAppServiceEnvVarUpdated();

    /// <summary>
    /// Utility to monitor the value of environment variables which may change 
    /// during the run of an application. Checks the environment variables 
    /// at regular set intervals.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class AppServiceEnvironmentVariableMonitor : EnvironmentVariableMonitor
    {
        // event raised whenever any of the environment variables being watched get updated
        public MonitoredAppServiceEnvVarUpdated MonitoredAppServiceEnvVarUpdatedEvent;

        // Default interval between environment variable checks
        internal static readonly TimeSpan DefaultMonitorInterval = TimeSpan.FromSeconds(60);

        // Default list of environment variables tracked by this monitor.
        internal static IReadOnlyCollection<string> PreloadedMonitoredEnvironmentVariables = new string[]
        {
            "WEBSITE_SITE_NAME",
            "WEBSITE_SLOT_NAME",
            "WEBSITE_HOME_STAMPNAME",
            "WEBSITE_HOSTNAME",
            "WEBSITE_OWNER_NAME",
            "WEBSITE_RESOURCE_GROUP",
        };

        // singleton pattern, this is the one instance of this class allowed
        private static readonly AppServiceEnvironmentVariableMonitor SingletonInstance = new AppServiceEnvironmentVariableMonitor();

        /// <summary>
        /// Prevents a default instance of the <see cref="AppServiceEnvironmentVariableMonitor" /> class from being created.
        /// </summary>
        private AppServiceEnvironmentVariableMonitor() : base(
                AppServiceEnvironmentVariableMonitor.PreloadedMonitoredEnvironmentVariables, 
                AppServiceEnvironmentVariableMonitor.DefaultMonitorInterval)
        {
            // check to ensure there is at least one known Azure App Service environment variable present
            bool validateAppServiceEnvironment = false;
            foreach (var environmentVariableName in AppServiceEnvironmentVariableMonitor.PreloadedMonitoredEnvironmentVariables)
            {
                string environmentVariableValue = string.Empty;
                this.GetCurrentEnvironmentVariableValue(environmentVariableName, ref environmentVariableValue);
                if (!string.IsNullOrEmpty(environmentVariableValue))
                {
                    validateAppServiceEnvironment = true;
                    break;
                }
            }

            // if not, disable this monitor
            if (!validateAppServiceEnvironment)
            {
                this.isEnabled = false;
            }
        }

        public static AppServiceEnvironmentVariableMonitor Instance => AppServiceEnvironmentVariableMonitor.SingletonInstance;

        internal static TimeSpan MonitorInterval
        {
            get => AppServiceEnvironmentVariableMonitor.Instance.checkInterval;
            set => AppServiceEnvironmentVariableMonitor.Instance.checkInterval = value;
        }

        protected override void OnEnvironmentVariableUpdated()
        {
            this.MonitoredAppServiceEnvVarUpdatedEvent?.Invoke();
        }
    }
}
