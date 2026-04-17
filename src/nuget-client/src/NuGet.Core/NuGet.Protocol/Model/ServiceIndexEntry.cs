// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    /// <summary>
    /// index.json entry for v3
    /// </summary>
    public class ServiceIndexEntry
    {
        /// <summary>
        /// Service Uri
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// Service Type
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Client version
        /// </summary>
        public SemanticVersion ClientVersion { get; }

        public ServiceIndexEntry(Uri serviceUri, string serviceType, SemanticVersion clientVersion)
        {
            if (serviceUri == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (clientVersion == null)
            {
                throw new ArgumentNullException(nameof(clientVersion));
            }

            Uri = serviceUri;
            Type = serviceType;
            ClientVersion = clientVersion;
        }
    }
}
