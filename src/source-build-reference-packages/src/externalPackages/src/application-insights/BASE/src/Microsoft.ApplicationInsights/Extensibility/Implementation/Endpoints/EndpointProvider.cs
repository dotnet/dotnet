namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.ConfigString;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// This class encapsulates parsing a connection string and returning an Endpoint's URI.
    /// </summary>
    internal class EndpointProvider : IEndpointProvider
    {
        /// <summary>
        /// Maximum allowed length connection string.
        /// </summary>
        /// <remarks>
        /// Currently 8 accepted keywords (~200 characters).
        /// Assuming 200 characters per value (~1600 characters). 
        /// Total theoretical max length = (1600 + 200) = 1800.
        /// Setting an over-exaggerated max length to protect against malicious injections (2^12 = 4096).
        /// </remarks>
        internal const int ConnectionStringMaxLength = 4096;

        private static readonly char[] TrimPeriod = new char[] { '.' };

        private string connectionString;
        private IDictionary<string, string> connectionStringParsed = new Dictionary<string, string>(0);

        /// <summary>
        /// Gets or sets the connection string. 
        /// Connection String will be in the format: "key1=value1;key2=value2;key3=value3".
        /// Keywords are: InstrumentationKey, Authorization, Location, EndpointSuffix.
        /// Explicit Endpoint Keywords are: IngestionEndpoint, LiveEndpoint, ProfilerEndpoint, SnapshotEndpoint.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(this.ConnectionString));
                }
                else if (value.Length > ConnectionStringMaxLength)
                {
                    CoreEventSource.Log.ConnectionStringExceedsMaxLength(ConnectionStringMaxLength);
                    throw new ArgumentOutOfRangeException(FormattableString.Invariant($"Values greater than {ConnectionStringMaxLength} characters are not allowed."), nameof(this.ConnectionString));
                }

                this.connectionString = value ?? throw new ArgumentNullException(nameof(this.ConnectionString));
                this.connectionStringParsed = ParseConnectionString(value);
            }
        }

        /// <summary>
        /// Will evaluate connection string and return the requested endpoint.
        /// </summary>
        /// <param name="endpointName">Specify which endpoint you want.</param>
        /// <returns>Returns a <see cref="Uri" /> for the requested endpoint.</returns>
        public Uri GetEndpoint(EndpointName endpointName)
        {
            // 1. check for explicit endpoint (location is ignored)
            // 2. check for endpoint suffix (location is optional)
            // 3. use classic endpoint (location is ignored)

            var endpointMeta = EndpointMetaAttribute.GetAttribute(endpointName);

            try
            {
                if (this.connectionStringParsed.TryGetValue(endpointMeta.ExplicitName, out string explicitEndpoint))
                {
                    if (Uri.TryCreate(explicitEndpoint, UriKind.Absolute, out var uri))
                    {
                        return uri;
                    }
                    else
                    {
                        throw new ArgumentException(FormattableString.Invariant($"Connection String Invalid: The value for {endpointMeta.ExplicitName} is invalid."));
                    }
                }
                else if (this.connectionStringParsed.TryGetValue("EndpointSuffix", out string endpointSuffix))
                {
                    if (TryBuildUri(prefix: endpointMeta.EndpointPrefix, suffix: endpointSuffix, location: this.GetLocation(), uri: out var uri))
                    {
                        return uri;
                    }
                    else
                    {
                        throw new ArgumentException("Connection String Invalid: The value for EndpointSuffix is invalid.");
                    }
                }
                else
                {
                    return new Uri(endpointMeta.Default);
                }
            }
            catch (Exception ex)
            {
                CoreEventSource.Log.ConnectionStringInvalidEndpoint(ex.ToInvariantString());
                throw; // Re-throw original exception
            }
        }

        /// <summary>
        /// Will evaluate connection string and return the requested instrumentation key.
        /// </summary>
        /// <returns>Returns the instrumentation key from the connection string.</returns>
        public string GetInstrumentationKey()
        {
            if (this.connectionStringParsed.TryGetValue("InstrumentationKey", out string value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException("Connection String Invalid: InstrumentationKey is required.");
            }
        }

        /// <summary>
        /// Parse a connection string and return a Dictionary.
        /// </summary>
        /// <remarks>Example: "key1=value1;key2=value2;key3=value3".</remarks>
        /// <returns>A dictionary parsed from the input connection string.</returns>
        internal static IDictionary<string, string> ParseConnectionString(string connectionString)
        {
            try
            {
                return ConfigStringParser.Parse(connectionString);
            }
            catch (Exception ex)
            {
                string message = "There was an error parsing the Connection String: " + ex.Message;
                CoreEventSource.Log.ConnectionStringParseError(message);
                throw new ArgumentException(message, ex);
            }
        }

        /// <summary>
        /// Construct a Uri from the possible parts.
        /// Will also attempt to sanitize user input.
        /// </summary>
        /// <remarks>
        /// Format: "location.prefix.suffix".
        /// Example: "https://westus2.dc.applicationinsights.azure.cn/".
        /// </remarks>
        /// <returns>Returns a <see cref="Uri"/> built from the inputs.</returns>
        internal static bool TryBuildUri(string prefix, string suffix, out Uri uri, string location = null)
        {
            // Location and Host are user input fields and need to be checked for extra periods.

            if (location != null)
            {
                location = location.Trim().TrimEnd(TrimPeriod);

                // Location names are expected to match Azure region names. No special characters allowed.
                if (!location.All(x => char.IsLetterOrDigit(x)))
                {
                    throw new ArgumentException("Connection String Invalid: Location must not contain special characters.");
                }
            }

            var uriString = string.Concat("https://",
                string.IsNullOrEmpty(location) ? string.Empty : (location + "."),
                prefix,
                ".",
                suffix.Trim().TrimStart(TrimPeriod));

            return Uri.TryCreate(uriString, UriKind.Absolute, out uri);
        }

        private string GetLocation() => this.connectionStringParsed.TryGetValue("Location", out string location) ? location : null;
    }
}
