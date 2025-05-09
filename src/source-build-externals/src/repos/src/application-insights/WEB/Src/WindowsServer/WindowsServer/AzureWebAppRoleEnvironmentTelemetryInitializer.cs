namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// A telemetry initializer that will gather Azure Web App Role Environment context information.
    /// </summary>
    public sealed class AzureWebAppRoleEnvironmentTelemetryInitializer : ITelemetryInitializer, IDisposable
    {
        /// <summary>
        /// Azure Web App Hostname. This will include the deployment slot, but will be 
        /// same across instances of same slot. Marked as internal to support testing.
        /// </summary>
        internal string WebAppHostNameEnvironmentVariable = "WEBSITE_HOSTNAME";

        /// <summary>Predefined suffix for Azure Web App Hostname.</summary>
        private const string WebAppSuffix = ".azurewebsites.net";

        private string nodeName;
        private string roleName;

        private volatile bool updateEnvVars = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureWebAppRoleEnvironmentTelemetryInitializer" /> class.
        /// </summary>
        public AzureWebAppRoleEnvironmentTelemetryInitializer()
        {
            WindowsServerEventSource.Log.TelemetryInitializerLoaded(this.GetType().FullName);
            AppServiceEnvironmentVariableMonitor.Instance.MonitoredAppServiceEnvVarUpdatedEvent += this.UpdateEnvironmentValues;
        }

        /// <summary>
        /// Initializes <see cref="ITelemetry" /> role and node context information.
        /// </summary>
        /// <param name="telemetry">The telemetry to initialize.</param>
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (this.updateEnvVars)
            {
                this.roleName = this.GetRoleName();
                this.nodeName = this.GetNodeName();
                this.updateEnvVars = false;
            }

            var context = telemetry.Context;
            var cloudContext = context.Cloud;
            if (string.IsNullOrEmpty(cloudContext.RoleName))
            {
                cloudContext.RoleName = this.roleName;
            }

            var internalContext = context.GetInternalContext();
            if (string.IsNullOrEmpty(internalContext.NodeName))
            {
                internalContext.NodeName = this.nodeName;
            }
        }

        /// <summary>
        /// Remove our event handler from the environment variable monitor.
        /// </summary>
        public void Dispose()
        {
            AppServiceEnvironmentVariableMonitor.Instance.MonitoredAppServiceEnvVarUpdatedEvent -= this.UpdateEnvironmentValues;
        }

        private string GetRoleName()
        {
            var result = this.GetNodeName();
            if (result.EndsWith(WebAppSuffix, StringComparison.OrdinalIgnoreCase))
            {
                result = result.Substring(0, result.Length - WebAppSuffix.Length);
            }

            return result;
        }

        private string GetNodeName()
        {
            string nodeName = string.Empty;
            AppServiceEnvironmentVariableMonitor.Instance.GetCurrentEnvironmentVariableValue(this.WebAppHostNameEnvironmentVariable, ref nodeName);
            return nodeName ?? string.Empty;
        }

        private void UpdateEnvironmentValues()
        {
            this.updateEnvVars = true;
        }
    }
}
