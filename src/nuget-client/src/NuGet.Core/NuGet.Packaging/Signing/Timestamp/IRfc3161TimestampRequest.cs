// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Threading.Tasks;

namespace NuGet.Packaging.Signing
{
    internal interface IRfc3161TimestampRequest
    {
        Task<IRfc3161TimestampToken> SubmitRequestAsync(Uri timestampUri, TimeSpan timeout);

        /// <summary>
        /// Gets the nonce for this timestamp request.
        /// </summary>
        /// <returns>The nonce for this timestamp request as byte[], if one was present; otherwise, null</returns>
        byte[] GetNonce();
    }
}
#endif
