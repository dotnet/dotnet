// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if !IS_CORECLR
using System.Net;
#endif

namespace NuGet.Common
{
    public static class NetworkProtocolUtility
    {
        /// <summary>
        /// Set ServicePointManager.DefaultConnectionLimit
        /// </summary>
        public static void SetConnectionLimit()
        {
#if !IS_CORECLR
            // Increase the maximum number of connections per server.
            if (!RuntimeEnvironmentHelper.IsMono)
            {
                ServicePointManager.DefaultConnectionLimit = 64;
            }
            else
            {
                // Keep mono limited to a single download to avoid issues.
                ServicePointManager.DefaultConnectionLimit = 1;
            }
#endif
        }
    }
}
