// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using NuGet.Versioning;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A handshake response.
    /// </summary>
    public sealed class HandshakeResponse
    {
        /// <summary>
        /// Gets the handshake responder's handshake response code.
        /// </summary>
        [JsonRequired]
        public MessageResponseCode ResponseCode { get; }

        /// <summary>
        /// Gets the handshake responder's plugin protocol version if the handshake was successful;
        /// otherwise, <see langword="null" />.
        /// </summary>
        public SemanticVersion ProtocolVersion { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeResponse" /> class.
        /// </summary>
        /// <param name="responseCode">The handshake responder's handshake response code.</param>
        /// <param name="protocolVersion">The handshake responder's plugin protocol version
        /// if the handshake was successful; otherwise, <see langword="null" />.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is an undefined <see cref="MessageResponseCode" /> value.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is <see cref="MessageResponseCode.Success" /> and <paramref name="protocolVersion" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is not <see cref="MessageResponseCode.Success" /> and <paramref name="protocolVersion" />
        /// is not <see langword="null" />.</exception>
        [JsonConstructor]
        public HandshakeResponse(MessageResponseCode responseCode, SemanticVersion protocolVersion)
        {
            if (!Enum.IsDefined(typeof(MessageResponseCode), responseCode))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_UnrecognizedEnumValue,
                        responseCode),
                    nameof(responseCode));
            }

            if (responseCode == MessageResponseCode.Success)
            {
                if (protocolVersion == null)
                {
                    throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(protocolVersion));
                }
            }
            else if (protocolVersion != null)
            {
                throw new ArgumentException(
                    Strings.Plugin_ProtocolVersionNotSupportedOnError,
                    nameof(protocolVersion));
            }

            ResponseCode = responseCode;
            ProtocolVersion = protocolVersion;
        }
    }
}
