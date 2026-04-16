#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;    
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;    

    /// <summary>
    /// A telemetry initializer that will gather Azure Role Environment context information.
    /// </summary>
    public class AzureRoleEnvironmentTelemetryInitializer : ITelemetryInitializer
    {
        private const string WebSiteEnvironmentVariable = "WEBSITE_SITE_NAME";
        private bool? isAzureWebApp = null;
        private string roleInstanceName;
        private string roleName;                

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureRoleEnvironmentTelemetryInitializer" /> class.
        /// </summary>
        public AzureRoleEnvironmentTelemetryInitializer()
        {
            WindowsServerEventSource.Log.TelemetryInitializerLoaded(this.GetType().FullName);

            if (this.IsAppRunningInAzureWebApp())
            {
                WindowsServerEventSource.Log.AzureRoleEnvironmentTelemetryInitializerNotInitializedInWebApp();
            }
            else
            {
                try
                {
                    this.roleName = AzureRoleEnvironmentContextReader.Instance.GetRoleName();
                    this.roleInstanceName = AzureRoleEnvironmentContextReader.Instance.GetRoleInstanceName();
                }
                catch (Exception ex)
                {
                    WindowsServerEventSource.Log.UnknownErrorOccured("AzureRoleEnvironmentTelemetryInitializer constructor", ex.ToString());
                }
            }            
        }

        /// <summary>
        /// Initializes <see cref="ITelemetry" /> device context.
        /// </summary>
        /// <param name="telemetry">The telemetry to initialize.</param>
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {                
                telemetry.Context.Cloud.RoleName = this.roleName;
            }

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleInstance))
            {                
                telemetry.Context.Cloud.RoleInstance = this.roleInstanceName;
            }

            if (string.IsNullOrEmpty(telemetry.Context.GetInternalContext().NodeName))
            {                
                telemetry.Context.GetInternalContext().NodeName = this.roleInstanceName;
            }
        }

        /// <summary>
        /// Searches for the environment variable specific to Azure web applications and confirms if the current application is a web application or not.
        /// </summary>
        /// <returns>Boolean, which is true if the current application is an Azure web application.</returns>
        private bool IsAppRunningInAzureWebApp()
        {
            if (!this.isAzureWebApp.HasValue)
            {
                try
                {
                    this.isAzureWebApp = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(WebSiteEnvironmentVariable));
                }
                catch (Exception ex)
                {
                    WindowsServerEventSource.Log.AccessingEnvironmentVariableFailedWarning(WebSiteEnvironmentVariable, ex.ToString());
                    return false;
                }
            }

            return (bool)this.isAzureWebApp;
        }
    }
}
#endif