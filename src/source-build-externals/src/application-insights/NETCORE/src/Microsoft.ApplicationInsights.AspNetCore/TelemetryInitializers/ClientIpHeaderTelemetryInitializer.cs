namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;

    /// <summary>
    /// This telemetry initializer extracts client IP address and populates telemetry.Context.Location.Ip property.
    /// </summary>
    public class ClientIpHeaderTelemetryInitializer : TelemetryInitializerBase
    {
        private const string HeaderNameDefault = "X-Forwarded-For";
        private readonly char[] headerValuesSeparatorDefault = { ',' };

        private char[] headerValueSeparators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIpHeaderTelemetryInitializer" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor to provide HttpContext corresponding to telemetry items.</param>
        public ClientIpHeaderTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
             : base(httpContextAccessor)
        {
            this.HeaderNames = new List<string>();
            this.HeaderNames.Add(HeaderNameDefault);
            this.UseFirstIp = true;
            this.headerValueSeparators = this.headerValuesSeparatorDefault;
        }

        /// <summary>
        /// Gets comma separated list of request header names that is used to check client id.
        /// </summary>
        public ICollection<string> HeaderNames { get; }

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

        /// <inheritdoc />
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

            if (!string.IsNullOrEmpty(telemetry.Context.Location.Ip))
            {
                // Ip is already populated.
                AspNetCoreEventSource.Instance.LogClientIpHeaderTelemetryInitializerOnInitializeTelemetryIpNull();
                return;
            }

            if (string.IsNullOrEmpty(requestTelemetry.Context.Location.Ip))
            {
                string resultIp = null;

                if (platformContext == null)
                {
                    throw new ArgumentNullException(nameof(platformContext));
                }

                if (platformContext.Request?.Headers != null)
                {
                    foreach (var name in this.HeaderNames)
                    {
                        string headerValue = platformContext.Request.Headers[name];
                        if (!string.IsNullOrEmpty(headerValue))
                        {
                            var ip = this.GetIpFromHeader(headerValue);
                            ip = CutPort(ip);
                            if (IsCorrectIpAddress(ip))
                            {
                                resultIp = ip;
                                break;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(resultIp))
                {
                    var connectionFeature = platformContext.Features.Get<IHttpConnectionFeature>();

                    if (connectionFeature?.RemoteIpAddress != null)
                    {
                        resultIp = connectionFeature.RemoteIpAddress.ToString();
                    }
                }

                requestTelemetry.Context.Location.Ip = resultIp;
            }

            telemetry.Context.Location.Ip = requestTelemetry.Context.Location.Ip;
        }

        private static string CutPort(string address)
        {
            // For Web sites in Azure header contains ip address with port e.g. 50.47.87.223:54464
            int portSeparatorIndex = address.IndexOf(":", StringComparison.OrdinalIgnoreCase);

            if (portSeparatorIndex > 0)
            {
                return address.Substring(0, portSeparatorIndex);
            }

            return address;
        }

        private static bool IsCorrectIpAddress(string address)
        {
            IPAddress outParameter;
            address = address.Trim();

            // Core SDK does not support setting Location.Ip to malformed ip address
            if (IPAddress.TryParse(address, out outParameter))
            {
                // Also SDK supports only ipv4!
                if (outParameter.AddressFamily == AddressFamily.InterNetwork)
                {
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
    }
}