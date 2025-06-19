namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.AspNetCore.Implementation;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// A telemetry initializer that will gather Azure Web App Role Environment context information to
    /// populate TelemetryContext.Cloud.RoleName
    /// This uses the http header "WAS-DEFAULT-HOSTNAME" to update role name, if available.
    /// Otherwise role name is populated from "WEBSITE_HOSTNAME" environment variable.
    /// </summary>
    /// <remarks>
    /// The RoleName is expected to contain the host name + slot name, but will be same across all instances of
    /// a single App Service.
    /// Populating RoleName from HOSTNAME environment variable will cause RoleName to be incorrect when a slot swap occurs in AppService.
    /// The most accurate way to determine the RoleName is to rely on the header WAS-DEFAULT-HOSTNAME, as its
    /// populated from App service front end on every request. Slot swaps are instantly reflected in this header.
    /// </remarks>
    public class AzureAppServiceRoleNameFromHostNameHeaderInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAppServiceRoleNameFromHostNameHeaderInitializer" /> class.
        /// </summary>
        /// <param name="webAppSuffix">WebApp name suffix.</param>
        public AzureAppServiceRoleNameFromHostNameHeaderInitializer(string webAppSuffix = ".azurewebsites.net")
        {
            try
            {
                RoleNameContainer.Instance = new RoleNameContainer(webAppSuffix);
            }
            catch (Exception ex)
            {
                AspNetCoreEventSource.Instance.LogAzureAppServiceRoleNameFromHostNameHeaderInitializerWarning(ex.ToInvariantString());
            }
        }

        /// <summary>
        /// Gets or sets suffix of website name. This must be changed when running in non public Azure region.
        /// Default value (Public Cloud):  ".azurewebsites.net"
        /// For US Gov Cloud: ".azurewebsites.us"
        /// For Azure Germany: ".azurewebsites.de".
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "We want this to be an instance variable for configuration.")]
        public string WebAppSuffix
        {
            get => RoleNameContainer.Instance.HostNameSuffix;

            set => RoleNameContainer.Instance = new RoleNameContainer(value);
        }

        /// <summary>
        /// Populates RoleName from the request telemetry associated with the http context.
        /// If RoleName is empty on the request telemetry, it'll be updated as well so that other telemetry
        /// belonging to the same requests gets it from request telemetry, without having to parse headers again.
        /// </summary>
        /// <remarks>
        /// RoleName is attempted from every incoming request as opposed to doing this periodically. This is
        /// done to ensure every request (and associated telemetry) gets the correct RoleName during slot swap.
        /// </remarks>
        /// <param name="telemetry">The telemetry item for which RoleName is to be set.</param>
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            try
            {
                if (!RoleNameContainer.Instance.IsAzureWebApp)
                {
                    // return immediately if not azure web app.
                    return;
                }

                if (!string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
                {
                    // RoleName is already populated.
                    return;
                }

                telemetry.Context.Cloud.RoleName = RoleNameContainer.Instance.RoleName;
            }
            catch (Exception ex)
            {
                AspNetCoreEventSource.Instance.LogAzureAppServiceRoleNameFromHostNameHeaderInitializerWarning(ex.ToInvariantString());
            }
        }
    }
}
