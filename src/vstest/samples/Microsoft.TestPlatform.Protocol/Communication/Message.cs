// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Protocol
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Message
    {
        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        public JToken Payload { get; set; }

        /// <summary>
        /// To string implementation.
        /// </summary>
        /// <returns> The <see cref="string"/>. </returns>
        public override string ToString()
        {
            return "(" + MessageType + ") -> " + (Payload == null ? "null" : Payload.ToString(Formatting.Indented));
        }
    }
}