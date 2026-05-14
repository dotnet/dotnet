// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Message types.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// A cancellation request for an existing request.
        /// </summary>
        Cancel,

        /// <summary>
        /// A fault notification, either standalone or for an existing request.
        /// </summary>
        Fault,

        /// <summary>
        /// A progress notification for an existing request.
        /// </summary>
        Progress,

        /// <summary>
        /// A request.
        /// </summary>
        Request,

        /// <summary>
        /// A response for an existing request.
        /// </summary>
        Response
    }
}
