namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// A telemetry initializer that determines if the request came from a synthetic source based on the user agent string.
    /// </summary>
    public class SyntheticUserAgentTelemetryInitializer : WebTelemetryInitializerBase
    {
        private const string SyntheticSourceNameKey = "Microsoft.ApplicationInsights.RequestTelemetry.SyntheticSource";
        private const string SyntheticSourceName = "Bot";
        private string filters = string.Empty;
        private string[] filterPatterns;        

        /// <summary>
        /// Gets or sets the configured patterns for matching synthetic traffic filters through user agent string.
        /// </summary>
        public string Filters
        {
            get
            {
                return this.filters;
            }

            set
            {
                if (value != null)
                {
                    this.filters = value;

                    // We expect customers to configure telemetry initializer before they add it to active configuration
                    // So we will not protect it with locks (to improve perf)
                    this.filterPatterns = value.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }

        /// <summary>
        /// Implements initialization logic.
        /// </summary>
        /// <param name="platformContext">Http context.</param>
        /// <param name="requestTelemetry">Request telemetry object associated with the current request.</param>
        /// <param name="telemetry">Telemetry item to initialize.</param>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (string.IsNullOrEmpty(telemetry.Context.Operation.SyntheticSource))
            {
                if (platformContext != null)
                {
                    if (platformContext.Items.Contains(SyntheticSourceNameKey))
                    {
                        // The value does not really matter.
                        telemetry.Context.Operation.SyntheticSource = SyntheticSourceName;
                    }
                    else
                    {
                        var request = platformContext.GetRequest();
                        string userAgent = request?.UserAgent;
                        if (!string.IsNullOrEmpty(userAgent))
                        { 
                            // We expect customers to configure telemetry initializer before they add it to active configuration
                            // So we will not protect filterPatterns array with locks (to improve perf)                            
                            for (int i = 0; i < this.filterPatterns.Length; i++)
                            {
                                if (userAgent.IndexOf(this.filterPatterns[i], StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    telemetry.Context.Operation.SyntheticSource = SyntheticSourceName;
                                    platformContext.Items.Add(SyntheticSourceNameKey, SyntheticSourceName);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}