// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Plugin operation claims.
    /// </summary>
    public enum OperationClaim
    {
        /// <summary>
        /// The download package operation claim.
        /// </summary>
        DownloadPackage,

        /// <summary>
        /// The authentication operation claim
        /// </summary>
        Authentication
    }
}
