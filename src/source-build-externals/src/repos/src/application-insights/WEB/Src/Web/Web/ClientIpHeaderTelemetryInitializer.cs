namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// Telemetry initializer populates client IP address for the current request.
    /// </summary>
    public class ClientIpHeaderTelemetryInitializer : WebTelemetryInitializerBase
    {
        private const string HeaderNameDefault = "X-Forwarded-For";
        private readonly char[] headerValuesSeparatorDefault = new char[] { ',' };

        private readonly ICollection<string> headerNames;
        private char[] headerValueSeparators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIpHeaderTelemetryInitializer"/> class.
        /// </summary>
        public ClientIpHeaderTelemetryInitializer()
        {
            this.headerNames = new List<string>();
            this.HeaderNames.Add(HeaderNameDefault);
            this.UseFirstIp = true;
            this.headerValueSeparators = this.headerValuesSeparatorDefault;
        }

        /// <summary>
        /// Gets a list of request header names that is used to check client id.
        /// </summary>
        public ICollection<string> HeaderNames
        {
            get
            {
                return this.headerNames;
            }
        }

        /// <summary>
        /// Gets or sets a header values separator.
        /// </summary>
        public string HeaderValueSeparators
        {
            get
            {
                return string.Concat(this.headerValueSeparators);
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.headerValueSeparators = value.ToCharArray();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the first or the last IP should be used from the lists of IPs in the header.
        /// </summary>
        public bool UseFirstIp { get; set; }

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

            if (requestTelemetry == null)
            {
                throw new ArgumentNullException(nameof(requestTelemetry));
            }

            if (platformContext == null)
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            if (string.IsNullOrEmpty(telemetry.Context.Location.Ip))
            {
                var location = requestTelemetry.Context.Location;

                if (string.IsNullOrEmpty(location.Ip))
                {
                    this.UpdateRequestTelemetry(platformContext, location);
                }

                telemetry.Context.Location.Ip = location.Ip;
            }
        }

        private static bool TryParseIpWithPort(string input, out string ipAddressString)
        {
            Uri uri;
            ipAddressString = null;

            if (Uri.TryCreate($"tcp://{input}", UriKind.Absolute, out uri) ||
                Uri.TryCreate($"tcp://[{input}]", UriKind.Absolute, out uri))
            {
                if (IPAddress.TryParse(uri.Host, out var ip))
                {
                    ipAddressString = new IPEndPoint(ip, uri.Port < 0 ? 0 : uri.Port).Address.ToString();
                    return true;
                }
            }

            return false;
        }

        private string GetIpFromHeader(string clientIpsFromHeader)
        {
            var ips = clientIpsFromHeader.Split(this.headerValueSeparators, StringSplitOptions.RemoveEmptyEntries);
            return this.UseFirstIp ? ips[0].Trim() : ips[ips.Length - 1].Trim();
        }

        private void UpdateRequestTelemetry(HttpContext platformContext, LocationContext location)
        {
            string resultIp = null;
            foreach (var clientIpHeaderName in this.HeaderNames)
            {
                var clientIpsFromHeader = platformContext.Request.UnvalidatedGetHeader(clientIpHeaderName);

                if (!string.IsNullOrWhiteSpace(clientIpsFromHeader))
                {
                    WebEventSource.Log.WebLocationIdHeaderFound(clientIpHeaderName);

                    string ip = this.GetIpFromHeader(clientIpsFromHeader);
                    if (TryParseIpWithPort(ip, out var ipAddressString))
                    {
                        resultIp = ipAddressString;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(resultIp))
            {
                location.Ip = resultIp;
            }
            else
            {
                location.Ip = platformContext.Request.GetUserHostAddress();
            }

            WebEventSource.Log.WebLocationIdSet(location.Ip);
        }
    }
}
