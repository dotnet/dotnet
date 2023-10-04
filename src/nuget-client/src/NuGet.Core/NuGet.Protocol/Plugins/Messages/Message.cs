// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a message between a NuGet client and a plugin.
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// Gets the request ID.
        /// </summary>
        [JsonRequired]
        public string RequestId { get; }

        /// <summary>
        /// Gets the message type.
        /// </summary>
        [JsonRequired]
        public MessageType Type { get; }

        /// <summary>
        /// Gets the message method.
        /// </summary>
        [JsonRequired]
        public MessageMethod Method { get; }

        /// <summary>
        /// Gets the optional message payload.
        /// </summary>
        public JObject Payload { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="type">The message type.</param>
        /// <param name="method">The message method.</param>
        /// <param name="payload">An optional message payload.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="requestId" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="type" />
        /// is an undefined <see cref="MessageType" /> value.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="method" />
        /// is an undefined <see cref="MessageMethod" /> value.</exception>
        [JsonConstructor]
        public Message(string requestId, MessageType type, MessageMethod method, JObject payload = null)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(requestId));
            }

            if (!Enum.IsDefined(typeof(MessageType), type))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_UnrecognizedEnumValue,
                        type),
                    nameof(type));
            }

            if (!Enum.IsDefined(typeof(MessageMethod), method))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_UnrecognizedEnumValue,
                        method),
                    nameof(method));
            }

            RequestId = requestId;
            Type = type;
            Method = method;
            Payload = payload;
        }
    }
}
