// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography.Pkcs;

namespace NuGet.Packaging.Signing
{
    public sealed class UnknownPrimarySignature : PrimarySignature
    {
        public UnknownPrimarySignature(SignedCms signedCms)
            : base(signedCms, SignatureType.Unknown)
        {
        }
    }
}
