namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// A telemetry context initializer that will set component context version on the base of BuildInfo.config information.
    /// </summary>
    public class BuildInfoConfigComponentVersionTelemetryInitializer : ITelemetryInitializer
    {
        private const string BuildInfoConfigFilename = "BuildInfo.config";

        /// <summary>
        /// The version for this component.
        /// </summary>
        private string version;

        /// <summary>
        /// Initializes version of the telemetry item with the version obtained from build info if it is available.
        /// </summary>
        /// <param name="telemetry">The telemetry context to initialize.</param>
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (telemetry.Context != null && telemetry.Context.Component != null)
            {
                if (string.IsNullOrEmpty(telemetry.Context.Component.Version))
                {
                    var buildVersion = LazyInitializer.EnsureInitialized(ref this.version, this.GetVersion);
                    telemetry.Context.Component.Version = buildVersion;
                }
            }
        }

        /// <summary>
        /// Loads BuildInfo.config and returns XElement.
        /// </summary>
        protected virtual XElement LoadBuildInfoConfig()
        {
            XElement result = null;
            try
            {
                string path = string.Empty;

                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BuildInfoConfigComponentVersionTelemetryInitializer.BuildInfoConfigFilename);

                if (File.Exists(path))
                {
                    var document = XDocument.Load(path);
                    result = document.Root;

                    WindowsServerEventSource.Log.BuildInfoConfigLoaded(path);
                }
                else
                {
                    WindowsServerEventSource.Log.BuildInfoConfigNotFound(path);
                }
            }
            catch (XmlException exp)
            {
                WindowsServerEventSource.Log.BuildInfoConfigBrokenXmlError(exp.Message);
            }

            return result;
        }

        /// <summary>
        /// Gets the version for the current application. If the version cannot be found, we will return the passed in default.
        /// </summary>
        /// <returns>The extracted data.</returns>
        private string GetVersion()
        {
            var buildInfoRoot = this.LoadBuildInfoConfig();

            if (buildInfoRoot == null)
            {
                return string.Empty;
            }

            var label = buildInfoRoot
                .Descendants()
                .Where(item => item.Name.LocalName == "Build")
                .Descendants()
                .Where(item => item.Name.LocalName == "MSBuild")
                .Descendants()
                .SingleOrDefault(item => item.Name.LocalName == "BuildLabel");

            if (label == null || string.IsNullOrEmpty(label.Value))
            {
                return string.Empty;
            }

            return label.Value;
        }
    }
}
