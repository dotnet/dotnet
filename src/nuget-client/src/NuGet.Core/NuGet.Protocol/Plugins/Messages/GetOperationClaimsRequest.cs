// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A query to a plugin about which operations it supports for a specific package source.
    /// In version 1.0.0, the fields are required. That's not the case for version 2.0.0
    /// </summary>
    public sealed class GetOperationClaimsRequest
    {
        /// <summary>
        /// Gets the package source repository location for the <see cref="ServiceIndexJson" />.
        /// </summary>
        public string PackageSourceRepository { get; }

#nullable enable
        /// <summary>
        /// Gets the service index (index.json) for the <see cref="PackageSourceRepository" /> as a raw JSON string.
        /// </summary>
        [JsonProperty("ServiceIndex")]
        [JsonConverter(typeof(NsjRawJsonStringConverter))]
        public string? ServiceIndexJson { get; }
#nullable disable

        /// <summary>
        /// Gets the service index (index.json) for the <see cref="PackageSourceRepository" />.
        /// </summary>
        [Obsolete("Use ServiceIndexJson instead. This property always returns null.")]
        [JsonIgnore]
        public JObject ServiceIndex => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOperationClaimsRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source location.</param>
        /// <param name="serviceIndex">The service index (index.json) as a raw JSON string.</param>
        /// <remarks>Both packageSourceRepository and service index can be null. If they are, the operation claims request is considered as source agnostic</remarks>
#nullable enable
        [JsonConstructor]
        public GetOperationClaimsRequest(string? packageSourceRepository, string? serviceIndex)
        {
            PackageSourceRepository = packageSourceRepository;
            ServiceIndexJson = serviceIndex;
        }
#nullable disable

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOperationClaimsRequest" /> class.
        /// </summary>
        /// <param name="packageSourceRepository">The package source location.</param>
        /// <param name="serviceIndex">The service index (index.json).</param>
        [Obsolete("Use GetOperationClaimsRequest(string, string) instead.")]
        public GetOperationClaimsRequest(string packageSourceRepository, JObject serviceIndex)
            : this(packageSourceRepository, serviceIndex?.ToString(Formatting.None))
        {
        }
    }
}
