namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Web.Implementation;

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
        private const string WebAppHostNameHeaderName = "WAS-DEFAULT-HOSTNAME";
        private const string WebAppHostNameEnvironmentVariable = "WEBSITE_HOSTNAME";
        private string roleName;
        private bool isAzureWebApp;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAppServiceRoleNameFromHostNameHeaderInitializer" /> class.
        /// </summary>
        public AzureAppServiceRoleNameFromHostNameHeaderInitializer() : this(".azurewebsites.net")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAppServiceRoleNameFromHostNameHeaderInitializer" /> class.
        /// </summary>
        /// <param name="webAppSuffix">WebApp name suffix.</param>
        public AzureAppServiceRoleNameFromHostNameHeaderInitializer(string webAppSuffix)
        {
            this.WebAppSuffix = webAppSuffix ?? throw new ArgumentNullException(nameof(webAppSuffix));
            try
            {
                var result = Environment.GetEnvironmentVariable(WebAppHostNameEnvironmentVariable);
                this.isAzureWebApp = !string.IsNullOrEmpty(result);

                if (!string.IsNullOrEmpty(result) && result.EndsWith(this.WebAppSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Substring(0, result.Length - this.WebAppSuffix.Length);
                }

                this.roleName = result;
            }
            catch (Exception ex)
            {
                WebEventSource.Log.LogAzureAppServiceRoleNameFromHostNameHeaderInitializerWarning(ex.ToInvariantString());
            }
        }

        /// <summary>
        /// Gets or sets suffix of website name. This must be changed when running in non public Azure region.
        /// Default value (Public Cloud):  ".azurewebsites.net"
        /// For US Gov Cloud: ".azurewebsites.us"
        /// For Azure Germany: ".azurewebsites.de".
        /// </summary>
        public string WebAppSuffix { get; set; }

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
                if (!this.isAzureWebApp)
                {
                    // return immediately if not azure web app.
                    return;
                }

                if (!string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
                {
                    // RoleName is already populated.
                    return;
                }

                string roleName = string.Empty;
                var context = this.ResolvePlatformContext();

                if (context != null)
                {
                    var request = this.GetRequestFromContext(context);

                    if (request != null)
                    {
                        if (string.IsNullOrEmpty(request.Context.Cloud.RoleName))
                        {
                            if (this.TryGetRoleNameFromHeader(context, out roleName))
                            {
                                request.Context.Cloud.RoleName = roleName;
                            }
                        }
                        else
                        {
                            roleName = request.Context.Cloud.RoleName;
                        }
                    }
                    else
                    {
                        if (!this.TryGetRoleNameFromHeader(context, out roleName))
                        {
                            roleName = this.roleName;
                        }
                    }
                }

                if (string.IsNullOrEmpty(roleName))
                {
                    // Fallback to last known value.
                    roleName = this.roleName;
                }
                else
                {
                    // Update to newest known value
                    this.roleName = roleName;
                }

                telemetry.Context.Cloud.RoleName = roleName;
            }
            catch (Exception ex)
            {
                WebEventSource.Log.LogAzureAppServiceRoleNameFromHostNameHeaderInitializerWarning(ex.ToInvariantString());
            }
        }

        /// <summary>
        /// Resolved web platform specific context.
        /// </summary>
        /// <returns>An instance of the context.</returns>
        protected virtual HttpContext ResolvePlatformContext()
        {
            return HttpContext.Current;
        }

        /// <summary>
        /// Returns request telemetry associated with this context.
        /// </summary>
        /// <returns>request telemetry from the context.</returns>
        protected virtual RequestTelemetry GetRequestFromContext(HttpContext context)
        {
            return context.GetRequestTelemetry();
        }

        private bool TryGetRoleNameFromHeader(HttpContext context, out string roleName)
        {
            roleName = string.Empty;

            if (context.GetRequest() != null)
            {
                string headerValue = context.Request.UnvalidatedGetHeader(WebAppHostNameHeaderName);
                if (!string.IsNullOrEmpty(headerValue))
                {
                    if (headerValue.EndsWith(this.WebAppSuffix, StringComparison.OrdinalIgnoreCase))
                    {
                        headerValue = headerValue.Substring(0, headerValue.Length - this.WebAppSuffix.Length);
                    }

                    roleName = headerValue;
                    return true;
                }
            }

            return false;
        }
    }
}
