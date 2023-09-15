// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Common;
using NuGet.Versioning;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Plugin connection options.
    /// </summary>
    public sealed class ConnectionOptions
    {
        /// <summary>
        /// Gets the plugin handshake timeout.
        /// </summary>
        public TimeSpan HandshakeTimeout { get; }

        /// <summary>
        /// Gets the minimum plugin protocol version.
        /// </summary>
        public SemanticVersion MinimumProtocolVersion { get; }

        /// <summary>
        /// Gets the plugin protocol version.
        /// </summary>
        public SemanticVersion ProtocolVersion { get; }

        /// <summary>
        /// Gets the plugin request timeout.
        /// </summary>
        public TimeSpan RequestTimeout { get; private set; }

        /// <summary>
        /// Instantiates a new <see cref="ConnectionOptions" /> class.
        /// </summary>
        /// <param name="protocolVersion">The plugin protocol version.</param>
        /// <param name="minimumProtocolVersion">The minimum plugin protocol version.</param>
        /// <param name="handshakeTimeout">The plugin handshake timeout.</param>
        /// <param name="requestTimeout">The plugin request timeout.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="protocolVersion" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="minimumProtocolVersion" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="protocolVersion" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="protocolVersion" />
        /// is less than <paramref name="minimumProtocolVersion" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="handshakeTimeout" />
        /// is either less than <see cref="ProtocolConstants.MinTimeout" /> or greater than
        /// <see cref="ProtocolConstants.MaxTimeout" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="requestTimeout" />
        /// is either less than <see cref="ProtocolConstants.MinTimeout" /> or greater than
        /// <see cref="ProtocolConstants.MaxTimeout" />.</exception>
        public ConnectionOptions(
            SemanticVersion protocolVersion,
            SemanticVersion minimumProtocolVersion,
            TimeSpan handshakeTimeout,
            TimeSpan requestTimeout)
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

            if (!TimeoutUtilities.IsValid(handshakeTimeout))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(handshakeTimeout),
                    handshakeTimeout,
                    Strings.Plugin_TimeoutOutOfRange);
            }

            if (!TimeoutUtilities.IsValid(requestTimeout))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestTimeout),
                    requestTimeout,
                    Strings.Plugin_TimeoutOutOfRange);
            }

            ProtocolVersion = protocolVersion;
            MinimumProtocolVersion = minimumProtocolVersion;
            HandshakeTimeout = handshakeTimeout;
            RequestTimeout = requestTimeout;
        }

        /// <summary>
        /// Sets a new request timeout.
        /// </summary>
        /// <param name="requestTimeout">The new request timeout.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="requestTimeout" />
        /// is either less than <see cref="ProtocolConstants.MinTimeout" /> or greater than
        /// <see cref="ProtocolConstants.MaxTimeout" />.</exception>
        public void SetRequestTimeout(TimeSpan requestTimeout)
        {
            if (!TimeoutUtilities.IsValid(requestTimeout))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestTimeout),
                    requestTimeout,
                    Strings.Plugin_TimeoutOutOfRange);
            }

            RequestTimeout = requestTimeout;
        }

        /// <summary>
        /// Instantiates a <see cref="ConnectionOptions" /> class with default values.
        /// </summary>
        /// <param name="reader">An environment variable reader.</param>
        /// <returns>A <see cref="ConnectionOptions" />.</returns>
        public static ConnectionOptions CreateDefault(IEnvironmentVariableReader reader = null)
        {
            reader = reader ?? EnvironmentVariableWrapper.Instance;

            var handshakeTimeoutInSeconds = reader.GetEnvironmentVariable(EnvironmentVariableConstants.HandshakeTimeout);
            var requestTimeoutInSeconds = reader.GetEnvironmentVariable(EnvironmentVariableConstants.RequestTimeout);

            var handshakeTimeout = TimeoutUtilities.GetTimeout(handshakeTimeoutInSeconds, ProtocolConstants.HandshakeTimeout);
            var requestTimeout = TimeoutUtilities.GetTimeout(requestTimeoutInSeconds, ProtocolConstants.RequestTimeout);

            return new ConnectionOptions(
                protocolVersion: ProtocolConstants.CurrentVersion,
                minimumProtocolVersion: ProtocolConstants.Version100,
                handshakeTimeout: handshakeTimeout,
                requestTimeout: requestTimeout);
        }
    }
}
