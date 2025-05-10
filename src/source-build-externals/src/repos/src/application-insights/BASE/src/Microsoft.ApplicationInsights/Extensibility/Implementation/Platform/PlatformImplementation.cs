namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Platform
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Security;

    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// The .NET 4.0 and 4.5 implementation of the <see cref="IPlatform"/> interface.
    /// </summary>
    internal class PlatformImplementation : IPlatform
    {
        private readonly IDictionary environmentVariables;

        private IDebugOutput debugOutput = null;
        private string hostName;

        /// <summary>
        /// Initializes a new instance of the PlatformImplementation class.
        /// </summary>
        public PlatformImplementation()
        {
            try
            {
                this.environmentVariables = Environment.GetEnvironmentVariables();
            }
            catch (Exception e)
            {
                CoreEventSource.Log.FailedToLoadEnvironmentVariables(e.ToString());
            }
        }

#if NETSTANDARD
        /// <summary>
        /// Returns contents of the ApplicationInsights.config file in the application directory.
        /// </summary>
        public string ReadConfigurationXml()
        {
            return null;
        }
#else
        /// <summary>
        /// Returns contents of the ApplicationInsights.config file in the application directory.
        /// </summary>
        public string ReadConfigurationXml()
        {
            string configFilePath;
            try
            {
                // Config file should be in the base directory of the app domain
                configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.config");
            }
            catch (SecurityException)
            {
                CoreEventSource.Log.ApplicationInsightsConfigNotAccessibleWarning();
                return string.Empty;
            }

            try
            {
                // Ensure config file actually exists
                if (File.Exists(configFilePath))
                {
                    return File.ReadAllText(configFilePath);
                }
            }
            catch (FileNotFoundException)
            {
                // For cases when file was deleted/modified while reading
                CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }
            catch (DirectoryNotFoundException)
            {
                // For cases when file was deleted/modified while reading
                CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }
            catch (IOException)
            {
                // For cases when file was deleted/modified while reading
                CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }
            catch (UnauthorizedAccessException)
            {
                CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }
            catch (SecurityException)
            {
                CoreEventSource.Log.ApplicationInsightsConfigNotFoundWarning(configFilePath);
            }

            return string.Empty;
        }
#endif

        /// <summary>
        /// Returns the platform specific Debugger writer to the VS output console.
        /// </summary>
        public IDebugOutput GetDebugOutput()
        {
            return this.debugOutput ?? (this.debugOutput = new TelemetryDebugWriter());
        }

        /// <inheritdoc />
        public bool TryGetEnvironmentVariable(string name, out string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            object resultObj = this.environmentVariables?[name];
            value = resultObj?.ToString();
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Returns the machine name.
        /// </summary>
        /// <returns>The machine name.</returns>
        public string GetMachineName()
        {
            return this.hostName ?? (this.hostName = GetHostName());
        }

        private static string GetHostName()
        {
            try
            {
                string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
                string hostName = Dns.GetHostName();

                if (!hostName.EndsWith(domainName, StringComparison.OrdinalIgnoreCase))
                {
                    hostName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", hostName, domainName);
                }

                return hostName;
            }
            catch (Exception ex)
            {
                CoreEventSource.Log.FailedToGetMachineName(ex.Message);
            }

            return string.Empty;
        }
    }
}