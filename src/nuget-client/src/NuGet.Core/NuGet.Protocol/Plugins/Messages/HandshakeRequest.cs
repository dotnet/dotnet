// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using NuGet.Versioning;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A handshake request.
    /// </summary>
    public sealed class HandshakeRequest
    {
        /// <summary>
        /// Gets the requestor's plugin protocol version.
        /// </summary>
        [JsonRequired]
        public SemanticVersion ProtocolVersion { get; }

        /// <summary>
        /// Gets the requestor's minimum plugin protocol version.
        /// </summary>
        [JsonRequired]
        public SemanticVersion MinimumProtocolVersion { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeRequest" /> class.
        /// </summary>
        /// <param name="protocolVersion">The requestor's plugin protocol version.</param>
        /// <param name="minimumProtocolVersion">The requestor's minimum plugin protocol version.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="protocolVersion" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="minimumProtocolVersion" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="protocolVersion" />
        /// is less than <paramref name="minimumProtocolVersion" />.</exception>
        [JsonConstructor]
        public HandshakeRequest(SemanticVersion protocolVersion, SemanticVersion minimumProtocolVersion)
        {
            if (protocolVersion == null)
            {
                throw new ArgumentNullException(nameof(protocolVersion));
            }

            if (minimumProtocolVersion == null)
            {
                throw new ArgumentNullException(nameof(minimumProtocolVersion));
            }

            if (minimumProtocolVersion > protocolVersion)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(protocolVersion),
                    protocolVersion,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_ProtocolVersionOutOfRange,
                        nameof(protocolVersion),
                        nameof(minimumProtocolVersion)));
            }

            ProtocolVersion = protocolVersion;
            MinimumProtocolVersion = minimumProtocolVersion;
        }
    }
}
