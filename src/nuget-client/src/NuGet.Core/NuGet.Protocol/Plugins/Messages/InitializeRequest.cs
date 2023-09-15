// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// An initialization request to a plugin.
    /// </summary>
    public sealed class InitializeRequest
    {
        /// <summary>
        /// Gets the requestor's NuGet client version.
        /// </summary>
        [JsonRequired]
        public string ClientVersion { get; }

        /// <summary>
        /// Gets the requestor's current culture.
        /// </summary>
        [JsonRequired]
        public string Culture { get; }

        /// <summary>
        /// Gets the default request timeout for all subsequent requests.
        /// </summary>
        [JsonRequired]
        public TimeSpan RequestTimeout { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeRequest" /> class.
        /// </summary>
        /// <param name="clientVersion">The requestor's NuGet client version.</param>
        /// <param name="culture">The requestor's current culture.</param>
        /// <param name="requestTimeout">The default request timeout.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="clientVersion" /> is either <see langword="null" />
        /// or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="culture" /> is either <see langword="null" />
        /// or an empty string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="requestTimeout" />
        /// is either less than <see cref="ProtocolConstants.MinTimeout" /> or greater than
        /// <see cref="ProtocolConstants.MaxTimeout" />.</exception>
        [JsonConstructor]
        public InitializeRequest(string clientVersion, string culture, TimeSpan requestTimeout)
        {
            if (string.IsNullOrEmpty(clientVersion))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(clientVersion));
            }

            if (string.IsNullOrEmpty(culture))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(culture));
            }

            if (!TimeoutUtilities.IsValid(requestTimeout))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestTimeout),
                    requestTimeout,
                    Strings.Plugin_TimeoutOutOfRange);
            }

            ClientVersion = clientVersion;
            Culture = culture;
            RequestTimeout = requestTimeout;
        }
    }
}
