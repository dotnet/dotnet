// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A response to a get package hash request.
    /// </summary>
    public sealed class GetPackageHashResponse
    {
        /// <summary>
        /// Gets the package hash.
        /// </summary>
        public string Hash { get; }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        [JsonRequired]
        public MessageResponseCode ResponseCode { get; }

        /// <summary>
        /// Initializes a new <see cref="GetPackageHashResponse" /> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is an undefined <see cref="MessageResponseCode" /> value.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" /> 
        /// is <see cref="MessageResponseCode.Success" /> and <paramref name="hash" />
        /// is either <see langword="null" /> or empty.</exception>
        [JsonConstructor]
        public GetPackageHashResponse(MessageResponseCode responseCode, string hash)
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

            if (responseCode == MessageResponseCode.Success && string.IsNullOrEmpty(hash))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(hash));
            }

            ResponseCode = responseCode;
            Hash = hash;
        }
    }
}
