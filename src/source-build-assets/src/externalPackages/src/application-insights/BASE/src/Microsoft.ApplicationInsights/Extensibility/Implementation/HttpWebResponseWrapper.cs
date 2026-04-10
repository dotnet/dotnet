namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    /// <summary>
    /// HttpWebResponse wrapper object.
    /// </summary>
    public class HttpWebResponseWrapper
    {
        /// <summary>
        /// Gets or sets HttpWebResponse content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets HttpWebResponse StatusCode. 
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets HttpWebResponse Retry-After header value.
        /// </summary>
        public string RetryAfterHeader { get; set; }

        /// <summary>
        /// Gets or sets HttpWebResponse StatusDescription.
        /// </summary>
        public string StatusDescription { get; set; }
    }
}
