namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers
{
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// HTTP Dependency parser that attempts to parse dependency as Azure IoT Hub call.
    /// </summary>
    internal static class AzureIotHubHttpParser
    {
        private static readonly string[] AzureIotHubHostSuffixes =
            {
                ".azure-devices.net",
            };

        /// <summary>
        /// Tries parsing given dependency telemetry item. 
        /// </summary>
        /// <param name="httpDependency">Dependency item to parse. It is expected to be of HTTP type.</param>
        /// <returns><code>true</code> if successfully parsed dependency.</returns>
        internal static bool TryParse(ref DependencyTelemetry httpDependency)
        {
            string name = httpDependency.Name;
            string host = httpDependency.Target;
            string url = httpDependency.Data;

            if (name == null || host == null || url == null)
            {
                return false;
            }

            if (!HttpParsingHelper.EndsWithAny(host, AzureIotHubHostSuffixes))
            {
                return false;
            }

            httpDependency.Type = RemoteDependencyConstants.AzureIotHub;

            return true;
        }
    }
}
