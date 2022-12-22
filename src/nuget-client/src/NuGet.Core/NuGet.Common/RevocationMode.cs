// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Common
{
    public enum RevocationMode
    {
        /// <summary>
        /// If needed go online to check for revocation.
        /// This translates to X509RevocationMode.Online
        /// </summary>
        Online,

        /// <summary>
        /// Only use the machine cache to check for revocation.
        /// This translates to X509RevocationMode.Offline
        /// </summary>
        Offline
    }
}
