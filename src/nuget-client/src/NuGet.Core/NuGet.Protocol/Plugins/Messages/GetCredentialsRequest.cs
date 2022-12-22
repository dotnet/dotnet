// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request to get credentials.
    /// </summary>
    public sealed class GetCredentialsRequest
    {
        /// <summary>
        /// Gets the package source repository location.
        /// </summary>
        [JsonRequired]
        public string PackageSourceRepository { get; }

        /// <summary>
        /// Gets the HTTP status code that necessitates credentials.
        /// </summary>
        [JsonRequired]
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new <see cref="GetCredentialsRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source repository location.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        [JsonConstructor]
        public GetCredentialsRequest(string packageSourceRepository, HttpStatusCode statusCode)
        {
            if (string.IsNullOrEmpty(packageSourceRepository))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageSourceRepository));
            }

            if (!Enum.IsDefined(typeof(HttpStatusCode), statusCode))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_UnrecognizedEnumValue,
                        statusCode),
                    nameof(statusCode));
            }

            PackageSourceRepository = packageSourceRepository;
            StatusCode = statusCode;
        }
    }
}
