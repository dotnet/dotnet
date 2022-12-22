using System;

namespace Microsoft.DotNet.Helix.Client
{
    public static class ApiFactory
    {
        /// <summary>
        /// Obtains API client for authenticated access to internal queues.
        /// The client will access production Helix instance.
        /// </summary>
        /// <param name="accessToken">
        /// You can get the access token by logging in to your Helix instance
        /// and proceeding to Profile page.
        /// </param>
        public static IHelixApi GetAuthenticated(string accessToken)
        {
            return new HelixApi(new HelixApiOptions(new HelixApiTokenCredential(accessToken)));
        }

        /// <summary>
        /// Obtains API client for unauthenticated access to external queues.
        /// The client will access production Helix instance.
        /// </summary>
        /// <remarks>
        /// Attempt to access internal queues by such client will cause
        /// <see cref="ArgumentException"/> triggered by <c>SendAsync</c> call.
        /// </remarks>
        public static IHelixApi GetAnonymous()
        {
            return new HelixApi(new HelixApiOptions());
        }

        /// <summary>
        /// Obtains API client for authenticated access to internal queues.
        /// The client will access Helix instance at the provided URI.
        /// </summary>
        /// <param name="accessToken">
        /// You can get the access token by logging in to your Helix instance
        /// and proceeding to Profile page.
        /// </param>
        public static IHelixApi GetAuthenticated(string baseUri, string accessToken)
        {
            return new HelixApi(new HelixApiOptions(new Uri(baseUri), new HelixApiTokenCredential(accessToken)));
        }

        /// <summary>
        /// Obtains API client for unauthenticated access to external queues.
        /// The client will access Helix instance at the provided URI.
        /// </summary>
        /// <remarks>
        /// Attempt to access internal queues by such client will cause
        /// <see cref="ArgumentException"/> triggered by <c>SendAsync</c> call.
        /// </remarks>
        public static IHelixApi GetAnonymous(string baseUri)
        {
            return new HelixApi(new HelixApiOptions(new Uri(baseUri)));
        }
    }
}
