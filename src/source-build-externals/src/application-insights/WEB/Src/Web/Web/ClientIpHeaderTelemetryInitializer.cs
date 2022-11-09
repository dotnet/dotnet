namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
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

        private static string CutPort(string address)
        {
            // For Web sites in Azure header contains ip address with port e.g. 50.47.87.223:54464
            int portSeparatorIndex = address.LastIndexOf(":", StringComparison.OrdinalIgnoreCase);

            if (portSeparatorIndex > 0)
            {
                return address.Substring(0, portSeparatorIndex);
            }

            return address;
        }

        private static bool IsCorrectIpAddress(string address)
        {
            IPAddress outParameter;

            // Base SDK does not support setting Location.Ip to malformed ip address
            // It also do not have sanitization logic implemented.
            // In order to minimize behavior changes - keeping this check in place
            // In future versions we may simply take the content of the header 
            // and let Base SDK sanitize and reject it
            if (IPAddress.TryParse(address.Trim(), out outParameter))
            {
                // This logic is faulty. IPv6 address may be used in the header without the port number
                // IPv6 addresses have colons in them and we have existing code that (mistakenly) 
                // assumes that everything after a colon is a port number. For now, we'll ignore the problem, 
                // but eventually we'll need to implement support for it.
                // We also need to support RFC7239 at least as an option.
                return true;
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
                    ip = CutPort(ip);
                    if (IsCorrectIpAddress(ip))
                    {
                        resultIp = ip;
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
