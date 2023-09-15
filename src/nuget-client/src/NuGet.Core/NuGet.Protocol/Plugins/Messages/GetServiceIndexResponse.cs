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
        /// Gets the service index (index.json) for the package source repository.
        /// </summary>
        public JObject ServiceIndex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetServiceIndexResponse" /> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="serviceIndex">The service index (index.json) for the package source repository.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is an undefined <see cref="MessageResponseCode" /> value.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="responseCode" /> 
        /// is <see cref="MessageResponseCode.Success" /> and <paramref name="serviceIndex" />
        /// is <see langword="null" />.</exception>
        [JsonConstructor]
        public GetServiceIndexResponse(MessageResponseCode responseCode, JObject serviceIndex)
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

            if (responseCode == MessageResponseCode.Success && serviceIndex == null)
            {
                throw new ArgumentNullException(nameof(serviceIndex));
            }

            ResponseCode = responseCode;
            ServiceIndex = serviceIndex;
        }
    }
}
