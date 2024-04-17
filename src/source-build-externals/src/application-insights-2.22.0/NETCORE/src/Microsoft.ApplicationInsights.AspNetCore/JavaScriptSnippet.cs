namespace Microsoft.ApplicationInsights.AspNetCore
{
    using System;
    using System.Globalization;
    using System.Security.Principal;
    using System.Text.Encodings.Web;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// This class helps to inject Application Insights JavaScript snippet into application code.
    /// </summary>
    public class JavaScriptSnippet : IJavaScriptSnippet
    {
        private const string ScriptTagBegin = @"<script type=""text/javascript"">";
        private const string ScriptTagEnd = "</script>";

        /// <summary>JavaScript snippet.</summary>
        private static readonly string Snippet = Resources.JavaScriptSnippet;

        /// <summary>JavaScript authenticated user tracking snippet.</summary>
        private static readonly string AuthSnippet = Resources.JavaScriptAuthSnippet;

        /// <summary> Http context accessor.</summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>Configuration instance.</summary>
        private readonly TelemetryConfiguration telemetryConfiguration;

        /// <summary> Weather to print authenticated user tracking snippet.</summary>
        private readonly bool enableAuthSnippet;

        private readonly JavaScriptEncoder encoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptSnippet"/> class.
        /// </summary>
        /// <param name="telemetryConfiguration">The configuration instance to use.</param>
        /// <param name="serviceOptions">Service options instance to use.</param>
        /// <param name="httpContextAccessor">Http context accessor to use.</param>
        /// <param name="encoder">Encoder used to encode user identity.</param>
        public JavaScriptSnippet(
            TelemetryConfiguration telemetryConfiguration,
            IOptions<ApplicationInsightsServiceOptions> serviceOptions,
            IHttpContextAccessor httpContextAccessor,
            JavaScriptEncoder encoder = null)
        {
            if (serviceOptions == null)
            {
                throw new ArgumentNullException(nameof(serviceOptions));
            }

            this.telemetryConfiguration = telemetryConfiguration;
            this.httpContextAccessor = httpContextAccessor;
            this.enableAuthSnippet = serviceOptions.Value.EnableAuthenticationTrackingJavaScript;
            this.encoder = encoder ?? JavaScriptEncoder.Default;
        }

        /// <summary>
        /// Gets the full JavaScript Snippet in HTML script tags with instrumentation key initialized from TelemetryConfiguration.
        /// </summary>
        /// <remarks>This method will evaluate if Telemetry has been disabled in the config and if the instrumentation key was provided by either setting InstrumentationKey or ConnectionString.</remarks>
        /// <returns>JavaScript code snippet with instrumentation key or returns string.Empty if instrumentation key was not set for the application.</returns>
        public string FullScript
        { 
            get
            {
                if (!this.IsAvailable())
                {
                    return string.Empty;
                }
                else
                {
                    return string.Concat(ScriptTagBegin, this.ScriptBody, ScriptTagEnd);
                }
            }
        }

        /// <summary>
        /// Gets the JavaScript Snippet body (without HTML script tags) with instrumentation key initialized from TelemetryConfiguration.
        /// </summary>
        /// <returns>JavaScript code snippet with instrumentation key or returns string.Empty if instrumentation key was not set for the application.</returns>
        public string ScriptBody
        {
            get
            {
                // Config JS SDK
                string insertConfig;
                if (!string.IsNullOrEmpty(this.telemetryConfiguration.ConnectionString))
                {
                    insertConfig = string.Format(CultureInfo.InvariantCulture, "connectionString: '{0}'", this.telemetryConfiguration.ConnectionString);
                }
                else if (!string.IsNullOrEmpty(this.telemetryConfiguration.InstrumentationKey))
                {
                    insertConfig = string.Format(CultureInfo.InvariantCulture, "instrumentationKey: '{0}'", this.telemetryConfiguration.InstrumentationKey);
                }
                else
                {
                    return string.Empty;
                }

                // Auth Snippet (setAuthenticatedUserContext)
                string insertAuthUserContext = string.Empty;
                if (this.enableAuthSnippet)
                {
                    IIdentity identity = this.httpContextAccessor?.HttpContext?.User?.Identity;
                    if (identity != null && identity.IsAuthenticated)
                    {
                        string escapedUserName = this.encoder.Encode(identity.Name);
                        insertAuthUserContext = string.Format(CultureInfo.InvariantCulture, AuthSnippet, escapedUserName);
                    }
                }

                var snippet = Snippet.Replace("instrumentationKey: \"INSTRUMENTATION_KEY\"", insertConfig);
                // Return snippet
                return string.Concat(snippet, insertAuthUserContext);
            }
        }

        /// <summary>
        /// Determine if we have enough information to build a full script.
        /// </summary>
        /// <returns>Returns true if we can build the JavaScript snippet.</returns>
        private bool IsAvailable()
        {
            if (this.telemetryConfiguration.DisableTelemetry)
            {
                return false;
            }
            else
            {
                return !(string.IsNullOrEmpty(this.telemetryConfiguration.ConnectionString)
                    && string.IsNullOrEmpty(this.telemetryConfiguration.InstrumentationKey));
            }
        }
    }
}
