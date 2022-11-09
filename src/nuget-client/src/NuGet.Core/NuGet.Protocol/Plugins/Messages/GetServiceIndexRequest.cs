// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request to get the service index for a package source repository.
    /// </summary>
    public sealed class GetServiceIndexRequest
    {
        /// <summary>
        /// Gets the package source repository location.
        /// </summary>
        [JsonRequired]
        public string PackageSourceRepository { get; }

        /// <summary>
        /// Initializes a new <see cref="GetServiceIndexRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source repository location.</param>
        [JsonConstructor]
        public GetServiceIndexRequest(string packageSourceRepository)
        {
            if (string.IsNullOrEmpty(packageSourceRepository))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(packageSourceRepository));
            }

            PackageSourceRepository = packageSourceRepository;
        }
    }
}
