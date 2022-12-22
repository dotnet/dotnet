// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System.Security.Cryptography.Pkcs;

namespace NuGet.Packaging.Signing
{
    internal interface IRfc3161TimestampToken
    {
        IRfc3161TimestampTokenInfo TokenInfo { get; }

        SignedCms AsSignedCms();
    }
}
#endif

