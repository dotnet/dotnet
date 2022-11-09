namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers
{
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;

    /// <summary>
    /// HTTP Dependency parser that attempts to parse dependency as Azure Table call.
    /// </summary>
    internal static class AzureTableHttpParser
    {
        private static readonly string[] AzureTableHostSuffixes =
            {
                ".table.core.windows.net",
                ".table.core.chinacloudapi.cn",
                ".table.core.cloudapi.de",
                ".table.core.usgovcloudapi.net",
            };

        private static readonly string[] AzureTableSupportedVerbs = { "GET", "PUT", "OPTIONS", "HEAD", "DELETE", "MERGE", "POST" };

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

            if (!HttpParsingHelper.EndsWithAny(host, AzureTableHostSuffixes))
            {
                return false;
            }

            //// 
            //// Table Service REST API: https://msdn.microsoft.com/en-us/library/azure/dd179363.aspx
            ////

            string account = host.Substring(0, host.IndexOf('.'));

            string verb;
            string nameWithoutVerb;

            // try to parse out the verb
            HttpParsingHelper.ExtractVerb(name, out verb, out nameWithoutVerb, AzureTableSupportedVerbs);

            List<string> pathTokens = HttpParsingHelper.TokenizeRequestPath(nameWithoutVerb);
            string tableName = pathTokens.Count > 0 ? pathTokens[0] : string.Empty;
            int idx = tableName.IndexOf('(');
            if (idx >= 0)
            {
                tableName = tableName.Substring(0, idx);
            }

            httpDependency.Type = RemoteDependencyConstants.AzureTable;
            httpDependency.Name = string.IsNullOrEmpty(verb)
                                      ? account + '/' + tableName
                                      : verb + " " + account + '/' + tableName;

            return true;
        }
    }
}
