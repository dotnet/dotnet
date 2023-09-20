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
    /// A response to a get files in package request.
    /// </summary>
    public sealed class GetFilesInPackageResponse
    {
        /// <summary>
        /// Gets the paths of files in the package.
        /// </summary>
        public IEnumerable<string> Files { get; }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        [JsonRequired]
        public MessageResponseCode ResponseCode { get; }

        /// <summary>
        /// Initializes a new <see cref="GetFilesInPackageResponse" /> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="files">The paths of files in the package.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" />
        /// is an undefined <see cref="MessageResponseCode" /> value.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="responseCode" /> 
        /// is <see cref="MessageResponseCode.Success" /> and <paramref name="files" />
        /// is either <see langword="null" /> or empty.</exception>
        [JsonConstructor]
        public GetFilesInPackageResponse(MessageResponseCode responseCode, IEnumerable<string> files)
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

            if (responseCode == MessageResponseCode.Success && (files == null || !files.Any()))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(files));
            }

            ResponseCode = responseCode;
            Files = files;
        }
    }
}
