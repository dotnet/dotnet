// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Security.Cryptography.Pkcs;

namespace NuGet.Packaging.Signing
{
    public interface ISignature
    {
        SignatureType Type { get; }
        SignerInfo SignerInfo { get; }
    }
}
