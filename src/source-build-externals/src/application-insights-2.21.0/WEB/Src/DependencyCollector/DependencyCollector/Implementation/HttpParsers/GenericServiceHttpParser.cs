namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;

    /// <summary>
    /// HTTP Dependency parser that attempts to parse dependency as generic WCF or Web Service call.
    /// </summary>
    internal static class GenericServiceHttpParser
    {
        /// <summary>
        /// Tries parsing given dependency telemetry item. 
        /// </summary>
        /// <param name="httpDependency">Dependency item to parse. It is expected to be of HTTP type.</param>
        /// <returns><code>true</code> if successfully parsed dependency.</returns>
        internal static bool TryParse(ref DependencyTelemetry httpDependency)
        {
            if (httpDependency.Name.EndsWith(".svc", StringComparison.OrdinalIgnoreCase))
            {
                httpDependency.Type = RemoteDependencyConstants.WcfService;
                return true;
            }

            if (httpDependency.Name.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase))
            {
                httpDependency.Type = RemoteDependencyConstants.WebService;
                return true;
            }

            if (httpDependency.Name.IndexOf(".svc/", StringComparison.OrdinalIgnoreCase) != -1)
            {
                httpDependency.Type = RemoteDependencyConstants.WcfService;
                httpDependency.Name = httpDependency.Name.Substring(0, httpDependency.Name.IndexOf(".svc/", StringComparison.OrdinalIgnoreCase) + ".svc".Length);
                return true;
            }

            if (httpDependency.Name.IndexOf(".asmx/", StringComparison.OrdinalIgnoreCase) != -1)
            {
                httpDependency.Type = RemoteDependencyConstants.WebService;
                httpDependency.Name = httpDependency.Name.Substring(0, httpDependency.Name.IndexOf(".asmx/", StringComparison.OrdinalIgnoreCase) + ".asmx".Length);
                return true;
            }

            return false;
        }
    }
}
