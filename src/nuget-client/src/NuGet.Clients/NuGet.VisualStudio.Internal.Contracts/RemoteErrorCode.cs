// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Internal.Contracts
{
    // Values -32768 to -32000 are reserved by the JSON-RPC protocol.  Do not use them!
    // See https://www.jsonrpc.org/specification#error_object for details.
    public enum RemoteErrorCode : int
    {
        /// <summary>
        /// This code serves as a hint to clients on how to deserialize the error data.
        /// It does not represent a unique error condition.
        /// </summary>
        RemoteError = -31999
    }
}
