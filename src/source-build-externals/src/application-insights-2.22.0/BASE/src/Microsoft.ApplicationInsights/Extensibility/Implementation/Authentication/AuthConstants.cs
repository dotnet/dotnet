namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication
{
    internal static class AuthConstants
    {
        public const string AuthorizationHeaderName = "Authorization";

        public const string AuthorizationTokenPrefix = "Bearer ";

        /// <summary>
        /// Source: 
        /// (https://docs.microsoft.com/azure/active-directory/develop/msal-acquire-cache-tokens#scopes-when-acquiring-tokens).
        /// (https://docs.microsoft.com/azure/active-directory/develop/v2-permissions-and-consent#the-default-scope).
        /// </summary>
        private const string AzureMonitorScope = "https://monitor.azure.com//.default";

        /// <summary>
        /// Get scopes for Azure Monitor as an array.
        /// </summary>
        /// <returns>An array of scopes.</returns>
        public static string[] GetScopes() => new string[] { AzureMonitorScope };
    }
}
