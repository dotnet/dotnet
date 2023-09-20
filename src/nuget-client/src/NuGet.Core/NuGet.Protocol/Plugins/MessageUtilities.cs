// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Helper methods for messages.
    /// </summary>
    public static class MessageUtilities
    {
        /// <summary>
        /// Instantiates a new <see cref="Message" /> class.
        /// </summary>
        /// <param name="requestId">The message request ID.</param>
        /// <param name="type">The message type.</param>
        /// <param name="method">The message method.</param>
        /// <returns>a <see cref="Message" /> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="requestId" />
        /// is either <see langword="null" /> or an empty string.</exception>
        public static Message Create(
            string requestId,
            MessageType type,
            MessageMethod method)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(requestId));
            }

            return new Message(requestId, type, method);
        }

        /// <summary>
        /// Instantiates a new <see cref="Message" /> class.
        /// </summary>
        /// <typeparam name="TPayload">The message payload type.</typeparam>
        /// <param name="requestId">The message request ID.</param>
        /// <param name="type">The message type.</param>
        /// <param name="method">The message method.</param>
        /// <param name="payload">The message payload.</param>
        /// <returns>a <see cref="Message" /> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="requestId" />
        /// is either <see langword="null" /> or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="payload" /> is <see langword="null" />.</exception>
        public static Message Create<TPayload>(
            string requestId,
            MessageType type,
            MessageMethod method,
            TPayload payload)
            where TPayload : class
        {
            if (string.IsNullOrEmpty(requestId))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(requestId));
            }

            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            var jsonPayload = JsonSerializationUtilities.FromObject(payload);

            return new Message(requestId, type, method, jsonPayload);
        }

        /// <summary>
        /// Deserializes a message payload.
        /// </summary>
        /// <typeparam name="TPayload">The message payload type.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>The deserialized message payload of type <typeparamref name="TPayload" />
        /// or <see langword="null" /> if no payload exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message" /> is <see langword="null" />.</exception>
        public static TPayload DeserializePayload<TPayload>(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.Payload == null)
            {
                return default(TPayload);
            }

            return JsonSerializationUtilities.ToObject<TPayload>(message.Payload);
        }
    }
}
