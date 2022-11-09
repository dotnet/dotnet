// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Security.Cryptography;

namespace NuGet.Packaging.Signing
{
    internal interface IRfc3161TimestampTokenInfo
    {
        string PolicyId { get; }

        DateTimeOffset Timestamp { get; }

        long? AccuracyInMicroseconds { get; }

        Oid HashAlgorithmId { get; }

        bool HasMessageHash(byte[] hash);

        byte[] GetNonce();
    }
}
#endif
