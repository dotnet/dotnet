// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request to set credentials for a package source with any last known good credentials.
    /// </summary>
    public sealed class SetCredentialsRequest
    {
        /// <summary>
        /// Gets the package source repository location.
        /// </summary>
        [JsonRequired]
        public string PackageSourceRepository { get; }

        /// <summary>
        /// Gets the package source repository password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the proxy password.
        /// </summary>
        public string ProxyPassword { get; }

        /// <summary>
        /// Gets the proxy username.
        /// </summary>
        public string ProxyUsername { get; }

        /// <summary>
        /// Gets the package source repository username.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Initializes a new <see cref="SetCredentialsRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source repository location.</param>
        /// <param name="proxyUsername">The proxy username.</param>
        /// <param name="proxyPassword">The proxy password.</param>
        /// <param name="username">The package source repository username.</param>
        /// <param name="password">The package source repository password.</param>
        public SetCredentialsRequest(
            string packageSourceRepository,
            string proxyUsername,
            string proxyPassword,
            string username,
            string password)
        {
            if (string.IsNullOrEmpty(packageSourceRepository))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageSourceRepository));
            }

            PackageSourceRepository = packageSourceRepository;
            ProxyUsername = proxyUsername;
            ProxyPassword = proxyPassword;
            Username = username;
            Password = password;
        }
    }
}
