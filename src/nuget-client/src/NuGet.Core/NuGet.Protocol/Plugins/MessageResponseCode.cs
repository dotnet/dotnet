// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Message response codes.
    /// </summary>
    public enum MessageResponseCode
    {
        /// <summary>
        /// The response is success.
        /// </summary>
        Success,

        /// <summary>
        /// The response is error.
        /// </summary>
        Error,

        /// <summary>
        /// The response is not found.
        /// </summary>
        NotFound
    }
}
