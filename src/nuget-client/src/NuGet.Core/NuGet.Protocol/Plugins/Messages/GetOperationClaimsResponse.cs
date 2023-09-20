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
    /// A plugin's response as to which operations it supports for a specific package source.
    /// </summary>
    public sealed class GetOperationClaimsResponse
    {
        /// <summary>
        /// Gets the plugin's operation claims.
        /// </summary>
        [JsonRequired]
        public IReadOnlyList<OperationClaim> Claims { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOperationClaimsResponse" /> class.
        /// </summary>
        /// <param name="claims">The operation claims.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="claims" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="claims" /> contains
        /// undefined <see cref="OperationClaim" /> values.</exception>
        [JsonConstructor]
        public GetOperationClaimsResponse(IEnumerable<OperationClaim> claims)
        {
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            var unrecognizedClaims = claims.Where(claim => !Enum.IsDefined(typeof(OperationClaim), claim));

            if (unrecognizedClaims.Any())
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Plugin_UnrecognizedOperationClaims,
                        string.Join(",", unrecognizedClaims)),
                    nameof(claims));
            }

            Claims = claims.ToList();
        }
    }
}
