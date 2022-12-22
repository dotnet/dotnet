// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;

namespace NuGet.Configuration
{
    /// <summary>
    /// <see cref="CredentialCache"/>-like interface with Update credential semantics rather than Add/Remove
    /// </summary>
    public interface IProxyCredentialCache : ICredentials
    {
        /// <summary>
        /// Tracks the cache version. Changes every time proxy credential is updated.
        /// </summary>
        Guid Version { get; }

        /// <summary>
        /// Add or update proxy credential
        /// </summary>
        /// <param name="proxyAddress">Proxy network address</param>
        /// <param name="credentials">New credential object</param>
        void UpdateCredential(Uri proxyAddress, NetworkCredential credentials);
    }
}
