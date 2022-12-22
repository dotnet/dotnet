namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// Provides default values for the heartbeat feature of Application Insights that
    /// are specific to Azure App Services (Web Apps, Functions, etc...).
    /// </summary>
    public sealed class AppServicesHeartbeatTelemetryModule : ITelemetryModule, IDisposable
    {
        /// <summary>
        /// Environment variables and the Application Insights heartbeat field names that accompany them.
        /// </summary>
        internal readonly KeyValuePair<string, string>[] WebHeartbeatPropertyNameEnvVarMap = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("appSrv_SiteName", "WEBSITE_SITE_NAME"),
            new KeyValuePair<string, string>("appSrv_SlotName", "WEBSITE_SLOT_NAME"),
            new KeyValuePair<string, string>("appSrv_wsStamp", "WEBSITE_HOME_STAMPNAME"),
            new KeyValuePair<string, string>("appSrv_wsHost", "WEBSITE_HOSTNAME"),
            new KeyValuePair<string, string>("appSrv_wsOwner", "WEBSITE_OWNER_NAME"),
            new KeyValuePair<string, string>("appSrv_ResourceGroup", "WEBSITE_RESOURCE_GROUP"),
        };

        private IHeartbeatPropertyManager heartbeatManager;

        /// <summary>
        /// Initializes a new instance of the<see cref="AppServicesHeartbeatTelemetryModule" /> class.
        /// </summary>
        public AppServicesHeartbeatTelemetryModule() : this(null)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServicesHeartbeatTelemetryModule" /> class. This is
        /// internal, and allows for overriding the Heartbeat Property Manager to test this module with.
        /// </summary>
        /// <param name="hbeatPropManager">The heartbeat property manager to use when setting/updating env var values.</param>
        internal AppServicesHeartbeatTelemetryModule(IHeartbeatPropertyManager hbeatPropManager) => this.HeartbeatPropertyManager = hbeatPropManager;

        /// <summary>
        /// Gets or sets an instance of IHeartbeatPropertyManager. 
        /// </summary>
        /// <remarks>
        /// This is expected to be an instance of <see cref="DiagnosticsTelemetryModule"/>.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IHeartbeatPropertyManager HeartbeatPropertyManager
        {
            get
            {
                if (this.heartbeatManager == null)
                {
                    this.heartbeatManager = HeartbeatPropertyManagerProvider.GetHeartbeatPropertyManager();
                }

                return this.heartbeatManager;
            }

            set => this.heartbeatManager = value;
        }

        /// <summary>Gets a value indicating whether this module has been initialized.</summary>
        /// <remarks>Used to determine if we call Add or Set heartbeat properties in the case of updates.</remarks>
        internal bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Initialize the default heartbeat provider for Azure App Services. This module
        /// looks for specific environment variables and sets them into the heartbeat 
        /// properties for Application Insights, if they exist.
        /// </summary>
        /// <param name="configuration">Unused parameter.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            this.UpdateHeartbeatWithAppServiceEnvVarValues();
            AppServiceEnvironmentVariableMonitor.Instance.MonitoredAppServiceEnvVarUpdatedEvent += this.UpdateHeartbeatWithAppServiceEnvVarValues;
        }

        /// <summary>
        /// Signal the AppServicesHeartbeatTelemetryModule to update the values of the 
        /// Environment variables we use in our heartbeat payload.
        /// </summary>
        public void UpdateHeartbeatWithAppServiceEnvVarValues()
        {
            try
            {
                var hbeatManager = this.HeartbeatPropertyManager;
                if (hbeatManager != null)
                {
                    this.IsInitialized = this.AddAppServiceEnvironmentVariablesToHeartbeat(hbeatManager, isUpdateOperation: this.IsInitialized);
                }
            }
            catch (Exception appSrvEnvVarHbeatFailure)
            {
                WindowsServerEventSource.Log.AppServiceHeartbeatPropertySettingFails(appSrvEnvVarHbeatFailure.ToInvariantString());
            }
        }

        /// <summary>
        /// Remove our event handler from the environment variable monitor.
        /// </summary>
        public void Dispose()
        {
            AppServiceEnvironmentVariableMonitor.Instance.MonitoredAppServiceEnvVarUpdatedEvent -= this.UpdateHeartbeatWithAppServiceEnvVarValues;
        }

        private bool AddAppServiceEnvironmentVariablesToHeartbeat(IHeartbeatPropertyManager hbeatManager, bool isUpdateOperation = false)
        {
            bool hasBeenUpdated = false;

            if (hbeatManager == null)
            {
                WindowsServerEventSource.Log.AppServiceHeartbeatSetCalledWithNullManager();
            }
            else
            {
                foreach (var kvp in this.WebHeartbeatPropertyNameEnvVarMap)
                {
                    try
                    {
                        string hbeatKey = kvp.Key;
                        string hbeatValue = string.Empty;
                        AppServiceEnvironmentVariableMonitor.Instance.GetCurrentEnvironmentVariableValue(kvp.Value, ref hbeatValue);
                        if (isUpdateOperation)
                        {
                            hbeatManager.SetHeartbeatProperty(hbeatKey, hbeatValue);
                        }
                        else
                        {
                            hbeatManager.AddHeartbeatProperty(hbeatKey, hbeatValue, true);
                        }

                        hasBeenUpdated = true;
                    }
                    catch (Exception heartbeatValueException)
                    {
                        WindowsServerEventSource.Log.AppServiceHeartbeatPropertyAquisitionFailed(kvp.Value, heartbeatValueException.ToInvariantString());
                    }
                }
            }

            return hasBeenUpdated;
        }
    }
}