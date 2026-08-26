// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Azure.Core;

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
        /// Obtains an API client using an Entra credential for authenticated access to internal queues.
        /// The client requests the production Helix API scope and refreshes tokens based on their expiry.
        /// </summary>
        public static IHelixApi GetAuthenticatedWithEntra(TokenCredential credential)
        {
            return new HelixApi(new HelixApiOptions(ValidateEntraCredential(credential)));
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
        /// Obtains an API client using an Entra credential for authenticated access to the provided Helix instance.
        /// Production and staging scopes are selected from the base URI.
        /// </summary>
        public static IHelixApi GetAuthenticatedWithEntra(string baseUri, TokenCredential credential)
        {
            return new HelixApi(new HelixApiOptions(new Uri(baseUri), ValidateEntraCredential(credential)));
        }

        /// <summary>
        /// Obtains an API client using an Entra credential and explicit scope for a custom Helix instance.
        /// </summary>
        public static IHelixApi GetAuthenticatedWithEntra(
            string baseUri,
            TokenCredential credential,
            string scope)
        {
            return new HelixApi(new HelixApiOptions(
                new Uri(baseUri),
                ValidateEntraCredential(credential),
                new[] { scope }));
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

        private static TokenCredential ValidateEntraCredential(TokenCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            if (credential is HelixApiTokenCredential)
            {
                throw new ArgumentException(
                    "HelixApiTokenCredential represents a PAT. Use GetAuthenticated(...) for PAT authentication.",
                    nameof(credential));
            }

            return credential;
        }
    }
}
