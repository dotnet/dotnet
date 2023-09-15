// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A response to a get package versions request.
    /// </summary>
    public sealed class GetPackageVersionsResponse
    {
        /// <summary>
        /// Gets the response code.
        /// </summary>
        [JsonRequired]
        public MessageResponseCode ResponseCode { get; }

        /// <summary>
        /// Gets the package versions.
        /// </summary>
        public IEnumerable<string> Versions { get; }

        /// <summary>
        /// Initializes a new <see cref="GetPackageVersionsResponse" /> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="versions">The package versions.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is an undefined <see cref="MessageResponseCode" /> value.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" /> 
        /// is <see cref="MessageResponseCode.Success" /> and <paramref name="versions" />
        /// is either <see langword="null" /> or empty.</exception>
        [JsonConstructor]
        public GetPackageVersionsResponse(MessageResponseCode responseCode, IEnumerable<string> versions)
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

            if (responseCode == MessageResponseCode.Success && (versions == null || !versions.Any()))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(versions));
            }

            ResponseCode = responseCode;
            Versions = versions;
        }
    }
}
