namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;

    /// <summary>
    /// HTTP Dependency parser that attempts to parse dependency as Azure Blob call.
    /// </summary>
    internal static class AzureBlobHttpParser
    {
        private static readonly string[] AzureBlobHostSuffixes =
            {
                ".blob.core.windows.net",
                ".blob.core.chinacloudapi.cn",
                ".blob.core.cloudapi.de",
                ".blob.core.usgovcloudapi.net",
            };

        private static readonly string[] AzureBlobSupportedVerbs = { "GET", "PUT", "OPTIONS", "HEAD", "DELETE" };

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

            if (!HttpParsingHelper.EndsWithAny(host, AzureBlobHostSuffixes))
            {
                return false;
            }

            ////
            //// Blob Service REST API: https://msdn.microsoft.com/en-us/library/azure/dd135733.aspx
            ////

            string account = host.Substring(0, host.IndexOf('.'));

            string verb;
            string nameWithoutVerb;

            // try to parse out the verb
            HttpParsingHelper.ExtractVerb(name, out verb, out nameWithoutVerb, AzureBlobSupportedVerbs);

            List<string> pathTokens = HttpParsingHelper.TokenizeRequestPath(nameWithoutVerb);

            string container = null;
            string blob = null;

            if (pathTokens.Count == 1)
            {
                container = pathTokens[0];
            } 
            else if (pathTokens.Count > 1)
            {
                Dictionary<string, string> queryParameters = HttpParsingHelper.ExtractQuryParameters(url);
                string resType;
                if (queryParameters == null || !queryParameters.TryGetValue("restype", out resType)
                    || !string.Equals(resType, "container", StringComparison.OrdinalIgnoreCase))
                {
                    // if restype != container then the last path entry is blob name
                    blob = pathTokens[pathTokens.Count - 1];
                    httpDependency.Properties["Blob"] = blob;

                    pathTokens.RemoveAt(pathTokens.Count - 1);
                }

                container = string.Join("/", pathTokens);
            }

            if (container != null)
            {
                httpDependency.Properties["Container"] = container;
            }

            // This is very naive overwriting of Azure Blob dependency that is compatible with the today's implementation
            //
            // Possible improvements:
            //
            // 1. Use specific name for specific operations. Like "Lease Blob" for "?comp=lease" query parameter
            // 2. Use account name as a target instead of "account.blob.core.windows.net"
            httpDependency.Type = RemoteDependencyConstants.AzureBlob;
            httpDependency.Name = string.IsNullOrEmpty(verb) ? account : verb + " " + account;

            return true;
        }
    }
}
