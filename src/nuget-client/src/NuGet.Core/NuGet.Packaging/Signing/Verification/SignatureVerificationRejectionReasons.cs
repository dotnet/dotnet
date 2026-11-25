// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging.Signing
{
    [Flags]
    internal enum SignatureVerificationRejectionReasons
    {
        None = 0,
        RevocationUnavailable = 1,
        Other = 2,
        Unknown = 4,
        UnattributedCollectorError = 8
    }
}
