// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NuGet.Common
{
    /// <summary>
    /// Wraps a NetworkCredentials object by returning null if the authType is not in a specified allow list
    /// </summary>
    public class AuthTypeFilteredCredentials : ICredentials
    {
        public IReadOnlyList<string> AuthTypes { get; }
        public NetworkCredential InnerCredential { get; }

        /// <summary>
        /// Initializes a new AuthTypeFilteredCredentials
        /// </summary>
        /// <param name="innerCredential">Credential to delegate to</param>
        /// <param name="authTypes">List of authTypes to respond to. If empty, responds to all authTypes.</param>
        public AuthTypeFilteredCredentials(NetworkCredential innerCredential, IEnumerable<string> authTypes)
        {
            if (innerCredential == null)
            {
                throw new ArgumentNullException(nameof(innerCredential));
            }

            if (authTypes == null)
            {
                throw new ArgumentNullException(nameof(authTypes));
            }

            InnerCredential = innerCredential;
            AuthTypes = new List<string>(authTypes);
        }

        public NetworkCredential? GetCredential(Uri uri, string authType)
        {
            return authType == null || !AuthTypes.Any() || AuthTypes.Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, authType))
                ? InnerCredential.GetCredential(uri, authType)
                : null;
        }
    }
}
