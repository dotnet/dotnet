// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A response to a get service index request.
    /// </summary>
    public sealed class GetServiceIndexResponse
    {
        /// <summary>
        /// Gets the response code.
        /// </summary>
        [JsonRequired]
        public MessageResponseCode ResponseCode { get; }

        /// <summary>
        /// Gets the service index (index.json) for the package source repository as a raw JSON string.
        /// </summary>
        [JsonProperty("ServiceIndex")]
        [JsonConverter(typeof(NsjRawJsonStringConverter))]
        [System.Text.Json.Serialization.JsonPropertyName("ServiceIndex")]
        [System.Text.Json.Serialization.JsonConverter(typeof(RawJsonStringConverter))]
        public string? ServiceIndexJson { get; }

        /// <summary>
        /// Gets the service index (index.json) for the package source repository.
        /// </summary>
        [Obsolete("Use ServiceIndexJson instead. This property always returns null.")]
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public JObject? ServiceIndex => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetServiceIndexResponse" /> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="serviceIndexJson">The service index (index.json) for the package source repository as a raw JSON string.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is an undefined <see cref="MessageResponseCode" /> value.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="responseCode" />
        /// is <see cref="MessageResponseCode.Success" /> and <paramref name="serviceIndexJson" />
        /// is <see langword="null" />.</exception>
        [JsonConstructor]
        [System.Text.Json.Serialization.JsonConstructor]
        public GetServiceIndexResponse(MessageResponseCode responseCode, string? serviceIndexJson)
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

            if (responseCode == MessageResponseCode.Success && serviceIndexJson == null)
            {
                throw new ArgumentNullException(nameof(serviceIndexJson));
            }

            ResponseCode = responseCode;
            ServiceIndexJson = serviceIndexJson;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetServiceIndexResponse" /> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="serviceIndex">The service index (index.json) for the package source repository.</param>
        [Obsolete("Use GetServiceIndexResponse(MessageResponseCode, string) instead.")]
        public GetServiceIndexResponse(MessageResponseCode responseCode, JObject? serviceIndex)
#pragma warning disable IL2026, IL3050 // WriteTo without converters is safe. See https://github.com/JamesNK/Newtonsoft.Json/blob/13.0.4/Src/Newtonsoft.Json/Linq/JToken.cs
            : this(responseCode, serviceIndex?.ToString(Formatting.None, Array.Empty<JsonConverter>()))
#pragma warning restore IL2026, IL3050
        {
        }
    }
}
