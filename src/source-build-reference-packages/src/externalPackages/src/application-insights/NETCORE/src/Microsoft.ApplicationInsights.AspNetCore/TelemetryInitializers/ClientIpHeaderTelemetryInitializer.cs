namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
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
                            if (TryParseIpWithPort(ip, out var ipAddressString))
                            {
                                resultIp = ipAddressString;
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
    }
}
