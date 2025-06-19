namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System;
    using System.Text.RegularExpressions;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Allows configuration of patterns for synthetic traffic filters.
    /// </summary>
    [Obsolete("Replaced with string compare in SyntheticUserAgentTelemetryInitializer. See: https://github.com/Microsoft/ApplicationInsights-dotnet-server/pull/85")]
    public class SyntheticUserAgentFilter
    {
        private string pattern;

        /// <summary>
        /// Gets or sets the regular expression pattern applied to the user agent string to determine whether traffic is synthetic.
        /// </summary>
        public string Pattern
        {
            get
            {
                return this.pattern;
            }

            set
            {
                this.pattern = value;

                try
                {
                    this.RegularExpression = new Regex(this.pattern, RegexOptions.Compiled);
                }
                catch (ArgumentException ex)
                {
                    WebEventSource.Log.SyntheticUserAgentTelemetryInitializerRegularExpressionParsingException(this.pattern, ex.ToInvariantString());
                }                
            }
        }

        /// <summary>
        /// Gets or sets the readable name for the synthetic traffic source. If not provided, defaults to the pattern match.
        /// </summary>
        public string SourceName { get; set; }

        internal Regex RegularExpression { get; set; }
    }
}
