namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;
    using System.Collections.Generic;

    internal class PerformanceCounterImplementation
    {
        /// <summary>
        /// Available Environment Variables in Azure Web Apps.
        /// </summary>
        private readonly Dictionary<AzureWebApEnvironmentVariables, string> environmentVariableMapping = new Dictionary<AzureWebApEnvironmentVariables, string>
        {
            { AzureWebApEnvironmentVariables.AspDotNet, "WEBSITE_COUNTERS_ASPNET" },
            { AzureWebApEnvironmentVariables.App, "WEBSITE_COUNTERS_APP" },
            { AzureWebApEnvironmentVariables.CLR, "WEBSITE_COUNTERS_CLR" },
            { AzureWebApEnvironmentVariables.All, "WEBSITE_COUNTERS_ALL" },
        };

        /// <summary>
        /// Retrieves counter data from Azure Web App Environment Variables.
        /// </summary>
        /// <param name="environmentVariable">Name of environment variable.</param>
        /// <returns>Raw JSON with counters.</returns>
        public string GetAzureWebAppEnvironmentVariables(AzureWebApEnvironmentVariables environmentVariable)
        {
            return Environment.GetEnvironmentVariable(this.environmentVariableMapping[environmentVariable]);
        }
    }
}
